namespace pos.Model.POS
{
    partial class frmCatecorySearch
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
            dgvProducts = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno = new DataGridViewTextBoxColumn();
            dgvCatID = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            btnClose = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
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
            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { dgSno, dgvCatID, dgvName });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(51, 51, 51);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvProducts.DefaultCellStyle = dataGridViewCellStyle4;
            dgvProducts.GridColor = Color.FromArgb(1, 95, 95);
            dgvProducts.Location = new Point(9, 62);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
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
            dgvProducts.Size = new Size(351, 408);
            dgvProducts.TabIndex = 4;
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
            dgvProducts.ThemeStyle.ReadOnly = false;
            dgvProducts.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(243, 243, 243);
            dgvProducts.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.Single;
            dgvProducts.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvProducts.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(51, 51, 51);
            dgvProducts.ThemeStyle.RowsStyle.Height = 29;
            dgvProducts.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgvProducts.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
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
            // dgvCatID
            // 
            dgvCatID.HeaderText = "CatID";
            dgvCatID.Name = "dgvCatID";
            dgvCatID.Visible = false;
            // 
            // dgvName
            // 
            dgvName.FillWeight = 193.630569F;
            dgvName.HeaderText = "الاسم";
            dgvName.MinimumWidth = 200;
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            // 
            // btnClose
            // 
            btnClose.BorderRadius = 5;
            btnClose.CustomizableEdges = customizableEdges1;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.Red;
            btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(6, 5);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnClose.Size = new Size(30, 20);
            btnClose.TabIndex = 5;
            btnClose.Text = "X";
            btnClose.Click += btnClose_Click;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.BorderColor = Color.FromArgb(1, 95, 95);
            txtSearch.BorderRadius = 8;
            txtSearch.BorderThickness = 2;
            txtSearch.CustomizableEdges = customizableEdges3;
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
            txtSearch.Location = new Point(42, 18);
            txtSearch.Margin = new Padding(3, 5, 3, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "ابحث بالاسم";
            txtSearch.RightToLeft = RightToLeft.Yes;
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtSearch.Size = new Size(318, 33);
            txtSearch.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            txtSearch.TabIndex = 3;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // frmCatecorySearch
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(367, 474);
            Controls.Add(dgvProducts);
            Controls.Add(btnClose);
            Controls.Add(txtSearch);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmCatecorySearch";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmCatecorySearch";
            Load += frmCatecorySearch_Load;
            MouseDown += frmCatecorySearch_MouseDown;
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvProducts;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private DataGridViewTextBoxColumn dgSno;
        private DataGridViewTextBoxColumn dgvCatID;
        private DataGridViewTextBoxColumn dgvName;
    }
}