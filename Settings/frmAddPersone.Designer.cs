namespace pos.Settings
{
    partial class frmAddPersone
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
            components = new System.ComponentModel.Container();
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            mainPanel = new Panel();
            secondPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            txtSalary = new Guna.UI2.WinForms.Guna2TextBox();
            chbMulti = new Guna.UI2.WinForms.Guna2CheckBox();
            txtName = new Guna.UI2.WinForms.Guna2TextBox();
            btnBack = new Guna.UI2.WinForms.Guna2Button();
            txtPhone = new Guna.UI2.WinForms.Guna2TextBox();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            txtRole = new Guna.UI2.WinForms.Guna2TextBox();
            fadeTimer = new System.Windows.Forms.Timer(components);
            cbTechnical = new Guna.UI2.WinForms.Guna2CheckBox();
            mainPanel.SuspendLayout();
            secondPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(243, 243, 243);
            mainPanel.Controls.Add(secondPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(478, 324);
            mainPanel.TabIndex = 0;
            // 
            // secondPanel
            // 
            secondPanel.BackColor = Color.Transparent;
            secondPanel.Controls.Add(cbTechnical);
            secondPanel.Controls.Add(txtSalary);
            secondPanel.Controls.Add(chbMulti);
            secondPanel.Controls.Add(txtName);
            secondPanel.Controls.Add(btnBack);
            secondPanel.Controls.Add(txtPhone);
            secondPanel.Controls.Add(btnSave);
            secondPanel.Controls.Add(txtRole);
            secondPanel.FillColor = Color.FromArgb(230, 230, 230);
            secondPanel.Location = new Point(12, 12);
            secondPanel.Name = "secondPanel";
            secondPanel.Radius = 7;
            secondPanel.ShadowColor = Color.Black;
            secondPanel.Size = new Size(454, 300);
            secondPanel.TabIndex = 43;
            // 
            // txtSalary
            // 
            txtSalary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSalary.BorderColor = Color.FromArgb(136, 214, 218);
            txtSalary.BorderRadius = 8;
            txtSalary.CustomizableEdges = customizableEdges1;
            txtSalary.DefaultText = "";
            txtSalary.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSalary.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSalary.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSalary.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSalary.FillColor = Color.FromArgb(243, 243, 243);
            txtSalary.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSalary.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtSalary.ForeColor = Color.Black;
            txtSalary.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSalary.IconLeftCursor = Cursors.IBeam;
            txtSalary.IconLeftOffset = new Point(20, 20);
            txtSalary.IconRightCursor = Cursors.IBeam;
            txtSalary.Location = new Point(38, 178);
            txtSalary.Margin = new Padding(3, 5, 3, 5);
            txtSalary.Name = "txtSalary";
            txtSalary.Padding = new Padding(3, 0, 3, 0);
            txtSalary.PlaceholderText = "المرتب";
            txtSalary.SelectedText = "";
            txtSalary.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSalary.Size = new Size(99, 37);
            txtSalary.TabIndex = 20;
            txtSalary.TextAlign = HorizontalAlignment.Center;
            txtSalary.TextChanged += txtSalary_TextChanged;
            txtSalary.KeyPress += txtSalary_KeyPress;
            // 
            // chbMulti
            // 
            chbMulti.AutoSize = true;
            chbMulti.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
            chbMulti.CheckedState.BorderRadius = 0;
            chbMulti.CheckedState.BorderThickness = 0;
            chbMulti.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
            chbMulti.Location = new Point(267, 178);
            chbMulti.Name = "chbMulti";
            chbMulti.RightToLeft = RightToLeft.Yes;
            chbMulti.Size = new Size(133, 19);
            chbMulti.TabIndex = 19;
            chbMulti.Text = "اضافة اكتر من موظف";
            chbMulti.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            chbMulti.UncheckedState.BorderRadius = 0;
            chbMulti.UncheckedState.BorderThickness = 0;
            chbMulti.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.BorderColor = Color.FromArgb(136, 214, 218);
            txtName.BorderRadius = 8;
            txtName.CustomizableEdges = customizableEdges3;
            txtName.DefaultText = "";
            txtName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtName.FillColor = Color.FromArgb(243, 243, 243);
            txtName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtName.ForeColor = Color.Black;
            txtName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtName.IconLeftCursor = Cursors.IBeam;
            txtName.IconLeftOffset = new Point(20, 20);
            txtName.IconRightCursor = Cursors.IBeam;
            txtName.Location = new Point(38, 28);
            txtName.Margin = new Padding(3, 5, 3, 5);
            txtName.Name = "txtName";
            txtName.Padding = new Padding(3, 0, 3, 0);
            txtName.PlaceholderText = "اسم الموظف";
            txtName.SelectedText = "";
            txtName.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtName.Size = new Size(375, 41);
            txtName.TabIndex = 13;
            txtName.TextAlign = HorizontalAlignment.Right;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // btnBack
            // 
            btnBack.BorderRadius = 10;
            btnBack.CustomizableEdges = customizableEdges5;
            btnBack.DisabledState.BorderColor = Color.DarkGray;
            btnBack.DisabledState.CustomBorderColor = Color.DarkGray;
            btnBack.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnBack.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnBack.FillColor = Color.Red;
            btnBack.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(85, 248);
            btnBack.Name = "btnBack";
            btnBack.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnBack.Size = new Size(132, 36);
            btnBack.TabIndex = 18;
            btnBack.Text = "رجوع";
            btnBack.Click += guna2Button1_Click;
            // 
            // txtPhone
            // 
            txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPhone.BorderColor = Color.FromArgb(136, 214, 218);
            txtPhone.BorderRadius = 8;
            txtPhone.CustomizableEdges = customizableEdges7;
            txtPhone.DefaultText = "";
            txtPhone.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPhone.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPhone.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPhone.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPhone.FillColor = Color.FromArgb(243, 243, 243);
            txtPhone.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPhone.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtPhone.ForeColor = Color.Black;
            txtPhone.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPhone.IconLeftCursor = Cursors.IBeam;
            txtPhone.IconLeftOffset = new Point(20, 20);
            txtPhone.IconRightCursor = Cursors.IBeam;
            txtPhone.Location = new Point(38, 78);
            txtPhone.Margin = new Padding(3, 5, 3, 5);
            txtPhone.Name = "txtPhone";
            txtPhone.Padding = new Padding(3, 0, 3, 0);
            txtPhone.PlaceholderText = "رقم الهاتف";
            txtPhone.SelectedText = "";
            txtPhone.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtPhone.Size = new Size(375, 41);
            txtPhone.TabIndex = 14;
            txtPhone.TextAlign = HorizontalAlignment.Right;
            txtPhone.TextChanged += txtPhone_TextChanged;
            txtPhone.KeyPress += txtPhone_KeyPress;
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 10;
            btnSave.CustomizableEdges = customizableEdges9;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.Enabled = false;
            btnSave.FillColor = Color.FromArgb(136, 214, 218);
            btnSave.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(238, 248);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnSave.Size = new Size(132, 36);
            btnSave.TabIndex = 16;
            btnSave.Text = "حفظ";
            btnSave.Click += btnSave_Click;
            // 
            // txtRole
            // 
            txtRole.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRole.BorderColor = Color.FromArgb(136, 214, 218);
            txtRole.BorderRadius = 8;
            txtRole.CustomizableEdges = customizableEdges11;
            txtRole.DefaultText = "";
            txtRole.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtRole.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtRole.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtRole.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtRole.FillColor = Color.FromArgb(243, 243, 243);
            txtRole.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtRole.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtRole.ForeColor = Color.Black;
            txtRole.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtRole.IconLeftCursor = Cursors.IBeam;
            txtRole.IconLeftOffset = new Point(20, 20);
            txtRole.IconRightCursor = Cursors.IBeam;
            txtRole.Location = new Point(152, 128);
            txtRole.Margin = new Padding(3, 5, 3, 5);
            txtRole.Name = "txtRole";
            txtRole.Padding = new Padding(3, 0, 3, 0);
            txtRole.PlaceholderText = "الرتبة";
            txtRole.SelectedText = "";
            txtRole.ShadowDecoration.CustomizableEdges = customizableEdges12;
            txtRole.Size = new Size(261, 41);
            txtRole.TabIndex = 15;
            txtRole.TextAlign = HorizontalAlignment.Right;
            txtRole.TextChanged += txtRole_TextChanged;
            // 
            // cbTechnical
            // 
            cbTechnical.AutoSize = true;
            cbTechnical.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
            cbTechnical.CheckedState.BorderRadius = 0;
            cbTechnical.CheckedState.BorderThickness = 0;
            cbTechnical.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
            cbTechnical.Location = new Point(38, 140);
            cbTechnical.Name = "cbTechnical";
            cbTechnical.RightToLeft = RightToLeft.Yes;
            cbTechnical.Size = new Size(74, 19);
            cbTechnical.TabIndex = 21;
            cbTechnical.Text = "الرتبة فني";
            cbTechnical.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            cbTechnical.UncheckedState.BorderRadius = 0;
            cbTechnical.UncheckedState.BorderThickness = 0;
            cbTechnical.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            cbTechnical.CheckedChanged += cbTechnical_CheckedChanged;
            // 
            // frmAddPersone
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(478, 324);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmAddPersone";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmAddPersone";
            Load += frmAddPersone_Load;
            mainPanel.ResumeLayout(false);
            secondPanel.ResumeLayout(false);
            secondPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Guna.UI2.WinForms.Guna2TextBox txtRole;
        private Guna.UI2.WinForms.Guna2TextBox txtPhone;
        private Guna.UI2.WinForms.Guna2TextBox txtName;
        public Guna.UI2.WinForms.Guna2Button btnSave;
        public Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2ShadowPanel secondPanel;
        private Guna.UI2.WinForms.Guna2CheckBox chbMulti;
        private System.Windows.Forms.Timer fadeTimer;
        private Guna.UI2.WinForms.Guna2TextBox txtSalary;
        private Guna.UI2.WinForms.Guna2CheckBox cbTechnical;
    }
}