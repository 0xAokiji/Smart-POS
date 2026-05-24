namespace pos.UserControls
{
    partial class ucProduct2
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucProduct2));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            btnEdite = new Guna.UI2.WinForms.Guna2PictureBox();
            btnInfo = new Guna.UI2.WinForms.Guna2PictureBox();
            lblName = new Label();
            lblQtyUse = new Label();
            lblWholName = new Label();
            lblQtyname = new Label();
            lblQty = new Label();
            lblPriceName = new Label();
            lblPrice = new Label();
            proImage = new Guna.UI2.WinForms.Guna2PictureBox();
            line = new Guna.UI2.WinForms.Guna2Separator();
            bottomPanel = new Panel();
            btnUse = new Guna.UI2.WinForms.Guna2Button();
            lblPlace = new Label();
            lblPlaceName = new Label();
            ((System.ComponentModel.ISupportInitialize)btnEdite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnInfo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)proImage).BeginInit();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // btnEdite
            // 
            btnEdite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdite.CustomizableEdges = customizableEdges1;
            btnEdite.Image = Properties.Resources.edite_light;
            btnEdite.ImageRotate = 0F;
            btnEdite.Location = new Point(193, 7);
            btnEdite.Name = "btnEdite";
            btnEdite.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnEdite.Size = new Size(30, 28);
            btnEdite.SizeMode = PictureBoxSizeMode.Zoom;
            btnEdite.TabIndex = 3;
            btnEdite.TabStop = false;
            btnEdite.Click += guna2PictureBox2_Click;
            // 
            // btnInfo
            // 
            btnInfo.CustomizableEdges = customizableEdges3;
            btnInfo.Image = Properties.Resources.info_light;
            btnInfo.ImageRotate = 0F;
            btnInfo.Location = new Point(5, 7);
            btnInfo.Name = "btnInfo";
            btnInfo.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnInfo.Size = new Size(30, 28);
            btnInfo.SizeMode = PictureBoxSizeMode.Zoom;
            btnInfo.TabIndex = 2;
            btnInfo.TabStop = false;
            btnInfo.Click += guna2PictureBox1_Click;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblName.ForeColor = Color.FromArgb(51, 51, 51);
            lblName.Location = new Point(79, 8);
            lblName.Name = "lblName";
            lblName.RightToLeft = RightToLeft.Yes;
            lblName.Size = new Size(72, 19);
            lblName.TabIndex = 4;
            lblName.Text = "اسم المنتج";
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            lblName.DoubleClick += bottomPanel_DoubleClick;
            // 
            // lblQtyUse
            // 
            lblQtyUse.AutoSize = true;
            lblQtyUse.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQtyUse.ForeColor = Color.FromArgb(51, 51, 51);
            lblQtyUse.Location = new Point(5, 88);
            lblQtyUse.Name = "lblQtyUse";
            lblQtyUse.Size = new Size(40, 15);
            lblQtyUse.TabIndex = 11;
            lblQtyUse.Text = "label6";
            lblQtyUse.Click += txtImage_Click;
            // 
            // lblWholName
            // 
            lblWholName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblWholName.AutoSize = true;
            lblWholName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblWholName.ForeColor = Color.FromArgb(51, 51, 51);
            lblWholName.Location = new Point(144, 88);
            lblWholName.Name = "lblWholName";
            lblWholName.Size = new Size(83, 15);
            lblWholName.TabIndex = 8;
            lblWholName.Text = "كمية المستعمل";
            lblWholName.TextAlign = ContentAlignment.MiddleLeft;
            lblWholName.Click += txtImage_Click;
            // 
            // lblQtyname
            // 
            lblQtyname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblQtyname.AutoSize = true;
            lblQtyname.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQtyname.ForeColor = Color.FromArgb(51, 51, 51);
            lblQtyname.Location = new Point(164, 58);
            lblQtyname.Name = "lblQtyname";
            lblQtyname.Size = new Size(63, 15);
            lblQtyname.TabIndex = 6;
            lblQtyname.Text = "كمية الجديد";
            lblQtyname.TextAlign = ContentAlignment.MiddleLeft;
            lblQtyname.Click += txtImage_Click;
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQty.ForeColor = Color.FromArgb(51, 51, 51);
            lblQty.Location = new Point(5, 58);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(40, 15);
            lblQty.TabIndex = 9;
            lblQty.Text = "label4";
            lblQty.Click += txtImage_Click;
            // 
            // lblPriceName
            // 
            lblPriceName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPriceName.AutoSize = true;
            lblPriceName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPriceName.ForeColor = Color.FromArgb(51, 51, 51);
            lblPriceName.Location = new Point(191, 142);
            lblPriceName.Name = "lblPriceName";
            lblPriceName.Size = new Size(36, 15);
            lblPriceName.TabIndex = 7;
            lblPriceName.Text = "السعر";
            lblPriceName.TextAlign = ContentAlignment.MiddleLeft;
            lblPriceName.Click += txtImage_Click;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPrice.ForeColor = Color.FromArgb(51, 51, 51);
            lblPrice.Location = new Point(5, 142);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(40, 15);
            lblPrice.TabIndex = 10;
            lblPrice.Text = "label5";
            lblPrice.Click += txtImage_Click;
            // 
            // proImage
            // 
            proImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            proImage.BackColor = Color.FromArgb(230, 230, 230);
            proImage.Cursor = Cursors.Hand;
            proImage.CustomizableEdges = customizableEdges5;
            proImage.Image = (Image)resources.GetObject("proImage.Image");
            proImage.ImageRotate = 0F;
            proImage.Location = new Point(5, 48);
            proImage.Name = "proImage";
            proImage.ShadowDecoration.CustomizableEdges = customizableEdges6;
            proImage.Size = new Size(220, 112);
            proImage.SizeMode = PictureBoxSizeMode.Zoom;
            proImage.TabIndex = 12;
            proImage.TabStop = false;
            proImage.Click += proImage_Click;
            proImage.DoubleClick += txtImage_Click;
            // 
            // line
            // 
            line.FillColor = Color.FromArgb(51, 51, 51);
            line.Location = new Point(5, 35);
            line.Name = "line";
            line.Size = new Size(218, 10);
            line.TabIndex = 13;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(136, 214, 218);
            bottomPanel.Controls.Add(lblName);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 166);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(230, 34);
            bottomPanel.TabIndex = 15;
            bottomPanel.DoubleClick += bottomPanel_DoubleClick;
            // 
            // btnUse
            // 
            btnUse.BorderRadius = 8;
            btnUse.CustomizableEdges = customizableEdges7;
            btnUse.DisabledState.BorderColor = Color.DarkGray;
            btnUse.DisabledState.CustomBorderColor = Color.DarkGray;
            btnUse.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnUse.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnUse.FillColor = Color.FromArgb(136, 214, 218);
            btnUse.Font = new Font("Segoe UI", 9F);
            btnUse.ForeColor = Color.FromArgb(51, 51, 51);
            btnUse.Location = new Point(63, 7);
            btnUse.Name = "btnUse";
            btnUse.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnUse.Size = new Size(104, 25);
            btnUse.TabIndex = 16;
            btnUse.Text = "مستعمل";
            btnUse.Click += btnUse_Click;
            // 
            // lblPlace
            // 
            lblPlace.AutoSize = true;
            lblPlace.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPlace.ForeColor = Color.FromArgb(51, 51, 51);
            lblPlace.Location = new Point(5, 113);
            lblPlace.Name = "lblPlace";
            lblPlace.Size = new Size(36, 15);
            lblPlace.TabIndex = 18;
            lblPlace.Text = "place";
            lblPlace.Visible = false;
            // 
            // lblPlaceName
            // 
            lblPlaceName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPlaceName.AutoSize = true;
            lblPlaceName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPlaceName.ForeColor = Color.FromArgb(51, 51, 51);
            lblPlaceName.Location = new Point(189, 113);
            lblPlaceName.Name = "lblPlaceName";
            lblPlaceName.Size = new Size(38, 15);
            lblPlaceName.TabIndex = 17;
            lblPlaceName.Text = "المكان";
            lblPlaceName.TextAlign = ContentAlignment.MiddleLeft;
            lblPlaceName.Visible = false;
            // 
            // ucProduct2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(230, 230, 230);
            Controls.Add(lblPlace);
            Controls.Add(lblPlaceName);
            Controls.Add(btnUse);
            Controls.Add(bottomPanel);
            Controls.Add(line);
            Controls.Add(proImage);
            Controls.Add(lblQtyUse);
            Controls.Add(lblWholName);
            Controls.Add(lblQtyname);
            Controls.Add(lblQty);
            Controls.Add(lblPriceName);
            Controls.Add(lblPrice);
            Controls.Add(btnEdite);
            Controls.Add(btnInfo);
            DoubleBuffered = true;
            Name = "ucProduct2";
            Size = new Size(230, 200);
            Click += txtImage_Click;
            ((System.ComponentModel.ISupportInitialize)btnEdite).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnInfo).EndInit();
            ((System.ComponentModel.ISupportInitialize)proImage).EndInit();
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox btnEdite;
        private Guna.UI2.WinForms.Guna2PictureBox btnInfo;
        private Label lblName;
        private Label lblQtyUse;
        private Label lblWholName;
        private Label lblQtyname;
        private Label lblQty;
        private Label lblPriceName;
        private Label lblPrice;
        private Guna.UI2.WinForms.Guna2PictureBox proImage;
        private Guna.UI2.WinForms.Guna2Separator line;
        private Panel bottomPanel;
        private Label lblPlace;
        private Label lblPlaceName;
        public Guna.UI2.WinForms.Guna2Button btnUse;
    }
}
