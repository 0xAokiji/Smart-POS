namespace pos.Model.POS
{
    partial class frmShowProductImg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowProductImg));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panel1 = new Panel();
            btnSaveImag = new Guna.UI2.WinForms.Guna2Button();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(38, 38, 38);
            panel1.Controls.Add(btnSaveImag);
            panel1.Controls.Add(guna2Button1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 49);
            panel1.TabIndex = 1;
            // 
            // btnSaveImag
            // 
            btnSaveImag.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveImag.CustomizableEdges = customizableEdges1;
            btnSaveImag.DisabledState.BorderColor = Color.DarkGray;
            btnSaveImag.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSaveImag.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSaveImag.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSaveImag.FillColor = Color.FromArgb(38, 38, 38);
            btnSaveImag.Font = new Font("Segoe UI", 9F);
            btnSaveImag.ForeColor = Color.White;
            btnSaveImag.Image = (Image)resources.GetObject("btnSaveImag.Image");
            btnSaveImag.ImageSize = new Size(30, 30);
            btnSaveImag.Location = new Point(654, 6);
            btnSaveImag.Name = "btnSaveImag";
            btnSaveImag.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnSaveImag.Size = new Size(68, 37);
            btnSaveImag.TabIndex = 1;
            btnSaveImag.Click += btnSaveImag_Click;
            // 
            // guna2Button1
            // 
            guna2Button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guna2Button1.CustomizableEdges = customizableEdges3;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(255, 128, 128);
            guna2Button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.Black;
            guna2Button1.Location = new Point(737, 10);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button1.Size = new Size(51, 29);
            guna2Button1.TabIndex = 0;
            guna2Button1.Text = "X";
            guna2Button1.Click += guna2Button1_Click;
            // 
            // frmShowProductImg
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(38, 38, 38);
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmShowProductImg";
            Text = "frmShowProductImg";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button btnSaveImag;
    }
}