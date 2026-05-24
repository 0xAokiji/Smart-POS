using pos.Classes;

namespace pos.GeneralForms.MainForm
{
    partial class frmPersonalReport
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            mainPanel = new Panel();
            dgvPanel = new Panel();
            dgvDetainls = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno2 = new DataGridViewTextBoxColumn();
            dgvBillNumber = new DataGridViewTextBoxColumn();
            dgvInvoiceCode = new DataGridViewTextBoxColumn();
            dgvTransfareType = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            panel2 = new Panel();
            groupBox2 = new GroupBox();
            btnPartySearch = new Guna.UI2.WinForms.Guna2Button();
            cbChooseParyties = new Guna.UI2.WinForms.Guna2ComboBox();
            btnSearch = new Guna.UI2.WinForms.Guna2Button();
            txtName = new Guna.UI2.WinForms.Guna2TextBox();
            groupBox1 = new GroupBox();
            btnSearchDate = new Guna.UI2.WinForms.Guna2Button();
            dtPickerStart = new Guna.UI2.WinForms.Guna2DateTimePicker();
            label6 = new Label();
            label5 = new Label();
            dtPickerEnd = new Guna.UI2.WinForms.Guna2DateTimePicker();
            btnPrint = new Guna.UI2.WinForms.Guna2Button();
            topPanel = new SmoothPanelTopConrner();
            mainPanel.SuspendLayout();
            dgvPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetainls).BeginInit();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(dgvPanel);
            mainPanel.Controls.Add(panel2);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 50);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1402, 734);
            mainPanel.TabIndex = 9;
            // 
            // dgvPanel
            // 
            dgvPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPanel.Controls.Add(dgvDetainls);
            dgvPanel.Location = new Point(12, 174);
            dgvPanel.Name = "dgvPanel";
            dgvPanel.Size = new Size(1378, 557);
            dgvPanel.TabIndex = 40;
            // 
            // dgvDetainls
            // 
            dgvDetainls.AllowUserToAddRows = false;
            dgvDetainls.AllowUserToDeleteRows = false;
            dgvDetainls.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(1, 95, 95);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(204, 204, 204);
            dgvDetainls.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvDetainls.BackgroundColor = Color.FromArgb(243, 243, 243);
            dgvDetainls.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvDetainls.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(136, 214, 218);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(136, 214, 218);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvDetainls.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvDetainls.ColumnHeadersHeight = 35;
            dgvDetainls.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvDetainls.Columns.AddRange(new DataGridViewColumn[] { dgSno2, dgvBillNumber, dgvInvoiceCode, dgvTransfareType, Column4, Column5, Column6, Column3, dataGridViewTextBoxColumn6 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(1, 95, 95);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(204, 204, 204);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvDetainls.DefaultCellStyle = dataGridViewCellStyle3;
            dgvDetainls.Dock = DockStyle.Fill;
            dgvDetainls.GridColor = Color.FromArgb(136, 214, 218);
            dgvDetainls.Location = new Point(0, 0);
            dgvDetainls.Name = "dgvDetainls";
            dgvDetainls.ReadOnly = true;
            dgvDetainls.RightToLeft = RightToLeft.Yes;
            dgvDetainls.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvDetainls.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvDetainls.RowHeadersVisible = false;
            dgvDetainls.RowHeadersWidth = 51;
            dgvDetainls.RowTemplate.Height = 29;
            dgvDetainls.ScrollBars = ScrollBars.Vertical;
            dgvDetainls.Size = new Size(1378, 557);
            dgvDetainls.TabIndex = 24;
            dgvDetainls.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark;
            dgvDetainls.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvDetainls.ThemeStyle.AlternatingRowsStyle.Font = new Font("Segoe UI", 10.2F);
            dgvDetainls.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvDetainls.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(1, 95, 95);
            dgvDetainls.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.FromArgb(204, 204, 204);
            dgvDetainls.ThemeStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvDetainls.ThemeStyle.GridColor = Color.FromArgb(136, 214, 218);
            dgvDetainls.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(136, 214, 218);
            dgvDetainls.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dgvDetainls.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvDetainls.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvDetainls.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvDetainls.ThemeStyle.HeaderStyle.Height = 35;
            dgvDetainls.ThemeStyle.ReadOnly = true;
            dgvDetainls.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvDetainls.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgvDetainls.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvDetainls.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvDetainls.ThemeStyle.RowsStyle.Height = 29;
            dgvDetainls.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(1, 95, 95);
            dgvDetainls.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(204, 204, 204);
            dgvDetainls.CellDoubleClick += dgvDetainls_CellDoubleClick;
            dgvDetainls.CellFormatting += dgvDetainls_CellFormatting;
            dgvDetainls.CellPainting += dgvDetainls_CellPainting;
            dgvDetainls.Scroll += dgvDetainls_Scroll;
            // 
            // dgSno2
            // 
            dgSno2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgSno2.FillWeight = 20F;
            dgSno2.Frozen = true;
            dgSno2.HeaderText = "#";
            dgSno2.MinimumWidth = 40;
            dgSno2.Name = "dgSno2";
            dgSno2.ReadOnly = true;
            dgSno2.Width = 43;
            // 
            // dgvBillNumber
            // 
            dgvBillNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvBillNumber.FillWeight = 110F;
            dgvBillNumber.HeaderText = "موظف الوردية";
            dgvBillNumber.MinimumWidth = 200;
            dgvBillNumber.Name = "dgvBillNumber";
            dgvBillNumber.ReadOnly = true;
            dgvBillNumber.Width = 200;
            // 
            // dgvInvoiceCode
            // 
            dgvInvoiceCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvInvoiceCode.HeaderText = "كود الفاتورة";
            dgvInvoiceCode.MinimumWidth = 150;
            dgvInvoiceCode.Name = "dgvInvoiceCode";
            dgvInvoiceCode.ReadOnly = true;
            dgvInvoiceCode.Width = 150;
            // 
            // dgvTransfareType
            // 
            dgvTransfareType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTransfareType.HeaderText = "نوع المعاملة";
            dgvTransfareType.MinimumWidth = 150;
            dgvTransfareType.Name = "dgvTransfareType";
            dgvTransfareType.ReadOnly = true;
            dgvTransfareType.Width = 150;
            // 
            // Column4
            // 
            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column4.HeaderText = "ملاحظات المعاملة";
            Column4.MinimumWidth = 300;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Width = 300;
            // 
            // Column5
            // 
            Column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Column5.HeaderText = "رصيد المدين السابق";
            Column5.MinimumWidth = 150;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Width = 158;
            // 
            // Column6
            // 
            Column6.HeaderText = "رصيد المدين الحالي";
            Column6.MinimumWidth = 150;
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column3.HeaderText = "التاريخ";
            Column3.MinimumWidth = 100;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewTextBoxColumn6.HeaderText = "الوقت";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Width = 85;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top;
            panel2.BackColor = Color.FromArgb(243, 243, 243);
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(groupBox1);
            panel2.Location = new Point(12, 6);
            panel2.Name = "panel2";
            panel2.Size = new Size(1378, 162);
            panel2.TabIndex = 39;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnPartySearch);
            groupBox2.Controls.Add(cbChooseParyties);
            groupBox2.Controls.Add(btnSearch);
            groupBox2.Controls.Add(txtName);
            groupBox2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(623, 10);
            groupBox2.Name = "groupBox2";
            groupBox2.RightToLeft = RightToLeft.Yes;
            groupBox2.Size = new Size(666, 143);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "البحث";
            // 
            // btnPartySearch
            // 
            btnPartySearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPartySearch.BorderRadius = 8;
            customizableEdges1.BottomRight = false;
            customizableEdges1.TopRight = false;
            btnPartySearch.CustomizableEdges = customizableEdges1;
            btnPartySearch.DisabledState.BorderColor = Color.DarkGray;
            btnPartySearch.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPartySearch.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPartySearch.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPartySearch.FillColor = Color.FromArgb(1, 95, 95);
            btnPartySearch.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPartySearch.ForeColor = Color.White;
            btnPartySearch.Image = Properties.Resources.magnifying_glass;
            btnPartySearch.ImageSize = new Size(15, 15);
            btnPartySearch.Location = new Point(312, 45);
            btnPartySearch.Name = "btnPartySearch";
            btnPartySearch.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnPartySearch.Size = new Size(44, 36);
            btnPartySearch.TabIndex = 59;
            btnPartySearch.TextAlign = HorizontalAlignment.Left;
            btnPartySearch.Click += btnPartySearch_Click;
            // 
            // cbChooseParyties
            // 
            cbChooseParyties.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbChooseParyties.BackColor = Color.Transparent;
            cbChooseParyties.BorderColor = Color.FromArgb(136, 214, 218);
            cbChooseParyties.BorderRadius = 8;
            cbChooseParyties.CustomizableEdges = customizableEdges3;
            cbChooseParyties.DrawMode = DrawMode.OwnerDrawFixed;
            cbChooseParyties.DropDownStyle = ComboBoxStyle.DropDownList;
            cbChooseParyties.FocusedColor = Color.FromArgb(94, 148, 255);
            cbChooseParyties.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbChooseParyties.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbChooseParyties.ForeColor = Color.FromArgb(68, 88, 112);
            cbChooseParyties.ItemHeight = 30;
            cbChooseParyties.Items.AddRange(new object[] { "عميل", "مورد" });
            cbChooseParyties.Location = new Point(30, 45);
            cbChooseParyties.Name = "cbChooseParyties";
            cbChooseParyties.RightToLeft = RightToLeft.Yes;
            cbChooseParyties.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cbChooseParyties.Size = new Size(267, 36);
            cbChooseParyties.StartIndex = 0;
            cbChooseParyties.TabIndex = 20;
            cbChooseParyties.TextAlign = HorizontalAlignment.Center;
            cbChooseParyties.SelectedIndexChanged += cbPayWay_SelectedIndexChanged;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.BorderRadius = 8;
            btnSearch.CustomizableEdges = customizableEdges5;
            btnSearch.DisabledState.BorderColor = Color.DarkGray;
            btnSearch.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSearch.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSearch.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSearch.FillColor = Color.FromArgb(136, 214, 218);
            btnSearch.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSearch.ForeColor = Color.FromArgb(55, 55, 55);
            btnSearch.Location = new Point(30, 95);
            btnSearch.Name = "btnSearch";
            btnSearch.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnSearch.Size = new Size(606, 28);
            btnSearch.TabIndex = 19;
            btnSearch.Text = "بحث";
            btnSearch.Click += btnSearch_Click;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtName.BorderColor = Color.FromArgb(136, 214, 218);
            txtName.BorderRadius = 8;
            customizableEdges7.BottomLeft = false;
            customizableEdges7.TopLeft = false;
            txtName.CustomizableEdges = customizableEdges7;
            txtName.DefaultText = "";
            txtName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtName.ForeColor = Color.FromArgb(64, 64, 64);
            txtName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Location = new Point(356, 45);
            txtName.Margin = new Padding(3, 5, 3, 5);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "الاسم";
            txtName.RightToLeft = RightToLeft.No;
            txtName.SelectedText = "";
            txtName.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtName.Size = new Size(280, 36);
            txtName.TabIndex = 17;
            txtName.TextAlign = HorizontalAlignment.Right;
            txtName.TextChanged += txtName_TextChanged;
            txtName.KeyDown += txtName_KeyDown;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSearchDate);
            groupBox1.Controls.Add(dtPickerStart);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(dtPickerEnd);
            groupBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(90, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.RightToLeft = RightToLeft.Yes;
            groupBox1.Size = new Size(518, 143);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "الفلاتر";
            // 
            // btnSearchDate
            // 
            btnSearchDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearchDate.BorderRadius = 8;
            btnSearchDate.CustomizableEdges = customizableEdges9;
            btnSearchDate.DisabledState.BorderColor = Color.DarkGray;
            btnSearchDate.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSearchDate.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSearchDate.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSearchDate.FillColor = Color.FromArgb(136, 214, 218);
            btnSearchDate.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSearchDate.ForeColor = Color.FromArgb(55, 55, 55);
            btnSearchDate.Location = new Point(38, 100);
            btnSearchDate.Name = "btnSearchDate";
            btnSearchDate.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnSearchDate.Size = new Size(443, 28);
            btnSearchDate.TabIndex = 60;
            btnSearchDate.Text = "بحث بالتاريخ";
            btnSearchDate.Click += btnSearchDate_Click;
            // 
            // dtPickerStart
            // 
            dtPickerStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtPickerStart.BorderRadius = 8;
            dtPickerStart.Checked = true;
            dtPickerStart.CustomizableEdges = customizableEdges11;
            dtPickerStart.FillColor = Color.FromArgb(1, 95, 95);
            dtPickerStart.Font = new Font("Segoe UI", 11.25F);
            dtPickerStart.ForeColor = Color.FromArgb(204, 204, 204);
            dtPickerStart.Format = DateTimePickerFormat.Long;
            dtPickerStart.Location = new Point(283, 53);
            dtPickerStart.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtPickerStart.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtPickerStart.Name = "dtPickerStart";
            dtPickerStart.ShadowDecoration.CustomizableEdges = customizableEdges12;
            dtPickerStart.Size = new Size(198, 36);
            dtPickerStart.TabIndex = 11;
            dtPickerStart.TextAlign = HorizontalAlignment.Center;
            dtPickerStart.Value = new DateTime(2024, 2, 23, 10, 27, 2, 243);
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(187, 29);
            label6.Name = "label6";
            label6.Size = new Size(49, 21);
            label6.TabIndex = 15;
            label6.Text = "النهاية";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(432, 29);
            label5.Name = "label5";
            label5.Size = new Size(49, 21);
            label5.TabIndex = 14;
            label5.Text = "البداية";
            // 
            // dtPickerEnd
            // 
            dtPickerEnd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtPickerEnd.BorderRadius = 8;
            dtPickerEnd.Checked = true;
            dtPickerEnd.CustomizableEdges = customizableEdges13;
            dtPickerEnd.FillColor = Color.FromArgb(1, 95, 95);
            dtPickerEnd.Font = new Font("Segoe UI", 11.25F);
            dtPickerEnd.ForeColor = Color.FromArgb(204, 204, 204);
            dtPickerEnd.Format = DateTimePickerFormat.Long;
            dtPickerEnd.Location = new Point(38, 53);
            dtPickerEnd.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtPickerEnd.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtPickerEnd.Name = "dtPickerEnd";
            dtPickerEnd.ShadowDecoration.CustomizableEdges = customizableEdges14;
            dtPickerEnd.Size = new Size(198, 36);
            dtPickerEnd.TabIndex = 13;
            dtPickerEnd.TextAlign = HorizontalAlignment.Center;
            dtPickerEnd.Value = new DateTime(2024, 2, 23, 10, 27, 2, 243);
            // 
            // btnPrint
            // 
            btnPrint.BorderRadius = 8;
            btnPrint.CustomizableEdges = customizableEdges15;
            btnPrint.DisabledState.BorderColor = Color.DarkGray;
            btnPrint.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPrint.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPrint.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPrint.Enabled = false;
            btnPrint.FillColor = Color.FromArgb(0, 152, 121);
            btnPrint.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnPrint.ForeColor = Color.FromArgb(204, 204, 204);
            btnPrint.Location = new Point(10, 4);
            btnPrint.Name = "btnPrint";
            btnPrint.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnPrint.Size = new Size(169, 41);
            btnPrint.TabIndex = 60;
            btnPrint.Text = "طباعة التقرير الحالي";
            btnPrint.Click += btnPrint_Click;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(1, 95, 95);
            topPanel.BorderColor = Color.FromArgb(1, 95, 95);
            topPanel.BorderSize = 1F;
            topPanel.Controls.Add(btnPrint);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1402, 50);
            topPanel.TabIndex = 7;
            // 
            // frmPersonalReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1402, 784);
            Controls.Add(mainPanel);
            Controls.Add(topPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmPersonalReport";
            Text = "frmPersonalReport";
            Load += frmPersonalReport_Load;
            SizeChanged += frmPersonalReport_SizeChanged;
            Resize += frmPersonalReport_Resize;
            mainPanel.ResumeLayout(false);
            dgvPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDetainls).EndInit();
            panel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            topPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel mainPanel;
        private Panel panel2;
        private GroupBox groupBox1;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtPickerStart;
        private Label label6;
        private Label label5;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtPickerEnd;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDetainls;
        private GroupBox groupBox2;
        public Guna.UI2.WinForms.Guna2Button btnSearch;
        public Guna.UI2.WinForms.Guna2Button btnPartySearch;
        public Guna.UI2.WinForms.Guna2Button btnPrint;
        private DataGridViewTextBoxColumn dgSno2;
        private DataGridViewTextBoxColumn dgvBillNumber;
        private DataGridViewTextBoxColumn dgvInvoiceCode;
        private DataGridViewTextBoxColumn dgvTransfareType;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private SmoothPanelTopConrner topPanel;
        public Panel dgvPanel;
        public Guna.UI2.WinForms.Guna2Button btnSearchDate;
        public Guna.UI2.WinForms.Guna2TextBox txtName;
        public Guna.UI2.WinForms.Guna2ComboBox cbChooseParyties;
    }
}