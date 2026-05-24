using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Model.POS
{
    public partial class frmShowProductImg : Form
    {
        private float zoomFactor = 1.0f; // 🔥 عامل التكبير
        private PictureBox productImg;
        private Panel container;

        // 🔥 متغيرات السحب (Pan)
        private Point mouseDownLocation;
        private bool isDragging = false;

        public frmShowProductImg(Image img)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            // 📌 إعداد الفورم
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;

            // 📌 Panel مع Scroll
            container = new Panel();
            container.Dock = DockStyle.Fill;
            container.BackColor = Color.Black;
            container.AutoScroll = true;
            this.Controls.Add(container);

            // 📌 PictureBox
            productImg = new PictureBox();
            productImg.Image = img;
            productImg.SizeMode = PictureBoxSizeMode.Zoom; // 🔥 Zoom Mode
            productImg.Width = img.Width;
            productImg.Height = img.Height;
            productImg.Anchor = AnchorStyles.None;
            productImg.Cursor = Cursors.Hand;
            container.Controls.Add(productImg);

            CenterImage();

            // 🔥 دعم التكبير بالسكرول
            this.MouseWheel += FrmShowProductImg_MouseWheel;

            // 🔥 دعم السحب بالماوس
            productImg.MouseDown += ProductImg_MouseDown;
            productImg.MouseMove += ProductImg_MouseMove;
            productImg.MouseUp += ProductImg_MouseUp;

            this.Resize += (s, e) => CenterImage();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;       // WS_EX_TOOLWINDOW - لجعل الفورم لا يظهر في شريط المهام
                return cp;
            }
        }
        private void FrmShowProductImg_MouseWheel(object sender, MouseEventArgs e)
        {
            if (productImg.Image == null) return;

            // 📌 نسبة الماوس بالنسبة للصورة قبل التكبير
            float mouseXRatio = (float)(e.X - productImg.Left) / productImg.Width;
            float mouseYRatio = (float)(e.Y - productImg.Top) / productImg.Height;

            // 📌 تكبير/تصغير
            if (e.Delta > 0)
                zoomFactor += 0.1f;
            else if (e.Delta < 0 && zoomFactor > 0.2f)
                zoomFactor -= 0.1f;

            // 📌 حجم جديد للصورة
            int newWidth = (int)(productImg.Image.Width * zoomFactor);
            int newHeight = (int)(productImg.Image.Height * zoomFactor);

            // 📌 حساب مكان الصورة الجديد بحيث يحافظ على النقطة تحت الماوس
            int newLeft = e.X - (int)(mouseXRatio * newWidth);
            int newTop = e.Y - (int)(mouseYRatio * newHeight);

            productImg.Width = newWidth;
            productImg.Height = newHeight;
            productImg.Left = newLeft;
            productImg.Top = newTop;
        }


        private void CenterImage()
        {
            if (productImg.Image != null)
            {
                int x = Math.Max((container.ClientSize.Width - productImg.Width) / 2, 0);
                int y = Math.Max((container.ClientSize.Height - productImg.Height) / 2, 0);
                productImg.Location = new Point(x, y);
            }
        }

        // 🔥 السحب (Pan)
        private void ProductImg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                mouseDownLocation = e.Location;
                productImg.Cursor = Cursors.SizeAll;
            }
        }

        private void ProductImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                productImg.Left += e.X - mouseDownLocation.X;
                productImg.Top += e.Y - mouseDownLocation.Y;
            }
        }

        private void ProductImg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                productImg.Cursor = Cursors.Hand;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveImag_Click(object sender, EventArgs e)
        {
            if (productImg.Image == null)
            {
                MessageBox.Show("لا توجد صورة لحفظها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "اختر مكان الحفظ";
                saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveDialog.FileName = "product_image";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 🔥 حفظ الصورة حسب الامتداد
                        var format = System.Drawing.Imaging.ImageFormat.Png;
                        string ext = System.IO.Path.GetExtension(saveDialog.FileName).ToLower();
                        if (ext == ".jpg") format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        else if (ext == ".bmp") format = System.Drawing.Imaging.ImageFormat.Bmp;

                        productImg.Image.Save(saveDialog.FileName, format);
                        MessageBox.Show("تم حفظ الصورة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("حدث خطأ أثناء حفظ الصورة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
