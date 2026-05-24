namespace pos.View
{
    partial class frmGraphicalinterFace
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGraphicalinterFace));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            timer1 = new System.Windows.Forms.Timer(components);
            label3 = new Label();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            label4 = new Label();
            guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            progressBar = new Guna.UI2.WinForms.Guna2ProgressBar();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 2000;
            timer1.Tick += timer1_Tick;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(13, 444);
            label3.Name = "label3";
            label3.Size = new Size(127, 24);
            label3.TabIndex = 3;
            label3.Text = "Smart Cashier";
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            guna2PictureBox1.CustomizableEdges = customizableEdges1;
            guna2PictureBox1.Image = (Image)resources.GetObject("guna2PictureBox1.Image");
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(238, 11);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Size = new Size(498, 427);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            guna2PictureBox1.TabIndex = 4;
            guna2PictureBox1.TabStop = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.Window;
            label4.Location = new Point(414, 222);
            label4.Name = "label4";
            label4.Size = new Size(118, 24);
            label4.TabIndex = 5;
            label4.Text = "LOADING...";
            // 
            // guna2MessageDialog1
            // 
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Caption = "تحذير";
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
            guna2MessageDialog1.Parent = this;
            guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Dark;
            guna2MessageDialog1.Text = null;
            // 
            // progressBar
            // 
            progressBar.CustomizableEdges = customizableEdges3;
            progressBar.Location = new Point(0, 488);
            progressBar.Name = "progressBar";
            progressBar.ProgressColor2 = Color.FromArgb(1, 95, 95);
            progressBar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            progressBar.Size = new Size(975, 19);
            progressBar.TabIndex = 6;
            progressBar.Text = "guna2ProgressBar1";
            progressBar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // frmGraphicalinterFace
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(1, 0, 0);
            ClientSize = new Size(975, 506);
            Controls.Add(progressBar);
            Controls.Add(label4);
            Controls.Add(guna2PictureBox1);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmGraphicalinterFace";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmGraphicalinterFace";
            Load += frmGraphicalinterFace_Load;
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public System.Windows.Forms.Timer timer1;
        private Label label3;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Label label4;
        private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
        private Guna.UI2.WinForms.Guna2ProgressBar progressBar;
    }
}