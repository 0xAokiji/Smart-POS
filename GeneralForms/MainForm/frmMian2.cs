using DevExpress.Data.Mask.Internal;
using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using pos.Classes;
using pos.GeneralForms.MainForm;
using pos.Model;
using pos.Model.Finance;
using pos.Model.POS;
using pos.Model.Stor;
using pos.Settings;
using pos.SystemApp;
using pos.Test;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using static Syncfusion.Windows.Forms.Tools.MenuDropDown;



namespace pos.GeneralForms
{
    public partial class frmMian2 : DevExpress.XtraEditors.XtraForm
    {
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color checkedFillColor;
        private Color checkedForColor;


        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private frmPOS posForm = null;
        private ToolStripButton myButton;

        [DllImport("user32.dll")]
        public static extern int ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        const int SB_HORZ = 0;
        const int SB_VERT = 1;


        public frmMian2()
        {
            // ✅ فعّل الـ DoubleBuffering للفورم وكل الرسم الداخلي
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            InitializeComponent();

            // ✅ اخفي الفورم أثناء التحميل لتمنع ظهور العناصر تدريجيًا
            this.Opacity = 0; // بدلاً من Visible=false (أنعم بصريًا)

            themRefresh();

            frmGraphicalinterFace frmGraphicalinterFace = new frmGraphicalinterFace(true);
            this.Icon = frmGraphicalinterFace.Icon;
            //this.FormBorderStyle = FormBorderStyle.Sizable;
            //this.WindowState = FormWindowState.Maximized;
            this.TopMost = false;
            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);

            this.timer1.Tick += timer1_Tick; // إضافة معالج الحدث Tick للـ Timer
            this.timer1.Interval = 500;
            this.timer1.Start(); // بدء Timer
            this.Resize += new EventHandler(MyForm_Resize);



        }
        private async void FadeIn()
        {
            for (double i = 0; i <= 1; i += 0.05)
            {
                this.Opacity = i;
                await Task.Delay(15);
            }

            // ✅ تأكيد إن الفورم مش شفافة إطلاقًا
            this.Opacity = 1;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

            this.SuspendLayout();
            this.ResumeLayout();
            this.BeginInvoke(new Action(FadeIn)); // يظهرها تدريجيًا

            lblUserName.Text = MainClass.USER;
            lblUserName.Left = (userImgPanel.Width - lblUserName.Width) / 2;
            if (MainClass.IMAGEBYTES != null)
            {
                using (MemoryStream stream = new MemoryStream(MainClass.IMAGEBYTES))
                {

                    userImage.Image = Image.FromStream(stream);
                }
            }

            AddControls(new frmHome());
            btnHome.Checked = true;
            frmLogout frmLogout = new frmLogout();
            if (IsFormOpen(typeof(frmPOS)))
            {
                Form openForm = GetOpenForm(typeof(frmGraphicalinterFace));
                if (openForm != null)
                {
                    openForm.Close(); // إغلاق الـ Form

                }
            }

            this.WindowState = FormWindowState.Normal; // تغيير حالة النافذة إلى الطبيعية قبل تعديل الحجم الأقصى
            this.MaximumSize = new Size(0, 0); // إزالة قيود الحجم الأقصى
            this.WindowState = FormWindowState.Maximized; // تغيير حالة النافذة إلى الطبيعية قبل تعديل الحجم الأقصى

            // اشعار تسجيل الدخول
            notifyIcon1.BalloonTipTitle = "تم تسجيل الدخول بنجاح";

            // تعيين النص المعروض في الإشعار
            notifyIcon1.BalloonTipText = "تم تسجيل الدخول بأسم " + MainClass.USER;

            // تعيين الأيقونة للإشعار (يمكنك تخصيص الأيقونة حسب الحاجة)
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;  // يمكن أن تكون Info أو Warning أو Error

            // إظهار الإشعار لمدة 3 ثواني
            notifyIcon1.ShowBalloonTip(3000);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
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

        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();

        public void AddControls(Form f)
        {
            // إخفاء أي فورم معروضة حاليًا
            foreach (var frm in openedForms.Values)
            {
                frm.Hide();
            }

            if (openedForms.ContainsKey(f.Name))
            {
                var existingForm = openedForms[f.Name];

                if (existingForm.IsDisposed)
                {
                    existingForm = CreateNewFormInstance(f.Name);
                    openedForms[f.Name] = existingForm;
                    PrepareForm(existingForm);
                }

                existingForm.Show();
                existingForm.BringToFront();

                // تنفيذ الحدث لو الفورم تدعم IRefreshableForm
                if (existingForm is IRefreshableForm refreshable)
                {
                    refreshable.OnFormShownAgain();
                }
            }
            else
            {
                PrepareForm(f);
                openedForms.Add(f.Name, f);
                f.Show();
            }
        }

        private void PrepareForm(Form frm)
        {
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            controlPanel.Controls.Add(frm);
        }

        public interface IRefreshableForm
        {
            void OnFormShownAgain(); // ← لازم تكون مكتوبة بنفس الاسم
        }


        private Form CreateNewFormInstance(string formName)
        {
            // الحصول على مجمع البرنامج الحالي حيث يتم تعريف النموذج
            Assembly assembly = Assembly.GetExecutingAssembly();

            // محاولة إنشاء مثيل للنموذج باستخدام اسمه
            // يُفترض أن اسم النموذج يتطابق مع الاسم الكامل للصنف بما في ذلك مساحة الاسم
            Type formType = assembly.GetType(formName);

            if (formType == null)
            {
                throw new ArgumentException($"No form found with the name {formName}.");
            }

            object formInstance = Activator.CreateInstance(formType);
            if (formInstance == null || !(formInstance is Form))
            {
                throw new ArgumentException($"The type {formName} is not a Form.");
            }

            return (Form)formInstance;
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            btnHome.Checked = true;
            btnPOS.Checked = false;
            btnMaintanance.Checked = false;
            btnStore.Checked = false;
            btnBills.Checked = false;
            btnpurchases.Checked = false;
            btnSettings.Checked = false;
            btnShortcomings.Checked = false;
            btnReturns.Checked = false;
            btnBills.Checked = false;
            notificationP.FillColor = backgroundPrmary;

            AddControls(new frmHome());
        }
        private bool IsFormOpen(Type formType)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.GetType() == formType)
                {
                    return true; // الـ Form مفتوح بالفعل
                }
            }
            return false; // الـ Form ليس مفتوحًا
        }
        private Form GetOpenForm(Type formType)
        {
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.GetType() == formType)
                {
                    return openForm; // إرجاع الـ Form المفتوح
                }
            }
            return null; // لا يوجد Form مفتوح من هذا النوع
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            if (!MainClass.OpenStore)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            btnHome.Checked = false;
            btnPOS.Checked = false;
            btnMaintanance.Checked = false;
            btnStore.Checked = true;
            btnBills.Checked = false;
            btnpurchases.Checked = false;
            btnSettings.Checked = false;
            btnShortcomings.Checked = false;
            btnReturns.Checked = false;
            btnBills.Checked = false;
            btnReturns.Checked = false;
            btnBills.Checked = false;
            notificationP.FillColor = backgroundPrmary;


            AddControls(new frmProductView());
        }


        private void btnPOS_Click(object sender, EventArgs e)
        {
            btnHome.Checked = false;
            btnPOS.Checked = true;
            btnMaintanance.Checked = false;
            btnStore.Checked = false;
            btnBills.Checked = false;
            btnpurchases.Checked = false;
            btnSettings.Checked = false;
            btnShortcomings.Checked = false;
            btnReturns.Checked = false;
            btnBills.Checked = false;
            notificationP.FillColor = backgroundPrmary;

            AddControls(new frmPOS());

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = this;
            frmAppSetting frmSettings = new frmAppSetting(this);
            frmSettings.ShowDialog(this);
            this.Focus();
            frmBlackout.Close();

        }

        private void btnpurchases_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowPurchases)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            btnHome.Checked = false;
            btnPOS.Checked = false;
            btnMaintanance.Checked = false;
            btnStore.Checked = false;
            btnBills.Checked = false;
            btnShortcomings.Checked = false;
            btnpurchases.Checked = true;
            btnSettings.Checked = false;
            btnReturns.Checked = false;
            btnBills.Checked = false;
            notificationP.FillColor = backgroundPrmary;

            AddControls(new frmpurchaseView());
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {

            message.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            message.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

            message.Parent = (Form)this.TopLevelControl;

            if (message.Show(" هل تريد تسجيل الخروج ") == DialogResult.Yes)
            {
                this.Hide();
                frmLogin frmLogin = new frmLogin();
                frmLogin.ShowDialog();
            }
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowShortages)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            btnHome.Checked = false;
            btnPOS.Checked = false;
            btnMaintanance.Checked = false;
            btnStore.Checked = false;
            btnBills.Checked = false;
            btnShortcomings.Checked = true;
            btnpurchases.Checked = false;
            btnSettings.Checked = false;
            btnReturns.Checked = false;
            btnBills.Checked = false;
            notificationP.FillColor = checkedFillColor;

            //controlPanel.Controls.Clear();

            frmShortcomings f = new frmShortcomings();
            //f.Dock = DockStyle.Fill;
            //f.TopLevel = false;
            //controlPanel.Controls.Add(f);
            //f.Show();
            AddControls(f);

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            btnHome.Checked = false;
            btnPOS.Checked = false;
            btnMaintanance.Checked = true;
            btnStore.Checked = false;
            btnBills.Checked = false;
            btnpurchases.Checked = false;
            btnSettings.Checked = false;
            btnShortcomings.Checked = false;
            btnReturns.Checked = false;
            btnBills.Checked = false;
            notificationP.FillColor = backgroundPrmary;

            AddControls(new frmMaintenanceView());
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

            if (MainClass.FinancePage)
            {
                btnHome.Checked = false;
                btnPOS.Checked = false;
                btnMaintanance.Checked = false;
                btnStore.Checked = false;
                btnFinance.Checked = true;
                btnShortcomings.Checked = false;
                btnpurchases.Checked = false;
                btnSettings.Checked = false;
                btnReturns.Checked = false;
                btnBills.Checked = false;
                notificationP.FillColor = backgroundPrmary;
                frmFinancialTransactions frm = new frmFinancialTransactions();
                frm.partyType = "عميل";
                AddControls(frm);

            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        private int i = 0;

        private bool isTimerBusy = false; // علشان نمنع الـ Overlap

        private async void timer1_Tick(object sender, EventArgs e)
        {
            //if (isTimerBusy) return; // لو فيه عملية شغالة، تجاهل tick جديد
            //isTimerBusy = true;

            //try
            //{
            //    // تحريك النص
            //    xPosition += 5;
            //    if (xPosition > this.bottomPanel.Width)
            //    {
            //        xPosition = -100;
            //    }
            //    this.bottomPanel.Invalidate();

            //    // الاستعلام الأول
            //    string qry1 = @"
            //            SELECT COUNT(*) 
            //            FROM products p
            //            INNER JOIN category c ON p.categoryID = c.catID
            //            INNER JOIN totalStor ts ON ts.pID = p.pID
            //            WHERE ts.qty < p.requestP";

            //    using (SqlConnection con = new SqlConnection(MainClass.con.ConnectionString))
            //    {
            //        await con.OpenAsync();

            //        using (SqlCommand cmd1 = new SqlCommand(qry1, con))
            //        {
            //            int count = Convert.ToInt32(await cmd1.ExecuteScalarAsync());
            //            notificationP.Text = count.ToString();
            //            notificationP.Visible = count != 0;
            //        }
            //    }


            //    // الاستعلام الثاني
            //    string qry2 = @"UPDATE shifts
            //            SET Amount = (
            //                SELECT Amount 
            //                FROM shifts 
            //                WHERE ID = (
            //                    SELECT MAX(ID) 
            //                    FROM shifts 
            //                    WHERE ID < (
            //                        SELECT MAX(ID) FROM shifts
            //                    )
            //                )
            //            )
            //            WHERE ID = (SELECT MAX(ID) FROM shifts)";

            //    Hashtable ht = new Hashtable();
            //    await MainClass.SQLAsync(qry2, ht);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    isTimerBusy = false; // السماح للـ tick التالي
            //}
        }


        private int xPosition = 0; // موقع البداية للنص

        private void panel14_Paint(object sender, PaintEventArgs e)
        {
            string text = "Free Soft_Team";
            using (Font font = new Font("Arial", 14))
            {
                SizeF textSize = e.Graphics.MeasureString(text, font); // قياس حجم النص
                                                                       // حساب الإحداثي العمودي ليكون النص في الوسط
                float yPosition = (this.bottomPanel.Height - textSize.Height) / 2;
                e.Graphics.Clear(bottomPanel.BackColor); // مسح الخلفية لتجنب التداخلات البصرية
                e.Graphics.DrawString(text, font, Brushes.Black, xPosition, yPosition); // رسم النص في الموقع الجديد
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

            // تعيين الـ Parent للنموذج الرئيسي
            guna2MessageDialog1.Parent = (Form)this.TopLevelControl;

            if (guna2MessageDialog1.Show(" هل تريد اغلاق البرنامج ") == DialogResult.Yes)
            {
                Application.Exit(); // إغلاق التطبيق بشكل نهائي

            }

        }

        private void MyForm_Resize(object sender, EventArgs e)
        {
            // تحقق إذا كانت النافذة ليست مُعظمة
            if (this.WindowState != FormWindowState.Maximized)
            {
                // ضبط النافذة لتظهر في منتصف الشاشة
                this.Location = new Point(
                    (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            }
        }
        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel7_Leave(object sender, EventArgs e)
        {
            btnShortcomings.Checked = true;
        }


        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            string linkUrl = "https://www.facebook.com/freesoft.egy";

            Process.Start(new ProcessStartInfo
            {
                FileName = linkUrl,
                UseShellExecute = true
            });
        }

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {
            string phoneNumber = "201010442330";

            string message = "مرحبا بك في الدعم الفني لشركة Free Soft .";

            string whatsappLink = "https://api.whatsapp.com/send?phone=" + phoneNumber + "&text=" + Uri.EscapeDataString(message);

            Process.Start(new ProcessStartInfo
            {
                FileName = whatsappLink,
                UseShellExecute = true
            });
        }

        // Form move
        bool isDragging = false;
        bool isMaximizedByDrag = false;
        Point dragOffset;
        Size normalSize;
        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragOffset = e.Location;

                if (this.WindowState == FormWindowState.Maximized)
                {
                    // نحفظ الحجم الحالي ونرجع الفورم لحجمها العادي
                    normalSize = this.RestoreBounds.Size;
                    this.WindowState = FormWindowState.Normal;

                    // تقليل الارتفاع وزيادة العرض
                    int newWidth = (int)(normalSize.Width * 1.3);   // زود العرض بنسبة 20%
                    int newHeight = (int)(normalSize.Height * 0.8); // قلل الارتفاع بنسبة 20%
                    this.Size = new Size(newWidth, newHeight);

                    // نخلي الفورم تحت الماوس على قد الإحداثيات الجديدة
                    Point cursorPos = Cursor.Position;
                    this.Location = new Point(cursorPos.X - this.Width / 4, cursorPos.Y - userImgPanel.Height / 2);
                }

                // تأثير التصغير
                this.Opacity = 0.9;
            }
        }

        private void panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentScreenPos = Cursor.Position;
                this.Location = new Point(currentScreenPos.X - dragOffset.X, currentScreenPos.Y - dragOffset.Y);

                // نحدد الشاشة اللي الماوس فيها حاليًا
                Screen currentScreen = Screen.FromPoint(Cursor.Position);

                // نتحقق إذا كان الماوس عند أعلى الشاشة الحالية
                if (currentScreenPos.Y <= currentScreen.Bounds.Top + 1 && !isMaximizedByDrag && this.WindowState != FormWindowState.Maximized)
                {
                    isMaximizedByDrag = true;
                    isDragging = false;
                    this.WindowState = FormWindowState.Maximized;
                    this.Opacity = 1;
                }
            }
        }

        private void panel5_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isMaximizedByDrag = false;
            this.Opacity = 1; // ترجع الشفافية لوضعها الطبيعي
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            test test1 = new test();
            test1.ShowDialog();
        }

        frmNotificationCenter notif = null;
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms.Cast<Form>().ToList())
            {
                if (frm.Name != "frmMian2")
                    frm.Close();
            }

        }

        private void notifyBell_Click(object sender, EventArgs e)
        {
            if (notif == null || notif.IsDisposed)
            {
                // إذا كانت null أو تم التخلص منها، أنشئ نموذج جديد
                notif = new frmNotificationCenter(this);
                notif.Show();
            }
            else
            {
                // إذا كان النموذج ظاهرًا بالفعل، أخفِه
                if (notif.Visible)
                {
                    notif.Hide();
                }
                else
                {
                    // إذا كان النموذج مخفيًا، أظهره مرة أخرى
                    notif.Show();
                    notif.BringToFront(); // لجعل النموذج في المقدمة
                }
            }
        }

        private void LightMode()
        {
            backgroundPrmary = Color.FromArgb(243, 243, 243);
            backgroundseconder = Color.FromArgb(230, 230, 230);
            textColor = Color.FromArgb(51, 51, 51);
            //checkedFillColor = Color.FromArgb(136, 214, 218);
            checkedFillColor = Color.FromArgb(1, 95, 95);

            checkedForColor = Color.White;

            btnHome.Image = Properties.Resources.home_light;
            btnHome.CheckedState.Image = Properties.Resources.home_white;

            btnPOS.Image = Properties.Resources.cashier_light;
            btnPOS.CheckedState.Image = Properties.Resources.cashier_white;

            btnMaintanance.Image = Properties.Resources.home_repair_light;
            btnMaintanance.CheckedState.Image = Properties.Resources.home_repair_white;

            btnShortcomings.Image = Properties.Resources.shortcaming_light;
            btnShortcomings.CheckedState.Image = Properties.Resources.shortcaming_white;

            btnpurchases.Image = Properties.Resources.purchase_light;
            btnpurchases.CheckedState.Image = Properties.Resources.purchase_white;


            btnSettings.Image = Properties.Resources.setting_light1;
            btnSettings.CheckedState.Image = Properties.Resources.setting_white;

            btnStore.Image = Properties.Resources.store;
            btnStore.CheckedState.Image = Properties.Resources.store__2_;
        }
        private void DarkMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.White;

            btnHome.Image = Properties.Resources.home_dark;
            btnHome.CheckedState.Image = Properties.Resources.home_white;

            btnPOS.Image = Properties.Resources.cashier_dark;
            btnPOS.CheckedState.Image = Properties.Resources.cashier_white;

            btnMaintanance.Image = Properties.Resources.home_repair_dark;
            btnMaintanance.CheckedState.Image = Properties.Resources.home_repair_white;

            btnShortcomings.Image = Properties.Resources.shortcaming_dark;
            btnShortcomings.CheckedState.Image = Properties.Resources.shortcaming_white;

            btnpurchases.Image = Properties.Resources.purchase_dark;
            btnpurchases.CheckedState.Image = Properties.Resources.purchase_white;

            btnSettings.Image = Properties.Resources.setting_dark1;
            btnSettings.CheckedState.Image = Properties.Resources.setting_white;

            btnStore.Image = Properties.Resources.store__1_;
            btnStore.CheckedState.Image = Properties.Resources.store__2_;


        }
        private void ThemeMode()
        {
            this.BackColor = backgroundPrmary;

            lblUserName.ForeColor = textColor;
            SLine.FillColor = Color.Gray;
            SLine.BackColor = backgroundPrmary;

            //->Panels
            controlPanel.BackColor = backgroundPrmary;
            navigationPanel.BackColor = backgroundPrmary;
            suportrPanel.BackColor = backgroundPrmary;
            userImgPanel.BackColor = backgroundPrmary;

            btnHome.FillColor = backgroundPrmary;
            btnHome.ForeColor = textColor;
            btnHome.CheckedState.FillColor = checkedFillColor;
            btnHome.CheckedState.ForeColor = checkedForColor;

            btnPOS.FillColor = backgroundPrmary;
            btnPOS.ForeColor = textColor;
            btnPOS.CheckedState.FillColor = checkedFillColor;
            btnPOS.CheckedState.ForeColor = checkedForColor;

            btnMaintanance.FillColor = backgroundPrmary;
            btnMaintanance.ForeColor = textColor;
            btnMaintanance.CheckedState.FillColor = checkedFillColor;
            btnMaintanance.CheckedState.ForeColor = checkedForColor;

            btnStore.FillColor = backgroundPrmary;
            btnStore.ForeColor = textColor;
            btnStore.CheckedState.FillColor = checkedFillColor;
            btnStore.CheckedState.ForeColor = checkedForColor;

            btnShortcomings.FillColor = backgroundPrmary;
            btnShortcomings.ForeColor = textColor;
            btnShortcomings.CheckedState.FillColor = checkedFillColor;
            btnShortcomings.CheckedState.ForeColor = checkedForColor;

            btnpurchases.FillColor = backgroundPrmary;
            btnpurchases.ForeColor = textColor;
            btnpurchases.CheckedState.FillColor = checkedFillColor;
            btnpurchases.CheckedState.ForeColor = checkedForColor;

            btnBills.FillColor = backgroundPrmary;
            btnBills.ForeColor = textColor;
            btnBills.CheckedState.FillColor = checkedFillColor;
            btnBills.CheckedState.ForeColor = checkedForColor;

            btnSettings.FillColor = backgroundPrmary;
            btnSettings.ForeColor = textColor;
            btnSettings.CheckedState.FillColor = checkedFillColor;
            btnSettings.CheckedState.ForeColor = checkedForColor;

            notificationP.FillColor = backgroundPrmary;
            notificationP.ForeColor = textColor;
            notificationP.BorderColor = textColor;
        }

        private void cmRefresh_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms.Cast<Form>().ToList())
            {
                if (frm.Name != "frmMian2")
                    frm.Close();
            }

        }
        public void themRefresh()
        {
            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();

            if (openedForms.ContainsKey("frmPOS"))
            {
                var form = openedForms["frmPOS"] as frmPOS;
                form?.ReloadData();
            }
            if (openedForms.ContainsKey("frmProductView"))
            {
                var form = openedForms["frmProductView"] as frmProductView;
                form?.ReloadData();
            }

            if (openedForms.ContainsKey("frmShortcomings"))
            {
                var form = openedForms["frmShortcomings"] as frmShortcomings;
                form?.ReloadData();
            }

        }

        private async void btnBackup_Click(object sender, EventArgs e)
        {
            frmShowBackup frm = new frmShowBackup();
            frm.ShowDialog();

        }

        bool backupDone = false; // فلاغ عشان نمنع التكرار

        private void frmMian2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // لو النسخة اتعملت خلاص → سيب البرنامج يقفل عادي
            if (backupDone)
            {
                return;
            }

            // امنع الإغلاق المباشر
            e.Cancel = true;

            // افتح فورم النسخ الاحتياطي
            frmShowBackup frm = new frmShowBackup();

            // لما الفورم يقفل → اقفل البرنامج كله
            frm.FormClosed += (s, args) =>
            {
                backupDone = true; // النسخة خلصت ✅
                Application.Exit();
            };

            frm.Show();

            // اخفي الـ MainForm عشان مايظهرش ورا
            this.Hide();
        }

        private void btnBills_Click(object sender, EventArgs e)
        {
            btnReturns.Checked = false;
            btnBills.Checked = true;
            btnHome.Checked = false;
            btnPOS.Checked = false;
            btnMaintanance.Checked = false;
            btnStore.Checked = false;
            btnpurchases.Checked = false;
            btnSettings.Checked = false;
            btnShortcomings.Checked = false;
            btnFinance.Checked = false;

            AddControls(new frmShowBills());

        }

        private void btnReturns_Click(object sender, EventArgs e)
        {
            btnReturns.Checked = true;
            btnBills.Checked = false;
            btnHome.Checked = false;
            btnPOS.Checked = false;
            btnMaintanance.Checked = false;
            btnStore.Checked = false;
            btnpurchases.Checked = false;
            btnSettings.Checked = false;
            btnShortcomings.Checked = false;
            btnFinance.Checked = false;

            AddControls(new frmShowReturns());


        }

        private void btnAddbill_Click(object sender, EventArgs e)
        {


            AddControls(new frmProductAdd2());
        }

        private void btnParties_Click(object sender, EventArgs e)
        {
            AddControls(new frmPartiesView(this));

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            bottomPanel.Visible = false;
            AddControls(new frmPartiesView(this));

        }
        public void openBalanceFollow(int pid, string name, bool isSupliser)
        {
            frmPersonalReport frm = new frmPersonalReport();
            frm.partiesID = pid;
            frm.fromParties = true;
            frm.txtName.Text = name;
            if (!isSupliser)
                frm.cbChooseParyties.SelectedIndex = 0;
            else
                frm.cbChooseParyties.SelectedIndex = 1;
            frm.dgvPanel.Dock = DockStyle.Fill;
            AddControls(frm);
            bottomPanel.Visible = true;

        }
    }
}
