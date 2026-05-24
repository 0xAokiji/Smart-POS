using pos.Classes;

namespace pos.Model
{
    partial class frmBillList
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
            iconImage = new Guna.UI2.WinForms.Guna2PictureBox();
            maindgv = new Guna.UI2.WinForms.Guna2DataGridView();
            dgSno = new DataGridViewTextBoxColumn();
            dgvid = new DataGridViewTextBoxColumn();
            dgvPid = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            dgvCode = new DataGridViewTextBoxColumn();
            dgvQty = new DataGridViewTextBoxColumn();
            dgvStatus = new DataGridViewTextBoxColumn();
            dgvTotal = new DataGridViewTextBoxColumn();
            dgvDetail = new DataGridViewImageColumn();
            dgvDel = new DataGridViewImageColumn();
            mainPanel = new SmoothPanel();
            bottomPanel = new SmoothPanel_BottomCorner();
            btnCansel = new Guna.UI2.WinForms.Guna2Button();
            btn_Delete = new Guna.UI2.WinForms.Guna2Button();
            topPanel = new SmoothPanelTopConrner();
            lblTitle = new Label();
            btnUnCom = new Guna.UI2.WinForms.Guna2Button();
            btnHold = new Guna.UI2.WinForms.Guna2Button();
            btnEnd = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)iconImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)maindgv).BeginInit();
            mainPanel.SuspendLayout();
            bottomPanel.SuspendLayout();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // iconImage
            // 
            iconImage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            iconImage.CustomizableEdges = customizableEdges1;
            iconImage.Image = Properties.Resources.bill_light;
            iconImage.ImageRotate = 0F;
            iconImage.Location = new Point(645, 6);
            iconImage.Margin = new Padding(3, 2, 3, 2);
            iconImage.Name = "iconImage";
            iconImage.ShadowDecoration.CustomizableEdges = customizableEdges2;
            iconImage.Size = new Size(50, 35);
            iconImage.SizeMode = PictureBoxSizeMode.Zoom;
            iconImage.TabIndex = 3;
            iconImage.TabStop = false;
            // 
            // maindgv
            // 
            maindgv.AllowUserToAddRows = false;
            maindgv.AllowUserToDeleteRows = false;
            maindgv.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            maindgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            maindgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            maindgv.BackgroundColor = Color.FromArgb(243, 243, 243);
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(232, 234, 237);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            maindgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            maindgv.ColumnHeadersHeight = 35;
            maindgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            maindgv.Columns.AddRange(new DataGridViewColumn[] { dgSno, dgvid, dgvPid, dgvName, dgvCode, dgvQty, dgvStatus, dgvTotal, dgvDetail, dgvDel });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(239, 241, 243);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            maindgv.DefaultCellStyle = dataGridViewCellStyle4;
            maindgv.GridColor = Color.FromArgb(231, 229, 255);
            maindgv.Location = new Point(5, 96);
            maindgv.Margin = new Padding(3, 2, 3, 2);
            maindgv.Name = "maindgv";
            maindgv.ReadOnly = true;
            maindgv.RightToLeft = RightToLeft.Yes;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            maindgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            maindgv.RowHeadersVisible = false;
            maindgv.RowHeadersWidth = 51;
            maindgv.RowTemplate.Height = 29;
            maindgv.Size = new Size(690, 475);
            maindgv.TabIndex = 6;
            maindgv.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            maindgv.ThemeStyle.AlternatingRowsStyle.Font = null;
            maindgv.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            maindgv.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            maindgv.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            maindgv.ThemeStyle.BackColor = Color.FromArgb(243, 243, 243);
            maindgv.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            maindgv.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(232, 234, 237);
            maindgv.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            maindgv.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            maindgv.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(64, 64, 64);
            maindgv.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            maindgv.ThemeStyle.HeaderStyle.Height = 35;
            maindgv.ThemeStyle.ReadOnly = true;
            maindgv.ThemeStyle.RowsStyle.BackColor = Color.White;
            maindgv.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            maindgv.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            maindgv.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            maindgv.ThemeStyle.RowsStyle.Height = 29;
            maindgv.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            maindgv.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            maindgv.CellClick += guna2DataGridView1_CellClick;
            maindgv.CellDoubleClick += guna2DataGridView1_CellContentClick;
            maindgv.CellFormatting += guna2DataGridView2_CellFormatting;
            maindgv.Scroll += maindgv_Scroll;
            // 
            // dgSno
            // 
            dgSno.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dgSno.FillWeight = 70F;
            dgSno.HeaderText = "#";
            dgSno.MinimumWidth = 20;
            dgSno.Name = "dgSno";
            dgSno.ReadOnly = true;
            dgSno.Width = 20;
            // 
            // dgvid
            // 
            dgvid.HeaderText = "dgvID";
            dgvid.MinimumWidth = 6;
            dgvid.Name = "dgvid";
            dgvid.ReadOnly = true;
            dgvid.Visible = false;
            // 
            // dgvPid
            // 
            dgvPid.HeaderText = "Pid";
            dgvPid.MinimumWidth = 6;
            dgvPid.Name = "dgvPid";
            dgvPid.ReadOnly = true;
            dgvPid.Visible = false;
            // 
            // dgvName
            // 
            dgvName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvName.HeaderText = "اسم العميل";
            dgvName.MinimumWidth = 200;
            dgvName.Name = "dgvName";
            dgvName.ReadOnly = true;
            dgvName.Width = 200;
            // 
            // dgvCode
            // 
            dgvCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvCode.HeaderText = "كود الفاتورة";
            dgvCode.Name = "dgvCode";
            dgvCode.ReadOnly = true;
            dgvCode.Width = 101;
            // 
            // dgvQty
            // 
            dgvQty.HeaderText = "الكمية";
            dgvQty.MinimumWidth = 6;
            dgvQty.Name = "dgvQty";
            dgvQty.ReadOnly = true;
            dgvQty.Visible = false;
            // 
            // dgvStatus
            // 
            dgvStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dgvStatus.FillWeight = 150F;
            dgvStatus.HeaderText = "الحالة";
            dgvStatus.MinimumWidth = 150;
            dgvStatus.Name = "dgvStatus";
            dgvStatus.ReadOnly = true;
            dgvStatus.Width = 150;
            // 
            // dgvTotal
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTotal.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTotal.HeaderText = "الحساب الكلي";
            dgvTotal.MinimumWidth = 100;
            dgvTotal.Name = "dgvTotal";
            dgvTotal.ReadOnly = true;
            // 
            // dgvDetail
            // 
            dgvDetail.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvDetail.FillWeight = 70F;
            dgvDetail.HeaderText = "تفاصيل";
            dgvDetail.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvDetail.MinimumWidth = 70;
            dgvDetail.Name = "dgvDetail";
            dgvDetail.ReadOnly = true;
            dgvDetail.Visible = false;
            dgvDetail.Width = 70;
            // 
            // dgvDel
            // 
            dgvDel.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvDel.FillWeight = 50F;
            dgvDel.HeaderText = "";
            dgvDel.Image = Properties.Resources.delete_Red;
            dgvDel.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvDel.MinimumWidth = 50;
            dgvDel.Name = "dgvDel";
            dgvDel.ReadOnly = true;
            dgvDel.Width = 50;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(243, 243, 243);
            mainPanel.BorderColor = Color.FromArgb(1, 95, 95);
            mainPanel.BorderSize = 2F;
            mainPanel.Controls.Add(bottomPanel);
            mainPanel.Controls.Add(topPanel);
            mainPanel.Controls.Add(btnUnCom);
            mainPanel.Controls.Add(btnHold);
            mainPanel.Controls.Add(btnEnd);
            mainPanel.Controls.Add(maindgv);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Margin = new Padding(3, 2, 3, 2);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(700, 614);
            mainPanel.TabIndex = 17;
            // 
            // bottomPanel
            // 
            bottomPanel.BorderColor = Color.FromArgb(1, 95, 95);
            bottomPanel.BorderSize = 1F;
            bottomPanel.Controls.Add(btnCansel);
            bottomPanel.Controls.Add(btn_Delete);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 576);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(700, 38);
            bottomPanel.TabIndex = 18;
            // 
            // btnCansel
            // 
            btnCansel.BorderRadius = 8;
            btnCansel.CustomizableEdges = customizableEdges3;
            btnCansel.DisabledState.BorderColor = Color.DarkGray;
            btnCansel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCansel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCansel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCansel.FillColor = Color.FromArgb(136, 214, 218);
            btnCansel.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCansel.ForeColor = Color.White;
            btnCansel.Location = new Point(482, 6);
            btnCansel.Margin = new Padding(3, 2, 3, 2);
            btnCansel.Name = "btnCansel";
            btnCansel.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnCansel.Size = new Size(94, 26);
            btnCansel.TabIndex = 1;
            btnCansel.Text = "خروج";
            btnCansel.TextOffset = new Point(0, -3);
            btnCansel.Click += btnCansel_Click;
            // 
            // btn_Delete
            // 
            btn_Delete.BorderRadius = 8;
            btn_Delete.CustomizableEdges = customizableEdges5;
            btn_Delete.DisabledState.BorderColor = Color.DarkGray;
            btn_Delete.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Delete.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Delete.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Delete.FillColor = Color.Red;
            btn_Delete.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Delete.ForeColor = Color.White;
            btn_Delete.Location = new Point(594, 6);
            btn_Delete.Margin = new Padding(3, 2, 3, 2);
            btn_Delete.Name = "btn_Delete";
            btn_Delete.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btn_Delete.Size = new Size(94, 26);
            btn_Delete.TabIndex = 0;
            btn_Delete.Text = "حذف الكل";
            btn_Delete.Click += guna2Button1_Click;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(136, 214, 218);
            topPanel.BorderColor = Color.FromArgb(1, 95, 95);
            topPanel.BorderSize = 1F;
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(iconImage);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(700, 50);
            topPanel.TabIndex = 17;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.ImeMode = ImeMode.NoControl;
            lblTitle.Location = new Point(594, 13);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(54, 20);
            lblTitle.TabIndex = 5;
            lblTitle.Text = "الفواتير";
            // 
            // btnUnCom
            // 
            btnUnCom.AutoRoundedCorners = true;
            btnUnCom.BackColor = Color.Transparent;
            btnUnCom.BorderRadius = 15;
            btnUnCom.CustomizableEdges = customizableEdges7;
            btnUnCom.DisabledState.BorderColor = Color.DarkGray;
            btnUnCom.DisabledState.CustomBorderColor = Color.DarkGray;
            btnUnCom.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnUnCom.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnUnCom.FillColor = Color.FromArgb(243, 243, 243);
            btnUnCom.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUnCom.ForeColor = Color.FromArgb(51, 51, 51);
            btnUnCom.Location = new Point(240, 51);
            btnUnCom.Margin = new Padding(3, 2, 3, 2);
            btnUnCom.Name = "btnUnCom";
            btnUnCom.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnUnCom.Size = new Size(105, 32);
            btnUnCom.TabIndex = 14;
            btnUnCom.Text = "غير مكتملة";
            btnUnCom.Click += btnUnCom_Click;
            // 
            // btnHold
            // 
            btnHold.AutoRoundedCorners = true;
            btnHold.BackColor = Color.Transparent;
            btnHold.BorderRadius = 15;
            btnHold.CustomizableEdges = customizableEdges9;
            btnHold.DisabledState.BorderColor = Color.DarkGray;
            btnHold.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHold.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHold.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHold.FillColor = Color.FromArgb(243, 243, 243);
            btnHold.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHold.ForeColor = Color.FromArgb(51, 51, 51);
            btnHold.Location = new Point(355, 51);
            btnHold.Margin = new Padding(3, 2, 3, 2);
            btnHold.Name = "btnHold";
            btnHold.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnHold.Size = new Size(105, 32);
            btnHold.TabIndex = 13;
            btnHold.Text = "معلقة";
            btnHold.Click += btnHold_Click;
            // 
            // btnEnd
            // 
            btnEnd.AutoRoundedCorners = true;
            btnEnd.BackColor = Color.Transparent;
            btnEnd.BorderRadius = 15;
            btnEnd.CustomizableEdges = customizableEdges11;
            btnEnd.DisabledState.BorderColor = Color.DarkGray;
            btnEnd.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEnd.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEnd.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEnd.FillColor = Color.FromArgb(243, 243, 243);
            btnEnd.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEnd.ForeColor = Color.FromArgb(51, 51, 51);
            btnEnd.Location = new Point(487, 55);
            btnEnd.Margin = new Padding(3, 2, 3, 2);
            btnEnd.Name = "btnEnd";
            btnEnd.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnEnd.Size = new Size(105, 32);
            btnEnd.TabIndex = 15;
            btnEnd.Text = "مدفوعة";
            btnEnd.Visible = false;
            btnEnd.Click += btnEnd_Click;
            // 
            // frmBillList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(700, 614);
            Controls.Add(mainPanel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmBillList";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmBillList";
            Load += frmBillList_Load;
            Paint += frmBillList_Paint;
            ((System.ComponentModel.ISupportInitialize)iconImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)maindgv).EndInit();
            mainPanel.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2PictureBox iconImage;
        private Guna.UI2.WinForms.Guna2MessageDialog messageBox;
        private Guna.UI2.WinForms.Guna2DataGridView maindgv;
        private SmoothPanel mainPanel;
        private SmoothPanelTopConrner topPanel;
        public Guna.UI2.WinForms.Guna2Button btnUnCom;
        public Guna.UI2.WinForms.Guna2Button btnHold;
        public Guna.UI2.WinForms.Guna2Button btnEnd;
        private SmoothPanel_BottomCorner bottomPanel;
        private Guna.UI2.WinForms.Guna2Button btnCansel;
        private Guna.UI2.WinForms.Guna2Button btn_Delete;
        private Label lblTitle;
        private DataGridViewTextBoxColumn dgSno;
        private DataGridViewTextBoxColumn dgvid;
        private DataGridViewTextBoxColumn dgvPid;
        private DataGridViewTextBoxColumn dgvName;
        private DataGridViewTextBoxColumn dgvCode;
        private DataGridViewTextBoxColumn dgvQty;
        private DataGridViewTextBoxColumn dgvStatus;
        private DataGridViewTextBoxColumn dgvTotal;
        private DataGridViewImageColumn dgvDetail;
        private DataGridViewImageColumn dgvDel;
    }
}