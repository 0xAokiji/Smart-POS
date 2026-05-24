namespace pos.Model.Stor
{
    partial class frmSearchProductToAdd
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dgvProducts = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno = new DataGridViewTextBoxColumn();
            dgvpID = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            dgvCode = new DataGridViewTextBoxColumn();
            dgvCodeUse = new DataGridViewTextBoxColumn();
            btnUse = new DataGridViewButtonColumn();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            btnClose = new Guna.UI2.WinForms.Guna2Button();
            cbCategory = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvProducts.BackgroundColor = Color.FromArgb(243, 243, 243);
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(1, 95, 95);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(136, 214, 218);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvProducts.ColumnHeadersHeight = 35;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { dgSno, dgvpID, dgvName, dgvCode, dgvCodeUse, btnUse });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvProducts.DefaultCellStyle = dataGridViewCellStyle4;
            dgvProducts.GridColor = Color.FromArgb(1, 95, 95);
            dgvProducts.Location = new Point(4, 64);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RightToLeft = RightToLeft.Yes;
            dgvProducts.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowHeadersWidth = 51;
            dgvProducts.RowTemplate.Height = 29;
            dgvProducts.ScrollBars = ScrollBars.Vertical;
            dgvProducts.Size = new Size(463, 511);
            dgvProducts.TabIndex = 3;
            dgvProducts.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark;
            dgvProducts.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgvProducts.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;
            dgvProducts.ThemeStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvProducts.ThemeStyle.GridColor = Color.FromArgb(1, 95, 95);
            dgvProducts.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(1, 95, 95);
            dgvProducts.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dgvProducts.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvProducts.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvProducts.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvProducts.ThemeStyle.HeaderStyle.Height = 35;
            dgvProducts.ThemeStyle.ReadOnly = true;
            dgvProducts.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvProducts.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.Single;
            dgvProducts.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvProducts.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvProducts.ThemeStyle.RowsStyle.Height = 29;
            dgvProducts.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgvProducts.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            dgvProducts.CellClick += dgvProducts_CellClick;
            dgvProducts.CellDoubleClick += dgvProducts_CellDoubleClick;
            dgvProducts.CellPainting += dgvProducts_CellPainting;
            dgvProducts.Scroll += dgvProducts_Scroll;
            // 
            // dgSno
            // 
            dgSno.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgSno.DefaultCellStyle = dataGridViewCellStyle3;
            dgSno.FillWeight = 20F;
            dgSno.Frozen = true;
            dgSno.HeaderText = "#";
            dgSno.MinimumWidth = 40;
            dgSno.Name = "dgSno";
            dgSno.ReadOnly = true;
            dgSno.Width = 40;
            // 
            // dgvpID
            // 
            dgvpID.HeaderText = "PID";
            dgvpID.Name = "dgvpID";
            dgvpID.ReadOnly = true;
            dgvpID.Visible = false;
            // 
            // dgvName
            // 
            dgvName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvName.HeaderText = "الاسم المنتج";
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            // 
            // dgvCode
            // 
            dgvCode.HeaderText = "الباركود";
            dgvCode.Name = "dgvCode";
            dgvCode.ReadOnly = true;
            dgvCode.Visible = false;
            // 
            // dgvCodeUse
            // 
            dgvCodeUse.HeaderText = "باركود مستعمل";
            dgvCodeUse.Name = "dgvCodeUse";
            dgvCodeUse.ReadOnly = true;
            dgvCodeUse.Visible = false;
            // 
            // btnUse
            // 
            btnUse.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            btnUse.FillWeight = 150F;
            btnUse.FlatStyle = FlatStyle.Popup;
            btnUse.HeaderText = "مستعمل";
            btnUse.MinimumWidth = 150;
            btnUse.Name = "btnUse";
            btnUse.ReadOnly = true;
            btnUse.Text = "باركود المستعمل";
            btnUse.ToolTipText = "مستعمل";
            btnUse.UseColumnTextForButtonValue = true;
            btnUse.Width = 150;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.BorderColor = Color.FromArgb(1, 95, 95);
            txtSearch.BorderRadius = 8;
            txtSearch.BorderThickness = 2;
            txtSearch.CustomizableEdges = customizableEdges1;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FillColor = Color.FromArgb(243, 243, 243);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtSearch.ForeColor = Color.FromArgb(64, 64, 64);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Location = new Point(222, 23);
            txtSearch.Margin = new Padding(3, 5, 3, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "ابحث بالاسم";
            txtSearch.RightToLeft = RightToLeft.Yes;
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSearch.Size = new Size(237, 33);
            txtSearch.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnClose
            // 
            btnClose.CustomizableEdges = customizableEdges3;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.Red;
            btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(5, 4);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnClose.Size = new Size(30, 20);
            btnClose.TabIndex = 4;
            btnClose.Text = "X";
            btnClose.Click += btnClose_Click;
            // 
            // cbCategory
            // 
            cbCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbCategory.BackColor = Color.Transparent;
            cbCategory.BorderColor = Color.FromArgb(1, 95, 95);
            cbCategory.BorderRadius = 8;
            cbCategory.BorderThickness = 2;
            cbCategory.CustomizableEdges = customizableEdges5;
            cbCategory.DrawMode = DrawMode.OwnerDrawFixed;
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategory.FillColor = Color.FromArgb(243, 243, 243);
            cbCategory.FocusedColor = Color.FromArgb(94, 148, 255);
            cbCategory.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbCategory.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbCategory.ForeColor = Color.FromArgb(68, 88, 112);
            cbCategory.IntegralHeight = false;
            cbCategory.ItemHeight = 30;
            cbCategory.Items.AddRange(new object[] { "عميل", "مورد" });
            cbCategory.Location = new Point(39, 20);
            cbCategory.MaxLength = 120;
            cbCategory.Name = "cbCategory";
            cbCategory.RightToLeft = RightToLeft.Yes;
            cbCategory.ShadowDecoration.CustomizableEdges = customizableEdges6;
            cbCategory.Size = new Size(177, 36);
            cbCategory.StartIndex = 0;
            cbCategory.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            cbCategory.TabIndex = 27;
            cbCategory.TextAlign = HorizontalAlignment.Center;
            cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
            // 
            // frmSearchProductToAdd
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(471, 579);
            Controls.Add(cbCategory);
            Controls.Add(btnClose);
            Controls.Add(dgvProducts);
            Controls.Add(txtSearch);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmSearchProductToAdd";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmSearchProductToAdd";
            Load += frmSearchProductToAdd_Load;
            MouseDown += frmSearchProductToAdd_MouseDown;
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvProducts;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private DataGridViewTextBoxColumn dgSno;
        private DataGridViewTextBoxColumn dgvpID;
        private DataGridViewTextBoxColumn dgvName;
        private DataGridViewTextBoxColumn dgvCode;
        private DataGridViewTextBoxColumn dgvCodeUse;
        private DataGridViewButtonColumn btnUse;
        private Guna.UI2.WinForms.Guna2ComboBox cbCategory;
    }
}