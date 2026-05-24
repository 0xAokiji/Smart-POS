using pos.Classes;

namespace pos.GeneralForms.MainForm
{
    partial class frmShowBackup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowBackup));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            smoothPanelTopConrner1 = new pos.Classes.SmoothPanelTopConrner();
            label5 = new Label();
            pictureBox1 = new PictureBox();
            bottomPanel = new pos.Classes.SmoothPanel_BottomCorner();
            btnExit = new Guna.UI2.WinForms.Guna2Button();
            btnFull = new Guna.UI2.WinForms.Guna2Button();
            btnDifferential = new Guna.UI2.WinForms.Guna2Button();
            smoothPanel1 = new pos.Classes.SmoothPanel();
            lblRemainingDays = new Label();
            lblLastDate = new Label();
            label7 = new Label();
            backupPanel = new Panel();
            label2 = new Label();
            lblSpeed = new Label();
            pbBackup = new Guna.UI2.WinForms.Guna2ProgressBar();
            lblSize = new Label();
            label1 = new Label();
            lblTackTime = new Label();
            label4 = new Label();
            lblSpend = new Label();
            label3 = new Label();
            smoothPanelTopConrner1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            bottomPanel.SuspendLayout();
            smoothPanel1.SuspendLayout();
            backupPanel.SuspendLayout();
            SuspendLayout();
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
            smoothPanelTopConrner1.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(204, 204, 204);
            label5.Location = new Point(335, 12);
            label5.Name = "label5";
            label5.Size = new Size(111, 20);
            label5.TabIndex = 6;
            label5.Text = "النسخ الاحتياطي";
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
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(230, 230, 230);
            bottomPanel.BorderColor = Color.FromArgb(1, 95, 95);
            bottomPanel.BorderSize = 1F;
            bottomPanel.Controls.Add(btnExit);
            bottomPanel.Controls.Add(btnFull);
            bottomPanel.Controls.Add(btnDifferential);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 251);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(506, 56);
            bottomPanel.TabIndex = 1;
            // 
            // btnExit
            // 
            btnExit.BorderRadius = 8;
            btnExit.CustomizableEdges = customizableEdges1;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.Red;
            btnExit.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(18, 13);
            btnExit.Name = "btnExit";
            btnExit.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnExit.Size = new Size(123, 30);
            btnExit.TabIndex = 61;
            btnExit.Text = "خروج";
            btnExit.TextOffset = new Point(0, -3);
            btnExit.Click += btnExit_Click;
            // 
            // btnFull
            // 
            btnFull.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFull.BorderRadius = 8;
            btnFull.CustomizableEdges = customizableEdges3;
            btnFull.DisabledState.BorderColor = Color.DarkGray;
            btnFull.DisabledState.CustomBorderColor = Color.DarkGray;
            btnFull.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnFull.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnFull.FillColor = Color.FromArgb(1, 95, 95);
            btnFull.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFull.ForeColor = Color.White;
            btnFull.Location = new Point(320, 13);
            btnFull.Name = "btnFull";
            btnFull.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnFull.Size = new Size(169, 30);
            btnFull.TabIndex = 59;
            btnFull.Text = "نسخة احتياطية كاملة";
            btnFull.Click += btnFull_Click;
            // 
            // btnDifferential
            // 
            btnDifferential.BorderRadius = 8;
            btnDifferential.CustomizableEdges = customizableEdges5;
            btnDifferential.DisabledState.BorderColor = Color.DarkGray;
            btnDifferential.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDifferential.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDifferential.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDifferential.FillColor = Color.FromArgb(136, 214, 218);
            btnDifferential.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDifferential.ForeColor = Color.FromArgb(51, 51, 51);
            btnDifferential.Location = new Point(146, 13);
            btnDifferential.Name = "btnDifferential";
            btnDifferential.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnDifferential.Size = new Size(169, 30);
            btnDifferential.TabIndex = 60;
            btnDifferential.Text = "نسخة احتياطية تفاضلية";
            btnDifferential.Click += btnDifferential_Click;
            // 
            // smoothPanel1
            // 
            smoothPanel1.BackColor = Color.FromArgb(243, 243, 243);
            smoothPanel1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(lblRemainingDays);
            smoothPanel1.Controls.Add(lblLastDate);
            smoothPanel1.Controls.Add(label7);
            smoothPanel1.Controls.Add(backupPanel);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 45);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(506, 206);
            smoothPanel1.TabIndex = 2;
            // 
            // lblRemainingDays
            // 
            lblRemainingDays.AutoSize = true;
            lblRemainingDays.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRemainingDays.Location = new Point(12, 181);
            lblRemainingDays.Name = "lblRemainingDays";
            lblRemainingDays.Size = new Size(78, 17);
            lblRemainingDays.TabIndex = 11;
            lblRemainingDays.Text = "الايام المتبقية";
            // 
            // lblLastDate
            // 
            lblLastDate.AutoSize = true;
            lblLastDate.Location = new Point(206, 181);
            lblLastDate.Name = "lblLastDate";
            lblLastDate.Size = new Size(136, 15);
            lblLastDate.TabIndex = 10;
            lblLastDate.Text = "dd-MM-yyyy HH:mm:ss";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(348, 179);
            label7.Name = "label7";
            label7.Size = new Size(148, 17);
            label7.TabIndex = 9;
            label7.Text = "تاريخ اخر عملية نسخ كاملة";
            // 
            // backupPanel
            // 
            backupPanel.Controls.Add(label2);
            backupPanel.Controls.Add(lblSpeed);
            backupPanel.Controls.Add(pbBackup);
            backupPanel.Controls.Add(lblSize);
            backupPanel.Controls.Add(label1);
            backupPanel.Controls.Add(lblTackTime);
            backupPanel.Controls.Add(label4);
            backupPanel.Controls.Add(lblSpend);
            backupPanel.Controls.Add(label3);
            backupPanel.Enabled = false;
            backupPanel.Location = new Point(8, 6);
            backupPanel.Name = "backupPanel";
            backupPanel.Size = new Size(491, 170);
            backupPanel.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(388, 11);
            label2.Name = "label2";
            label2.Size = new Size(98, 17);
            label2.TabIndex = 2;
            label2.Text = "الوقت المستغرق";
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new Point(334, 136);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(44, 15);
            lblSpeed.TabIndex = 8;
            lblSpeed.Text = "0 MB/s";
            // 
            // pbBackup
            // 
            pbBackup.BorderColor = Color.FromArgb(1, 95, 95);
            pbBackup.BorderRadius = 5;
            pbBackup.BorderThickness = 1;
            pbBackup.CustomizableEdges = customizableEdges7;
            pbBackup.Location = new Point(4, 101);
            pbBackup.Name = "pbBackup";
            pbBackup.ProgressColor2 = Color.FromArgb(1, 95, 95);
            pbBackup.ShadowDecoration.CustomizableEdges = customizableEdges8;
            pbBackup.ShowText = true;
            pbBackup.Size = new Size(482, 30);
            pbBackup.TabIndex = 0;
            pbBackup.Text = "guna2ProgressBar1";
            pbBackup.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // lblSize
            // 
            lblSize.AutoSize = true;
            lblSize.Location = new Point(281, 65);
            lblSize.Name = "lblSize";
            lblSize.Size = new Size(43, 15);
            lblSize.TabIndex = 7;
            lblSize.Text = "0.0 MB";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(400, 37);
            label1.Name = "label1";
            label1.Size = new Size(86, 17);
            label1.TabIndex = 1;
            label1.Text = "الوقت المتوقع";
            // 
            // lblTackTime
            // 
            lblTackTime.AutoSize = true;
            lblTackTime.Location = new Point(281, 39);
            lblTackTime.Name = "lblTackTime";
            lblTackTime.Size = new Size(49, 15);
            lblTackTime.TabIndex = 6;
            lblTackTime.Text = "00:00:00";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(373, 63);
            label4.Name = "label4";
            label4.Size = new Size(113, 17);
            label4.TabIndex = 3;
            label4.Text = "الحجم الحالي للملف";
            // 
            // lblSpend
            // 
            lblSpend.AutoSize = true;
            lblSpend.Location = new Point(281, 13);
            lblSpend.Name = "lblSpend";
            lblSpend.Size = new Size(49, 15);
            lblSpend.TabIndex = 5;
            lblSpend.Text = "00:00:00";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(442, 134);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 4;
            label3.Text = "السرعة";
            // 
            // frmShowBackup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(506, 307);
            Controls.Add(smoothPanel1);
            Controls.Add(bottomPanel);
            Controls.Add(smoothPanelTopConrner1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmShowBackup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmShowBackup";
            Load += frmShowBackup_Load;
            smoothPanelTopConrner1.ResumeLayout(false);
            smoothPanelTopConrner1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            bottomPanel.ResumeLayout(false);
            smoothPanel1.ResumeLayout(false);
            smoothPanel1.PerformLayout();
            backupPanel.ResumeLayout(false);
            backupPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SmoothPanelTopConrner smoothPanelTopConrner1;
        private SmoothPanel_BottomCorner bottomPanel;
        private SmoothPanel smoothPanel1;
        private Label label2;
        private Label label1;
        private Guna.UI2.WinForms.Guna2ProgressBar pbBackup;
        private Label lblSpeed;
        private Label lblSize;
        private Label lblTackTime;
        private Label lblSpend;
        private Label label3;
        private Label label4;
        private Label label5;
        private PictureBox pictureBox1;
        private Panel backupPanel;
        public Guna.UI2.WinForms.Guna2Button btnExit;
        public Guna.UI2.WinForms.Guna2Button btnDifferential;
        public Guna.UI2.WinForms.Guna2Button btnFull;
        private Label lblLastDate;
        private Label label7;
        private Label lblRemainingDays;
    }
}