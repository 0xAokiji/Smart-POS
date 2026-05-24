using pos.Classes;

namespace pos.Model
{
    partial class ucStore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucStore));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblName = new Label();
            btnDel = new Guna.UI2.WinForms.Guna2PictureBox();
            btnEdite = new Guna.UI2.WinForms.Guna2PictureBox();
            checkBox = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            panel1 = new Panel();
            lblPrice = new Label();
            lblWhol = new Label();
            lblQty = new Label();
            lblCat = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            imgProduct = new Guna.UI2.WinForms.Guna2PictureBox();
            bottomPanel = new pos.Classes.SmoothPanel_BottomCorner();
            topPanel = new pos.Classes.SmoothPanelTopConrner();
            ((System.ComponentModel.ISupportInitialize)btnDel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnEdite).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgProduct).BeginInit();
            bottomPanel.SuspendLayout();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblName.ForeColor = Color.FromArgb(51, 51, 51);
            lblName.Location = new Point(97, 3);
            lblName.Name = "lblName";
            lblName.Size = new Size(66, 21);
            lblName.TabIndex = 6;
            lblName.Text = "label10";
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnDel
            // 
            btnDel.CustomizableEdges = customizableEdges1;
            btnDel.Image = (Image)resources.GetObject("btnDel.Image");
            btnDel.ImageRotate = 0F;
            btnDel.Location = new Point(6, 3);
            btnDel.Margin = new Padding(3, 2, 3, 2);
            btnDel.Name = "btnDel";
            btnDel.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnDel.Size = new Size(26, 22);
            btnDel.SizeMode = PictureBoxSizeMode.Zoom;
            btnDel.TabIndex = 30;
            btnDel.TabStop = false;
            btnDel.Click += guna2PictureBox2_Click;
            // 
            // btnEdite
            // 
            btnEdite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdite.CustomizableEdges = customizableEdges3;
            btnEdite.Image = (Image)resources.GetObject("btnEdite.Image");
            btnEdite.ImageRotate = 0F;
            btnEdite.Location = new Point(226, 3);
            btnEdite.Margin = new Padding(3, 2, 3, 2);
            btnEdite.Name = "btnEdite";
            btnEdite.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnEdite.Size = new Size(26, 22);
            btnEdite.SizeMode = PictureBoxSizeMode.Zoom;
            btnEdite.TabIndex = 29;
            btnEdite.TabStop = false;
            btnEdite.Click += guna2PictureBox1_Click;
            // 
            // checkBox
            // 
            checkBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            checkBox.CheckedState.BorderRadius = 2;
            checkBox.CheckedState.BorderThickness = 0;
            checkBox.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            checkBox.CustomizableEdges = customizableEdges5;
            checkBox.Enabled = false;
            checkBox.Location = new Point(227, 3);
            checkBox.Margin = new Padding(3, 2, 3, 2);
            checkBox.Name = "checkBox";
            checkBox.ShadowDecoration.CustomizableEdges = customizableEdges6;
            checkBox.Size = new Size(22, 21);
            checkBox.TabIndex = 31;
            checkBox.Text = "guna2CustomCheckBox1";
            checkBox.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            checkBox.UncheckedState.BorderRadius = 2;
            checkBox.UncheckedState.BorderThickness = 0;
            checkBox.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            checkBox.Visible = false;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(lblPrice);
            panel1.Controls.Add(lblWhol);
            panel1.Controls.Add(lblQty);
            panel1.Controls.Add(lblCat);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(imgProduct);
            panel1.Location = new Point(4, 27);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(252, 120);
            panel1.TabIndex = 33;
            panel1.Click += panel1_Click;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 10.8F);
            lblPrice.ForeColor = Color.FromArgb(51, 51, 51);
            lblPrice.Location = new Point(122, 92);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(50, 20);
            lblPrice.TabIndex = 19;
            lblPrice.Text = "label6";
            // 
            // lblWhol
            // 
            lblWhol.AutoSize = true;
            lblWhol.Font = new Font("Segoe UI", 10.8F);
            lblWhol.ForeColor = Color.FromArgb(51, 51, 51);
            lblWhol.Location = new Point(122, 65);
            lblWhol.Name = "lblWhol";
            lblWhol.Size = new Size(50, 20);
            lblWhol.TabIndex = 18;
            lblWhol.Text = "label7";
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Font = new Font("Segoe UI", 10.8F);
            lblQty.ForeColor = Color.FromArgb(51, 51, 51);
            lblQty.Location = new Point(122, 38);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(50, 20);
            lblQty.TabIndex = 17;
            lblQty.Text = "label8";
            // 
            // lblCat
            // 
            lblCat.AutoSize = true;
            lblCat.Font = new Font("Segoe UI", 10.8F);
            lblCat.ForeColor = Color.FromArgb(51, 51, 51);
            lblCat.Location = new Point(122, 12);
            lblCat.Name = "lblCat";
            lblCat.Size = new Size(50, 20);
            lblCat.TabIndex = 16;
            lblCat.Text = "label9";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.8F);
            label5.ForeColor = Color.FromArgb(51, 51, 51);
            label5.Location = new Point(183, 92);
            label5.Name = "label5";
            label5.Size = new Size(69, 20);
            label5.TabIndex = 15;
            label5.Text = "سعر البيع";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.8F);
            label4.ForeColor = Color.FromArgb(51, 51, 51);
            label4.Location = new Point(171, 65);
            label4.Name = "label4";
            label4.Size = new Size(81, 20);
            label4.TabIndex = 14;
            label4.Text = "سعر الجملة";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.8F);
            label3.ForeColor = Color.FromArgb(51, 51, 51);
            label3.Location = new Point(204, 38);
            label3.Name = "label3";
            label3.Size = new Size(48, 20);
            label3.TabIndex = 13;
            label3.Text = "الكمية";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.8F);
            label2.ForeColor = Color.FromArgb(51, 51, 51);
            label2.Location = new Point(199, 12);
            label2.Name = "label2";
            label2.Size = new Size(53, 20);
            label2.TabIndex = 12;
            label2.Text = "الصنف";
            // 
            // imgProduct
            // 
            imgProduct.CustomizableEdges = customizableEdges7;
            imgProduct.ImageRotate = 0F;
            imgProduct.Location = new Point(6, 4);
            imgProduct.Margin = new Padding(3, 2, 3, 2);
            imgProduct.Name = "imgProduct";
            imgProduct.ShadowDecoration.CustomizableEdges = customizableEdges8;
            imgProduct.Size = new Size(110, 110);
            imgProduct.SizeMode = PictureBoxSizeMode.Zoom;
            imgProduct.TabIndex = 11;
            imgProduct.TabStop = false;
            imgProduct.Click += imgProduct_Click;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(230, 230, 230);
            bottomPanel.BorderColor = Color.FromArgb(136, 214, 218);
            bottomPanel.BorderSize = 1F;
            bottomPanel.Controls.Add(lblName);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 147);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(261, 27);
            bottomPanel.TabIndex = 20;
            bottomPanel.Click += panel1_Click;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(136, 214, 218);
            topPanel.BorderColor = Color.FromArgb(136, 214, 218);
            topPanel.BorderSize = 1F;
            topPanel.Controls.Add(btnDel);
            topPanel.Controls.Add(btnEdite);
            topPanel.Controls.Add(checkBox);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(261, 28);
            topPanel.TabIndex = 34;
            // 
            // ucStore
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            Controls.Add(bottomPanel);
            Controls.Add(topPanel);
            Controls.Add(panel1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "ucStore";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(261, 174);
            Load += ucStore_Load;
            ((System.ComponentModel.ISupportInitialize)btnDel).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnEdite).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgProduct).EndInit();
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            topPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        public Label lblName;
        public Guna.UI2.WinForms.Guna2PictureBox btnDel;
        public Guna.UI2.WinForms.Guna2PictureBox btnEdite;
        private Panel panel1;
        private Label lblPrice;
        private Label lblWhol;
        public Label lblQty;
        private Label lblCat;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Guna.UI2.WinForms.Guna2PictureBox imgProduct;
        public Guna.UI2.WinForms.Guna2CustomCheckBox checkBox;
        private SmoothPanel_BottomCorner bottomPanel;
        private SmoothPanelTopConrner topPanel;
    }
}
