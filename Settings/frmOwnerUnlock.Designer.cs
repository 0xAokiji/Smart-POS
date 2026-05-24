namespace pos.Settings
{
    partial class frmOwnerUnlock
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
            smoothPanel1 = new pos.Classes.SmoothPanel();
            txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            smoothPanelTopConrner1 = new pos.Classes.SmoothPanelTopConrner();
            pictureBox1 = new PictureBox();
            smoothPanel_BottomCorner1 = new pos.Classes.SmoothPanel_BottomCorner();
            btnClose = new Guna.UI2.WinForms.Guna2Button();
            btnEnter = new Guna.UI2.WinForms.Guna2Button();
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
            smoothPanel1.Controls.Add(txtPassword);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 46);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(321, 87);
            smoothPanel1.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.BorderColor = Color.FromArgb(1, 95, 95);
            txtPassword.BorderRadius = 8;
            txtPassword.CustomizableEdges = customizableEdges1;
            txtPassword.DefaultText = "";
            txtPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.ForeColor = Color.Black;
            txtPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.Location = new Point(26, 25);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Enter The Passkey";
            txtPassword.SelectedText = "";
            txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPassword.Size = new Size(269, 36);
            txtPassword.TabIndex = 0;
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
            smoothPanelTopConrner1.Size = new Size(321, 46);
            smoothPanelTopConrner1.TabIndex = 2;
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
            smoothPanel_BottomCorner1.Controls.Add(btnEnter);
            smoothPanel_BottomCorner1.Dock = DockStyle.Bottom;
            smoothPanel_BottomCorner1.Location = new Point(0, 133);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(321, 43);
            smoothPanel_BottomCorner1.TabIndex = 1;
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
            btnClose.CustomizableEdges = customizableEdges3;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.Red;
            btnClose.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.ImageAlign = HorizontalAlignment.Right;
            btnClose.ImageSize = new Size(30, 30);
            btnClose.Location = new Point(188, 6);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnClose.Size = new Size(107, 30);
            btnClose.TabIndex = 1;
            btnClose.Text = "Exite";
            btnClose.Click += btnCancel_Click;
            // 
            // btnEnter
            // 
            btnEnter.BorderColor = Color.FromArgb(230, 230, 230);
            btnEnter.BorderRadius = 8;
            btnEnter.BorderThickness = 2;
            btnEnter.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
            btnEnter.CheckedState.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnEnter.CheckedState.ForeColor = Color.White;
            btnEnter.CheckedState.Image = Properties.Resources.logout_light;
            btnEnter.CustomizableEdges = customizableEdges5;
            btnEnter.DisabledState.BorderColor = Color.DarkGray;
            btnEnter.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEnter.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEnter.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEnter.FillColor = Color.FromArgb(1, 95, 95);
            btnEnter.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEnter.ForeColor = Color.FromArgb(204, 204, 204);
            btnEnter.ImageAlign = HorizontalAlignment.Right;
            btnEnter.ImageSize = new Size(30, 30);
            btnEnter.Location = new Point(26, 6);
            btnEnter.Name = "btnEnter";
            btnEnter.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEnter.Size = new Size(107, 30);
            btnEnter.TabIndex = 0;
            btnEnter.Text = "Enter";
            btnEnter.Click += btnOK_Click;
            // 
            // frmOwnerUnlock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(321, 176);
            Controls.Add(smoothPanel1);
            Controls.Add(smoothPanelTopConrner1);
            Controls.Add(smoothPanel_BottomCorner1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmOwnerUnlock";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmOwnerUnlock";
            smoothPanel1.ResumeLayout(false);
            smoothPanelTopConrner1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            smoothPanel_BottomCorner1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Classes.SmoothPanel smoothPanel1;
        private Guna.UI2.WinForms.Guna2TextBox txtPasskey;
        private Classes.SmoothPanelTopConrner smoothPanelTopConrner1;
        private PictureBox pictureBox1;
        private Classes.SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnEnter;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
    }
}