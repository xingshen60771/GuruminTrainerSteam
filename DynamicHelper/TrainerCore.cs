using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GuruminTrainerSteam.DynamicHelper
{
    /// <summary>
    /// 修改器主体类
    /// </summary>
    public class TrainerCore
    {
        #region Windows API 声明
        // ==========================================
        // Windows API 声明
        // ==========================================
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
        #endregion

        #region 变量常量表

        // 权限常量
        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const uint PAGE_EXECUTE_READWRITE = 0x40;

        // 内存申请/释放常量(代码注入用)
        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RESERVE = 0x2000;
        private const uint MEM_RELEASE = 0x8000;

        // ==========================================
        // 对外暴露的状态属性
        // ==========================================

        /// <summary>修改器是否已成功激活</summary>
        public bool IsActivated { get; private set; }

        /// <summary>最后一次操作的描述信息(含成功/错误详情)</summary>
        public string LastMessage { get; private set; }

        /// <summary>游戏进程的操作句柄(激活成功后保持打开)</summary>
        public IntPtr ProcessHandle { get; private set; }

        /// <summary>游戏主模块的 .text 区段运行时基址</summary>
        public IntPtr TextSectionBase { get; private set; }

        /// <summary>游戏进程对象(激活成功后保持引用)</summary>
        public Process GameProcess { get; private set; }

        // ==========================================
        // 修改器目标常量
        // ==========================================

        // 基本属性地址
        // ------------------------------------------
        private const int RVA_MONEY = 0xB37298;    // 金钱
        private const int RVA_FRAGMENT = 0xF354B4; // 装备类道具碎片

        // 道具属性地址
        // ------------------------------------------
        // 道具基址 RVA
        private const int RVA_ITEM_BASE = 0x5E2400;
        // 消耗类道具偏移
        private readonly int[] OFFSET_ITEM_CONSUMABLE = new int[]
        {
            0x04,   // 1.曲奇饼
            0x05,   // 2.巧克力
            0x13,   // 3.强化油
            0x14,   // 4.蛋糕
            0x16,   // 5.耐久油
        };
        // 装备类道具基址RVA
        private const int RVA_EQUIP_BASE = 0xF34000;

        // 防具等级地址偏移 ,第一列为相对于道具基址的偏移,第二列为相当于装备类道具等级基址的偏移
        // ------------------------------------------
        public readonly int[,] EQUIP_DATA = new int[,]
        {
          { 0x29,   0xEEC }, // 1.潜水眼镜
          { 0x2A,   0xEF0 }, // 2.毒气面罩
          { 0x2D,   0xEF4 }, // 3.丝带
          { 0x15,   0xEF8 }, // 4.吸血帽子
          { 0x28,   0xEFC }, // 5.猫咪
          { 0x2B,   0xF00 }, // 6.安全帽
          { 0x2C,   0xF04 }, // 7.扎头巾
          { 0x2F,   0xF08 }, // 8.魔法头巾
          { 0x2E,   0xF0C }, // 9.武士钢盔
          { 0x30,   0xF10 }, // 10.霸者之面
          { 0x3D,   0xF14 }, // 11.王冠
          { 0x38,   0xF18 }, // 12.奇异的帽子
          { 0x39,   0xF1C }, // 13.睡帽
          { 0x25,   0xF20 }, // 14.宠物衣裳
          { 0x3C,   0xF24 }, // 15.团子
        };

        // 【核心】屏蔽数据校验的补丁字节序列
        // 此处的常量用于修改器初始化与激活,因为Steam版带有基本属性数值自校验,所以需要在游戏运行时屏蔽校验逻辑,否则修改器闪退。
        // ------------------------------------------
        // 此处工作原理:将字节覆盖成 "xor eax,eax; ret" 指令序列,直接返回 0,绕过校验逻辑
        private const int OFFSET_NOCHECK1 = 0x1F3650;// 创建校验数据的函数入口偏移
        private const int OFFSET_NOCHECK2 = 0x1B33F0;// 校验数据流程的函数入口偏移
        private byte[] BYTES_NOCHECK_ORIGINAL1 = new byte[] { 0xA1, 0xFC, 0x81 };// 创建校验数据的原始字节序列
        private byte[] BYTES_NOCHECK_ORIGINAL2 = new byte[] { 0x66, 0xFF, 0x05 };// 校验数据流程的原始字节序列
        private byte[] BYTES_NOCHECK_PATCHED = new byte[] { 0x33, 0xC0, 0xC3 }; //  xor eax,eax; ret

        // 必杀技修改
        // ------------------------------------------
        private const int RVA_ULTIMATE = 0xF33854;// 必杀技持有情况基址

        public readonly int[] OFFSET_ULTIMATE = new int[]    // 资源上限检测的偏移
        {
            0x00,   // 1.前转斩
            0x0C,   // 2.可爱导弹飞腿
            0x08,   // 3.巨龙火焰
            0x10,   // 4.旋转下突击
            0x14,   // 5.钻头龙卷回波
            0x18,   // 6.旋转乱舞
        };

        // 主角无敌补丁字节序列
        // ------------------------------------------
        private const int OFFSET_GOD1 = 0x1C8D72;
        private const int OFFSET_GOD2 = 0x1C8D78;
        private byte[] BYTES_GOD_ORIGINAL1 = new byte[] { 0x01 };
        private byte[] BYTES_GOD_ORIGINAL2 = new byte[] { 0x85 };
        private byte[] BYTES_GOD_PATCHED1 = new byte[] { 0x00 };
        private byte[] BYTES_GOD_PATCHED2 = new byte[] { 0x33 };

        // 无限跳跃补丁字节序列
        // ------------------------------------------
        private const int OFFSET_FREEJUMP = 0x213E49; // 此处字节原本为0x00,表示非跳跃状态
        private byte[] BYTES_FREEJUMP_ORIGINAL = new byte[] { 0x00 };// 此处字节原本为0x00,表示非跳跃状态
        private byte[] BYTES_FREEJUMP_PATCHED = new byte[] { 0x01 };// 改成0x01即可无限跳跃

        // 一击必杀补丁字节序列
        // ------------------------------------------
        private const int OFFSET_SUPERKILL = 0x1C4F6A;
        private byte[] BYTES_SUPERKILL_ORIGINAL = new byte[] { 0x28, 0xD6 };
        private byte[] BYTES_SUPERKILL_PATCHED = new byte[] { 0x33, 0xD2 };

        // 快速移动
        // ------------------------------------------
        // 原理:hook 坐标写入函数 game.exe+37A173,只对玩家 transform 放大本帧 X/Y 的写入增量
        //       newX = oldX + (targetX - oldX) * factor , Y 同理 , Z 不动
        //       增量超出 ±gmMaxDelta 的视为传送/切图,原样写回不放大,防止切图时人被甩出地图
        // 注意:CE 脚本里的 game.exe+XXX 是相对"模块基址"的,而本类所有偏移都是相对".text基址"的,
        //       所以这里统一减 0x1000(写法与 RVA_ALLMASTER_OFFSET、RVA_NOGAMEOVER_OFFSET 一致)
        private const int OFFSET_FASTMOVE = 0x37A173 - 0x1000;          // 快速移动注入的入口点(CE: game.exe+37A173)
        private const int OFFSET_FASTMOVE_RETURN = 0x37A178 - 0x1000;   // 快速移动注入返回点(入口点+5,即被顶掉的5字节之后)
        private const int RVA_FASTMOVE_TRANSFORM = 0x108A318 - 0x1000;  // 玩家transform对象(CE: game.exe+108A318,X/Y/Z=+30/+34/+38)
        private const int FASTMOVE_CAVE_SIZE = 0x1000;                  // 申请的内存空间大小

        // 注入点原始字节: movss xmm0,[ebp+08],关闭功能时用它还原
        private readonly byte[] BYTES_FASTMOVE_ORIGINAL = { 0xF3, 0x0F, 0x10, 0x45, 0x08 };

        // gmCoordMem 内存块模板,共 0x92 字节。所有值为 0x00 的四字节地址位,由 FastMoveInit() 在运行时回填
        private readonly byte[] BYTES_FASTMOVE_PATCHED = {
            0x3B, 0x0D, 0x00, 0x00, 0x00, 0x00,                           // +000  cmp ecx,[gmPlayerTransform]     不是玩家的transform就不动
            0x75, 0x70,                                                   // +006  jne gmCoordOriginal   → +78
            0xF3, 0x0F, 0x10, 0x45, 0x08,                                 // +008  movss xmm0,[ebp+08]             取本次要写的 X
            0xF3, 0x0F, 0x5C, 0x41, 0x30,                                 // +00D  subss xmm0,[ecx+30]             算出 X 增量
            0x0F, 0x2F, 0x05, 0x00, 0x00, 0x00, 0x00,                     // +012  comiss xmm0,[gmMaxDelta]
            0x77, 0x5D,                                                   // +019  ja  gmSkipBoost       → +78     X增量过大=传送,不放大
            0x0F, 0x2F, 0x05, 0x00, 0x00, 0x00, 0x00,                     // +01B  comiss xmm0,[gmNegMaxDelta]
            0x72, 0x54,                                                   // +022  jb  gmSkipBoost       → +78
            0xF3, 0x0F, 0x10, 0x45, 0x0C,                                 // +024  movss xmm0,[ebp+0C]             取本次要写的 Y
            0xF3, 0x0F, 0x5C, 0x41, 0x34,                                 // +029  subss xmm0,[ecx+34]             算出 Y 增量
            0x0F, 0x2F, 0x05, 0x00, 0x00, 0x00, 0x00,                     // +02E  comiss xmm0,[gmMaxDelta]
            0x77, 0x41,                                                   // +035  ja  gmSkipBoost       → +78     Y增量过大=传送,不放大
            0x0F, 0x2F, 0x05, 0x00, 0x00, 0x00, 0x00,                     // +037  comiss xmm0,[gmNegMaxDelta]
            0x72, 0x38,                                                   // +03E  jb  gmSkipBoost       → +78
            0xF3, 0x0F, 0x10, 0x45, 0x08,                                 // +040  movss xmm0,[ebp+08]
            0xF3, 0x0F, 0x5C, 0x41, 0x30,                                 // +045  subss xmm0,[ecx+30]
            0xF3, 0x0F, 0x59, 0x05, 0x00, 0x00, 0x00, 0x00,               // +04A  mulss xmm0,[gmCoordFactor]      X增量 × 倍率
            0xF3, 0x0F, 0x58, 0x41, 0x30,                                 // +052  addss xmm0,[ecx+30]
            0xF3, 0x0F, 0x11, 0x45, 0x08,                                 // +057  movss [ebp+08],xmm0             写回放大后的 X
            0xF3, 0x0F, 0x10, 0x45, 0x0C,                                 // +05C  movss xmm0,[ebp+0C]
            0xF3, 0x0F, 0x5C, 0x41, 0x34,                                 // +061  subss xmm0,[ecx+34]
            0xF3, 0x0F, 0x59, 0x05, 0x00, 0x00, 0x00, 0x00,               // +066  mulss xmm0,[gmCoordFactor]      Y增量 × 倍率
            0xF3, 0x0F, 0x58, 0x41, 0x34,                                 // +06E  addss xmm0,[ecx+34]
            0xF3, 0x0F, 0x11, 0x45, 0x0C,                                 // +073  movss [ebp+0C],xmm0             写回放大后的 Y
            0xF3, 0x0F, 0x10, 0x45, 0x08,                                 // +078  gmSkipBoost/gmCoordOriginal:被顶掉的原指令
            0xE9, 0x00, 0x00, 0x00, 0x00,                                 // +07D  jmp gmCoordReturn               跳回 注入点+5
            0x00, 0x00, 0xC0, 0x3F,                                       // +082  gmCoordFactor     dd 3FC00000  = 1.5
            0x00, 0x00, 0x70, 0x42,                                       // +086  gmMaxDelta        dd 42700000  = 60.0
            0x00, 0x00, 0x70, 0xC2,                                       // +08A  gmNegMaxDelta     dd C2700000  = -60.0
            0x00, 0x00, 0x00, 0x00,                                       // +08E  gmPlayerTransform dd game.exe+108A318
        };

        // gmCoordMem 内部的"地址回填位"(指令里 disp32 / rel32 字段相对内存块起点的偏移)
        private const int GM_REF_PLAYERTRANSFORM = 0x002;   // cmp ecx,[gmPlayerTransform]
        private const int GM_REF_MAXDELTA_1 = 0x015;        // comiss xmm0,[gmMaxDelta]      (X轴)
        private const int GM_REF_NEGMAXDELTA_1 = 0x01E;     // comiss xmm0,[gmNegMaxDelta]   (X轴)
        private const int GM_REF_MAXDELTA_2 = 0x031;        // comiss xmm0,[gmMaxDelta]      (Y轴)
        private const int GM_REF_NEGMAXDELTA_2 = 0x03A;     // comiss xmm0,[gmNegMaxDelta]   (Y轴)
        private const int GM_REF_FACTOR_1 = 0x04E;          // mulss xmm0,[gmCoordFactor]    (X轴)
        private const int GM_REF_FACTOR_2 = 0x06A;          // mulss xmm0,[gmCoordFactor]    (Y轴)
        private const int GM_REF_RETURN_REL32 = 0x07E;      // jmp gmCoordReturn 的 rel32 字段

        // gmCoordMem 内部的"数据区"偏移(CE 脚本里 registersymbol 的那几个变量)
        private const int GM_DATA_FACTOR = 0x082;           // gmCoordFactor    默认 1.5,调速就改这里
        private const int GM_DATA_MAXDELTA = 0x086;         // gmMaxDelta       默认 60.0
        private const int GM_DATA_NEGMAXDELTA = 0x08A;      // gmNegMaxDelta    默认 -60.0
        private const int GM_DATA_PLAYERTRANSFORM = 0x08E;  // gmPlayerTransform 玩家transform运行时地址

        private readonly float[] FLOAT_SPEEDLEVEL = { 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f };// 预设的速度倍率
        private int OFFSET_FASTMOVE_PATCHED;//此处用于记录申请好的内存空间的地址偏移，后续开关快速移动功能都以此为准

        // 游戏总时间
        // ------------------------------------------
        private const int RVA_GAMETIME = 0x5E23A8;

        // 修改后字节
        private readonly byte[] BYTES_FOOTBALL_PATCHED = new byte[] {
            0xE9, 0xB3, 0x00, 0x00, 0x00
         };

        // 游戏难度
        // ------------------------------------------
        private const int RVA_DIFFICULTY = 0xF323A0;// 游戏难度偏移
        private readonly byte BYTES_DIFFICULTY_NORMAL = 0x00;// 普通难度
        private readonly byte BYTES_DIFFICULTY_BEGINNER = 0x01;// 初学者难度
        private readonly byte BYTES_DIFFICULTY_HARD = 0x02;// 困难难度
        private readonly byte BYTES_DIFFICULTY_PLEASURE = 0x03;// 娱乐难度
        private readonly byte BYTES_DIFFICULTY_CRAZY = 0x05;// 疯狂难度

        // 突破资源上限
        // ------------------------------------------
        public readonly int[] OFFSET_UNLOCKRESOURCELIMIT = new int[]    // 资源上限检测的偏移
        {
            0x1D1A72 + 2,   // 金钱1,此次原始值为0x0001869F,四字节
            0x1D1A7A + 1,   // 金钱2,此次原始值为0x0001869F,四字节
            0x1D1B43 + 2,   // 碎片1,此次原始值为0x000003E7,四字节
            0x1D1B4B + 1,   // 碎片2,此次原始值为0x000003E7,四字节
            0x1EB478 + 6,   // 道具1,此次原始值为0x04,单字节
            0x1EB48C + 2,   // 道具2,此次原始值为0x03,单字节
            0x1EB498 + 6,   // 道具3,此次原始值为0x03,单字节
            0x249C95 + 1,   // 碎片3,此次原始值为0x000003E7,四字节
            0x249CB3 + 1,   // 碎片4,此次原始值为0x000003E7,四字节
            0x249CB9 + 1,   // 碎片5,此次原始值为0x000003E7,四字节
            0x249CE3 + 1,   // 碎片6,此次原始值为0x000003E7,四字节
            0x249CE9 + 1,   // 碎片7,此次原始值为0x000003E7,四字节
            0x249D4D + 1,   // 碎片8,此次原始值为0x000003E7,四字节
            0x249D53 + 1,   // 碎片9,此次原始值为0x000003E7,四字节
            0x24A155 + 1,   // 碎片10,此次原始值为0x000003E7,四字节
            0x24A15B + 1,   // 碎片10,此次原始值为0x000003E7,四字节
        };

        // 原始字节序列
        private byte[][] BYTES_UNLOCKRESOURCELIMIT_ORIGINAL = new byte[][]
        {
            new byte[] { 0x9F, 0x86, 0x01, 0x00 },           // 金钱1,此次原始值为0x0001869F,四字节
            new byte[] { 0x9F, 0x86, 0x01, 0x00 },           // 金钱2,此次原始值为0x0001869F,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片1,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片2,此次原始值为0x000003E7,四字节
            new byte[] { 0x04 },                             // 道具1,此次原始值为0x04,单字节
            new byte[] { 0x03 },                             // 道具2,此次原始值为0x03,单字节
            new byte[] { 0x03 },                             // 道具3,此次原始值为0x03,单字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片3,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片4,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片5,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片6,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片7,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片8,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片9,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片8,此次原始值为0x000003E7,四字节
            new byte[] { 0xE7, 0x03, 0x00, 0x00 },           // 碎片9,此次原始值为0x000003E7,四字节
        };
        // 补丁字节序列
        private byte[][] BYTES_UNLOCKRESOURCELIMIT_PATCHED = new byte[][]
        {
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 金钱1,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 金钱2,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片1,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片2,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF },                             // 道具1,此次补丁值为0x04,单字节
            new byte[] { 0x7F },                             // 道具2,此次补丁值为0x03,单字节
            new byte[] { 0x7F },                             // 道具3,此次补丁值为0x03,单字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片3,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片4,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片5,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片6,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片7,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片8,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片9,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片8,此次补丁值为0x7FFFFFFF,四字节
            new byte[] { 0xFF, 0xFF, 0xFF, 0x7F },           // 碎片9,此次补丁值为0x7FFFFFFF,四字节
        };


        // 20%强化油
        // ------------------------------------------
        private const int OFFSET_ENHANCEOILPLUS1 = 0x200F46;
        private const int OFFSET_ENHANCEOILPLUS2 = 0x1F107B;
        private byte[] BYTES_ENHANCEOILPLUS_ORIGINAL = new byte[] { 0x64 };
        private byte[] BYTES_ENHANCEOILPLUS_PATCHED = new byte[] { 0x78 };

        // 钻头属性
        // ------------------------------------------
        private const int OFFSET_FOURDRILL = 0xF33878;      // 四级钻头开启状态偏移
        private const int OFFSET_DRILLLEVEL = 0xF3387C;     // 钻头等级偏移
        private const int OFFSET_DRILLENERGY = 0xF33880;    // 钻头能量偏移

        // 耐久油时间不减
        // ------------------------------------------
        private const int OFFSET_UNLIMITDURABLEOIL = 0x1B5DA1;
        private byte[] BYTES_UNLIMITDURABLEOIL_ORIGINAL = new byte[] { 0x4E };
        private byte[] BYTES_UNLIMITDURABLEOIL_PATCHED = new byte[] { 0x90 };

        // 敌人、宝壶、宝箱全清
        // ------------------------------------------
        private readonly int[] RVA_ALLMASTER_OFFSET = // 相对基址的偏移量
        {
            0x20004F-0x1000+0x2,//敌人全清
            0x2000F4-0x1000+0x2,//宝壶全清
            0x200114-0x1000+0x2,//宝箱全清
        };

        private const int RVA_ALLMASTER_ORIGINAL_OFFSET = 0xF33314 - 0x1000;//原始内存地址偏移

        private readonly int[] RVA_ALLMASTER_PATCHED_OFFSET =// 修改后内存地址偏移
        {
            0xF33C84-0x1000,//敌人全清
            0xF33CE4-0x1000,//宝壶全清
            0xF33D74-0x1000,//宝箱全清
        };

        // 结算时不累计GameOver
        // ------------------------------------------
        private const int RVA_NOGAMEOVER_OFFSET = 0x2001BD - 0x1000;// 相对基址的偏移量
        private const int RVA_NOGAMEOVER_ORIGINAL_OFFSET = 0xF33DA8 - 0x1000;// GameOver次数原始地址
        private readonly byte[] BYTES_NOGAMEOVER_PATCHED = new byte[] { 0x00, 0x00, 0x00, 0x00 };// 直接NOP

        // 敌方碎石头白送分
        // ------------------------------------------
        private const int OFFSET_ROCKCRUSH = 0x1F48BD;// 相对基址的偏移量
        private readonly byte BYTES_ROCKCRUSH_ORIGINAL = 0x2D; // 原始字节
        private readonly byte BYTES_ROCKCRUSH_PATCHED = 0x00; // 修改后字节

        // 敌方进球白送分
        // ------------------------------------------
        private const int OFFSET_FOOTBALL = 0x2518F1;// 相对基址的偏移量
        private const int RVA_ENEMY_SCORE = 0x515F80;//敌方进球积分临时地址偏移
        // 原始字节
        byte BYTES_FOOTBALL_ORIGINAL = 0xA1;

        // 无限打地鼠
        // ------------------------------------------
        private const int OFFSET_MOLECOMBO = 0x2357C2; // 相对基址的偏移量
        // 原字节
        private readonly byte[] BYTES_MOLECOMBO_ORIGINAL = new byte[] {
            0xEC
        };
        // 修改后字节
        private readonly byte[] BYTES_MOLECOMBO_PATCHED = new byte[] {
            0x00
         };

        // 打地鼠/足球赛时间暂停
        // ------------------------------------------
        private const int OFFSET_TIMEPAUSE_MOLEHIT = 0x235757;
        private const int OFFSET_TIMEPAUSE_FOOTBALL = 0x24D7D2;
        private byte[] BYTES_TIMEPAUSE_ORIGINAL = new byte[] { 0x4E };
        private byte[] BYTES_TIMEPAUSE_PATCHED = new byte[] { 0x90 };

        // 无限躲避激光
        // ------------------------------------------
        private const int OFFSET_LASERDODG = 0x238034; // 相对基址的偏移量
                                                       // 原字节
        private readonly byte[] BYTES_LASERDODG_ORIGINAL = new byte[] {
            0x74 ,0x44
        };
        // 修改后字节
        private readonly byte[] BYTES_LASERDODG_PATCHED = new byte[] {
            0x90 , 0x90
         };

        #endregion

        #region 激活与内存辅助

        /// <summary>
        /// 执行一次扫描+激活
        /// <returns><para>true=激活成功;false=未找到或失败</para></returns>
        /// </summary>
        public bool Activate()
        {
            // 防止重复激活
            if (IsActivated)
            {
                LastMessage = "修改器已处于激活状态";
                return true;
            }

            // Step1:遍历进程,查找游戏窗口
            Process targetProcess = null;
            foreach (var proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.MainWindowTitle == "Gurumin Ver.1.4 (STEAM)")
                    {
                        targetProcess = proc;
                        break;
                    }
                }
                catch
                {
                    // 某些系统进程无权限访问 MainWindowTitle,跳过
                    continue;
                }
            }

            if (targetProcess == null)
            {
                LastMessage = "未找到游戏窗口 'Gurumin Ver.1.4 (STEAM)'";
                return false;
            }

            // Step2:解析 PE 头,获取 .text 区段运行时基址
            IntPtr textSectionBase = GetTextSectionBase(targetProcess);
            if (textSectionBase == IntPtr.Zero)
            {
                LastMessage = "无法定位 .text 区段基址";
                return false;
            }

            // Step2:打开进程句柄
            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, targetProcess.Id);
            if (hProcess == IntPtr.Zero)
            {
                LastMessage = "无法打开进程,请以管理员身份运行修改器";
                return false;
            }

            /* 在此鸣谢52pojie @Coxxs！帮我理清了重要属性绕过防篡改校验的思路，相关讨论帖：
            https://www.52pojie.cn/thread-2112391-1-1.html
            */

            // 补丁1: 屏蔽创建校验数据 (text + 0x1F3650)
            bool patch1 = ApplyPatch(hProcess,
                IntPtr.Add(textSectionBase, OFFSET_NOCHECK1),
                BYTES_NOCHECK_PATCHED,
                "创建校验数据");

            // 补丁2: 屏蔽校验数据流程 (text + 0x1B33F0)
            bool patch2 = ApplyPatch(hProcess,
                IntPtr.Add(textSectionBase, OFFSET_NOCHECK2),
                BYTES_NOCHECK_PATCHED,
                "校验数据流程");

            if (patch1 && patch2)
            {
                // 激活成功:保存句柄和状态,不关闭句柄
                ProcessHandle = hProcess;
                TextSectionBase = textSectionBase;
                GameProcess = targetProcess;
                IsActivated = true;

                /* 
                 Step4::激活成功后立即初始化快速移动的内存块,提前把 gmCoordMem 申请并写好
                 注意:必须放在 IsActivated=true 之后,FastMoveInit() 内部的安全判定要用到这个状态
                 就算初始化失败也不影响本次激活结果——移速是附加功能,不能让它拖累整体激活;
                 玩家真正点开快速移动开关时,FastMove() 里还有一次兜底初始化会再试一次 
                */
                if (!FastMoveInit())
                {
                    LastMessage = "安全修改环境已构建完毕(快速移动初始化失败,可在开启快速移动时重试)";
                }
                else
                {
                    LastMessage = "安全修改环境已构建完毕";
                }

                return true;
            }
            else
            {
                // 补丁失败:释放资源
                CloseHandle(hProcess);
                LastMessage = "部分补丁应用失败";
                return false;
            }
        }

        /// <summary>
        /// &lt;逻辑型&gt;应用补丁
        /// <param name="hProcess">(IntPtr 目标进程, </param>
        /// <param name="address">IntPtr 目标地址, </param>
        /// <param name="patch">字节集 欲补丁的字节, </param>
        /// <param name="description">文本型 相关说明)</param>
        /// <returns><para>成功返回真</para></returns>
        /// </summary>
        private bool ApplyPatch(IntPtr hProcess, IntPtr address, byte[] patch, string description)
        {
            // 先读取原始字节进行对比
            byte[] original = new byte[patch.Length];
            ReadProcessMemory(hProcess, address, original, patch.Length, out _);

            // 修改内存保护属性为可执行+可读+可写
            VirtualProtectEx(hProcess, address, patch.Length, PAGE_EXECUTE_READWRITE, out uint oldProtect);

            // 写入补丁
            bool success = WriteProcessMemory(hProcess, address, patch, patch.Length, out int bytesWritten);

            // 恢复原始内存保护属性
            VirtualProtectEx(hProcess, address, patch.Length, oldProtect, out _);

            if (!success || bytesWritten != patch.Length)
            {
                LastMessage = $"补丁 [{description}] 写入失败 (written={bytesWritten}, err={Marshal.GetLastWin32Error()})";
            }

            return success;
        }

        /// <summary>
        /// &lt;IntPtr&gt;取 .text 区段基址
        /// <param name="process">(Process 目标进程)</param>
        /// <returns><para>成功返回基址</para></returns>
        /// </summary>
        private IntPtr GetTextSectionBase(Process process)
        {
            IntPtr moduleBase = process.MainModule.BaseAddress;

            // 读取 DOS 头
            byte[] dosHeader = new byte[64];
            ReadProcessMemory(process.Handle, moduleBase, dosHeader, 64, out _);
            int peOffset = BitConverter.ToInt32(dosHeader, 0x3C);

            // 读取 PE 签名 + COFF 头
            byte[] coffHeader = new byte[24];
            ReadProcessMemory(process.Handle, IntPtr.Add(moduleBase, peOffset), coffHeader, 24, out _);
            int numberOfSections = BitConverter.ToInt16(coffHeader, 6);
            int sizeOfOptionalHeader = BitConverter.ToInt16(coffHeader, 20);

            // 跳过可选头,到达区段表
            int sectionTableOffset = peOffset + 24 + sizeOfOptionalHeader;

            // 遍历区段表查找 .text
            byte[] sectionEntry = new byte[40];
            for (int i = 0; i < numberOfSections; i++)
            {
                ReadProcessMemory(process.Handle,
                    IntPtr.Add(moduleBase, sectionTableOffset + i * 40),
                    sectionEntry, 40, out _);

                string name = Encoding.ASCII.GetString(sectionEntry, 0, 8).TrimEnd('\0');
                if (name == ".text")
                {
                    int virtualAddress = BitConverter.ToInt32(sectionEntry, 12);
                    return IntPtr.Add(moduleBase, virtualAddress);
                }
            }

            return IntPtr.Zero;
        }
        #endregion

        #region 基本属性修改

        /// <summary>
        /// 作弊功能1：主角无敌
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void God(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 补丁1:无敌状态触发位
            IntPtr addr1 = IntPtr.Add(TextSectionBase, OFFSET_GOD1);
            byte[] buf1 = enable ? BYTES_GOD_PATCHED1 : BYTES_GOD_ORIGINAL1;
            WriteProcessMemory(ProcessHandle, addr1, buf1, buf1.Length, out _);

            // 补丁2:无敌判定逻辑
            IntPtr addr2 = IntPtr.Add(TextSectionBase, OFFSET_GOD2);
            byte[] buf2 = enable ? BYTES_GOD_PATCHED2 : BYTES_GOD_ORIGINAL2;
            WriteProcessMemory(ProcessHandle, addr2, buf2, buf2.Length, out _);
        }

        /// <summary>
        /// 作弊功能2：无限跳跃
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void FreeJump(bool enable)
        {
            // 安全判定:如果没有激活成功,直接退出
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 核心寻址算法:.text 区段基址 + 无限跳跃偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_FREEJUMP);

            // 根据传入的布尔值,决定写入哪一套字节码
            byte[] bufferToWrite = enable ? BYTES_FREEJUMP_PATCHED : BYTES_FREEJUMP_ORIGINAL;

            // 将字节码强行写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// 作弊功能3：一击必杀
        /// </summary>
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        public void SuperKill(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 核心寻址:.text 基址 + 一击必杀偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_SUPERKILL);

            // 选字节:开启时写入 patched,关闭时还原 original
            byte[] bufferToWrite = enable ? BYTES_SUPERKILL_PATCHED : BYTES_SUPERKILL_ORIGINAL;

            // 写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// 作弊功能4：设置快速移动速度
        /// <param name="speedLevel">(整数型 欲设置的速度等级)</param>
        /// <para>0=关闭并还原注入点;1~6=对应 FLOAT_SPEEDLEVEL 的预设倍率</para>
        /// </summary>
        public void FastMove(int speedLevel)
        {
            // 安全判定:如果没有激活成功,直接退出
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 档位越界直接退出(0=不加速,1~6=映射到数组下标 0~5)
            if (speedLevel < 0 || speedLevel > FLOAT_SPEEDLEVEL.Length) return;

            // 核心寻址算法:.text 区段基址 + 快速移动注入点偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_FASTMOVE);

            // 档位0:把注入点还原成原始5字节 movss xmm0,[ebp+08],执行流不再进入我们的内存块
            // 注意:这里只还原注入点,不释放内存块,重新开启时就不用再申请一次内存了
            if (speedLevel == 0)
            {
                ApplyPatch(ProcessHandle, targetAddress, BYTES_FASTMOVE_ORIGINAL, "快速移动-还原");
                return;
            }

            // 档位1~6:内存块还没准备好就先初始化,初始化失败直接退出
            if (OFFSET_FASTMOVE_PATCHED == 0 && !FastMoveInit()) return;

            // Step1: 先把倍率写进内存块的 gmCoordFactor,再挂钩子
            //        顺序不能反,否则挂上去的那一瞬间会先用到上一档的旧倍率
            float speedFactor = FLOAT_SPEEDLEVEL[speedLevel - 1];
            IntPtr factorAddress = (IntPtr)(OFFSET_FASTMOVE_PATCHED + GM_DATA_FACTOR);
            WriteProcessMemory(ProcessHandle, factorAddress, BitConverter.GetBytes(speedFactor), 4, out _);

            // Step2: 拼 jmp 指令 = 0xE9 + rel32,rel32 = 内存块地址 - (注入点地址 + 5)
            byte[] bufferToWrite = new byte[5];
            bufferToWrite[0] = 0xE9;
            int jmpRelative = OFFSET_FASTMOVE_PATCHED - (targetAddress.ToInt32() + 5);
            Buffer.BlockCopy(BitConverter.GetBytes(jmpRelative), 0, bufferToWrite, 1, 4);

            // Step3: 写入注入点。已经是同一条 jmp 时重复写是无害的,所以切换档位不必先还原
            ApplyPatch(ProcessHandle, targetAddress, bufferToWrite, "快速移动-注入");
        }

        /// <summary>
        /// &lt;逻辑型&gt;快速移动功能初始化
        /// <returns><para>内存块已就绪返回真，申请或写入失败返回假</para></returns>
        /// </summary>
        public bool FastMoveInit()
        {
            // 安全判定:如果没有激活成功,直接退出
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero)
            {
                LastMessage = "修改器未激活,无法初始化快速移动";
                return false;
            }

            // 已经申请过就直接复用,重复申请会造成游戏进程内存泄漏
            if (OFFSET_FASTMOVE_PATCHED != 0) return true;

            // Step1: 在游戏进程里申请 0x1000 字节,权限为可读可写可执行
            // 说明:游戏是32位进程,jmp rel32 的目标是按 2^32 取模算出来的,内存申请在哪都够得着,不必就近分配
            IntPtr codeCaveBase = VirtualAllocEx(ProcessHandle, IntPtr.Zero, FASTMOVE_CAVE_SIZE,
                                                 MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
            if (codeCaveBase == IntPtr.Zero)
            {
                LastMessage = $"快速移动:申请内存失败 (err={Marshal.GetLastWin32Error()})";
                return false;
            }

            int caveAddress = codeCaveBase.ToInt32();
            int baseAddress = TextSectionBase.ToInt32();

            // Step2: 复制一份模板再回填,不要直接改常量数组,否则二次初始化会拿到上一次的脏地址
            byte[] bufferToWrite = (byte[])BYTES_FASTMOVE_PATCHED.Clone();

            // Step3: 回填7处指向内存块内部数据区的绝对地址(CE脚本里的 gmXXX 标签)
            WriteInt32ToBuffer(bufferToWrite, GM_REF_PLAYERTRANSFORM, caveAddress + GM_DATA_PLAYERTRANSFORM);
            WriteInt32ToBuffer(bufferToWrite, GM_REF_MAXDELTA_1, caveAddress + GM_DATA_MAXDELTA);
            WriteInt32ToBuffer(bufferToWrite, GM_REF_NEGMAXDELTA_1, caveAddress + GM_DATA_NEGMAXDELTA);
            WriteInt32ToBuffer(bufferToWrite, GM_REF_MAXDELTA_2, caveAddress + GM_DATA_MAXDELTA);
            WriteInt32ToBuffer(bufferToWrite, GM_REF_NEGMAXDELTA_2, caveAddress + GM_DATA_NEGMAXDELTA);
            WriteInt32ToBuffer(bufferToWrite, GM_REF_FACTOR_1, caveAddress + GM_DATA_FACTOR);
            WriteInt32ToBuffer(bufferToWrite, GM_REF_FACTOR_2, caveAddress + GM_DATA_FACTOR);

            // Step4: 回填返回用的 jmp rel32,公式 = 返回点地址 - (rel32字段地址 + 4)
            int returnAddress = baseAddress + OFFSET_FASTMOVE_RETURN;
            WriteInt32ToBuffer(bufferToWrite, GM_REF_RETURN_REL32,
                               returnAddress - (caveAddress + GM_REF_RETURN_REL32 + 4));

            // Step5: 回填数据区里的玩家transform运行时绝对地址 = .text基址 + transform偏移
            WriteInt32ToBuffer(bufferToWrite, GM_DATA_PLAYERTRANSFORM, baseAddress + RVA_FASTMOVE_TRANSFORM);

            // Step6: 把整块作弊字节码写进申请到的内存
            if (!WriteProcessMemory(ProcessHandle, codeCaveBase, bufferToWrite, bufferToWrite.Length, out int bytesWritten)
                || bytesWritten != bufferToWrite.Length)
            {
                LastMessage = $"快速移动:字节码写入失败 (written={bytesWritten}, err={Marshal.GetLastWin32Error()})";
                VirtualFreeEx(ProcessHandle, codeCaveBase, 0, MEM_RELEASE);
                return false;
            }

            // Step7: 记录内存块地址,后续开关与调速全部以它为准
            OFFSET_FASTMOVE_PATCHED = caveAddress;
            LastMessage = $"快速移动初始化完成,内存块地址: 0x{caveAddress:X8}";
            Console.WriteLine(LastMessage);
            return true;
        }

        /// <summary>
        /// 把一个32位整数按小端序回填到字节集的指定偏移
        /// <param name="buffer">(字节集 欲回填的字节集, </param>
        /// <param name="offset">整数型 欲回填的偏移, </param>
        /// <param name="value">整数型 欲回填的字节量)</param>
        /// </summary>
        private static void WriteInt32ToBuffer(byte[] buffer, int offset, int value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
        }

        /// <summary>
        /// &lt;逻辑型&gt;作弊功能5：游戏总时间清零
        /// <returns><para>清零成功返回真，修改器未激活或写入失败返回假</para></returns>
        /// </summary>
        public bool TimeClear()
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return false;

            // 核心寻址:.text 基址 + 游戏总时间偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, RVA_GAMETIME);

            // 写入 4 字节零值,清零游戏总时间
            byte[] zeroBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            return WriteProcessMemory(ProcessHandle, targetAddress, zeroBytes, 4, out _);
        }

        /// <summary>
        /// &lt;逻辑型&gt;作弊功能6：修改游戏难度
        /// <param name="difficultyCode">(整数型 欲修改的游戏难度)</param>
        /// <para>0=初学者, 1=普通, 2=困难, 3=娱乐, 4=疯狂</para>
        /// <returns>修改成功返回真，无效参数或未激活返回假</returns>
        /// </summary>
        public bool ChangeDifficulty(int difficultyCode)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return false;

            // 根据传入的难度编号选择对应的字节
            byte targetByte;
            switch (difficultyCode)
            {
                case 0: targetByte = BYTES_DIFFICULTY_BEGINNER; break;
                case 1: targetByte = BYTES_DIFFICULTY_NORMAL; break;
                case 2: targetByte = BYTES_DIFFICULTY_HARD; break;
                case 3: targetByte = BYTES_DIFFICULTY_PLEASURE; break;
                case 4: targetByte = BYTES_DIFFICULTY_CRAZY; break;
                default: return false;
            }

            // 写入单字节到目标地址
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, RVA_DIFFICULTY);
            byte[] bufferToWrite = new byte[] { targetByte };
            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, 1, out _);
            return true;
        }

        /// <summary>
        /// &lt;整数型&gt;取游戏难度
        /// <returns><para>0=初学者, 1=普通, 2=困难, 3=娱乐, 4=疯狂</para></returns>
        /// <exception cref="InvalidOperationException">返回的异常：无法读取游戏内存或遇到未知难度值时抛出</exception>
        /// </summary>
        public int GetDifficulty()
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero)
                throw new InvalidOperationException("修改器未激活,无法读取游戏难度。");

            // 读取目标地址的单字节
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, RVA_DIFFICULTY);
            byte[] buffer = new byte[1];
            ReadProcessMemory(ProcessHandle, targetAddress, buffer, 1, out _);
            byte rawValue = buffer[0];

            // 根据字节值匹配对应的难度编号
            if (rawValue == BYTES_DIFFICULTY_BEGINNER) return 0;
            if (rawValue == BYTES_DIFFICULTY_NORMAL) return 1;
            if (rawValue == BYTES_DIFFICULTY_HARD) return 2;
            if (rawValue == BYTES_DIFFICULTY_PLEASURE) return 3;
            if (rawValue == BYTES_DIFFICULTY_CRAZY) return 4;

            throw new InvalidOperationException($"未知的游戏难度值: 0x{rawValue:X2},当心存档损坏!");
        }
        #endregion

        #region 资源装备修改

        /// <summary>
        /// 作弊功能7：突破资源上限开关
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// <para>解除金钱99999上限、碎片999上限、道具3上限</para>
        /// </summary>
        public void UnlockResourceLimit(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 遍历 13 处资源上限检测偏移,逐次写入对应字节
            for (int i = 0; i < OFFSET_UNLOCKRESOURCELIMIT.Length; i++)
            {
                IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_UNLOCKRESOURCELIMIT[i]);
                byte[] bufferToWrite = enable
                    ? BYTES_UNLOCKRESOURCELIMIT_PATCHED[i]
                    : BYTES_UNLOCKRESOURCELIMIT_ORIGINAL[i];
                WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
            }
        }

        /// <summary>
        /// 作弊功能8：增加金钱
        /// <param name="amount">(整数型 欲增加的金额)</param>
        /// </summary>
        public void AddMoney(int amount)
        {
            // 安全判定:如果没有连接成功,直接退出
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 计算绝对地址
            IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + RVA_MONEY);

            // 读取当前金钱
            byte[] buffer = new byte[4];
            ReadProcessMemory(ProcessHandle, targetAddress, buffer, 4, out _);

            // 将读取到的4字节小端序转换成整数
            int currentMoney = BitConverter.ToInt32(buffer, 0);

            // 计算新金钱
            int newMoney = currentMoney + amount;
            if (newMoney < 0) newMoney = 0; // 防止变负数

            // 将新数值转换回4个字节,写回内存
            byte[] writeBuffer = BitConverter.GetBytes(newMoney);

            // 写回内存
            WriteProcessMemory(ProcessHandle, targetAddress, writeBuffer, 4, out _);
        }

        /// <summary>
        /// 作弊功能9：增加装备类道具碎片
        /// <param name="amount">(整数型 欲增加的碎片数量)</param>
        /// </summary>
        public void AddFragment(int amount)
        {
            // 安全判定:如果没有连接成功,直接退出
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 计算绝对地址
            IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + RVA_FRAGMENT);

            // 读取当前碎片数
            byte[] buffer = new byte[4];
            ReadProcessMemory(ProcessHandle, targetAddress, buffer, 4, out _);

            // 将读取到的4字节小端序转换成整数
            int currentFragment = BitConverter.ToInt32(buffer, 0);

            // 计算新碎片数
            int newFragment = currentFragment + amount;
            if (newFragment < 0) newFragment = 0; // 防止变负数

            // 将新数值转换回4个字节,写回内存
            byte[] writeBuffer = BitConverter.GetBytes(newFragment);

            // 写回内存
            WriteProcessMemory(ProcessHandle, targetAddress, writeBuffer, 4, out _);
        }

        /// <summary>
        /// &lt;逻辑型&gt;作弊功能10：增加消耗品道具数量
        /// <param name="inc">(整数型 欲增加的消耗品道具数量)</param>
        /// <returns>增加成功返回真</returns>
        /// </summary>
        public bool AddItems(int inc)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return false;

            for (int i = 0; i < OFFSET_ITEM_CONSUMABLE.Length; i++)
            {
                // 地址 = .text基址 + 道具基址 + 消耗品偏移
                IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + RVA_ITEM_BASE + OFFSET_ITEM_CONSUMABLE[i]);

                // 读取当前数量
                byte[] buffer = new byte[4];
                if (!ReadProcessMemory(ProcessHandle, targetAddress, buffer, 4, out _)) return false;

                // 加上增量
                int current = BitConverter.ToInt32(buffer, 0);
                int newValue = inc;
                if (newValue < 0) newValue = 0;

                // 写回
                byte[] writeBuffer = BitConverter.GetBytes(newValue);
                if (!WriteProcessMemory(ProcessHandle, targetAddress, writeBuffer, 4, out _)) return false;
            }

            return true;
        }

        /// <summary>
        /// &lt;逻辑型&gt;取防具持有状态
        /// <param name="equipStatus">(整数型 欲获取的防具持有状态偏移)</param>
        /// <returns><para>已持有返回真，未持有或获取失败返回假</para></returns>
        /// </summary>
        public bool GetEquipStatus(int equipOffset)
        {
            // 安全判定:未激活则抛异常
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero)
                throw new InvalidOperationException("修改器未激活,无法读取道具持有状态。");

            // 地址 = .text基址 + 道具基址 + 道具偏移
            IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + RVA_ITEM_BASE + equipOffset);

            // 读取单字节
            byte[] buffer = new byte[1];
            if (!ReadProcessMemory(ProcessHandle, targetAddress, buffer, 1, out _))
                throw new InvalidOperationException($"读取道具持有状态失败,地址: 0x{targetAddress.ToInt32():X8}");

            byte rawValue = buffer[0];

            // 非零即视为已持有(0x01~0xFF → true),0x00 → false
            if (rawValue >= 1) return true;
            if (rawValue == 0) return false;

            // byte 只有 0~255,上面已穷举,此行理论不可达
            throw new InvalidOperationException($"非法的道具持有状态值: 0x{rawValue:X2}");
        }

        /// <summary>
        /// &lt;整数型&gt;取装备类道具等级
        /// <param name="equipLevel">(整数型 欲获取的防具等级偏移)</param>
        /// <returns><para>返回防具等级</para></returns>
        /// </summary>
        public int GetEquipLevel(int equipLevelOffset)
        {
            // 安全判定:未激活则抛异常
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero)
                throw new InvalidOperationException("修改器未激活,无法读取装备等级。");

            // 地址 = .text基址 + 装备等级基址 + 等级偏移
            IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + RVA_EQUIP_BASE + equipLevelOffset);

            // 读取4字节
            byte[] buffer = new byte[4];
            if (!ReadProcessMemory(ProcessHandle, targetAddress, buffer, 4, out _))
                throw new InvalidOperationException($"读取装备等级失败,地址: 0x{targetAddress.ToInt32():X8}");

            int levelValue = BitConverter.ToInt32(buffer, 0);

            // 数据值允许 0x00 ~ 0x05,非法值抛异常
            if (levelValue < 0)
                throw new InvalidOperationException($"非法的装备等级数据: {levelValue}");

            return levelValue;
        }

        /// <summary>
        /// &lt;逻辑型&gt;写入装备类道具持有状态
        /// <param name="equipOffset">(整数型 欲获取的防具持有状态偏移, </param>
        /// <param name="equipStatus">整数型 欲写入的防具持有状态)</param>
        /// <para>0=未持有,1=已持有。不能写入非0和非1的其他数值</para>
        /// <returns>成功返回真</returns>
        /// </summary>
        public bool WritetEquipStatus(int equipOffset, int equipStatus)
        {
            // 安全判定:未激活则抛异常
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero)
                throw new InvalidOperationException("修改器未激活,无法写入道具持有状态。");

            // equipStatus 只能是 0 或 1
            if (equipStatus != 0 && equipStatus != 1)
                throw new ArgumentException($"非法的道具持有状态参数: {equipStatus},仅接受 0(未持有)或 1(已持有)", nameof(equipStatus));

            // 地址 = .text基址 + 道具基址 + 道具偏移
            IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + RVA_ITEM_BASE + equipOffset);

            // 写入单字节(0=未持有,1=已持有)
            byte[] bufferToWrite = new byte[] { (byte)equipStatus };
            if (!WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, 1, out _))
                throw new InvalidOperationException($"写入道具持有状态失败,地址: 0x{targetAddress.ToInt32():X8}");

            return true;
        }

        /// <summary>
        /// &lt;逻辑型&gt;作弊功能11：写入装备类道具等级
        /// <param name="equippLevelOffset">(整数型 欲获取的防具等级偏移, </param>
        /// <param name="equippLevel">整数型 欲写入的防具等级)</param>
        /// <para>仅允许写入1-5</para>
        /// <returns>成功返回真</returns>
        /// </summary>
        public bool WritetEquipLevel(int equippLevelOffset, int equippLevel)
        {
            // 安全判定:未激活则抛异常
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero)
                throw new InvalidOperationException("修改器未激活,无法写入装备等级。");

            // equippLevel 允许 1 ~ 5(给人看的),内部减一映射为 0x00 ~ 0x04(给机器看的)
            if (equippLevel < 1 || equippLevel > 6)
                throw new ArgumentException($"非法的装备等级参数: {equippLevel},仅接受 1 ~ 5", nameof(equippLevel));

            int internalLevel = equippLevel - 1; // 1→0x00, 2→0x01, ..., 5→0x04

            // 地址 = .text基址 + 装备等级基址 + 等级偏移
            IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + RVA_EQUIP_BASE + equippLevelOffset);

            // 写入4字节
            byte[] bufferToWrite = BitConverter.GetBytes(internalLevel);
            if (!WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, 4, out _))
                throw new InvalidOperationException($"写入装备等级失败,地址: 0x{targetAddress.ToInt32():X8}");

            return true;
        }
        #endregion

        #region 钻头装备修改

        /// <summary>
        /// 作弊功能12：20%强化油
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void EnhanceOilPlus(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 核心寻址:.text 基址 + 双倍强化油偏移
            IntPtr targetAddress1 = IntPtr.Add(TextSectionBase, OFFSET_ENHANCEOILPLUS1);
            IntPtr targetAddress2 = IntPtr.Add(TextSectionBase, OFFSET_ENHANCEOILPLUS2);

            // 选字节:开启时写入 patched,关闭时还原 original
            byte[] bufferToWrite = enable ? BYTES_ENHANCEOILPLUS_PATCHED : BYTES_ENHANCEOILPLUS_ORIGINAL;

            // 写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress1, bufferToWrite, bufferToWrite.Length, out _);
            WriteProcessMemory(ProcessHandle, targetAddress2, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// &lt;逻辑型&gt;作弊功能13：激活四级钻头
        /// <returns><para>写入成功返回真，失败或修改器未激活返回假</para></returns>
        /// </summary>
        public bool ActivateFourDrill()
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return false;

            // 写入四级钻头激活标志
            IntPtr addrFourDrill = IntPtr.Add(TextSectionBase, OFFSET_FOURDRILL);
            byte[] bufFourDrill = BitConverter.GetBytes(1);
            bool ok1 = WriteProcessMemory(ProcessHandle, addrFourDrill, bufFourDrill, 4, out _);

            // 写入钻头等级为3
            IntPtr setDrillLevel = IntPtr.Add(TextSectionBase, OFFSET_DRILLLEVEL);
            byte[] bufDrillLevel = BitConverter.GetBytes(3);
            bool ok2 = WriteProcessMemory(ProcessHandle, setDrillLevel, bufDrillLevel, 4, out _);

            // 写入钻头能量值 400
            IntPtr addrEnergy = IntPtr.Add(TextSectionBase, OFFSET_DRILLENERGY);
            byte[] bufEnergy = BitConverter.GetBytes(400);
            bool ok3 = WriteProcessMemory(ProcessHandle, addrEnergy, bufEnergy, 4, out _);

            return ok1 && ok2 && ok3;
        }

        /// <summary>
        /// &lt;逻辑型&gt;作弊功能14：加满钻头能量
        /// <para>四级钻头=400,否则=300</para>
        /// <returns>写入成功返回真，失败或修改器未激活返回假</returns>
        /// </summary>
        public bool AddDrillEnergy()
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return false;

            // 读取钻头等级
            IntPtr addrLevel = IntPtr.Add(TextSectionBase, OFFSET_DRILLLEVEL);
            byte[] bufLevel = new byte[4];
            if (!ReadProcessMemory(ProcessHandle, addrLevel, bufLevel, 4, out _)) return false;
            int drillLevel = BitConverter.ToInt32(bufLevel, 0);

            // 读取四级钻头开启状态
            IntPtr addrFourDrill = IntPtr.Add(TextSectionBase, OFFSET_FOURDRILL);
            byte[] bufFourDrill = new byte[4];
            if (!ReadProcessMemory(ProcessHandle, addrFourDrill, bufFourDrill, 4, out _)) return false;
            int fourDrill = BitConverter.ToInt32(bufFourDrill, 0);

            // 钻头等级=3 且四级钻头已激活 → 400,否则 → 300
            int energy = (drillLevel <= 3 && fourDrill == 1) ? 400 : 300;

            // 写入钻头能量
            IntPtr addrEnergy = IntPtr.Add(TextSectionBase, OFFSET_DRILLENERGY);
            byte[] bufEnergy = BitConverter.GetBytes(energy);
            return WriteProcessMemory(ProcessHandle, addrEnergy, bufEnergy, 4, out _);
        }
        #endregion

        #region 战斗属性修改

        /// <summary>
        /// 作弊功能15：敌人、宝壶、宝箱全清
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void AllMaster(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            for (int i = 0; i < RVA_ALLMASTER_OFFSET.Length; i++)
            {
                // 目标地址:基址 + 全清偏移
                IntPtr targetAddress = IntPtr.Add(TextSectionBase, RVA_ALLMASTER_OFFSET[i]);

                IntPtr sourceAddress;
                if (enable)
                {
                    // 开启:计算基址+补丁偏移,转四字节写入
                    sourceAddress = IntPtr.Add(TextSectionBase, RVA_ALLMASTER_PATCHED_OFFSET[i]);
                }
                else
                {
                    // 还原:计算基址+原始偏移,转四字节写入
                    sourceAddress = IntPtr.Add(TextSectionBase, RVA_ALLMASTER_ORIGINAL_OFFSET);
                }

                // 将算出的地址值用 BitConverter 转四字节,写入目标
                byte[] bufferToWrite = BitConverter.GetBytes(sourceAddress.ToInt32());
                WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, 4, out _);
            }
        }

        /// <summary>
        /// 作弊功能16：结算时不累计GameOver
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void NoGameOver(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 目标地址:基址 + GameOver偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, RVA_NOGAMEOVER_OFFSET);

            // 说明：原语句mov eax,[游戏基址 + F33DA8]和mov eax,0x00000000的mov eax指令
            // 完全不同，一个是"A1 A8 3D A4 01"一个是"B8 00 00 00 00"，所以不能直接
            // 用BYTES_NOGAMEOVER_PATCHED覆盖原指令，需要进行数值拼接。
            byte[] bufferToWrite = new byte[5];
            if (enable)
            {
                // 开启：拼接 B8 + {0,0,0,0} → mov eax, 0
                bufferToWrite[0] = 0xB8;
                Array.Copy(BYTES_NOGAMEOVER_PATCHED, 0, bufferToWrite, 1, BYTES_NOGAMEOVER_PATCHED.Length);
            }
            else
            {
                // 还原：拼接 A1 + 原始地址(4字节) → mov eax, [原地址]
                IntPtr sourceAddress = IntPtr.Add(TextSectionBase, RVA_NOGAMEOVER_ORIGINAL_OFFSET);
                byte[] addrBytes = BitConverter.GetBytes(sourceAddress.ToInt32());
                bufferToWrite[0] = 0xA1;
                Array.Copy(addrBytes, 0, bufferToWrite, 1, addrBytes.Length);
            }

            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// 作弊功能17：耐久油时间不减
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void UnLimitDurableOil(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 核心寻址:.text 基址 + 耐久油时间不减偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_UNLIMITDURABLEOIL);

            // 选字节:开启时写入 patched,关闭时还原 original
            byte[] bufferToWrite = enable ? BYTES_UNLIMITDURABLEOIL_PATCHED : BYTES_UNLIMITDURABLEOIL_ORIGINAL;

            // 写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// &lt;逻辑型数组&gt;取所有必杀技持有情况
        /// <returns><para>返回各必杀技持有情况，已持有为真，未持有为假</para></returns>
        /// </summary>
        public bool[] GetUltimateStatus()
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero)
                return new bool[OFFSET_ULTIMATE.Length];

            bool[] result = new bool[OFFSET_ULTIMATE.Length];
            for (int i = 0; i < OFFSET_ULTIMATE.Length; i++)
            {
                // 目标地址 = .text基址 + 必杀技基址 + 必杀技子偏移
                IntPtr targetAddress = IntPtr.Add(TextSectionBase, RVA_ULTIMATE + OFFSET_ULTIMATE[i]);
                byte[] buffer = new byte[4];
                if (ReadProcessMemory(ProcessHandle, targetAddress, buffer, 4, out _))
                {
                    int value = BitConverter.ToInt32(buffer, 0);
                    result[i] = (value >= 1); // 0x00=未持有, >=0x01=已持有
                }
                else
                {
                    result[i] = false;
                }
            }
            return result;
        }

        /// <summary>
        /// &lt;逻辑型&gt;作弊功能18：置必杀技持有情况
        /// <param name="ultIndex">(整数型 必杀技索引ID，</param>
        /// <param name="ishad">逻辑型 是否持有)</param>
        /// <returns><para>成功返回真</para></returns>
        /// </summary>
        public bool SetUltimateStatus(int ultIndex, bool ishad)
        {
            // 安全判定 + 索引越界检查
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return false;
            if (ultIndex < 0 || ultIndex >= OFFSET_ULTIMATE.Length) return false;

            // 按索引号映射偏移，计算目标地址 = .text基址 + 必杀技基址 + 必杀技子偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, RVA_ULTIMATE + OFFSET_ULTIMATE[ultIndex]);

            // 布尔值转字节数组：true→1, false→0
            byte[] bufferToWrite = BitConverter.GetBytes(ishad ? 1 : 0);
            return WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, 4, out _);
        }
        #endregion

        #region 小游戏修改

        /// <summary>
        /// 作弊功能19：敌方碎石头我方得分
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void RockCrush(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 目标地址 = .text基址 + 偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_ROCKCRUSH);

            // 开启=0x00（秒碎岩石），还原=0x2D（原始）
            byte value = enable ? BYTES_ROCKCRUSH_PATCHED : BYTES_ROCKCRUSH_ORIGINAL;
            WriteProcessMemory(ProcessHandle, targetAddress, new byte[] { value }, 1, out _);
        }

        /// <summary>
        /// 作弊功能20：无限打地鼠
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void MoleCombo(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 核心寻址:.text 基址 + 无限打地鼠偏移
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_MOLECOMBO);

            // 选字节:开启时写入 patched,关闭时还原 original
            byte[] bufferToWrite = enable ? BYTES_MOLECOMBO_PATCHED : BYTES_MOLECOMBO_ORIGINAL;

            // 写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// 作弊功能21：无限躲激光
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void LaserDodg(bool enable)
        {
            // 安全判定:如果没有连接成功,直接退出
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 计算绝对地址
            IntPtr targetAddress = (IntPtr)(TextSectionBase.ToInt32() + OFFSET_LASERDODG);

            // 选字节:开启时写入 patched,关闭时还原 original
            byte[] bufferToWrite = enable ? BYTES_LASERDODG_PATCHED : BYTES_LASERDODG_ORIGINAL;

            // 写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// 作弊功能22：敌方进球我方白送分
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void Football(bool enable)
        {
            // 安全判定:如果没有连接成功,直接退出
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 计算绝对地址
            IntPtr targetAddress = IntPtr.Add(TextSectionBase, OFFSET_FOOTBALL);
            int baseAddr = TextSectionBase.ToInt32();
            byte[] bufferToWrite;

            // 根据传入的布尔值,取消勾选则执行还原版指令
            if (!enable)
            {
                // Step1: 计算敌方进球积分绝对地址并转为小端序字节数组
                int rtEnemyBackend = baseAddr + RVA_ENEMY_SCORE;
                byte[] addressBytes = BitConverter.GetBytes(rtEnemyBackend);

                // Step2: 创建一个长度为5的字节数组
                bufferToWrite = new byte[5];

                // Step3: 将 0xA1 (mov eax, [...]) 放入第1个位置
                bufferToWrite[0] = BYTES_FOOTBALL_ORIGINAL;

                // Step4: 将计算出的4字节小端序地址追加到后面
                Buffer.BlockCopy(addressBytes, 0, bufferToWrite, 1, 4);
            }
            else
            {
                // 执行敌方进球白送分补丁 (jmp 到送分逻辑)
                bufferToWrite = BYTES_FOOTBALL_PATCHED;
            }

            // 将字节码强行写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress, bufferToWrite, bufferToWrite.Length, out _);
        }

        /// <summary>
        /// 作弊功能23：足球赛/打地鼠时间暂停
        /// <param name="enable">(逻辑型 欲设置的开启状态)</param>
        /// </summary>
        public void TimePause(bool enable)
        {
            // 安全判定
            if (!IsActivated || ProcessHandle == IntPtr.Zero || TextSectionBase == IntPtr.Zero) return;

            // 核心寻址:.text 基址 + 打地鼠时间暂停偏移
            IntPtr targetAddress_MoleHit = IntPtr.Add(TextSectionBase, OFFSET_TIMEPAUSE_MOLEHIT);
            IntPtr targetAddress_Football = IntPtr.Add(TextSectionBase, OFFSET_TIMEPAUSE_FOOTBALL);

            // 选字节:开启时写入 patched,关闭时还原 original
            byte[] bufferToWrite = enable ? BYTES_TIMEPAUSE_PATCHED : BYTES_TIMEPAUSE_ORIGINAL;

            // 写入游戏内存
            WriteProcessMemory(ProcessHandle, targetAddress_MoleHit, bufferToWrite, bufferToWrite.Length, out _);
            WriteProcessMemory(ProcessHandle, targetAddress_Football, bufferToWrite, bufferToWrite.Length, out _);
        }
        #endregion

        #region 资源释放

        /// <summary>
        /// 关闭进程句柄,重置状态
        /// </summary>
        public void Cleanup()
        {
            if (ProcessHandle != IntPtr.Zero)
            {
                // 先还原快速移动注入点再释放内存块,否则游戏里会留下一条指向已释放内存的 jmp,直接崩
                if (OFFSET_FASTMOVE_PATCHED != 0)
                {
                    try
                    {
                        FastMove(0);
                        VirtualFreeEx(ProcessHandle, (IntPtr)OFFSET_FASTMOVE_PATCHED, 0, MEM_RELEASE);
                    }
                    catch { /* 游戏可能已经退出,忽略即可 */ }
                    OFFSET_FASTMOVE_PATCHED = 0;
                }

                CloseHandle(ProcessHandle);
                ProcessHandle = IntPtr.Zero;
            }
            TextSectionBase = IntPtr.Zero;
            GameProcess = null;
            IsActivated = false;
            LastMessage = string.Empty;
        }
        #endregion

    }
}

