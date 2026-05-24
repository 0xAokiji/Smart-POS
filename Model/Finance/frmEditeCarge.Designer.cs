using pos.Classes;

namespace pos.Model.Finance
{
    partial class frmEditeCarge
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
            topPanel = new SmoothPanelTopConrner();
            lblTitle = new Label();
            smoothPanel_BottomCorner1 = new SmoothPanel_BottomCorner();
            btnExite = new Guna.UI2.WinForms.Guna2Button();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            smoothPanel1 = new SmoothPanel();
            label1 = new Label();
            lblTotal = new Label();
            txtCurrentBalance = new Guna.UI2.WinForms.Guna2TextBox();
            txtcharge = new Guna.UI2.WinForms.Guna2TextBox();
            topPanel.SuspendLayout();
            smoothPanel_BottomCorner1.SuspendLayout();
            smoothPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(1, 95, 95);
            topPanel.BorderColor = Color.FromArgb(1, 95, 95);
            topPanel.BorderSize = 2F;
            topPanel.Controls.Add(lblTitle);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(397, 39);
            topPanel.TabIndex = 36;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(204, 204, 204);
            lblTitle.ImeMode = ImeMode.NoControl;
            lblTitle.Location = new Point(339, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(46, 20);
            lblTitle.TabIndex = 5;
            lblTitle.Text = "تعديل";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // smoothPanel_BottomCorner1
            // 
            smoothPanel_BottomCorner1.BackColor = Color.FromArgb(230, 230, 230);
            smoothPanel_BottomCorner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel_BottomCorner1.BorderSize = 2F;
            smoothPanel_BottomCorner1.Controls.Add(btnExite);
            smoothPanel_BottomCorner1.Controls.Add(btnSave);
            smoothPanel_BottomCorner1.Dock = DockStyle.Bottom;
            smoothPanel_BottomCorner1.Location = new Point(0, 155);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(397, 48);
            smoothPanel_BottomCorner1.TabIndex = 37;
            // 
            // btnExite
            // 
            btnExite.BorderRadius = 8;
            btnExite.CustomizableEdges = customizableEdges1;
            btnExite.DisabledState.BorderColor = Color.DarkGray;
            btnExite.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExite.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExite.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExite.FillColor = Color.Red;
            btnExite.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnExite.ForeColor = Color.White;
            btnExite.ImageSize = new Size(15, 15);
            btnExite.Location = new Point(181, 9);
            btnExite.Name = "btnExite";
            btnExite.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnExite.Size = new Size(87, 30);
            btnExite.TabIndex = 32;
            btnExite.Text = "خروج";
            btnExite.Click += btnExite_Click;
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 8;
            btnSave.CustomizableEdges = customizableEdges3;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.FillColor = Color.FromArgb(1, 95, 95);
            btnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(286, 9);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSave.Size = new Size(101, 30);
            btnSave.TabIndex = 34;
            btnSave.Text = "حفظ";
            btnSave.Click += btnSave_Click;
            // 
            // smoothPanel1
            // 
            smoothPanel1.BackColor = Color.FromArgb(243, 243, 243);
            smoothPanel1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(label1);
            smoothPanel1.Controls.Add(lblTotal);
            smoothPanel1.Controls.Add(txtCurrentBalance);
            smoothPanel1.Controls.Add(txtcharge);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 39);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(397, 116);
            smoothPanel1.TabIndex = 38;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F);
            label1.Location = new Point(33, 33);
            label1.Name = "label1";
            label1.Size = new Size(141, 17);
            label1.TabIndex = 41;
            label1.Text = "اجمالي المدين قبل الدفع";
            // 
            // lblTotal
            // 
            lblTotal.Anchor = AnchorStyles.None;
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 9.75F);
            lblTotal.Location = new Point(237, 33);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(96, 17);
            lblTotal.TabIndex = 40;
            lblTotal.Text = "القيمة المدفوعة";
            // 
            // txtCurrentBalance
            // 
            txtCurrentBalance.BorderColor = Color.FromArgb(136, 214, 218);
            txtCurrentBalance.BorderRadius = 8;
            txtCurrentBalance.CustomizableEdges = customizableEdges5;
            txtCurrentBalance.DefaultText = "";
            txtCurrentBalance.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtCurrentBalance.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtCurrentBalance.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtCurrentBalance.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtCurrentBalance.FillColor = Color.WhiteSmoke;
            txtCurrentBalance.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtCurrentBalance.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtCurrentBalance.ForeColor = Color.FromArgb(64, 64, 64);
            txtCurrentBalance.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtCurrentBalance.Location = new Point(30, 54);
            txtCurrentBalance.Margin = new Padding(3, 4, 3, 4);
            txtCurrentBalance.Name = "txtCurrentBalance";
            txtCurrentBalance.PlaceholderText = "اجمالي المدين قبل الدفع";
            txtCurrentBalance.ReadOnly = true;
            txtCurrentBalance.RightToLeft = RightToLeft.No;
            txtCurrentBalance.SelectedText = "";
            txtCurrentBalance.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtCurrentBalance.Size = new Size(150, 30);
            txtCurrentBalance.TabIndex = 10;
            txtCurrentBalance.TextAlign = HorizontalAlignment.Center;
            // 
            // txtcharge
            // 
            txtcharge.BorderColor = Color.FromArgb(136, 214, 218);
            txtcharge.BorderRadius = 8;
            txtcharge.CustomizableEdges = customizableEdges7;
            txtcharge.DefaultText = "";
            txtcharge.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtcharge.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtcharge.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtcharge.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtcharge.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtcharge.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            txtcharge.ForeColor = Color.FromArgb(64, 64, 64);
            txtcharge.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtcharge.Location = new Point(204, 54);
            txtcharge.Margin = new Padding(3, 4, 3, 4);
            txtcharge.Name = "txtcharge";
            txtcharge.PlaceholderText = "القيمة المدفوعة";
            txtcharge.RightToLeft = RightToLeft.No;
            txtcharge.SelectedText = "";
            txtcharge.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtcharge.Size = new Size(167, 30);
            txtcharge.TabIndex = 9;
            txtcharge.TextAlign = HorizontalAlignment.Center;
            txtcharge.KeyPress += txtcharge_KeyPress;
            // 
            // frmEditeCarge
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(397, 203);
            Controls.Add(smoothPanel1);
            Controls.Add(smoothPanel_BottomCorner1);
            Controls.Add(topPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmEditeCarge";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmEditeCarge";
            Load += frmEditeCarge_Load;
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            smoothPanel_BottomCorner1.ResumeLayout(false);
            smoothPanel1.ResumeLayout(false);
            smoothPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SmoothPanelTopConrner topPanel;
        public Label lblTitle;
        private SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
        public Guna.UI2.WinForms.Guna2Button btnExite;
        public Guna.UI2.WinForms.Guna2Button btnSave;
        private SmoothPanel smoothPanel1;
        private Guna.UI2.WinForms.Guna2TextBox txtCurrentBalance;
        private Guna.UI2.WinForms.Guna2TextBox txtcharge;
        private Label label1;
        private Label lblTotal;
    }
}