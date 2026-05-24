using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pos.View
{
    public partial class frmBlackout : XtraForm
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        bool fullScreen = false;
        public frmBlackout(Form ownerForm, bool full = false)
        {
            InitializeComponent();

            // إزالة الإطار
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;
            this.TopMost = false;

            // تحديد الشاشة اللي الفورم الأصلية موجودة فيها
            Screen screen;

            if (ownerForm != null)
            {
                screen = Screen.FromControl(ownerForm);
            }
            else
            {
                screen = Screen.PrimaryScreen; // ← fallback آمن
            }

            // خلي الفورم بنفس حجم الشاشة (مش بس الفورم)
            this.Bounds = screen.Bounds;

            // تعيين لون أسود وشفافية 50%
            this.BackColor = Color.Black;
            this.Opacity = 0.5;

            // تعيين المالك لضمان الظهور فوق الفورم الأصلية
            this.Owner = ownerForm;

            // منع الظهور في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

            // جعلها تغطي الفورم الأصلية بالضبط على نفس الشاشة
            AlignToOwner(ownerForm);
            fullScreen = full;
        }

        private void AlignToOwner(Form ownerForm)
        {
            Screen screen = ownerForm != null
                ? Screen.FromControl(ownerForm)
                : Screen.PrimaryScreen;

            int relativeX = 0;
            int relativeY = 0;
            int width = this.Width;
            int height = this.Height;

            if (ownerForm != null)
            {
                relativeX = ownerForm.Left - screen.Bounds.Left;
                relativeY = ownerForm.Top - screen.Bounds.Top;

                width = ownerForm.Width;
                height = ownerForm.Height;
            }

            this.Location = new Point(screen.Bounds.Left + relativeX,
                                      screen.Bounds.Top + relativeY);

            if (fullScreen)
                this.Size = new Size(width, height + 25);
            else
                this.Size = new Size(width, height);
        }


        private void frmBlackout_Load(object sender, EventArgs e)
        {
        }
    }
}
