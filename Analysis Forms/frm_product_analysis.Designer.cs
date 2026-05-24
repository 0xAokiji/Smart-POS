namespace pos.Analysis_Forms
{
    partial class frm_product_analysis
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panelTop = new Panel();
            BtnYearlySales = new Guna.UI2.WinForms.Guna2Button();
            BtnMonthlySales = new Guna.UI2.WinForms.Guna2Button();
            BtnWeeklySales = new Guna.UI2.WinForms.Guna2Button();
            BtnTodaySales = new Guna.UI2.WinForms.Guna2Button();
            panelContent = new DevExpress.XtraEditors.PanelControl();
            gridControlSales = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            chartControlSeles = new DevExpress.XtraCharts.ChartControl();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelContent).BeginInit();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControlSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartControlSeles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.LightGray;
            panelTop.Controls.Add(BtnYearlySales);
            panelTop.Controls.Add(BtnMonthlySales);
            panelTop.Controls.Add(BtnWeeklySales);
            panelTop.Controls.Add(BtnTodaySales);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(800, 60);
            panelTop.TabIndex = 0;
            // 
            // BtnYearlySales
            // 
            BtnYearlySales.CheckedState.ForeColor = Color.White;
            BtnYearlySales.CustomizableEdges = customizableEdges9;
            BtnYearlySales.DisabledState.BorderColor = Color.DarkGray;
            BtnYearlySales.DisabledState.CustomBorderColor = Color.DarkGray;
            BtnYearlySales.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            BtnYearlySales.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            BtnYearlySales.Dock = DockStyle.Right;
            BtnYearlySales.FillColor = Color.FromArgb(1, 95, 95);
            BtnYearlySales.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            BtnYearlySales.ForeColor = Color.White;
            BtnYearlySales.Location = new Point(80, 0);
            BtnYearlySales.Name = "BtnYearlySales";
            BtnYearlySales.ShadowDecoration.CustomizableEdges = customizableEdges10;
            BtnYearlySales.Size = new Size(180, 60);
            BtnYearlySales.TabIndex = 3;
            BtnYearlySales.Text = "تحليل المبيعات السنوى";
            BtnYearlySales.Click += BtnYearlySales_Click;
            // 
            // BtnMonthlySales
            // 
            BtnMonthlySales.CheckedState.ForeColor = Color.White;
            BtnMonthlySales.CustomizableEdges = customizableEdges11;
            BtnMonthlySales.DisabledState.BorderColor = Color.DarkGray;
            BtnMonthlySales.DisabledState.CustomBorderColor = Color.DarkGray;
            BtnMonthlySales.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            BtnMonthlySales.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            BtnMonthlySales.Dock = DockStyle.Right;
            BtnMonthlySales.FillColor = Color.FromArgb(1, 95, 95);
            BtnMonthlySales.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            BtnMonthlySales.ForeColor = Color.White;
            BtnMonthlySales.Location = new Point(260, 0);
            BtnMonthlySales.Name = "BtnMonthlySales";
            BtnMonthlySales.ShadowDecoration.CustomizableEdges = customizableEdges12;
            BtnMonthlySales.Size = new Size(180, 60);
            BtnMonthlySales.TabIndex = 2;
            BtnMonthlySales.Text = "تحليل المبيعات الشهرية";
            BtnMonthlySales.Click += BtnMonthlySales_Click;
            // 
            // BtnWeeklySales
            // 
            BtnWeeklySales.CheckedState.ForeColor = Color.White;
            BtnWeeklySales.CustomizableEdges = customizableEdges13;
            BtnWeeklySales.DisabledState.BorderColor = Color.DarkGray;
            BtnWeeklySales.DisabledState.CustomBorderColor = Color.DarkGray;
            BtnWeeklySales.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            BtnWeeklySales.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            BtnWeeklySales.Dock = DockStyle.Right;
            BtnWeeklySales.FillColor = Color.FromArgb(1, 95, 95);
            BtnWeeklySales.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            BtnWeeklySales.ForeColor = Color.White;
            BtnWeeklySales.Location = new Point(440, 0);
            BtnWeeklySales.Name = "BtnWeeklySales";
            BtnWeeklySales.ShadowDecoration.CustomizableEdges = customizableEdges14;
            BtnWeeklySales.Size = new Size(180, 60);
            BtnWeeklySales.TabIndex = 1;
            BtnWeeklySales.Text = "تحليل المبيعات الاسبوعية";
            BtnWeeklySales.Click += BtnWeeklySales_Click;
            // 
            // BtnTodaySales
            // 
            BtnTodaySales.Checked = true;
            BtnTodaySales.CheckedState.BorderColor = Color.IndianRed;
            BtnTodaySales.CheckedState.CustomBorderColor = Color.Black;
            BtnTodaySales.CheckedState.ForeColor = Color.White;
            BtnTodaySales.CustomBorderColor = Color.FromArgb(64, 64, 64);
            BtnTodaySales.CustomizableEdges = customizableEdges15;
            BtnTodaySales.DisabledState.BorderColor = Color.DarkGray;
            BtnTodaySales.DisabledState.CustomBorderColor = Color.DarkGray;
            BtnTodaySales.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            BtnTodaySales.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            BtnTodaySales.Dock = DockStyle.Right;
            BtnTodaySales.FillColor = Color.FromArgb(1, 95, 95);
            BtnTodaySales.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            BtnTodaySales.ForeColor = Color.White;
            BtnTodaySales.Location = new Point(620, 0);
            BtnTodaySales.Name = "BtnTodaySales";
            BtnTodaySales.ShadowDecoration.CustomizableEdges = customizableEdges16;
            BtnTodaySales.Size = new Size(180, 60);
            BtnTodaySales.TabIndex = 0;
            BtnTodaySales.Text = "تحليل المبيعات اليومية";
            BtnTodaySales.Click += BtnTodaySales_Click;
            // 
            // panelContent
            // 
            panelContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContent.Controls.Add(gridControlSales);
            panelContent.Location = new Point(0, 297);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(800, 274);
            panelContent.TabIndex = 1;
            // 
            // gridControlSales
            // 
            gridControlSales.Dock = DockStyle.Fill;
            gridControlSales.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gridControlSales.Location = new Point(2, 2);
            gridControlSales.MainView = gridView1;
            gridControlSales.Name = "gridControlSales";
            gridControlSales.RightToLeft = RightToLeft.Yes;
            gridControlSales.Size = new Size(796, 270);
            gridControlSales.TabIndex = 0;
            gridControlSales.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { gridColumn1, gridColumn2, gridColumn3, gridColumn4 });
            gridView1.GridControl = gridControlSales;
            gridView1.Name = "gridView1";
            gridView1.OptionsPrint.EnableAppearanceOddRow = true;
            gridView1.OptionsView.ShowFooter = true;
            // 
            // gridColumn1
            // 
            gridColumn1.Caption = "اسم المنتج";
            gridColumn1.FieldName = "ProductName";
            gridColumn1.Name = "gridColumn1";
            gridColumn1.OptionsColumn.AllowEdit = false;
            gridColumn1.OptionsColumn.ReadOnly = true;
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            gridColumn2.Caption = "سعر المنتج";
            gridColumn2.FieldName = "productprice";
            gridColumn2.Name = "gridColumn2";
            gridColumn2.OptionsColumn.AllowEdit = false;
            gridColumn2.OptionsColumn.ReadOnly = true;
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            gridColumn3.Caption = "المبيعات";
            gridColumn3.FieldName = "SalesAmount";
            gridColumn3.Name = "gridColumn3";
            gridColumn3.OptionsColumn.AllowEdit = false;
            gridColumn3.OptionsColumn.ReadOnly = true;
            gridColumn3.Visible = true;
            gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            gridColumn4.Caption = "الكمية الحالية";
            gridColumn4.FieldName = "CurrentQty";
            gridColumn4.Name = "gridColumn4";
            gridColumn4.Visible = true;
            gridColumn4.VisibleIndex = 3;
            // 
            // chartControlSeles
            // 
            chartControlSeles.Dock = DockStyle.Fill;
            chartControlSeles.Location = new Point(2, 2);
            chartControlSeles.Name = "chartControlSeles";
            chartControlSeles.Size = new Size(792, 233);
            chartControlSeles.TabIndex = 1;
            // 
            // panelControl1
            // 
            panelControl1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelControl1.Controls.Add(chartControlSeles);
            panelControl1.Location = new Point(2, 54);
            panelControl1.Name = "panelControl1";
            panelControl1.Size = new Size(796, 237);
            panelControl1.TabIndex = 2;
            // 
            // frm_product_analysis
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 571);
            Controls.Add(panelControl1);
            Controls.Add(panelContent);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frm_product_analysis";
            Text = "frm_product_analysis";
            WindowState = FormWindowState.Maximized;
            Load += frm_product_analysis_Load;
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panelContent).EndInit();
            panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControlSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartControlSeles).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private Guna.UI2.WinForms.Guna2Button BtnMonthlySales;
        private Guna.UI2.WinForms.Guna2Button BtnWeeklySales;
        private Guna.UI2.WinForms.Guna2Button BtnTodaySales;
        private DevExpress.XtraEditors.PanelControl panelContent;
        private DevExpress.XtraGrid.GridControl gridControlSales;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraCharts.ChartControl chartControlSeles;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private Guna.UI2.WinForms.Guna2Button BtnYearlySales;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}