namespace pos.View
{
    partial class frmDetailsProView
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDetailsProView));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dataGridView1 = new Guna.UI2.WinForms.Guna2DataGridView();
            dgvso = new DataGridViewTextBoxColumn();
            dgvName = new DataGridViewTextBoxColumn();
            dgvQty = new DataGridViewTextBoxColumn();
            dgvPrice = new DataGridViewTextBoxColumn();
            dgvTotal = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            guna2ImageButton1 = new Guna.UI2.WinForms.Guna2ImageButton();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonFace;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Tahoma", 7.8F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeight = 22;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { dgvso, dgvName, dgvQty, dgvPrice, dgvTotal });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(40, 40, 40);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Enabled = false;
            dataGridView1.GridColor = Color.FromArgb(231, 229, 255);
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RightToLeft = RightToLeft.Yes;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(361, 24);
            dataGridView1.TabIndex = 0;
            dataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dataGridView1.ThemeStyle.AlternatingRowsStyle.Font = null;
            dataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dataGridView1.ThemeStyle.BackColor = SystemColors.ButtonFace;
            dataGridView1.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dataGridView1.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dataGridView1.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dataGridView1.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ThemeStyle.HeaderStyle.Height = 22;
            dataGridView1.ThemeStyle.ReadOnly = false;
            dataGridView1.ThemeStyle.RowsStyle.BackColor = Color.White;
            dataGridView1.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dataGridView1.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridView1.ThemeStyle.RowsStyle.Height = 29;
            dataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridView1.RowsAdded += dataGridView1_RowsAdded;
            dataGridView1.RowsRemoved += dataGridView1_RowsRemoved;
            // 
            // dgvso
            // 
            dgvso.FillWeight = 19.014719F;
            dgvso.HeaderText = "#";
            dgvso.MinimumWidth = 6;
            dgvso.Name = "dgvso";
            // 
            // dgvName
            // 
            dgvName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvName.FillWeight = 98.23529F;
            dgvName.HeaderText = "الاسم";
            dgvName.MinimumWidth = 6;
            dgvName.Name = "dgvName";
            dgvName.Width = 67;
            // 
            // dgvQty
            // 
            dgvQty.FillWeight = 50F;
            dgvQty.HeaderText = "الكمية";
            dgvQty.MinimumWidth = 50;
            dgvQty.Name = "dgvQty";
            // 
            // dgvPrice
            // 
            dgvPrice.FillWeight = 50F;
            dgvPrice.HeaderText = "السعر";
            dgvPrice.MinimumWidth = 6;
            dgvPrice.Name = "dgvPrice";
            // 
            // dgvTotal
            // 
            dgvTotal.FillWeight = 80.81255F;
            dgvTotal.HeaderText = "الاجمالي";
            dgvTotal.MinimumWidth = 6;
            dgvTotal.Name = "dgvTotal";
            // 
            // panel1
            // 
            panel1.Controls.Add(guna2ImageButton1);
            panel1.Controls.Add(dataGridView1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(361, 24);
            panel1.TabIndex = 3;
            // 
            // guna2ImageButton1
            // 
            guna2ImageButton1.BackColor = Color.FromArgb(100, 88, 255);
            guna2ImageButton1.CheckedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.HoverState.ImageSize = new Size(64, 64);
            guna2ImageButton1.Image = (Image)resources.GetObject("guna2ImageButton1.Image");
            guna2ImageButton1.ImageOffset = new Point(0, 0);
            guna2ImageButton1.ImageRotate = 0F;
            guna2ImageButton1.ImageSize = new Size(24, 24);
            guna2ImageButton1.Location = new Point(9, -2);
            guna2ImageButton1.Name = "guna2ImageButton1";
            guna2ImageButton1.PressedState.ImageSize = new Size(64, 64);
            guna2ImageButton1.ShadowDecoration.CustomizableEdges = customizableEdges1;
            guna2ImageButton1.Size = new Size(24, 24);
            guna2ImageButton1.TabIndex = 4;
            guna2ImageButton1.Click += guna2ImageButton1_Click;
            // 
            // frmDetailsProView
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(361, 24);
            Controls.Add(panel1);
            FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmDetailsProView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmDetailsProView";
            Load += frmDetailsProView_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dataGridView1;
        private DataGridViewTextBoxColumn dgvso;
        private DataGridViewTextBoxColumn dgvName;
        private DataGridViewTextBoxColumn dgvQty;
        private Panel panel1;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton1;
        private DataGridViewTextBoxColumn dgvPrice;
        private DataGridViewTextBoxColumn dgvTotal;
    }
}