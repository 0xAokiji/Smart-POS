namespace pos.Settings
{
    partial class frmEnterpasskey
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
            smoothPanelTopConrner1 = new pos.Classes.SmoothPanelTopConrner();
            pictureBox1 = new PictureBox();
            smoothPanel_BottomCorner1 = new pos.Classes.SmoothPanel_BottomCorner();
            btnClose = new Guna.UI2.WinForms.Guna2Button();
            btnEnter = new Guna.UI2.WinForms.Guna2Button();
            smoothPanel1 = new pos.Classes.SmoothPanel();
            txtPasskey = new Guna.UI2.WinForms.Guna2TextBox();
            smoothPanelTopConrner1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            smoothPanel_BottomCorner1.SuspendLayout();
            smoothPanel1.SuspendLayout();
            SuspendLayout();
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
            smoothPanelTopConrner1.Size = new Size(327, 46);
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
            smoothPanel_BottomCorner1.Location = new Point(0, 137);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(327, 43);
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
            btnClose.CustomizableEdges = customizableEdges1;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.Red;
            btnClose.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.ImageAlign = HorizontalAlignment.Right;
            btnClose.ImageSize = new Size(30, 30);
            btnClose.Location = new Point(191, 6);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnClose.Size = new Size(107, 30);
            btnClose.TabIndex = 1;
            btnClose.Text = "Exite";
            btnClose.Click += btnClose_Click;
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
            btnEnter.CustomizableEdges = customizableEdges3;
            btnEnter.DisabledState.BorderColor = Color.DarkGray;
            btnEnter.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEnter.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEnter.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEnter.FillColor = Color.FromArgb(1, 95, 95);
            btnEnter.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEnter.ForeColor = Color.FromArgb(204, 204, 204);
            btnEnter.ImageAlign = HorizontalAlignment.Right;
            btnEnter.ImageSize = new Size(30, 30);
            btnEnter.Location = new Point(29, 6);
            btnEnter.Name = "btnEnter";
            btnEnter.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnEnter.Size = new Size(107, 30);
            btnEnter.TabIndex = 0;
            btnEnter.Text = "Enter";
            btnEnter.Click += btnEnter_Click;
            // 
            // smoothPanel1
            // 
            smoothPanel1.BorderColor = Color.Black;
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(txtPasskey);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 46);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(327, 91);
            smoothPanel1.TabIndex = 0;
            // 
            // txtPasskey
            // 
            txtPasskey.BorderColor = Color.FromArgb(1, 95, 95);
            txtPasskey.BorderRadius = 8;
            txtPasskey.CustomizableEdges = customizableEdges5;
            txtPasskey.DefaultText = "";
            txtPasskey.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPasskey.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPasskey.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPasskey.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPasskey.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPasskey.Font = new Font("Segoe UI", 9F);
            txtPasskey.ForeColor = Color.Black;
            txtPasskey.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPasskey.Location = new Point(29, 27);
            txtPasskey.Name = "txtPasskey";
            txtPasskey.PlaceholderText = "Enter The Passkey";
            txtPasskey.SelectedText = "";
            txtPasskey.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtPasskey.Size = new Size(269, 36);
            txtPasskey.TabIndex = 0;
            // 
            // frmEnterpasskey
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(327, 180);
            Controls.Add(smoothPanel1);
            Controls.Add(smoothPanel_BottomCorner1);
            Controls.Add(smoothPanelTopConrner1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmEnterpasskey";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmEnterpasskey";
            smoothPanelTopConrner1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            smoothPanel_BottomCorner1.ResumeLayout(false);
            smoothPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Classes.SmoothPanelTopConrner smoothPanelTopConrner1;
        private PictureBox pictureBox1;
        private Classes.SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnEnter;
        private Classes.SmoothPanel smoothPanel1;
        private Guna.UI2.WinForms.Guna2TextBox txtPasskey;
    }
}