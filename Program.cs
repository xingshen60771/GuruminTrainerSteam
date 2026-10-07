using System;
using System.Windows.Forms;

namespace GuruminTrainerSteam
{
    internal static class Program
    {
        /*
            咕噜小天使 Steam 23项属性内存修改器

            52pojie@烟99

            转载务必保留此部分注释！本源码首发平台为GitHub，如果你是通
            过付费下载（充值会员、下载点等），表示你已经上当受骗！转载
            过的源码安全性无法保证，请尽快删除，并到本源码首发链接下载。

            本源码发布链接：https://github.com/xingshen60771/GuruminTrainerSteam

            【基本介绍】
            本修改器仅适配Steam版Ver 1.4版本，日文原版、YLT代理的汉化版均不适用！
            修改器启动后会自动扫描游戏进程，激活成功后可操作以下23项属性修改。
            ※ 一、基本属性修改 ※
            1) 主角无敌 [Shift+F1]
            2) 无限跳跃 [Shift+F2]
            3) 一击必杀 [Shift+F3]
            4) 快速移动 [Shift+F4]
            5) 清零总游戏时间 [Shift+F5]
            6) 修改游戏难度
            ※ 二、战斗属性修改 ※
            7) 敌人数/宝壶/宝箱全清 [Shift+F10]
            8) 迷宫结算不累计GameOver [Shift+F11]
            9) 耐久油时间不减 [Shift+F12]
            10) 修改必杀技
            ※ 三、资源装备修改 ※
            11) 突破资源上限 [Shift+F6]
            12) 给我1000块钱 [Shift+F7]
            13) 给我100碎片 [Shift+F8]
            14) 消耗类道具加满 [Shift+F9]
            15) 修改持有装备及等级
            ※ 四、钻头属性修改 ※
            16) 强化油、钻头挑土能量增量20% [Ctrl+F1]
            17) 激活四级钻头 [Ctrl+F2]
            18) 钻头能量加满 [Ctrl+F3]
            ※ 五、小游戏修改 ※
            19) 敌方碎石头我方得分 [Alt+F1]
            20) 打地鼠连击不中断 [Alt+F2]
            21) 无限躲激光 [Alt+F3]
            22) 敌方进球我方得分 [Alt+F6]
            23) 打地鼠/足球赛时间暂停 [Alt+F5]

            【更新日志】
            ;--------------------------------------------
            2026.10.07 -- v1.0.0
            ;--------------------------------------------
            1、修改器正式发布。

            【郑重声明】
            本源码（含成品）仅供技术学习与交流讨论使用！严禁任何非法用途！
*/

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 提示信息
            MessageBox.Show("本修改器为开源项目，完全免费使用！如果你是通过付费获得的修改器，那说明你已经上当受骗！", "郑重声明", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("建议不要在Steam正版游戏本体中使用本修改器，以免因检测到你的Steam账号出现不正常的游戏成就而被风控。\n\n账号异常概不负责！", "小心", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            // 打开主窗体
            Application.Run(new MainForm());
        }
    }
}
