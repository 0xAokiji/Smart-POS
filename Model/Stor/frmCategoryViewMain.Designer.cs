namespace pos.Model.Stor
{
    partial class frmCategoryViewMain
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            topPanel = new Panel();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            panel1 = new Panel();
            dgvCategory = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno2 = new DataGridViewTextBoxColumn();
            dgvId = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            topPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCategory).BeginInit();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(243, 243, 243);
            topPanel.Controls.Add(txtSearch);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(800, 47);
            topPanel.TabIndex = 7;
            topPanel.SizeChanged += topPanel_SizeChanged;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearch.BorderColor = Color.FromArgb(136, 214, 218);
            txtSearch.BorderRadius = 8;
            txtSearch.CustomizableEdges = customizableEdges1;
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
            txtSearch.Location = new Point(272, 6);
            txtSearch.Margin = new Padding(4, 7, 4, 7);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "ابحث هنا";
            txtSearch.RightToLeft = RightToLeft.Yes;
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSearch.Size = new Size(257, 35);
            txtSearch.TabIndex = 10;
            txtSearch.TextOffset = new Point(5, 0);
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(243, 243, 243);
            panel1.Controls.Add(dgvCategory);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 47);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 403);
            panel1.TabIndex = 8;
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
            dgvCategory.Columns.AddRange(new DataGridViewColumn[] { dgSno2, dgvId, dgvName });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCategory.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCategory.Dock = DockStyle.Fill;
            dgvCategory.GridColor = Color.FromArgb(1, 95, 95);
            dgvCategory.Location = new Point(0, 0);
            dgvCategory.Name = "dgvCategory";
            dgvCategory.ReadOnly = true;
            dgvCategory.RightToLeft = RightToLeft.Yes;
            dgvCategory.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvCategory.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvCategory.RowHeadersVisible = false;
            dgvCategory.RowHeadersWidth = 51;
            dgvCategory.RowTemplate.Height = 29;
            dgvCategory.ScrollBars = ScrollBars.Vertical;
            dgvCategory.Size = new Size(800, 403);
            dgvCategory.TabIndex = 23;
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
            dgvCategory.Scroll += dgvCategory_Scroll;
            // 
            // dgSno2
            // 
            dgSno2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgSno2.FillWeight = 20F;
            dgSno2.Frozen = true;
            dgSno2.HeaderText = "#";
            dgSno2.MinimumWidth = 40;
            dgSno2.Name = "dgSno2";
            dgSno2.ReadOnly = true;
            dgSno2.Width = 40;
            // 
            // dgvId
            // 
            dgvId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvId.FillWeight = 1F;
            dgvId.HeaderText = "bill_Id";
            dgvId.MinimumWidth = 6;
            dgvId.Name = "dgvId";
            dgvId.ReadOnly = true;
            dgvId.Visible = false;
            dgvId.Width = 6;
            // 
            // dgvName
            // 
            dgvName.HeaderText = "الاسم";
            dgvName.MinimumWidth = 150;
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            // 
            // frmCategoryViewMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(topPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmCategoryViewMain";
            Text = "frmCategoryViewMain";
            Load += frmCategoryViewMain_Load;
            topPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCategory).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel topPanel;
        public Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Panel panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvCategory;
        private DataGridViewTextBoxColumn dgSno2;
        private DataGridViewTextBoxColumn dgvId;
        private DataGridViewTextBoxColumn dgvName;
    }
}