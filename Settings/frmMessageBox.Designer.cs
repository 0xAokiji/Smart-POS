namespace pos.Settings
{
    partial class frmMessageBox
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
            mainPanel = new Panel();
            therdPanel = new Panel();
            btnOK = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            picIcon = new Guna.UI2.WinForms.Guna2PictureBox();
            seconandPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            lblMessage = new Label();
            lblTitel = new Label();
            mainPanel.SuspendLayout();
            therdPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picIcon).BeginInit();
            seconandPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(therdPanel);
            mainPanel.Controls.Add(picIcon);
            mainPanel.Controls.Add(seconandPanel);
            mainPanel.Controls.Add(lblTitel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(421, 204);
            mainPanel.TabIndex = 0;
            mainPanel.Paint += mainPanel_Paint;
            // 
            // therdPanel
            // 
            therdPanel.BackColor = Color.FromArgb(230, 230, 230);
            therdPanel.Controls.Add(btnOK);
            therdPanel.Controls.Add(btnCancel);
            therdPanel.Dock = DockStyle.Bottom;
            therdPanel.Location = new Point(0, 158);
            therdPanel.Name = "therdPanel";
            therdPanel.Size = new Size(421, 46);
            therdPanel.TabIndex = 46;
            therdPanel.Paint += therdPanel_Paint;
            // 
            // btnOK
            // 
            btnOK.BorderRadius = 10;
            btnOK.CustomizableEdges = customizableEdges1;
            btnOK.DisabledState.BorderColor = Color.DarkGray;
            btnOK.DisabledState.CustomBorderColor = Color.DarkGray;
            btnOK.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnOK.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnOK.FillColor = Color.FromArgb(136, 214, 218);
            btnOK.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(225, 8);
            btnOK.Name = "btnOK";
            btnOK.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnOK.Size = new Size(120, 30);
            btnOK.TabIndex = 19;
            btnOK.Text = "موافق";
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 10;
            btnCancel.CustomizableEdges = customizableEdges3;
            btnCancel.DisabledState.BorderColor = Color.DarkGray;
            btnCancel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancel.FillColor = Color.Red;
            btnCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(76, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnCancel.Size = new Size(120, 30);
            btnCancel.TabIndex = 20;
            btnCancel.Text = "الغاء";
            btnCancel.Click += btnCancel_Click;
            // 
            // picIcon
            // 
            picIcon.CustomizableEdges = customizableEdges5;
            picIcon.Image = Properties.Resources.error;
            picIcon.ImageRotate = 0F;
            picIcon.Location = new Point(12, 7);
            picIcon.Name = "picIcon";
            picIcon.ShadowDecoration.CustomizableEdges = customizableEdges6;
            picIcon.Size = new Size(30, 30);
            picIcon.SizeMode = PictureBoxSizeMode.Zoom;
            picIcon.TabIndex = 45;
            picIcon.TabStop = false;
            // 
            // seconandPanel
            // 
            seconandPanel.BackColor = Color.Transparent;
            seconandPanel.Controls.Add(lblMessage);
            seconandPanel.FillColor = Color.FromArgb(230, 230, 230);
            seconandPanel.Location = new Point(12, 42);
            seconandPanel.Name = "seconandPanel";
            seconandPanel.Radius = 7;
            seconandPanel.ShadowColor = Color.Black;
            seconandPanel.Size = new Size(397, 101);
            seconandPanel.TabIndex = 44;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMessage.ForeColor = Color.FromArgb(51, 51, 51);
            lblMessage.Location = new Point(177, 42);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(45, 17);
            lblMessage.TabIndex = 21;
            lblMessage.Text = "label1";
            // 
            // lblTitel
            // 
            lblTitel.AutoSize = true;
            lblTitel.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitel.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitel.Location = new Point(364, 9);
            lblTitel.Name = "lblTitel";
            lblTitel.Size = new Size(51, 20);
            lblTitel.TabIndex = 22;
            lblTitel.Text = "label2";
            // 
            // frmMessageBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(421, 204);
            Controls.Add(mainPanel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmMessageBox";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmMessageBox";
            Paint += frmMessageBox_Paint;
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            therdPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picIcon).EndInit();
            seconandPanel.ResumeLayout(false);
            seconandPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Label lblTitel;
        private Label lblMessage;
        public Guna.UI2.WinForms.Guna2Button btnCancel;
        public Guna.UI2.WinForms.Guna2Button btnOK;
        private Guna.UI2.WinForms.Guna2ShadowPanel seconandPanel;
        private Guna.UI2.WinForms.Guna2PictureBox picIcon;
        private Panel therdPanel;
    }
}