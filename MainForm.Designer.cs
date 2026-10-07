namespace GuruminTrainerSteam
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.Trainer = new System.Windows.Forms.TabPage();
            this.groupBox_MiniGame = new System.Windows.Forms.GroupBox();
            this.label_HotKey_TimePause = new System.Windows.Forms.Label();
            this.label_HotKey_Football = new System.Windows.Forms.Label();
            this.label_HotKey_LaserDodg = new System.Windows.Forms.Label();
            this.label_HotKey_MoleCombo = new System.Windows.Forms.Label();
            this.label_HotKey_RockCrush = new System.Windows.Forms.Label();
            this.checkBox_RockCrush = new System.Windows.Forms.CheckBox();
            this.checkBox_Football = new System.Windows.Forms.CheckBox();
            this.checkBox_LaserDodg = new System.Windows.Forms.CheckBox();
            this.checkBox_MoleCombo = new System.Windows.Forms.CheckBox();
            this.checkBox_TimePause = new System.Windows.Forms.CheckBox();
            this.groupBox_Drill = new System.Windows.Forms.GroupBox();
            this.label_HotKey_AddDrillEnergy = new System.Windows.Forms.Label();
            this.ActivateFourDrill = new System.Windows.Forms.Label();
            this.label_HotKey_EnhanceOilPlus = new System.Windows.Forms.Label();
            this.btn_AddDrillEnergy = new System.Windows.Forms.Button();
            this.btn_ActivateFourDrill = new System.Windows.Forms.Button();
            this.checkBox_EnhanceOilPlus = new System.Windows.Forms.CheckBox();
            this.groupBox_Item = new System.Windows.Forms.GroupBox();
            this.label_HotKey_AddItems = new System.Windows.Forms.Label();
            this.label_HotKey_AddFragment = new System.Windows.Forms.Label();
            this.label_HotKey_AddMoney = new System.Windows.Forms.Label();
            this.label_HotKey_UnlockResourceLimit = new System.Windows.Forms.Label();
            this.btn_AddItems = new System.Windows.Forms.Button();
            this.checkBox_UnlockResourceLimit = new System.Windows.Forms.CheckBox();
            this.btn_EquipEdit = new System.Windows.Forms.Button();
            this.btn_AddFragment = new System.Windows.Forms.Button();
            this.btn_AddMoney = new System.Windows.Forms.Button();
            this.groupBox_Battle = new System.Windows.Forms.GroupBox();
            this.label_HotKey_UnLimitDurableOil = new System.Windows.Forms.Label();
            this.label_HotKey_NoGameOver = new System.Windows.Forms.Label();
            this.label_HotKey_AllMaster = new System.Windows.Forms.Label();
            this.btn_SetUltimateStatus = new System.Windows.Forms.Button();
            this.checkBox_NoGameOver = new System.Windows.Forms.CheckBox();
            this.checkBox_AllMaster = new System.Windows.Forms.CheckBox();
            this.checkBox_UnLimitDurableOil = new System.Windows.Forms.CheckBox();
            this.groupBox_Character = new System.Windows.Forms.GroupBox();
            this.label_Caution_SuperKill = new System.Windows.Forms.Label();
            this.label_HotKey_FastMove = new System.Windows.Forms.Label();
            this.comboBox_FastMove = new System.Windows.Forms.ComboBox();
            this.checkBox_FastMove = new System.Windows.Forms.CheckBox();
            this.label_HotKey_TimeClear = new System.Windows.Forms.Label();
            this.label_HotKey_SuperKill = new System.Windows.Forms.Label();
            this.label_HotKey_God = new System.Windows.Forms.Label();
            this.label_HotKey_FreeJump = new System.Windows.Forms.Label();
            this.btn_TimeClear = new System.Windows.Forms.Button();
            this.btn_ChangeDifficulty = new System.Windows.Forms.Button();
            this.label_ChangeDifficulty = new System.Windows.Forms.Label();
            this.comboBox_ChangeDifficulty = new System.Windows.Forms.ComboBox();
            this.checkBox_SuperKill = new System.Windows.Forms.CheckBox();
            this.checkBox_God = new System.Windows.Forms.CheckBox();
            this.checkBox_FreeJump = new System.Windows.Forms.CheckBox();
            this.Description = new System.Windows.Forms.TabPage();
            this.textBox_Description = new System.Windows.Forms.TextBox();
            this.About = new System.Windows.Forms.TabPage();
            this.label_URL02 = new System.Windows.Forms.Label();
            this.label_URL01 = new System.Windows.Forms.Label();
            this.label_About03 = new System.Windows.Forms.Label();
            this.label_About02 = new System.Windows.Forms.Label();
            this.label_About01 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label_Status = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.Trainer.SuspendLayout();
            this.groupBox_MiniGame.SuspendLayout();
            this.groupBox_Drill.SuspendLayout();
            this.groupBox_Item.SuspendLayout();
            this.groupBox_Battle.SuspendLayout();
            this.groupBox_Character.SuspendLayout();
            this.Description.SuspendLayout();
            this.About.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.Trainer);
            this.tabControl.Controls.Add(this.Description);
            this.tabControl.Controls.Add(this.About);
            this.tabControl.Location = new System.Drawing.Point(131, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(873, 629);
            this.tabControl.TabIndex = 25;
            // 
            // Trainer
            // 
            this.Trainer.BackColor = System.Drawing.SystemColors.Control;
            this.Trainer.Controls.Add(this.groupBox_MiniGame);
            this.Trainer.Controls.Add(this.groupBox_Drill);
            this.Trainer.Controls.Add(this.groupBox_Item);
            this.Trainer.Controls.Add(this.groupBox_Battle);
            this.Trainer.Controls.Add(this.groupBox_Character);
            this.Trainer.Location = new System.Drawing.Point(4, 25);
            this.Trainer.Name = "Trainer";
            this.Trainer.Padding = new System.Windows.Forms.Padding(3);
            this.Trainer.Size = new System.Drawing.Size(865, 600);
            this.Trainer.TabIndex = 0;
            this.Trainer.Text = "修改器";
            // 
            // groupBox_MiniGame
            // 
            this.groupBox_MiniGame.Controls.Add(this.label_HotKey_TimePause);
            this.groupBox_MiniGame.Controls.Add(this.label_HotKey_Football);
            this.groupBox_MiniGame.Controls.Add(this.label_HotKey_LaserDodg);
            this.groupBox_MiniGame.Controls.Add(this.label_HotKey_MoleCombo);
            this.groupBox_MiniGame.Controls.Add(this.label_HotKey_RockCrush);
            this.groupBox_MiniGame.Controls.Add(this.checkBox_RockCrush);
            this.groupBox_MiniGame.Controls.Add(this.checkBox_Football);
            this.groupBox_MiniGame.Controls.Add(this.checkBox_LaserDodg);
            this.groupBox_MiniGame.Controls.Add(this.checkBox_MoleCombo);
            this.groupBox_MiniGame.Controls.Add(this.checkBox_TimePause);
            this.groupBox_MiniGame.Location = new System.Drawing.Point(9, 450);
            this.groupBox_MiniGame.Name = "groupBox_MiniGame";
            this.groupBox_MiniGame.Size = new System.Drawing.Size(853, 120);
            this.groupBox_MiniGame.TabIndex = 26;
            this.groupBox_MiniGame.TabStop = false;
            this.groupBox_MiniGame.Text = "小游戏修改";
            // 
            // label_HotKey_TimePause
            // 
            this.label_HotKey_TimePause.AutoSize = true;
            this.label_HotKey_TimePause.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_TimePause.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_TimePause.Location = new System.Drawing.Point(776, 63);
            this.label_HotKey_TimePause.Name = "label_HotKey_TimePause";
            this.label_HotKey_TimePause.Size = new System.Drawing.Size(71, 15);
            this.label_HotKey_TimePause.TabIndex = 35;
            this.label_HotKey_TimePause.Text = "Alt + F5";
            // 
            // label_HotKey_Football
            // 
            this.label_HotKey_Football.AutoSize = true;
            this.label_HotKey_Football.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_Football.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_Football.Location = new System.Drawing.Point(776, 32);
            this.label_HotKey_Football.Name = "label_HotKey_Football";
            this.label_HotKey_Football.Size = new System.Drawing.Size(71, 15);
            this.label_HotKey_Football.TabIndex = 34;
            this.label_HotKey_Football.Text = "Alt + F6";
            // 
            // label_HotKey_LaserDodg
            // 
            this.label_HotKey_LaserDodg.AutoSize = true;
            this.label_HotKey_LaserDodg.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_LaserDodg.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_LaserDodg.Location = new System.Drawing.Point(335, 94);
            this.label_HotKey_LaserDodg.Name = "label_HotKey_LaserDodg";
            this.label_HotKey_LaserDodg.Size = new System.Drawing.Size(71, 15);
            this.label_HotKey_LaserDodg.TabIndex = 33;
            this.label_HotKey_LaserDodg.Text = "Alt + F3";
            // 
            // label_HotKey_MoleCombo
            // 
            this.label_HotKey_MoleCombo.AutoSize = true;
            this.label_HotKey_MoleCombo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_MoleCombo.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_MoleCombo.Location = new System.Drawing.Point(335, 63);
            this.label_HotKey_MoleCombo.Name = "label_HotKey_MoleCombo";
            this.label_HotKey_MoleCombo.Size = new System.Drawing.Size(71, 15);
            this.label_HotKey_MoleCombo.TabIndex = 32;
            this.label_HotKey_MoleCombo.Text = "Alt + F2";
            // 
            // label_HotKey_RockCrush
            // 
            this.label_HotKey_RockCrush.AutoSize = true;
            this.label_HotKey_RockCrush.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_RockCrush.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_RockCrush.Location = new System.Drawing.Point(335, 32);
            this.label_HotKey_RockCrush.Name = "label_HotKey_RockCrush";
            this.label_HotKey_RockCrush.Size = new System.Drawing.Size(71, 15);
            this.label_HotKey_RockCrush.TabIndex = 31;
            this.label_HotKey_RockCrush.Text = "Alt + F1";
            // 
            // checkBox_RockCrush
            // 
            this.checkBox_RockCrush.AutoSize = true;
            this.checkBox_RockCrush.Location = new System.Drawing.Point(10, 30);
            this.checkBox_RockCrush.Name = "checkBox_RockCrush";
            this.checkBox_RockCrush.Size = new System.Drawing.Size(164, 19);
            this.checkBox_RockCrush.TabIndex = 9;
            this.checkBox_RockCrush.Text = "敌方碎石头我方得分";
            this.checkBox_RockCrush.UseVisualStyleBackColor = true;
            this.checkBox_RockCrush.CheckedChanged += new System.EventHandler(this.CheckBox_RockCrush_CheckedChanged);
            // 
            // checkBox_Football
            // 
            this.checkBox_Football.AutoSize = true;
            this.checkBox_Football.Location = new System.Drawing.Point(444, 30);
            this.checkBox_Football.Name = "checkBox_Football";
            this.checkBox_Football.Size = new System.Drawing.Size(149, 19);
            this.checkBox_Football.TabIndex = 8;
            this.checkBox_Football.Text = "敌方进球我方得分";
            this.checkBox_Football.UseVisualStyleBackColor = true;
            this.checkBox_Football.CheckedChanged += new System.EventHandler(this.CheckBox_Football_CheckedChanged);
            // 
            // checkBox_LaserDodg
            // 
            this.checkBox_LaserDodg.AutoSize = true;
            this.checkBox_LaserDodg.Location = new System.Drawing.Point(10, 92);
            this.checkBox_LaserDodg.Name = "checkBox_LaserDodg";
            this.checkBox_LaserDodg.Size = new System.Drawing.Size(104, 19);
            this.checkBox_LaserDodg.TabIndex = 7;
            this.checkBox_LaserDodg.Text = "无限躲激光";
            this.checkBox_LaserDodg.UseVisualStyleBackColor = true;
            this.checkBox_LaserDodg.CheckedChanged += new System.EventHandler(this.CheckBox_LaserDodg_CheckedChanged);
            // 
            // checkBox_MoleCombo
            // 
            this.checkBox_MoleCombo.AutoSize = true;
            this.checkBox_MoleCombo.Location = new System.Drawing.Point(10, 61);
            this.checkBox_MoleCombo.Name = "checkBox_MoleCombo";
            this.checkBox_MoleCombo.Size = new System.Drawing.Size(104, 19);
            this.checkBox_MoleCombo.TabIndex = 6;
            this.checkBox_MoleCombo.Text = "无限打地鼠";
            this.checkBox_MoleCombo.UseVisualStyleBackColor = true;
            this.checkBox_MoleCombo.CheckedChanged += new System.EventHandler(this.CheckBox_MoleCombo_CheckedChanged);
            // 
            // checkBox_TimePause
            // 
            this.checkBox_TimePause.AutoSize = true;
            this.checkBox_TimePause.Location = new System.Drawing.Point(444, 61);
            this.checkBox_TimePause.Name = "checkBox_TimePause";
            this.checkBox_TimePause.Size = new System.Drawing.Size(187, 19);
            this.checkBox_TimePause.TabIndex = 5;
            this.checkBox_TimePause.Text = "打地鼠/足球赛时间暂停";
            this.checkBox_TimePause.UseVisualStyleBackColor = true;
            this.checkBox_TimePause.CheckedChanged += new System.EventHandler(this.CheckBox_TimePause_CheckedChanged);
            // 
            // groupBox_Drill
            // 
            this.groupBox_Drill.Controls.Add(this.label_HotKey_AddDrillEnergy);
            this.groupBox_Drill.Controls.Add(this.ActivateFourDrill);
            this.groupBox_Drill.Controls.Add(this.label_HotKey_EnhanceOilPlus);
            this.groupBox_Drill.Controls.Add(this.btn_AddDrillEnergy);
            this.groupBox_Drill.Controls.Add(this.btn_ActivateFourDrill);
            this.groupBox_Drill.Controls.Add(this.checkBox_EnhanceOilPlus);
            this.groupBox_Drill.Enabled = false;
            this.groupBox_Drill.Location = new System.Drawing.Point(9, 268);
            this.groupBox_Drill.Name = "groupBox_Drill";
            this.groupBox_Drill.Size = new System.Drawing.Size(417, 172);
            this.groupBox_Drill.TabIndex = 30;
            this.groupBox_Drill.TabStop = false;
            this.groupBox_Drill.Text = "钻头属性修改";
            // 
            // label_HotKey_AddDrillEnergy
            // 
            this.label_HotKey_AddDrillEnergy.AutoSize = true;
            this.label_HotKey_AddDrillEnergy.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_AddDrillEnergy.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_AddDrillEnergy.Location = new System.Drawing.Point(327, 101);
            this.label_HotKey_AddDrillEnergy.Name = "label_HotKey_AddDrillEnergy";
            this.label_HotKey_AddDrillEnergy.Size = new System.Drawing.Size(79, 15);
            this.label_HotKey_AddDrillEnergy.TabIndex = 32;
            this.label_HotKey_AddDrillEnergy.Text = "Ctrl + F3";
            // 
            // ActivateFourDrill
            // 
            this.ActivateFourDrill.AutoSize = true;
            this.ActivateFourDrill.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ActivateFourDrill.ForeColor = System.Drawing.Color.Blue;
            this.ActivateFourDrill.Location = new System.Drawing.Point(327, 64);
            this.ActivateFourDrill.Name = "ActivateFourDrill";
            this.ActivateFourDrill.Size = new System.Drawing.Size(79, 15);
            this.ActivateFourDrill.TabIndex = 31;
            this.ActivateFourDrill.Text = "Ctrl + F2";
            // 
            // label_HotKey_EnhanceOilPlus
            // 
            this.label_HotKey_EnhanceOilPlus.AutoSize = true;
            this.label_HotKey_EnhanceOilPlus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_EnhanceOilPlus.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_EnhanceOilPlus.Location = new System.Drawing.Point(327, 32);
            this.label_HotKey_EnhanceOilPlus.Name = "label_HotKey_EnhanceOilPlus";
            this.label_HotKey_EnhanceOilPlus.Size = new System.Drawing.Size(79, 15);
            this.label_HotKey_EnhanceOilPlus.TabIndex = 30;
            this.label_HotKey_EnhanceOilPlus.Text = "Ctrl + F1";
            // 
            // btn_AddDrillEnergy
            // 
            this.btn_AddDrillEnergy.Location = new System.Drawing.Point(10, 93);
            this.btn_AddDrillEnergy.Name = "btn_AddDrillEnergy";
            this.btn_AddDrillEnergy.Size = new System.Drawing.Size(180, 30);
            this.btn_AddDrillEnergy.TabIndex = 20;
            this.btn_AddDrillEnergy.Text = "钻头能量加满";
            this.btn_AddDrillEnergy.UseVisualStyleBackColor = true;
            this.btn_AddDrillEnergy.Click += new System.EventHandler(this.Btn_AddDrillEnergy_Click);
            // 
            // btn_ActivateFourDrill
            // 
            this.btn_ActivateFourDrill.Location = new System.Drawing.Point(10, 56);
            this.btn_ActivateFourDrill.Name = "btn_ActivateFourDrill";
            this.btn_ActivateFourDrill.Size = new System.Drawing.Size(180, 30);
            this.btn_ActivateFourDrill.TabIndex = 19;
            this.btn_ActivateFourDrill.Text = "激活四级钻头";
            this.btn_ActivateFourDrill.UseVisualStyleBackColor = true;
            this.btn_ActivateFourDrill.Click += new System.EventHandler(this.Btn_ActivateFourDrill_Click);
            // 
            // checkBox_EnhanceOilPlus
            // 
            this.checkBox_EnhanceOilPlus.AutoSize = true;
            this.checkBox_EnhanceOilPlus.Location = new System.Drawing.Point(10, 30);
            this.checkBox_EnhanceOilPlus.Name = "checkBox_EnhanceOilPlus";
            this.checkBox_EnhanceOilPlus.Size = new System.Drawing.Size(233, 19);
            this.checkBox_EnhanceOilPlus.TabIndex = 18;
            this.checkBox_EnhanceOilPlus.Text = "强化油、钻头挑土能量增量20%";
            this.checkBox_EnhanceOilPlus.UseVisualStyleBackColor = true;
            this.checkBox_EnhanceOilPlus.CheckedChanged += new System.EventHandler(this.CheckBox_EnhanceOilPlus_CheckedChanged);
            // 
            // groupBox_Item
            // 
            this.groupBox_Item.Controls.Add(this.label_HotKey_AddItems);
            this.groupBox_Item.Controls.Add(this.label_HotKey_AddFragment);
            this.groupBox_Item.Controls.Add(this.label_HotKey_AddMoney);
            this.groupBox_Item.Controls.Add(this.label_HotKey_UnlockResourceLimit);
            this.groupBox_Item.Controls.Add(this.btn_AddItems);
            this.groupBox_Item.Controls.Add(this.checkBox_UnlockResourceLimit);
            this.groupBox_Item.Controls.Add(this.btn_EquipEdit);
            this.groupBox_Item.Controls.Add(this.btn_AddFragment);
            this.groupBox_Item.Controls.Add(this.btn_AddMoney);
            this.groupBox_Item.Enabled = false;
            this.groupBox_Item.Location = new System.Drawing.Point(441, 13);
            this.groupBox_Item.Name = "groupBox_Item";
            this.groupBox_Item.Size = new System.Drawing.Size(417, 250);
            this.groupBox_Item.TabIndex = 29;
            this.groupBox_Item.TabStop = false;
            this.groupBox_Item.Text = "资源装备修改";
            // 
            // label_HotKey_AddItems
            // 
            this.label_HotKey_AddItems.AutoSize = true;
            this.label_HotKey_AddItems.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_AddItems.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_AddItems.Location = new System.Drawing.Point(320, 152);
            this.label_HotKey_AddItems.Name = "label_HotKey_AddItems";
            this.label_HotKey_AddItems.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_AddItems.TabIndex = 32;
            this.label_HotKey_AddItems.Text = "Shift + F9";
            // 
            // label_HotKey_AddFragment
            // 
            this.label_HotKey_AddFragment.AutoSize = true;
            this.label_HotKey_AddFragment.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_AddFragment.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_AddFragment.Location = new System.Drawing.Point(320, 110);
            this.label_HotKey_AddFragment.Name = "label_HotKey_AddFragment";
            this.label_HotKey_AddFragment.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_AddFragment.TabIndex = 31;
            this.label_HotKey_AddFragment.Text = "Shift + F8";
            // 
            // label_HotKey_AddMoney
            // 
            this.label_HotKey_AddMoney.AutoSize = true;
            this.label_HotKey_AddMoney.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_AddMoney.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_AddMoney.Location = new System.Drawing.Point(320, 68);
            this.label_HotKey_AddMoney.Name = "label_HotKey_AddMoney";
            this.label_HotKey_AddMoney.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_AddMoney.TabIndex = 30;
            this.label_HotKey_AddMoney.Text = "Shift + F7";
            // 
            // label_HotKey_UnlockResourceLimit
            // 
            this.label_HotKey_UnlockResourceLimit.AutoSize = true;
            this.label_HotKey_UnlockResourceLimit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_UnlockResourceLimit.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_UnlockResourceLimit.Location = new System.Drawing.Point(320, 32);
            this.label_HotKey_UnlockResourceLimit.Name = "label_HotKey_UnlockResourceLimit";
            this.label_HotKey_UnlockResourceLimit.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_UnlockResourceLimit.TabIndex = 29;
            this.label_HotKey_UnlockResourceLimit.Text = "Shift + F6";
            // 
            // btn_AddItems
            // 
            this.btn_AddItems.Location = new System.Drawing.Point(10, 144);
            this.btn_AddItems.Name = "btn_AddItems";
            this.btn_AddItems.Size = new System.Drawing.Size(180, 30);
            this.btn_AddItems.TabIndex = 25;
            this.btn_AddItems.Text = "消耗类道具加满";
            this.btn_AddItems.UseVisualStyleBackColor = true;
            this.btn_AddItems.Click += new System.EventHandler(this.Btn_AddItems_Click);
            // 
            // checkBox_UnlockResourceLimit
            // 
            this.checkBox_UnlockResourceLimit.AutoSize = true;
            this.checkBox_UnlockResourceLimit.Location = new System.Drawing.Point(10, 30);
            this.checkBox_UnlockResourceLimit.Name = "checkBox_UnlockResourceLimit";
            this.checkBox_UnlockResourceLimit.Size = new System.Drawing.Size(119, 19);
            this.checkBox_UnlockResourceLimit.TabIndex = 23;
            this.checkBox_UnlockResourceLimit.Text = "突破资源上限";
            this.checkBox_UnlockResourceLimit.UseVisualStyleBackColor = true;
            this.checkBox_UnlockResourceLimit.CheckedChanged += new System.EventHandler(this.CheckBox_UnlockResourceLimit_CheckedChanged);
            // 
            // btn_EquipEdit
            // 
            this.btn_EquipEdit.Location = new System.Drawing.Point(10, 186);
            this.btn_EquipEdit.Name = "btn_EquipEdit";
            this.btn_EquipEdit.Size = new System.Drawing.Size(180, 30);
            this.btn_EquipEdit.TabIndex = 22;
            this.btn_EquipEdit.Text = "修改持有装备及等级";
            this.btn_EquipEdit.UseVisualStyleBackColor = true;
            this.btn_EquipEdit.Click += new System.EventHandler(this.Btn_EquipEdit_Click);
            // 
            // btn_AddFragment
            // 
            this.btn_AddFragment.Location = new System.Drawing.Point(10, 102);
            this.btn_AddFragment.Name = "btn_AddFragment";
            this.btn_AddFragment.Size = new System.Drawing.Size(180, 30);
            this.btn_AddFragment.TabIndex = 21;
            this.btn_AddFragment.Text = "给我100碎片";
            this.btn_AddFragment.UseVisualStyleBackColor = true;
            this.btn_AddFragment.Click += new System.EventHandler(this.Btn_AddFragment_Click);
            // 
            // btn_AddMoney
            // 
            this.btn_AddMoney.Location = new System.Drawing.Point(10, 60);
            this.btn_AddMoney.Name = "btn_AddMoney";
            this.btn_AddMoney.Size = new System.Drawing.Size(180, 30);
            this.btn_AddMoney.TabIndex = 20;
            this.btn_AddMoney.Text = "给我1000块钱";
            this.btn_AddMoney.UseVisualStyleBackColor = true;
            this.btn_AddMoney.Click += new System.EventHandler(this.Btn_AddMoney_Click);
            // 
            // groupBox_Battle
            // 
            this.groupBox_Battle.Controls.Add(this.label_HotKey_UnLimitDurableOil);
            this.groupBox_Battle.Controls.Add(this.label_HotKey_NoGameOver);
            this.groupBox_Battle.Controls.Add(this.label_HotKey_AllMaster);
            this.groupBox_Battle.Controls.Add(this.btn_SetUltimateStatus);
            this.groupBox_Battle.Controls.Add(this.checkBox_NoGameOver);
            this.groupBox_Battle.Controls.Add(this.checkBox_AllMaster);
            this.groupBox_Battle.Controls.Add(this.checkBox_UnLimitDurableOil);
            this.groupBox_Battle.Enabled = false;
            this.groupBox_Battle.Location = new System.Drawing.Point(441, 268);
            this.groupBox_Battle.Name = "groupBox_Battle";
            this.groupBox_Battle.Size = new System.Drawing.Size(417, 172);
            this.groupBox_Battle.TabIndex = 28;
            this.groupBox_Battle.TabStop = false;
            this.groupBox_Battle.Text = "战斗属性修改";
            // 
            // label_HotKey_UnLimitDurableOil
            // 
            this.label_HotKey_UnLimitDurableOil.AutoSize = true;
            this.label_HotKey_UnLimitDurableOil.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_UnLimitDurableOil.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_UnLimitDurableOil.Location = new System.Drawing.Point(320, 100);
            this.label_HotKey_UnLimitDurableOil.Name = "label_HotKey_UnLimitDurableOil";
            this.label_HotKey_UnLimitDurableOil.Size = new System.Drawing.Size(95, 15);
            this.label_HotKey_UnLimitDurableOil.TabIndex = 33;
            this.label_HotKey_UnLimitDurableOil.Text = "Shift + F12";
            // 
            // label_HotKey_NoGameOver
            // 
            this.label_HotKey_NoGameOver.AutoSize = true;
            this.label_HotKey_NoGameOver.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_NoGameOver.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_NoGameOver.Location = new System.Drawing.Point(320, 66);
            this.label_HotKey_NoGameOver.Name = "label_HotKey_NoGameOver";
            this.label_HotKey_NoGameOver.Size = new System.Drawing.Size(95, 15);
            this.label_HotKey_NoGameOver.TabIndex = 32;
            this.label_HotKey_NoGameOver.Text = "Shift + F11";
            // 
            // label_HotKey_AllMaster
            // 
            this.label_HotKey_AllMaster.AutoSize = true;
            this.label_HotKey_AllMaster.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_AllMaster.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_AllMaster.Location = new System.Drawing.Point(320, 32);
            this.label_HotKey_AllMaster.Name = "label_HotKey_AllMaster";
            this.label_HotKey_AllMaster.Size = new System.Drawing.Size(95, 15);
            this.label_HotKey_AllMaster.TabIndex = 31;
            this.label_HotKey_AllMaster.Text = "Shift + F10";
            // 
            // btn_SetUltimateStatus
            // 
            this.btn_SetUltimateStatus.Location = new System.Drawing.Point(10, 127);
            this.btn_SetUltimateStatus.Name = "btn_SetUltimateStatus";
            this.btn_SetUltimateStatus.Size = new System.Drawing.Size(180, 30);
            this.btn_SetUltimateStatus.TabIndex = 22;
            this.btn_SetUltimateStatus.Text = "修改必杀技";
            this.btn_SetUltimateStatus.UseVisualStyleBackColor = true;
            this.btn_SetUltimateStatus.Click += new System.EventHandler(this.Btn_SetUltimate_Click);
            // 
            // checkBox_NoGameOver
            // 
            this.checkBox_NoGameOver.AutoSize = true;
            this.checkBox_NoGameOver.Location = new System.Drawing.Point(10, 64);
            this.checkBox_NoGameOver.Name = "checkBox_NoGameOver";
            this.checkBox_NoGameOver.Size = new System.Drawing.Size(198, 19);
            this.checkBox_NoGameOver.TabIndex = 17;
            this.checkBox_NoGameOver.Text = "迷宫结算不累计GameOver";
            this.checkBox_NoGameOver.UseVisualStyleBackColor = true;
            this.checkBox_NoGameOver.CheckedChanged += new System.EventHandler(this.CheckBox_NoGameOver_CheckedChanged);
            // 
            // checkBox_AllMaster
            // 
            this.checkBox_AllMaster.AutoSize = true;
            this.checkBox_AllMaster.Location = new System.Drawing.Point(10, 30);
            this.checkBox_AllMaster.Name = "checkBox_AllMaster";
            this.checkBox_AllMaster.Size = new System.Drawing.Size(180, 19);
            this.checkBox_AllMaster.TabIndex = 16;
            this.checkBox_AllMaster.Text = "敌人数/宝壶/宝箱全清";
            this.checkBox_AllMaster.UseVisualStyleBackColor = true;
            this.checkBox_AllMaster.CheckedChanged += new System.EventHandler(this.CheckBox_AllMaster_CheckedChanged);
            // 
            // checkBox_UnLimitDurableOil
            // 
            this.checkBox_UnLimitDurableOil.AutoSize = true;
            this.checkBox_UnLimitDurableOil.Location = new System.Drawing.Point(10, 98);
            this.checkBox_UnLimitDurableOil.Name = "checkBox_UnLimitDurableOil";
            this.checkBox_UnLimitDurableOil.Size = new System.Drawing.Size(134, 19);
            this.checkBox_UnLimitDurableOil.TabIndex = 15;
            this.checkBox_UnLimitDurableOil.Text = "耐久油时间不减";
            this.checkBox_UnLimitDurableOil.UseVisualStyleBackColor = true;
            this.checkBox_UnLimitDurableOil.CheckedChanged += new System.EventHandler(this.CheckBox_UnLimitDurableOil_CheckedChanged);
            // 
            // groupBox_Character
            // 
            this.groupBox_Character.Controls.Add(this.label_Caution_SuperKill);
            this.groupBox_Character.Controls.Add(this.label_HotKey_FastMove);
            this.groupBox_Character.Controls.Add(this.comboBox_FastMove);
            this.groupBox_Character.Controls.Add(this.checkBox_FastMove);
            this.groupBox_Character.Controls.Add(this.label_HotKey_TimeClear);
            this.groupBox_Character.Controls.Add(this.label_HotKey_SuperKill);
            this.groupBox_Character.Controls.Add(this.label_HotKey_God);
            this.groupBox_Character.Controls.Add(this.label_HotKey_FreeJump);
            this.groupBox_Character.Controls.Add(this.btn_TimeClear);
            this.groupBox_Character.Controls.Add(this.btn_ChangeDifficulty);
            this.groupBox_Character.Controls.Add(this.label_ChangeDifficulty);
            this.groupBox_Character.Controls.Add(this.comboBox_ChangeDifficulty);
            this.groupBox_Character.Controls.Add(this.checkBox_SuperKill);
            this.groupBox_Character.Controls.Add(this.checkBox_God);
            this.groupBox_Character.Controls.Add(this.checkBox_FreeJump);
            this.groupBox_Character.Enabled = false;
            this.groupBox_Character.Location = new System.Drawing.Point(9, 13);
            this.groupBox_Character.Name = "groupBox_Character";
            this.groupBox_Character.Size = new System.Drawing.Size(417, 250);
            this.groupBox_Character.TabIndex = 27;
            this.groupBox_Character.TabStop = false;
            this.groupBox_Character.Text = "基本属性修改";
            // 
            // label_Caution_SuperKill
            // 
            this.label_Caution_SuperKill.AutoSize = true;
            this.label_Caution_SuperKill.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Caution_SuperKill.ForeColor = System.Drawing.Color.Red;
            this.label_Caution_SuperKill.Location = new System.Drawing.Point(100, 94);
            this.label_Caution_SuperKill.Name = "label_Caution_SuperKill";
            this.label_Caution_SuperKill.Size = new System.Drawing.Size(201, 15);
            this.label_Caution_SuperKill.TabIndex = 31;
            this.label_Caution_SuperKill.Text = "(不要在足球赛中一击必杀)";
            // 
            // label_HotKey_FastMove
            // 
            this.label_HotKey_FastMove.AutoSize = true;
            this.label_HotKey_FastMove.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_FastMove.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_FastMove.Location = new System.Drawing.Point(319, 123);
            this.label_HotKey_FastMove.Name = "label_HotKey_FastMove";
            this.label_HotKey_FastMove.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_FastMove.TabIndex = 30;
            this.label_HotKey_FastMove.Text = "Shift + F4";
            // 
            // comboBox_FastMove
            // 
            this.comboBox_FastMove.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_FastMove.FormattingEnabled = true;
            this.comboBox_FastMove.Location = new System.Drawing.Point(194, 119);
            this.comboBox_FastMove.Name = "comboBox_FastMove";
            this.comboBox_FastMove.Size = new System.Drawing.Size(102, 23);
            this.comboBox_FastMove.TabIndex = 29;
            this.comboBox_FastMove.SelectedIndexChanged += new System.EventHandler(this.ComboBox_FastMove_SelectedIndexChanged);
            // 
            // checkBox_FastMove
            // 
            this.checkBox_FastMove.AutoSize = true;
            this.checkBox_FastMove.Location = new System.Drawing.Point(10, 121);
            this.checkBox_FastMove.Name = "checkBox_FastMove";
            this.checkBox_FastMove.Size = new System.Drawing.Size(180, 19);
            this.checkBox_FastMove.TabIndex = 28;
            this.checkBox_FastMove.Text = "快速移动(请选择倍率)";
            this.checkBox_FastMove.UseVisualStyleBackColor = true;
            this.checkBox_FastMove.CheckedChanged += new System.EventHandler(this.CheckBox_FastMove_CheckedChanged);
            // 
            // label_HotKey_TimeClear
            // 
            this.label_HotKey_TimeClear.AutoSize = true;
            this.label_HotKey_TimeClear.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_TimeClear.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_TimeClear.Location = new System.Drawing.Point(319, 154);
            this.label_HotKey_TimeClear.Name = "label_HotKey_TimeClear";
            this.label_HotKey_TimeClear.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_TimeClear.TabIndex = 27;
            this.label_HotKey_TimeClear.Text = "Shift + F5";
            // 
            // label_HotKey_SuperKill
            // 
            this.label_HotKey_SuperKill.AutoSize = true;
            this.label_HotKey_SuperKill.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_SuperKill.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_SuperKill.Location = new System.Drawing.Point(319, 94);
            this.label_HotKey_SuperKill.Name = "label_HotKey_SuperKill";
            this.label_HotKey_SuperKill.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_SuperKill.TabIndex = 26;
            this.label_HotKey_SuperKill.Text = "Shift + F3";
            // 
            // label_HotKey_God
            // 
            this.label_HotKey_God.AutoSize = true;
            this.label_HotKey_God.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_God.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_God.Location = new System.Drawing.Point(319, 32);
            this.label_HotKey_God.Name = "label_HotKey_God";
            this.label_HotKey_God.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_God.TabIndex = 25;
            this.label_HotKey_God.Text = "Shift + F1";
            // 
            // label_HotKey_FreeJump
            // 
            this.label_HotKey_FreeJump.AutoSize = true;
            this.label_HotKey_FreeJump.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_HotKey_FreeJump.ForeColor = System.Drawing.Color.Blue;
            this.label_HotKey_FreeJump.Location = new System.Drawing.Point(319, 63);
            this.label_HotKey_FreeJump.Name = "label_HotKey_FreeJump";
            this.label_HotKey_FreeJump.Size = new System.Drawing.Size(87, 15);
            this.label_HotKey_FreeJump.TabIndex = 24;
            this.label_HotKey_FreeJump.Text = "Shift + F2";
            // 
            // btn_TimeClear
            // 
            this.btn_TimeClear.Location = new System.Drawing.Point(10, 146);
            this.btn_TimeClear.Name = "btn_TimeClear";
            this.btn_TimeClear.Size = new System.Drawing.Size(180, 30);
            this.btn_TimeClear.TabIndex = 19;
            this.btn_TimeClear.Text = "清零总游戏时间";
            this.btn_TimeClear.UseVisualStyleBackColor = true;
            this.btn_TimeClear.Click += new System.EventHandler(this.Btn_TimeClear_Click);
            // 
            // btn_ChangeDifficulty
            // 
            this.btn_ChangeDifficulty.Location = new System.Drawing.Point(181, 210);
            this.btn_ChangeDifficulty.Name = "btn_ChangeDifficulty";
            this.btn_ChangeDifficulty.Size = new System.Drawing.Size(80, 30);
            this.btn_ChangeDifficulty.TabIndex = 18;
            this.btn_ChangeDifficulty.Text = "提交";
            this.btn_ChangeDifficulty.UseVisualStyleBackColor = true;
            this.btn_ChangeDifficulty.Click += new System.EventHandler(this.Btn_ChangeDifficulty_Click);
            // 
            // label_ChangeDifficulty
            // 
            this.label_ChangeDifficulty.AutoSize = true;
            this.label_ChangeDifficulty.Location = new System.Drawing.Point(10, 188);
            this.label_ChangeDifficulty.Name = "label_ChangeDifficulty";
            this.label_ChangeDifficulty.Size = new System.Drawing.Size(97, 15);
            this.label_ChangeDifficulty.TabIndex = 17;
            this.label_ChangeDifficulty.Text = "修改游戏难度";
            // 
            // comboBox_ChangeDifficulty
            // 
            this.comboBox_ChangeDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ChangeDifficulty.FormattingEnabled = true;
            this.comboBox_ChangeDifficulty.Location = new System.Drawing.Point(10, 214);
            this.comboBox_ChangeDifficulty.Name = "comboBox_ChangeDifficulty";
            this.comboBox_ChangeDifficulty.Size = new System.Drawing.Size(150, 23);
            this.comboBox_ChangeDifficulty.TabIndex = 16;
            // 
            // checkBox_SuperKill
            // 
            this.checkBox_SuperKill.AutoSize = true;
            this.checkBox_SuperKill.Location = new System.Drawing.Point(10, 92);
            this.checkBox_SuperKill.Name = "checkBox_SuperKill";
            this.checkBox_SuperKill.Size = new System.Drawing.Size(89, 19);
            this.checkBox_SuperKill.TabIndex = 15;
            this.checkBox_SuperKill.Text = "一击必杀";
            this.checkBox_SuperKill.UseVisualStyleBackColor = true;
            this.checkBox_SuperKill.CheckedChanged += new System.EventHandler(this.CheckBox_SuperKill_CheckedChanged);
            // 
            // checkBox_God
            // 
            this.checkBox_God.AutoSize = true;
            this.checkBox_God.Location = new System.Drawing.Point(10, 30);
            this.checkBox_God.Name = "checkBox_God";
            this.checkBox_God.Size = new System.Drawing.Size(89, 19);
            this.checkBox_God.TabIndex = 14;
            this.checkBox_God.Text = "主角无敌";
            this.checkBox_God.UseVisualStyleBackColor = true;
            this.checkBox_God.CheckedChanged += new System.EventHandler(this.CheckBox_God_CheckedChanged);
            // 
            // checkBox_FreeJump
            // 
            this.checkBox_FreeJump.AutoSize = true;
            this.checkBox_FreeJump.Location = new System.Drawing.Point(10, 61);
            this.checkBox_FreeJump.Name = "checkBox_FreeJump";
            this.checkBox_FreeJump.Size = new System.Drawing.Size(89, 19);
            this.checkBox_FreeJump.TabIndex = 13;
            this.checkBox_FreeJump.Text = "无限跳跃";
            this.checkBox_FreeJump.UseVisualStyleBackColor = true;
            this.checkBox_FreeJump.CheckedChanged += new System.EventHandler(this.CheckBox_FreeJump_CheckedChanged);
            // 
            // Description
            // 
            this.Description.BackColor = System.Drawing.SystemColors.Control;
            this.Description.Controls.Add(this.textBox_Description);
            this.Description.Location = new System.Drawing.Point(4, 25);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(865, 600);
            this.Description.TabIndex = 2;
            this.Description.Text = "使用说明";
            // 
            // textBox_Description
            // 
            this.textBox_Description.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Description.Location = new System.Drawing.Point(0, 0);
            this.textBox_Description.Multiline = true;
            this.textBox_Description.Name = "textBox_Description";
            this.textBox_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Description.Size = new System.Drawing.Size(159, 25);
            this.textBox_Description.TabIndex = 1;
            // 
            // About
            // 
            this.About.BackColor = System.Drawing.SystemColors.Control;
            this.About.Controls.Add(this.label_URL02);
            this.About.Controls.Add(this.label_URL01);
            this.About.Controls.Add(this.label_About03);
            this.About.Controls.Add(this.label_About02);
            this.About.Controls.Add(this.label_About01);
            this.About.Location = new System.Drawing.Point(4, 25);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(865, 600);
            this.About.TabIndex = 3;
            this.About.Text = "关于修改器";
            // 
            // label_URL02
            // 
            this.label_URL02.AutoSize = true;
            this.label_URL02.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_URL02.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Underline);
            this.label_URL02.ForeColor = System.Drawing.Color.Blue;
            this.label_URL02.Location = new System.Drawing.Point(228, 356);
            this.label_URL02.Name = "label_URL02";
            this.label_URL02.Size = new System.Drawing.Size(529, 20);
            this.label_URL02.TabIndex = 4;
            this.label_URL02.Text = "https://github.com/xingshen60771/GuruminTrainerSteam";
            this.label_URL02.Click += new System.EventHandler(this.label_URL02_Click);
            // 
            // label_URL01
            // 
            this.label_URL01.AutoSize = true;
            this.label_URL01.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_URL01.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Underline);
            this.label_URL01.ForeColor = System.Drawing.Color.Blue;
            this.label_URL01.Location = new System.Drawing.Point(228, 310);
            this.label_URL01.Name = "label_URL01";
            this.label_URL01.Size = new System.Drawing.Size(469, 20);
            this.label_URL01.TabIndex = 3;
            this.label_URL01.Text = "https://www.52pojie.cn/thread-2112391-1-1.html";
            this.label_URL01.Click += new System.EventHandler(this.label_URL01_Click);
            // 
            // label_About03
            // 
            this.label_About03.AutoSize = true;
            this.label_About03.Font = new System.Drawing.Font("宋体", 12F);
            this.label_About03.Location = new System.Drawing.Point(93, 356);
            this.label_About03.Name = "label_About03";
            this.label_About03.Size = new System.Drawing.Size(129, 20);
            this.label_About03.TabIndex = 2;
            this.label_About03.Text = "修改器源码：";
            // 
            // label_About02
            // 
            this.label_About02.AutoSize = true;
            this.label_About02.Font = new System.Drawing.Font("宋体", 12F);
            this.label_About02.Location = new System.Drawing.Point(13, 310);
            this.label_About02.Name = "label_About02";
            this.label_About02.Size = new System.Drawing.Size(209, 20);
            this.label_About02.TabIndex = 1;
            this.label_About02.Text = "本修改器的技术讨论：";
            // 
            // label_About01
            // 
            this.label_About01.AutoSize = true;
            this.label_About01.Font = new System.Drawing.Font("宋体", 16F);
            this.label_About01.Location = new System.Drawing.Point(23, 21);
            this.label_About01.Name = "label_About01";
            this.label_About01.Size = new System.Drawing.Size(96, 27);
            this.label_About01.TabIndex = 0;
            this.label_About01.Text = "label1";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(1, 1);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(12, 70);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(55, 15);
            this.label_Status.TabIndex = 26;
            this.label_Status.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1017, 719);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.tabControl.ResumeLayout(false);
            this.Trainer.ResumeLayout(false);
            this.groupBox_MiniGame.ResumeLayout(false);
            this.groupBox_MiniGame.PerformLayout();
            this.groupBox_Drill.ResumeLayout(false);
            this.groupBox_Drill.PerformLayout();
            this.groupBox_Item.ResumeLayout(false);
            this.groupBox_Item.PerformLayout();
            this.groupBox_Battle.ResumeLayout(false);
            this.groupBox_Battle.PerformLayout();
            this.groupBox_Character.ResumeLayout(false);
            this.groupBox_Character.PerformLayout();
            this.Description.ResumeLayout(false);
            this.Description.PerformLayout();
            this.About.ResumeLayout(false);
            this.About.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage Trainer;
        private System.Windows.Forms.TabPage Description;
        private System.Windows.Forms.TabPage About;
        private System.Windows.Forms.GroupBox groupBox_MiniGame;
        private System.Windows.Forms.Label label_HotKey_TimePause;
        private System.Windows.Forms.Label label_HotKey_Football;
        private System.Windows.Forms.Label label_HotKey_LaserDodg;
        private System.Windows.Forms.Label label_HotKey_MoleCombo;
        private System.Windows.Forms.Label label_HotKey_RockCrush;
        private System.Windows.Forms.CheckBox checkBox_RockCrush;
        private System.Windows.Forms.CheckBox checkBox_Football;
        private System.Windows.Forms.CheckBox checkBox_LaserDodg;
        private System.Windows.Forms.CheckBox checkBox_MoleCombo;
        private System.Windows.Forms.CheckBox checkBox_TimePause;
        private System.Windows.Forms.GroupBox groupBox_Drill;
        private System.Windows.Forms.Label label_HotKey_AddDrillEnergy;
        private System.Windows.Forms.Label ActivateFourDrill;
        private System.Windows.Forms.Label label_HotKey_EnhanceOilPlus;
        private System.Windows.Forms.Button btn_AddDrillEnergy;
        private System.Windows.Forms.Button btn_ActivateFourDrill;
        private System.Windows.Forms.CheckBox checkBox_EnhanceOilPlus;
        private System.Windows.Forms.GroupBox groupBox_Item;
        private System.Windows.Forms.Label label_HotKey_AddItems;
        private System.Windows.Forms.Label label_HotKey_AddFragment;
        private System.Windows.Forms.Label label_HotKey_AddMoney;
        private System.Windows.Forms.Label label_HotKey_UnlockResourceLimit;
        private System.Windows.Forms.Button btn_AddItems;
        private System.Windows.Forms.CheckBox checkBox_UnlockResourceLimit;
        private System.Windows.Forms.Button btn_EquipEdit;
        private System.Windows.Forms.Button btn_AddFragment;
        private System.Windows.Forms.Button btn_AddMoney;
        private System.Windows.Forms.GroupBox groupBox_Battle;
        private System.Windows.Forms.Label label_HotKey_UnLimitDurableOil;
        private System.Windows.Forms.Label label_HotKey_NoGameOver;
        private System.Windows.Forms.Label label_HotKey_AllMaster;
        private System.Windows.Forms.Button btn_SetUltimateStatus;
        private System.Windows.Forms.CheckBox checkBox_NoGameOver;
        private System.Windows.Forms.CheckBox checkBox_AllMaster;
        private System.Windows.Forms.CheckBox checkBox_UnLimitDurableOil;
        private System.Windows.Forms.GroupBox groupBox_Character;
        private System.Windows.Forms.Label label_HotKey_FastMove;
        private System.Windows.Forms.ComboBox comboBox_FastMove;
        private System.Windows.Forms.CheckBox checkBox_FastMove;
        private System.Windows.Forms.Label label_HotKey_TimeClear;
        private System.Windows.Forms.Label label_HotKey_SuperKill;
        private System.Windows.Forms.Label label_HotKey_God;
        private System.Windows.Forms.Label label_HotKey_FreeJump;
        private System.Windows.Forms.Button btn_TimeClear;
        private System.Windows.Forms.Button btn_ChangeDifficulty;
        private System.Windows.Forms.Label label_ChangeDifficulty;
        private System.Windows.Forms.ComboBox comboBox_ChangeDifficulty;
        private System.Windows.Forms.CheckBox checkBox_SuperKill;
        private System.Windows.Forms.CheckBox checkBox_God;
        private System.Windows.Forms.CheckBox checkBox_FreeJump;
        private System.Windows.Forms.Label label_Caution_SuperKill;
        private System.Windows.Forms.TextBox textBox_Description;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Label label_About01;
        private System.Windows.Forms.Label label_About03;
        private System.Windows.Forms.Label label_About02;
        private System.Windows.Forms.Label label_URL02;
        private System.Windows.Forms.Label label_URL01;
    }
}