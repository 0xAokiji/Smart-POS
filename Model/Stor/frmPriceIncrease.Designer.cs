using pos.Classes;

namespace pos.Model
{
    partial class frmPriceIncrease
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
            txtPrice = new Guna.UI2.WinForms.Guna2TextBox();
            rbPercent = new Guna.UI2.WinForms.Guna2RadioButton();
            rbValue = new Guna.UI2.WinForms.Guna2RadioButton();
            topPanel = new pos.Classes.SmoothPanelTopConrner();
            lblTitle = new Label();
            iconImage = new Guna.UI2.WinForms.Guna2PictureBox();
            bottomPanel = new pos.Classes.SmoothPanel_BottomCorner();
            btn_Save = new Guna.UI2.WinForms.Guna2Button();
            btn_Close = new Guna.UI2.WinForms.Guna2Button();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconImage).BeginInit();
            bottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // txtPrice
            // 
            txtPrice.BorderColor = Color.FromArgb(136, 214, 218);
            txtPrice.BorderRadius = 8;
            txtPrice.CustomizableEdges = customizableEdges1;
            txtPrice.DefaultText = "";
            txtPrice.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPrice.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPrice.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPrice.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPrice.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPrice.Font = new Font("Segoe UI", 9F);
            txtPrice.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPrice.Location = new Point(59, 82);
            txtPrice.Margin = new Padding(3, 4, 3, 4);
            txtPrice.Name = "txtPrice";
            txtPrice.PlaceholderText = "";
            txtPrice.SelectedText = "";
            txtPrice.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPrice.Size = new Size(134, 30);
            txtPrice.TabIndex = 6;
            txtPrice.TextAlign = HorizontalAlignment.Center;
            txtPrice.KeyPress += guna2TextBox1_KeyPress;
            // 
            // rbPercent
            // 
            rbPercent.AutoSize = true;
            rbPercent.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            rbPercent.CheckedState.BorderThickness = 0;
            rbPercent.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            rbPercent.CheckedState.InnerColor = Color.White;
            rbPercent.CheckedState.InnerOffset = -4;
            rbPercent.Location = new Point(59, 56);
            rbPercent.Name = "rbPercent";
            rbPercent.RightToLeft = RightToLeft.Yes;
            rbPercent.Size = new Size(80, 19);
            rbPercent.TabIndex = 9;
            rbPercent.Text = "نسبة مئوية";
            rbPercent.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            rbPercent.UncheckedState.BorderThickness = 2;
            rbPercent.UncheckedState.FillColor = Color.Transparent;
            rbPercent.UncheckedState.InnerColor = Color.Transparent;
            // 
            // rbValue
            // 
            rbValue.AutoSize = true;
            rbValue.Checked = true;
            rbValue.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            rbValue.CheckedState.BorderThickness = 0;
            rbValue.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            rbValue.CheckedState.InnerColor = Color.White;
            rbValue.CheckedState.InnerOffset = -4;
            rbValue.Location = new Point(145, 56);
            rbValue.Name = "rbValue";
            rbValue.RightToLeft = RightToLeft.Yes;
            rbValue.Size = new Size(48, 19);
            rbValue.TabIndex = 10;
            rbValue.TabStop = true;
            rbValue.Text = "قيمة";
            rbValue.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            rbValue.UncheckedState.BorderThickness = 2;
            rbValue.UncheckedState.FillColor = Color.Transparent;
            rbValue.UncheckedState.InnerColor = Color.Transparent;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(136, 214, 218);
            topPanel.BorderColor = Color.FromArgb(136, 214, 218);
            topPanel.BorderSize = 1F;
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(iconImage);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(253, 50);
            topPanel.TabIndex = 9;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(51, 51, 51);
            lblTitle.ImeMode = ImeMode.NoControl;
            lblTitle.Location = new Point(120, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(90, 20);
            lblTitle.TabIndex = 6;
            lblTitle.Text = "زيادة الاسعار";
            // 
            // iconImage
            // 
            iconImage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            iconImage.CustomizableEdges = customizableEdges3;
            iconImage.Image = Properties.Resources.store_Light;
            iconImage.ImageRotate = 0F;
            iconImage.Location = new Point(200, 8);
            iconImage.Name = "iconImage";
            iconImage.ShadowDecoration.CustomizableEdges = customizableEdges4;
            iconImage.Size = new Size(50, 35);
            iconImage.SizeMode = PictureBoxSizeMode.Zoom;
            iconImage.TabIndex = 1;
            iconImage.TabStop = false;
            // 
            // bottomPanel
            // 
            bottomPanel.BackColor = Color.FromArgb(230, 230, 230);
            bottomPanel.BorderColor = Color.FromArgb(136, 214, 218);
            bottomPanel.BorderSize = 1F;
            bottomPanel.Controls.Add(btn_Save);
            bottomPanel.Controls.Add(btn_Close);
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Location = new Point(0, 123);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new Size(253, 46);
            bottomPanel.TabIndex = 11;
            // 
            // btn_Save
            // 
            btn_Save.Anchor = AnchorStyles.None;
            btn_Save.AutoRoundedCorners = true;
            btn_Save.BorderRadius = 12;
            btn_Save.CustomizableEdges = customizableEdges5;
            btn_Save.DisabledState.BorderColor = Color.DarkGray;
            btn_Save.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Save.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Save.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Save.FillColor = Color.FromArgb(136, 214, 218);
            btn_Save.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btn_Save.ForeColor = Color.White;
            btn_Save.Location = new Point(138, 10);
            btn_Save.Name = "btn_Save";
            btn_Save.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btn_Save.Size = new Size(94, 26);
            btn_Save.TabIndex = 0;
            btn_Save.Text = "حفظ";
            btn_Save.Click += btnSave_Click;
            // 
            // btn_Close
            // 
            btn_Close.Anchor = AnchorStyles.None;
            btn_Close.AutoRoundedCorners = true;
            btn_Close.BorderRadius = 12;
            btn_Close.CustomizableEdges = customizableEdges7;
            btn_Close.DisabledState.BorderColor = Color.DarkGray;
            btn_Close.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Close.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Close.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Close.FillColor = Color.Red;
            btn_Close.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btn_Close.ForeColor = Color.White;
            btn_Close.Location = new Point(20, 10);
            btn_Close.Name = "btn_Close";
            btn_Close.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btn_Close.Size = new Size(94, 26);
            btn_Close.TabIndex = 1;
            btn_Close.Text = "الغاء";
            btn_Close.Click += btnClose_Click;
            // 
            // frmPriceIncrease
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(253, 169);
            Controls.Add(bottomPanel);
            Controls.Add(topPanel);
            Controls.Add(rbValue);
            Controls.Add(rbPercent);
            Controls.Add(txtPrice);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmPriceIncrease";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmPriceIncrease";
            Load += frmPriceIncrease_Load;
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconImage).EndInit();
            bottomPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtPrice;
        private Guna.UI2.WinForms.Guna2RadioButton rbPercent;
        private Guna.UI2.WinForms.Guna2RadioButton rbValue;
        private SmoothPanelTopConrner topPanel;
        private Label lblTitle;
        private Guna.UI2.WinForms.Guna2PictureBox iconImage;
        private SmoothPanel_BottomCorner bottomPanel;
        public Guna.UI2.WinForms.Guna2Button btn_Save;
        public Guna.UI2.WinForms.Guna2Button btn_Close;
    }
}