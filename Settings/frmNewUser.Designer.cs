namespace pos.AccountManagement
{
    partial class frmNewUser
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
            userPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            userImage = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            lblPassMach = new Label();
            txtUser = new Guna.UI2.WinForms.Guna2TextBox();
            btnChoseImage = new Guna.UI2.WinForms.Guna2Button();
            txtPass = new Guna.UI2.WinForms.Guna2TextBox();
            txtRepass = new Guna.UI2.WinForms.Guna2TextBox();
            comboBoxUser = new Guna.UI2.WinForms.Guna2ComboBox();
            lblStaff = new Label();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            namePanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            btnClose = new Guna.UI2.WinForms.Guna2Button();
            lblComName = new Label();
            userPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)userImage).BeginInit();
            namePanel.SuspendLayout();
            SuspendLayout();
            // 
            // userPanel
            // 
            userPanel.Anchor = AnchorStyles.None;
            userPanel.BackColor = Color.Transparent;
            userPanel.Controls.Add(userImage);
            userPanel.Controls.Add(lblPassMach);
            userPanel.Controls.Add(txtUser);
            userPanel.Controls.Add(btnChoseImage);
            userPanel.Controls.Add(txtPass);
            userPanel.Controls.Add(txtRepass);
            userPanel.Controls.Add(comboBoxUser);
            userPanel.Controls.Add(lblStaff);
            userPanel.Controls.Add(btnSave);
            userPanel.FillColor = Color.FromArgb(230, 230, 230);
            userPanel.Location = new Point(89, 38);
            userPanel.Name = "userPanel";
            userPanel.Radius = 7;
            userPanel.ShadowColor = Color.Black;
            userPanel.Size = new Size(450, 492);
            userPanel.TabIndex = 42;
            // 
            // userImage
            // 
            userImage.Image = Properties.Resources.user;
            userImage.ImageRotate = 0F;
            userImage.Location = new Point(175, 12);
            userImage.Name = "userImage";
            userImage.ShadowDecoration.CustomizableEdges = customizableEdges1;
            userImage.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            userImage.Size = new Size(100, 100);
            userImage.SizeMode = PictureBoxSizeMode.Zoom;
            userImage.TabIndex = 39;
            userImage.TabStop = false;
            // 
            // lblPassMach
            // 
            lblPassMach.AutoSize = true;
            lblPassMach.Font = new Font("Segoe UI", 8.5F);
            lblPassMach.ForeColor = Color.Red;
            lblPassMach.Location = new Point(297, 285);
            lblPassMach.Name = "lblPassMach";
            lblPassMach.RightToLeft = RightToLeft.Yes;
            lblPassMach.Size = new Size(124, 15);
            lblPassMach.TabIndex = 41;
            lblPassMach.Text = "كلمة المرور غير متطابقة";
            lblPassMach.TextAlign = ContentAlignment.MiddleCenter;
            lblPassMach.Visible = false;
            // 
            // txtUser
            // 
            txtUser.BorderColor = Color.FromArgb(136, 214, 218);
            txtUser.BorderRadius = 8;
            txtUser.CustomizableEdges = customizableEdges2;
            txtUser.DefaultText = "";
            txtUser.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtUser.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtUser.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtUser.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtUser.FillColor = Color.FromArgb(243, 243, 243);
            txtUser.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtUser.ForeColor = Color.Black;
            txtUser.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUser.IconLeftCursor = Cursors.IBeam;
            txtUser.IconLeftOffset = new Point(20, 20);
            txtUser.IconRightCursor = Cursors.IBeam;
            txtUser.Location = new Point(17, 158);
            txtUser.Margin = new Padding(3, 4, 3, 4);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "اسم المستخدم";
            txtUser.SelectedText = "";
            txtUser.ShadowDecoration.CustomizableEdges = customizableEdges3;
            txtUser.Size = new Size(417, 36);
            txtUser.TabIndex = 12;
            txtUser.TextAlign = HorizontalAlignment.Right;
            txtUser.TextChanged += txtUser_TextChanged;
            // 
            // btnChoseImage
            // 
            btnChoseImage.BorderRadius = 7;
            btnChoseImage.CustomizableEdges = customizableEdges4;
            btnChoseImage.DisabledState.BorderColor = Color.DarkGray;
            btnChoseImage.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChoseImage.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChoseImage.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChoseImage.FillColor = Color.FromArgb(136, 214, 218);
            btnChoseImage.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChoseImage.ForeColor = Color.White;
            btnChoseImage.Location = new Point(136, 121);
            btnChoseImage.Name = "btnChoseImage";
            btnChoseImage.ShadowDecoration.CustomizableEdges = customizableEdges5;
            btnChoseImage.Size = new Size(178, 24);
            btnChoseImage.TabIndex = 40;
            btnChoseImage.Text = "اختر صورة";
            btnChoseImage.TextOffset = new Point(0, -2);
            btnChoseImage.Click += btnChoseImage_Click;
            // 
            // txtPass
            // 
            txtPass.BorderColor = Color.FromArgb(136, 214, 218);
            txtPass.BorderRadius = 8;
            txtPass.CustomizableEdges = customizableEdges6;
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
            txtPass.Location = new Point(17, 202);
            txtPass.Margin = new Padding(3, 4, 3, 4);
            txtPass.Name = "txtPass";
            txtPass.PasswordChar = '●';
            txtPass.PlaceholderText = "كلمة المرور";
            txtPass.SelectedText = "";
            txtPass.ShadowDecoration.CustomizableEdges = customizableEdges7;
            txtPass.Size = new Size(417, 36);
            txtPass.TabIndex = 14;
            txtPass.TextAlign = HorizontalAlignment.Right;
            txtPass.IconRightClick += txtPass_IconRightClick;
            txtPass.TextChanged += txtPass_TextChanged;
            // 
            // txtRepass
            // 
            txtRepass.BorderColor = Color.FromArgb(136, 214, 218);
            txtRepass.BorderRadius = 8;
            txtRepass.CustomizableEdges = customizableEdges8;
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
            txtRepass.Location = new Point(17, 246);
            txtRepass.Margin = new Padding(3, 4, 3, 4);
            txtRepass.Name = "txtRepass";
            txtRepass.PasswordChar = '●';
            txtRepass.PlaceholderText = "اعد كتابة كلمة المرور";
            txtRepass.SelectedText = "";
            txtRepass.ShadowDecoration.CustomizableEdges = customizableEdges9;
            txtRepass.Size = new Size(417, 36);
            txtRepass.TabIndex = 16;
            txtRepass.TextAlign = HorizontalAlignment.Right;
            txtRepass.IconRightClick += txtRepass_IconRightClick;
            txtRepass.TextChanged += txtRepass_TextChanged;
            // 
            // comboBoxUser
            // 
            comboBoxUser.BackColor = Color.Transparent;
            comboBoxUser.BorderColor = Color.FromArgb(136, 214, 218);
            comboBoxUser.BorderRadius = 8;
            comboBoxUser.CustomizableEdges = customizableEdges10;
            comboBoxUser.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxUser.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxUser.FillColor = Color.FromArgb(242, 242, 242);
            comboBoxUser.FocusedColor = Color.FromArgb(1, 95, 95);
            comboBoxUser.FocusedState.BorderColor = Color.FromArgb(1, 95, 95);
            comboBoxUser.Font = new Font("Segoe UI", 10F);
            comboBoxUser.ForeColor = Color.FromArgb(51, 51, 51);
            comboBoxUser.HoverState.BorderColor = Color.FromArgb(1, 95, 95);
            comboBoxUser.ItemHeight = 30;
            comboBoxUser.Location = new Point(17, 330);
            comboBoxUser.Name = "comboBoxUser";
            comboBoxUser.ShadowDecoration.BorderRadius = 8;
            comboBoxUser.ShadowDecoration.CustomizableEdges = customizableEdges11;
            comboBoxUser.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            comboBoxUser.Size = new Size(417, 36);
            comboBoxUser.TabIndex = 38;
            comboBoxUser.Tag = "";
            comboBoxUser.TextAlign = HorizontalAlignment.Right;
            comboBoxUser.SelectedIndexChanged += comboBoxUser_SelectedIndexChanged;
            // 
            // lblStaff
            // 
            lblStaff.AutoSize = true;
            lblStaff.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStaff.ForeColor = Color.FromArgb(51, 51, 51);
            lblStaff.Location = new Point(365, 310);
            lblStaff.Name = "lblStaff";
            lblStaff.RightToLeft = RightToLeft.Yes;
            lblStaff.Size = new Size(64, 15);
            lblStaff.TabIndex = 17;
            lblStaff.Text = "حدد موظف";
            lblStaff.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 7;
            btnSave.CustomizableEdges = customizableEdges12;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.FillColor = Color.FromArgb(136, 214, 218);
            btnSave.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(17, 417);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges13;
            btnSave.Size = new Size(417, 36);
            btnSave.TabIndex = 2;
            btnSave.Text = "حفظ";
            btnSave.Click += btnSave_Click;
            // 
            // namePanel
            // 
            namePanel.BackColor = Color.Transparent;
            namePanel.Controls.Add(btnClose);
            namePanel.Controls.Add(lblComName);
            namePanel.Dock = DockStyle.Top;
            namePanel.FillColor = Color.FromArgb(1, 95, 95);
            namePanel.ForeColor = Color.White;
            namePanel.Location = new Point(0, 0);
            namePanel.Name = "namePanel";
            namePanel.Radius = 5;
            namePanel.ShadowColor = Color.Black;
            namePanel.ShadowShift = 7;
            namePanel.Size = new Size(628, 38);
            namePanel.TabIndex = 43;
            namePanel.Visible = false;
            // 
            // btnClose
            // 
            btnClose.BorderRadius = 5;
            btnClose.CustomizableEdges = customizableEdges14;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.FromArgb(255, 128, 128);
            btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(12, 9);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges15;
            btnClose.Size = new Size(30, 20);
            btnClose.TabIndex = 32;
            btnClose.Text = "X";
            btnClose.Click += btnClose_Click_1;
            // 
            // lblComName
            // 
            lblComName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblComName.AutoSize = true;
            lblComName.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblComName.ForeColor = Color.White;
            lblComName.Location = new Point(218, 9);
            lblComName.Name = "lblComName";
            lblComName.Size = new Size(192, 20);
            lblComName.TabIndex = 31;
            lblComName.Text = "Owner Password Manager";
            // 
            // frmNewUser
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(628, 537);
            Controls.Add(namePanel);
            Controls.Add(userPanel);
            ForeColor = Color.FromArgb(2, 2, 2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmNewUser";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmNewUser";
            Load += frmNewUser_Load;
            userPanel.ResumeLayout(false);
            userPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)userImage).EndInit();
            namePanel.ResumeLayout(false);
            namePanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel userPanel;
        private Guna.UI2.WinForms.Guna2CirclePictureBox userImage;
        private Label lblPassMach;
        private Guna.UI2.WinForms.Guna2TextBox txtUser;
        private Guna.UI2.WinForms.Guna2Button btnChoseImage;
        private Guna.UI2.WinForms.Guna2TextBox txtPass;
        private Guna.UI2.WinForms.Guna2TextBox txtRepass;
        private Guna.UI2.WinForms.Guna2ComboBox comboBoxUser;
        private Label lblStaff;
        public Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2ShadowPanel namePanel;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Label lblComName;
    }
}