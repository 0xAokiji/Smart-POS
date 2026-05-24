using pos.Classes;

namespace pos.SystemApp
{
    partial class frmTrialTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrialTime));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtKey = new Guna.UI2.WinForms.Guna2TextBox();
            label1 = new Label();
            btn_cont = new Guna.UI2.WinForms.Guna2Button();
            btnClose = new Guna.UI2.WinForms.Guna2Button();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            smoothPanelTopConrner1 = new SmoothPanelTopConrner();
            btnExit = new Guna.UI2.WinForms.Guna2Button();
            smoothPanel_BottomCorner1 = new SmoothPanel_BottomCorner();
            smoothPanel1 = new SmoothPanel();
            guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            smoothPanelTopConrner1.SuspendLayout();
            smoothPanel_BottomCorner1.SuspendLayout();
            smoothPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtKey
            // 
            txtKey.BorderColor = Color.FromArgb(1, 95, 95);
            txtKey.BorderRadius = 8;
            txtKey.BorderThickness = 2;
            txtKey.CustomizableEdges = customizableEdges1;
            txtKey.DefaultText = "";
            txtKey.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtKey.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtKey.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtKey.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtKey.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtKey.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtKey.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtKey.IconLeftSize = new Size(40, 40);
            txtKey.IconRight = (Image)resources.GetObject("txtKey.IconRight");
            txtKey.IconRightSize = new Size(40, 40);
            txtKey.Location = new Point(9, 82);
            txtKey.Margin = new Padding(4, 5, 4, 5);
            txtKey.Name = "txtKey";
            txtKey.PlaceholderText = "ادخل المفتاح التفعيل";
            txtKey.RightToLeft = RightToLeft.No;
            txtKey.SelectedText = "";
            txtKey.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtKey.Size = new Size(389, 35);
            txtKey.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            txtKey.TabIndex = 5;
            txtKey.TextChanged += txtKey_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(8, 42);
            label1.Name = "label1";
            label1.Size = new Size(401, 19);
            label1.TabIndex = 3;
            label1.Text = "هذه نسخة تجريبية لمدة 30 يومًا. بعد ذلك، يلزم إدخال مفتاح التفعيل.";
            // 
            // btn_cont
            // 
            btn_cont.BorderRadius = 8;
            btn_cont.CustomizableEdges = customizableEdges3;
            btn_cont.DisabledState.BorderColor = Color.DarkGray;
            btn_cont.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_cont.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_cont.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_cont.FillColor = Color.FromArgb(1, 95, 95);
            btn_cont.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btn_cont.ForeColor = Color.White;
            btn_cont.Location = new Point(185, 7);
            btn_cont.Name = "btn_cont";
            btn_cont.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btn_cont.Size = new Size(183, 35);
            btn_cont.TabIndex = 4;
            btn_cont.Text = "استمرار";
            btn_cont.Click += btn_cont_Click;
            // 
            // btnClose
            // 
            btnClose.BorderRadius = 8;
            btnClose.CustomizableEdges = customizableEdges5;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.FromArgb(1, 95, 95);
            btnClose.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(185, 7);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnClose.Size = new Size(183, 35);
            btnClose.TabIndex = 3;
            btnClose.Text = "نتهت النسخة التجريبية";
            btnClose.Click += btnClose_Click;
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 8;
            btnSave.CustomizableEdges = customizableEdges7;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.FillColor = Color.Green;
            btnSave.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(47, 7);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnSave.Size = new Size(112, 35);
            btnSave.TabIndex = 2;
            btnSave.Text = "تسجيل";
            btnSave.Click += btnSave_Click;
            // 
            // smoothPanelTopConrner1
            // 
            smoothPanelTopConrner1.BackColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanelTopConrner1.BorderSize = 1F;
            smoothPanelTopConrner1.Controls.Add(btnExit);
            smoothPanelTopConrner1.Dock = DockStyle.Top;
            smoothPanelTopConrner1.Location = new Point(0, 0);
            smoothPanelTopConrner1.Name = "smoothPanelTopConrner1";
            smoothPanelTopConrner1.Size = new Size(415, 49);
            smoothPanelTopConrner1.TabIndex = 6;
            // 
            // btnExit
            // 
            btnExit.BorderRadius = 5;
            btnExit.CustomizableEdges = customizableEdges9;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.FromArgb(255, 128, 128);
            btnExit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(8, 12);
            btnExit.Name = "btnExit";
            btnExit.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnExit.Size = new Size(40, 25);
            btnExit.TabIndex = 42;
            btnExit.Text = "X";
            btnExit.Click += btnExit_Click;
            // 
            // smoothPanel_BottomCorner1
            // 
            smoothPanel_BottomCorner1.BackColor = Color.FromArgb(230, 230, 230);
            smoothPanel_BottomCorner1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel_BottomCorner1.BorderSize = 1F;
            smoothPanel_BottomCorner1.Controls.Add(btnSave);
            smoothPanel_BottomCorner1.Controls.Add(btn_cont);
            smoothPanel_BottomCorner1.Controls.Add(btnClose);
            smoothPanel_BottomCorner1.Dock = DockStyle.Bottom;
            smoothPanel_BottomCorner1.Location = new Point(0, 208);
            smoothPanel_BottomCorner1.Name = "smoothPanel_BottomCorner1";
            smoothPanel_BottomCorner1.Size = new Size(415, 48);
            smoothPanel_BottomCorner1.TabIndex = 7;
            // 
            // smoothPanel1
            // 
            smoothPanel1.BackColor = Color.FromArgb(243, 243, 243);
            smoothPanel1.BorderColor = Color.FromArgb(1, 95, 95);
            smoothPanel1.BorderSize = 1F;
            smoothPanel1.Controls.Add(label1);
            smoothPanel1.Controls.Add(txtKey);
            smoothPanel1.Dock = DockStyle.Fill;
            smoothPanel1.Location = new Point(0, 49);
            smoothPanel1.Name = "smoothPanel1";
            smoothPanel1.Size = new Size(415, 159);
            smoothPanel1.TabIndex = 8;
            // 
            // guna2MessageDialog1
            // 
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Caption = null;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            guna2MessageDialog1.Parent = null;
            guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            guna2MessageDialog1.Text = null;
            // 
            // frmTrialTime
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(415, 256);
            Controls.Add(smoothPanel1);
            Controls.Add(smoothPanel_BottomCorner1);
            Controls.Add(smoothPanelTopConrner1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmTrialTime";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmTrialTime";
            Load += frmTrialTime_Load;
            smoothPanelTopConrner1.ResumeLayout(false);
            smoothPanel_BottomCorner1.ResumeLayout(false);
            smoothPanel1.ResumeLayout(false);
            smoothPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        public Guna.UI2.WinForms.Guna2Button btnClose;
        public Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2TextBox txtKey;
        public Guna.UI2.WinForms.Guna2Button btn_cont;
        private SmoothPanelTopConrner smoothPanelTopConrner1;
        private SmoothPanel_BottomCorner smoothPanel_BottomCorner1;
        private SmoothPanel smoothPanel1;
        private Guna.UI2.WinForms.Guna2Button btnExit;
        private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
    }
}