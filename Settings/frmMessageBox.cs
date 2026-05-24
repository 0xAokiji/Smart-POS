using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Settings
{
    public partial class frmMessageBox : Form
    {
        //-> Dark Mode
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color checkedFillColor;
        private Color checkedForColor;

        private bool DarkState = true;

        //Fields
        private int bordarRadius = 15;
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(32, 32, 32);
        private Color borderColor2 = Color.FromArgb(243, 243, 243);

        public frmMessageBox(string titel, string message, string icon)
        {
            InitializeComponent();
            lblTitel.Text = titel;
            lblMessage.Text = message;
            labels_location();
            Icon(icon);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();


            this.Padding = new Padding(borderSize);

            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
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
        private void FormRegionAndBorder(Form form, float radius, Graphics graph, Color borderColor, float borderSize)
        {
            if (this.WindowState != FormWindowState.Minimized)
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
        }
        private void ControlRegionAndBorder(Control control, float radius, Graphics graph, Color borderColor)
        {
            using (GraphicsPath roundPath = GetRoundedPath(control.ClientRectangle, radius, true, true, true, true))
            using (Pen penBorder = new Pen(borderColor, 1))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                control.Region = new Region(roundPath);
                graph.DrawPath(penBorder, roundPath);
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
                fbColor.TopLeftColor = bmp.GetPixel(0, 0);
                //Top Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopRightColor = bmp.GetPixel(0, 0);
                //Bottom Left
                rectBmp.X = this.Bounds.X;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomLeftColor = bmp.GetPixel(0, 0);
                //Bottom Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomRightColor = bmp.GetPixel(0, 0);
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



        private void frmMessageBox_Paint(object sender, PaintEventArgs e)
        {
            FormRegionAndBorder(this, bordarRadius, e.Graphics, borderColor, borderSize);

        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
            ControlRegionAndBorder(mainPanel, bordarRadius - (borderSize / 2), e.Graphics, borderColor);

        }
        private void therdPanel_Paint(object sender, PaintEventArgs e)
        {
            ApplyRoundedCornersWithBorder(therdPanel, bordarRadius - (borderSize / 2), false, false, true, true, 
                                            false,true,true,true,borderColor,borderSize);
        }

        private void LightMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(243, 243, 243);
            backgroundseconder = Color.FromArgb(230, 230, 230);
            textColor = Color.FromArgb(51, 51, 51);
            checkedFillColor = Color.FromArgb(136, 214, 218);
            checkedForColor = Color.FromArgb(250, 250, 20);
        }
        private void DarkMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.FromArgb(2, 2, 2);
            borderColor = checkedFillColor;
            borderColor2 = backgroundPrmary;
        }
        private void ThemeMode()
        {
            this.BackColor = backgroundPrmary;

            //-> Panel
            seconandPanel.FillColor = backgroundseconder;
            mainPanel.BackColor = backgroundPrmary;
            therdPanel.BackColor = backgroundseconder;
            //-> Labels
            lblTitel.ForeColor = textColor;
            lblMessage.ForeColor = textColor;
            //-> Button
            btnOK.BackColor = backgroundseconder;
            btnOK.FillColor = checkedFillColor;
            btnOK.ForeColor = textColor;

            btnCancel.BackColor = backgroundseconder;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void labels_location()
        {
            lblTitel.Left = (mainPanel.Width - lblTitel.Width) - 10;

            lblMessage.AutoSize = false;
            lblMessage.MaximumSize = new Size(seconandPanel.Width, 0); // أقصى عرض = عرض البانل، الارتفاع غير محدود
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblMessage.AutoEllipsis = false;
            lblMessage.UseCompatibleTextRendering = true; // لتحسين الالتفاف
            AdjustLabelPosition();

        }
        private void AdjustLabelPosition()
        {
            lblMessage.Width = seconandPanel.Width;

            // احسب الحجم المطلوب بناءً على النص والعرض الحالي
            Size preferredSize = TextRenderer.MeasureText(lblMessage.Text, lblMessage.Font,
                new Size(lblMessage.Width, int.MaxValue), TextFormatFlags.WordBreak);

            lblMessage.Height = preferredSize.Height;

            // توسيط عموديًا بعد معرفة الارتفاع
            lblMessage.Left = 0; // لأن عرضه يساوي البانل
            lblMessage.Top = (seconandPanel.Height - lblMessage.Height) / 2;
        }
        private void Icon(string icon)
        {
            if(icon == "W")
                picIcon.Image = Properties.Resources.warning;
            else if (icon == "E")
                picIcon.Image = Properties.Resources.error;
            else
                picIcon.Image = Properties.Resources.info;


        }
    }
}
