namespace pos
{
    partial class SampleView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleView));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtSearch1 = new Guna.UI2.WinForms.Guna2TextBox();
            label1 = new Label();
            btnAdd = new Guna.UI2.WinForms.Guna2ImageButton();
            label2 = new Label();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            SuspendLayout();
            // 
            // txtSearch1
            // 
            txtSearch1.AutoRoundedCorners = true;
            txtSearch1.BorderRadius = 19;
            txtSearch1.CustomizableEdges = customizableEdges1;
            txtSearch1.DefaultText = "";
            txtSearch1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch1.Font = new Font("Segoe UI", 9F);
            txtSearch1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch1.IconRight = (Image)resources.GetObject("txtSearch1.IconRight");
            txtSearch1.Location = new Point(25, 38);
            txtSearch1.Margin = new Padding(3, 4, 3, 4);
            txtSearch1.Name = "txtSearch1";
            txtSearch1.PasswordChar = '\0';
            txtSearch1.PlaceholderText = "ابحث هنا";
            txtSearch1.SelectedText = "";
            txtSearch1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSearch1.Size = new Size(286, 40);
            txtSearch1.TabIndex = 0;
            txtSearch1.TextAlign = HorizontalAlignment.Right;
            txtSearch1.TextChanged += txtSearch_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(269, 9);
            label1.Name = "label1";
            label1.Size = new Size(42, 23);
            label1.TabIndex = 1;
            label1.Text = "بحث";
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.CheckedState.ImageSize = new Size(64, 64);
            btnAdd.HoverState.ImageSize = new Size(64, 64);
            btnAdd.Image = (Image)resources.GetObject("btnAdd.Image");
            btnAdd.ImageOffset = new Point(0, 0);
            btnAdd.ImageRotate = 0F;
            btnAdd.Location = new Point(800, 14);
            btnAdd.Name = "btnAdd";
            btnAdd.PressedState.ImageSize = new Size(64, 64);
            btnAdd.ShadowDecoration.CustomizableEdges = customizableEdges3;
            btnAdd.Size = new Size(64, 64);
            btnAdd.TabIndex = 2;
            btnAdd.Click += btnAdd_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(710, 38);
            label2.Name = "label2";
            label2.Size = new Size(84, 23);
            label2.TabIndex = 3;
            label2.Text = "رأس النص";
            // 
            // guna2Separator1
            // 
            guna2Separator1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            guna2Separator1.Location = new Point(25, 85);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(839, 12);
            guna2Separator1.TabIndex = 4;
            // 
            // guna2MessageDialog1
            // 
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Caption = "تنبية";
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Parent = this;
            guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            guna2MessageDialog1.Text = null;
            // 
            // SampleView
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(900, 518);
            Controls.Add(guna2Separator1);
            Controls.Add(label2);
            Controls.Add(btnAdd);
            Controls.Add(label1);
            Controls.Add(txtSearch1);
            Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SampleView";
            Text = "SampleView";
            Load += SampleView_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Guna.UI2.WinForms.Guna2TextBox txtSearch1;
        public Label label1;
        public Guna.UI2.WinForms.Guna2ImageButton btnAdd;
        public Label label2;
        public Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        public Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
    }
}