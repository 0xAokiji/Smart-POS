using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraMap.ItemEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.UserControls
{
    public partial class saveLogingIcon : UserControl
    {
        public string username { get; set; }
        public string password { get; set; }
        public string lastLogin { get; set; }

        public event EventHandler onClick = null;
        public event EventHandler onDelete = null;

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
            get { return perImage.Image; }
            set
            {
                originalImage = value;      // 🔹 خزّن الصورة الأصلية هنا
                SetProductImageAsync(value);
            }
        }

        public saveLogingIcon()
        {
            InitializeComponent();
        }

        private void saveLogingIcon_Load(object sender, EventArgs e)
        {
        }
        private void CenterLabelName()
        {
            lblName.Dock = DockStyle.Fill; // ياخد المساحة كلها في البانيل
            lblName.TextAlign = ContentAlignment.TopCenter; // النص في النص
            lblName.AutoSize = false;
            lblName.AutoEllipsis = false; // ما يعملش "..."
            lblName.UseMnemonic = false;

            // 🔹 خط عربي مناسب
            lblName.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            // ممكن تجرب: new Font("Tahoma", 12, FontStyle.Regular);
            // أو new Font("Cairo", 13, FontStyle.Bold);
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
                while (!perImage.IsHandleCreated)
                {
                    if (token.IsCancellationRequested) return;
                    Thread.Sleep(10);
                }

                if (token.IsCancellationRequested || perImage.IsDisposed)
                    return;

                // ✅ عرض الصورة الأصلية بدون تعديل حجمها
                perImage.BeginInvoke((System.Action)(() =>
                {
                    if (!perImage.IsDisposed)
                    {
                        perImage.SizeMode = PictureBoxSizeMode.Zoom; // 🔥 الصورة كاملة
                        perImage.Image = img; // 🔥 الصورة الأصلية
                    }
                }));
            }, token);
        }

        private void perImage_Click(object sender, EventArgs e)
        {
            onClick?.Invoke(this, e);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            onDelete?.Invoke(this, e);

        }
    }

}
