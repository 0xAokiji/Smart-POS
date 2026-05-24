namespace pos.UserControls
{
    partial class ucShowPrice
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
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            lblName = new Label();
            lblNumber = new Label();
            label1 = new Label();
            lblPrice = new Label();
            lblQty = new Label();
            label3 = new Label();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.FromArgb(50, 55, 89);
            guna2Panel1.Controls.Add(lblName);
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.Dock = DockStyle.Bottom;
            guna2Panel1.Location = new Point(0, 78);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(231, 34);
            guna2Panel1.TabIndex = 32;
            guna2Panel1.Click += guna2Panel1_Click;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblName.ForeColor = Color.White;
            lblName.Location = new Point(74, 3);
            lblName.Name = "lblName";
            lblName.Size = new Size(82, 28);
            lblName.TabIndex = 6;
            lblName.Text = "label10";
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            lblName.Click += guna2Panel1_Click;
            // 
            // lblNumber
            // 
            lblNumber.AutoSize = true;
            lblNumber.Font = new Font("Segoe UI", 10.8F);
            lblNumber.Location = new Point(3, 0);
            lblNumber.Name = "lblNumber";
            lblNumber.Size = new Size(22, 25);
            lblNumber.TabIndex = 33;
            lblNumber.Text = "0";
            lblNumber.Click += guna2Panel1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.8F);
            label1.Location = new Point(163, 18);
            label1.Name = "label1";
            label1.Size = new Size(57, 25);
            label1.TabIndex = 34;
            label1.Text = "السعر";
            label1.Click += guna2Panel1_Click;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 10.8F);
            lblPrice.Location = new Point(53, 18);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(46, 25);
            lblPrice.TabIndex = 35;
            lblPrice.Text = "0.00";
            lblPrice.Click += guna2Panel1_Click;
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Font = new Font("Segoe UI", 10.8F);
            lblQty.Location = new Point(53, 48);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(22, 25);
            lblQty.TabIndex = 37;
            lblQty.Text = "0";
            lblQty.Click += guna2Panel1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.8F);
            label3.Location = new Point(163, 48);
            label3.Name = "label3";
            label3.Size = new Size(58, 25);
            label3.TabIndex = 36;
            label3.Text = "الكمية";
            label3.Click += guna2Panel1_Click;
            // 
            // ucShowPrice
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            Controls.Add(lblQty);
            Controls.Add(label3);
            Controls.Add(lblPrice);
            Controls.Add(label1);
            Controls.Add(lblNumber);
            Controls.Add(guna2Panel1);
            Name = "ucShowPrice";
            Size = new Size(231, 112);
            Load += ucShowPrice_Load;
            Click += guna2Panel1_Click;
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        public Label lblName;
        private Label lblNumber;
        private Label label1;
        private Label lblPrice;
        private Label lblQty;
        private Label label3;
    }
}
