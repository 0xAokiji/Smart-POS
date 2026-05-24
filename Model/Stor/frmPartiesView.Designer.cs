namespace pos.Model.Stor
{
    partial class frmPartiesView
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            topPanel = new Panel();
            cbChooseParyties = new Guna.UI2.WinForms.Guna2ComboBox();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            dgvCategory = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno2 = new DataGridViewTextBoxColumn();
            dgvId = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            dgvParties = new DataGridViewTextBoxColumn();
            dgvAddress = new DataGridViewTextBoxColumn();
            dgvPhone1 = new DataGridViewTextBoxColumn();
            dgvPhone2 = new DataGridViewTextBoxColumn();
            dgvCode = new DataGridViewTextBoxColumn();
            dgvBalance = new DataGridViewTextBoxColumn();
            dgvEdite = new DataGridViewImageColumn();
            dgvDelete = new DataGridViewImageColumn();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategory).BeginInit();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(230, 230, 230);
            topPanel.Controls.Add(cbChooseParyties);
            topPanel.Controls.Add(txtSearch);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1168, 58);
            topPanel.TabIndex = 8;
            // 
            // cbChooseParyties
            // 
            cbChooseParyties.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbChooseParyties.BackColor = Color.FromArgb(230, 230, 230);
            cbChooseParyties.BorderColor = Color.FromArgb(136, 214, 218);
            cbChooseParyties.BorderRadius = 8;
            cbChooseParyties.CustomizableEdges = customizableEdges1;
            cbChooseParyties.DrawMode = DrawMode.OwnerDrawFixed;
            cbChooseParyties.DropDownStyle = ComboBoxStyle.DropDownList;
            cbChooseParyties.FocusedColor = Color.FromArgb(94, 148, 255);
            cbChooseParyties.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbChooseParyties.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbChooseParyties.ForeColor = Color.FromArgb(68, 88, 112);
            cbChooseParyties.ItemHeight = 30;
            cbChooseParyties.Items.AddRange(new object[] { "عميل", "مورد" });
            cbChooseParyties.Location = new Point(276, 10);
            cbChooseParyties.MaxLength = 80;
            cbChooseParyties.Name = "cbChooseParyties";
            cbChooseParyties.RightToLeft = RightToLeft.Yes;
            cbChooseParyties.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cbChooseParyties.Size = new Size(173, 36);
            cbChooseParyties.StartIndex = 0;
            cbChooseParyties.TabIndex = 27;
            cbChooseParyties.TextAlign = HorizontalAlignment.Center;
            cbChooseParyties.SelectedIndexChanged += cbChooseParyties_SelectedIndexChanged;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearch.BorderColor = Color.FromArgb(136, 214, 218);
            txtSearch.BorderRadius = 8;
            txtSearch.CustomizableEdges = customizableEdges3;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtSearch.ForeColor = Color.FromArgb(64, 64, 64);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.IconRight = Properties.Resources.search_Dark;
            txtSearch.IconRightOffset = new Point(5, 0);
            txtSearch.Location = new Point(456, 11);
            txtSearch.Margin = new Padding(4, 7, 4, 7);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "ابحث هنا";
            txtSearch.RightToLeft = RightToLeft.Yes;
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtSearch.Size = new Size(257, 35);
            txtSearch.TabIndex = 10;
            txtSearch.TextOffset = new Point(5, 0);
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvCategory
            // 
            dgvCategory.AllowUserToAddRows = false;
            dgvCategory.AllowUserToDeleteRows = false;
            dgvCategory.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(230, 230, 230);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvCategory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCategory.BackgroundColor = Color.FromArgb(243, 243, 243);
            dgvCategory.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvCategory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(1, 95, 95);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(136, 214, 218);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCategory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCategory.ColumnHeadersHeight = 35;
            dgvCategory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvCategory.Columns.AddRange(new DataGridViewColumn[] { dgSno2, dgvId, dgvName, dgvParties, dgvAddress, dgvPhone1, dgvPhone2, dgvCode, dgvBalance, dgvEdite, dgvDelete });
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle8.SelectionForeColor = Color.White;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvCategory.DefaultCellStyle = dataGridViewCellStyle8;
            dgvCategory.Dock = DockStyle.Fill;
            dgvCategory.GridColor = Color.FromArgb(1, 95, 95);
            dgvCategory.Location = new Point(0, 58);
            dgvCategory.Name = "dgvCategory";
            dgvCategory.ReadOnly = true;
            dgvCategory.RightToLeft = RightToLeft.Yes;
            dgvCategory.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvCategory.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvCategory.RowHeadersVisible = false;
            dgvCategory.RowHeadersWidth = 51;
            dgvCategory.RowTemplate.Height = 29;
            dgvCategory.ScrollBars = ScrollBars.Vertical;
            dgvCategory.Size = new Size(1168, 392);
            dgvCategory.TabIndex = 24;
            dgvCategory.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark;
            dgvCategory.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvCategory.ThemeStyle.AlternatingRowsStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvCategory.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvCategory.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgvCategory.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;
            dgvCategory.ThemeStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvCategory.ThemeStyle.GridColor = Color.FromArgb(1, 95, 95);
            dgvCategory.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(1, 95, 95);
            dgvCategory.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dgvCategory.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvCategory.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvCategory.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvCategory.ThemeStyle.HeaderStyle.Height = 35;
            dgvCategory.ThemeStyle.ReadOnly = true;
            dgvCategory.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvCategory.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.Single;
            dgvCategory.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvCategory.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvCategory.ThemeStyle.RowsStyle.Height = 29;
            dgvCategory.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgvCategory.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            dgvCategory.CellDoubleClick += dgvCategory_CellDoubleClick;
            dgvCategory.CellFormatting += dgvCategory_CellFormatting;
            dgvCategory.Scroll += dgvCategory_Scroll;
            // 
            // dgSno2
            // 
            dgSno2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgSno2.FillWeight = 20F;
            dgSno2.Frozen = true;
            dgSno2.HeaderText = "#";
            dgSno2.MinimumWidth = 30;
            dgSno2.Name = "dgSno2";
            dgSno2.ReadOnly = true;
            dgSno2.Width = 40;
            // 
            // dgvId
            // 
            dgvId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvId.FillWeight = 1F;
            dgvId.HeaderText = "Id";
            dgvId.MinimumWidth = 6;
            dgvId.Name = "dgvId";
            dgvId.ReadOnly = true;
            dgvId.Visible = false;
            dgvId.Width = 6;
            // 
            // dgvName
            // 
            dgvName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvName.Frozen = true;
            dgvName.HeaderText = "الاسم";
            dgvName.MinimumWidth = 300;
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            dgvName.Width = 300;
            // 
            // dgvParties
            // 
            dgvParties.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvParties.DefaultCellStyle = dataGridViewCellStyle3;
            dgvParties.Frozen = true;
            dgvParties.HeaderText = "الحالة";
            dgvParties.Name = "dgvParties";
            dgvParties.ReadOnly = true;
            dgvParties.Width = 80;
            // 
            // dgvAddress
            // 
            dgvAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvAddress.DefaultCellStyle = dataGridViewCellStyle4;
            dgvAddress.Frozen = true;
            dgvAddress.HeaderText = "العنوان";
            dgvAddress.MinimumWidth = 300;
            dgvAddress.Name = "dgvAddress";
            dgvAddress.ReadOnly = true;
            dgvAddress.Width = 300;
            // 
            // dgvPhone1
            // 
            dgvPhone1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPhone1.DefaultCellStyle = dataGridViewCellStyle5;
            dgvPhone1.Frozen = true;
            dgvPhone1.HeaderText = "رقم الهاتف 1";
            dgvPhone1.MinimumWidth = 160;
            dgvPhone1.Name = "dgvPhone1";
            dgvPhone1.ReadOnly = true;
            dgvPhone1.Width = 160;
            // 
            // dgvPhone2
            // 
            dgvPhone2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPhone2.DefaultCellStyle = dataGridViewCellStyle6;
            dgvPhone2.Frozen = true;
            dgvPhone2.HeaderText = "رقم الهاتف 2";
            dgvPhone2.MinimumWidth = 160;
            dgvPhone2.Name = "dgvPhone2";
            dgvPhone2.ReadOnly = true;
            dgvPhone2.Width = 160;
            // 
            // dgvCode
            // 
            dgvCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCode.DefaultCellStyle = dataGridViewCellStyle7;
            dgvCode.Frozen = true;
            dgvCode.HeaderText = "كود المورد";
            dgvCode.Name = "dgvCode";
            dgvCode.ReadOnly = true;
            dgvCode.Width = 120;
            // 
            // dgvBalance
            // 
            dgvBalance.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvBalance.HeaderText = "رصيد المدين الحالي";
            dgvBalance.Name = "dgvBalance";
            dgvBalance.ReadOnly = true;
            // 
            // dgvEdite
            // 
            dgvEdite.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvEdite.HeaderText = "";
            dgvEdite.Image = Properties.Resources.edit_text;
            dgvEdite.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvEdite.MinimumWidth = 50;
            dgvEdite.Name = "dgvEdite";
            dgvEdite.ReadOnly = true;
            dgvEdite.Width = 50;
            // 
            // dgvDelete
            // 
            dgvDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvDelete.HeaderText = "";
            dgvDelete.Image = Properties.Resources.delete_Red;
            dgvDelete.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvDelete.MinimumWidth = 50;
            dgvDelete.Name = "dgvDelete";
            dgvDelete.ReadOnly = true;
            dgvDelete.Resizable = DataGridViewTriState.True;
            dgvDelete.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvDelete.Width = 50;
            // 
            // frmPartiesView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1168, 450);
            Controls.Add(dgvCategory);
            Controls.Add(topPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmPartiesView";
            Text = "frmPartiesView";
            Load += frmPartiesView_Load;
            SizeChanged += frmPartiesView_SizeChanged;
            topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategory).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel topPanel;
        public Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2DataGridView dgvCategory;
        private Guna.UI2.WinForms.Guna2ComboBox cbChooseParyties;
        private DataGridViewTextBoxColumn dgSno2;
        private DataGridViewTextBoxColumn dgvId;
        private DataGridViewTextBoxColumn dgvName;
        private DataGridViewTextBoxColumn dgvParties;
        private DataGridViewTextBoxColumn dgvAddress;
        private DataGridViewTextBoxColumn dgvPhone1;
        private DataGridViewTextBoxColumn dgvPhone2;
        private DataGridViewTextBoxColumn dgvCode;
        private DataGridViewTextBoxColumn dgvBalance;
        private DataGridViewImageColumn dgvEdite;
        private DataGridViewImageColumn dgvDelete;
    }
}