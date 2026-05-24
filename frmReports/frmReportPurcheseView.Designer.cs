namespace pos.View
{
    partial class frmReportPurcheseView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportPurcheseView));
            guna2DataGridView2 = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno = new DataGridViewTextBoxColumn();
            dgvid = new DataGridViewTextBoxColumn();
            dgvproID = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            dgvPname = new DataGridViewTextBoxColumn();
            dgvPrice = new DataGridViewTextBoxColumn();
            dgvQty = new DataGridViewTextBoxColumn();
            dgvTime = new DataGridViewTextBoxColumn();
            dgvDate = new DataGridViewTextBoxColumn();
            dgvMainID = new DataGridViewTextBoxColumn();
            dgvdel = new DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView2).BeginInit();
            SuspendLayout();
            // 
            // txtSearch1
            // 
            txtSearch1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch1.Location = new Point(503, 3);
            txtSearch1.ShadowDecoration.CustomizableEdges = customizableEdges1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.Location = new Point(262, 10);
            label1.Visible = false;
            // 
            // btnAdd
            // 
            btnAdd.CheckedState.ImageSize = new Size(64, 64);
            btnAdd.DialogResult = DialogResult.None;
            btnAdd.HoverState.ImageSize = new Size(64, 64);
            btnAdd.ImageFlip = Guna.UI2.WinForms.Enums.FlipOrientation.Normal;
            btnAdd.Location = new Point(240, 16);
            btnAdd.PressedState.ImageSize = new Size(64, 64);
            btnAdd.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnAdd.Visible = false;
            // 
            // label2
            // 
            label2.Location = new Point(156, 39);
            label2.Size = new Size(125, 23);
            label2.Text = "تقرير المشتريات";
            label2.Visible = false;
            // 
            // guna2Separator1
            // 
            guna2Separator1.Location = new Point(13, 50);
            guna2Separator1.Size = new Size(776, 12);
            // 
            // guna2DataGridView2
            // 
            guna2DataGridView2.AllowUserToAddRows = false;
            guna2DataGridView2.AllowUserToDeleteRows = false;
            guna2DataGridView2.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            guna2DataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            guna2DataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(232, 234, 237);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            guna2DataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            guna2DataGridView2.ColumnHeadersHeight = 35;
            guna2DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            guna2DataGridView2.Columns.AddRange(new DataGridViewColumn[] { dgSno, dgvid, dgvproID, dgvName, dgvPname, dgvPrice, dgvQty, dgvTime, dgvDate, dgvMainID, dgvdel });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            guna2DataGridView2.DefaultCellStyle = dataGridViewCellStyle3;
            guna2DataGridView2.GridColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView2.Location = new Point(12, 65);
            guna2DataGridView2.Name = "guna2DataGridView2";
            guna2DataGridView2.ReadOnly = true;
            guna2DataGridView2.RightToLeft = RightToLeft.Yes;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            guna2DataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            guna2DataGridView2.RowHeadersVisible = false;
            guna2DataGridView2.RowHeadersWidth = 51;
            guna2DataGridView2.Size = new Size(776, 711);
            guna2DataGridView2.TabIndex = 9;
            guna2DataGridView2.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            guna2DataGridView2.ThemeStyle.AlternatingRowsStyle.Font = null;
            guna2DataGridView2.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            guna2DataGridView2.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            guna2DataGridView2.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            guna2DataGridView2.ThemeStyle.BackColor = Color.White;
            guna2DataGridView2.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView2.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(232, 234, 237);
            guna2DataGridView2.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            guna2DataGridView2.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2DataGridView2.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(64, 64, 64);
            guna2DataGridView2.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            guna2DataGridView2.ThemeStyle.HeaderStyle.Height = 35;
            guna2DataGridView2.ThemeStyle.ReadOnly = true;
            guna2DataGridView2.ThemeStyle.RowsStyle.BackColor = Color.White;
            guna2DataGridView2.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            guna2DataGridView2.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2DataGridView2.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            guna2DataGridView2.ThemeStyle.RowsStyle.Height = 29;
            guna2DataGridView2.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView2.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            guna2DataGridView2.CellClick += guna2DataGridView2_CellClick;
            // 
            // dgSno
            // 
            dgSno.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgSno.FillWeight = 20F;
            dgSno.HeaderText = "#";
            dgSno.MinimumWidth = 20;
            dgSno.Name = "dgSno";
            dgSno.ReadOnly = true;
            // 
            // dgvid
            // 
            dgvid.HeaderText = "id";
            dgvid.MinimumWidth = 6;
            dgvid.Name = "dgvid";
            dgvid.ReadOnly = true;
            dgvid.Visible = false;
            // 
            // dgvproID
            // 
            dgvproID.HeaderText = "productID";
            dgvproID.MinimumWidth = 6;
            dgvproID.Name = "dgvproID";
            dgvproID.ReadOnly = true;
            dgvproID.Visible = false;
            // 
            // dgvName
            // 
            dgvName.HeaderText = "الاسم";
            dgvName.MinimumWidth = 100;
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            // 
            // dgvPname
            // 
            dgvPname.HeaderText = "نوع المشتريات";
            dgvPname.MinimumWidth = 6;
            dgvPname.Name = "dgvPname";
            dgvPname.ReadOnly = true;
            // 
            // dgvPrice
            // 
            dgvPrice.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvPrice.FillWeight = 70F;
            dgvPrice.HeaderText = "السعر";
            dgvPrice.MinimumWidth = 70;
            dgvPrice.Name = "dgvPrice";
            dgvPrice.ReadOnly = true;
            dgvPrice.Width = 70;
            // 
            // dgvQty
            // 
            dgvQty.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvQty.FillWeight = 50F;
            dgvQty.HeaderText = "الكمية";
            dgvQty.MinimumWidth = 50;
            dgvQty.Name = "dgvQty";
            dgvQty.ReadOnly = true;
            dgvQty.Width = 50;
            // 
            // dgvTime
            // 
            dgvTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvTime.HeaderText = "الوقت";
            dgvTime.MinimumWidth = 100;
            dgvTime.Name = "dgvTime";
            dgvTime.ReadOnly = true;
            dgvTime.Width = 125;
            // 
            // dgvDate
            // 
            dgvDate.HeaderText = "التاريخ";
            dgvDate.MinimumWidth = 150;
            dgvDate.Name = "dgvDate";
            dgvDate.ReadOnly = true;
            // 
            // dgvMainID
            // 
            dgvMainID.HeaderText = "MainID";
            dgvMainID.MinimumWidth = 6;
            dgvMainID.Name = "dgvMainID";
            dgvMainID.ReadOnly = true;
            dgvMainID.Visible = false;
            // 
            // dgvdel
            // 
            dgvdel.FillWeight = 30F;
            dgvdel.HeaderText = "";
            dgvdel.Image = (Image)resources.GetObject("dgvdel.Image");
            dgvdel.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvdel.MinimumWidth = 30;
            dgvdel.Name = "dgvdel";
            dgvdel.ReadOnly = true;
            // 
            // frmReportPurcheseView
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 788);
            Controls.Add(guna2DataGridView2);
            Name = "frmReportPurcheseView";
            Text = "frmReportView";
            Load += frm_Load;
            Controls.SetChildIndex(txtSearch1, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(btnAdd, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(guna2Separator1, 0);
            Controls.SetChildIndex(guna2DataGridView2, 0);
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        public Guna.UI2.WinForms.Guna2DataGridView guna2DataGridView2;
        private DataGridViewTextBoxColumn dgSno;
        private DataGridViewTextBoxColumn dgvid;
        private DataGridViewTextBoxColumn dgvproID;
        private DataGridViewTextBoxColumn dgvName;
        private DataGridViewTextBoxColumn dgvPname;
        private DataGridViewTextBoxColumn dgvPrice;
        private DataGridViewTextBoxColumn dgvQty;
        private DataGridViewTextBoxColumn dgvTime;
        private DataGridViewTextBoxColumn dgvDate;
        private DataGridViewTextBoxColumn dgvMainID;
        private DataGridViewImageColumn dgvdel;
    }
}