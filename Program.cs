using pos.View;
using System.Globalization;
using System;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;
using pos.SystemApp;
using pos.GeneralForms;
using pos.Model;
using pos.GeneralForms.MainForm;
using System.Drawing; // 🟧 Added for Tray icon support

namespace pos
{
    internal static class Program
    {
        // 🟩 اسم النسخة الحالية ← غيّرها حسب النسخة
        //public static string AppInstance = "Smart Cashier"; // ← النسخة الأولى
        //static Mutex mutex = new Mutex(true, "{A1B2C3D4-1111-2222-3333-444444444444}");

        // لو دي النسخة الثانية، غير القيمتين دول 👇
        //public static string AppInstance = "Smart Cashier Two";
        //static Mutex mutex = new Mutex(true, "{A1B2C3D4-5555-6666-7777-888888888888}");


        //public static string AppInstance = "Smart Cashier"; // ← النسخة الأولى
        //static Mutex mutex = new Mutex(true, "{A1B2C3D4-1111-2222-3333-444455544444}");

        public static string AppInstance = "Smart Cashier Main"; // ← النسخة الأولى
        static Mutex mutex = new Mutex(true, "{A1B2C3D4-1111-2222-3333-444455544466}");

        [STAThread]
        static void Main()
        {
            // 🔑 تفعيل ترخيص Syncfusion
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cWWNCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXpecXRSRGdfVEB3X0pWYUA=");

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(GlobalThreadExceptionHandler);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalDomainExceptionHandler);

            ApplicationConfiguration.Initialize();

            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    frmGraphicalinterFace mainForm = new frmGraphicalinterFace(false);
                    mainForm.Text = $"{AppInstance} - Main Window";

                    // 🟧 Added for Tray support
                    NotifyIcon trayIcon = new NotifyIcon();
                    trayIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                    trayIcon.Text = AppInstance;
                    trayIcon.Visible = true;

                    ContextMenuStrip trayMenu = new ContextMenuStrip();
                    trayMenu.Items.Add("عرض النافذة", null, (s, e) =>
                    {
                        mainForm.Show();
                        mainForm.WindowState = FormWindowState.Normal;
                        mainForm.BringToFront();
                    });
                    trayMenu.Items.Add("خروج", null, (s, e) =>
                    {
                        trayIcon.Visible = false;
                        Application.Exit();
                    });
                    trayIcon.ContextMenuStrip = trayMenu;

                    // عند إغلاق الفورم ⇒ لا يغلق البرنامج فعليًا
                    mainForm.FormClosing += (s, e) =>
                    {
                        if (e.CloseReason == CloseReason.UserClosing)
                        {
                            e.Cancel = true;
                            mainForm.Hide();
                        }
                    };

                    // عند الضغط مرتين على الأيقونة ⇒ يظهر البرنامج
                    trayIcon.DoubleClick += (s, e) =>
                    {
                        mainForm.Show();
                        mainForm.WindowState = FormWindowState.Normal;
                        mainForm.BringToFront();
                    };
                    // 🟧 End Tray support

                    Application.Run(mainForm);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                MessageBox.Show($"{AppInstance} is already running.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        static void GlobalThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            ShowErrorMessage(e.Exception);
        }

        static void GlobalDomainExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                ShowErrorMessage(ex);
        }

        static void ShowErrorMessage(Exception ex)
        {
            MessageBox.Show(
                "حدث خطأ في البرنامج:\n" + ex.Message,
                "⚠️ خطأ غير متوقع",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}
