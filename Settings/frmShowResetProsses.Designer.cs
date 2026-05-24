using Guna.UI2.WinForms;

namespace pos.Settings
{
    partial class frmShowResetProsses
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowResetProsses));
            label2 = new Label();
            lblTableName = new Label();
            pbReset = new Guna2ProgressBar();
            label1 = new Label();
            lblTackTime = new Label();
            lblProsseName = new Label();
            resetPanel = new Panel();
            lblSpend = new Label();
            smoothPanel1 = new pos.Classes.SmoothPanel();
            btnExit = new Guna2Button();
            btnFull = new Guna2Button();
            btnStartReset = new Guna2Button();
            bottomPanel = new pos.Classes.SmoothPanel_BottomCorner();
            label5 = new Label();
            pictureBox1 = new PictureBox();
            smoothPanelTopConrner1 = new pos.Classes.SmoothPanelTopConrner();
            resetPanel.SuspendLayout();
            smoothPanel1.SuspendLayout();
            bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            smoothPanelTopConrner1.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(4, 13);
            label2.Name = "label2";
            label2.Size = new Size(73, 17);
            label2.TabIndex = 2;
            label2.Text = "Time Taken";
            // 
            // lblTableName
            // 
            lblTableName.AutoSize = true;
            lblTableName.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTableName.Location = new Point(4, 129);
            lblTableName.Name = "lblTableName";
            lblTableName.Size = new Size(42, 13);
            lblTableName.TabIndex = 8;
            lblTableName.Text = "0 MB/s";
            lblTableName.TextAlign = ContentAlignment.MiddleLeft;
            lblTableName.Visible = false;
            // 
            // pbReset
            // 
            pbReset.BorderColor = Color.FromArgb(1, 95, 95);
            pbReset.BorderRadius = 5;
            pbReset.BorderThickness = 1;
            pbReset.CustomizableEdges = customizableEdges1;
            pbReset.Location = new Point(4, 67);
            pbReset.Name = "pbReset";
            pbReset.ProgressColor2 = Color.FromArgb(1, 95, 95);
            pbReset.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pbReset.ShowText = true;
            pbReset.Size = new Size(482, 30);
            pbReset.TabIndex = 0;
            pbReset.Text = "guna2ProgressBar1";
            pbReset.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(4, 39);
            label1.Name = "label1";
            label1.Size = new Size(113, 17);
            label1.TabIndex = 1;
            label1.Text = "Number of Tables";
            // 
            // lblTackTime
            // 
            lblTackTime.AutoSize = true;
            lblTackTime.Location = new Point(132, 41);
            lblTackTime.Name = "lblTackTime";
            lblTackTime.Size = new Size(13, 15);
            lblTackTime.TabIndex = 6;
            lblTackTime.Text = "0";
            // 
            // lblProsseName
            // 
            lblProsseName.AutoSize = true;
            lblProsseName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblProsseName.Location = new Point(4, 104);
            lblProsseName.Name = "lblProsseName";
            lblProsseName.Size = new Size(40, 15);
            lblProsseName.TabIndex = 4;
            lblProsseName.Text = "السرعة";
            lblProsseName.TextAlign = ContentAlignment.MiddleLeft;
            lblProsseName.Visible = false;
            // 
            // resetPanel
            // 
            resetPanel.Controls.Add(label2);
            resetPanel.Controls.Add(lblTableName);
            resetPanel.Controls.Add(pbReset);
            resetPanel.Controls.Add(label1);
            resetPanel.Controls.Add(lblTackTime);
            resetPanel.Controls.Add(lblSpend);
            resetPanel.Controls.Add(lblProsseName);
            resetPanel.Enabled = false;
            resetPanel.Location = new Point(8, 6);
            resetPanel.Name = "resetPanel";
            resetPanel.Size = new Size(491, 170);
            resetPanel.TabIndex = 9;
            // 
            // lblSpend
            // 
            lblSpend.AutoSize = true;
            lblSpend.Location = new Point(92, 15);
            lblSpend.Name = "lblSpend";
            lblSpend.Size = new Size(49, 15);
            lblSpend.TabIndex = 5;
            lblSpend.Text = "00:00:00";
            // 
            // smoothPanel1
            // 
            smoothPanel1.BackColor = Color.FromArgb(243, 243, 243);
            smoothPanel1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(resetPanel);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 45);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(506, 184);
            smoothPanel1.TabIndex = 5;
            // 
            // btnExit
            // 
            btnExit.BorderRadius = 8;
            btnExit.CustomizableEdges = customizableEdges3;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.Green;
            btnExit.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(174, 13);
            btnExit.Name = "btnExit";
            btnExit.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnExit.Size = new Size(123, 30);
            btnExit.TabIndex = 61;
            btnExit.Text = "الغاء";
            btnExit.TextOffset = new Point(0, -3);
            btnExit.Click += btnExit_Click;
            // 
            // btnFull
            // 
            btnFull.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFull.BorderRadius = 8;
            btnFull.CustomizableEdges = customizableEdges5;
            btnFull.DisabledState.BorderColor = Color.DarkGray;
            btnFull.DisabledState.CustomBorderColor = Color.DarkGray;
            btnFull.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnFull.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnFull.FillColor = Color.FromArgb(1, 95, 95);
            btnFull.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFull.ForeColor = Color.White;
            btnFull.Location = new Point(626, 13);
            btnFull.Name = "btnFull";
            btnFull.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnFull.Size = new Size(169, 30);
            btnFull.TabIndex = 59;
            btnFull.Text = "نسخة احتياطية كاملة";
            // 
            // btnStartReset
            // 
            btnStartReset.BorderRadius = 8;
            btnStartReset.CustomizableEdges = customizableEdges7;
            btnStartReset.DisabledState.BorderColor = Color.DarkGray;
            btnStartReset.DisabledState.CustomBorderColor = Color.DarkGray;
            btnStartReset.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnStartReset.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnStartReset.FillColor = Color.Red;
            btnStartReset.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartReset.ForeColor = Color.White;
            btnStartReset.Location = new Point(312, 13);
            btnStartReset.Name = "btnStartReset";
            btnStartReset.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnStartReset.Size = new Size(169, 30);
            btnStartReset.TabIndex = 60;
            btnStartReset.Text = "إعادة ضبط النظام";
            btnStartReset.Click += btnStartReset_Click;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(230, 230, 230);
            bottomPanel.BorderColor = Color.FromArgb(1, 95, 95);
            bottomPanel.BorderSize = 1F;
            bottomPanel.Controls.Add(btnExit);
            bottomPanel.Controls.Add(btnFull);
            bottomPanel.Controls.Add(btnStartReset);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 229);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(506, 56);
            bottomPanel.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(204, 204, 204);
            label5.Location = new Point(329, 12);
            label5.Name = "label5";
            label5.Size = new Size(123, 20);
            label5.TabIndex = 6;
            label5.Text = "إعادة ضبط النظام";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(450, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // smoothPanelTopConrner1
            // 
            smoothPanelTopConrner1.BackColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderSize = 1F;
            smoothPanelTopConrner1.Controls.Add(label5);
            smoothPanelTopConrner1.Controls.Add(pictureBox1);
            smoothPanelTopConrner1.Dock = DockStyle.Top;
            smoothPanelTopConrner1.Location = new Point(0, 0);
            smoothPanelTopConrner1.Name = "smoothPanelTopConrner1";
            smoothPanelTopConrner1.Size = new Size(506, 45);
            smoothPanelTopConrner1.TabIndex = 3;
            // 
            // frmShowResetProsses
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(506, 285);
            Controls.Add(smoothPanel1);
            Controls.Add(bottomPanel);
            Controls.Add(smoothPanelTopConrner1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmShowResetProsses";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmShowResetProsses";
            resetPanel.ResumeLayout(false);
            resetPanel.PerformLayout();
            smoothPanel1.ResumeLayout(false);
            bottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            smoothPanelTopConrner1.ResumeLayout(false);
            smoothPanelTopConrner1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label2;
        private Label lblTableName;
        private Label lblSpeed;
        private Guna.UI2.WinForms.Guna2ProgressBar pbBackup;
        private Label lblSize;
        private Guna2ProgressBar pbReset;
        private Label label1;
        private Label lblTackTime;
        private Label lblProsseName;
        private Label label4;
        private Label label3;
        private Panel resetPanel;
        private Label lblSpend;
        private Classes.SmoothPanel smoothPanel1;
        public Guna.UI2.WinForms.Guna2Button btnExit;
        public Guna.UI2.WinForms.Guna2Button btnFull;
        public Guna.UI2.WinForms.Guna2Button btnStartReset;
        private Classes.SmoothPanel_BottomCorner bottomPanel;
        private Label label5;
        private PictureBox pictureBox1;
        private Classes.SmoothPanelTopConrner smoothPanelTopConrner1;
    }
}