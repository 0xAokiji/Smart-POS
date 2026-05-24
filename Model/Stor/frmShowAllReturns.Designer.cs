namespace pos.Model.Stor
{
    partial class frmShowAllReturns
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            topPanel = new Panel();
            contenerPanel = new Panel();
            titelPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            lblTitel = new Label();
            dtPickerEnd = new Guna.UI2.WinForms.Guna2DateTimePicker();
            btnSearchParties = new Guna.UI2.WinForms.Guna2Button();
            txtName = new Guna.UI2.WinForms.Guna2TextBox();
            btnSearchDate = new Guna.UI2.WinForms.Guna2Button();
            btnSearch = new Guna.UI2.WinForms.Guna2Button();
            label6 = new Label();
            dtPickerStart = new Guna.UI2.WinForms.Guna2DateTimePicker();
            label5 = new Label();
            panel1 = new Panel();
            dgvProducts = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno = new DataGridViewTextBoxColumn();
            dgvpMainID = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            dgvStatus = new DataGridViewTextBoxColumn();
            dgvCategory = new DataGridViewTextBoxColumn();
            dgvUnit = new DataGridViewTextBoxColumn();
            dgvQty = new DataGridViewTextBoxColumn();
            dgvPrice = new DataGridViewTextBoxColumn();
            dgvTotal = new DataGridViewTextBoxColumn();
            dgvDate = new DataGridViewTextBoxColumn();
            dgvTime = new DataGridViewTextBoxColumn();
            dgvShift = new DataGridViewTextBoxColumn();
            topPanel.SuspendLayout();
            contenerPanel.SuspendLayout();
            titelPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.Controls.Add(contenerPanel);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(995, 143);
            topPanel.TabIndex = 55;
            topPanel.Resize += topPanel_Resize;
            // 
            // contenerPanel
            // 
            contenerPanel.Controls.Add(titelPanel);
            contenerPanel.Controls.Add(dtPickerEnd);
            contenerPanel.Controls.Add(btnSearchParties);
            contenerPanel.Controls.Add(txtName);
            contenerPanel.Controls.Add(btnSearchDate);
            contenerPanel.Controls.Add(btnSearch);
            contenerPanel.Controls.Add(label6);
            contenerPanel.Controls.Add(dtPickerStart);
            contenerPanel.Controls.Add(label5);
            contenerPanel.Location = new Point(129, 3);
            contenerPanel.Name = "contenerPanel";
            contenerPanel.Size = new Size(793, 137);
            contenerPanel.TabIndex = 69;
            // 
            // titelPanel
            // 
            titelPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            titelPanel.BackColor = Color.Transparent;
            titelPanel.Controls.Add(lblTitel);
            titelPanel.FillColor = Color.FromArgb(1, 95, 95);
            titelPanel.ForeColor = Color.White;
            titelPanel.Location = new Point(242, 0);
            titelPanel.Name = "titelPanel";
            titelPanel.Radius = 5;
            titelPanel.ShadowColor = Color.Black;
            titelPanel.ShadowShift = 7;
            titelPanel.Size = new Size(309, 41);
            titelPanel.TabIndex = 69;
            // 
            // lblTitel
            // 
            lblTitel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTitel.AutoSize = true;
            lblTitel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitel.ForeColor = Color.White;
            lblTitel.Location = new Point(97, 10);
            lblTitel.Name = "lblTitel";
            lblTitel.Size = new Size(114, 21);
            lblTitel.TabIndex = 31;
            lblTitel.Text = "مرتجعات العميل";
            // 
            // dtPickerEnd
            // 
            dtPickerEnd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtPickerEnd.BorderRadius = 8;
            dtPickerEnd.Checked = true;
            dtPickerEnd.CustomizableEdges = customizableEdges1;
            dtPickerEnd.FillColor = Color.FromArgb(1, 95, 95);
            dtPickerEnd.Font = new Font("Segoe UI", 9F);
            dtPickerEnd.ForeColor = Color.FromArgb(204, 204, 204);
            dtPickerEnd.Format = DateTimePickerFormat.Long;
            dtPickerEnd.Location = new Point(43, 61);
            dtPickerEnd.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtPickerEnd.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtPickerEnd.Name = "dtPickerEnd";
            dtPickerEnd.ShadowDecoration.CustomizableEdges = customizableEdges2;
            dtPickerEnd.Size = new Size(198, 30);
            dtPickerEnd.TabIndex = 64;
            dtPickerEnd.Value = new DateTime(2024, 2, 23, 10, 27, 2, 243);
            // 
            // btnSearchParties
            // 
            btnSearchParties.BorderRadius = 8;
            customizableEdges3.BottomRight = false;
            customizableEdges3.TopRight = false;
            btnSearchParties.CustomizableEdges = customizableEdges3;
            btnSearchParties.DisabledState.BorderColor = Color.DarkGray;
            btnSearchParties.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSearchParties.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSearchParties.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSearchParties.FillColor = Color.FromArgb(1, 95, 95);
            btnSearchParties.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSearchParties.ForeColor = Color.White;
            btnSearchParties.Image = Properties.Resources.magnifying_glass;
            btnSearchParties.ImageSize = new Size(15, 15);
            btnSearchParties.Location = new Point(477, 61);
            btnSearchParties.Name = "btnSearchParties";
            btnSearchParties.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSearchParties.Size = new Size(49, 30);
            btnSearchParties.TabIndex = 68;
            btnSearchParties.TextAlign = HorizontalAlignment.Left;
            btnSearchParties.Click += btnSearchParties_Click;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtName.BorderColor = Color.FromArgb(136, 214, 218);
            txtName.BorderRadius = 8;
            customizableEdges5.BottomLeft = false;
            customizableEdges5.TopLeft = false;
            txtName.CustomizableEdges = customizableEdges5;
            txtName.DefaultText = "";
            txtName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtName.ForeColor = Color.FromArgb(64, 64, 64);
            txtName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Location = new Point(526, 61);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "الاسم";
            txtName.RightToLeft = RightToLeft.No;
            txtName.SelectedText = "";
            txtName.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtName.Size = new Size(224, 30);
            txtName.TabIndex = 60;
            txtName.TextAlign = HorizontalAlignment.Right;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // btnSearchDate
            // 
            btnSearchDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearchDate.BorderRadius = 8;
            btnSearchDate.CustomizableEdges = customizableEdges7;
            btnSearchDate.DisabledState.BorderColor = Color.DarkGray;
            btnSearchDate.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSearchDate.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSearchDate.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSearchDate.FillColor = Color.FromArgb(136, 214, 218);
            btnSearchDate.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSearchDate.ForeColor = Color.FromArgb(55, 55, 55);
            btnSearchDate.Location = new Point(43, 96);
            btnSearchDate.Name = "btnSearchDate";
            btnSearchDate.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnSearchDate.Size = new Size(402, 28);
            btnSearchDate.TabIndex = 67;
            btnSearchDate.Text = "بحث في فتره معينة";
            btnSearchDate.Click += btnSearchDate_Click;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.BorderRadius = 8;
            btnSearch.CustomizableEdges = customizableEdges9;
            btnSearch.DisabledState.BorderColor = Color.DarkGray;
            btnSearch.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSearch.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSearch.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSearch.FillColor = Color.FromArgb(136, 214, 218);
            btnSearch.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSearch.ForeColor = Color.FromArgb(55, 55, 55);
            btnSearch.Location = new Point(477, 96);
            btnSearch.Name = "btnSearch";
            btnSearch.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnSearch.Size = new Size(273, 28);
            btnSearch.TabIndex = 61;
            btnSearch.Text = "بحث";
            btnSearch.Click += btnSearch_Click;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(199, 41);
            label6.Name = "label6";
            label6.Size = new Size(37, 15);
            label6.TabIndex = 66;
            label6.Text = "النهاية";
            // 
            // dtPickerStart
            // 
            dtPickerStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtPickerStart.BorderRadius = 8;
            dtPickerStart.Checked = true;
            dtPickerStart.CustomizableEdges = customizableEdges11;
            dtPickerStart.FillColor = Color.FromArgb(1, 95, 95);
            dtPickerStart.Font = new Font("Segoe UI", 9F);
            dtPickerStart.ForeColor = Color.FromArgb(204, 204, 204);
            dtPickerStart.Format = DateTimePickerFormat.Long;
            dtPickerStart.Location = new Point(247, 61);
            dtPickerStart.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtPickerStart.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtPickerStart.Name = "dtPickerStart";
            dtPickerStart.ShadowDecoration.CustomizableEdges = customizableEdges12;
            dtPickerStart.Size = new Size(198, 30);
            dtPickerStart.TabIndex = 63;
            dtPickerStart.Value = new DateTime(2024, 2, 23, 10, 27, 2, 243);
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(403, 44);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 65;
            label5.Text = "البداية";
            // 
            // panel1
            // 
            panel1.Controls.Add(dgvProducts);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 143);
            panel1.Name = "panel1";
            panel1.Size = new Size(995, 307);
            panel1.TabIndex = 56;
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvProducts.BackgroundColor = Color.FromArgb(243, 243, 243);
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(1, 95, 95);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(136, 214, 218);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvProducts.ColumnHeadersHeight = 35;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { dgSno, dgvpMainID, dgvName, dgvStatus, dgvCategory, dgvUnit, dgvQty, dgvPrice, dgvTotal, dgvDate, dgvTime, dgvShift });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvProducts.DefaultCellStyle = dataGridViewCellStyle3;
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.GridColor = Color.FromArgb(1, 95, 95);
            dgvProducts.Location = new Point(0, 0);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.RightToLeft = RightToLeft.Yes;
            dgvProducts.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowHeadersWidth = 51;
            dgvProducts.RowTemplate.Height = 29;
            dgvProducts.ScrollBars = ScrollBars.Vertical;
            dgvProducts.Size = new Size(995, 307);
            dgvProducts.TabIndex = 22;
            dgvProducts.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark;
            dgvProducts.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.Font = new Font("Segoe UI", 10.2F);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;
            dgvProducts.ThemeStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvProducts.ThemeStyle.GridColor = Color.FromArgb(1, 95, 95);
            dgvProducts.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(1, 95, 95);
            dgvProducts.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dgvProducts.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvProducts.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvProducts.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvProducts.ThemeStyle.HeaderStyle.Height = 35;
            dgvProducts.ThemeStyle.ReadOnly = false;
            dgvProducts.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvProducts.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.Single;
            dgvProducts.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvProducts.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvProducts.ThemeStyle.RowsStyle.Height = 29;
            dgvProducts.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgvProducts.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            dgvProducts.Scroll += dgvProducts_Scroll;
            // 
            // dgSno
            // 
            dgSno.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgSno.FillWeight = 20F;
            dgSno.Frozen = true;
            dgSno.HeaderText = "#";
            dgSno.MinimumWidth = 20;
            dgSno.Name = "dgSno";
            dgSno.ReadOnly = true;
            dgSno.Width = 43;
            // 
            // dgvpMainID
            // 
            dgvpMainID.HeaderText = "mainID";
            dgvpMainID.Name = "dgvpMainID";
            dgvpMainID.ReadOnly = true;
            dgvpMainID.Visible = false;
            // 
            // dgvName
            // 
            dgvName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvName.FillWeight = 200F;
            dgvName.HeaderText = "المنتج";
            dgvName.MinimumWidth = 200;
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            dgvName.Width = 200;
            // 
            // dgvStatus
            // 
            dgvStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvStatus.HeaderText = "الحالية";
            dgvStatus.MinimumWidth = 60;
            dgvStatus.Name = "dgvStatus";
            dgvStatus.ReadOnly = true;
            dgvStatus.Width = 74;
            // 
            // dgvCategory
            // 
            dgvCategory.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvCategory.HeaderText = "الصنف";
            dgvCategory.MinimumWidth = 70;
            dgvCategory.Name = "dgvCategory";
            dgvCategory.ReadOnly = true;
            dgvCategory.Width = 78;
            // 
            // dgvUnit
            // 
            dgvUnit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvUnit.HeaderText = "الوحدة";
            dgvUnit.Name = "dgvUnit";
            dgvUnit.ReadOnly = true;
            dgvUnit.Width = 76;
            // 
            // dgvQty
            // 
            dgvQty.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvQty.FillWeight = 110F;
            dgvQty.HeaderText = "الكمية";
            dgvQty.MinimumWidth = 70;
            dgvQty.Name = "dgvQty";
            dgvQty.ReadOnly = true;
            dgvQty.Width = 73;
            // 
            // dgvPrice
            // 
            dgvPrice.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvPrice.HeaderText = "السعر";
            dgvPrice.MinimumWidth = 80;
            dgvPrice.Name = "dgvPrice";
            dgvPrice.ReadOnly = true;
            dgvPrice.Width = 80;
            // 
            // dgvTotal
            // 
            dgvTotal.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTotal.HeaderText = "الاجمالي";
            dgvTotal.Name = "dgvTotal";
            dgvTotal.ReadOnly = true;
            dgvTotal.Width = 87;
            // 
            // dgvDate
            // 
            dgvDate.HeaderText = "التاريخ";
            dgvDate.Name = "dgvDate";
            // 
            // dgvTime
            // 
            dgvTime.HeaderText = "الوقت";
            dgvTime.Name = "dgvTime";
            // 
            // dgvShift
            // 
            dgvShift.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvShift.HeaderText = "موظف الشيفت";
            dgvShift.Name = "dgvShift";
            // 
            // frmShowAllReturns
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(995, 450);
            Controls.Add(panel1);
            Controls.Add(topPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmShowAllReturns";
            Text = "frmShowAllReturns";
            Load += frmShowAllReturns_Load;
            topPanel.ResumeLayout(false);
            contenerPanel.ResumeLayout(false);
            contenerPanel.PerformLayout();
            titelPanel.ResumeLayout(false);
            titelPanel.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel topPanel;
        private Panel panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvProducts;
        public Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2TextBox txtName;
        private Label label6;
        private Label label5;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtPickerEnd;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtPickerStart;
        public Guna.UI2.WinForms.Guna2Button btnSearchDate;
        public Guna.UI2.WinForms.Guna2Button btnSearchParties;
        private Panel contenerPanel;
        private DataGridViewTextBoxColumn dgSno;
        private DataGridViewTextBoxColumn dgvpMainID;
        private DataGridViewTextBoxColumn dgvName;
        private DataGridViewTextBoxColumn dgvStatus;
        private DataGridViewTextBoxColumn dgvCategory;
        private DataGridViewTextBoxColumn dgvUnit;
        private DataGridViewTextBoxColumn dgvQty;
        private DataGridViewTextBoxColumn dgvPrice;
        private DataGridViewTextBoxColumn dgvTotal;
        private DataGridViewTextBoxColumn dgvDate;
        private DataGridViewTextBoxColumn dgvTime;
        private DataGridViewTextBoxColumn dgvShift;
        private Guna.UI2.WinForms.Guna2ShadowPanel titelPanel;
        public Label lblTitel;
    }
}