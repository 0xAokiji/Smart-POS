using pos.Classes;

namespace pos.Model.POS
{
    partial class frmBillName
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBillName));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            smoothPanelTopConrner1 = new pos.Classes.SmoothPanelTopConrner();
            pictureBox1 = new PictureBox();
            lblTitle = new Label();
            smoothPanel_BottomCorner1 = new pos.Classes.SmoothPanel_BottomCorner();
            btnExit = new Guna.UI2.WinForms.Guna2Button();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            smoothPanel1 = new pos.Classes.SmoothPanel();
            btnEditParties = new Guna.UI2.WinForms.Guna2Button();
            btnAddParties = new Guna.UI2.WinForms.Guna2Button();
            txtName = new Guna.UI2.WinForms.Guna2TextBox();
            smoothPanelTopConrner1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            smoothPanel_BottomCorner1.SuspendLayout();
            smoothPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // smoothPanelTopConrner1
            // 
            smoothPanelTopConrner1.BackColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderSize = 1F;
            smoothPanelTopConrner1.Controls.Add(pictureBox1);
            smoothPanelTopConrner1.Controls.Add(lblTitle);
            smoothPanelTopConrner1.Dock = DockStyle.Top;
            smoothPanelTopConrner1.Location = new Point(0, 0);
            smoothPanelTopConrner1.Name = "smoothPanelTopConrner1";
            smoothPanelTopConrner1.Size = new Size(339, 45);
            smoothPanelTopConrner1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(286, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(204, 204, 204);
            lblTitle.ImeMode = ImeMode.NoControl;
            lblTitle.Location = new Point(105, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(175, 20);
            lblTitle.TabIndex = 7;
            lblTitle.Text = "أضافة اسم صاحب الفاتورة";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // smoothPanel_BottomCorner1
            // 
            smoothPanel_BottomCorner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel_BottomCorner1.BorderSize = 1F;
            smoothPanel_BottomCorner1.Controls.Add(btnExit);
            smoothPanel_BottomCorner1.Controls.Add(btnSave);
            smoothPanel_BottomCorner1.Dock = DockStyle.Bottom;
            smoothPanel_BottomCorner1.Location = new Point(0, 148);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(339, 43);
            smoothPanel_BottomCorner1.TabIndex = 1;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.None;
            btnExit.BorderRadius = 8;
            btnExit.CustomizableEdges = customizableEdges1;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.Red;
            btnExit.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(54, 6);
            btnExit.Name = "btnExit";
            btnExit.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnExit.Size = new Size(94, 30);
            btnExit.TabIndex = 4;
            btnExit.Text = "الغاء";
            btnExit.Click += btnExit_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Right;
            btnSave.BorderRadius = 8;
            btnSave.CustomizableEdges = customizableEdges3;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.Enabled = false;
            btnSave.FillColor = Color.FromArgb(1, 95, 95);
            btnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(190, 6);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSave.Size = new Size(94, 30);
            btnSave.TabIndex = 3;
            btnSave.Text = "حفظ";
            btnSave.Click += btnSave_Click;
            // 
            // smoothPanel1
            // 
            smoothPanel1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(btnEditParties);
            smoothPanel1.Controls.Add(btnAddParties);
            smoothPanel1.Controls.Add(txtName);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 45);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(339, 103);
            smoothPanel1.TabIndex = 2;
            // 
            // btnEditParties
            // 
            btnEditParties.Anchor = AnchorStyles.Right;
            btnEditParties.BorderRadius = 8;
            btnEditParties.CustomizableEdges = customizableEdges5;
            btnEditParties.DisabledState.BorderColor = Color.DarkGray;
            btnEditParties.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEditParties.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEditParties.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEditParties.Enabled = false;
            btnEditParties.FillColor = Color.Green;
            btnEditParties.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnEditParties.ForeColor = Color.White;
            btnEditParties.Image = Properties.Resources.edit_white;
            btnEditParties.ImageAlign = HorizontalAlignment.Left;
            btnEditParties.ImageSize = new Size(15, 15);
            btnEditParties.Location = new Point(17, 67);
            btnEditParties.Name = "btnEditParties";
            btnEditParties.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEditParties.Size = new Size(174, 30);
            btnEditParties.TabIndex = 33;
            btnEditParties.Text = "تعديل بيانات العميل";
            btnEditParties.Click += btnEditParties_Click;
            // 
            // btnAddParties
            // 
            btnAddParties.Anchor = AnchorStyles.Right;
            btnAddParties.BorderRadius = 8;
            btnAddParties.CustomizableEdges = customizableEdges7;
            btnAddParties.DisabledState.BorderColor = Color.DarkGray;
            btnAddParties.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAddParties.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAddParties.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAddParties.FillColor = Color.FromArgb(136, 214, 218);
            btnAddParties.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnAddParties.ForeColor = Color.White;
            btnAddParties.Image = Properties.Resources.add_user_light;
            btnAddParties.ImageAlign = HorizontalAlignment.Left;
            btnAddParties.ImageSize = new Size(15, 15);
            btnAddParties.Location = new Point(197, 67);
            btnAddParties.Name = "btnAddParties";
            btnAddParties.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnAddParties.Size = new Size(125, 30);
            btnAddParties.TabIndex = 32;
            btnAddParties.Text = "اضافة عميل";
            btnAddParties.Click += btnAddParties_Click;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtName.BorderColor = Color.FromArgb(136, 214, 218);
            txtName.BorderRadius = 8;
            txtName.CustomizableEdges = customizableEdges9;
            txtName.DefaultText = "";
            txtName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtName.ForeColor = Color.FromArgb(64, 64, 64);
            txtName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Location = new Point(17, 18);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "اسم العميل";
            txtName.RightToLeft = RightToLeft.No;
            txtName.SelectedText = "";
            txtName.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtName.Size = new Size(305, 30);
            txtName.TabIndex = 3;
            txtName.TextAlign = HorizontalAlignment.Right;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // frmBillName
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 191);
            Controls.Add(smoothPanel1);
            Controls.Add(smoothPanel_BottomCorner1);
            Controls.Add(smoothPanelTopConrner1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmBillName";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmBillName";
            Load += frmBillName_Load;
            smoothPanelTopConrner1.ResumeLayout(false);
            smoothPanelTopConrner1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            smoothPanel_BottomCorner1.ResumeLayout(false);
            smoothPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SmoothPanelTopConrner smoothPanelTopConrner1;
        private SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
        private SmoothPanel smoothPanel1;
        private Guna.UI2.WinForms.Guna2TextBox txtName;
        public Guna.UI2.WinForms.Guna2Button btnEditParties;
        public Guna.UI2.WinForms.Guna2Button btnAddParties;
        public Guna.UI2.WinForms.Guna2Button btnExit;
        public Guna.UI2.WinForms.Guna2Button btnSave;
        private PictureBox pictureBox1;
        private Label lblTitle;
    }
}