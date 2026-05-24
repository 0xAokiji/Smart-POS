namespace pos.Settings
{
    partial class frmStaff
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStaff));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            mainPanel = new Panel();
            btnSalary = new Guna.UI2.WinForms.Guna2Button();
            dgvStaff = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno = new DataGridViewTextBoxColumn();
            dgvid = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            dgvPhone = new DataGridViewTextBoxColumn();
            dgvRole = new DataGridViewTextBoxColumn();
            dgvSalary = new DataGridViewTextBoxColumn();
            dgvAdvance = new DataGridViewTextBoxColumn();
            dgvTackSalary = new DataGridViewTextBoxColumn();
            dgvSelect = new DataGridViewCheckBoxColumn();
            btnReload = new Guna.UI2.WinForms.Guna2Button();
            btnEdite = new Guna.UI2.WinForms.Guna2Button();
            btnDelete = new Guna.UI2.WinForms.Guna2Button();
            btnAddStaff = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            timer1 = new System.Windows.Forms.Timer(components);
            mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStaff).BeginInit();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(243, 243, 243);
            mainPanel.Controls.Add(btnSalary);
            mainPanel.Controls.Add(dgvStaff);
            mainPanel.Controls.Add(btnReload);
            mainPanel.Controls.Add(btnEdite);
            mainPanel.Controls.Add(btnDelete);
            mainPanel.Controls.Add(btnAddStaff);
            mainPanel.Controls.Add(txtSearch);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(709, 623);
            mainPanel.TabIndex = 15;
            mainPanel.Paint += mainPanel_Paint;
            // 
            // btnSalary
            // 
            btnSalary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalary.BorderRadius = 7;
            btnSalary.CheckedState.FillColor = Color.FromArgb(50, 55, 89);
            btnSalary.CheckedState.Image = (Image)resources.GetObject("resource.Image");
            btnSalary.CustomizableEdges = customizableEdges1;
            btnSalary.DisabledState.BorderColor = Color.DarkGray;
            btnSalary.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSalary.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSalary.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSalary.Enabled = false;
            btnSalary.FillColor = Color.FromArgb(1, 95, 95);
            btnSalary.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalary.ForeColor = Color.White;
            btnSalary.Image = (Image)resources.GetObject("btnSalary.Image");
            btnSalary.ImageSize = new Size(30, 20);
            btnSalary.Location = new Point(370, 3);
            btnSalary.Name = "btnSalary";
            btnSalary.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnSalary.Size = new Size(60, 50);
            btnSalary.TabIndex = 26;
            btnSalary.TextOffset = new Point(-10, 0);
            btnSalary.Click += brnSalary_Click;
            // 
            // dgvStaff
            // 
            dgvStaff.AllowUserToAddRows = false;
            dgvStaff.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(235, 235, 235);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvStaff.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvStaff.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvStaff.BackgroundColor = Color.FromArgb(243, 243, 243);
            dgvStaff.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvStaff.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvStaff.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvStaff.ColumnHeadersHeight = 35;
            dgvStaff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvStaff.Columns.AddRange(new DataGridViewColumn[] { dgSno, dgvid, dgvName, dgvPhone, dgvRole, dgvSalary, dgvAdvance, dgvTackSalary, dgvSelect });
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle7.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvStaff.DefaultCellStyle = dataGridViewCellStyle7;
            dgvStaff.GridColor = Color.FromArgb(243, 243, 243);
            dgvStaff.Location = new Point(3, 65);
            dgvStaff.Name = "dgvStaff";
            dgvStaff.ReadOnly = true;
            dgvStaff.RightToLeft = RightToLeft.Yes;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Control;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dgvStaff.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dgvStaff.RowHeadersVisible = false;
            dgvStaff.RowHeadersWidth = 51;
            dgvStaff.RowTemplate.Height = 29;
            dgvStaff.Size = new Size(703, 555);
            dgvStaff.TabIndex = 25;
            dgvStaff.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark;
            dgvStaff.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvStaff.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvStaff.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvStaff.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(235, 235, 235);
            dgvStaff.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvStaff.ThemeStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvStaff.ThemeStyle.GridColor = Color.FromArgb(243, 243, 243);
            dgvStaff.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvStaff.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dgvStaff.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvStaff.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvStaff.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvStaff.ThemeStyle.HeaderStyle.Height = 35;
            dgvStaff.ThemeStyle.ReadOnly = true;
            dgvStaff.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvStaff.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvStaff.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvStaff.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvStaff.ThemeStyle.RowsStyle.Height = 29;
            dgvStaff.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(243, 243, 243);
            dgvStaff.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);
            dgvStaff.CellClick += guna2DataGridView1_CellClick;
            dgvStaff.CellDoubleClick += guna2DataGridView1_CellDoubleClick;
            dgvStaff.CellPainting += dgvStaff_CellPainting;
            dgvStaff.CellValueChanged += dgvStaff_CellValueChanged;
            dgvStaff.Scroll += dgvStaff_Scroll;
            // 
            // dgSno
            // 
            dgSno.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgSno.FillWeight = 20F;
            dgSno.Frozen = true;
            dgSno.HeaderText = "#";
            dgSno.MinimumWidth = 30;
            dgSno.Name = "dgSno";
            dgSno.ReadOnly = true;
            dgSno.Width = 41;
            // 
            // dgvid
            // 
            dgvid.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvid.FillWeight = 1F;
            dgvid.HeaderText = "id";
            dgvid.MinimumWidth = 6;
            dgvid.Name = "dgvid";
            dgvid.ReadOnly = true;
            dgvid.Visible = false;
            dgvid.Width = 6;
            // 
            // dgvName
            // 
            dgvName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvName.DefaultCellStyle = dataGridViewCellStyle3;
            dgvName.FillWeight = 120F;
            dgvName.Frozen = true;
            dgvName.HeaderText = "الاسم";
            dgvName.MinimumWidth = 120;
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            dgvName.Width = 120;
            // 
            // dgvPhone
            // 
            dgvPhone.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvPhone.HeaderText = "رقم الهاتف";
            dgvPhone.MinimumWidth = 110;
            dgvPhone.Name = "dgvPhone";
            dgvPhone.ReadOnly = true;
            dgvPhone.Width = 110;
            // 
            // dgvRole
            // 
            dgvRole.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRole.DefaultCellStyle = dataGridViewCellStyle4;
            dgvRole.HeaderText = "الرتبة";
            dgvRole.MinimumWidth = 100;
            dgvRole.Name = "dgvRole";
            dgvRole.ReadOnly = true;
            // 
            // dgvSalary
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSalary.DefaultCellStyle = dataGridViewCellStyle5;
            dgvSalary.HeaderText = "المرتب";
            dgvSalary.MinimumWidth = 100;
            dgvSalary.Name = "dgvSalary";
            dgvSalary.ReadOnly = true;
            // 
            // dgvAdvance
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdvance.DefaultCellStyle = dataGridViewCellStyle6;
            dgvAdvance.HeaderText = "السلفة";
            dgvAdvance.MinimumWidth = 100;
            dgvAdvance.Name = "dgvAdvance";
            dgvAdvance.ReadOnly = true;
            // 
            // dgvTackSalary
            // 
            dgvTackSalary.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvTackSalary.HeaderText = "حالة المرتب";
            dgvTackSalary.MinimumWidth = 100;
            dgvTackSalary.Name = "dgvTackSalary";
            dgvTackSalary.ReadOnly = true;
            dgvTackSalary.Width = 104;
            // 
            // dgvSelect
            // 
            dgvSelect.FillWeight = 32F;
            dgvSelect.HeaderText = "";
            dgvSelect.MinimumWidth = 32;
            dgvSelect.Name = "dgvSelect";
            dgvSelect.ReadOnly = true;
            dgvSelect.Resizable = DataGridViewTriState.False;
            dgvSelect.Visible = false;
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReload.BorderRadius = 7;
            btnReload.CheckedState.FillColor = Color.FromArgb(50, 55, 89);
            btnReload.CheckedState.Image = (Image)resources.GetObject("resource.Image1");
            btnReload.CustomizableEdges = customizableEdges3;
            btnReload.DisabledState.BorderColor = Color.DarkGray;
            btnReload.DisabledState.CustomBorderColor = Color.DarkGray;
            btnReload.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnReload.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnReload.FillColor = Color.FromArgb(136, 214, 218);
            btnReload.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReload.ForeColor = Color.White;
            btnReload.Image = Properties.Resources.refresh_blak;
            btnReload.ImageSize = new Size(30, 20);
            btnReload.Location = new Point(634, 3);
            btnReload.Name = "btnReload";
            btnReload.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnReload.Size = new Size(60, 50);
            btnReload.TabIndex = 24;
            btnReload.TextOffset = new Point(-10, 0);
            btnReload.Click += btnReload_Click;
            // 
            // btnEdite
            // 
            btnEdite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdite.BorderRadius = 7;
            btnEdite.CheckedState.FillColor = Color.FromArgb(50, 55, 89);
            btnEdite.CheckedState.Image = (Image)resources.GetObject("resource.Image2");
            btnEdite.CustomizableEdges = customizableEdges5;
            btnEdite.DisabledState.BorderColor = Color.DarkGray;
            btnEdite.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEdite.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEdite.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEdite.Enabled = false;
            btnEdite.FillColor = Color.FromArgb(136, 214, 218);
            btnEdite.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEdite.ForeColor = Color.White;
            btnEdite.Image = Properties.Resources.edit_light2;
            btnEdite.ImageSize = new Size(30, 20);
            btnEdite.Location = new Point(502, 3);
            btnEdite.Name = "btnEdite";
            btnEdite.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEdite.Size = new Size(60, 50);
            btnEdite.TabIndex = 23;
            btnEdite.TextOffset = new Point(-10, 0);
            btnEdite.Click += guna2Button2_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BorderRadius = 7;
            btnDelete.CheckedState.FillColor = Color.FromArgb(50, 55, 89);
            btnDelete.CheckedState.Image = (Image)resources.GetObject("resource.Image3");
            btnDelete.CustomizableEdges = customizableEdges7;
            btnDelete.DisabledState.BorderColor = Color.DarkGray;
            btnDelete.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDelete.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDelete.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDelete.Enabled = false;
            btnDelete.FillColor = Color.FromArgb(136, 214, 218);
            btnDelete.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.White;
            btnDelete.Image = Properties.Resources.delete_Red;
            btnDelete.ImageSize = new Size(30, 20);
            btnDelete.Location = new Point(436, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnDelete.Size = new Size(60, 50);
            btnDelete.TabIndex = 22;
            btnDelete.TextOffset = new Point(-10, 0);
            btnDelete.Click += btnDelete_Click;
            // 
            // btnAddStaff
            // 
            btnAddStaff.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddStaff.BorderRadius = 7;
            btnAddStaff.CheckedState.FillColor = Color.FromArgb(50, 55, 89);
            btnAddStaff.CheckedState.Image = (Image)resources.GetObject("resource.Image4");
            btnAddStaff.CustomizableEdges = customizableEdges9;
            btnAddStaff.DisabledState.BorderColor = Color.DarkGray;
            btnAddStaff.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAddStaff.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAddStaff.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAddStaff.FillColor = Color.FromArgb(136, 214, 218);
            btnAddStaff.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddStaff.ForeColor = Color.White;
            btnAddStaff.Image = Properties.Resources.add_light3;
            btnAddStaff.ImageSize = new Size(30, 20);
            btnAddStaff.Location = new Point(568, 3);
            btnAddStaff.Name = "btnAddStaff";
            btnAddStaff.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnAddStaff.Size = new Size(60, 50);
            btnAddStaff.TabIndex = 19;
            btnAddStaff.TextOffset = new Point(-10, 0);
            btnAddStaff.Click += btnAddStaff_Click;
            // 
            // txtSearch
            // 
            txtSearch.BorderColor = Color.FromArgb(136, 214, 218);
            txtSearch.BorderRadius = 8;
            txtSearch.CustomizableEdges = customizableEdges11;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.ForeColor = Color.FromArgb(64, 64, 64);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.IconRight = Properties.Resources.searching_light;
            txtSearch.IconRightOffset = new Point(5, 0);
            txtSearch.IconRightSize = new Size(15, 15);
            txtSearch.Location = new Point(17, 8);
            txtSearch.Margin = new Padding(3, 5, 3, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "ابحث هنا";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges12;
            txtSearch.Size = new Size(177, 25);
            txtSearch.TabIndex = 21;
            txtSearch.TextAlign = HorizontalAlignment.Right;
            txtSearch.TextOffset = new Point(2, 0);
            txtSearch.TextChanged += txtSearch_TextChanged_1;
            // 
            // guna2MessageDialog1
            // 
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Caption = null;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            guna2MessageDialog1.Parent = null;
            guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            guna2MessageDialog1.Text = null;
            // 
            // frmStaff
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(709, 623);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmStaff";
            Text = "frmStaff";
            Load += frmStaff_Load;
            mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStaff).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel mainPanel;
        private Guna.UI2.WinForms.Guna2Button btnAddStaff;
        public Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
        private Guna.UI2.WinForms.Guna2Button btnEdite;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private System.Windows.Forms.Timer timer1;
        private Guna.UI2.WinForms.Guna2Button btnReload;
        private Guna.UI2.WinForms.Guna2DataGridView dgvStaff;
        private Guna.UI2.WinForms.Guna2Button btnSalary;
        private DataGridViewTextBoxColumn dgSno;
        private DataGridViewTextBoxColumn dgvid;
        private DataGridViewTextBoxColumn dgvName;
        private DataGridViewTextBoxColumn dgvPhone;
        private DataGridViewTextBoxColumn dgvRole;
        private DataGridViewTextBoxColumn dgvSalary;
        private DataGridViewTextBoxColumn dgvAdvance;
        private DataGridViewTextBoxColumn dgvTackSalary;
        private DataGridViewCheckBoxColumn dgvSelect;
    }
}