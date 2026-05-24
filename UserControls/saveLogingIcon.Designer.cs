namespace pos.UserControls
{
    partial class saveLogingIcon
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            perImage = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            lblName = new Label();
            bottomPanel = new Panel();
            btnDelete = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)perImage).BeginInit();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // perImage
            // 
            perImage.ImageRotate = 0F;
            perImage.Location = new Point(19, 11);
            perImage.Name = "perImage";
            perImage.ShadowDecoration.CustomizableEdges = customizableEdges1;
            perImage.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            perImage.Size = new Size(65, 65);
            perImage.TabIndex = 0;
            perImage.TabStop = false;
            perImage.DoubleClick += perImage_Click;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(32, 10);
            lblName.Name = "lblName";
            lblName.Size = new Size(38, 15);
            lblName.TabIndex = 1;
            lblName.Text = "label1";
            lblName.DoubleClick += perImage_Click;
            // 
            // bottomPanel
            // 
            bottomPanel.Controls.Add(lblName);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 78);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(103, 34);
            bottomPanel.TabIndex = 2;
            bottomPanel.DoubleClick += perImage_Click;
            // 
            // btnDelete
            // 
            btnDelete.CustomizableEdges = customizableEdges2;
            btnDelete.DisabledState.BorderColor = Color.DarkGray;
            btnDelete.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDelete.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDelete.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDelete.FillColor = Color.FromArgb(243, 243, 243);
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.Red;
            btnDelete.Location = new Point(0, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.ShadowDecoration.CustomizableEdges = customizableEdges3;
            btnDelete.Size = new Size(29, 21);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "X";
            btnDelete.Click += btnDelete_Click;
            // 
            // saveLogingIcon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            Controls.Add(btnDelete);
            Controls.Add(bottomPanel);
            Controls.Add(perImage);
            Name = "saveLogingIcon";
            Size = new Size(103, 112);
            Load += saveLogingIcon_Load;
            DoubleClick += perImage_Click;
            ((System.ComponentModel.ISupportInitialize)perImage).EndInit();
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2CirclePictureBox perImage;
        private Label lblName;
        private Panel bottomPanel;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
    }
}
