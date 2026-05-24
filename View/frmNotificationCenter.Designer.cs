namespace pos.View
{
    partial class frmNotificationCenter
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
            flowNotifPanel = new FlowLayoutPanel();
            guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            button1 = new Button();
            label2 = new Label();
            label1 = new Label();
            slideTimer = new System.Windows.Forms.Timer(components);
            flowNotifPanel.SuspendLayout();
            guna2ShadowPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowNotifPanel
            // 
            flowNotifPanel.AutoScroll = true;
            flowNotifPanel.Controls.Add(guna2ShadowPanel1);
            flowNotifPanel.Dock = DockStyle.Fill;
            flowNotifPanel.ForeColor = Color.Gray;
            flowNotifPanel.Location = new Point(0, 0);
            flowNotifPanel.Name = "flowNotifPanel";
            flowNotifPanel.Size = new Size(800, 450);
            flowNotifPanel.TabIndex = 1;
            // 
            // guna2ShadowPanel1
            // 
            guna2ShadowPanel1.BackColor = Color.Transparent;
            guna2ShadowPanel1.Controls.Add(button1);
            guna2ShadowPanel1.Controls.Add(label2);
            guna2ShadowPanel1.Controls.Add(label1);
            guna2ShadowPanel1.FillColor = Color.FromArgb(64, 64, 0);
            guna2ShadowPanel1.ForeColor = Color.FromArgb(128, 64, 64);
            guna2ShadowPanel1.Location = new Point(3, 3);
            guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            guna2ShadowPanel1.Radius = 10;
            guna2ShadowPanel1.ShadowColor = Color.Black;
            guna2ShadowPanel1.Size = new Size(453, 100);
            guna2ShadowPanel1.TabIndex = 1;
            guna2ShadowPanel1.Visible = false;
            // 
            // button1
            // 
            button1.Location = new Point(369, 3);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 5;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Top;
            label2.ForeColor = Color.Transparent;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.RightToLeft = RightToLeft.No;
            label2.Size = new Size(453, 15);
            label2.TabIndex = 4;
            label2.Text = "ملاحظات إضافية:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(27, 48);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.No;
            label1.Size = new Size(504, 135);
            label1.TabIndex = 3;
            label1.Text = "هل تحب أضيف خاصية تمييز لون الإشعار حسب النوع؟ زي أخضر للنجاح، وأحمر للخطأ، وأزرق للمعلومات؟\n\n\n\n\n\n\n\n\n";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmNotificationCenter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.Black;
            ClientSize = new Size(800, 450);
            Controls.Add(flowNotifPanel);
            ForeColor = Color.Black;
            Name = "frmNotificationCenter";
            RightToLeftLayout = true;
            Text = "frmNotificationCenter";
            Deactivate += frmNotificationCenter_Deactivate;
            Load += frmNotificationCenter_Load;
            flowNotifPanel.ResumeLayout(false);
            guna2ShadowPanel1.ResumeLayout(false);
            guna2ShadowPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private FlowLayoutPanel flowNotifPanel;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Label label2;
        private Label label1;
        private Button button1;
        private System.Windows.Forms.Timer slideTimer;
    }
}