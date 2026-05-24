using DevExpress.XtraGauges.Core.Base;
using pos.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frmLogout : Form
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public frmLogout()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            // تغيير خصائص النافذة لمنع ظهورها في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
        }

        private bool _myBool;
        public bool MyBool
        {
            get
            {
                if (_myBool)
                {
                    _myBool = false;
                    return false;
                }
                else
                {
                    _myBool = false;
                    return true;
                }
            }
            set
            {

            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            _myBool = true;
            bool check = true;
            this.Close();
            frmMain frm = new frmMain();
            frm.logOut(check);


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogout_Load(object sender, EventArgs e)
        {
            this.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int radius = 12; // تقليل قطر الدائرة لجعل الحواف أقل دائرية

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


        }
    }
}
