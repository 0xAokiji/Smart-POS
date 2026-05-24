using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.View
{
    public partial class frmQustion : DevExpress.XtraEditors.XtraForm
    {
        public frmQustion()
        {
            InitializeComponent();
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Continue;
            this.Close();
        }

        private void frmQustion_Load(object sender, EventArgs e)
        {
            this.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int radius = 12; // قطر الدائرة التي تحدد منحنى الحواف

                // أركان النافذة
                Rectangle corner1 = new Rectangle(0, 0, radius * 2, radius * 2);
                Rectangle corner2 = new Rectangle(this.Width - radius * 2, 0, radius * 2, radius * 2);
                Rectangle corner3 = new Rectangle(0, this.Height - radius * 2, radius * 2, radius * 2);
                Rectangle corner4 = new Rectangle(this.Width - radius * 2, this.Height - radius * 2, radius * 2, radius * 2);

                path.AddArc(corner1, 180, 90);
                path.AddArc(corner2, 270, 90);
                path.AddArc(corner4, 0, 90);
                path.AddArc(corner3, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            };
            SystemSounds.Beep.Play();

            int x = (guna2Panel2.Width - lblTxt.Width) / 2;
            int y = (guna2Panel2.Height - lblTxt.Height) / 2;
            lblTxt.Location = new Point(x, y);
        }

        private void lblTxt_Resize(object sender, EventArgs e)
        {
            guna2Panel2.Width = lblTxt.Width + 50 ;
        }

        private void guna2Panel2_Resize(object sender, EventArgs e)
        {
            this.Width = guna2Panel2.Width;
        }
    }
}