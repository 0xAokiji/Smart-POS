namespace pos.Settings
{
    partial class frmProfile
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            mainPanel = new Panel();
            passPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            txtPass = new Guna.UI2.WinForms.Guna2TextBox();
            btnSavePass = new Guna.UI2.WinForms.Guna2Button();
            txtOldPass = new Guna.UI2.WinForms.Guna2TextBox();
            lblPassMach = new Label();
            txtRepass = new Guna.UI2.WinForms.Guna2TextBox();
            userImage = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            btnChoseImage = new Guna.UI2.WinForms.Guna2Button();
            userPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            txtName = new Guna.UI2.WinForms.Guna2TextBox();
            txtPhone = new Guna.UI2.WinForms.Guna2TextBox();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            messageBox = new Guna.UI2.WinForms.Guna2MessageDialog();
            mainPanel.SuspendLayout();
            passPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)userImage).BeginInit();
            userPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(passPanel);
            mainPanel.Controls.Add(userImage);
            mainPanel.Controls.Add(btnChoseImage);
            mainPanel.Controls.Add(userPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(527, 516);
            mainPanel.TabIndex = 0;
            // 
            // passPanel
            // 
            passPanel.BackColor = Color.Transparent;
            passPanel.Controls.Add(txtPass);
            passPanel.Controls.Add(btnSavePass);
            passPanel.Controls.Add(txtOldPass);
            passPanel.Controls.Add(lblPassMach);
            passPanel.Controls.Add(txtRepass);
            passPanel.FillColor = Color.FromArgb(243, 243, 243);
            passPanel.Location = new Point(12, 285);
            passPanel.Name = "passPanel";
            passPanel.Radius = 7;
            passPanel.ShadowColor = Color.Black;
            passPanel.Size = new Size(503, 219);
            passPanel.TabIndex = 2;
            // 
            // txtPass
            // 
            txtPass.BorderColor = Color.FromArgb(136, 214, 218);
            txtPass.BorderRadius = 8;
            txtPass.CustomizableEdges = customizableEdges1;
            txtPass.DefaultText = "";
            txtPass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPass.FillColor = Color.FromArgb(243, 243, 243);
            txtPass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtPass.ForeColor = Color.FromArgb(64, 64, 64);
            txtPass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPass.IconLeftCursor = Cursors.IBeam;
            txtPass.IconLeftOffset = new Point(20, 20);
            txtPass.IconRight = Properties.Resources.showpass_dark;
            txtPass.IconRightCursor = Cursors.Hand;
            txtPass.IconRightOffset = new Point(3, 0);
            txtPass.Location = new Point(43, 63);
            txtPass.Margin = new Padding(3, 4, 3, 4);
            txtPass.Name = "txtPass";
            txtPass.Padding = new Padding(3, 0, 3, 0);
            txtPass.PasswordChar = '●';
            txtPass.PlaceholderText = "كلمة المرور الجديدة";
            txtPass.SelectedText = "";
            txtPass.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPass.Size = new Size(417, 36);
            txtPass.TabIndex = 1;
            txtPass.TextAlign = HorizontalAlignment.Right;
            txtPass.IconRightClick += txtPass_IconRightClick;
            txtPass.TextChanged += txtPass_TextChanged;
            // 
            // btnSavePass
            // 
            btnSavePass.BorderRadius = 10;
            btnSavePass.CustomizableEdges = customizableEdges3;
            btnSavePass.DisabledState.BorderColor = Color.DarkGray;
            btnSavePass.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSavePass.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSavePass.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSavePass.Enabled = false;
            btnSavePass.FillColor = Color.FromArgb(136, 214, 218);
            btnSavePass.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSavePass.ForeColor = Color.White;
            btnSavePass.Location = new Point(43, 169);
            btnSavePass.Name = "btnSavePass";
            btnSavePass.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSavePass.Size = new Size(132, 36);
            btnSavePass.TabIndex = 3;
            btnSavePass.Text = "حفظ";
            btnSavePass.Click += btnSavePass_Click;
            // 
            // txtOldPass
            // 
            txtOldPass.BorderColor = Color.FromArgb(136, 214, 218);
            txtOldPass.BorderRadius = 8;
            txtOldPass.CustomizableEdges = customizableEdges5;
            txtOldPass.DefaultText = "";
            txtOldPass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtOldPass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtOldPass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtOldPass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtOldPass.FillColor = Color.FromArgb(243, 243, 243);
            txtOldPass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtOldPass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtOldPass.ForeColor = Color.FromArgb(64, 64, 64);
            txtOldPass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtOldPass.IconLeftCursor = Cursors.IBeam;
            txtOldPass.IconLeftOffset = new Point(20, 20);
            txtOldPass.IconRight = Properties.Resources.showpass_dark;
            txtOldPass.IconRightCursor = Cursors.Hand;
            txtOldPass.IconRightOffset = new Point(3, 0);
            txtOldPass.Location = new Point(43, 21);
            txtOldPass.Margin = new Padding(3, 4, 3, 4);
            txtOldPass.Name = "txtOldPass";
            txtOldPass.Padding = new Padding(3, 0, 3, 0);
            txtOldPass.PasswordChar = '●';
            txtOldPass.PlaceholderText = "كلمة المرور القديمة";
            txtOldPass.SelectedText = "";
            txtOldPass.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtOldPass.Size = new Size(417, 36);
            txtOldPass.TabIndex = 0;
            txtOldPass.TextAlign = HorizontalAlignment.Right;
            txtOldPass.IconRightClick += txtOldPass_IconRightClick;
            txtOldPass.TextChanged += txtOldPass_TextChanged;
            // 
            // lblPassMach
            // 
            lblPassMach.AutoSize = true;
            lblPassMach.Font = new Font("Segoe UI", 8.5F);
            lblPassMach.ForeColor = Color.Red;
            lblPassMach.Location = new Point(336, 145);
            lblPassMach.Name = "lblPassMach";
            lblPassMach.RightToLeft = RightToLeft.Yes;
            lblPassMach.Size = new Size(124, 15);
            lblPassMach.TabIndex = 4;
            lblPassMach.Text = "كلمة المرور غير متطابقة";
            lblPassMach.TextAlign = ContentAlignment.MiddleCenter;
            lblPassMach.Visible = false;
            // 
            // txtRepass
            // 
            txtRepass.BorderColor = Color.FromArgb(136, 214, 218);
            txtRepass.BorderRadius = 8;
            txtRepass.CustomizableEdges = customizableEdges7;
            txtRepass.DefaultText = "";
            txtRepass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtRepass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtRepass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtRepass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtRepass.FillColor = Color.FromArgb(243, 243, 243);
            txtRepass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtRepass.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtRepass.ForeColor = Color.FromArgb(64, 64, 64);
            txtRepass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtRepass.IconLeftCursor = Cursors.IBeam;
            txtRepass.IconLeftOffset = new Point(20, 20);
            txtRepass.IconRight = Properties.Resources.showpass_dark;
            txtRepass.IconRightCursor = Cursors.Hand;
            txtRepass.IconRightOffset = new Point(3, 0);
            txtRepass.Location = new Point(43, 105);
            txtRepass.Margin = new Padding(3, 4, 3, 4);
            txtRepass.Name = "txtRepass";
            txtRepass.Padding = new Padding(3, 0, 3, 0);
            txtRepass.PasswordChar = '●';
            txtRepass.PlaceholderText = "اعد كتابة كلمة المرور الجديدة";
            txtRepass.SelectedText = "";
            txtRepass.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtRepass.Size = new Size(417, 36);
            txtRepass.TabIndex = 2;
            txtRepass.TextAlign = HorizontalAlignment.Right;
            txtRepass.IconRightClick += txtRepass_IconRightClick;
            txtRepass.TextChanged += txtRepass_TextChanged;
            // 
            // userImage
            // 
            userImage.Image = Properties.Resources.user;
            userImage.ImageRotate = 0F;
            userImage.Location = new Point(223, 3);
            userImage.Name = "userImage";
            userImage.ShadowDecoration.CustomizableEdges = customizableEdges9;
            userImage.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            userImage.Size = new Size(80, 80);
            userImage.SizeMode = PictureBoxSizeMode.Zoom;
            userImage.TabIndex = 45;
            userImage.TabStop = false;
            // 
            // btnChoseImage
            // 
            btnChoseImage.BorderRadius = 7;
            btnChoseImage.CustomizableEdges = customizableEdges10;
            btnChoseImage.DisabledState.BorderColor = Color.DarkGray;
            btnChoseImage.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChoseImage.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChoseImage.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChoseImage.FillColor = Color.FromArgb(136, 214, 218);
            btnChoseImage.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChoseImage.ForeColor = Color.White;
            btnChoseImage.Location = new Point(174, 89);
            btnChoseImage.Name = "btnChoseImage";
            btnChoseImage.ShadowDecoration.CustomizableEdges = customizableEdges11;
            btnChoseImage.Size = new Size(178, 24);
            btnChoseImage.TabIndex = 0;
            btnChoseImage.Text = "اختر صورة";
            btnChoseImage.TextOffset = new Point(0, -2);
            btnChoseImage.Click += btnChoseImage_Click;
            // 
            // userPanel
            // 
            userPanel.BackColor = Color.Transparent;
            userPanel.Controls.Add(txtName);
            userPanel.Controls.Add(txtPhone);
            userPanel.Controls.Add(btnSave);
            userPanel.FillColor = Color.FromArgb(243, 243, 243);
            userPanel.Location = new Point(12, 117);
            userPanel.Name = "userPanel";
            userPanel.Radius = 7;
            userPanel.ShadowColor = Color.Black;
            userPanel.Size = new Size(503, 163);
            userPanel.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.BorderColor = Color.FromArgb(136, 214, 218);
            txtName.BorderRadius = 8;
            txtName.CustomizableEdges = customizableEdges12;
            txtName.DefaultText = "";
            txtName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtName.FillColor = Color.FromArgb(243, 243, 243);
            txtName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtName.ForeColor = Color.Black;
            txtName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.IconLeftCursor = Cursors.IBeam;
            txtName.IconLeftOffset = new Point(20, 20);
            txtName.IconRightCursor = Cursors.IBeam;
            txtName.Location = new Point(43, 18);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.Name = "txtName";
            txtName.Padding = new Padding(3, 0, 3, 0);
            txtName.PlaceholderText = "اسم الموظف";
            txtName.SelectedText = "";
            txtName.ShadowDecoration.CustomizableEdges = customizableEdges13;
            txtName.Size = new Size(417, 36);
            txtName.TabIndex = 0;
            txtName.TextAlign = HorizontalAlignment.Right;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // txtPhone
            // 
            txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPhone.BorderColor = Color.FromArgb(136, 214, 218);
            txtPhone.BorderRadius = 8;
            txtPhone.CustomizableEdges = customizableEdges14;
            txtPhone.DefaultText = "";
            txtPhone.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPhone.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPhone.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPhone.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPhone.FillColor = Color.FromArgb(243, 243, 243);
            txtPhone.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtPhone.ForeColor = Color.Black;
            txtPhone.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPhone.IconLeftCursor = Cursors.IBeam;
            txtPhone.IconLeftOffset = new Point(20, 20);
            txtPhone.IconRightCursor = Cursors.IBeam;
            txtPhone.Location = new Point(43, 62);
            txtPhone.Margin = new Padding(3, 4, 3, 4);
            txtPhone.Name = "txtPhone";
            txtPhone.Padding = new Padding(3, 0, 3, 0);
            txtPhone.PlaceholderText = "رقم الهاتف";
            txtPhone.SelectedText = "";
            txtPhone.ShadowDecoration.CustomizableEdges = customizableEdges15;
            txtPhone.Size = new Size(417, 36);
            txtPhone.TabIndex = 1;
            txtPhone.TextAlign = HorizontalAlignment.Right;
            txtPhone.TextChanged += txtPhone_TextChanged;
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 10;
            btnSave.CustomizableEdges = customizableEdges16;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.Enabled = false;
            btnSave.FillColor = Color.FromArgb(136, 214, 218);
            btnSave.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(43, 114);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges17;
            btnSave.Size = new Size(132, 36);
            btnSave.TabIndex = 2;
            btnSave.Text = "حفظ";
            btnSave.Click += btnSave_Click;
            // 
            // messageBox
            // 
            messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            messageBox.Caption = null;
            messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            messageBox.Parent = null;
            messageBox.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            messageBox.Text = null;
            // 
            // frmProfile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(527, 516);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmProfile";
            Text = " ";
            Load += frmProfile_Load;
            mainPanel.ResumeLayout(false);
            passPanel.ResumeLayout(false);
            passPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)userImage).EndInit();
            userPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Guna.UI2.WinForms.Guna2ShadowPanel userPanel;
        private Guna.UI2.WinForms.Guna2TextBox txtName;
        private Guna.UI2.WinForms.Guna2TextBox txtPhone;
        public Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2CirclePictureBox userImage;
        private Guna.UI2.WinForms.Guna2Button btnChoseImage;
        private Guna.UI2.WinForms.Guna2ShadowPanel passPanel;
        public Guna.UI2.WinForms.Guna2Button btnSavePass;
        private Guna.UI2.WinForms.Guna2TextBox txtOldPass;
        private Label lblPassMach;
        private Guna.UI2.WinForms.Guna2TextBox txtOldPasstxtPass;
        private Guna.UI2.WinForms.Guna2TextBox txtRepass;
        private Guna.UI2.WinForms.Guna2TextBox txtPass;
        private Guna.UI2.WinForms.Guna2MessageDialog messageBox;
    }
}