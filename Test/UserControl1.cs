using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Xpo.Logger;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace pos.Test
{
    public partial class UserControl1 : UserControl
    {
        //Fields
        private int bordarRadius = 8;
        private int borderSize = 1;
        private Color borderColor = Color.FromArgb(136, 214, 218);
        private Color borderColor2 = Color.FromArgb(243, 243, 243);

        public UserControl1()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private GraphicsPath GetRoundedPath(Rectangle rect, float radius, bool topLeft, bool topRight, bool bottomRight, bool bottomLeft)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();

            // Top Left corner
            if (topLeft)
                path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            else
                path.AddLine(rect.X, rect.Y, rect.X, rect.Y);

            // Top edge
            path.AddLine(topLeft ? rect.X + radius : rect.X, rect.Y,
                         topRight ? rect.Right - radius : rect.Right, rect.Y);

            // Top Right corner
            if (topRight)
                path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            else
                path.AddLine(rect.Right, rect.Y, rect.Right, rect.Y);

            // Right edge
            path.AddLine(rect.Right, topRight ? rect.Y + radius : rect.Y,
                         rect.Right, bottomRight ? rect.Bottom - radius : rect.Bottom);

            // Bottom Right corner
            if (bottomRight)
                path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            else
                path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);

            // Bottom edge
            path.AddLine(bottomRight ? rect.Right - radius : rect.Right, rect.Bottom,
                         bottomLeft ? rect.X + radius : rect.X, rect.Bottom);

            // Bottom Left corner
            if (bottomLeft)
                path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            else
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom);

            // Left edge
            path.AddLine(rect.X, bottomLeft ? rect.Bottom - radius : rect.Bottom,
                         rect.X, topLeft ? rect.Y + radius : rect.Y);

            path.CloseFigure();
            return path;
        }
        private void FormRegionAndBorder(UserControl form, float radius, Graphics graph, Color borderColor, float borderSize)
        {
            using (GraphicsPath roundPath = GetRoundedPath(form.ClientRectangle, radius, true, true, true, true))
            using (Pen penBorder = new Pen(borderColor, borderSize))
            using (Matrix transform = new Matrix())
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                form.Region = new Region(roundPath);
                if (borderSize >= 1)
                {
                    Rectangle rect = form.ClientRectangle;
                    float scaleX = 1.0F - ((borderSize + 1) / rect.Width);
                    float scaleY = 1.0F - ((borderSize + 1) / rect.Height);
                    transform.Scale(scaleX, scaleY);
                    transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                    graph.Transform = transform;
                    graph.DrawPath(penBorder, roundPath);
                }
            }
        }
      
        private void DrawPath(Rectangle rect, Graphics graph, Color color)
        {
            using (GraphicsPath roundPath = GetRoundedPath(rect, bordarRadius, true, true, true, true))
            using (Pen penBorder = new Pen(color, 3))
            {
                graph.DrawPath(penBorder, roundPath);
            }
        }
        private struct FormBoundsColors
        {
            public Color TopLeftColor;
            public Color TopRightColor;
            public Color BottomLeftColor;
            public Color BottomRightColor;
        }
        private FormBoundsColors GetFormBoundsColors()
        {
            var fbColor = new FormBoundsColors();
            using (var bmp = new Bitmap(1, 1))
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle rectBmp = new Rectangle(0, 0, 1, 1);
                //Top Left
                rectBmp.X = this.Bounds.X - 1;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopLeftColor = borderColor2;
                //Top Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopRightColor = borderColor2;
                //Bottom Left
                rectBmp.X = this.Bounds.X;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomLeftColor = borderColor2;
                //Bottom Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomRightColor = borderColor2;
            }
            return fbColor;
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //-> SMOOTH OUTER DORDER
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle recForm = this.ClientRectangle;
            int mWight = recForm.Width / 2;
            int mHight = recForm.Height / 2;
            var fbColors = GetFormBoundsColors();

            //Top Left
            DrawPath(recForm, e.Graphics, fbColors.TopLeftColor);

            //Top Right
            Rectangle recTopRight = new Rectangle(mWight, recForm.Y, mWight, mHight);
            DrawPath(recTopRight, e.Graphics, fbColors.TopRightColor);

            //Bottom Left
            Rectangle recBottomLeft = new Rectangle(recForm.X, recForm.X + mHight, mWight, mHight);
            DrawPath(recBottomLeft, e.Graphics, fbColors.BottomLeftColor);

            //Bottom Right
            Rectangle recBottomRight = new Rectangle(mWight, recForm.Y + mHight, mWight, mHight);
            DrawPath(recBottomRight, e.Graphics, fbColors.BottomRightColor);
        }
        public void ApplyRoundedCornersWithBorder(Control control, float radius,
                                 bool topLeftCorner = true, bool topRightCorner = true,
                                 bool bottomRightCorner = true, bool bottomLeftCorner = true,
                                 bool borderTop = true, bool borderRight = true,
                                 bool borderBottom = true, bool borderLeft = true,
                                 Color? borderColor = null, float borderWidth = 1f)
        {
            Rectangle rect = control.ClientRectangle;
            float curveSize = radius * 2F;
            Color actualBorderColor = borderColor ?? Color.Black;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();

            // Top Left
            if (topLeftCorner)
                path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            else
                path.AddLine(rect.X, rect.Y, rect.X, rect.Y);

            // Top edge
            path.AddLine(topLeftCorner ? rect.X + radius : rect.X, rect.Y,
                         topRightCorner ? rect.Right - radius : rect.Right, rect.Y);

            // Top Right
            if (topRightCorner)
                path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            else
                path.AddLine(rect.Right, rect.Y, rect.Right, rect.Y);

            // Right edge
            path.AddLine(rect.Right, topRightCorner ? rect.Y + radius : rect.Y,
                         rect.Right, bottomRightCorner ? rect.Bottom - radius : rect.Bottom);

            // Bottom Right
            if (bottomRightCorner)
                path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            else
                path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);

            // Bottom edge
            path.AddLine(bottomRightCorner ? rect.Right - radius : rect.Right, rect.Bottom,
                         bottomLeftCorner ? rect.X + radius : rect.X, rect.Bottom);

            // Bottom Left
            if (bottomLeftCorner)
                path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            else
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom);

            // Left edge
            path.AddLine(rect.X, bottomLeftCorner ? rect.Bottom - radius : rect.Bottom,
                         rect.X, topLeftCorner ? rect.Y + radius : rect.Y);

            path.CloseFigure();

            control.Region = new Region(path);

            // Border
            control.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (Pen pen = new Pen(actualBorderColor, borderWidth))
                {
                    // نرسم المسار نفسه للحدود المستديرة
                    // نرسم فقط الزوايا المحددة
                    if (topLeftCorner)
                        e.Graphics.DrawArc(pen, rect.X, rect.Y, curveSize, curveSize, 180, 90);

                    if (topRightCorner)
                        e.Graphics.DrawArc(pen, rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);

                    if (bottomRightCorner)
                        e.Graphics.DrawArc(pen, rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);

                    if (bottomLeftCorner)
                        e.Graphics.DrawArc(pen, rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);

                    // نرسم الجوانب المستقيمة لو محددة
                    if (borderTop)
                        e.Graphics.DrawLine(pen,
                            new Point((int)(topLeftCorner ? rect.X + radius : rect.X), rect.Y),
                            new Point((int)(topRightCorner ? rect.Right - radius : rect.Right), rect.Y));

                    if (borderRight)
                        e.Graphics.DrawLine(pen,
                            new Point(rect.Right, (int)(topRightCorner ? rect.Y + radius : rect.Y)),
                            new Point(rect.Right, (int)(bottomRightCorner ? rect.Bottom - radius : rect.Bottom)));

                    if (borderBottom)
                        e.Graphics.DrawLine(pen,
                            new Point((int)(bottomRightCorner ? rect.Right - radius : rect.Right), rect.Bottom),
                            new Point((int)(bottomLeftCorner ? rect.X + radius : rect.X), rect.Bottom));

                    if (borderLeft)
                        e.Graphics.DrawLine(pen,
                            new Point(rect.X, (int)(bottomLeftCorner ? rect.Bottom - radius : rect.Bottom)),
                            new Point(rect.X, (int)(topLeftCorner ? rect.Y + radius : rect.Y)));
                }
            };
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            FormRegionAndBorder(this, bordarRadius, e.Graphics, borderColor, borderSize);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
