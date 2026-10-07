using GuruminTrainerSteam.DynamicHelper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GuruminTrainerSteam
{
    public partial class MainForm : Form
    {
        // 修改器激活器实例
        private TrainerCore trainerCore = new TrainerCore();

        // 全局热键核心
        private HotKeyCore hotKeyCore;

        // 用于周期扫描游戏进程的时钟
        private Timer timer_Scanner;

        /// <summary>
        /// 实例化当前窗体
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // 设置窗体标题
            this.Text = $"咕噜小天使 Steam 23项属性内存修改器(仅适配V1.4版本)";
            // 设置标题图
            pictureBox.Image = global::GuruminTrainerSteam.Properties.Resources.img_Title;
            pictureBox.Width = (int)(global::GuruminTrainerSteam.Properties.Resources.img_Title.Width);
            pictureBox.Height = (int)(global::GuruminTrainerSteam.Properties.Resources.img_Title.Height);
            // 设置窗体宽度
            this.Width = pictureBox.Width + 5 + tabControl.Width + 10;
            this.Height = tabControl.Height + 50;
            // 设置选项卡位置
            tabControl.Left = pictureBox.Width + 5;
            // 设置修改器激活状态位置
            label_Status.Top = this.Height - label_Status.Height * 2 - 25;

            // 说明文本框处理
            textBox_Description.ReadOnly = true;
            textBox_Description.BackColor = Color.FromArgb(199, 237, 204);
            textBox_Description.Width = tabControl.Width - 5;
            textBox_Description.Height = tabControl.Height - 30;
            textBox_Description.Text = Properties.Resources.Description;

            // 关于文本
            label_About01.Text = PublicFunction.GetAPPInformation(1) + "\n\n\n版本:" + PublicFunction.GetAPPInformation(2) + "\nby:" + PublicFunction.GetAPPInformation(3);

            // 创建扫描时钟（1 秒间隔）
            timer_Scanner = new Timer();
            timer_Scanner.Interval = 500;
            timer_Scanner.Tick += timer_Scanner_Tick;

            // 窗体内存加载时启动扫描
            this.Load += Form_Dynamic_Load;

            // 初始化全局热键
            hotKeyCore = new HotKeyCore(this);
            RegisterAllHotKeys();

        }

        /// <summary>
        /// 载入快速移动倍率下拉框
        /// </summary>
        private void LoadFastMove()
        {
            // 列出快速移动文本
            string[] fastMoveOptions = { "不加速", "1.5x", "1.6x", "1.7x", "1.8x", "1.9x", "2.0x" };
            comboBox_FastMove.Items.AddRange(fastMoveOptions);
            comboBox_FastMove.SelectedIndex = 0;
            comboBox_FastMove.Enabled = true;
        }

        /// <summary>
        /// 载入难度到下拉框
        /// </summary>
        private void LoadDifficulty()
        {
            // 列出难度文本
            string[] difficulties = { "初级(Beginner)", "普通(Normal)", "困难(Hard)", "娱乐(Pleasure)", "疯狂(Crazy)" };
            // 载入到下拉框
            comboBox_ChangeDifficulty.Items.AddRange(difficulties);
            comboBox_ChangeDifficulty.SelectedIndex = trainerCore.GetDifficulty();
        }

        #region 窗体与时钟事件

        /// <summary>
        /// 窗体加载后的响应函数：初始化状态标签并启动扫描时钟
        /// </summary>
        private void Form_Dynamic_Load(object sender, EventArgs e)
        {
            // 设置初始状态：红色加粗
            label_Status.Text = "● 未找到游戏进程，等待中...";
            label_Status.ForeColor = Color.Red;
            label_Status.Font = new Font(label_Status.Font, FontStyle.Bold);

            // 四个功能分组框禁用
            groupBox_Character.Enabled = false;
            groupBox_Item.Enabled = false;
            groupBox_Drill.Enabled = false;
            groupBox_Battle.Enabled = false;
            groupBox_MiniGame.Enabled = false;

            // 开始周期扫描
            timer_Scanner.Start();
        }

        /// <summary>
        /// 时钟周期事件：每秒扫描一次游戏进程，尝试激活修改器
        /// </summary>
        private void timer_Scanner_Tick(object sender, EventArgs e)
        {
            // 已经激活则停止扫描
            if (trainerCore.IsActivated)
            {
                timer_Scanner.Stop();
                return;
            }

            // 尝试激活
            bool success = trainerCore.Activate();

            if (success)
            {
                // 激活成功：停止扫描，更新标签为绿色加粗
                timer_Scanner.Stop();
                label_Status.Text = "● 修改器已激活";
                label_Status.ForeColor = Color.Green;
                label_Status.Font = new Font(label_Status.Font, FontStyle.Bold);

                // 启动全局热键钩子
                hotKeyCore.Start();

                // 激活成功后，启用四个功能分组框
                groupBox_Character.Enabled = true;
                groupBox_Item.Enabled = true;
                groupBox_Drill.Enabled = true;
                groupBox_Battle.Enabled = true;
                groupBox_MiniGame.Enabled = true;

                // 播放提示音
                System.Media.SoundPlayer playerAsync = new System.Media.SoundPlayer(Properties.Resources.Sound1);
                playerAsync.Play();
                // 载入快速移动、难度到下拉框
                try
                {
                    LoadFastMove();
                    LoadDifficulty();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // 未激活：保持红色加粗，更新详情
                label_Status.Text = "● 未找到游戏进程，等待中...";
                label_Status.ForeColor = Color.Red;
                label_Status.Font = new Font(label_Status.Font, FontStyle.Bold);
                // 四个功能分组框禁用
                groupBox_Character.Enabled = false;
                groupBox_Item.Enabled = false;
                groupBox_Drill.Enabled = false;
                groupBox_Battle.Enabled = false;
                groupBox_MiniGame.Enabled = false;
            }
        }

        /// <summary>
        /// 窗体关闭后的响应函数：停止时钟并释放修改器资源
        /// </summary>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 停止扫描时钟
            if (timer_Scanner != null)
            {
                timer_Scanner.Stop();
                timer_Scanner.Dispose();
            }

            // 释放激活器资源（关闭进程句柄等）
            trainerCore.Cleanup();

            // 释放热键钩子
            if (hotKeyCore != null)
            {
                hotKeyCore.Dispose();
                hotKeyCore = null;
            }

            // 从打开窗体集合中找到 MainForm 并显示
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm)
                {
                    form.Show();
                    break;
                }
            }
        }

        #endregion

        #region 控件事件绑定

        #region 基本属性修改

        /// <summary>
        /// 主角无敌复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_God_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_God.Checked;

            // 直接调用核心类的无敌方法
            trainerCore.God(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 无限跳跃复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_FreeJump_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_FreeJump.Checked;

            // 直接调用核心类的无限跳跃方法
            trainerCore.FreeJump(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 一击必杀复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_SuperKill_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_SuperKill.Checked;

            // 直接调用核心类的一击必杀方法
            trainerCore.SuperKill(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 快速移动复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_FastMove_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_FastMove.Checked;

            // 直接调用核心类的快速移动方法
            trainerCore.FastMove(isEnabled ? 4 : 0);
            comboBox_FastMove.Enabled = isEnabled;
            comboBox_FastMove.SelectedIndex = isEnabled ? 4 : 0; // 如果启用快速移动，默认选择2.0x，否则选择不加速
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 快速移动倍率下拉框索引改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_FastMove_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 下拉框索引是几trainerCore.FastMove就是几，如果是0，灰掉comboBox_FastMove
            trainerCore.FastMove(comboBox_FastMove.SelectedIndex);
            comboBox_FastMove.Enabled = comboBox_FastMove.SelectedIndex != 0; // 如果选择了不加速，禁用快速移动复选框
            checkBox_FastMove.Checked = comboBox_FastMove.SelectedIndex != 0;
            PlaySound(true);
        }

        /// <summary>
        /// 清零游戏时间按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_TimeClear_Click(object sender, EventArgs e)
        {
            // 清零游戏时间
            bool clear = trainerCore.TimeClear();
            PlaySound(true);
            if (clear)
            {
                MessageBox.Show("游戏时间已清零，现在将从0秒开始计时！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("发生未知错误，清零失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 修改一下难度提交按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ChangeDifficulty_Click(object sender, EventArgs e)
        {
            int targetDifficulty = comboBox_ChangeDifficulty.SelectedIndex; // 获取下拉框选中的难度索引
            bool change = trainerCore.ChangeDifficulty(targetDifficulty);
            PlaySound(true);
            if (change)
            {
                MessageBox.Show($"已将游戏难度修改为：『{comboBox_ChangeDifficulty.SelectedItem.ToString()}』\n保存并重新载入游戏生效。", "难度已修改", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"修改难度失败，请确保游戏处于可更改状态。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region 资源装备修改

        /// <summary>
        /// 突破资源上限复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_UnlockResourceLimit_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_UnlockResourceLimit.Checked;

            //  直接调用核心类的解锁资源上限方法
            trainerCore.UnlockResourceLimit(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 加金钱按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddMoney_Click(object sender, EventArgs e)
        {
            // 增加 1000 金钱
            trainerCore.AddMoney(1000);
            PlaySound(true);
        }

        /// <summary>
        /// 加碎片按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddFragment_Click(object sender, EventArgs e)
        {
            // 增加 100 碎片
            trainerCore.AddFragment(100);
            PlaySound(true);
        }

        /// <summary>
        /// 增加消耗品按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddItems_Click(object sender, EventArgs e)
        {
            // 确认修改器有没有选中突破资源上限
            int inc = checkBox_UnlockResourceLimit.Checked ? 0x7F : 0x3;

            // 直接调用核心类的增加物品方法
            trainerCore.AddItems(inc);
            PlaySound(true);
        }

        /// <summary>
        /// 装备修改按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_EquipEdit_Click(object sender, EventArgs e)
        {
            // 创建装备修改窗口
            Form_EquipEdit equipForm = new Form_EquipEdit(trainerCore);
            PlaySound(true);
            equipForm.ShowDialog();
        }

        #endregion

        #region 钻头装备修改

        /// <summary>
        /// 强化油增量20%复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_EnhanceOilPlus_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_EnhanceOilPlus.Checked;

            // 直接调用核心类的双倍强化油方法
            trainerCore.EnhanceOilPlus(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 激活四钻级钻头按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ActivateFourDrill_Click(object sender, EventArgs e)
        {
            // 直接调用核心类的激活四钻方法
            trainerCore.ActivateFourDrill();
            PlaySound(true);
        }

        /// <summary>
        /// 钻头能量加满按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddDrillEnergy_Click(object sender, EventArgs e)
        {
            // 钻头能量加满
            trainerCore.AddDrillEnergy();
            PlaySound(true);
        }

        #endregion

        #region 战斗属性修改

        /// <summary>
        /// 敌人数/宝壶/宝箱全清按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_AllMaster_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_AllMaster.Checked;

            // 直接调用核心类的全满技能方法
            trainerCore.AllMaster(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 结算不累计GameOver复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_NoGameOver_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_NoGameOver.Checked;

            // 直接调用核心类的无游戏结束方法
            trainerCore.NoGameOver(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 耐久油不减复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_UnLimitDurableOil_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_UnLimitDurableOil.Checked;

            // 直接调用核心类的无限耐久油方法
            trainerCore.UnLimitDurableOil(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 必杀技修改按钮点击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SetUltimate_Click(object sender, EventArgs e)
        {
            // 创建必杀技修改窗口
            Form_SetUltimate equipForm = new Form_SetUltimate(trainerCore);
            PlaySound(true);
            equipForm.ShowDialog();
        }

        #endregion

        #region 小游戏修改

        /// <summary>
        /// 敌方碎石我方得分复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_RockCrush_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_RockCrush.Checked;

            // 直接调用核心类的岩石粉碎方法
            trainerCore.RockCrush(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 无限打地鼠复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_MoleCombo_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_MoleCombo.Checked;

            // 直接调用核心类的地鼠连击方法
            trainerCore.MoleCombo(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 无限躲激光复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_LaserDodg_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_LaserDodg.Checked;

            // 直接调用核心类的激光闪避方法
            trainerCore.LaserDodg(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 足球塞对方进球我方得分复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Football_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_Football.Checked;

            // 直接调用核心类的足球模式方法
            trainerCore.Football(isEnabled);
            PlaySound(isEnabled);
        }

        /// <summary>
        /// 打地鼠/足球赛时间暂停复选框状态改变响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_TimePause_CheckedChanged(object sender, EventArgs e)
        {
            // 获取当前复选框的状态
            bool isEnabled = checkBox_TimePause.Checked;
            // 直接调用核心类的时间暂停方法
            trainerCore.TimePause(isEnabled);
            PlaySound(isEnabled);
        }

        #endregion

        #region 关于页链接

        private void label_URL01_Click(object sender, EventArgs e)
        {
            // 打开指定网址
            Process.Start(
                new ProcessStartInfo(label_URL01.Text)
                {
                    UseShellExecute = true
                }
                );
        }

        private void label_URL02_Click(object sender, EventArgs e)
        {
            // 打开指定网址
            Process.Start(
                new ProcessStartInfo(label_URL02.Text)
                {
                    UseShellExecute = true
                }
                );
        }

        #endregion

        #endregion

        #region 全局热键注册

        /// <summary>
        /// 注册所有 SHIFT+Fx 热键，映射到修改器功能
        /// </summary>
        private void RegisterAllHotKeys()
        {
            var S = HotKeyCore.HotKeyModifier.Shift;
            var A = HotKeyCore.HotKeyModifier.Alt;
            var C = HotKeyCore.HotKeyModifier.Ctrl;

            // ── 角色修改 (Shift+F1~F3) ──
            hotKeyCore.RegisterHotKey(Keys.F1, S, "主角无敌", _ => ToggleCheckBox(checkBox_God));
            hotKeyCore.RegisterHotKey(Keys.F2, S, "无限跳跃", _ => ToggleCheckBox(checkBox_FreeJump));
            hotKeyCore.RegisterHotKey(Keys.F3, S, "一击必杀", _ => ToggleCheckBox(checkBox_SuperKill));
            hotKeyCore.RegisterHotKey(Keys.F4, S, "快速移动", _ => ToggleCheckBox(checkBox_FastMove));


            // ── 战斗修改 (Shift+F5, F10~F12) ──
            hotKeyCore.RegisterHotKey(Keys.F5, S, "清零游戏时间", _ => btn_TimeClear.PerformClick());
            hotKeyCore.RegisterHotKey(Keys.F10, S, "全清敌人", _ => ToggleCheckBox(checkBox_AllMaster));
            hotKeyCore.RegisterHotKey(Keys.F11, S, "不增GameOver", _ => ToggleCheckBox(checkBox_NoGameOver));
            hotKeyCore.RegisterHotKey(Keys.F12, S, "耐久油不减", _ => ToggleCheckBox(checkBox_UnLimitDurableOil));

            // ── 道具修改 (Shift+F6~F9) ──
            hotKeyCore.RegisterHotKey(Keys.F6, S, "突破资源上限", _ => ToggleCheckBox(checkBox_UnlockResourceLimit));
            hotKeyCore.RegisterHotKey(Keys.F7, S, "加钱1000", _ => btn_AddMoney.PerformClick());
            hotKeyCore.RegisterHotKey(Keys.F8, S, "加碎片100", _ => btn_AddFragment.PerformClick());
            hotKeyCore.RegisterHotKey(Keys.F9, S, "消耗品加满", _ => btn_AddItems.PerformClick());

            // ── 小游戏修改 (Alt+F1~F5) ──
            hotKeyCore.RegisterHotKey(Keys.F1, A, "敌碎石得分", _ => ToggleCheckBox(checkBox_RockCrush));
            hotKeyCore.RegisterHotKey(Keys.F2, A, "打地鼠连击不中断", _ => ToggleCheckBox(checkBox_MoleCombo));
            hotKeyCore.RegisterHotKey(Keys.F3, A, "无限躲激光", _ => ToggleCheckBox(checkBox_LaserDodg));
            hotKeyCore.RegisterHotKey(Keys.F6, A, "敌进球得分", _ => ToggleCheckBox(checkBox_Football));
            hotKeyCore.RegisterHotKey(Keys.F5, A, "时间暂停", _ => ToggleCheckBox(checkBox_TimePause));

            // ── 钻头修改 (Shift+Num1, Shift+Num3) ──
            hotKeyCore.RegisterHotKey(Keys.F1, C, "强化油增量20%", _ => ToggleCheckBox(checkBox_EnhanceOilPlus));
            hotKeyCore.RegisterHotKey(Keys.F2, C, "激活四钻级钻头", _ => btn_ActivateFourDrill.PerformClick());
            hotKeyCore.RegisterHotKey(Keys.F3, C, "钻头能量加满", _ => btn_AddDrillEnergy.PerformClick());

#if DEBUG
            System.Diagnostics.Debug.WriteLine("[Form_Dynamic] 全部热键注册完成");
#endif
        }

        /// <summary>
        /// 切换 CheckBox 状态
        /// <param name="cb">(CheckBox 欲设置的状态)</param>
        /// </summary>
        private void ToggleCheckBox(CheckBox cb)
        {
            if (cb == null) return;
            cb.Checked = !cb.Checked;
        }

        #endregion

        #region 提示音      
        /// <summary>
        /// 播放操作提示音
        /// <param name="isChecked">(逻辑型 欲设置的状态)</param>
        /// <para>true=选中/操作音(Sound1)，false=取消音(Sound2)</para>
        /// </summary>
        private void PlaySound(bool isChecked)
        {
            var player = new System.Media.SoundPlayer(
                isChecked ? Properties.Resources.Sound1 : Properties.Resources.Sound2);
            player.Play();
        }

        #endregion

    }
}
