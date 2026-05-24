using System;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Classes
{
    public class SmoothPanel_BottomCorner : Panel
    {
        public float BorderSize { get; set; } = 1f;
        public Color BorderColor { get; set; } = Color.Black; // لون افتراضي وقت التصميم

        public SmoothPanel_BottomCorner()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e); // 🟢 يرسم الخلفية والأدوات جوة Panel
            base.OnPaint(e);

            // 🟢 ما تنفذش كود MainClass وقت التصميم
            if (!DesignMode)
            {
                MainClass.themeMode();
                BorderColor = MainClass.CheckedFillColor;
            }

            using (var pen = new Pen(BorderColor, BorderSize))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Rectangle rect = ClientRectangle;

                // Left border
                e.Graphics.DrawLine(pen,
                    rect.Left,
                    rect.Top,
                    rect.Left,
                    rect.Bottom);

                // Right border
                e.Graphics.DrawLine(pen,
                    rect.Right - 1,
                    rect.Top,
                    rect.Right - 1,
                    rect.Bottom);

                // Bottom border
                e.Graphics.DrawLine(pen,
                    rect.Left,
                    rect.Bottom - 1,
                    rect.Right,
                    rect.Bottom - 1);
            }
        }
    }
}
