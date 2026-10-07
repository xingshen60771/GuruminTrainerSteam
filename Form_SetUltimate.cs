using GuruminTrainerSteam.DynamicHelper;
using System.Windows.Forms;

namespace GuruminTrainerSteam
{
    public partial class Form_SetUltimate : Form
    {
        private TrainerCore trainerCore; // 修改器核心实例

        /// <summary>
        /// 实例化当前窗体
        /// </summary>
        /// <param name="core"></param>
        public Form_SetUltimate(TrainerCore core)
        {
            InitializeComponent();
            trainerCore = core;

            // 事件绑定
            Load += (s, e) => LaodSetUltimateStatus();

            // 关闭按钮点击事件绑定
            btn_Close.Click += (s, e) => this.Close();

            // 按复选框状态设置必杀技持有情况
            checkBox_Ultimate01.Click += (s, e) => trainerCore.SetUltimateStatus(0, checkBox_Ultimate01.Checked);
            checkBox_Ultimate02.Click += (s, e) => trainerCore.SetUltimateStatus(1, checkBox_Ultimate02.Checked);
            checkBox_Ultimate03.Click += (s, e) => trainerCore.SetUltimateStatus(2, checkBox_Ultimate03.Checked);
            checkBox_Ultimate04.Click += (s, e) => trainerCore.SetUltimateStatus(3, checkBox_Ultimate04.Checked);
            checkBox_Ultimate05.Click += (s, e) => trainerCore.SetUltimateStatus(4, checkBox_Ultimate05.Checked);
            checkBox_Ultimate06.Click += (s, e) => trainerCore.SetUltimateStatus(5, checkBox_Ultimate06.Checked);
        }

        /// <summary>
        /// 载入必杀技状态
        /// </summary>
        private void LaodSetUltimateStatus()
        {
            // 按得到的逻辑型数组来展示各必杀技的持有状态
            bool[] ultimateStatus = trainerCore.GetUltimateStatus();
            checkBox_Ultimate01.Checked = ultimateStatus[0];
            checkBox_Ultimate02.Checked = ultimateStatus[1];
            checkBox_Ultimate03.Checked = ultimateStatus[2];
            checkBox_Ultimate04.Checked = ultimateStatus[3];
            checkBox_Ultimate05.Checked = ultimateStatus[4];
            checkBox_Ultimate06.Checked = ultimateStatus[5];
        }

        /// <summary>
        /// 全部拥有复选框状态改变事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_haveAll_CheckedChanged(object sender, System.EventArgs e)
        {
            bool haveAll = checkBox_haveAll.Checked;
            checkBox_Ultimate01.Enabled = !haveAll;
            checkBox_Ultimate02.Enabled = !haveAll;
            checkBox_Ultimate03.Enabled = !haveAll;
            checkBox_Ultimate04.Enabled = !haveAll;
            checkBox_Ultimate05.Enabled = !haveAll;
            checkBox_Ultimate06.Enabled = !haveAll;

            if (haveAll)
            {
                // 如果选中“拥有全部”，则设置所有终极装备为已拥有
                trainerCore.SetUltimateStatus(0, true);
                trainerCore.SetUltimateStatus(1, true);
                trainerCore.SetUltimateStatus(2, true);
                trainerCore.SetUltimateStatus(3, true);
                trainerCore.SetUltimateStatus(4, true);
                trainerCore.SetUltimateStatus(5, true);

                // 更新复选框状态
                LaodSetUltimateStatus();
            }
        }
    }
}
