using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using pos.Classes;
using Syncfusion.Windows.Forms.Tools;
using Microsoft.DotNet.DesignTools.Protocol.Values;
using pos.Model.POS;

namespace pos.UserControls
{
    public partial class ucProduct2 : UserControl
    {

        //-> Dark Mode
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color checkedFillColor;
        private Color borderColor;

        public double AvailableNew { get; set; }
        public double AvailableUsed { get; set; }

        public ucProduct2()
        {
            InitializeComponent();


            themRefresh();
        }

        public void themRefresh()
        {
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
        public event EventHandler onSelect = null;
        public event EventHandler onSelect2 = null;
        public event EventHandler onEdite = null;
        public event EventHandler onAbout = null;
        public event EventHandler showImg = null;

        public int id { get; set; }
        public string barCode { get; set; }
        public string barCodeUse { get; set; }

        //   public int uid { get; set; }
        public string pprice
        {
            get { return lblPrice.Text; }
            set { lblPrice.Text = value; }
        }

        public string pshortFall
        {
            get { return lblQtyUse.Text; }
            set { lblQtyUse.Text = value; }
        }

        public string pPlace
        {
            get { return lblPlace.Text; }
            set { lblPlace.Text = value; }
        }
       
        public string PCategory { get; set; }

        public string PName
        {
            get { return lblName.Text; }
            set
            {
                lblName.Text = value;
                CenterLabelName();
            }
        }


        private Image originalImage;

        public Image PImage
        {
            get { return proImage.Image; }
            set
            {
                originalImage = value;      // 🔹 خزّن الصورة الأصلية هنا
                SetProductImageAsync(value);
            }
        }

        public Image OriginalImage
        {
            get { return originalImage; }
        }

        public string pQty
        {
            get { return lblQty.Text; }
            set { lblQty.Text = value; }
        }

        public string pQtyUse
        {
            get { return lblQtyUse.Text; }
            set { lblQtyUse.Text = value; }
        }




        private void ucProduct_Load(object sender, EventArgs e)
        {
            string appBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configFilePath = Path.Combine(appBaseDirectory, "Settings.config");

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(Directory.GetCurrentDirectory(), "Settings.config") };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            if (settings["ucProdact_X"] != null && settings["ucProdact_Y"] != null)
            {
                int X = int.TryParse(settings["ucProdact_X"].Value, out X) ? X : 0; // القيمة الافتراضية 0 إذا فشل التحويل
                int Y = int.TryParse(settings["ucProdact_Y"].Value, out Y) ? Y : 0; // القيمة الافتراضية 0 إذا فشل التحويل

                this.Size = new Size(X, Y);
            }
            if(pQtyUse == "0")
            {
                btnUse.Visible = false;
            }
            else
            {
                btnUse.Visible = true;
            }
            RefreshItem();
        }
        private void CenterLabelName()
        {
            // ✅ ضبط خصائص اللابل عشان يكسر السطر تلقائي
            lblName.AutoSize = false;
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            lblName.MaximumSize = new Size(bottomPanel.Width - 20, 0); // عرض ثابت + ارتفاع ديناميكي
            lblName.AutoEllipsis = false; // إلغاء "..."

            // ✅ تحديث الارتفاع بناءً على النص الجديد
            Size textSize = TextRenderer.MeasureText(lblName.Text, lblName.Font, lblName.MaximumSize, TextFormatFlags.WordBreak);
            lblName.Height = textSize.Height;

            // ✅ ضبط موقعه في النص
            int centerX = (bottomPanel.Width - lblName.Width) / 2;
            int centerY = (bottomPanel.Height - lblName.Height) / 2;

            lblName.Location = new Point(centerX, centerY);
        }


        private void txtImage_Click(object sender, EventArgs e)
        {

           

        }
        private void panelName_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
            //RefreshItem();
        }


        public void RefreshItem()
        {
            string qry = "SELECT pName, sellPrice, pImage, shorcut FROM products WHERE pID = " + id;

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    lblName.Text = dt.Rows[0]["pName"].ToString();
                    lblPrice.Text = dt.Rows[0]["sellPrice"].ToString();
                    lblPlace.Text = dt.Rows[0]["shorcut"].ToString();

                    if (dt.Rows[0]["pImage"] != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])dt.Rows[0]["pImage"];
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            originalImage = Image.FromStream(ms);
                            SetProductImageAsync(originalImage);
                        }
                    }
                    else
                    {
                        originalImage = null; // 🔥 مفيش صورة
                        proImage.Image = null;
                    }
                }
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (proImage.Visible == true)
            {
                proImage.Visible = false;

                lblPlace.Visible = true;
                lblPlaceName.Visible = true;
            }
            else
            {
                proImage.Visible = true;

                lblPlace.Visible = false;
                lblPlaceName.Visible = false;
            }
            onAbout?.Invoke(this, e);

        }

        private void ucProduct_Leave(object sender, EventArgs e)
        {
            proImage.Visible = true;
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            onEdite?.Invoke(this, e);
            RefreshItem();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
            proImage.Visible = true;

            RefreshItem();
        }

        public void CheckAndRemoveIfOutOfStock()
        {
            if (double.TryParse(this.pQty, out double qty) && qty <= 0)
            {
                if (this.Parent != null)
                {
                    this.Parent.Controls.Remove(this);
                    this.Dispose();
                }
            }
        }

        public void ApplyFlatBorder(Control control, Color borderColor, float borderWidth = 1f)
        {
            control.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (Pen pen = new Pen(borderColor, borderWidth))
                {
                    Rectangle rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            // للتأكد أن الـ Paint يتنفذ
            control.Invalidate();
        }
        private void LightMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(243, 243, 243);
            backgroundseconder = Color.FromArgb(230, 230, 230);
            textColor = Color.FromArgb(51, 51, 51);
            checkedFillColor = Color.FromArgb(136, 214, 218);
            borderColor = Color.FromArgb(1, 95, 95);

            btnInfo.Image = Properties.Resources.info_light;
            btnEdite.Image = Properties.Resources.edite_light;
        }
        private void DarkMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            borderColor = Color.FromArgb(136, 214, 218);

            btnInfo.Image = Properties.Resources.info_dark;
            btnEdite.Image = Properties.Resources.edite_dark;

        }
        private void ThemeMode()
        {
            this.BackColor = backgroundseconder;
            ApplyFlatBorder(this, borderColor, 2f);

            bottomPanel.BackColor = checkedFillColor;

            //-> Image
            proImage.BackColor = backgroundseconder;

            btnEdite.BackColor = backgroundseconder;

            btnInfo.BackColor = backgroundseconder;

            //-> Lables
            lblName.ForeColor = textColor;
            lblQtyname.ForeColor = textColor;
            lblQty.ForeColor = textColor;
            lblPriceName.ForeColor = textColor;
            lblPrice.ForeColor = textColor;
            lblWholName.ForeColor = textColor;
            lblQtyUse.ForeColor = textColor;

            line.BackColor = backgroundseconder;
            line.FillColor = textColor;
        }

        private CancellationTokenSource _resizeCts;

        public void SetProductImageAsync(Image img)
        {
            // ألغِ أي مهمة سابقة
            _resizeCts?.Cancel();
            _resizeCts = new CancellationTokenSource();
            var token = _resizeCts.Token;

            Task.Run(() =>
            {
                if (token.IsCancellationRequested) return;

                // ✅ انتظر إنشاء الـ Handle
                while (!proImage.IsHandleCreated)
                {
                    if (token.IsCancellationRequested) return;
                    Thread.Sleep(10);
                }

                if (token.IsCancellationRequested || proImage.IsDisposed)
                    return;

                // ✅ عرض الصورة الأصلية بدون تعديل حجمها
                proImage.BeginInvoke((System.Action)(() =>
                {
                    if (!proImage.IsDisposed)
                    {
                        proImage.SizeMode = PictureBoxSizeMode.Zoom; // 🔥 الصورة كاملة
                        proImage.Image = img; // 🔥 الصورة الأصلية
                    }
                }));
            }, token);
        }


        // ثم في حدث إغلاق الفورم:
        private void frmPOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            _resizeCts?.Cancel();

        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            onSelect2?.Invoke(this, e);

            proImage.Visible = true;


            lblPlace.Visible = false;
            lblPlaceName.Visible = false;
        }

        private void proImage_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
            proImage.Visible = true;

        }

        private void bottomPanel_DoubleClick(object sender, EventArgs e)
        {
           
            proImage.Visible = true;
            showImg?.Invoke(this, e);

        }

    }
}
