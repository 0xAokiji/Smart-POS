namespace pos.frmReports
{
    partial class frmReportSalesView
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

        
        private void InitializeComponent()
        {
            salesPanel = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // salesPanel
            // 
            salesPanel.AutoScroll = true;
            salesPanel.Dock = DockStyle.Fill;
            salesPanel.Location = new Point(0, 0);
            salesPanel.Name = "salesPanel";
            salesPanel.RightToLeft = RightToLeft.Yes;
            salesPanel.Size = new Size(540, 587);
            salesPanel.TabIndex = 3;
            // 
            // frmReportSalesView
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(540, 587);
            Controls.Add(salesPanel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmReportSalesView";
            Text = "frmReportSalesView";
            Load += frmReportSalesView_Load;
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel salesPanel;
    }
}