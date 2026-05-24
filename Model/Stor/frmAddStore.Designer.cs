using pos.Classes;

namespace pos.Model
{
    partial class frmAddStore
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
            btn_Close = new Guna.UI2.WinForms.Guna2Button();
            btn_Save = new Guna.UI2.WinForms.Guna2Button();
            guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            lblCode = new Label();
            txtCode = new Guna.UI2.WinForms.Guna2TextBox();
            txtName = new Guna.UI2.WinForms.Guna2TextBox();
            lblName = new Label();
            topPanel = new SmoothPanelTopConrner();
            lblTitle = new Label();
            iconImage = new Guna.UI2.WinForms.Guna2PictureBox();
            bottomPanel = new SmoothPanel_BottomCorner();
            smoothPanel_BottomCorner1 = new SmoothPanel_BottomCorner();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconImage).BeginInit();
            bottomPanel.SuspendLayout();
            smoothPanel_BottomCorner1.SuspendLayout();
            SuspendLayout();
            // 
            // btn_Close
            // 
            btn_Close.Anchor = AnchorStyles.None;
            btn_Close.BorderRadius = 8;
            btn_Close.CustomizableEdges = customizableEdges1;
            btn_Close.DisabledState.BorderColor = Color.DarkGray;
            btn_Close.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Close.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Close.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Close.FillColor = Color.Red;
            btn_Close.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btn_Close.ForeColor = Color.White;
            btn_Close.Location = new Point(121, 10);
            btn_Close.Name = "btn_Close";
            btn_Close.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btn_Close.Size = new Size(94, 26);
            btn_Close.TabIndex = 1;
            btn_Close.Text = "الغاء";
            btn_Close.Click += btnClose_Click;
            // 
            // btn_Save
            // 
            btn_Save.Anchor = AnchorStyles.None;
            btn_Save.BorderRadius = 8;
            btn_Save.CustomizableEdges = customizableEdges3;
            btn_Save.DisabledState.BorderColor = Color.DarkGray;
            btn_Save.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Save.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Save.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Save.FillColor = Color.FromArgb(1, 95, 95);
            btn_Save.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btn_Save.ForeColor = Color.White;
            btn_Save.Location = new Point(229, 10);
            btn_Save.Name = "btn_Save";
            btn_Save.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btn_Save.Size = new Size(94, 26);
            btn_Save.TabIndex = 0;
            btn_Save.Text = "حفظ";
            btn_Save.Click += btnSave_Click;
            // 
            // guna2MessageDialog1
            // 
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Caption = "RMS";
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Parent = null;
            guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;
            guna2MessageDialog1.Text = null;
            // 
            // lblCode
            // 
            lblCode.AutoSize = true;
            lblCode.Location = new Point(250, 16);
            lblCode.Name = "lblCode";
            lblCode.Size = new Size(62, 15);
            lblCode.TabIndex = 4;
            lblCode.Text = "كود المخزن";
            // 
            // txtCode
            // 
            txtCode.BorderColor = Color.FromArgb(1, 95, 95);
            txtCode.BorderRadius = 8;
            txtCode.CustomizableEdges = customizableEdges5;
            txtCode.DefaultText = "";
            txtCode.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtCode.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtCode.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtCode.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtCode.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtCode.Font = new Font("Segoe UI", 9F);
            txtCode.ForeColor = Color.FromArgb(64, 64, 64);
            txtCode.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtCode.Location = new Point(241, 35);
            txtCode.Margin = new Padding(3, 4, 3, 4);
            txtCode.Name = "txtCode";
            txtCode.PlaceholderText = "";
            txtCode.SelectedText = "";
            txtCode.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtCode.Size = new Size(82, 26);
            txtCode.TabIndex = 5;
            txtCode.TextAlign = HorizontalAlignment.Center;
            // 
            // txtName
            // 
            txtName.BorderColor = Color.FromArgb(1, 95, 95);
            txtName.BorderRadius = 8;
            txtName.CustomizableEdges = customizableEdges7;
            txtName.DefaultText = "";
            txtName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Font = new Font("Segoe UI", 9F);
            txtName.ForeColor = Color.FromArgb(64, 64, 64);
            txtName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Location = new Point(11, 35);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "";
            txtName.RightToLeft = RightToLeft.Yes;
            txtName.SelectedText = "";
            txtName.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtName.Size = new Size(212, 26);
            txtName.TabIndex = 7;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(151, 16);
            lblName.Name = "lblName";
            lblName.Size = new Size(64, 15);
            lblName.TabIndex = 6;
            lblName.Text = "اسم المخزن";
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(1, 95, 95);
            topPanel.BorderColor = Color.FromArgb(1, 95, 95);
            topPanel.BorderSize = 1F;
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(iconImage);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(335, 50);
            topPanel.TabIndex = 8;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(204, 204, 204);
            lblTitle.ImeMode = ImeMode.NoControl;
            lblTitle.Location = new Point(202, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(86, 20);
            lblTitle.TabIndex = 6;
            lblTitle.Text = "أضافة مخزن";
            // 
            // iconImage
            // 
            iconImage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            iconImage.CustomizableEdges = customizableEdges9;
            iconImage.Image = Properties.Resources.store_Light;
            iconImage.ImageRotate = 0F;
            iconImage.Location = new Point(282, 7);
            iconImage.Name = "iconImage";
            iconImage.ShadowDecoration.CustomizableEdges = customizableEdges10;
            iconImage.Size = new Size(50, 35);
            iconImage.SizeMode = PictureBoxSizeMode.Zoom;
            iconImage.TabIndex = 1;
            iconImage.TabStop = false;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(230, 230, 230);
            bottomPanel.BorderColor = Color.FromArgb(1, 95, 95);
            bottomPanel.BorderSize = 1F;
            bottomPanel.Controls.Add(btn_Save);
            bottomPanel.Controls.Add(btn_Close);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 127);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(335, 46);
            bottomPanel.TabIndex = 9;
            // 
            // smoothPanel_BottomCorner1
            // 
            smoothPanel_BottomCorner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel_BottomCorner1.BorderSize = 1F;
            smoothPanel_BottomCorner1.Controls.Add(txtName);
            smoothPanel_BottomCorner1.Controls.Add(lblCode);
            smoothPanel_BottomCorner1.Controls.Add(txtCode);
            smoothPanel_BottomCorner1.Controls.Add(lblName);
            smoothPanel_BottomCorner1.Dock = DockStyle.Fill;
            smoothPanel_BottomCorner1.Location = new Point(0, 50);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(335, 77);
            smoothPanel_BottomCorner1.TabIndex = 10;
            // 
            // frmAddStore
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(335, 173);
            Controls.Add(smoothPanel_BottomCorner1);
            Controls.Add(bottomPanel);
            Controls.Add(topPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmAddStore";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmAddStore";
            Load += frmAddStore_Load;
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconImage).EndInit();
            bottomPanel.ResumeLayout(false);
            smoothPanel_BottomCorner1.ResumeLayout(false);
            smoothPanel_BottomCorner1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        public Guna.UI2.WinForms.Guna2Button btn_Close;
        public Guna.UI2.WinForms.Guna2Button btn_Save;
        public Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
        private Label lblCode;
        private Label lblName;
        public Guna.UI2.WinForms.Guna2TextBox txtCode;
        public Guna.UI2.WinForms.Guna2TextBox txtName;
        private SmoothPanelTopConrner topPanel;
        private Label lblTitle;
        private Guna.UI2.WinForms.Guna2PictureBox iconImage;
        private SmoothPanel_BottomCorner bottomPanel;
        private SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
    }
}