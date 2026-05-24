namespace pos.Model.Stor
{
    partial class frmShowBills
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
            menuStrip = new MenuStrip();
            AddItem = new ToolStripMenuItem();
            فواتيرالعميلToolStripMenuItem = new ToolStripMenuItem();
            فواتيرالعملاءالمحذوفةToolStripMenuItem = new ToolStripMenuItem();
            toolStripProduct = new ToolStripMenuItem();
            عرضالمنتجاتToolStripMenuItem1 = new ToolStripMenuItem();
            اضافةمنتجبدونفاتورهToolStripMenuItem = new ToolStripMenuItem();
            mainPanel = new Panel();
            guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.FromArgb(1, 95, 95);
            menuStrip.Font = new Font("Segoe UI", 11F);
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { AddItem, toolStripProduct });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.RightToLeft = RightToLeft.Yes;
            menuStrip.Size = new Size(800, 28);
            menuStrip.TabIndex = 14;
            menuStrip.Text = "menuStrip1";
            // 
            // AddItem
            // 
            AddItem.DropDownItems.AddRange(new ToolStripItem[] { فواتيرالعميلToolStripMenuItem, فواتيرالعملاءالمحذوفةToolStripMenuItem });
            AddItem.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            AddItem.ForeColor = Color.FromArgb(204, 204, 204);
            AddItem.Image = Properties.Resources.customer;
            AddItem.Name = "AddItem";
            AddItem.Size = new Size(123, 24);
            AddItem.Text = "فواتير العملاء";
            // 
            // فواتيرالعميلToolStripMenuItem
            // 
            فواتيرالعميلToolStripMenuItem.Image = Properties.Resources.showpass_dark;
            فواتيرالعميلToolStripMenuItem.Name = "فواتيرالعميلToolStripMenuItem";
            فواتيرالعميلToolStripMenuItem.Size = new Size(232, 24);
            فواتيرالعميلToolStripMenuItem.Text = "عرض فواتير العميل ";
            فواتيرالعميلToolStripMenuItem.Click += فواتيرالعميلToolStripMenuItem_Click;
            // 
            // فواتيرالعملاءالمحذوفةToolStripMenuItem
            // 
            فواتيرالعملاءالمحذوفةToolStripMenuItem.Image = Properties.Resources.delete_black;
            فواتيرالعملاءالمحذوفةToolStripMenuItem.Name = "فواتيرالعملاءالمحذوفةToolStripMenuItem";
            فواتيرالعملاءالمحذوفةToolStripMenuItem.Size = new Size(232, 24);
            فواتيرالعملاءالمحذوفةToolStripMenuItem.Text = "عرض الفوواتير المحذوفة";
            فواتيرالعملاءالمحذوفةToolStripMenuItem.Click += فواتيرالعملاءالمحذوفةToolStripMenuItem_Click;
            // 
            // toolStripProduct
            // 
            toolStripProduct.DropDownItems.AddRange(new ToolStripItem[] { عرضالمنتجاتToolStripMenuItem1, اضافةمنتجبدونفاتورهToolStripMenuItem });
            toolStripProduct.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            toolStripProduct.ForeColor = Color.FromArgb(204, 204, 204);
            toolStripProduct.Image = Properties.Resources.supplier;
            toolStripProduct.Name = "toolStripProduct";
            toolStripProduct.Size = new Size(135, 24);
            toolStripProduct.Text = "فواتير الموردين";
            // 
            // عرضالمنتجاتToolStripMenuItem1
            // 
            عرضالمنتجاتToolStripMenuItem1.Image = Properties.Resources.showpass_dark;
            عرضالمنتجاتToolStripMenuItem1.Name = "عرضالمنتجاتToolStripMenuItem1";
            عرضالمنتجاتToolStripMenuItem1.Size = new Size(228, 26);
            عرضالمنتجاتToolStripMenuItem1.Text = "عرض فواتير الموردين";
            عرضالمنتجاتToolStripMenuItem1.Click += عرضالمنتجاتToolStripMenuItem1_Click;
            // 
            // اضافةمنتجبدونفاتورهToolStripMenuItem
            // 
            اضافةمنتجبدونفاتورهToolStripMenuItem.Image = Properties.Resources.delete_black;
            اضافةمنتجبدونفاتورهToolStripMenuItem.Name = "اضافةمنتجبدونفاتورهToolStripMenuItem";
            اضافةمنتجبدونفاتورهToolStripMenuItem.Size = new Size(228, 26);
            اضافةمنتجبدونفاتورهToolStripMenuItem.Text = "عرض الفواتير المحذوفة";
            اضافةمنتجبدونفاتورهToolStripMenuItem.Click += اضافةمنتجبدونفاتورهToolStripMenuItem_Click;
            // 
            // mainPanel
            // 
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 28);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(800, 422);
            mainPanel.TabIndex = 15;
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
            // frmShowBills
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(mainPanel);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmShowBills";
            Text = "frmShowBills";
            Load += frmShowBills_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem AddItem;
        private ToolStripMenuItem فواتيرالعميلToolStripMenuItem;
        private ToolStripMenuItem فواتيرالعملاءالمحذوفةToolStripMenuItem;
        private ToolStripMenuItem toolStripProduct;
        private ToolStripMenuItem عرضالمنتجاتToolStripMenuItem1;
        private ToolStripMenuItem اضافةمنتجبدونفاتورهToolStripMenuItem;
        private Panel mainPanel;
        private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
    }
}