using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GuruminTrainerSteam.DynamicHelper
{
    /// <summary>
    /// 全局热键驱动
    /// </summary>
    public class HotKeyCore : IDisposable
    {
        #region Win32 常量定义

        // 低级键盘钩子类型 (Low Level Keyboard Hook)，值为 13
        private const int WH_KEYBOARD_LL = 13;
        // 键盘按键按下消息 (普通键)
        private const int WM_KEYDOWN = 0x0100;
        // 键盘按键按下消息 (系统键，如 Alt 组合键)
        private const int WM_SYSKEYDOWN = 0x0104;
        // 虚拟键码：Shift 键
        private const int VK_SHIFT = 0x10;
        // 虚拟键码：左 Shift 键
        private const int VK_LSHIFT = 0xA0;
        // 虚拟键码：右 Shift 键
        private const int VK_RSHIFT = 0xA1;
        // 虚拟键码：Alt 键
        private const int VK_MENU = 0x12;
        // 虚拟键码：Ctrl 键
        private const int VK_CONTROL = 0x11;

        #endregion

        #region Win32 API 导入 (P/Invoke)

        /// <summary>
        /// 安装一个应用程序定义的钩子过程到钩子链中
        /// </summary>
        /// <param name="idHook">钩子类型（此处为 WH_KEYBOARD_LL 低级键盘钩子）</param>
        /// <param name="lpfn">钩子回调函数指针</param>
        /// <param name="hMod">包含钩子回调函数的 DLL 模块句柄</param>
        /// <param name="dwThreadId">要关联钩子的线程 ID，0 表示全局钩子</param>
        /// <returns>成功返回钩子句柄；失败返回 IntPtr.Zero</returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// 卸载由 SetWindowsHookEx 安装的钩子
        /// </summary>
        /// <param name="hhk">要卸载的钩子句柄</param>
        /// <returns>成功返回 true，失败返回 false</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// 将钩子消息传递给钩子链中的下一个钩子过程
        /// </summary>
        /// <param name="hhk">当前钩子句柄（可忽略）</param>
        /// <param name="nCode">传递给下一个钩子的钩子代码</param>
        /// <param name="wParam">传递给下一个钩子的消息参数</param>
        /// <param name="lParam">传递给下一个钩子的消息参数</param>
        /// <returns>下一个钩子过程的返回值</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 获取指定模块的句柄（用于向 SetWindowsHookEx 提供当前模块句柄）
        /// </summary>
        /// <param name="lpModuleName">模块名称，传 null 表示当前进程主模块</param>
        /// <returns>模块句柄</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// 获取指定虚拟键的状态（最高位为 1 表示该键当前处于按下状态）
        /// </summary>
        /// <param name="nVirtKey">虚拟键码</param>
        /// <returns>键状态标志位</returns>
        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        /// <summary>
        /// 获取当前线程 ID
        /// </summary>
        /// <returns>当前线程 ID</returns>
        [DllImport("user32.dll")]
        private static extern uint GetCurrentThreadId();

        #endregion

        #region 委托与结构体

        /// <summary>
        /// 低级键盘钩子回调委托（与 Windows 底层钩子回调签名一致）
        /// </summary>
        /// <param name="nCode">钩子代码，决定如何处理消息</param>
        /// <param name="wParam">键盘消息类型</param>
        /// <param name="lParam">指向 KBDLLHOOKSTRUCT 结构体的指针</param>
        /// <returns>通常应返回 CallNextHookEx 的结果</returns>
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 低级键盘钩子的按键信息结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            /// <summary>虚拟键码（Virtual Key Code）</summary>
            public uint vkCode;
            /// <summary>硬件扫描码（Scan Code）</summary>
            public uint scanCode;
            /// <summary>扩展标志、注入标志等</summary>
            public uint flags;
            /// <summary>消息时间戳</summary>
            public uint time;
            /// <summary>附加信息（通常为 0）</summary>
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// 热键修饰键枚举
        /// </summary>
        public enum HotKeyModifier
        {
            None = 0,
            Shift = 1,
            Alt = 2,
            Ctrl = 3,
        }

        /// <summary>
        /// 热键回调委托
        /// </summary>
        /// <param name="name">触发的热键名称</param>
        public delegate void HotKeyHandler(string name);

        #endregion

        #region 字段与键名映射

        // 已安装钩子的句柄，IntPtr.Zero 表示尚未安装
        private IntPtr _hookId = IntPtr.Zero;
        // 钩子回调委托实例（必须持有引用防止被 GC 回收）
        private LowLevelKeyboardProc _proc;
        // 复合键 -> 回调 映射，键格式: "Shift|F1" / "Alt|F1"，解决同一功能键在不同修饰键下的冲突
        private readonly Dictionary<string, HotKeyHandler> _hotkeyActions = new Dictionary<string, HotKeyHandler>();
        // 复合键 -> 显示名称 映射
        private readonly Dictionary<string, string> _hotkeyNames = new Dictionary<string, string>();
        // 是否已释放
        private bool _disposed;
        // UI 线程同步用的控件引用（跨线程回调需切回 UI 线程）
        private readonly Control _syncControl;

        /// <summary>
        /// 生成"修饰键|功能键"的复合键字符串，作为字典的唯一键
        /// </summary>
        private static string MakeKey(Keys key, HotKeyModifier mod) => $"{mod}|{key}";

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化热键核心
        /// </summary>
        /// <param name="syncControl">用于跨线程 UI 同步的控件（通常是主窗体）</param>
        public HotKeyCore(Control syncControl)
        {
            _syncControl = syncControl ?? throw new ArgumentNullException(nameof(syncControl));
            _proc = HookCallback;
        }

        #endregion

        #region 热键注册与注销

        /// <summary>
        /// 注册一个"修饰键 + 功能键"热键
        /// </summary>
        /// <param name="key">功能键 (Keys)</param>
        /// <param name="modifier">修饰键 (HotKeyModifier)</param>
        /// <param name="name">热键显示名称</param>
        /// <param name="handler">触发热键时执行的回调 (HotKeyHandler)</param>
        public void RegisterHotKey(Keys key, HotKeyModifier modifier, string name, HotKeyHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            string ck = MakeKey(key, modifier);
            _hotkeyActions[ck] = handler;
            _hotkeyNames[ck] = name;
#if DEBUG
            /*string prefix = modifier switch
            {
                HotKeyModifier.Alt   => "ALT",
                HotKeyModifier.Ctrl  => "CTRL",
                _                    => "SHIFT"
            };
            Debug.WriteLine($"[HotKeyCore] 注册热键: {prefix}+{key} -> \"{name}\"");*/

            string prefix;
            switch (modifier)
            {
                case HotKeyModifier.Alt: prefix = "ALT"; break;
                case HotKeyModifier.Ctrl: prefix = "CTRL"; break;
                default: prefix = "SHIFT"; break;
            }
            Debug.WriteLine($"[HotKeyCore] 注册热键: {prefix}+{key} -> \"{name}\"");


#endif
        }

        /// <summary>
        /// 注销指定"修饰键 + 功能键"热键
        /// </summary>
        public void UnregisterHotKey(Keys key, HotKeyModifier modifier)
        {
            string ck = MakeKey(key, modifier);
            _hotkeyActions.Remove(ck);
            _hotkeyNames.Remove(ck);
        }

        /// <summary>
        /// 注销全部热键
        /// </summary>
        public void UnregisterAll()
        {
            _hotkeyActions.Clear();
            _hotkeyNames.Clear();
        }

        #endregion

        #region 钩子生命周期管理

        /// <summary>
        /// 安装键盘钩子
        /// </summary>
        public void Start()
        {
            if (_hookId != IntPtr.Zero) return; // 已启动，避免重复安装

            // 获取当前进程主模块句柄，作为钩子回调所在模块传给 SetWindowsHookEx
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _hookId = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    _proc,
                    GetModuleHandle(curModule.ModuleName),
                    0); // 线程 ID 传 0，安装全局钩子
            }

            // 钩子句柄为 IntPtr.Zero 表示安装失败，否则安装成功
            if (_hookId == IntPtr.Zero)
            {
                int err = Marshal.GetLastWin32Error();
                Debug.WriteLine($"[HotKeyCore] SetWindowsHookEx 失败, 错误码: {err}");
            }
            else
            {
                Debug.WriteLine("[HotKeyCore] 键盘钩子已安装");
            }
        }

        /// <summary>
        /// 卸载键盘钩子
        /// </summary>
        public void Stop()
        {
            if (_hookId != IntPtr.Zero)
            {
                // 传入安装时获得的钩子句柄，从钩子链中移除
                UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
                Debug.WriteLine("[HotKeyCore] 键盘钩子已卸载");
            }
        }

        #endregion

        #region 钩子回调

        /// <summary>
        /// 低级键盘钩子回调（由系统在按键时调用，可能运行于非 UI 线程）
        /// </summary>
        /// <param name="nCode">
        /// 钩子代码，决定本回调如何处理消息：
        ///   - nCode &gt;= 0（即 HC_ACTION）：表示 wParam/lParam 携带有效的键盘消息，应正常处理；
        ///   - nCode &lt; 0：表示本钩子不应处理该消息，必须原样转交给 CallNextHookEx。
        /// </param>
        /// <param name="wParam">
        /// 键盘消息类型。此处仅关注 WM_KEYDOWN（普通键按下）与 WM_SYSKEYDOWN（系统键按下，
        /// 如 Alt 组合键），其余消息（如 WM_KEYUP 抬起）直接忽略并传递给下一个钩子。
        /// </param>
        /// <param name="lParam">
        /// 指向 KBDLLHOOKSTRUCT 结构体的指针，内含本次按键的虚拟键码、扫描码、标志位等信息，
        /// 需通过 Marshal.PtrToStructure 解包后读取。
        /// </param>
        /// <returns>
        /// 返回 CallNextHookEx 的结果，把消息继续沿钩子链向下传递；除非需要吞掉该消息，
        /// 否则不应返回其他值。
        /// </returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int msg = wParam.ToInt32();
                if (msg == WM_KEYDOWN || msg == WM_SYSKEYDOWN)
                {
                    KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                    Keys key = (Keys)kb.vkCode;

                    // 检测当前按住的修饰键，匹配对应复合键
                    bool shiftDn = (GetKeyState(VK_SHIFT) & 0x8000) != 0;
                    bool altDn = (GetKeyState(VK_MENU) & 0x8000) != 0;
                    bool ctrlDn = (GetKeyState(VK_CONTROL) & 0x8000) != 0;

                    // 统计按下的修饰键数量
                    int modCount = (shiftDn ? 1 : 0) + (altDn ? 1 : 0) + (ctrlDn ? 1 : 0);

                    // 仅当恰好一个修饰键按下时触发
                    if (modCount == 1)
                    {
                        HotKeyModifier activeMod = ctrlDn ? HotKeyModifier.Ctrl
                            : altDn ? HotKeyModifier.Alt
                            : HotKeyModifier.Shift;

                        string ck = MakeKey(key, activeMod);

                        if (_hotkeyActions.TryGetValue(ck, out HotKeyHandler handler))
                        {
                            string name = _hotkeyNames.TryGetValue(ck, out string n) ? n : key.ToString();
#if DEBUG
                            string lbl = ctrlDn ? "CTRL" : altDn ? "ALT" : "SHIFT";
                            Debug.WriteLine($"[HotKeyCore] 触发热键: {lbl}+{key} -> \"{name}\"");
#endif
                            // 跨线程同步到 UI 线程执行回调
                            _syncControl.BeginInvoke(new Action(() =>
                            {
                                try { handler(name); }
                                catch (Exception ex) { Debug.WriteLine($"[HotKeyCore] 回调异常: {ex.Message}"); }
                            }));
                        }
                    }
                }
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        #endregion

        #region 资源释放

        /// <summary>
        /// 释放资源：卸载钩子并清空热键映射
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                UnregisterAll();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 析构函数：确保未显式释放时也能清理钩子
        /// </summary>
        ~HotKeyCore()
        {
            Dispose();
        }

        #endregion
    }
}
