using DevExpress.CodeParser;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit.UI;
using DevExpress.XtraRichEdit.Utils;
using pos.Classes;
using pos.Model.POS;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pos.Model
{
    public partial class frmCategoryCard : SampleAdd
    {
        public int id = 0;

        public const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;

        private string qry;
        private string qry2;
        private int catID = 0;
        private int UnitID1 = 0;
        private int UnitID2 = 0;
        private int UnitID3 = 0;
        private int UnitIDdefalt = 0;
        bool thisClose = false;
        private string NewBarode = string.Empty;
        private string UsedBarode = string.Empty;
        private bool isUsed = false;

        private Image barcodeNewImag;
        private Image barcodeUsedImag;

        // تحرك الفروم من خلال سحب بنال
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        // استدعاء API من user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //////////
        ///
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public frmCategoryCard()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            txtSugester();
            textSuggesterPro();

            //ThemeMode();
            iconImage.Image = Properties.Resources.product_image_Dark;
            btnMoreQRcode.Image = Properties.Resources.addition_dark;
            btnAddUnit.Image = Properties.Resources.addition_dark;

            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);



        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED - لتقليل الوميض أثناء الرسم
                cp.ExStyle |= 0x80;       // WS_EX_TOOLWINDOW - لجعل الفورم لا يظهر في شريط المهام
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // اجعل الحواف ناعمة (AntiAlias)
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // لون الإطار من MainClass
            Color borderColor = MainClass.CheckedFillColor;
            float borderSize = 1f;

            using (Pen pen = new Pen(borderColor, borderSize))
            {
                int offset = (int)(borderSize / 2);

                // رسم مستطيل حول الفورم
                e.Graphics.DrawRectangle(pen, new Rectangle(offset, offset, this.Width - (int)borderSize, this.Height - (int)borderSize));
            }
        }
        private string Hdisc = "";
        private string Max = "";
        private async void frmCategoryCard_Load(object sender, EventArgs e)
        {

            loadData();
            await LoadPartyNamesAsync();
            GetProductByPID();
            string qry1 = "select uID 'id' , uName 'name' from untits ";
            MainClass.CBFill(qry1, comboBox1);
            LoadDataIntoContextMenu2();
            MakePanelRoundedCorners(paneQRcode, 12);
            MakePanelRoundedCorners(AddUnitPanel, 12);

            if (id == 0)
            {
                //btnEdit.Enabled = false;
                //btnEdit2.Enabled = false;
                //btnPSave.Enabled = true;
                //gbUnits.Enabled = false;
                //gbPrices.Enabled = false;

                // توليد أكواد جديدة للمنتج الجديد
                UsedBarode = GenerateUniqueInvoiceCode(false);
                NewBarode = GenerateUniqueInvoiceCode(true);
            }
            else
            {
                btnEdit.Enabled = true;
                btnEdit2.Enabled = true;
                btnEdit3.Enabled = true;
                btnPSave.Enabled = true;
                btnDone.Enabled = false;
                gbUnits.Enabled = false;
                gbPrices.Enabled = false;
                btnDone2.Enabled = false;

                // لو الأكواد مش موجودة في الداتابيز → توليد جديد
                if (string.IsNullOrEmpty(NewBarode))
                    NewBarode = GenerateUniqueInvoiceCode(true);

                if (string.IsNullOrEmpty(UsedBarode))
                    UsedBarode = GenerateUniqueInvoiceCode(false);
            }
            var barGenerator = new generatBarCode();
            barcodeUsedImag = barGenerator.CreateBarCode(UsedBarode);
            imgUsedCode.Image = barcodeUsedImag;

            var barGenerator2 = new generatBarCode();
            barcodeNewImag = barGenerator2.CreateBarCode(NewBarode);
            imgNewCode.Image = barcodeNewImag;


            txtPname.Focus();
        }

        private void textSuggesterPro()
        {
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT pName FROM products", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                    while (reader.Read())
                    {
                        dataSource.Add(reader.GetString(0));
                    }

                    txtPname.AutoCompleteCustomSource = dataSource;
                    txtPname.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtPname.AutoCompleteMode = AutoCompleteMode.Suggest; // ✅ عرض بس بدون كتابة تلقائية
                }
            }
        }


        private void txtSugester()
        {
            string qry = @"SELECT catName FROM category";

            using (SqlConnection con = MainClass.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;
                    DataTable dt2 = new DataTable();

                    using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                    {
                        da2.Fill(dt2);
                    }

                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        dataSource.Add(dt2.Rows[i][0].ToString());
                    }

                    this.txtCat.AutoCompleteCustomSource = dataSource;
                    this.txtCat.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtCat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    this.txtCat.RightToLeft = System.Windows.Forms.RightToLeft.No;
                }
            }
        }

        private void loadData()
        {
            if (id == 0)
                return;

            try
            {
                using (SqlConnection connection = MainClass.GetConnection())
                {
                    string qry = @"
                SELECT pName, pCode, shorcut, pNewBarode, pUsedBarode, compName, categoryID, purPrice, purUsedPrice, 
                       purPriceUnit2, purUsedPriceUnit2, purPriceUnit3, purUsedPriceUnit3,
                       sellPrice, sellPriceUsed, discountPro, hDiscountPro, hDiscountProUse, hDiscountProU2, hDiscountProUseU2, hDiscountProU3, hDiscountProUseU3,
                       lowestSellingPrice, lowestSellingPriceUse, lowestSellingPriceUnit2, lowestSellingPriceUseUnit2,
                       lowestSellingPriceUnit3, lowestSellingPriceUseUnit3,
                       ProductInfo, showInShortcomming, semiWholesale, semiWholesaleUse, 
                       wholesaleUse, wholesale, discAllaw, pImage, minimump, requestP, 
                       idUnite1, idUnite2, idUniteDef, idUnite3, 
                       numberU2, numberU3, priceU2, priceU3, priceU2Used, priceU3Used,
                       wholesaleUnit2, semiWholesaleUnit2, wholesaleUseUnit2, semiWholesaleUseUnit2,
                       wholesaleUnit3, semiWholesaleUnit3, wholesaleUseUnit3, semiWholesaleUseUnit3
                FROM products WHERE pID = @id";

                    SqlCommand command = new SqlCommand(qry, connection);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // بيانات أساسية
                        txtPname.Text = reader["pName"].ToString();
                        txtPcode.Text = reader["pCode"].ToString();
                        txtPshortcut.Text = reader["shorcut"].ToString();
                        txtcompName.Text = reader["compName"].ToString();
                        txtProInfo.Text = reader["ProductInfo"].ToString();

                        NewBarode = reader["pNewBarode"] == DBNull.Value ? null : reader["pNewBarode"].ToString();
                        UsedBarode = reader["pUsedBarode"] == DBNull.Value ? null : reader["pUsedBarode"].ToString();

                        // أسعار شراء
                        txtPurchasePrice.Text = reader["purPrice"].ToString();
                        txtPurchasePriceUsed.Text = reader["purUsedPrice"].ToString();
                        txtPurchasePriceU2.Text = reader["purPriceUnit2"].ToString();
                        txtPurchasePriceUsedU2.Text = reader["purUsedPriceUnit2"].ToString();
                        txtPurchasePriceU3.Text = reader["purPriceUnit3"].ToString();
                        txtPurchasePriceUsedU3.Text = reader["purUsedPriceUnit3"].ToString();

                        // أسعار بيع
                        txtSellPrice.Text = reader["sellPrice"].ToString();
                        txtUsedPrice.Text = reader["sellPriceUsed"].ToString();

                        // أسعار جملة
                        txtWholesale.Text = reader["wholesale"].ToString();
                        txtSemiWholesale.Text = reader["semiWholesale"].ToString();
                        txtWholesaleUse.Text = reader["wholesaleUse"].ToString();
                        txtSemiWholesaleUse.Text = reader["semiWholesaleUse"].ToString();

                        txtWholesaleU2.Text = reader["wholesaleUnit2"].ToString();
                        txtSemiWholesaleU2.Text = reader["semiWholesaleUnit2"].ToString();
                        txtWholesaleUseU2.Text = reader["wholesaleUseUnit2"].ToString();
                        txtSemiWholesaleUseU2.Text = reader["semiWholesaleUseUnit2"].ToString();

                        txtWholesaleU3.Text = reader["wholesaleUnit3"].ToString();
                        txtSemiWholesaleU3.Text = reader["semiWholesaleUnit3"].ToString();
                        txtWholesaleUseU3.Text = reader["wholesaleUseUnit3"].ToString();
                        txtSemiWholesaleUseU3.Text = reader["semiWholesaleUseUnit3"].ToString();

                        // أعداد وأسعار وحدات
                        txtUnumber2.Text = reader["numberU2"].ToString();
                        txtUnumber3.Text = reader["numberU3"].ToString();
                        txtUprice2.Text = reader["priceU2"].ToString();
                        txtUprice3.Text = reader["priceU3"].ToString();
                        txtPriceUsed2.Text = reader["priceU2Used"].ToString();
                        txtPriceUsed3.Text = reader["priceU3Used"].ToString();

                        // أقل سعر بيع
                        txtlowestSellingPriceNew.Text = reader["lowestSellingPrice"].ToString();
                        txtlowestSellingPriceUse.Text = reader["lowestSellingPriceUse"].ToString();
                        txtlowestSellingPriceNewU2.Text = reader["lowestSellingPriceUnit2"].ToString();
                        txtlowestSellingPriceUseU2.Text = reader["lowestSellingPriceUseUnit2"].ToString();
                        txtlowestSellingPriceNewU3.Text = reader["lowestSellingPriceUnit3"].ToString();
                        txtlowestSellingPriceUseU3.Text = reader["lowestSellingPriceUseUnit3"].ToString();

                        // خصومات
                        percentNew = reader["hDiscountPro"] == DBNull.Value ? 0 : Convert.ToDouble(reader["hDiscountPro"]);
                        percentUse = reader["hDiscountProUse"] == DBNull.Value ? 0 : Convert.ToDouble(reader["hDiscountProUse"]);
                        percentU2 = reader["hDiscountProU2"] == DBNull.Value ? 0 : Convert.ToDouble(reader["hDiscountProU2"]);
                        percentU3 = reader["hDiscountProU3"] == DBNull.Value ? 0 : Convert.ToDouble(reader["hDiscountProU3"]);
                        percentUseU2 = reader["hDiscountProUseU2"] == DBNull.Value ? 0 : Convert.ToDouble(reader["hDiscountProUseU2"]);
                        percentUseU3 = reader["hDiscountProUseU3"] == DBNull.Value ? 0 : Convert.ToDouble(reader["hDiscountProUseU3"]);
                        Hdisc = reader["hDiscountPro"].ToString();

                        // أعمدة إضافية
                        catID = Convert.ToInt32(reader["categoryID"]);
                        UnitID1 = Convert.ToInt32(reader["idUnite1"]);
                        UnitID2 = Convert.ToInt32(reader["idUnite2"]);
                        UnitID3 = Convert.ToInt32(reader["idUnite3"]);
                        UnitIDdefalt = Convert.ToInt32(reader["idUniteDef"]);

                        cbDiscountAllow.Checked = reader["discAllaw"] != DBNull.Value && Convert.ToBoolean(reader["discAllaw"]);
                        cbShortcomming.Checked = reader["showInShortcomming"] != DBNull.Value && Convert.ToBoolean(reader["showInShortcomming"]);

                        txtMinimum.Text = reader["minimump"].ToString();
                        txtRequst.Text = reader["requestP"].ToString();

                        // صورة المنتج
                        if (reader["pImage"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["pImage"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                txtImage.Image = Image.FromStream(ms);
                            }
                        }

                        Max = txtWholesale.Text; // تحديد الحد الأقصى من سعر الجملة
                    }
                    reader.Close();

                    // جلب أسماء الوحدات
                    string qryUnit = "SELECT uName FROM untits WHERE uID = @id";
                    SqlCommand cmdUnit = new SqlCommand(qryUnit, connection);
                    cmdUnit.Parameters.Add("@id", SqlDbType.Int);

                    cmdUnit.Parameters["@id"].Value = UnitID1;
                    txtUnite1.Text = cmdUnit.ExecuteScalar()?.ToString();

                    cmdUnit.Parameters["@id"].Value = UnitID2;
                    txtUnite2.Text = cmdUnit.ExecuteScalar()?.ToString();

                    cmdUnit.Parameters["@id"].Value = UnitID3;
                    txtUnite3.Text = cmdUnit.ExecuteScalar()?.ToString();

                    cmdUnit.Parameters["@id"].Value = UnitIDdefalt;
                    txtUniteSell.Text = cmdUnit.ExecuteScalar()?.ToString();

                    // جلب اسم التصنيف
                    SqlCommand cmdCat = new SqlCommand("SELECT catName FROM category WHERE catID = @id", connection);
                    cmdCat.Parameters.AddWithValue("@id", catID);
                    txtCat.Text = cmdCat.ExecuteScalar()?.ToString();

                    // جلب أكواد الباركود
                    SqlCommand cmdBarcode = new SqlCommand("SELECT * FROM internationalBarcode WHERE pID = @id", connection);
                    cmdBarcode.Parameters.AddWithValue("@id", id);
                    SqlDataReader rBarcode = cmdBarcode.ExecuteReader();
                    if (rBarcode.Read())
                    {
                        txtInternationalCode.Text = rBarcode["barcode1"].ToString();
                        txtBarcode1.Text = rBarcode["barcode2"].ToString();
                        txtBarcode2.Text = rBarcode["barcode3"].ToString();
                        txtBarcode3.Text = rBarcode["barcode4"].ToString();
                        txtBarcode4.Text = rBarcode["barcode5"].ToString();
                    }
                    rBarcode.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string filePath;
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.webp";
            ofd.Title = "Select an Image";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);
            }

        }

        private void btnPSave_Click(object sender, EventArgs e)
        {
            System.Drawing.Image temp = new Bitmap(txtImage.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();

            if (id == 0)
            {
                qry = @"
                   INSERT INTO products 
                    (pName, pCode, pNewBarode, pUsedBarode, shorcut, compName, categoryID, expitation,
                     purPrice, purUsedPrice, purPriceUnit2, purUsedPriceUnit2, purPriceUnit3, purUsedPriceUnit3,
                     sellPrice, sellPriceUsed, wholesale, semiWholesale, wholesaleUse, semiWholesaleUse,
                     wholesaleUnit2, semiWholesaleUnit2, wholesaleUseUnit2, semiWholesaleUseUnit2,
                     wholesaleUnit3, semiWholesaleUnit3, wholesaleUseUnit3, semiWholesaleUseUnit3,
                     idUnite1, idUnite2, idUniteDef, idUnite3, numberU2, numberU3, priceU2, priceU3,
                     priceU2Used, priceU3Used, discountPro, hDiscountPro, hDiscountProUse, hDiscountProU2, hDiscountProUseU2, hDiscountProU3, hDiscountProUseU3,
                     lowestSellingPrice, lowestSellingPriceUse, lowestSellingPriceUnit2, lowestSellingPriceUseUnit2,
                     lowestSellingPriceUnit3, lowestSellingPriceUseUnit3,
                     discAllaw, minimump, requestP, pImage, ProductInfo, showInShortcomming)
                  VALUES 
                    (@Name, @pCode, @pNewBarode, @pUsedBarode, @shorcut, @compName, @categoryID, @expitation,
                     @purPrice, @purUsedPrice, @purPriceUnit2, @purUsedPriceUnit2, @purPriceUnit3, @purUsedPriceUnit3,
                     @sellPrice, @sellPriceUsed, @wholesale, @semiWholesale, @wholesaleUse, @semiWholesaleUse,
                     @wholesaleUnit2, @semiWholesaleUnit2, @wholesaleUseUnit2, @semiWholesaleUseUnit2,
                     @wholesaleUnit3, @semiWholesaleUnit3, @wholesaleUseUnit3, @semiWholesaleUseUnit3,
                     @idUnite1, @idUnite2, @idUniteDef, @idUnite3, @numberU2, @numberU3, @priceU2, @priceU3,
                     @priceU2Used, @priceU3Used, @discountPro, @hDiscountPro, @hDiscountProUse, @hDiscountProU2, @hDiscountProUseU2, @hDiscountProU3, @hDiscountProUseU3,
                     @lowestSellingPrice, @lowestSellingPriceUse, @lowestSellingPriceUnit2, @lowestSellingPriceUseUnit2,
                     @lowestSellingPriceUnit3, @lowestSellingPriceUseUnit3,
                     @discAllaw, @minimump, @requestP, @pImage, @ProductInfo, @showInShortcomming); 
              SELECT SCOPE_IDENTITY()";

                qry2 = @"INSERT INTO internationalBarcode 
        (pID, barcode1, barcode2, barcode3, barcode4, barcode5)
        VALUES (@pID, @barcode1, @barcode2, @barcode3, @barcode4, @barcode5)";

                thisClose = false;
            }
            else
            {
                qry = @"
                  UPDATE products SET 
                    pName = @Name, pCode = @pCode, pNewBarode = @pNewBarode, pUsedBarode = @pUsedBarode,
                    shorcut = @shorcut, compName = @compName, categoryID = @categoryID, expitation = @expitation,
                    purPrice = @purPrice, purUsedPrice = @purUsedPrice, purPriceUnit2 = @purPriceUnit2,
                    purUsedPriceUnit2 = @purUsedPriceUnit2, purPriceUnit3 = @purPriceUnit3, purUsedPriceUnit3 = @purUsedPriceUnit3,
                    sellPrice = @sellPrice, sellPriceUsed = @sellPriceUsed,
                    wholesale = @wholesale, semiWholesale = @semiWholesale, wholesaleUse = @wholesaleUse, semiWholesaleUse = @semiWholesaleUse,
                    wholesaleUnit2 = @wholesaleUnit2, semiWholesaleUnit2 = @semiWholesaleUnit2,
                    wholesaleUseUnit2 = @wholesaleUseUnit2, semiWholesaleUseUnit2 = @semiWholesaleUseUnit2,
                    wholesaleUnit3 = @wholesaleUnit3, semiWholesaleUnit3 = @semiWholesaleUnit3,
                    wholesaleUseUnit3 = @wholesaleUseUnit3, semiWholesaleUseUnit3 = @semiWholesaleUseUnit3,
                    idUnite1 = @idUnite1, idUnite2 = @idUnite2, idUniteDef = @idUniteDef, idUnite3 = @idUnite3,
                    numberU2 = @numberU2, numberU3 = @numberU3,
                    priceU2 = @priceU2, priceU3 = @priceU3, priceU2Used = @priceU2Used, priceU3Used = @priceU3Used,
                    discountPro = @discountPro, hDiscountPro = @hDiscountPro, hDiscountProUse = @hDiscountProUse,
                    hDiscountProU2 = @hDiscountProU2, hDiscountProUseU2 = @hDiscountProUseU2,
                    hDiscountProU3 = @hDiscountProU3, hDiscountProUseU3 = @hDiscountProUseU3,
                    lowestSellingPrice = @lowestSellingPrice, lowestSellingPriceUse = @lowestSellingPriceUse,
                    lowestSellingPriceUnit2 = @lowestSellingPriceUnit2, lowestSellingPriceUseUnit2 = @lowestSellingPriceUseUnit2,
                    lowestSellingPriceUnit3 = @lowestSellingPriceUnit3, lowestSellingPriceUseUnit3 = @lowestSellingPriceUseUnit3,
                    discAllaw = @discAllaw, minimump = @minimump, requestP = @requestP,
                    pImage = @pImage, ProductInfo = @ProductInfo, showInShortcomming = @showInShortcomming
                  WHERE pID = @id";

                qry2 = @"UPDATE internationalBarcode SET 
            pID = @pID, barcode1 = @barcode1, barcode2 = @barcode2, barcode3 = @barcode3,
            barcode4 = @barcode4, barcode5 = @barcode5
         WHERE pID = @id";

                thisClose = true;
            }

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open(); // فتح الاتصال قبل أي Execute
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Name", txtPname.Text);
                cmd.Parameters.AddWithValue("@pCode", txtPcode.Text);
                cmd.Parameters.AddWithValue("@pNewBarode", NewBarode);
                cmd.Parameters.AddWithValue("@pUsedBarode", UsedBarode);
                cmd.Parameters.AddWithValue("@shorcut", txtPshortcut.Text);
                cmd.Parameters.AddWithValue("@compName", txtcompName.Text);
                cmd.Parameters.AddWithValue("@ProductInfo", txtProInfo.Text);
                cmd.Parameters.AddWithValue("@categoryID", catID);
                cmd.Parameters.AddWithValue("@expitation", DBNull.Value);

                // أسعار الشراء
                cmd.Parameters.AddWithValue("@purPrice", GetDouble(txtPurchasePrice.Text));
                cmd.Parameters.AddWithValue("@purUsedPrice", GetDouble(txtPurchasePriceUsed.Text));
                cmd.Parameters.AddWithValue("@purPriceUnit2", GetDouble(txtPurchasePriceU2.Text));
                cmd.Parameters.AddWithValue("@purUsedPriceUnit2", GetDouble(txtPurchasePriceUsedU2.Text));
                cmd.Parameters.AddWithValue("@purPriceUnit3", GetDouble(txtPurchasePriceU3.Text));
                cmd.Parameters.AddWithValue("@purUsedPriceUnit3", GetDouble(txtPurchasePriceUsedU3.Text));

                // أسعار البيع
                cmd.Parameters.AddWithValue("@sellPrice", GetDouble(txtSellPrice.Text));
                cmd.Parameters.AddWithValue("@sellPriceUsed", GetDouble(txtUsedPrice.Text));

                // أسعار الجملة (الوحدة الأساسية)
                cmd.Parameters.AddWithValue("@wholesale", GetDouble(txtWholesale.Text));
                cmd.Parameters.AddWithValue("@semiWholesale", GetDouble(txtSemiWholesale.Text));
                cmd.Parameters.AddWithValue("@wholesaleUse", GetDouble(txtWholesaleUse.Text));
                cmd.Parameters.AddWithValue("@semiWholesaleUse", GetDouble(txtSemiWholesaleUse.Text));

                // أسعار الجملة (الوحدة الثانية)
                cmd.Parameters.AddWithValue("@wholesaleUnit2", GetDouble(txtWholesaleU2.Text));
                cmd.Parameters.AddWithValue("@semiWholesaleUnit2", GetDouble(txtSemiWholesaleU2.Text));
                cmd.Parameters.AddWithValue("@wholesaleUseUnit2", GetDouble(txtWholesaleUseU2.Text));
                cmd.Parameters.AddWithValue("@semiWholesaleUseUnit2", GetDouble(txtSemiWholesaleUseU2.Text));

                // أسعار الجملة (الوحدة الثالثة)
                cmd.Parameters.AddWithValue("@wholesaleUnit3", GetDouble(txtWholesaleU3.Text));
                cmd.Parameters.AddWithValue("@semiWholesaleUnit3", GetDouble(txtSemiWholesaleU3.Text));
                cmd.Parameters.AddWithValue("@wholesaleUseUnit3", GetDouble(txtWholesaleUseU3.Text));
                cmd.Parameters.AddWithValue("@semiWholesaleUseUnit3", GetDouble(txtSemiWholesaleUseU3.Text));

                // معرفات الوحدات
                cmd.Parameters.AddWithValue("@idUnite1", UnitID1);
                cmd.Parameters.AddWithValue("@idUnite2", UnitID2);
                cmd.Parameters.AddWithValue("@idUniteDef", UnitIDdefalt);
                cmd.Parameters.AddWithValue("@idUnite3", UnitID3);

                // أعداد الوحدات
                cmd.Parameters.AddWithValue("@numberU2", GetDouble(txtUnumber2.Text));
                cmd.Parameters.AddWithValue("@numberU3", GetDouble(txtUnumber3.Text));

                // أسعار بيع الوحدات
                cmd.Parameters.AddWithValue("@priceU2", GetDouble(txtUprice2.Text));
                cmd.Parameters.AddWithValue("@priceU3", GetDouble(txtUprice3.Text));
                cmd.Parameters.AddWithValue("@priceU2Used", GetDouble(txtPriceUsed2.Text));
                cmd.Parameters.AddWithValue("@priceU3Used", GetDouble(txtPriceUsed3.Text));

                // الخصومات
                cmd.Parameters.AddWithValue("@discountPro", DBNull.Value);
                cmd.Parameters.AddWithValue("@hDiscountPro", percentNew);
                cmd.Parameters.AddWithValue("@hDiscountProUse", percentUse);
                cmd.Parameters.AddWithValue("@hDiscountProU2", percentU2);
                cmd.Parameters.AddWithValue("@hDiscountProUseU2", percentUseU3);
                cmd.Parameters.AddWithValue("@hDiscountProU3", percentU3);
                cmd.Parameters.AddWithValue("@hDiscountProUseU3", percentUseU3);

                // أقل سعر بيع
                cmd.Parameters.AddWithValue("@lowestSellingPrice", GetDouble(txtlowestSellingPriceNew.Text));
                cmd.Parameters.AddWithValue("@lowestSellingPriceUse", GetDouble(txtlowestSellingPriceUse.Text));
                cmd.Parameters.AddWithValue("@lowestSellingPriceUnit2", GetDouble(txtlowestSellingPriceNewU2.Text));
                cmd.Parameters.AddWithValue("@lowestSellingPriceUseUnit2", GetDouble(txtlowestSellingPriceUseU2.Text));
                cmd.Parameters.AddWithValue("@lowestSellingPriceUnit3", GetDouble(txtlowestSellingPriceNewU3.Text));
                cmd.Parameters.AddWithValue("@lowestSellingPriceUseUnit3", GetDouble(txtlowestSellingPriceUseU3.Text));


                cmd.Parameters.AddWithValue("@discAllaw", cbDiscountAllow.Checked ? 1 : 0);
                cmd.Parameters.AddWithValue("@showInShortcomming", cbShortcomming.Checked ? 1 : 0);
                cmd.Parameters.AddWithValue("@minimump", GetDouble(txtMinimum.Text));
                cmd.Parameters.AddWithValue("@requestP", GetDouble(txtRequst.Text));
                cmd.Parameters.AddWithValue("@pImage", imageByteArray);

                if (id == 0)
                {
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                    string qry3 = @"INSERT INTO totalStor 
                    (pID, qtyU1, qtyU2, qtyU3, qtyUsedU1, qtyUsedU2, qtyUsedU3) 
                    VALUES (@pID, @qtyU1, @qtyU2, @qtyU3, @qtyUsedU1, @qtyUsedU2, @qtyUsedU3)";
                    using (SqlCommand cmd3 = new SqlCommand(qry3, con))
                    {
                        cmd3.Parameters.AddWithValue("@pID", id);
                        cmd3.Parameters.AddWithValue("@qtyU1", Convert.ToDecimal(txtLargNew.Text == string.Empty ? "0" : txtLargNew.Text));
                        cmd3.Parameters.AddWithValue("@qtyU2", Convert.ToDecimal(txtMiduamNew.Text == string.Empty ? "0" : txtMiduamNew.Text));
                        cmd3.Parameters.AddWithValue("@qtyU3", Convert.ToDecimal(txtSmaillNew.Text == string.Empty ? "0" : txtSmaillNew.Text));
                        cmd3.Parameters.AddWithValue("@qtyUsedU1", Convert.ToDecimal(txtLargUsed.Text == string.Empty ? "0" : txtLargUsed.Text));
                        cmd3.Parameters.AddWithValue("@qtyUsedU2", Convert.ToDecimal(txtMiduamUsed.Text == string.Empty ? "0" : txtMiduamUsed.Text));
                        cmd3.Parameters.AddWithValue("@qtyUsedU3", Convert.ToDecimal(txtSmaillUsed.Text == string.Empty ? "0" : txtSmaillUsed.Text));
                        cmd3.ExecuteNonQuery();
                    }
                }
                else
                {
                    cmd.ExecuteNonQuery();
                    UpdateProduct();
                }

                // تحديث أو إدخال الباركود الدولي
                SqlCommand cmd2 = new SqlCommand(qry2, con);
                cmd2.Parameters.AddWithValue("@pID", id);
                cmd2.Parameters.AddWithValue("@barcode1", txtBarcode1.Text);
                cmd2.Parameters.AddWithValue("@barcode2", txtBarcode2.Text);
                cmd2.Parameters.AddWithValue("@barcode3", txtBarcode3.Text);
                cmd2.Parameters.AddWithValue("@barcode4", txtBarcode4.Text);
                cmd2.Parameters.AddWithValue("@barcode5", txtBarcode4.Text);
                cmd2.Parameters.AddWithValue("@id", id);
                cmd2.ExecuteNonQuery();

                // إغلاق الاتصال
                con.Close();
            }
            ClearFormFields();
            //// إغلاق الفورم إذا لزم الأمر
            if (thisClose)
                this.Close();

            Notifier.ShowNotification("Done ✅", "تم الحفظ بنجاح");
        }

        private void ClearFormFields()
        {
            // إعادة تعيين نصوص TextBox
            txtBarcode1.Text = null;
            txtBarcode2.Text = null;
            txtBarcode3.Text = null;
            txtBarcode4.Text = null;
            txtCat.Text = null;
            txtcompName.Text = null;
            txtlowestSellingPriceUse.Text = null;
            txtWholesale.Text = null;
            txtMinimum.Text = null;
            txtPname.Text = null;
            txtPshortcut.Text = null;
            txtPurchasePrice.Text = null;
            txtRequst.Text = null;
            txtSellPrice.Text = null;
            txtlowestSellingPriceNew.Text = null;
            txtUnite1.Text = null;
            txtUnite2.Text = null;
            txtUnite3.Text = null;
            txtUniteSell.Text = null;
            txtUnumber2.Text = null;
            txtUnumber3.Text = null;
            txtUprice2.Text = null;
            txtUprice3.Text = null;
            txtInternationalCode.Text = null;
            txtPurchasePriceUsed.Text = null;
            txtSemiWholesale.Text = null;
            txtPriceUsed2.Text = null;
            txtPriceUsed3.Text = null;
            txtProInfo.Text = null;
            txtPurchasePriceU2.Text = null;
            txtPurchasePriceU3.Text = null;
            txtSemiWholesaleU2.Text = null;
            txtSemiWholesaleU3.Text = null;
            txtlowestSellingPriceNewU2.Text = null;
            txtlowestSellingPriceNewU3.Text = null;
            txtWholesaleU2.Text = null;
            txtWholesaleU3.Text = null;
            txtWholesaleUseU2.Text = null;
            txtWholesaleUseU3.Text = null;
            txtSemiWholesaleUseU2.Text = null;
            txtSemiWholesaleUseU3.Text = null;
            txtPurchasePriceUsedU2.Text = null;
            txtPurchasePriceUsedU3.Text = null;
            txtUsedPrice.Text = null;
            txtWholesaleUse.Text = null;
            txtSemiWholesaleUse.Text = null;
            txtPcode.Text = null;

            txtlowestSellingPriceUse.Text = null;
            txtlowestSellingPriceUseU2.Text = null;
            txtlowestSellingPriceUseU3.Text = null;

            // إعادة تعيين قيم الأرقام
            txtLargNew.Text = "0.0";
            txtMiduamNew.Text = "0.0";
            txtSmaillNew.Text = "0.0";
            txtLargUsed.Text = "0.0";
            txtMiduamUsed.Text = "0.0";
            txtSmaillUsed.Text = "0.0";

            // إعادة الصورة الافتراضية
            txtImage.Image = Properties.Resources.ecommerce;

            // تعطيل أزرار التعديل حتى يتم اختيار منتج جديد
            btnEdit.Enabled = false;
            btnEdit2.Enabled = false;
            btnEdit3.Enabled = false;
            btnNext.Enabled = false;
            btnDone.Enabled = false;
            btnDone2.Enabled = false;

            btnPSave.Enabled = true;
            gbUnits.Enabled = false;
            gbPrices.Enabled = false;
            groupBox5.Enabled = false;

            catID = 0;

            // إعادة الباركود
            NewBarode = GenerateUniqueInvoiceCode(true);
            var barGenerator = new generatBarCode();
            imgUsedCode.Image = barGenerator.CreateBarCode(NewBarode);

            UsedBarode = GenerateUniqueInvoiceCode(false);
            var barGenerator2 = new generatBarCode();
            imgNewCode.Image = barGenerator2.CreateBarCode(UsedBarode);

            // وضع التركيز على حقل الاسم لبدء إدخال منتج جديد
            txtPname.Focus();

            // إعادة معرف المنتج
            id = 0;

            // إعادة تعيين CheckBox
            cbDiscountAllow.Checked = false;
            cbShortcomming.Checked = false;
        }

        private double GetDouble(string text)
        {
            return double.TryParse(text, out double val) ? val : 0;
        }


        private void guna2Button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Point buttonLocation = label4.Location;

            paneQRcode.Location = new Point(49, 86);

            paneQRcode.Visible = !paneQRcode.Visible;
            paneQRcode.BringToFront();

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Point buttonLocation = btnMoreQRcode.Location;

            // ضبط موقع Panel ليظهر أسفل الزر مباشرةً
            // يمكنك تعديل القيمة 5 لضبط المسافة العمودية بين الزر وPanel حسب الحاجة
            paneQRcode.Location = new Point(49, 86);

            paneQRcode.Visible = !paneQRcode.Visible;
            paneQRcode.BringToFront();

        }

        private void MakePanelRoundedCorners(Panel panel, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int x = 0;
            int y = 0;
            int width = panel.Width - 1; // تعديل لتفادي قص الحواف
            int height = panel.Height - 1; // تعديل لتفادي قص الحواف

            // تعريف الأركان الدائرية
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);

            path.CloseFigure();

            // تطبيق الشكل على Panel
            panel.Region = new System.Drawing.Region(path);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //LoadDataIntoContextMenu();
            //contextMenuStrip1.Show(btnCategory, new Point(0, btnCategory.Height));
            frmCatecorySearch frm = new frmCatecorySearch(this);
            frm.ShowDialog();
        }
        //private void LoadDataIntoContextMenu()
        //{
        //    string query = "SELECT catID, catName FROM category";

        //    using (SqlConnection conn = MainClass.GetConnection()) // ✅ بدل new SqlConnection
        //    {
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        try
        //        {
        //            conn.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            contextMenuStrip1.Items.Clear();   // مسح العناصر قبل الإضافة

        //            while (reader.Read())
        //            {
        //                var item = contextMenuStrip1.Items.Add(reader["catName"].ToString());
        //                item.Tag = reader["catID"]; // تخزين الـ ID في خاصية Tag
        //                item.Click += contextMenu_Item_Click1;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}



        private void contextMenu_Item_Click1(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item != null)
            {
                txtCat.Text = item.Text;
                catID = Convert.ToInt32(item.Tag);
            }
        }
        private int select = 0;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(btnMaxUnit, new Point(0, btnMaxUnit.Height));
            LoadDataIntoContextMenu2();
            select = 1;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(btnMidUnit, new Point(0, btnMidUnit.Height));
            LoadDataIntoContextMenu2();
            select = 2;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(btnMinUnit, new Point(0, btnMinUnit.Height));
            LoadDataIntoContextMenu2();
            select = 3;
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(btnSellUnit, new Point(0, btnSellUnit.Height));
            LoadDataIntoContextMenu2();
            select = 4;
        }


        private void LoadDataIntoContextMenu2()
        {
            string query = "SELECT uID, uName FROM untits";

            using (SqlConnection conn = MainClass.GetConnection()) // ✅ بدل new SqlConnection
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    contextMenuStrip2.Items.Clear(); // مسح العناصر قبل الإضافة

                    while (reader.Read())
                    {
                        var item = contextMenuStrip2.Items.Add(reader["uName"].ToString());
                        item.Tag = reader["uID"]; // تخزين الـ ID في خاصية Tag
                        item.Click += contextMenu_Item_Click2;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            try
            {
                string qry = string.Empty;

                if (Convert.ToInt32(comboBox1.SelectedValue) == 0)
                {
                    qry = @"Insert into untits Values (@uName)";
                }
                else
                {
                    qry = @"Update untits Set sName = @uName";
                }

                using (SqlConnection con = MainClass.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@uName", comboBox1.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                string qry1 = "select uID 'id' , uName 'name' from untits";
                MainClass.CBFill(qry1, comboBox1);
                Notifier.ShowNotification("عملية ناجحة", "تم اضافة وحدة بنجاح");
            }
            catch
            {
                Notifier.ShowNotification("تحذير", "حدث خطأ");
                return;
            }
        }


        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    Notifier.ShowNotification("تحذير", "لم يتم تحديد العنصر");
                    return;
                }
                else
                {
                    int unitID = Convert.ToInt32(comboBox1.SelectedValue);

                    // ✅ تحقق لو الوحدة مستخدمة في أي منتج
                    string checkQry = @"
                    SELECT COUNT(*) 
                    FROM products 
                    WHERE idUnite1 = @uID 
                       OR idUnite2 = @uID 
                       OR idUniteDef = @uID 
                       OR idUnite3 = @uID";

                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(checkQry, con))
                    {
                        cmd.Parameters.AddWithValue("@uID", unitID);
                        con.Open();
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Notifier.ShowNotification("تحذير", "⚠️ هذه الوحدة مرتبطة بمنتجات، حذفها سيؤثر على المنتجات ويستلزم تعديلها أولاً.");
                            return; // وقف الحذف
                        }
                    }

                    // ✅ لو مش مستخدمة → نفذ الحذف
                    string qry = "DELETE FROM untits WHERE uID = @uID";
                    Hashtable ht = new Hashtable();
                    ht.Add("@uID", unitID);
                    MainClass.SQL(qry, ht);

                    // ✅ رجّع الكومبو بوكس
                    string qry1 = "SELECT uID 'id', uName 'name' FROM untits";
                    MainClass.CBFill(qry1, comboBox1);

                    Notifier.ShowNotification("عملية ناجحة", "تم الحذف بنجاح");
                    btnDel.Visible = false;
                }
            }
            catch
            {
                Notifier.ShowNotification("خطأ", "حدث خطأ");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDel.Visible = true;

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            AddUnitPanel.Visible = !AddUnitPanel.Visible;
            AddUnitPanel.Location = new Point(11, 39);
            AddUnitPanel.BringToFront();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            AddUnitPanel.Visible = false;

        }

        private void txtPurchasePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtWholesale.Focus();

            }
        }

        private void txtTax_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtSellPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام +, -, *, /, . و Backspace فقط
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '+' && e.KeyChar != '-' &&
                e.KeyChar != '*' && e.KeyChar != '/' &&
                e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtHdisc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.' || txtlowestSellingPriceUse.Text.Contains(".")))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtlowestSellingPriceNew.Focus();

            }
        }

        private void txtUprice2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
            if (e.KeyChar == (char)Keys.Enter)
            {

                e.Handled = true;
            }
        }

        private void txtUprice3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtUnumber2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام فقط وBackspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                unteThree();
                //txtPriceUsed3.Focus();
            }
        }

        private void unteThree()
        {
            int amount2 = int.Parse(string.IsNullOrWhiteSpace(txtUnumber3.Text) ? "1" : txtUnumber3.Text);

            // 🟢 سعر البيع الجديد
            double price2 = double.Parse(string.IsNullOrWhiteSpace(txtUprice2.Text) ? "0" : txtUprice2.Text);
            txtUprice3.Text = Math.Ceiling(price2 / amount2).ToString();

            // 🟢 سعر البيع المستعمل
            double usedPrice2 = double.Parse(string.IsNullOrWhiteSpace(txtPriceUsed2.Text) ? "0" : txtPriceUsed2.Text);
            txtPriceUsed3.Text = Math.Ceiling(usedPrice2 / amount2).ToString();

            // 🟢 سعر الشراء الجديد
            double purPrice2 = double.Parse(string.IsNullOrWhiteSpace(txtPurchasePriceU2.Text) ? "0" : txtPurchasePriceU2.Text);
            txtPurchasePriceU3.Text = Math.Ceiling(purPrice2 / amount2).ToString();

            // 🟢 سعر الشراء المستعمل
            double purUsedPrice2 = double.Parse(string.IsNullOrWhiteSpace(txtPurchasePriceUsedU2.Text) ? "0" : txtPurchasePriceUsedU2.Text);
            txtPurchasePriceUsedU3.Text = Math.Ceiling(purUsedPrice2 / amount2).ToString();

            // 🟢 أسعار الجملة
            double wholesale2 = double.Parse(string.IsNullOrWhiteSpace(txtWholesaleU2.Text) ? "0" : txtWholesaleU2.Text);
            txtWholesaleU3.Text = Math.Ceiling(wholesale2 / amount2).ToString();

            double semiWholesale2 = double.Parse(string.IsNullOrWhiteSpace(txtSemiWholesaleU2.Text) ? "0" : txtSemiWholesaleU2.Text);
            txtSemiWholesaleU3.Text = Math.Ceiling(semiWholesale2 / amount2).ToString();

            double wholesaleUse2 = double.Parse(string.IsNullOrWhiteSpace(txtWholesaleUseU2.Text) ? "0" : txtWholesaleUseU2.Text);
            txtWholesaleUseU3.Text = Math.Ceiling(wholesaleUse2 / amount2).ToString();

            double semiWholesaleUse2 = double.Parse(string.IsNullOrWhiteSpace(txtSemiWholesaleUseU2.Text) ? "0" : txtSemiWholesaleUseU2.Text);
            txtSemiWholesaleUseU3.Text = Math.Ceiling(semiWholesaleUse2 / amount2).ToString();

            // 🟢 أقل سعر بيع
            double lowestNew2 = double.Parse(string.IsNullOrWhiteSpace(txtlowestSellingPriceNewU2.Text) ? "0" : txtlowestSellingPriceNewU2.Text);
            txtlowestSellingPriceNewU3.Text = Math.Ceiling(lowestNew2 / amount2).ToString();

            double lowestUsed2 = double.Parse(string.IsNullOrWhiteSpace(txtlowestSellingPriceUseU2.Text) ? "0" : txtlowestSellingPriceUseU2.Text);
            txtlowestSellingPriceUseU3.Text = Math.Ceiling(lowestUsed2 / amount2).ToString();

        }
        private void cbDiscountAllow_CheckedChanged(object sender, EventArgs e)
        {
            //txtlowestSellingPriceUse.Enabled = cbDiscountAllow.Checked;
            //if (!txtlowestSellingPriceUse.Enabled)
            //{
            //    txtlowestSellingPriceUse.Text = "0";
            //}

        }



        private void guna2Button5_Click(object sender, EventArgs e)
        {
            contextMenuStrip2.Show(btnSellUnit,
            new Point(0, btnDefult.Height));
            select = 5;
        }
        private void contextMenu_Item_Click2(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item != null)
            {
                if (select == 1)
                {
                    txtUnite1.Text = item.Text;
                    UnitID1 = Convert.ToInt32(item.Tag);
                }
                else if (select == 2)
                {
                    txtUnite2.Text = item.Text;
                    UnitID2 = Convert.ToInt32(item.Tag);
                }
                else if (select == 3)
                {
                    txtUnite3.Text = item.Text;
                    UnitID3 = Convert.ToInt32(item.Tag);
                }
                else if (select == 4)
                {
                    txtUniteSell.Text = item.Text;
                    UnitIDdefalt = Convert.ToInt32(item.Tag);
                }
                else if (select == 5)
                {
                    // كل الوحدات زي الافتراضية
                    txtUniteSell.Text = item.Text;
                    UnitIDdefalt = Convert.ToInt32(item.Tag);

                    txtUnite1.Text = item.Text;
                    UnitID1 = Convert.ToInt32(item.Tag);

                    txtUnite2.Text = item.Text;
                    UnitID2 = Convert.ToInt32(item.Tag);

                    txtUnite3.Text = item.Text;
                    UnitID3 = Convert.ToInt32(item.Tag);

                    LoadDataIntoContextMenu2();

                    // ضبط الكميات
                    txtUnumber2.Text = "1";
                    txtUnumber3.Text = "1";

                    // ضبط أسعار البيع
                    txtUprice2.Text = txtSellPrice.Text;
                    txtUprice3.Text = txtSellPrice.Text;
                    txtPriceUsed2.Text = txtUsedPrice.Text;
                    txtPriceUsed3.Text = txtUsedPrice.Text;

                    // 🔥 أسعار الجملة
                    txtWholesaleU2.Text = txtWholesale.Text;
                    txtSemiWholesaleU2.Text = txtSemiWholesale.Text;
                    txtWholesaleUseU2.Text = txtWholesaleUse.Text;
                    txtSemiWholesaleUseU2.Text = txtSemiWholesaleUse.Text;
                    txtPurchasePriceU2.Text = txtPurchasePrice.Text;
                    txtPurchasePriceUsedU2.Text = txtPurchasePriceUsed.Text;

                    txtPurchasePriceUsedU3.Text = txtPurchasePriceUsed.Text;
                    txtPurchasePriceU3.Text = txtPurchasePrice.Text;

                    txtWholesaleU3.Text = txtWholesale.Text;
                    txtSemiWholesaleU3.Text = txtSemiWholesale.Text;
                    txtWholesaleUseU3.Text = txtWholesaleUse.Text;
                    txtSemiWholesaleUseU3.Text = txtSemiWholesaleUse.Text;

                    // 🔥 أقل سعر بيع
                    txtlowestSellingPriceNewU2.Text = txtlowestSellingPriceNew.Text;
                    txtlowestSellingPriceUseU2.Text = txtlowestSellingPriceUse.Text;
                    txtlowestSellingPriceNewU3.Text = txtlowestSellingPriceNew.Text;
                    txtlowestSellingPriceUseU3.Text = txtlowestSellingPriceUse.Text;
                }
            }
        }


        private void cbMax_CheckedChanged(object sender, EventArgs e)
        {
            if (!txtWholesale.Enabled)
            {
                txtWholesale.Text = null;
            }
            else
                txtWholesale.Text = Max;
        }



        private void txtPurchasePrice_Enter(object sender, EventArgs e)
        {
            txtPurchasePrice.SelectAll();
        }

        private void txtPurchasePrice_Click(object sender, EventArgs e)
        {
            txtPurchasePrice.SelectAll();
        }

        private void txtDiscount_Click(object sender, EventArgs e)
        {

        }

        private void txtTax_Click(object sender, EventArgs e)
        {
            txtlowestSellingPriceNew.SelectAll();

        }

        private void txtSellPrice_Click(object sender, EventArgs e)
        {
            txtSellPrice.SelectAll();

        }

        private void txtHdisc_Click(object sender, EventArgs e)
        {
            txtlowestSellingPriceUse.SelectAll();

        }

        private void txtdisV_Click(object sender, EventArgs e)
        {
            txtPurchasePriceUsed.SelectAll();

        }


        private bool eventHandled = false; // تعريف متغير بولياني لتتبع ما إذا تم تنفيذ الحدث بالفعل

        private void txtPcode_Leave(object sender, EventArgs e)
        {
            if (!eventHandled) // تحقق مما إذا تم تنفيذ الحدث بالفعل
            {


                eventHandled = true; // قم بتعيين المتغير إلى true بعد تنفيذ الحدث
            }
        }


        private void txtPcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {

            using (frmCategoryAdd frm = new frmCategoryAdd())
            {

                frm.Owner = this;
                frm.ShowDialog();
            }
            this.Focus();
        }



        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            textColor2 = MainClass.TextColor2;
            checkedFillColor = MainClass.CheckedFillColor;
            checkedForeColor = MainClass.CheckedForeColor;
        }
        private void ThemeMode()
        {

            if (MainClass.ThemeMode == "dark")
            {
                iconImage.Image = Properties.Resources.product_image_Dark;
                btnMoreQRcode.Image = Properties.Resources.addition_dark;
                btnAddUnit.Image = Properties.Resources.addition_dark;
            }
            else if (MainClass.ThemeMode == "light")
            {
                iconImage.Image = Properties.Resources.product_image__LIght;
                btnMoreQRcode.Image = Properties.Resources.addition_light;
                btnAddUnit.Image = Properties.Resources.addition_light;
            }

            ThemeColor();

            this.BackColor = backgroundPrimary;

            //Panels
            mainPanel.BackColor = backgroundPrimary;
            bottomPanel.BackColor = backgroundSecondary;
            topPanel.BackColor = checkedFillColor;
            AddUnitPanel.BackColor = backgroundSecondary;
            paneQRcode.BackColor = backgroundSecondary;
            UnitPanel.BackColor = backgroundPrimary;


            iconImage.BackColor = checkedFillColor;

            //Lables
            lblTitle.ForeColor = textColor;


            //Text box

            txtPname.BackColor = backgroundPrimary;
            txtPname.ForeColor = textColor2;
            txtPname.BorderColor = checkedFillColor;
            txtPname.FillColor = backgroundPrimary;

            txtInternationalCode.BackColor = backgroundPrimary;
            txtInternationalCode.ForeColor = textColor2;
            txtInternationalCode.BorderColor = checkedFillColor;
            txtInternationalCode.FillColor = backgroundPrimary;

            txtPshortcut.BackColor = backgroundPrimary;
            txtPshortcut.ForeColor = textColor2;
            txtPshortcut.BorderColor = checkedFillColor;
            txtPshortcut.FillColor = backgroundPrimary;

            txtCat.BackColor = backgroundPrimary;
            txtCat.ForeColor = textColor2;
            txtCat.BorderColor = checkedFillColor;
            txtCat.FillColor = backgroundPrimary;

            txtcompName.BackColor = backgroundPrimary;
            txtcompName.ForeColor = textColor2;
            txtcompName.BorderColor = checkedFillColor;
            txtcompName.FillColor = backgroundPrimary;

            ////////////
            txtSellPrice.BackColor = backgroundPrimary;
            txtSellPrice.ForeColor = textColor2;
            txtSellPrice.BorderColor = checkedFillColor;
            txtSellPrice.FillColor = backgroundPrimary;

            txtPurchasePrice.BackColor = backgroundPrimary;
            txtPurchasePrice.ForeColor = textColor2;
            txtPurchasePrice.BorderColor = checkedFillColor;
            txtPurchasePrice.FillColor = backgroundPrimary;

            txtWholesale.BackColor = backgroundPrimary;
            txtWholesale.ForeColor = textColor2;
            txtWholesale.BorderColor = checkedFillColor;
            txtWholesale.FillColor = backgroundPrimary;

            txtSemiWholesale.BackColor = backgroundPrimary;
            txtSemiWholesale.ForeColor = textColor2;
            txtSemiWholesale.BorderColor = checkedFillColor;
            txtSemiWholesale.FillColor = backgroundPrimary;

            txtlowestSellingPriceUse.BackColor = backgroundPrimary;
            txtlowestSellingPriceUse.ForeColor = textColor2;
            txtlowestSellingPriceUse.BorderColor = checkedFillColor;
            txtlowestSellingPriceUse.FillColor = backgroundPrimary;

            txtPurchasePriceUsed.BackColor = backgroundPrimary;
            txtPurchasePriceUsed.ForeColor = textColor2;
            txtPurchasePriceUsed.BorderColor = checkedFillColor;
            txtPurchasePriceUsed.FillColor = backgroundPrimary;

            txtlowestSellingPriceNew.BackColor = backgroundPrimary;
            txtlowestSellingPriceNew.ForeColor = textColor2;
            txtlowestSellingPriceNew.BorderColor = checkedFillColor;
            txtlowestSellingPriceNew.FillColor = backgroundPrimary;

            //////////////
            txtMinimum.BackColor = backgroundPrimary;
            txtMinimum.ForeColor = textColor2;
            txtMinimum.BorderColor = checkedFillColor;
            txtMinimum.FillColor = backgroundPrimary;

            txtRequst.BackColor = backgroundPrimary;
            txtRequst.ForeColor = textColor2;
            txtRequst.BorderColor = checkedFillColor;
            txtRequst.FillColor = backgroundPrimary;

            //////////////////////
            txtUnite1.BackColor = backgroundPrimary;
            txtUnite1.ForeColor = textColor2;
            txtUnite1.BorderColor = checkedFillColor;
            txtUnite1.FillColor = backgroundPrimary;

            txtUnite2.BackColor = backgroundPrimary;
            txtUnite2.ForeColor = textColor2;
            txtUnite2.BorderColor = checkedFillColor;
            txtUnite2.FillColor = backgroundPrimary;

            txtUnite3.BackColor = backgroundPrimary;
            txtUnite3.ForeColor = textColor2;
            txtUnite3.BorderColor = checkedFillColor;
            txtUnite3.FillColor = backgroundPrimary;

            txtUniteSell.BackColor = backgroundPrimary;
            txtUniteSell.ForeColor = textColor2;
            txtUniteSell.BorderColor = checkedFillColor;
            txtUniteSell.FillColor = backgroundPrimary;

            txtUnumber2.BackColor = backgroundPrimary;
            txtUnumber2.ForeColor = textColor2;
            txtUnumber2.BorderColor = checkedFillColor;
            txtUnumber2.FillColor = backgroundPrimary;

            txtUnumber3.BackColor = backgroundPrimary;
            txtUnumber3.ForeColor = textColor2;
            txtUnumber3.BorderColor = checkedFillColor;
            txtUnumber3.FillColor = backgroundPrimary;

            txtUprice2.BackColor = backgroundPrimary;
            txtUprice2.ForeColor = textColor2;
            txtUprice2.BorderColor = checkedFillColor;
            txtUprice2.FillColor = backgroundPrimary;

            txtUprice3.BackColor = backgroundPrimary;
            txtUprice3.ForeColor = textColor2;
            txtUprice3.BorderColor = checkedFillColor;
            txtUprice3.FillColor = backgroundPrimary;

            /////////////////
            txtBarcode1.BackColor = backgroundSecondary;
            txtBarcode1.ForeColor = textColor2;
            txtBarcode1.BorderColor = checkedFillColor;
            txtBarcode1.FillColor = backgroundPrimary;

            txtBarcode2.BackColor = backgroundSecondary;
            txtBarcode2.ForeColor = textColor2;
            txtBarcode2.BorderColor = checkedFillColor;
            txtBarcode2.FillColor = backgroundPrimary;

            txtBarcode3.BackColor = backgroundSecondary;
            txtBarcode3.ForeColor = textColor2;
            txtBarcode3.BorderColor = checkedFillColor;
            txtBarcode3.FillColor = backgroundPrimary;

            txtBarcode4.BackColor = backgroundSecondary;
            txtBarcode4.ForeColor = textColor2;
            txtBarcode4.BorderColor = checkedFillColor;
            txtBarcode4.FillColor = backgroundPrimary;

            //->Button  
            btnBrowse.FillColor = checkedFillColor;
            btnBrowse.ForeColor = textColor;

            btnCategory.FillColor = checkedFillColor;
            btnCategory.ForeColor = textColor;

            btnMaxUnit.FillColor = checkedFillColor;
            btnMaxUnit.ForeColor = textColor;

            btnMidUnit.FillColor = checkedFillColor;
            btnMidUnit.ForeColor = textColor;

            btnMinUnit.FillColor = checkedFillColor;
            btnMinUnit.ForeColor = textColor;

            btnSellUnit.FillColor = checkedFillColor;
            btnSellUnit.ForeColor = textColor;

            btnSaveUnit.FillColor = checkedFillColor;
            btnSaveUnit.ForeColor = textColor;

            btnDefult.FillColor = checkedFillColor;
            btnDefult.ForeColor = textColor;


            btnClose.FillColor = Color.Red;
            btnClose.ForeColor = textColor;

            btnPSave.FillColor = checkedFillColor;
            btnPSave.ForeColor = textColor;

            //groupBox
            groupBox1.ForeColor = textColor;
            groupBox2.ForeColor = textColor;
            gbPrices.ForeColor = textColor;
            gbUnits.ForeColor = textColor;
            groupBox5.ForeColor = textColor;


            //comboBox 
            comboBox1.ForeColor = textColor;
            comboBox1.BackColor = backgroundPrimary;
        }
        private string GenerateUniqueInvoiceCode(bool isUsed)
        {
            const string digits = "0123456789";
            Random random = new Random();
            string code;
            bool exists;

            do
            {
                // توليد كود عشوائي 14 رقم فقط
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 14; i++)
                {
                    sb.Append(digits[random.Next(digits.Length)]);
                }
                code = sb.ToString();

                // التأكد من أنه غير موجود مسبقاً في قاعدة البيانات
                using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدم GetConnection
                {
                    con.Open();

                    if (isUsed)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM products WHERE pUsedBarode = @code", con))
                        {
                            cmd.Parameters.AddWithValue("@code", code);
                            exists = (int)cmd.ExecuteScalar() > 0;
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM products WHERE pCode = @code", con))
                        {
                            cmd.Parameters.AddWithValue("@code", code);
                            exists = (int)cmd.ExecuteScalar() > 0;
                        }
                    }
                }


            } while (exists);

            // إضافة بادئة حسب نوع الكود
            if (isUsed)
                return code + "00";  // مستخدم
            else
                return code + "99";  // جديد
        }


        private void btnNext_Click(object sender, EventArgs e)
        {

            //int payParice = int.Parse(txtSellPrice.Text == string.Empty ? "0" : txtSellPrice.Text);


            //int purchasePrice = int.Parse(txtPurchasePrice.Text == string.Empty ? "0" : txtPurchasePrice.Text);
            //int profit = payParice - purchasePrice;
            //txtSemiWholesale.Text = profit.ToString();


            btnEdit.Enabled = true;
            gbPrices.Enabled = false;
            gbUnits.Enabled = true;
            btnNext.Enabled = false;
            btnEdit2.Enabled = false;

            if (!string.IsNullOrEmpty(txtUniteSell.Text) &&
            txtUniteSell.Text == txtUnite1.Text &&
            txtUniteSell.Text == txtUnite2.Text &&
            txtUniteSell.Text == txtUnite3.Text)
            {
                unteTwo();
                unteThree();
            }



        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            btnDone.Enabled = false;
            btnEdit2.Enabled = true;
            btnEdit.Enabled = true;
            gbUnits.Enabled = false;
            groupBox5.Enabled = true;
            if ((!String.IsNullOrEmpty(txtPname.Text) || !String.IsNullOrEmpty(txtCat.Text)) && !String.IsNullOrEmpty(txtMinimum.Text))
            {
                btnDone2.Enabled = true;
            }
        }

        private void txtPname_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtPname.Text.Trim();

            // 🔹 1. تحقق لو الحقل فاضي
            if (string.IsNullOrWhiteSpace(currentText))
            {
                txtPname.TextAlign = HorizontalAlignment.Left;

                lblWarning.Visible = true;
                lblWarning.Text = "⚠️ المنتج فارغ";
                lblWarning.ForeColor = Color.Red;

                txtPname.HoverState.BorderColor = Color.Red;
                txtPname.FocusedState.BorderColor = Color.Red;
                txtPname.BorderColor = Color.Red;

                gbPrices.Enabled = false;
                return;
            }

            // 🔹 2. ضبط اتجاه الكتابة حسب اللغة
            char firstChar = currentText[0];
            txtPname.TextAlign = IsArabic(firstChar) ? HorizontalAlignment.Left : HorizontalAlignment.Right;

            // 🔹 3. تفعيل gbPrices لو الحقول المطلوبة مليانة
            gbPrices.Enabled =
                !string.IsNullOrEmpty(txtPname.Text) &&
                !string.IsNullOrEmpty(txtCat.Text) &&
                !string.IsNullOrEmpty(txtMinimum.Text);

            // 🔹 4. تحقق لو المنتج موجود أو لا
            if (proNames.Any(n => n.Equals(currentText, StringComparison.OrdinalIgnoreCase)))
            {
                lblWarning.Visible = true;
                lblWarning.Text = "⚠️ المنتج موجود بالفعل";
                lblWarning.ForeColor = Color.Red;

                txtPname.HoverState.BorderColor = Color.Red;
                txtPname.FocusedState.BorderColor = Color.Red;
                txtPname.BorderColor = Color.Red;
            }
            else
            {
                lblWarning.Visible = true;
                lblWarning.Text = "✅ هذا المنتج متاح";
                lblWarning.ForeColor = Color.Green;

                txtPname.HoverState.BorderColor = Color.Green;
                txtPname.FocusedState.BorderColor = Color.Green;
                txtPname.BorderColor = Color.Green;
            }

            // 🔹 5. ضبط مكان التحذير
            lblWarning.Location = new Point(txtPname.Right - lblWarning.PreferredWidth, txtPname.Bottom + 5);
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private List<string> proNames = new List<string>();
        private async Task LoadPartyNamesAsync()
        {
            proNames.Clear();
            string query = "SELECT pName FROM products";

            try
            {
                // 🔥 تشغيل تحميل البيانات في Thread منفصل
                List<string> tempNames = await Task.Run(() =>
                {
                    List<string> names = new List<string>();

                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                names.Add(reader["pName"].ToString().Trim());
                            }
                        }
                    }

                    return names;
                });

                // 🔥 تحديث الـ UI بعد تحميل البيانات
                proNames.AddRange(tempNames);
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("خطأ", "❌ حدث خطأ أثناء تحميل اسماء المنتجات القديمة");
            }
        }


        private void txtUsedPrice_TextChanged(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtSellPrice.Text) && !string.IsNullOrEmpty(txtPurchasePrice.Text))
                 || (!string.IsNullOrEmpty(txtUsedPrice.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsed.Text)))
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnEdit2.Enabled = false;
            btnDone.Enabled = false;
            gbUnits.Enabled = false;
            gbPrices.Enabled = true;
            btnEdit3.Enabled = false;
            btnPSave.Enabled = false;

            //txtUprice2.Text = string.Empty;
            //txtUprice3.Text = string.Empty;
            //txtPriceUsed2.Text = string.Empty;
            //txtPriceUsed3.Text = string.Empty;

            if ((!string.IsNullOrEmpty(txtSellPrice.Text) && !string.IsNullOrEmpty(txtPurchasePrice.Text))
                || (!string.IsNullOrEmpty(txtUsedPrice.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsed.Text)))
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }
        }

        private void txtUsedPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPurchasePriceUsed.Focus();

            }
        }

        private void txtPurchasePriceUsed_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtlowestSellingPriceUse.Focus();

            }
        }

        private void txtUnumber2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام فقط وBackspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {

                unteTwo();
                //txtPriceUsed2.Focus();
            }
        }

        private void unteTwo()
        {
            int amount1 = int.Parse(string.IsNullOrWhiteSpace(txtUnumber2.Text) ? "1" : txtUnumber2.Text);

            // 🟢 سعر البيع الجديد
            double price = double.Parse(string.IsNullOrWhiteSpace(txtSellPrice.Text) ? "0" : txtSellPrice.Text);
            txtUprice2.Text = Math.Ceiling(price / amount1).ToString();

            // 🟢 سعر البيع المستعمل
            double usedPrice = double.Parse(string.IsNullOrWhiteSpace(txtUsedPrice.Text) ? "0" : txtUsedPrice.Text);
            txtPriceUsed2.Text = Math.Ceiling(usedPrice / amount1).ToString();

            // 🟢 سعر الشراء الجديد
            double purPrice = double.Parse(string.IsNullOrWhiteSpace(txtPurchasePrice.Text) ? "0" : txtPurchasePrice.Text);
            txtPurchasePriceU2.Text = Math.Ceiling(purPrice / amount1).ToString();

            // 🟢 سعر الشراء المستعمل
            double purUsedPrice = double.Parse(string.IsNullOrWhiteSpace(txtPurchasePriceUsed.Text) ? "0" : txtPurchasePriceUsed.Text);
            txtPurchasePriceUsedU2.Text = Math.Ceiling(purUsedPrice / amount1).ToString();

            // 🟢 أسعار الجملة
            double wholesale = double.Parse(string.IsNullOrWhiteSpace(txtWholesale.Text) ? "0" : txtWholesale.Text);
            txtWholesaleU2.Text = Math.Ceiling(wholesale / amount1).ToString();

            double semiWholesale = double.Parse(string.IsNullOrWhiteSpace(txtSemiWholesale.Text) ? "0" : txtSemiWholesale.Text);
            txtSemiWholesaleU2.Text = Math.Ceiling(semiWholesale / amount1).ToString();

            double wholesaleUse = double.Parse(string.IsNullOrWhiteSpace(txtWholesaleUse.Text) ? "0" : txtWholesaleUse.Text);
            txtWholesaleUseU2.Text = Math.Ceiling(wholesaleUse / amount1).ToString();

            double semiWholesaleUse = double.Parse(string.IsNullOrWhiteSpace(txtSemiWholesaleUse.Text) ? "0" : txtSemiWholesaleUse.Text);
            txtSemiWholesaleUseU2.Text = Math.Ceiling(semiWholesaleUse / amount1).ToString();

            // 🟢 أقل سعر بيع
            double lowestNew = double.Parse(string.IsNullOrWhiteSpace(txtlowestSellingPriceNew.Text) ? "0" : txtlowestSellingPriceNew.Text);
            txtlowestSellingPriceNewU2.Text = Math.Ceiling(lowestNew / amount1).ToString();

            double lowestUsed = double.Parse(string.IsNullOrWhiteSpace(txtlowestSellingPriceUse.Text) ? "0" : txtlowestSellingPriceUse.Text);
            txtlowestSellingPriceUseU2.Text = Math.Ceiling(lowestUsed / amount1).ToString();
        }

        private void txtUnite1_TextChanged(object sender, EventArgs e)
        {
            if (
                // الشروط الأساسية (لازم كلها تتحقق)
                !string.IsNullOrEmpty(txtUnite1.Text) &&
                !string.IsNullOrEmpty(txtUnite2.Text) &&
                !string.IsNullOrEmpty(txtUnite3.Text) &&
                !string.IsNullOrWhiteSpace(txtUnumber2.Text) &&
                !string.IsNullOrEmpty(txtUnumber3.Text) &&
                !string.IsNullOrEmpty(txtUniteSell.Text) &&

                // شرط Unite 2 (واحدة من الحالتين لازم تتحقق)
                (
                    (!string.IsNullOrEmpty(txtUprice2.Text) && !string.IsNullOrEmpty(txtPurchasePriceU2.Text)) ||
                    (!string.IsNullOrEmpty(txtPriceUsed2.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsedU2.Text))
                ) &&

                // شرط Unite 3 (واحدة من الحالتين لازم تتحقق)
                (
                    (!string.IsNullOrEmpty(txtUprice3.Text) && !string.IsNullOrEmpty(txtPurchasePriceU3.Text)) ||
                    (!string.IsNullOrEmpty(txtPriceUsed3.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsedU3.Text))
                )
            )
            {
                btnDone.Enabled = true;
            }
            else
                btnDone.Enabled = false;

        }

        private void txtWholesale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtUsedPrice.Focus();

            }
        }
        PrintDocument printDoc = new PrintDocument();
        private void btnPrintNew_Click(object sender, EventArgs e)
        {
            int parNewNum = int.Parse(txtNumNewBar.Text == string.Empty ? "1" : txtNumNewBar.Text);
            for (int i = 0; i < parNewNum; i++)
            {
                isUsed = false;
                PrintBarcodes();
            }

        }

        public void PrintBarcodes()
        {

            try
            {
                printDoc.PrinterSettings.PrinterName = MainClass.BarcodePrinter;
                PaperSize paperSize = new PaperSize("Custom", 260, 98);
                printDoc.DefaultPageSettings.PaperSize = paperSize;
                printDoc.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);

                printDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الطباعة: " + ex.Message);
            }

        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var barGenerator = new generatBarCode();
            int barcodeWidth = 230;
            int barcodeHeight = 40; // ⬅️ قللت الارتفاع من 60 لـ 40

            int x = (e.PageBounds.Width - barcodeWidth) / 2;
            int x2 = (e.PageBounds.Width - 260) / 2;

            int y = 10;

            // اسم المنتج (فوق الباركود في النص)
            RectangleF nameRect = new RectangleF(x - 26, y, barcodeWidth, 20);
            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            centerFormat.LineAlignment = StringAlignment.Center;
            centerFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            e.Graphics.DrawString(MainClass.CompanyName,
                new Font("Arial", 10, FontStyle.Bold), Brushes.Black, nameRect, centerFormat);

            y += 15;

            // ⬅️ رسم الباركود بالارتفاع الجديد
            if (isUsed)
            {
                barcodeUsedImag = barGenerator.CreateBarCode(UsedBarode);
                e.Graphics.DrawImage(barcodeUsedImag, new Rectangle(x - 21, y, barcodeWidth, barcodeHeight));
            }
            else
            {
                barcodeNewImag = barGenerator.CreateBarCode(NewBarode);
                e.Graphics.DrawImage(barcodeNewImag, new Rectangle(x - 21, y, barcodeWidth, barcodeHeight));
            }

            y += barcodeHeight - 15;

            int productWidth = 170;

            // احسب X جديد علشان يبقى في النص
            int productX = (e.PageBounds.Width - productWidth) / 2;

            RectangleF nameRect2 = new RectangleF(productX - 23, y, productWidth, 40);
            StringFormat centerFormat2 = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.DirectionRightToLeft,
                Trimming = StringTrimming.Word
            };

            e.Graphics.DrawString(
                txtPname.Text,
                new Font("Arial", 9, FontStyle.Regular),
                Brushes.Black,
                nameRect2,
                centerFormat2
            );

            e.HasMorePages = false;
        }



        private void btnPrintUse_Click(object sender, EventArgs e)
        {
            // تحقق هل القيمة رقم صحيح؟
            if (!int.TryParse(txtNumUseBar.Text, out int paUseNum))
            {
                MessageBox.Show("من فضلك أدخل رقم صحيح لعدد النسخ", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // وقف التنفيذ
            }

            // لو الرقم أقل من 1
            if (paUseNum < 1)
            {
                MessageBox.Show("عدد النسخ يجب أن يكون رقم أكبر من صفر", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < paUseNum; i++)
            {
                isUsed = true;
                PrintBarcodes();
            }
        }


        private void txtlowestSellingPrice_TextChanged(object sender, EventArgs e)
        {

        }
        private double percentNew = 0;
        private double percentUse = 0;

        private void txtlowestSellingPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (double.TryParse(txtSellPrice.Text, out double payPrice) &&
                    double.TryParse(txtlowestSellingPriceNew.Text, out double lowestPrice))
                {
                    double discount = CalculateDiscount(payPrice, lowestPrice);
                    percentNew = Math.Round(discount, 2);
                }
                else
                {
                    percentNew = 0; // 🔹 في حالة إدخال خاطئ
                }

                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private double CalculateDiscount(double payPrice, double lowestPrice)
        {
            if (payPrice <= 0) return 0; // 🔹 تجنب القسمة على صفر
            double discount = (1 - (lowestPrice / payPrice)) * 100;
            return Math.Round(discount, 2); // 🔹 رقمين عشريين
        }

        private double CalculateLowestPrice(double payPrice, double discount)
        {
            if (payPrice <= 0) return 0; // تجنب القسمة على صفر
            double lowestPrice = payPrice * (1 - discount / 100.0);
            return Math.Round(lowestPrice, 2); // 🔹 رقمين عشريين
        }

        private void txtHdisc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (double.TryParse(txtUsedPrice.Text, out double payPrice) &&
                    double.TryParse(txtlowestSellingPriceUse.Text, out double lowestPrice))
                {
                    double discount = CalculateDiscount(payPrice, lowestPrice);
                    percentUse = Math.Round(discount, 2);
                }
                else
                {
                    percentNew = 0; // 🔹 في حالة إدخال خاطئ
                }

                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private void txtcompName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtcompName.Text))
            {
                txtcompName.TextAlign = HorizontalAlignment.Left;
                return;

            }
            char firstChar = txtcompName.Text[0];

            if (IsArabic(firstChar))
                txtcompName.TextAlign = HorizontalAlignment.Left;
            else
                txtcompName.TextAlign = HorizontalAlignment.Right;
            if (!String.IsNullOrEmpty(txtPname.Text) && !String.IsNullOrEmpty(txtCat.Text))
            {
                gbPrices.Enabled = true;
            }
        }

        private void txtMinimum_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtPname.Text) && !String.IsNullOrEmpty(txtCat.Text))
            {
                gbPrices.Enabled = true;
            }
        }


        private void txtNumNewBar_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true; // يمنع الكتابة
            //}
        }

        private void txtlowestSellingPriceUse_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(txtUsedPrice.Text, out double payPrice) &&
                   double.TryParse(txtlowestSellingPriceUse.Text, out double lowestPrice))
            {
                double discount = CalculateDiscount(payPrice, lowestPrice);
                percentUse = Math.Round(discount, 2);
            }
            else
            {
                percentNew = 0; // 🔹 في حالة إدخال خاطئ
            }
            if ((!string.IsNullOrEmpty(txtSellPrice.Text) && !string.IsNullOrEmpty(txtPurchasePrice.Text))
                || (!string.IsNullOrEmpty(txtUsedPrice.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsed.Text)))
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }
        }

        private void txtlowestSellingPriceNew_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(txtSellPrice.Text, out double payPrice) &&
                  double.TryParse(txtlowestSellingPriceNew.Text, out double lowestPrice))
            {
                double discount = CalculateDiscount(payPrice, lowestPrice);
                percentNew = Math.Round(discount, 2);
            }
            else
            {
                percentNew = 0; // 🔹 في حالة إدخال خاطئ
            }
            if ((!string.IsNullOrEmpty(txtSellPrice.Text) && !string.IsNullOrEmpty(txtPurchasePrice.Text))
                || (!string.IsNullOrEmpty(txtUsedPrice.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsed.Text)))
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }
        }

        private void btnNoSmallUnit_Click(object sender, EventArgs e)
        {
            txtUnite3.Text = txtUnite2.Text;
            UnitID3 = UnitID2;

            // الكمية
            txtUnumber3.Text = "1";

            // أسعار البيع
            txtUprice3.Text = txtUprice2.Text;
            txtPriceUsed3.Text = txtPriceUsed2.Text;

            // اسعار الشراء
            txtPurchasePriceUsedU3.Text = txtPurchasePriceUsedU2.Text;
            txtPurchasePriceU3.Text = txtPurchasePriceU2.Text;

            // أسعار الجملة
            txtWholesaleU3.Text = txtWholesaleU2.Text;
            txtSemiWholesaleU3.Text = txtSemiWholesaleU2.Text;
            txtWholesaleUseU3.Text = txtWholesaleUseU2.Text;
            txtSemiWholesaleUseU3.Text = txtSemiWholesaleUseU2.Text;

            // أقل سعر بيع
            txtlowestSellingPriceNewU3.Text = txtlowestSellingPriceNewU2.Text;
            txtlowestSellingPriceUseU3.Text = txtlowestSellingPriceUseU2.Text;
        }

        private void txtWholesaleUse_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.' || txtlowestSellingPriceUse.Text.Contains(".")))
            {
                e.Handled = true;
            }
        }

        private double percentU2 = 0;
        private double percentUseU2 = 0;
        private double percentU3 = 0;
        private double percentUseU3 = 0;
        private void txtlowestSellingPriceNewU2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtlowestSellingPriceNewU2_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(txtUprice2.Text, out double payPrice) &&
                   double.TryParse(txtlowestSellingPriceNewU2.Text, out double lowestPrice))
            {
                double discount = CalculateDiscount(payPrice, lowestPrice);
                percentU2 = Math.Round(discount, 2);
            }
            else
            {
                percentU2 = 0; // 🔹 في حالة إدخال خاطئ
            }
        }

        private void txtlowestSellingPriceUseU2_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(txtPriceUsed2.Text, out double payPrice) &&
                   double.TryParse(txtlowestSellingPriceUseU2.Text, out double lowestPrice))
            {
                double discount = CalculateDiscount(payPrice, lowestPrice);
                percentUseU2 = Math.Round(discount, 2);
            }
            else
            {
                percentUseU2 = 0; // 🔹 في حالة إدخال خاطئ
            }
        }

        private void txtlowestSellingPriceNewU3_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(txtUprice3.Text, out double payPrice) &&
                  double.TryParse(txtlowestSellingPriceNewU3.Text, out double lowestPrice))
            {
                double discount = CalculateDiscount(payPrice, lowestPrice);
                percentU3 = Math.Round(discount, 2);
            }
            else
            {
                percentU3 = 0; // 🔹 في حالة إدخال خاطئ
            }
        }

        private void txtlowestSellingPriceUseU3_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(txtPriceUsed3.Text, out double payPrice) &&
                 double.TryParse(txtlowestSellingPriceUseU3.Text, out double lowestPrice))
            {
                double discount = CalculateDiscount(payPrice, lowestPrice);
                percentUseU3 = Math.Round(discount, 2);
            }
            else
            {
                percentUseU3 = 0; // 🔹 في حالة إدخال خاطئ
            }
        }

        private void txtSemiWholesaleU2_Leave(object sender, EventArgs e)
        {
            if (
               // الشروط الأساسية (لازم كلها تتحقق)
               !string.IsNullOrEmpty(txtUnite1.Text) &&
               !string.IsNullOrEmpty(txtUnite2.Text) &&
               !string.IsNullOrEmpty(txtUnite3.Text) &&
               !string.IsNullOrWhiteSpace(txtUnumber2.Text) &&
               !string.IsNullOrEmpty(txtUnumber3.Text) &&
               !string.IsNullOrEmpty(txtUniteSell.Text) &&

               // شرط Unite 2 (واحدة من الحالتين لازم تتحقق)
               (
                   (!string.IsNullOrEmpty(txtUprice2.Text) && !string.IsNullOrEmpty(txtPurchasePriceU2.Text)) ||
                   (!string.IsNullOrEmpty(txtPriceUsed2.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsedU2.Text))
               ) &&

               // شرط Unite 3 (واحدة من الحالتين لازم تتحقق)
               (
                   (!string.IsNullOrEmpty(txtUprice3.Text) && !string.IsNullOrEmpty(txtPurchasePriceU3.Text)) ||
                   (!string.IsNullOrEmpty(txtPriceUsed3.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsedU3.Text))
               )
           )
            {
                btnDone.Enabled = true;
            }
            else
                btnDone.Enabled = false;

        }

        private void btnEdit2_Click(object sender, EventArgs e)
        {
            btnEdit2.Enabled = false;
            btnEdit.Enabled = false;
            btnEdit3.Enabled = false;
            gbPrices.Enabled = false;
            gbUnits.Enabled = true;
            btnDone.Enabled = true;
            btnPSave.Enabled = false;


            if (
              // الشروط الأساسية (لازم كلها تتحقق)
              !string.IsNullOrEmpty(txtUnite1.Text) &&
              !string.IsNullOrEmpty(txtUnite2.Text) &&
              !string.IsNullOrEmpty(txtUnite3.Text) &&
              !string.IsNullOrWhiteSpace(txtUnumber2.Text) &&
              !string.IsNullOrEmpty(txtUnumber3.Text) &&
              !string.IsNullOrEmpty(txtUniteSell.Text) &&

              // شرط Unite 2 (واحدة من الحالتين لازم تتحقق)
              (
                  (!string.IsNullOrEmpty(txtUprice2.Text) && !string.IsNullOrEmpty(txtPurchasePriceU2.Text)) ||
                  (!string.IsNullOrEmpty(txtPriceUsed2.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsedU2.Text))
              ) &&

              // شرط Unite 3 (واحدة من الحالتين لازم تتحقق)
              (
                  (!string.IsNullOrEmpty(txtUprice3.Text) && !string.IsNullOrEmpty(txtPurchasePriceU3.Text)) ||
                  (!string.IsNullOrEmpty(txtPriceUsed3.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsedU3.Text))
              )
          )
            {
                btnDone.Enabled = true;
            }
            else
                btnDone.Enabled = false;
        }

        private void btnPrintNumber_Click(object sender, EventArgs e)
        {
            int parNewNum = int.Parse(txtNumNewBar.Text == string.Empty ? "1" : txtNumNewBar.Text);
            for (int i = 0; i < parNewNum; i++)
            {
                int num = i + 1;
                PrintText(num.ToString()); // تقدر تغيّر النص لأي حاجة
            }

        }
        public void PrintText(string text)
        {
            try
            {
                PrintDocument doc = new PrintDocument();
                doc.PrinterSettings.PrinterName = MainClass.BarcodePrinter;
                PaperSize paperSize = new PaperSize("Custom", 260, 98);
                doc.DefaultPageSettings.PaperSize = paperSize;
                doc.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);

                doc.PrintPage += (sender, e) =>
                {
                    StringFormat centerFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    RectangleF rect = e.MarginBounds;
                    rect.Offset(-22, 0);
                    int fonte = int.TryParse(cbFonte.Text, out int f) ? f : 40;
                    using (Font bigFont = new Font("Arial", fonte, FontStyle.Bold))
                    {
                        e.Graphics.DrawString(text, bigFont, Brushes.Black, rect, centerFormat);
                    }

                    e.HasMorePages = false;
                };

                doc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الطباعة: " + ex.Message);
            }
        }

        private void btnPrintNumberOne_Click(object sender, EventArgs e)
        {
            PrintText(txtNumUseBar.Text); // تقدر تغيّر النص لأي حاجة

        }

        private void txtSmaillNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtSmaillUsed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }
        private System.Windows.Forms.Timer inputTimer = new System.Windows.Forms.Timer();

        private double qtyU3;
        private double qtyU2;
        private double qtyU1;
        private void txtSmaillNew_TextChanged(object sender, EventArgs e)
        {
            inputTimer.Tick -= InputTimerUsed_Tick; // ربط الحدث

            inputTimer.Tick += InputTimerNew_Tick; // ربط الحدث
            inputTimer.Stop();  // كل مرة المستخدم يكتب، نوقف المؤقت
            inputTimer.Start(); // ونشغله من جديد

            if ((!String.IsNullOrEmpty(txtPname.Text) || !String.IsNullOrEmpty(txtCat.Text)) && !String.IsNullOrEmpty(txtMinimum.Text))
            {
                btnDone2.Enabled = true;
            }
        }

        private void txtSmaillUsed_TextChanged(object sender, EventArgs e)
        {
            inputTimer.Tick -= InputTimerNew_Tick; // ربط الحدث

            inputTimer.Tick += InputTimerUsed_Tick; // ربط الحدث
            inputTimer.Stop();  // كل مرة المستخدم يكتب، نوقف المؤقت
            inputTimer.Start(); // ونشغله من جديد

            if ((!String.IsNullOrEmpty(txtPname.Text) || !String.IsNullOrEmpty(txtCat.Text)) && !String.IsNullOrEmpty(txtMinimum.Text))
            {
                btnDone2.Enabled = true;
            }
        }
        private void InputTimerUsed_Tick(object sender, EventArgs e)
        {
            inputTimer.Stop(); // وقف المؤقت لأنه خلص

            // 📥 قراءة الرقم بأمان
            int qty = 0;
            int.TryParse(txtSmaillUsed.Text, out qty);

            // 🔢 حساب الوحدات
            SetProductUnitInfo(qty);
            txtMiduamUsed.Text = qtyU2.ToString("F1");
            txtLargUsed.Text = qtyU1.ToString("F1");
        }
        private void InputTimerNew_Tick(object sender, EventArgs e)
        {
            inputTimer.Stop(); // وقف المؤقت لأنه خلص

            // 📥 قراءة الرقم بأمان
            int qty = 0;
            int.TryParse(txtSmaillNew.Text, out qty);

            // 🔢 حساب الوحدات
            SetProductUnitInfo(qty);
            txtMiduamNew.Text = qtyU2.ToString("F1");
            txtLargNew.Text = qtyU1.ToString("F1");
        }

        private void SetProductUnitInfo(double newQty = 0)
        {
            // قراءة القيم بشكل آمن
            int numberU2 = 0;
            int numberU3 = 0;

            int.TryParse(txtUnumber2.Text, out numberU2);
            int.TryParse(txtUnumber3.Text, out numberU3);

            qtyU3 = newQty;

            // 2️⃣ حساب الكميات بوحدات مختلفة (مع التحقق من القسمة على صفر)
            if (numberU3 > 0)
                qtyU2 = qtyU3 / numberU3;
            else
                qtyU2 = 0;

            if (numberU2 > 0)
                qtyU1 = qtyU2 / numberU2;
            else
                qtyU1 = 0;
        }

        private void UpdateProduct()
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string query = @"
                    UPDATE [smartpos].[dbo].[totalStor]
                    SET qtyU1 = @qtyU1,
                        qtyU2 = @qtyU2,
                        qtyU3 = @qtyU3,
                        qtyUsedU1 = @qtyUsedU1,
                        qtyUsedU2 = @qtyUsedU2,
                        qtyUsedU3 = @qtyUsedU3
                    WHERE pID = @pID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pID", id);
                    cmd.Parameters.AddWithValue("@qtyU1", Convert.ToDecimal(txtLargNew.Text == string.Empty ? "0" : txtLargNew.Text));
                    cmd.Parameters.AddWithValue("@qtyU2", Convert.ToDecimal(txtMiduamNew.Text == string.Empty ? "0" : txtMiduamNew.Text));
                    cmd.Parameters.AddWithValue("@qtyU3", Convert.ToDecimal(txtSmaillNew.Text == string.Empty ? "0" : txtSmaillNew.Text));
                    cmd.Parameters.AddWithValue("@qtyUsedU1", Convert.ToDecimal(txtLargUsed.Text == string.Empty ? "0" : txtLargUsed.Text));
                    cmd.Parameters.AddWithValue("@qtyUsedU2", Convert.ToDecimal(txtMiduamUsed.Text == string.Empty ? "0" : txtMiduamUsed.Text));
                    cmd.Parameters.AddWithValue("@qtyUsedU3", Convert.ToDecimal(txtSmaillUsed.Text == string.Empty ? "0" : txtSmaillUsed.Text));

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnDone2_Click(object sender, EventArgs e)
        {
            groupBox5.Enabled = false;
            btnDone2.Enabled = false;
            btnPSave.Enabled = true;
            btnEdit.Enabled = true;
            btnEdit2.Enabled = true;
            btnEdit3.Enabled = true;

        }

        private void btnEdit3_Click(object sender, EventArgs e)
        {
            btnDone2.Enabled = false;
            groupBox5.Enabled = true;
            btnEdit.Enabled = false;
            btnEdit2.Enabled = false;
            btnPSave.Enabled = false;

            if ((!String.IsNullOrEmpty(txtPname.Text) || !String.IsNullOrEmpty(txtCat.Text)) && !String.IsNullOrEmpty(txtMinimum.Text))
            {
                btnDone2.Enabled = true;
            }
        }

        private void txtSemiWholesale_Leave(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtSellPrice.Text) && !string.IsNullOrEmpty(txtPurchasePrice.Text))
                || (!string.IsNullOrEmpty(txtUsedPrice.Text) && !string.IsNullOrEmpty(txtPurchasePriceUsed.Text)))
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }
        }

        private int ID;
        private int PID;
        private decimal QtyU1;
        private decimal QtyU2;
        private decimal QtyU3;
        private decimal QtyUsedU1;
        private decimal QtyUsedU2;
        private decimal QtyUsedU3;

        private void GetProductByPID()
        {

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string query = @"SELECT [ID],[pID],[qtyU1],[qtyU2],[qtyU3],
                                        [qtyUsedU1],[qtyUsedU2],[qtyUsedU3]
                                 FROM [smartpos].[dbo].[totalStor]
                                 WHERE pID = @pID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pID", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0;
                            PID = reader["pID"] != DBNull.Value ? Convert.ToInt32(reader["pID"]) : 0;
                            QtyU1 = reader["qtyU1"] != DBNull.Value ? Convert.ToDecimal(reader["qtyU1"]) : 0;
                            QtyU2 = reader["qtyU2"] != DBNull.Value ? Convert.ToDecimal(reader["qtyU2"]) : 0;
                            QtyU3 = reader["qtyU3"] != DBNull.Value ? Convert.ToDecimal(reader["qtyU3"]) : 0;
                            QtyUsedU1 = reader["qtyUsedU1"] != DBNull.Value ? Convert.ToDecimal(reader["qtyUsedU1"]) : 0;
                            QtyUsedU2 = reader["qtyUsedU2"] != DBNull.Value ? Convert.ToDecimal(reader["qtyUsedU2"]) : 0;
                            QtyUsedU3 = reader["qtyUsedU3"] != DBNull.Value ? Convert.ToDecimal(reader["qtyUsedU3"]) : 0;
                        }
                    }
                }
            }
            txtSmaillNew.Text = QtyU3.ToString();
            txtMiduamNew.Text = QtyU2.ToString();
            txtLargNew.Text = QtyU1.ToString();

            txtSmaillUsed.Text = QtyUsedU3.ToString();
            txtMiduamUsed.Text = QtyUsedU2.ToString();
            txtLargUsed.Text = QtyUsedU1.ToString();

        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        private static bool IsValidExpression(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (Regex.IsMatch(input, @"[\+\-\*/]{2,}"))
                return false;

            if (!Regex.IsMatch(input, @"^\d+(\.\d+)?([\+\-\*/]\d+(\.\d+)?)*$"))
                return false;

            if (Regex.IsMatch(input, @"/0+(\D|$)"))
                return false;

            return true;
        }
        public void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox txt = sender as Guna.UI2.WinForms.Guna2TextBox;
            string text = txt.Text;

            // السماح بالأرقام و Backspace
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                return;

            // السماح بعلامات العمليات لمرة واحدة بين الأرقام فقط
            if (e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '*' || e.KeyChar == '/')
            {
                if (string.IsNullOrEmpty(text) || Regex.IsMatch(text, @"[\+\-\*/]$"))
                {
                    e.Handled = true;
                    return;
                }
                return;
            }

            // السماح بالنقطة العشرية لمرة واحدة
            if (e.KeyChar == '.')
            {
                if (text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }
                return;
            }

            // منع أي رموز أخرى
            e.Handled = true;
        }

        // دالة KeyDown عامة لأي TextBox
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Guna.UI2.WinForms.Guna2TextBox txt = sender as Guna.UI2.WinForms.Guna2TextBox;
                string input = txt.Text.Trim();

                if (IsValidExpression(input))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        var result = dt.Compute(input, "");
                        txt.Text = result.ToString();
                    }
                    catch (DivideByZeroException)
                    {
                        MessageBox.Show("لا يمكن القسمة على صفر!");
                        txt.Clear();
                    }
                    catch
                    {
                        MessageBox.Show("التعبير غير صحيح!");
                        txt.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("التعبير يحتوي على أخطاء!");
                    txt.Clear();
                }
            }
        }

        private async void btnSync_Click(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = true;
            btnSync.Enabled = false;
            btnPSave.Enabled = false;
            btnExite.Enabled = false;
            mainPanel.Enabled = false;

            try
            {
                await Task.Run(() => RunSync());

                Notifier.ShowNotification("Done ✅", "✅ تمت مزامنة البيانات في الاتجاهين بنجاح!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ خطأ أثناء المزامنة:\n" + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
                btnPSave.Enabled = true;
                btnExite.Enabled = true;
                mainPanel.Enabled = true;
            }
        }
        private string GetMasterConnectionString()
        {
            DBConfig config = DBConfig.Load();

            string serverName = config.Server;
            string databaseName = config.Database; // هتستخدمه لاحقاً مش في الكونكشن نفسه
            string dbUserName = config.User;
            string decrypted = DecryptText(config.Password);


            // إزالة BOM إذا موجود
            if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                decrypted = decrypted.Substring(1);

            // إزالة أي whitespace إضافية
            string dbPassword = decrypted.Trim();

            bool sqlAuthentication = config.sqlAuthentication;

            if (sqlAuthentication)
            {
                // SQL Auth → اتصل بـ master
                return $"Server={serverName};Database=master;User Id={dbUserName};Password={dbPassword};";
            }
            else
            {
                // Windows Auth
                return $"Server={serverName};Database=master;Integrated Security=True;";
            }
        }
        private void RunSync()
        {
            string connectionString = GetMasterConnectionString();

            string sqlScript = @"
            -------------------- 🔁 SYNC PRODUCTS BOTH WAYS --------------------
            SET IDENTITY_INSERT [smartposAli].[dbo].[products] ON;
            MERGE [smartposAli].[dbo].[products] AS target
            USING [smartpos].[dbo].[products] AS source
            ON target.[pID] = source.[pID]
            WHEN MATCHED THEN
                UPDATE SET
                    target.[pName] = source.[pName],
                    target.[pCode] = source.[pCode],
                    target.[pNewBarode] = source.[pNewBarode],
                    target.[pUsedBarode] = source.[pUsedBarode],
                    target.[compName] = source.[compName],
                    target.[categoryID] = source.[categoryID]
            WHEN NOT MATCHED BY TARGET THEN
                INSERT ([pID], [pName], [pCode], [pNewBarode], [pUsedBarode], [compName], [categoryID])
                VALUES (source.[pID], source.[pName], source.[pCode], source.[pNewBarode], source.[pUsedBarode], source.[compName], source.[categoryID]);
            SET IDENTITY_INSERT [smartposAli].[dbo].[products] OFF;

            SET IDENTITY_INSERT [smartpos].[dbo].[products] ON;
            MERGE [smartpos].[dbo].[products] AS target
            USING [smartposAli].[dbo].[products] AS source
            ON target.[pID] = source.[pID]
            WHEN MATCHED THEN
                UPDATE SET
                    target.[pName] = source.[pName],
                    target.[pCode] = source.[pCode],
                    target.[pNewBarode] = source.[pNewBarode],
                    target.[pUsedBarode] = source.[pUsedBarode],
                    target.[compName] = source.[compName],
                    target.[categoryID] = source.[categoryID]
            WHEN NOT MATCHED BY TARGET THEN
                INSERT ([pID], [pName], [pCode], [pNewBarode], [pUsedBarode], [compName], [categoryID])
                VALUES (source.[pID], source.[pName], source.[pCode], source.[pNewBarode], source.[pUsedBarode], source.[compName], source.[categoryID]);
            SET IDENTITY_INSERT [smartpos].[dbo].[products] OFF;

            -------------------- 🔁 SYNC UNITS BOTH WAYS --------------------
            SET IDENTITY_INSERT [smartposAli].[dbo].[untits] ON;
            MERGE [smartposAli].[dbo].[untits] AS target
            USING [smartpos].[dbo].[untits] AS source
            ON target.[uID] = source.[uID]
            WHEN MATCHED THEN UPDATE SET target.[uName] = source.[uName]
            WHEN NOT MATCHED BY TARGET THEN INSERT ([uID], [uName]) VALUES (source.[uID], source.[uName]);
            SET IDENTITY_INSERT [smartposAli].[dbo].[untits] OFF;

            SET IDENTITY_INSERT [smartpos].[dbo].[untits] ON;
            MERGE [smartpos].[dbo].[untits] AS target
            USING [smartposAli].[dbo].[untits] AS source
            ON target.[uID] = source.[uID]
            WHEN MATCHED THEN UPDATE SET target.[uName] = source.[uName]
            WHEN NOT MATCHED BY TARGET THEN INSERT ([uID], [uName]) VALUES (source.[uID], source.[uName]);
            SET IDENTITY_INSERT [smartpos].[dbo].[untits] OFF;

            -------------------- 🔁 SYNC CATEGORY BOTH WAYS --------------------
            SET IDENTITY_INSERT [smartposAli].[dbo].[category] ON;
            MERGE [smartposAli].[dbo].[category] AS target
            USING [smartpos].[dbo].[category] AS source
            ON target.[catID] = source.[catID]
            WHEN MATCHED THEN UPDATE SET target.[catName] = source.[catName]
            WHEN NOT MATCHED BY TARGET THEN INSERT ([catID], [catName]) VALUES (source.[catID], source.[catName]);
            SET IDENTITY_INSERT [smartposAli].[dbo].[category] OFF;

            SET IDENTITY_INSERT [smartpos].[dbo].[category] ON;
            MERGE [smartpos].[dbo].[category] AS target
            USING [smartposAli].[dbo].[category] AS source
            ON target.[catID] = source.[catID]
            WHEN MATCHED THEN UPDATE SET target.[catName] = source.[catName]
            WHEN NOT MATCHED BY TARGET THEN INSERT ([catID], [catName]) VALUES (source.[catID], source.[catName]);
            SET IDENTITY_INSERT [smartpos].[dbo].[category] OFF;

            -------------------- 🔁 SYNC TOTALSTOR BOTH WAYS --------------------
            SET IDENTITY_INSERT [smartposAli].[dbo].[totalStor] ON;
            MERGE [smartposAli].[dbo].[totalStor] AS target
            USING [smartpos].[dbo].[totalStor] AS source
            ON target.[ID] = source.[ID]
            WHEN MATCHED THEN
                UPDATE SET target.[pID] = source.[pID]
            WHEN NOT MATCHED BY TARGET THEN
                INSERT ([ID], [pID], [qtyU1], [qtyU2], [qtyU3], [qtyUsedU1], [qtyUsedU2], [qtyUsedU3])
                VALUES (source.[ID], source.[pID], 0, 0, 0, 0, 0, 0);
            SET IDENTITY_INSERT [smartposAli].[dbo].[totalStor] OFF;

            SET IDENTITY_INSERT [smartpos].[dbo].[totalStor] ON;
            MERGE [smartpos].[dbo].[totalStor] AS target
            USING [smartposAli].[dbo].[totalStor] AS source
            ON target.[ID] = source.[ID]
            WHEN MATCHED THEN
                UPDATE SET target.[pID] = source.[pID]
            WHEN NOT MATCHED BY TARGET THEN
                INSERT ([ID], [pID], [qtyU1], [qtyU2], [qtyU3], [qtyUsedU1], [qtyUsedU2], [qtyUsedU3])
                VALUES (source.[ID], source.[pID], 0, 0, 0, 0, 0, 0);
            SET IDENTITY_INSERT [smartpos].[dbo].[totalStor] OFF;
            ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlScript, conn);
                cmd.CommandTimeout = 300; // لو البيانات كتيرة
                cmd.ExecuteNonQuery();
            }
        }
        public static string DecryptText(string encryptedText)
        {
            byte[] key = KeyManager.GetOrCreateKey();
            string decrypted = AesEncryption.Decrypt(encryptedText, key);

            // إزالة BOM إذا موجود
            if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                decrypted = decrypted.Substring(1);

            // إزالة أي whitespace إضافية
            decrypted = decrypted.Trim();

            return decrypted;
        }
        public void resultSearch(string pName, int catid)
        {
            txtCat.Text = pName;
            catID = catid;
        }

        private void btnWithoutDis_Click(object sender, EventArgs e)
        {
            txtlowestSellingPriceNew.Text = txtWholesale.Text = txtSemiWholesale.Text = txtSellPrice.Text;
            txtlowestSellingPriceUse.Text = txtWholesaleUse.Text = txtSemiWholesaleUse.Text = txtUsedPrice.Text;
        }
    }
}
