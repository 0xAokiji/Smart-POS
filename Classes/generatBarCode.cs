using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace pos.Classes
{
    internal class generatBarCode
    {
        public Bitmap CreateBarCode(string text)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128, // نوع الباركود
                Options = new EncodingOptions
                {
                    Height = 50,   // ارتفاع الباركود
                    Width = 170,   // عرض الباركود
                    Margin = 1     // تقليل المسافة البيضاء
                }
            };

            // توليد صورة الباركود
            Bitmap barcodeImage = writer.Write(text);

            // الأبعاد النهائية (باركود + الرقم تحت)
            int labelWidth = 189;
            int labelHeight = 80;

            Bitmap finalImage = new Bitmap(labelWidth, labelHeight);

            using (Graphics g = Graphics.FromImage(finalImage))
            {
                g.Clear(Color.White);

                // 🔹 رسم الباركود في النص
                int barcodeX = 7;
                int barcodeY = 5;
                g.DrawImage(barcodeImage, barcodeX, barcodeY);

                //// 🔹 كتابة الرقم تحت الباركود
                //using (Font font = new Font("Arial", 10, FontStyle.Bold))
                //using (SolidBrush brush = new SolidBrush(Color.Black))
                //{
                //    SizeF textSize = g.MeasureString(text, font);
                //    float textX = (labelWidth - textSize.Width) / 2;
                //    float textY = barcodeY + barcodeImage.Height + 2;
                //    g.DrawString(text, font, brush, textX, textY);
                //}
            }

            return finalImage;
        }
    }
}
