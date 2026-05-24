namespace pos.SystemApp
{
    partial class frmResetUserPassword
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
            smoothPanel1 = new pos.Classes.SmoothPanel();
            txtPass = new Guna.UI2.WinForms.Guna2TextBox();
            lblPassMach = new Label();
            txtRepass = new Guna.UI2.WinForms.Guna2TextBox();
            smoothPanelTopConrner1 = new pos.Classes.SmoothPanelTopConrner();
            pictureBox1 = new PictureBox();
            smoothPanel_BottomCorner1 = new pos.Classes.SmoothPanel_BottomCorner();
            btnClose = new Guna.UI2.WinForms.Guna2Button();
            btnSavePass = new Guna.UI2.WinForms.Guna2Button();
            messageBox = new Guna.UI2.WinForms.Guna2MessageDialog();
            smoothPanel1.SuspendLayout();
            smoothPanelTopConrner1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            smoothPanel_BottomCorner1.SuspendLayout();
            SuspendLayout();
            // 
            // smoothPanel1
            // 
            smoothPanel1.BorderColor = Color.Black;
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(txtPass);
            smoothPanel1.Controls.Add(lblPassMach);
            smoothPanel1.Controls.Add(txtRepass);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 46);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(325, 133);
            smoothPanel1.TabIndex = 8;
            // 
            // txtPass
            // 
            txtPass.BorderColor = Color.FromArgb(1, 95, 95);
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
            txtPass.Location = new Point(16, 18);
            txtPass.Margin = new Padding(3, 4, 3, 4);
            txtPass.Name = "txtPass";
            txtPass.Padding = new Padding(3, 0, 3, 0);
            txtPass.PasswordChar = '●';
            txtPass.PlaceholderText = "كلمة المرور الجديدة";
            txtPass.SelectedText = "";
            txtPass.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPass.Size = new Size(293, 36);
            txtPass.TabIndex = 5;
            txtPass.TextAlign = HorizontalAlignment.Right;
            txtPass.IconRightClick += txtPass_IconRightClick;
            txtPass.TextChanged += txtPass_TextChanged;
            // 
            // lblPassMach
            // 
            lblPassMach.AutoSize = true;
            lblPassMach.Font = new Font("Segoe UI", 8.5F);
            lblPassMach.ForeColor = Color.Red;
            lblPassMach.Location = new Point(185, 100);
            lblPassMach.Name = "lblPassMach";
            lblPassMach.RightToLeft = RightToLeft.Yes;
            lblPassMach.Size = new Size(124, 15);
            lblPassMach.TabIndex = 7;
            lblPassMach.Text = "كلمة المرور غير متطابقة";
            lblPassMach.TextAlign = ContentAlignment.MiddleCenter;
            lblPassMach.Visible = false;
            // 
            // txtRepass
            // 
            txtRepass.BorderColor = Color.FromArgb(1, 95, 95);
            txtRepass.BorderRadius = 8;
            txtRepass.CustomizableEdges = customizableEdges3;
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
            txtRepass.Location = new Point(16, 60);
            txtRepass.Margin = new Padding(3, 4, 3, 4);
            txtRepass.Name = "txtRepass";
            txtRepass.Padding = new Padding(3, 0, 3, 0);
            txtRepass.PasswordChar = '●';
            txtRepass.PlaceholderText = "اعد كتابة كلمة المرور الجديدة";
            txtRepass.SelectedText = "";
            txtRepass.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtRepass.Size = new Size(293, 36);
            txtRepass.TabIndex = 6;
            txtRepass.TextAlign = HorizontalAlignment.Right;
            txtRepass.IconRightClick += txtRepass_IconRightClick;
            txtRepass.TextChanged += txtRepass_TextChanged;
            // 
            // smoothPanelTopConrner1
            // 
            smoothPanelTopConrner1.BackColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderColor = Color.Black;
            smoothPanelTopConrner1.BorderSize = 1F;
            smoothPanelTopConrner1.Controls.Add(pictureBox1);
            smoothPanelTopConrner1.Dock = DockStyle.Top;
            smoothPanelTopConrner1.Location = new Point(0, 0);
            smoothPanelTopConrner1.Name = "smoothPanelTopConrner1";
            smoothPanelTopConrner1.Size = new Size(325, 46);
            smoothPanelTopConrner1.TabIndex = 6;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.password;
            pictureBox1.Location = new Point(265, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // smoothPanel_BottomCorner1
            // 
            smoothPanel_BottomCorner1.BackColor = Color.FromArgb(230, 230, 230);
            smoothPanel_BottomCorner1.BorderColor = Color.Black;
            smoothPanel_BottomCorner1.BorderSize = 1F;
            smoothPanel_BottomCorner1.Controls.Add(btnClose);
            smoothPanel_BottomCorner1.Controls.Add(btnSavePass);
            smoothPanel_BottomCorner1.Dock = DockStyle.Bottom;
            smoothPanel_BottomCorner1.Location = new Point(0, 179);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(325, 43);
            smoothPanel_BottomCorner1.TabIndex = 7;
            // 
            // btnClose
            // 
            btnClose.BorderColor = Color.FromArgb(230, 230, 230);
            btnClose.BorderRadius = 8;
            btnClose.BorderThickness = 2;
            btnClose.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
            btnClose.CheckedState.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnClose.CheckedState.ForeColor = Color.White;
            btnClose.CheckedState.Image = Properties.Resources.logout_light;
            btnClose.CustomizableEdges = customizableEdges5;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.Red;
            btnClose.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.ImageAlign = HorizontalAlignment.Right;
            btnClose.ImageSize = new Size(30, 30);
            btnClose.Location = new Point(39, 6);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnClose.Size = new Size(107, 30);
            btnClose.TabIndex = 14;
            btnClose.Text = "خروج";
            btnClose.Click += btnClose_Click;
            // 
            // btnSavePass
            // 
            btnSavePass.BorderColor = Color.FromArgb(230, 230, 230);
            btnSavePass.BorderRadius = 8;
            btnSavePass.BorderThickness = 2;
            btnSavePass.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
            btnSavePass.CheckedState.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnSavePass.CheckedState.ForeColor = Color.White;
            btnSavePass.CheckedState.Image = Properties.Resources.logout_light;
            btnSavePass.CustomizableEdges = customizableEdges7;
            btnSavePass.DisabledState.BorderColor = Color.DarkGray;
            btnSavePass.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSavePass.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSavePass.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSavePass.Enabled = false;
            btnSavePass.FillColor = Color.FromArgb(1, 95, 95);
            btnSavePass.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSavePass.ForeColor = Color.FromArgb(204, 204, 204);
            btnSavePass.ImageAlign = HorizontalAlignment.Right;
            btnSavePass.ImageSize = new Size(30, 30);
            btnSavePass.Location = new Point(179, 6);
            btnSavePass.Name = "btnSavePass";
            btnSavePass.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnSavePass.Size = new Size(107, 30);
            btnSavePass.TabIndex = 13;
            btnSavePass.Text = "حفظ";
            btnSavePass.Click += btnSavePass_Click;
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
            // frmResetUserPassword
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(325, 222);
            Controls.Add(smoothPanel1);
            Controls.Add(smoothPanelTopConrner1);
            Controls.Add(smoothPanel_BottomCorner1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmResetUserPassword";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmResetUserPassword";
            Load += frmResetUserPassword_Load;
            smoothPanel1.ResumeLayout(false);
            smoothPanel1.PerformLayout();
            smoothPanelTopConrner1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            smoothPanel_BottomCorner1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Classes.SmoothPanel smoothPanel1;
        private Classes.SmoothPanelTopConrner smoothPanelTopConrner1;
        private PictureBox pictureBox1;
        private Classes.SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnSavePass;
        private Guna.UI2.WinForms.Guna2TextBox txtPass;
        private Label lblPassMach;
        private Guna.UI2.WinForms.Guna2TextBox txtRepass;
        private Guna.UI2.WinForms.Guna2MessageDialog messageBox;
    }
}