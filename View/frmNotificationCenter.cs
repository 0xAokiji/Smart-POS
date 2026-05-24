using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using pos.GeneralForms;


namespace pos.View
{
    public partial class frmNotificationCenter : Form
    {
        private frmMian2 _parentForm ;

        private Point targetLocation;
        private int slideSpeed = 50; // كل ما قل الرقم، زادت السرعة

        public frmNotificationCenter(frmMian2 frm)
        {
            InitializeComponent();
            _parentForm = frm;

            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Opacity = 0.9;

            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.Gray; // أو أي لون يناسبك
            this.Size = new Size(400, 500); // حجم مناسب
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsPath path = new GraphicsPath();
            int radius = 20; // نصف قطر الزاوية
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            this.Region = new Region(path);
        }

        private void frmNotificationCenter_Load(object sender, EventArgs e)
        {
            // أضف كل الإشعارات هنا (زي ما عملت)
            for (int i = 1; i <= 6; i++)
            {
                AddNotification($"إشعار {i}", "هذا نص الإشعار التجريبي لمعرفة الشكل النهائي للبطاقة داخل مركز التنبيهات");
            }

            // احسب الموقع المستهدف
            int topMargin = 50;
            int bottomMargin = 50;
            int leftMargin = 10;
            var screen = Screen.FromControl(_parentForm);

            int screenHeight = screen.WorkingArea.Height;
            int screenTop = screen.WorkingArea.Top;
            int screenLeft = screen.WorkingArea.Left;

            this.Height = screenHeight - (topMargin + bottomMargin);
            this.Width = 400;

            // النقطة النهائية
            targetLocation = new Point(screenLeft + leftMargin, screenTop + topMargin);

            // تعيين الموقع مباشرة
            this.Location = targetLocation;

            // أظهر الفورم فورًا بدون حركة
            this.Opacity = 1;
            this.Show();
        }

        private void StartSlideIn()
        {
            slideTimer.Interval = 1; // سرعة الحركة
            slideTimer.Tick += SlideStep;
            slideTimer.Start();
        }
        private void SlideStep(object sender, EventArgs e)
        {
            if (this.Left < targetLocation.X)
            {
                this.Left += slideSpeed;
            }
            else
            {
                this.Left = targetLocation.X;
                slideTimer.Stop();
                slideTimer.Dispose();
            }
        }


        private void frmNotificationCenter_Deactivate(object sender, EventArgs e)
        {
            this.Hide(); // أو this.Hide() إن كنت لا تريد إنهاء الفورم

        }

        private void position_Size()
        {
            int topMargin = 50;
            int bottomMargin = 50;
            int leftMargin = 10;

            var screen = Screen.FromControl(_parentForm); // الشاشة اللي فيها الفورم الأساسي

            int screenHeight = screen.WorkingArea.Height;
            int screenTop = screen.WorkingArea.Top;

            this.Height = screenHeight - (topMargin + bottomMargin);
            this.Width = 400;

            this.Location = new Point(
                screen.WorkingArea.Left + leftMargin,
                screenTop + topMargin
            );
        }



        private void AddNotification(string title, string text)
        {
            var notifPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            notifPanel.Size = new Size(flowNotifPanel.Width - 30, 115);
            notifPanel.FillColor = Color.FromArgb(64, 64, 0);
            notifPanel.Radius = 8;
            notifPanel.ShadowColor = Color.Black;
            notifPanel.Padding = new Padding(10);
            notifPanel.Margin = new Padding(5);
            notifPanel.Dock = DockStyle.Top;

            // زر الإغلاق داخل Panel خاص
            var btnPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 20,
                BackColor = Color.Transparent
            };

            var btnClose = new Button
            {
                Text = "X",
                BackColor = Color.Transparent,
                ForeColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 20),
                Dock = DockStyle.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => flowNotifPanel.Controls.Remove(notifPanel);

            btnPanel.Controls.Add(btnClose);

            // العنوان
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                RightToLeft = RightToLeft.No,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                ForeColor = Color.White
            };

            // النص
            var lblText = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9),
                Dock = DockStyle.Fill,
                RightToLeft = RightToLeft.No,
                TextAlign = ContentAlignment.TopRight,
                AutoSize = false,
                ForeColor = Color.White
            };

            // ترتيب العناصر
            notifPanel.Controls.Add(lblText);
            notifPanel.Controls.Add(lblTitle);
            notifPanel.Controls.Add(btnPanel); // أضفها في النهاية ليتم عرضها في الأعلى

            // أضف البطاقة في الأعلى داخل FlowPanel
            flowNotifPanel.Controls.Add(notifPanel);
            flowNotifPanel.Controls.SetChildIndex(notifPanel, 0); // هذا السطر يضمن أن الإشعار يظهر في الأعلى
        }


    }

}
