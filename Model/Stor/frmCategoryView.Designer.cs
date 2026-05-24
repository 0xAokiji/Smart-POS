namespace pos.View
{
    partial class frmCategoryView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCategoryView));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2DataGridView1 = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno = new DataGridViewTextBoxColumn();
            dgvid = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            topPanel = new Panel();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView1).BeginInit();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // txtSearch1
            // 
            resources.ApplyResources(txtSearch1, "txtSearch1");
            txtSearch1.AutoRoundedCorners = false;
            txtSearch1.BorderColor = Color.FromArgb(1, 95, 95);
            txtSearch1.BorderRadius = 8;
            txtSearch1.BorderThickness = 2;
            txtSearch1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch1.ForeColor = Color.FromArgb(64, 64, 64);
            txtSearch1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch1.IconRight = Properties.Resources.search_Dark;
            txtSearch1.IconRightOffset = new Point(5, 0);
            txtSearch1.ShadowDecoration.CustomizableEdges = customizableEdges1;
            txtSearch1.TextOffset = new Point(2, 0);
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            // 
            // btnAdd
            // 
            resources.ApplyResources(btnAdd, "btnAdd");
            btnAdd.CheckedState.ImageSize = new Size(64, 64);
            btnAdd.DialogResult = DialogResult.None;
            btnAdd.HoverState.ImageSize = new Size(64, 64);
            btnAdd.ImageFlip = Guna.UI2.WinForms.Enums.FlipOrientation.Normal;
            btnAdd.PressedState.ImageSize = new Size(64, 64);
            btnAdd.ShadowDecoration.CustomizableEdges = customizableEdges2;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            // 
            // guna2Separator1
            // 
            resources.ApplyResources(guna2Separator1, "guna2Separator1");
            // 
            // guna2MessageDialog1
            // 
            guna2MessageDialog1.Parent = null;
            // 
            // guna2DataGridView1
            // 
            resources.ApplyResources(guna2DataGridView1, "guna2DataGridView1");
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView1.AllowUserToDeleteRows = false;
            guna2DataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            guna2DataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            guna2DataGridView1.BackgroundColor = SystemColors.ButtonFace;
            guna2DataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            guna2DataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(232, 234, 237);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            guna2DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            guna2DataGridView1.Columns.AddRange(new DataGridViewColumn[] { dgSno, dgvid, dgvName });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            guna2DataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            guna2DataGridView1.GridColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView1.Name = "guna2DataGridView1";
            guna2DataGridView1.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            guna2DataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            guna2DataGridView1.RowHeadersVisible = false;
            guna2DataGridView1.RowTemplate.Height = 29;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = null;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            guna2DataGridView1.ThemeStyle.BackColor = SystemColors.ButtonFace;
            guna2DataGridView1.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(232, 234, 237);
            guna2DataGridView1.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            guna2DataGridView1.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(64, 64, 64);
            guna2DataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            guna2DataGridView1.ThemeStyle.HeaderStyle.Height = 35;
            guna2DataGridView1.ThemeStyle.ReadOnly = true;
            guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = Color.White;
            guna2DataGridView1.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.Single;
            guna2DataGridView1.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            guna2DataGridView1.ThemeStyle.RowsStyle.Height = 29;
            guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            guna2DataGridView1.CellDoubleClick += guna2DataGridView1_CellDoubleClick;
            // 
            // dgSno
            // 
            dgSno.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgSno.FillWeight = 70F;
            resources.ApplyResources(dgSno, "dgSno");
            dgSno.Name = "dgSno";
            dgSno.ReadOnly = true;
            // 
            // dgvid
            // 
            resources.ApplyResources(dgvid, "dgvid");
            dgvid.Name = "dgvid";
            dgvid.ReadOnly = true;
            // 
            // dgvName
            // 
            resources.ApplyResources(dgvName, "dgvName");
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            // 
            // topPanel
            // 
            resources.ApplyResources(topPanel, "topPanel");
            topPanel.Controls.Add(txtSearch);
            topPanel.Name = "topPanel";
            // 
            // txtSearch
            // 
            resources.ApplyResources(txtSearch, "txtSearch");
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
            txtSearch.ForeColor = Color.FromArgb(64, 64, 64);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.IconRight = Properties.Resources.search_Dark;
            txtSearch.IconRightOffset = new Point(5, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "ابحث هنا";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtSearch.TextAlign = HorizontalAlignment.Right;
            txtSearch.TextOffset = new Point(5, 0);
            // 
            // frmCategoryView
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.ButtonFace;
            Controls.Add(topPanel);
            Controls.Add(guna2DataGridView1);
            Name = "frmCategoryView";
            Load += frmCategoryView_Load;
            Controls.SetChildIndex(guna2DataGridView1, 0);
            Controls.SetChildIndex(txtSearch1, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(btnAdd, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(guna2Separator1, 0);
            Controls.SetChildIndex(topPanel, 0);
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView1).EndInit();
            topPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView guna2DataGridView1;
        private DataGridViewTextBoxColumn dgSno;
        private DataGridViewTextBoxColumn dgvid;
        private DataGridViewTextBoxColumn dgvName;
        private Panel topPanel;
        public Guna.UI2.WinForms.Guna2TextBox txtSearch;
    }
}