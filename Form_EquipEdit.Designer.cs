namespace GuruminTrainerSteam
{
    partial class Form_EquipEdit
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
            this.trackBar_EquipLevel = new System.Windows.Forms.TrackBar();
            this.listView_Equip = new System.Windows.Forms.ListView();
            this.col_Num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_EquipName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Unlocked = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_EquipLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.radio_Unlocked = new System.Windows.Forms.RadioButton();
            this.radio_Locked = new System.Windows.Forms.RadioButton();
            this.label_Unlocked = new System.Windows.Forms.Label();
            this.label_EquipLevel = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.label_CurrentEquip = new System.Windows.Forms.Label();
            this.label_Waring = new System.Windows.Forms.Label();
            this.label_Vaule = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_EquipLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar_EquipLevel
            // 
            this.trackBar_EquipLevel.Location = new System.Drawing.Point(117, 288);
            this.trackBar_EquipLevel.Maximum = 5;
            this.trackBar_EquipLevel.Minimum = 1;
            this.trackBar_EquipLevel.Name = "trackBar_EquipLevel";
            this.trackBar_EquipLevel.Size = new System.Drawing.Size(208, 56);
            this.trackBar_EquipLevel.TabIndex = 2;
            this.trackBar_EquipLevel.Value = 1;
            this.trackBar_EquipLevel.Scroll += new System.EventHandler(this.trackBar_EquipLevel_Scroll);
            // 
            // listView_Equip
            // 
            this.listView_Equip.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_Num,
            this.col_EquipName,
            this.col_Unlocked,
            this.col_EquipLevel});
            this.listView_Equip.FullRowSelect = true;
            this.listView_Equip.GridLines = true;
            this.listView_Equip.HideSelection = false;
            this.listView_Equip.Location = new System.Drawing.Point(10, 10);
            this.listView_Equip.Name = "listView_Equip";
            this.listView_Equip.Size = new System.Drawing.Size(760, 200);
            this.listView_Equip.TabIndex = 3;
            this.listView_Equip.UseCompatibleStateImageBehavior = false;
            this.listView_Equip.View = System.Windows.Forms.View.Details;
            // 
            // col_Num
            // 
            this.col_Num.Text = "序号";
            this.col_Num.Width = 100;
            // 
            // col_EquipName
            // 
            this.col_EquipName.Text = "防具名称";
            this.col_EquipName.Width = 200;
            // 
            // col_Unlocked
            // 
            this.col_Unlocked.Text = "是否持有";
            this.col_Unlocked.Width = 100;
            // 
            // col_EquipLevel
            // 
            this.col_EquipLevel.Text = "防具等级";
            this.col_EquipLevel.Width = 100;
            // 
            // radio_Unlocked
            // 
            this.radio_Unlocked.AutoSize = true;
            this.radio_Unlocked.Location = new System.Drawing.Point(12, 288);
            this.radio_Unlocked.Name = "radio_Unlocked";
            this.radio_Unlocked.Size = new System.Drawing.Size(43, 19);
            this.radio_Unlocked.TabIndex = 4;
            this.radio_Unlocked.TabStop = true;
            this.radio_Unlocked.Text = "是";
            this.radio_Unlocked.UseVisualStyleBackColor = true;
            // 
            // radio_Locked
            // 
            this.radio_Locked.AutoSize = true;
            this.radio_Locked.Location = new System.Drawing.Point(12, 317);
            this.radio_Locked.Name = "radio_Locked";
            this.radio_Locked.Size = new System.Drawing.Size(43, 19);
            this.radio_Locked.TabIndex = 5;
            this.radio_Locked.TabStop = true;
            this.radio_Locked.Text = "否";
            this.radio_Locked.UseVisualStyleBackColor = true;
            // 
            // label_Unlocked
            // 
            this.label_Unlocked.AutoSize = true;
            this.label_Unlocked.Location = new System.Drawing.Point(12, 263);
            this.label_Unlocked.Name = "label_Unlocked";
            this.label_Unlocked.Size = new System.Drawing.Size(67, 15);
            this.label_Unlocked.TabIndex = 6;
            this.label_Unlocked.Text = "是否持有";
            // 
            // label_EquipLevel
            // 
            this.label_EquipLevel.AutoSize = true;
            this.label_EquipLevel.Location = new System.Drawing.Point(117, 263);
            this.label_EquipLevel.Name = "label_EquipLevel";
            this.label_EquipLevel.Size = new System.Drawing.Size(67, 15);
            this.label_EquipLevel.TabIndex = 7;
            this.label_EquipLevel.Text = "防具等级";
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(512, 411);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(126, 30);
            this.btn_Save.TabIndex = 11;
            this.btn_Save.Text = "提交保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(644, 411);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(126, 30);
            this.btn_Close.TabIndex = 12;
            this.btn_Close.Text = "关闭";
            this.btn_Close.UseVisualStyleBackColor = true;
            // 
            // label_CurrentEquip
            // 
            this.label_CurrentEquip.AutoSize = true;
            this.label_CurrentEquip.Font = new System.Drawing.Font("宋体", 16F);
            this.label_CurrentEquip.ForeColor = System.Drawing.Color.Blue;
            this.label_CurrentEquip.Location = new System.Drawing.Point(12, 221);
            this.label_CurrentEquip.Name = "label_CurrentEquip";
            this.label_CurrentEquip.Size = new System.Drawing.Size(82, 27);
            this.label_CurrentEquip.TabIndex = 13;
            this.label_CurrentEquip.Text = "label";
            // 
            // label_Waring
            // 
            this.label_Waring.BackColor = System.Drawing.Color.Yellow;
            this.label_Waring.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold);
            this.label_Waring.ForeColor = System.Drawing.Color.Red;
            this.label_Waring.Location = new System.Drawing.Point(7, 347);
            this.label_Waring.Name = "label_Waring";
            this.label_Waring.Size = new System.Drawing.Size(463, 94);
            this.label_Waring.TabIndex = 14;
            this.label_Waring.Text = "警告：无论单独修改装备是否持有还是单独修改等级，修改任何一项属性都必须提交保存！否则修改无效！";
            this.label_Waring.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Vaule
            // 
            this.label_Vaule.AutoSize = true;
            this.label_Vaule.Font = new System.Drawing.Font("宋体", 16F);
            this.label_Vaule.ForeColor = System.Drawing.Color.Blue;
            this.label_Vaule.Location = new System.Drawing.Point(351, 303);
            this.label_Vaule.Name = "label_Vaule";
            this.label_Vaule.Size = new System.Drawing.Size(82, 27);
            this.label_Vaule.TabIndex = 15;
            this.label_Vaule.Text = "label";
            // 
            // Form_EquipEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 453);
            this.Controls.Add(this.label_Vaule);
            this.Controls.Add(this.label_Waring);
            this.Controls.Add(this.label_CurrentEquip);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.label_EquipLevel);
            this.Controls.Add(this.label_Unlocked);
            this.Controls.Add(this.radio_Locked);
            this.Controls.Add(this.radio_Unlocked);
            this.Controls.Add(this.listView_Equip);
            this.Controls.Add(this.trackBar_EquipLevel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_EquipEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改装备持有情况及等级";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_EquipLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar trackBar_EquipLevel;
        private System.Windows.Forms.ListView listView_Equip;
        private System.Windows.Forms.ColumnHeader col_Num;
        private System.Windows.Forms.ColumnHeader col_EquipName;
        private System.Windows.Forms.ColumnHeader col_Unlocked;
        private System.Windows.Forms.ColumnHeader col_EquipLevel;
        private System.Windows.Forms.RadioButton radio_Unlocked;
        private System.Windows.Forms.RadioButton radio_Locked;
        private System.Windows.Forms.Label label_Unlocked;
        private System.Windows.Forms.Label label_EquipLevel;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Label label_CurrentEquip;
        private System.Windows.Forms.Label label_Waring;
        private System.Windows.Forms.Label label_Vaule;
    }
}