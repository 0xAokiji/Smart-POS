using System;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Classes
{
    public class SmoothPanelTopConrner : Panel
    {
        public float BorderSize { get; set; } = 1f;
        public Color BorderColor { get; set; } = Color.Black; // لون افتراضي يظهر في المصمم

        public SmoothPanelTopConrner()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e); // 🟢 خلي الأدوات اللي جوا panel تبان
            base.OnPaint(e);

            if (!DesignMode)
            {
                MainClass.themeMode();
                BorderColor = MainClass.CheckedFillColor;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var pen = new Pen(BorderColor, BorderSize))
            {
                Rectangle rect = ClientRectangle;
                rect.Width -= (int)BorderSize;
                rect.Height -= (int)BorderSize;

                // 🟢 هنا بيرسم مستطيل كامل (حدود الأربعة جوانب)
                e.Graphics.DrawRectangle(pen, rect);
            }
        }
    }
}
