using pos.Classes;

namespace pos
{
    partial class frmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges33 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges34 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges35 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges36 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            label3 = new Label();
            txtUser = new Guna.UI2.WinForms.Guna2TextBox();
            txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            btnExit = new Guna.UI2.WinForms.Guna2Button();
            btnLogin = new Guna.UI2.WinForms.Guna2Button();
            guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            notifyIcon1 = new NotifyIcon(components);
            guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            smoothPanelTopConrner1 = new SmoothPanelTopConrner();
            smoothPanel_BottomCorner1 = new SmoothPanel_BottomCorner();
            smoothPanel1 = new SmoothPanel();
            spLine = new Guna.UI2.WinForms.Guna2Separator();
            picArrow = new PictureBox();
            currentFlowPanelPerson = new FlowLayoutPanel();
            cbRememberMe = new Guna.UI2.WinForms.Guna2CheckBox();
            lblForgetPass = new Label();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox2).BeginInit();
            smoothPanelTopConrner1.SuspendLayout();
            smoothPanel_BottomCorner1.SuspendLayout();
            smoothPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picArrow).BeginInit();
            SuspendLayout();
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guna2PictureBox1.BackColor = Color.Transparent;
            guna2PictureBox1.CustomizableEdges = customizableEdges25;
            guna2PictureBox1.Image = Properties.Resources.avatar;
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(138, 70);
            guna2PictureBox1.Margin = new Padding(3, 2, 3, 2);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges26;
            guna2PictureBox1.Size = new Size(114, 102);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox1.TabIndex = 12;
            guna2PictureBox1.TabStop = false;
            guna2PictureBox1.UseTransparentBackground = true;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(204, 204, 204);
            label3.Location = new Point(133, 174);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.Yes;
            label3.Size = new Size(124, 25);
            label3.TabIndex = 11;
            label3.Text = "تسجيل الدخول";
            // 
            // txtUser
            // 
            txtUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtUser.BorderColor = Color.FromArgb(1, 95, 95);
            txtUser.BorderRadius = 8;
            txtUser.BorderThickness = 2;
            txtUser.CustomizableEdges = customizableEdges27;
            txtUser.DefaultText = "";
            txtUser.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtUser.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtUser.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtUser.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtUser.FillColor = Color.FromArgb(243, 243, 243);
            txtUser.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUser.Font = new Font("Segoe UI", 11.25F);
            txtUser.ForeColor = Color.FromArgb(51, 51, 51);
            txtUser.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUser.Location = new Point(19, 40);
            txtUser.Margin = new Padding(3, 4, 3, 4);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "اسم المستخدم";
            txtUser.RightToLeft = RightToLeft.No;
            txtUser.SelectedText = "";
            txtUser.ShadowDecoration.CustomizableEdges = customizableEdges28;
            txtUser.Size = new Size(352, 40);
            txtUser.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            txtUser.TabIndex = 0;
            txtUser.TextAlign = HorizontalAlignment.Right;
            txtUser.TextOffset = new Point(5, 0);
            txtUser.TextChanged += txtUser_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtPassword.BorderColor = Color.FromArgb(1, 95, 95);
            txtPassword.BorderRadius = 8;
            txtPassword.BorderThickness = 2;
            txtPassword.CustomizableEdges = customizableEdges29;
            txtPassword.DefaultText = "";
            txtPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.FillColor = Color.FromArgb(243, 243, 243);
            txtPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.Font = new Font("Segoe UI", 11.25F);
            txtPassword.ForeColor = Color.FromArgb(51, 51, 51);
            txtPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.IconRightOffset = new Point(2, 0);
            txtPassword.Location = new Point(19, 88);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderText = "كلمة المرور";
            txtPassword.RightToLeft = RightToLeft.No;
            txtPassword.SelectedText = "";
            txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges30;
            txtPassword.Size = new Size(352, 37);
            txtPassword.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            txtPassword.TabIndex = 1;
            txtPassword.TextAlign = HorizontalAlignment.Right;
            txtPassword.TextOffset = new Point(5, 0);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.IconRightClick += txtPassword_IconRightClick;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExit.BorderRadius = 8;
            btnExit.CustomizableEdges = customizableEdges31;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.Red;
            btnExit.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = Color.FromArgb(204, 204, 204);
            btnExit.Location = new Point(139, 18);
            btnExit.Margin = new Padding(3, 2, 3, 2);
            btnExit.Name = "btnExit";
            btnExit.ShadowDecoration.CustomizableEdges = customizableEdges32;
            btnExit.Size = new Size(111, 34);
            btnExit.TabIndex = 3;
            btnExit.Text = "خروج";
            btnExit.Click += guna2Button2_Click;
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogin.BackgroundImageLayout = ImageLayout.Center;
            btnLogin.BorderRadius = 8;
            btnLogin.CustomizableEdges = customizableEdges33;
            btnLogin.DisabledState.BorderColor = Color.DarkGray;
            btnLogin.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLogin.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLogin.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLogin.Enabled = false;
            btnLogin.FillColor = Color.FromArgb(1, 95, 95);
            btnLogin.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.FromArgb(204, 204, 204);
            btnLogin.Location = new Point(267, 18);
            btnLogin.Margin = new Padding(3, 2, 3, 2);
            btnLogin.Name = "btnLogin";
            btnLogin.ShadowDecoration.CustomizableEdges = customizableEdges34;
            btnLogin.Size = new Size(111, 34);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "دخول";
            btnLogin.Click += btnLogin_Click;
            // 
            // guna2MessageDialog1
            // 
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Caption = "تحذير";
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
            guna2MessageDialog1.Parent = this;
            guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            guna2MessageDialog1.Text = null;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // guna2PictureBox2
            // 
            guna2PictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guna2PictureBox2.BackColor = Color.Transparent;
            guna2PictureBox2.CustomizableEdges = customizableEdges35;
            guna2PictureBox2.FillColor = Color.FromArgb(1, 95, 95);
            guna2PictureBox2.Image = Properties.Resources.setting_dark1;
            guna2PictureBox2.ImageRotate = 0F;
            guna2PictureBox2.Location = new Point(4, 11);
            guna2PictureBox2.Margin = new Padding(3, 2, 3, 2);
            guna2PictureBox2.Name = "guna2PictureBox2";
            guna2PictureBox2.ShadowDecoration.CustomizableEdges = customizableEdges36;
            guna2PictureBox2.Size = new Size(35, 35);
            guna2PictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox2.TabIndex = 13;
            guna2PictureBox2.TabStop = false;
            guna2PictureBox2.UseTransparentBackground = true;
            guna2PictureBox2.Click += guna2PictureBox2_Click;
            // 
            // smoothPanelTopConrner1
            // 
            smoothPanelTopConrner1.BackColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderSize = 1F;
            smoothPanelTopConrner1.Controls.Add(guna2PictureBox2);
            smoothPanelTopConrner1.Controls.Add(guna2PictureBox1);
            smoothPanelTopConrner1.Controls.Add(label3);
            smoothPanelTopConrner1.Dock = DockStyle.Top;
            smoothPanelTopConrner1.Location = new Point(0, 0);
            smoothPanelTopConrner1.Name = "smoothPanelTopConrner1";
            smoothPanelTopConrner1.Size = new Size(390, 243);
            smoothPanelTopConrner1.TabIndex = 8;
            smoothPanelTopConrner1.MouseDown += smoothPanelTopConrner1_MouseDown;
            // 
            // smoothPanel_BottomCorner1
            // 
            smoothPanel_BottomCorner1.BackColor = Color.FromArgb(230, 230, 230);
            smoothPanel_BottomCorner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel_BottomCorner1.BorderSize = 1F;
            smoothPanel_BottomCorner1.Controls.Add(btnExit);
            smoothPanel_BottomCorner1.Controls.Add(btnLogin);
            smoothPanel_BottomCorner1.Dock = DockStyle.Bottom;
            smoothPanel_BottomCorner1.Location = new Point(0, 436);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(390, 63);
            smoothPanel_BottomCorner1.TabIndex = 9;
            // 
            // smoothPanel1
            // 
            smoothPanel1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(lblForgetPass);
            smoothPanel1.Controls.Add(spLine);
            smoothPanel1.Controls.Add(picArrow);
            smoothPanel1.Controls.Add(currentFlowPanelPerson);
            smoothPanel1.Controls.Add(cbRememberMe);
            smoothPanel1.Controls.Add(txtUser);
            smoothPanel1.Controls.Add(txtPassword);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 243);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(390, 193);
            smoothPanel1.TabIndex = 10;
            // 
            // spLine
            // 
            spLine.FillColor = Color.FromArgb(51, 51, 51);
            spLine.FillThickness = 2;
            spLine.Location = new Point(13, 158);
            spLine.Name = "spLine";
            spLine.Size = new Size(334, 14);
            spLine.TabIndex = 6;
            // 
            // picArrow
            // 
            picArrow.Image = Properties.Resources.down;
            picArrow.Location = new Point(345, 157);
            picArrow.Name = "picArrow";
            picArrow.Size = new Size(33, 20);
            picArrow.SizeMode = PictureBoxSizeMode.Zoom;
            picArrow.TabIndex = 5;
            picArrow.TabStop = false;
            picArrow.Click += picArrow_Click;
            // 
            // currentFlowPanelPerson
            // 
            currentFlowPanelPerson.Anchor = AnchorStyles.Top;
            currentFlowPanelPerson.AutoScroll = true;
            currentFlowPanelPerson.Location = new Point(8, 192);
            currentFlowPanelPerson.Name = "currentFlowPanelPerson";
            currentFlowPanelPerson.RightToLeft = RightToLeft.No;
            currentFlowPanelPerson.Size = new Size(374, 183);
            currentFlowPanelPerson.TabIndex = 3;
            // 
            // cbRememberMe
            // 
            cbRememberMe.AutoSize = true;
            cbRememberMe.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
            cbRememberMe.CheckedState.BorderRadius = 0;
            cbRememberMe.CheckedState.BorderThickness = 0;
            cbRememberMe.CheckedState.FillColor = Color.FromArgb(1, 95, 95);
            cbRememberMe.Location = new Point(311, 132);
            cbRememberMe.Name = "cbRememberMe";
            cbRememberMe.RightToLeft = RightToLeft.Yes;
            cbRememberMe.Size = new Size(60, 19);
            cbRememberMe.TabIndex = 2;
            cbRememberMe.Text = "تذكرني";
            cbRememberMe.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            cbRememberMe.UncheckedState.BorderRadius = 0;
            cbRememberMe.UncheckedState.BorderThickness = 0;
            cbRememberMe.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            // 
            // lblForgetPass
            // 
            lblForgetPass.AutoSize = true;
            lblForgetPass.Font = new Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, 0);
            lblForgetPass.ForeColor = Color.FromArgb(255, 128, 128);
            lblForgetPass.Location = new Point(19, 132);
            lblForgetPass.Name = "lblForgetPass";
            lblForgetPass.Size = new Size(118, 15);
            lblForgetPass.TabIndex = 7;
            lblForgetPass.Text = "هل نسيت كلمة المرور؟";
            lblForgetPass.Click += lblForgetPass_Click;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(390, 499);
            Controls.Add(smoothPanel1);
            Controls.Add(smoothPanel_BottomCorner1);
            Controls.Add(smoothPanelTopConrner1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmLogin";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += frmLogin_Load;
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox2).EndInit();
            smoothPanelTopConrner1.ResumeLayout(false);
            smoothPanelTopConrner1.PerformLayout();
            smoothPanel_BottomCorner1.ResumeLayout(false);
            smoothPanel1.ResumeLayout(false);
            smoothPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picArrow).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2TextBox txtUser;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2Button btnExit;
        private Label label3;
        private Guna.UI2.WinForms.Guna2Button btnLogin;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
        private NotifyIcon notifyIcon1;
        private SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
        private SmoothPanelTopConrner smoothPanelTopConrner1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private SmoothPanel smoothPanel1;
        private Guna.UI2.WinForms.Guna2CheckBox cbRememberMe;
        private FlowLayoutPanel currentFlowPanelPerson;
        private Guna.UI2.WinForms.Guna2Separator spLine;
        private PictureBox picArrow;
        private Label lblForgetPass;
    }
}
