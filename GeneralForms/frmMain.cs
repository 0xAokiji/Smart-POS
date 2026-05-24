using pos.GeneralForms;
using pos.Model;
using pos.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frmMain : Form
    {
        bool x = true;
        public frmMain()
        {

            InitializeComponent();
            frmGraphicalinterFace frmGraphicalinterFace = new frmGraphicalinterFace(true);
            this.Icon = frmGraphicalinterFace.Icon;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = false;
            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);

            this.panel14.Paint += panel1_Paint; // إضافة معالج الحدث Paint
            this.timer1.Tick += timer1_Tick; // إضافة معالج الحدث Tick للـ Timer
            this.timer1.Interval = 180;
            this.timer1.Start(); // بدء Timer

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogout frmLogout = new frmLogout();

            if (frmLogout.MyBool)
                Application.Exit();
            //else { }
        }


        static frmMain _obj;
        public static frmMain Instance
        {
            get { if (_obj == null) { _obj = new frmMain(); } return _obj; }
        }


        //Method to add controls in Main form
        public void AddControls(Form f)
        {

            ControlsPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            ControlsPanel.Controls.Add(f);
            f.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            btnHome.ShadowDecoration.Enabled = true; // تفعيل تأثير الظل
                                                     // lblUser.Text = MainClass.USER;
            _obj = this;

            AddControls(new frmHome());
            btnHome.Checked = true;
            frmLogout frmLogout = new frmLogout();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            btnHome.Checked = true;
            AddControls(new frmHome());
        }



        private void btnStaff_Click(object sender, EventArgs e)
        {
            //AddControls(new frmStaffView(false));
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            AddControls(new frmProductView());
        }


        private void btnPOS_Click(object sender, EventArgs e)
        {
            
            AddControls(new frmPOS());

        }

        private void btnKetchen_Click(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            //frmSettings1 frmSettings = new frmSettings1();
            //frmSettings.ShowDialog();
        }

        private void btnpurchases_Click(object sender, EventArgs e)
        {
            AddControls(new frmpurchaseView());
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmLogout frmLogout = new frmLogout();
            frmLogout.ShowDialog();
        }
        public void logOut(bool check)
        {
            if (check)
            {
                frmLogin frmLogin = new frmLogin();
                frmLogin.ShowDialog();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddControls(new frmShortcomings());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            AddControls(new frmMaintenanceView());
        }

        private void btnHome_Leave(object sender, EventArgs e)
        {
            btnHome.Checked = false;
        }

        private void btnPOS_Leave(object sender, EventArgs e)
        {
            btnPOS.Checked = false;
        }

        private void guna2Button3_Leave(object sender, EventArgs e)
        {
            guna2Button3.Checked = false;
        }



        private void btnProduct_Leave(object sender, EventArgs e)
        {
            btnProduct.Checked = false;
        }

        private void guna2Button2_Leave(object sender, EventArgs e)
        {
            guna2Button2.Checked = false;
        }

        private void btnpurchases_Leave(object sender, EventArgs e)
        {
            btnpurchases.Checked = false;
        }



        private void btnStaff_Leave(object sender, EventArgs e)
        {
            btnStaff.Checked = false;
        }

        private void btnSettings_Leave(object sender, EventArgs e)
        {
            btnSettings.Checked = false;
        }

        private void guna2Button1_Leave(object sender, EventArgs e)
        {
            guna2Button1.Checked = false;
        }

        private int i = 0;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (i % 2 == 0)
            {
                guna2Panel1.Size = new System.Drawing.Size(258, 901);
                i++;
                pictureBox1.Image = Properties.Resources.mor2;

            }
            else
            {
                guna2Panel1.Size = new System.Drawing.Size(88, 901);
                i++;
                pictureBox1.Image = Properties.Resources.more;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            xPosition += 5; // تحديث موقع النص بزيادة 5 بكسل
            if (xPosition > this.panel14.Width)
            {
                xPosition = -100; // إعادة تعيين الموقع إلى البداية إذا تجاوز النص الـ Panel
            }
            this.panel14.Invalidate(); // إجبار الـ Panel على إعادة الرسم
        }
        private int xPosition = 0; // موقع البداية للنص

        private void panel14_Paint(object sender, PaintEventArgs e)
        {
            string text = MainClass.USER;
            using (Font font = new Font("Arial", 12))
            {
                SizeF textSize = e.Graphics.MeasureString(text, font); // قياس حجم النص
                                                                       // حساب الإحداثي العمودي ليكون النص في الوسط
                float yPosition = (this.panel14.Height - textSize.Height) / 2;
                e.Graphics.Clear(panel14.BackColor); // مسح الخلفية لتجنب التداخلات البصرية
                e.Graphics.DrawString(text, font, Brushes.Black, xPosition, yPosition); // رسم النص في الموقع الجديد
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void ptnResidual_Click(object sender, EventArgs e)
        {
            AddControls(new frmPOS());
        }
    }
}
