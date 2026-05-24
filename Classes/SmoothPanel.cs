using System;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Classes
{
    public class SmoothPanel : Panel
    {
        public float BorderSize { get; set; } = 1f;
        public Color BorderColor { get; set; } = Color.Black; // لون افتراضي

        public SmoothPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED لتقليل الفليكر
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e); // 🟢 مهم عشان يظهر في المصمم
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // لو البرنامج شغال عادي (مش في الـ Designer) غير اللون
            if (!DesignMode)
            {
                BorderColor = MainClass.CheckedFillColor;
            }

            using (Pen pen = new Pen(BorderColor, BorderSize))
            {
                int x1 = 0;
                int x2 = Width - (int)BorderSize;

                // 🟢 رسم الحدود الجانبية
                e.Graphics.DrawLine(pen, x1, 0, x1, Height); // يسار
                e.Graphics.DrawLine(pen, x2, 0, x2, Height); // يمين
            }
        }
    }
}
