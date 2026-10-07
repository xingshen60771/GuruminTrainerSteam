using GuruminTrainerSteam.DynamicHelper;
using System.Windows.Forms;

namespace GuruminTrainerSteam
{
    public partial class Form_EquipEdit : Form
    {
        // 装备名称数组
        string[] equipName = {
                                "潜水眼镜",
                                "毒气面罩",
                                "丝带",
                                "吸血帽子",
                                "猫咪",
                                "安全帽",
                                "扎头巾",
                                "魔法头巾",
                                "武士钢盔",
                                "霸者之面",
                                "王冠",
                                "奇异的帽子",
                                "睡帽",
                                "宠物衣裳",
                                "团子"
                            };

        // 现行装备索引
        int currentEquipIndex = -1;// 固定当前要修改的装备，以防选了这个装备后又选了别的装备

        // 修改器核心实例
        private TrainerCore trainerCore;

        /// <summary>
        /// 载入装备状态至列表
        /// </summary>
        private void LaodEquipStatus()
        {
            // 清空列表
            listView_Equip.Items.Clear();

            // 载入所有防具状态到listView_Equip控件
            for (int i = 0; i < equipName.Length; i++)
            {
                try
                {
                    // 获取装备持有状态和等级
                    int a = trainerCore.EQUIP_DATA[i, 0];
                    bool unlocked = trainerCore.GetEquipStatus(a);
                    int level = trainerCore.GetEquipLevel(trainerCore.EQUIP_DATA[i, 1]) + 1;

                    // 创建列表项：序号 | 防具名称 | 是否持有 | 防具等级
                    ListViewItem item = new ListViewItem((i + 1).ToString());
                    item.SubItems.Add(equipName[i]);
                    item.SubItems.Add(unlocked ? "是" : "否");
                    item.SubItems.Add(level.ToString());
                    listView_Equip.Items.Add(item);
                }
                catch
                {
                    // 读取失败时显示占位符
                    ListViewItem item = new ListViewItem((i + 1).ToString());
                    item.SubItems.Add(equipName[i]);
                    item.SubItems.Add("-");
                    item.SubItems.Add("-");
                    listView_Equip.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 载入选中装备状态
        /// </summary>
        private void LaodSingleEquipStatus()
        {
            // 获取listView_Equip当前选择项给currentEquipIndex
            if (listView_Equip.SelectedIndices.Count == 0) return;
            currentEquipIndex = listView_Equip.SelectedIndices[0];

            try
            {
                // 1、获取装备持有状态，映射到单选框
                bool unlocked = trainerCore.GetEquipStatus(trainerCore.EQUIP_DATA[currentEquipIndex, 0]);
                radio_Unlocked.Checked = unlocked;
                radio_Locked.Checked = !unlocked;

                // 2、获取装备等级，反馈到滑块（值 0-5，轨范围 1-5，0 时设为最小值 1）
                int level = trainerCore.GetEquipLevel(trainerCore.EQUIP_DATA[currentEquipIndex, 1]);
                if (level < trackBar_EquipLevel.Minimum)
                    trackBar_EquipLevel.Value = trackBar_EquipLevel.Minimum;
                else if (level > trackBar_EquipLevel.Maximum)
                    trackBar_EquipLevel.Value = trackBar_EquipLevel.Maximum;
                else
                    trackBar_EquipLevel.Value = level + 1;
                // 显示当前装备名称
                label_CurrentEquip.Text = $"当前装备：{equipName[currentEquipIndex]}";
                label_Vaule.Text = $"LV{trackBar_EquipLevel.Value}";
            }
            catch
            {
                // 读取失败时保持默认状态
                radio_Unlocked.Checked = false;
                radio_Locked.Checked = true;
                trackBar_EquipLevel.Value = trackBar_EquipLevel.Minimum;
            }
        }

        /// <summary>
        /// 保存装备状态
        /// </summary>
        private void SaveEquipStatus()
        {
            // 确保已选中装备
            if (currentEquipIndex < 0 || currentEquipIndex >= trainerCore.EQUIP_DATA.GetLength(0)) return;

            try
            {
                // 确定选中状态：radio_Unlocked 选中为 true（1），否则为 false（0）
                int equipStatus = radio_Unlocked.Checked ? 1 : 0;
                int equipOffset = trainerCore.EQUIP_DATA[currentEquipIndex, 0];
                trainerCore.WritetEquipStatus(equipOffset, equipStatus);

                // 写入装备等级：滑块值为给人看的等级(1-5)
                int equipLevel = trackBar_EquipLevel.Value;
                int equipLevelOffset = trainerCore.EQUIP_DATA[currentEquipIndex, 1];
                trainerCore.WritetEquipLevel(equipLevelOffset, equipLevel);

                MessageBox.Show("装备状态保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 实例化当前窗体
        /// </summary>
        /// <param name="core"></param>
        public Form_EquipEdit(TrainerCore core)
        {
            InitializeComponent();
            label_CurrentEquip.Text = "当前装备：未选择";
            trainerCore = core;
            Load += (s, e) => LaodEquipStatus();
            listView_Equip.SelectedIndexChanged += (s, e) => LaodSingleEquipStatus();
            btn_Save.Click += (s, e) => SaveEquipStatus();
        }

        /// <summary>
        /// 装备等级滑块滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_EquipLevel_Scroll(object sender, System.EventArgs e)
        {
            label_Vaule.Text = $"LV{trackBar_EquipLevel.Value}";
        }
    }
}
