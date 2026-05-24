using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;
using ZXing.Windows.Compatibility;


namespace pos.Classes
{
    internal class generatQR_Code
    {
        public Bitmap CreateQRCode(string text)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 1
                },
                Renderer = new BitmapRenderer() // لازم تحدد Renderer
            };

            return writer.Write(text);
        }
    }
}
