using DevExpress.CodeParser;
using DevExpress.DataAccess.Sql;
using DevExpress.Pdf.Xmp;
using DevExpress.Printing.Utils.DocumentStoring;
using DevExpress.XtraBars.Customization;
using DevExpress.XtraCharts.Designer.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraMap.ItemEditor;
using DevExpress.XtraReports.UI;
using Guna.UI2.WinForms;
using pos.Classes;
using pos.GeneralForms;
using pos.GeneralForms.MainForm;
using pos.Model.POS;
using pos.Model.Stor;
using pos.View;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using System.Xml.Linq;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace pos.Model
{
    public partial class frmProductAdd2 : Form
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        public int id = 0;
        public bool shortfalls = false;
        public int DetailID = 0;
        public int billID = 0;
        private string typeEdit;
        public event EventHandler ButtonClicked;
        Byte[] imageByteArray;
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        private int selectedPartyID;
        //private frmpurchasesBill mainfrm;

        public event EventHandler ButtonHide;

        private static frmProductAdd2 instance;
        private int storeID;
        private int supplierID;
        public static frmProductAdd2 Instance
        {
            get
            {
                if (instance == null)
                    instance = new frmProductAdd2();
                return instance;

            }
        }

        public int cID = 0;
        public int sID = 0;
        public string pName = string.Empty;
        public bool Add = false;
        public bool bill = false;
        int pID = 0;

        frmProductView parirFrm;
        public frmProductAdd2(frmProductView frm)
        {
            InitializeComponent();
            dgvProductes.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(guna2DataGridView2_EditingControlShowing);
            this.KeyPreview = true;
            this.dgvProductes.RowsAdded += new DataGridViewRowsAddedEventHandler(myDataGridView_RowsAdded);
            dgvProductes.RowsRemoved += (sender, e) => ReindexColumn();
            this.dgvProductes.CellValueChanged += new DataGridViewCellEventHandler(myDataGridView_CellValueChanged);
            dgvProductes.EditingControlShowing += guna2DataGridView2_EditingControlShowing;

            this.ShowInTaskbar = false;

            // تغيير خصائص النافذة لمنع ظهورها في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            showName();
            //ThemeMode();

            this.parirFrm = frm;
            textSuggester();

            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);
            ApplyGridStyle(dgvProductes);

        }
        public frmProductAdd2()
        {
            InitializeComponent();
            ApplyGridStyle(dgvProductes);
            dgvProductes.CellPainting -= dgvProductes_CellPainting;

            //dgvProductes.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(guna2DataGridView2_EditingControlShowing);
            this.KeyPreview = true;
            this.dgvProductes.RowsAdded += new DataGridViewRowsAddedEventHandler(myDataGridView_RowsAdded);
            dgvProductes.RowsRemoved += (sender, e) => ReindexColumn();
            this.dgvProductes.CellValueChanged += new DataGridViewCellEventHandler(myDataGridView_CellValueChanged);
            dgvProductes.EditingControlShowing += guna2DataGridView2_EditingControlShowing;

            this.ShowInTaskbar = false;

            // تغيير خصائص النافذة لمنع ظهورها في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            showName();
            //ThemeMode();
            textSuggester();

            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);

        }
        private void frmProductAdd2_Load(object sender, EventArgs e)
        {
            // إعداد التاريخ
            dtPicker.Value = DateTime.Today;
            dtPicker.Format = DateTimePickerFormat.Custom;
            dtPicker.CustomFormat = "yyyy-MM-dd";
            //// تحميل الفئات
            LoadStores();

        }
        private void LoadStores()
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    string qry = "SELECT storeID, storeName FROM addStore ORDER BY storeName";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cbStore.DataSource = dt;
                        cbStore.DisplayMember = "storeName";
                        cbStore.ValueMember = "storeID";

                        // ✅ لو فيه صفوف اختار أول عنصر
                        if (dt.Rows.Count > 0)
                            cbStore.SelectedIndex = 0;
                        else
                            cbStore.SelectedIndex = -1;   // ✅ لو فاضي متختارش حاجة
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل المخازن: " + ex.Message);
            }

        }


        private void showName()
        {
            string qry = @"SELECT pName FROM products";
            DataTable dt2 = new DataTable();

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);
                }
            }

            // إنشاء مجموعة البيانات المتكاملة للأتمتة التلقائية
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            // ملء مجموعة البيانات المتكاملة بالبيانات من DataTable
            foreach (DataRow row in dt2.Rows)
            {
                dataSource.Add(row["pName"].ToString());
            }

            // تعيين مصدر الاقتراحات للعمود dgvName في dgvProductes
            Guna.UI2.WinForms.Guna2TextBox textBox = (Guna.UI2.WinForms.Guna2TextBox)dgvProductes.Controls["dgvName"];
            if (textBox != null)
            {
                textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox.AutoCompleteCustomSource = dataSource;
            }
        }


        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProductes.Columns["dgvdel"].Index && e.RowIndex >= 0)
            {
                dgvProductes.Rows.RemoveAt(e.RowIndex);
                if (dgvProductes.Rows.Count == 0)
                {
                    txtPriceTotal.Text = "0";
                    txtPayPrice.Text = "0";
                    txtProfit.Text = "0";
                }
            }
        }

        private void qtyStore()
        {
            Hashtable ht = new Hashtable();
            string qry;
            string qry2;
            string status1;
            Image myImage = null;
            int pID;

            foreach (DataGridViewRow row in dgvProductes.Rows)
            {
                if (row.IsNewRow) continue; // تخطي الصف الجديد الفاضي

                ht.Clear();

                using (System.Drawing.Image temp = new Bitmap(Properties.Resources.ecommerce))
                using (MemoryStream ms = new MemoryStream())
                {
                    temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    imageByteArray = ms.ToArray();
                }

                status1 = row.Cells["dgvStatus"].Value?.ToString();
                pID = Convert.ToInt32(row.Cells["dgvproID"].Value);

                if (status1 == "مستعمل")
                {
                    qry2 = @"UPDATE totalStor 
                     SET qtyUsedU1 = @qtyU1,
                         qtyUsedU2 = @qtyU2,
                         qtyUsedU3 = @qtyU3
                     WHERE pID = @pID";

                    isUsed = true;
                }
                else
                {
                    qry2 = @"UPDATE totalStor 
                     SET qtyU1 = @qtyU1,
                         qtyU2 = @qtyU2,
                         qtyU3 = @qtyU3
                     WHERE pID = @pID";
                    isUsed = false;
                }

                typeEdit = "اضافه";

                SetProductUnitInfo(
                    pID,
                    isUsed,
                    Convert.ToInt32(row.Cells["dgvUniteID"].Value),
                    Convert.ToDouble(row.Cells["dgvQty"].Value)
                );

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry2, con))
                {
                    cmd.Parameters.AddWithValue("@pID", pID);
                    cmd.Parameters.AddWithValue("@qtyU1", qtyU1);
                    cmd.Parameters.AddWithValue("@qtyU2", qtyU2);
                    cmd.Parameters.AddWithValue("@qtyU3", qtyU3);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    cmd.ExecuteNonQuery();
                }
            }

            string billNumber = "NULL";
            string qry5 = "SELECT billNumber FROM billPrcheses WHERE bID = @billID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd1 = new SqlCommand(qry5, con))
            {
                cmd1.Parameters.AddWithValue("@billID", billID);

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd1))
                {
                    da2.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        billNumber = dt2.Rows[0]["billNumber"].ToString();
                    }
                }
            }

            Hashtable ht3 = new Hashtable();
            string qry3 = @"Insert into rconrdEditingPro Values(@posName, @editeIn,@editeTo, @tableName , @typeEdit,@date ,@time); Select SCOPE_IDENTITY()";

            ht3.Add("@posName", MainClass.USER);
            ht3.Add("@editeIn", billNumber);
            ht3.Add("@editeTo", DBNull.Value);
            ht3.Add("@tableName", "الفاتوره رقم");
            ht3.Add("@typeEdit", typeEdit);
            ht3.Add("@date", Convert.ToDateTime(DateTime.Now.Date));
            ht3.Add("@time", Convert.ToString(DateTime.Now.ToShortTimeString()));

            MainClass.SQL(qry3, ht3);
        }

        public event EventHandler<bool> newBill;

        private async void btnSave_Click(object sender, EventArgs e)
        {
            newBill?.Invoke(this, true);

            for (int i = dgvProductes.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvProductes.Rows[i];

                // تجاهل صف الإدخال الجديد (الصف الأخير اللي بيكون فاضي لإضافة بيانات)
                if (row.IsNewRow) continue;

                if (string.IsNullOrWhiteSpace(row.Cells["dgvName"].Value?.ToString()))
                {
                    dgvProductes.Rows.RemoveAt(i);
                }
            }
            saveProducts();

            frmShowBackup frmshowBackup = new frmShowBackup();
            frmshowBackup.backupType = "DIFFERENTIAL";
            frmshowBackup.showNotification = false;
            frmshowBackup.ShowDialog(this);
            
            this.Focus();
            btnclse();
            try
            {


            }
            catch
            {

            }
        }

        private string GenerateUniqueInvoiceCode()
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
                using (SqlConnection con = MainClass.GetConnection()) // بدل ما تعمل new SqlConnection
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM billPrcheses WHERE InvoiceCode = @code", con))
                    {
                        cmd.Parameters.AddWithValue("@code", code ?? (object)DBNull.Value);
                        exists = (int)cmd.ExecuteScalar() > 0;
                    }
                }


            } while (exists);
            return code;
        }

        private async void saveProducts()
        {
            string qry;
            string qry2;
            frmBlackout frmBlackout = new frmBlackout(parirFrm);
            frmBlackout.Show();
            frmBlackout.Owner = parirFrm;

            if (billID == 0)
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {

                            // 1) حفظ الفاتورة
                            billID = InsertBill(con, tran, storeID, supplierID, qty, txtNote.Text, txtBillNumber.Text, dtPicker.Value);


                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("خطأ أثناء الحفظ: " + ex.Message);
                        }
                    }
                }
            }


            using (frmPayWays frm = new frmPayWays())
            {
                frm.mainID = billID;
                frm.partyType = "مورد";
                frm.status = "new";
                frm.totalClean = decimal.Parse(txtPriceTotal.Text.Replace('٫', '.').Trim(), CultureInfo.InvariantCulture);
                frm.total = decimal.Parse(txtPriceTotal.Text.Replace('٫', '.').Trim(), CultureInfo.InvariantCulture);
                frm.partyName = txtSupName.Text;
                frm.selectedPartyID = supplierID;
                frm.btnNext1.Enabled = true;
                frm.txtName.Enabled = false;
                
                frm.btnUnknow.Enabled = false;
                frm.btnSearch.Enabled = false;
                frm.btnAddParties.Visible = false;
                frm.btnEditParties.Visible = false;
                frm.Owner = this;
                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {

                    SaveBillAndDetails();
                    qtyStore();

                    PrintBarcodes();

                    // إظهار رسالة نجاح
                    dgvProductes.Rows.Clear();

                    mainPanel.Enabled = false;
                    btnAddProducts.Enabled = false;
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    groupBox4.Enabled = true;

                    btnEdit1.Enabled = false;
                    btnNext.Enabled = true;

                    txtPriceTotal.Text = "0";
                    txtPayPrice.Text = "0";
                    txtProfit.Text = "0";
                    txtSupName.Text = string.Empty;
                    txtSupNumber.Text = string.Empty;
                    txtSumSupp.Text = string.Empty;
                    txtBillNumber.Text = string.Empty;
                    txtNote.Text = string.Empty;
                    cbStore.SelectedIndex = 0;
                    dtPicker.Value = DateTime.Today;


                    billID = 0;

                    await MainClass.BackUpWithoutSpinnerAsync();

                }
                else
                {

                }
            }
            this.Focus();
            frmBlackout.Close();

            this.Focus();
        }
        private int InsertBill(SqlConnection con, SqlTransaction tran, int storeID, int supplierID, double qty, string txtNote, string txtBillNumber, DateTime dtPicker)
        {
            string qry1 = @"
            INSERT INTO billPrcheses 
                (storeID, supplierID, pqty, serialNumber, notes, payWay, billNumber, total, clear, date, Time, billStatus, shiftID, InvoiceCode)
            VALUES 
                (@storeID, @supplierID, @pqty, @serialNumber, @notes, @payWay, @billNumber, @total, @clear, @date, @Time, @billStatus, @shiftID , @InvoiceCode);
            SELECT SCOPE_IDENTITY()";

            string InvoiceCode = GenerateUniqueInvoiceCode();
            using (SqlCommand cmd1 = new SqlCommand(qry1, con, tran))
            {
                cmd1.Parameters.Add("@storeID", SqlDbType.Int).Value = storeID;
                cmd1.Parameters.Add("@supplierID", SqlDbType.Int).Value = supplierID;
                cmd1.Parameters.Add("@pqty", SqlDbType.Int).Value = qty;
                cmd1.Parameters.Add("@serialNumber", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd1.Parameters.Add("@notes", SqlDbType.NVarChar).Value = txtNote ?? (object)DBNull.Value;
                cmd1.Parameters.Add("@payWay", SqlDbType.NVarChar).Value = DBNull.Value;
                cmd1.Parameters.Add("@billNumber", SqlDbType.NVarChar).Value = txtBillNumber ?? (object)DBNull.Value;
                cmd1.Parameters.Add("@total", SqlDbType.Decimal).Value = DBNull.Value;
                cmd1.Parameters.Add("@clear", SqlDbType.Decimal).Value = DBNull.Value;
                cmd1.Parameters.Add("@date", SqlDbType.Date).Value = dtPicker.Date;
                cmd1.Parameters.Add("@Time", SqlDbType.NVarChar).Value = DateTime.Now.ToShortTimeString();
                cmd1.Parameters.Add("@billStatus", SqlDbType.NVarChar).Value = "UnderWork";
                cmd1.Parameters.Add("@shiftID", SqlDbType.NVarChar).Value = MainClass.shiftID;
                cmd1.Parameters.Add("@InvoiceCode", SqlDbType.NVarChar).Value = InvoiceCode;




                return Convert.ToInt32(cmd1.ExecuteScalar());
            }
        }

        // List لتخزين الأكواد والعدد
        private List<(string Code, int Qty, string name, string status, double price)> productList = new List<(string, int, string, string, double)>();
        private Queue<(string Code, string Name, string Status, double price)> printQueue = new Queue<(string, string, string, double price)>();
        private PrintDocument printDoc = new PrintDocument();

        private void InsertBillDetails(SqlConnection con, SqlTransaction tran, int billID, DataGridView dgvProductes)
        {
            productList.Clear(); // 👈 امسح القائمة قبل ما تبدأ تضيف من جديد

            string qry2 = @"
            INSERT INTO tblDetailsSupliser 
                (billPrchesesID, proID, qty, price, cleanPrice, amount, unite, pDescount, vDescount, priceAfterDes, isUsed, uniteID, status) 
            VALUES 
                (@billPrchesesID, @proID, @qty, @price, @cleanPrice, @amount, @unite, @pDescount, @vDescount, @priceAfterDes, @isUsed, @uniteID, @status)";

            foreach (DataGridViewRow row in dgvProductes.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = row.Cells["dgvBarCode"]?.Value != null && (bool)row.Cells["dgvBarCode"].Value;

                // 1️⃣ خزّن المنتج في القاعدة مهما كان CheckBox متحدد أو لا
                using (SqlCommand cmd2 = new SqlCommand(qry2, con, tran))
                {
                    int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);

                    // ✅ price (السعر قبل أي خصومات)
                    decimal price = Convert.ToDecimal(row.Cells["dgvPriceTotal"].Value);

                    // ✅ cleanPrice (السعر الصافي) مع معالجة الفاصلة العربية والغربية
                    decimal cleanPrice = 0;
                    if (row.Cells["dgvTotal"].Value != null)
                    {
                        string cp = row.Cells["dgvTotal"].Value.ToString().Trim();
                        cp = cp.Replace("٫", "."); // تحويل الفاصلة العربية
                        decimal.TryParse(cp, NumberStyles.Any, CultureInfo.InvariantCulture, out cleanPrice);
                    }

                    // ✅ amount = qty × cleanPrice (السعر الصافي × الكمية)
                    decimal amount = cleanPrice;

                    // ✅ priceAfterDes = نفس amount (لأنه بدون خصومات)
                    decimal priceAfterDes = amount;

                    cmd2.Parameters.Add("@billPrchesesID", SqlDbType.Int).Value = billID;
                    cmd2.Parameters.Add("@proID", SqlDbType.Int).Value = Convert.ToInt32(row.Cells["dgvproID"].Value);
                    cmd2.Parameters.Add("@qty", SqlDbType.Int).Value = qty;
                    cmd2.Parameters.Add("@price", SqlDbType.Decimal).Value = price;
                    cmd2.Parameters.Add("@cleanPrice", SqlDbType.Decimal).Value = cleanPrice;
                    cmd2.Parameters.Add("@amount", SqlDbType.Decimal).Value = amount;

                    cmd2.Parameters.Add("@unite", SqlDbType.NVarChar).Value = row.Cells["dgvUnite"].Value ?? DBNull.Value;
                    cmd2.Parameters.Add("@pDescount", SqlDbType.Decimal).Value = 0;
                    cmd2.Parameters.Add("@vDescount", SqlDbType.Decimal).Value = 0;
                    cmd2.Parameters.Add("@priceAfterDes", SqlDbType.Decimal).Value = priceAfterDes;
                    cmd2.Parameters.Add("@uniteID", SqlDbType.Int).Value = Convert.ToInt32(row.Cells["dgvUniteID"].Value);

                    string proStatus = row.Cells["dgvStatus"].Value?.ToString();
                    cmd2.Parameters.Add("@status", SqlDbType.NVarChar).Value = (string.IsNullOrWhiteSpace(proStatus) ? DBNull.Value : (object)proStatus);
                    cmd2.Parameters.Add("@isUsed", SqlDbType.Bit).Value = (proStatus == "مستعمل");

                    cmd2.ExecuteNonQuery();
                }


                // 2️⃣ ضيف للـ List **لو الـ CheckBox متحدد بس**
                if (isChecked)
                {
                    string code = row.Cells["dgvCode"].Value?.ToString();
                    int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);
                    string name = row.Cells["dgvName"].Value?.ToString();
                    string status = row.Cells["dgvStatus"].Value?.ToString();
                    double price = Convert.ToDouble(row.Cells["dgvPrice"].Value?.ToString());

                    if (!string.IsNullOrEmpty(code))
                    {
                        productList.Add((code, qty, name, status, price));
                    }
                }
            }


        }

        public void PrintBarcodes()
        {
            // إعدادات الطابعة
            printDoc.PrinterSettings.PrinterName = MainClass.BarcodePrinter;        
            PaperSize paperSize = new PaperSize("Custom", 260, 98);
            printDoc.DefaultPageSettings.PaperSize = paperSize;
            printDoc.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);

            // فضي الطابور
            printQueue.Clear();

            // ضيف الأكواد حسب الكمية
            foreach (var item in productList)
            {
                for (int i = 0; i < item.Qty; i++)
                {
                    printQueue.Enqueue((item.Code, item.name, item.status, item.price));
                }
            }

            // ابدأ الطباعة
            if (printQueue.Count > 0)
                printDoc.Print();
            else
                Notifier.ShowNotification("تنبيه", "لا يوجد أي منتجات للطباعة.");
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (printQueue.Count > 0)
            {
                var item = printQueue.Dequeue();
                string code = item.Code;
                string name = item.Name;
                string status = item.Status;
                string price = item.price.ToString();

                var barGenerator = new generatBarCode();
                int barcodeWidth = 230;
                int barcodeHeight = 60;

                int x = (e.PageBounds.Width - barcodeWidth) / 2;
                int x2 = (e.PageBounds.Width - 260) / 2;

                int y = 10;

                // اسم المنتج (فوق الباركود في النص)
                RectangleF nameRect = new RectangleF(x - 8, y, barcodeWidth, 20); // نفس عرض الباركود ومتمركز
                StringFormat centerFormat = new StringFormat();
                centerFormat.Alignment = StringAlignment.Center;
                centerFormat.LineAlignment = StringAlignment.Center;
                centerFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft; // عشان عربي

                e.Graphics.DrawString(MainClass.CompanyName, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, nameRect, centerFormat);

                // بعد الاسم نسيب مسافة ونرسم الباركود
                y += 15;
                Image barcodeImg = barGenerator.CreateBarCode(code);
                e.Graphics.DrawImage(barcodeImg, new Rectangle(x - 9, y, barcodeWidth, barcodeHeight));

                // الحالة (تحت الباركود على اليمين)
                y += barcodeHeight - 10;

                RectangleF nameRect2 = new RectangleF(x - 30, y, barcodeWidth, 20); // نفس عرض الباركود ومتمركز
                StringFormat centerFormat2 = new StringFormat();
                centerFormat2.Alignment = StringAlignment.Center;
                centerFormat2.LineAlignment = StringAlignment.Center;
                centerFormat2.FormatFlags = StringFormatFlags.DirectionRightToLeft; // عشان عربي

                e.Graphics.DrawString(name, new Font("Arial", 9, FontStyle.Regular), Brushes.Black, nameRect2, centerFormat2);

                // لو فيه منتجات تانية اطبع صفحة جديدة
                e.HasMorePages = (printQueue.Count > 0);
            }
        }



        private void SaveBillAndDetails()
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        // 2) حفظ التفاصيل
                        InsertBillDetails(con, tran, billID, dgvProductes);

                        tran.Commit();
                        Notifier.ShowNotification("تم الحفظ", "تم حفظ الفاتورة ,والمنتجات بنجاح ✅");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("خطأ أثناء الحفظ: " + ex.Message);
                    }
                }
            }
        }


        // تعريف الحقول على مستوى الفورم
        private int currentUinte;
        private int currentNumber;
        private double currentUnitePrice;
        private double currentPurchese;
        private double currentQty;

        private double qtyU1, qtyU2, qtyU3;


        // الدالة المعدلة
        private void SetProductUnitInfo(int pID, bool isUsed, int currentUinte, double extraQtyU = 0)
        {
            string query = @"
                        SELECT p.*, c.*, u.uName, ts.*
                        FROM products p
                        INNER JOIN category c ON c.catID = p.categoryID
                        INNER JOIN untits u ON p.idUniteDef = u.uID
                        INNER JOIN totalStor ts ON ts.pID = p.pID
                        WHERE p.pID = @value";

            using (SqlConnection con = MainClass.GetConnection()) // استخدام GetConnection
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@value", pID);

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                if (dt.Rows.Count == 0)
                    return; // لو مفيش بيانات للمنتج ده

                DataRow row = dt.Rows[0];

                int idUnite1 = Convert.ToInt32(row["idUnite1"]);
                int idUnite2 = Convert.ToInt32(row["idUnite2"]);
                int idUnite3 = Convert.ToInt32(row["idUnite3"]);

                int numberU2 = Convert.ToInt32(row["numberU2"]); // كم وحدة U3 في U2
                int numberU3 = Convert.ToInt32(row["numberU3"]); // كم U2 في U1

                // 1️⃣ الحصول على الكمية حسب الوحدة الافتراضية
                if (currentUinte == idUnite3)
                {
                    qtyU3 = isUsed ? Convert.ToDouble(row["qtyUsedU3"]) : Convert.ToDouble(row["qtyU3"]);
                    qtyU3 += extraQtyU;
                }
                else if (currentUinte == idUnite2)
                {
                    double baseQty = isUsed ? Convert.ToDouble(row["qtyUsedU2"]) : Convert.ToDouble(row["qtyU2"]);
                    baseQty += extraQtyU;
                    qtyU3 = baseQty * numberU3; // نحولها للوحدة الأصغر U3
                }
                else
                {
                    double baseQty = isUsed ? Convert.ToDouble(row["qtyUsedU1"]) : Convert.ToDouble(row["qtyU1"]);
                    baseQty += extraQtyU;
                    qtyU3 = baseQty * numberU2 * numberU3; // نحولها للوحدة الأصغر U3
                }

                // 2️⃣ حساب الكميات بوحدات مختلفة
                qtyU2 = qtyU3 / numberU3; // كام وحدة U2
                qtyU1 = qtyU2 / numberU2; // كام وحدة U1
            }

        }



        protected virtual void OnButtonClicked()
        {
            ButtonClicked?.Invoke(this, EventArgs.Empty);
        }
        HashSet<string> columnsToFilter = new HashSet<string> { "dgvPrice", "dgvPriceTotal", "dgvQty", "dgvShorlfall", "dgvClear", "dgvbonus", "dgvCode" }; // استبدل بأسماء الأعمدة الفعلية

        private void guna2DataGridView2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dgvProductes.CurrentCell != null)
            {
                string currentColumnName = dgvProductes.Columns[dgvProductes.CurrentCell.ColumnIndex].Name;

                if (columnsToFilter.Contains(currentColumnName))
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                }
            }

        }

        private void guna2DataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            string currentColumnName = dgvProductes.Columns[dgvProductes.CurrentCell.ColumnIndex].Name;
            if (columnsToFilter.Contains(currentColumnName))
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }


        }
        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            string currentColumnName = dgvProductes.Columns[dgvProductes.CurrentCell.ColumnIndex].Name;

            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.' || currentColumnName.Contains(".")))
            {
                e.Handled = true;
            }
        }

        private void guna2DataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                try
                {
                    if (dgvProductes.Rows.Count == 0)
                    {
                        dgvProductes.Rows.Add();
                        dgvProductes.CurrentCell = dgvProductes.Rows[0].Cells[0];
                        e.Handled = true;
                        return;
                    }

                    int currentRowIndex = dgvProductes.CurrentCell.RowIndex;

                    if (currentRowIndex == dgvProductes.Rows.Count - 1)
                    {
                        object cellValue = dgvProductes.Rows[currentRowIndex].Cells["dgvproID"].Value;

                        if (cellValue != null)
                        {
                            int newIndex = dgvProductes.Rows.Add();
                            dgvProductes.CurrentCell = dgvProductes.Rows[newIndex].Cells[0];
                            e.Handled = true;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("حدث خطأ أثناء إضافة صف جديد.");
                }
            }

            if (dgvProductes.SelectedRows.Count == 0) return;

            int pID = Convert.ToInt32(dgvProductes.CurrentRow.Cells["dgvproID"].Value);

            string qry = @"SELECT p.purPrice,
	                     p.idUnite1,
	                     p.idUnite2,
	                     p.idUnite3,
	                     p.sellPrice,
	                     p.priceU2,
	                     p.priceU3,
	                     p.sellPriceUsed,
	                     p.priceU2Used,
	                     p.priceU3Used,
	                     p.purPrice,
	                     p.purUsedPrice,
                         p.numberU2,
                         p.numberU3,
	                     c.*,
	                     ts.*
             FROM products p 
             INNER JOIN category c ON c.catID = p.categoryID 
             INNER JOIN totalStor ts ON ts.pID = p.pID 
             WHERE p.pID = @pID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@pID", pID);
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (dt.Rows.Count == 0) return;

                DataRow product = dt.Rows[0];
                string status1 = dgvProductes.CurrentRow.Cells["dgvStatus"].Value.ToString();
                double purPrice;
                double sellPrice;

                if (status1 == "مستعمل")
                {
                    sellPrice = Convert.ToDouble(product["sellPriceUsed"]);
                    purPrice = Convert.ToDouble(product["purUsedPrice"].ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                    isUsed = true;
                }
                else
                {
                    sellPrice = Convert.ToDouble(product["sellPrice"]);
                    purPrice = Convert.ToDouble(product["purPrice"].ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                    isUsed = false;
                }

                if (e.KeyCode == Keys.F6)
                {
                    currentQty = isUsed ? Convert.ToDouble(product["qtyUsedU1"]) : Convert.ToDouble(product["qtyU1"]);
                    sellPrice = isUsed ? Convert.ToDouble(product["sellPriceUsed"]) : Convert.ToDouble(product["sellPrice"]);

                    UpdateRows(dgvProductes.SelectedRows,
                               Convert.ToInt32(product["idUnite1"]),
                               sellPrice,
                               purPrice,
                               currentQty);
                }
                else if (e.KeyCode == Keys.F5)
                {
                    int numU2 = Convert.ToInt32(product["numberU2"]);
                    currentQty = isUsed ? Convert.ToInt32(product["qtyUsedU2"]) : Convert.ToInt32(product["qtyU2"]);
                    sellPrice = isUsed ? Convert.ToDouble(product["priceU2Used"]) : Convert.ToDouble(product["priceU2"]);

                    UpdateRows(dgvProductes.SelectedRows,
                               Convert.ToInt32(product["idUnite2"]),
                               sellPrice,
                               purPrice / numU2,
                               currentQty);
                }
                else if (e.KeyCode == Keys.F4)
                {
                    int numU2 = Convert.ToInt32(product["numberU2"]);
                    int numU3 = Convert.ToInt32(product["numberU3"]);
                    currentQty = isUsed ? Convert.ToInt32(product["qtyUsedU3"]) : Convert.ToInt32(product["qtyU3"]);
                    sellPrice = isUsed ? Convert.ToDouble(product["priceU3Used"]) : Convert.ToDouble(product["priceU3"]);

                    UpdateRows(dgvProductes.SelectedRows,
                               Convert.ToInt32(product["idUnite3"]),
                               sellPrice,
                               (purPrice / numU2) / numU3,
                               currentQty);
                }
                else if (e.KeyCode == Keys.F3)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }




        private void UpdateRows(DataGridViewSelectedRowCollection rows, int unitId, double sellPrice, double purPrice, double qty)
        {
            string qry = "SELECT uName FROM untits WHERE uID = @uID";
            string uName = "";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@uID", unitId);
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                    uName = dt.Rows[0]["uName"].ToString();
            }

            foreach (DataGridViewRow row in rows)
            {
                row.Cells["dgvUnite"].Value = uName;
                row.Cells["dgvPrice"].Value = sellPrice;      // سعر البيع
                row.Cells["dgvPriceTotal"].Value = purPrice;  // سعر الشراء
                row.Cells["dgvBalance"].Value = qty;
                row.Cells["dgvUniteID"].Value = unitId;
            }
        }



        private void guna2DataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvProductes.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;

        }

        private void guna2DataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvProductes.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;

        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void myDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            if (dgvProductes.Rows.Count > 0)
            {
                int newValue = 1;

                if (dgvProductes.Rows.Count > 1)
                {
                    int lastIndex = dgvProductes.Rows.Count - 2;

                    newValue = Convert.ToInt32(dgvProductes.Rows[lastIndex].Cells["dgSno"].Value) + 1;
                }


                int newIndex = dgvProductes.Rows.Count - 1;
                dgvProductes.Rows[newIndex].Cells["dgSno"].Value = newValue;

                BeginInvoke(new MethodInvoker(() =>
                {

                    int columnIndex = dgvProductes.Columns["dgvCode"].Index;

                    dgvProductes.CurrentCell = dgvProductes.Rows[dgvProductes.Rows.Count - 1].Cells[columnIndex];

                    dgvProductes.BeginEdit(true);
                }));
            }


        }
        private void ReindexColumn()
        {
            int rowIndex = 1; // بدء الترقيم من 1
            foreach (DataGridViewRow row in dgvProductes.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells["dgSno"].Value = rowIndex++;
                }
            }
        }
        private bool sortAscending = true;

        private void guna2DataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SortDataGridView(dgvProductes.Columns[e.ColumnIndex].Name, sortAscending);
            sortAscending = !sortAscending;
        }

        private void SortDataGridView(string columnName, bool ascending)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>(dgvProductes.Rows.Cast<DataGridViewRow>());

            rows = rows.Where(r => !r.IsNewRow).ToList();

            if (ascending)
            {
                rows = rows.OrderBy(r => int.TryParse(r.Cells[columnName].Value?.ToString(), out int tempVal) ? tempVal : int.MinValue).ToList();
            }
            else
            {
                rows = rows.OrderByDescending(r => int.TryParse(r.Cells[columnName].Value?.ToString(), out int tempVal) ? tempVal : int.MaxValue).ToList();
            }

            dgvProductes.Rows.Clear();
            foreach (var row in rows)
            {
                dgvProductes.Rows.Add(row);
            }
        }
        private void myDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (dgvProductes.CurrentCell != null)
                {
                    double des = 0;
                    double total = 0;
                    double clear = 0;
                    double percent = 0;
                    double desP = 0;
                    double change = 0;


                    if (e.ColumnIndex == dgvProductes.Columns["dgvPriceTotal"].Index)
                    {
                        DataGridViewRow row = dgvProductes.Rows[e.RowIndex];
                        int selectedIndex = dgvProductes.CurrentCell.RowIndex;
                        if (dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value != null && dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value.ToString() != string.Empty)
                        {

                            if (selectedIndex >= 0 && selectedIndex < dgvProductes.Rows.Count)
                            {
                                if (double.TryParse(dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value.ToString(), out total))
                                {
                                    if (dgvProductes.Rows[selectedIndex].Cells["dgvDp"].Value == null)
                                        desP = 0;
                                    else
                                        double.TryParse(dgvProductes.Rows[selectedIndex].Cells["dgvDp"].Value.ToString(), out desP);

                                    percent = total * (desP / 100);
                                    change = total - percent;
                                    dgvProductes.Rows[selectedIndex].Cells["dgvClear"].Value = change.ToString("F2");
                                }
                                dgvProductes.Rows[selectedIndex].Cells["dgvDv"].Value = percent.ToString("F2");
                                foreach (DataGridViewRow item in dgvProductes.Rows)
                                {
                                    if (item.Cells["dgvTotal"].Value != null && item.Cells["dgvTotal"].Value.ToString() != string.Empty)
                                    {

                                        var cellValue = dgvProductes.Rows[selectedIndex].Cells["dgvQty"].Value;
                                        var qtyCellValue = dgvProductes.Rows[selectedIndex].Cells["dgvQty"].Value;
                                        var priceCellValue = dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value;

                                        double qty = 0;
                                        double price = 0;

                                        if (qtyCellValue != null)
                                        {
                                            qty = Convert.ToDouble(qtyCellValue.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                                        }

                                        if (priceCellValue != null)
                                        {
                                            price = Convert.ToDouble(priceCellValue.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                                        }

                                        double priceTotal = qty * price;
                                        dgvProductes.Rows[selectedIndex].Cells["dgvTotal"].Value = priceTotal.ToString("F2");


                                    }

                                }


                            }
                        }

                    }
                    else if (e.ColumnIndex == dgvProductes.Columns["dgvDp"].Index)
                    {
                        DataGridViewRow row = dgvProductes.Rows[e.RowIndex];
                        int selectedIndex = dgvProductes.CurrentCell.RowIndex;
                        if (dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value != null && dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value.ToString() != string.Empty)
                        {
                            if (selectedIndex >= 0 && selectedIndex < dgvProductes.Rows.Count)
                            {
                                if (double.TryParse(dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value.ToString(), out total))
                                {
                                    if (dgvProductes.Rows[selectedIndex].Cells["dgvDp"].Value == null)
                                        desP = 0;
                                    else
                                        double.TryParse(dgvProductes.Rows[selectedIndex].Cells["dgvDp"].Value.ToString(), out desP);

                                    percent = total * (desP / 100);
                                    change = total - percent;
                                    dgvProductes.Rows[selectedIndex].Cells["dgvClear"].Value = change.ToString("F2");
                                }
                                dgvProductes.Rows[selectedIndex].Cells["dgvDv"].Value = percent.ToString("F2");
                                foreach (DataGridViewRow item in dgvProductes.Rows)
                                {
                                    if (item.Cells["dgvTotal"].Value != null && item.Cells["dgvTotal"].Value.ToString() != string.Empty)
                                    {

                                        var qtyCellValue = dgvProductes.Rows[selectedIndex].Cells["dgvQty"].Value;
                                        var priceCellValue = dgvProductes.Rows[selectedIndex].Cells["dgvPriceTotal"].Value;

                                        double qty = 0;  // تعيين قيمة افتراضية
                                        double price = 0;  // تعيين قيمة افتراضية

                                        if (qtyCellValue != null && qtyCellValue.ToString() != "")
                                        {
                                            qty = Convert.ToDouble(qtyCellValue.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                                        }

                                        if (priceCellValue != null && priceCellValue.ToString() != "")
                                        {
                                            price = Convert.ToDouble(priceCellValue.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                                        }

                                        double priceTotal = qty * price;
                                        dgvProductes.Rows[selectedIndex].Cells["dgvTotal"].Value = priceTotal.ToString("F2");


                                    }

                                }
                            }
                        }

                    }

                    else if (e.ColumnIndex == dgvProductes.Columns["dgvexpDate"].Index)
                    {

                    }
                }
                else
                {
                }
            }
            catch { MessageBox.Show("حدث خطأ"); }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnHide();
            this.Close();
        }
        protected virtual void btnHide()
        {
            ButtonHide?.Invoke(this, EventArgs.Empty);

        }


        bool isEditing = false;

        private void guna2DataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (isEditing) return;
            isEditing = true;

            var dgv = sender as DataGridView;
            if (dgv == null) { isEditing = true; return; }

            string colName = dgv.Columns[e.ColumnIndex].Name;
            string value = dgv[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "";


            // بحث عن المنتج
            if (colName == "dgvCode")
            {
                if (HandleProductSearch(dgv, e.RowIndex, "pCode", value))
                {
                    isEditing = false; // تأكيد إرجاع الفلاج

                    return; // لو المنتج اتلاقى خلاص نوقف

                }

                if (HandleProductSearch(dgv, e.RowIndex, "pNewBarode", value))
                {
                    isEditing = false; // تأكيد إرجاع الفلاج
                    return; // لو اتلاقى هنا نوقف

                }

                HandleProductSearch(dgv, e.RowIndex, "pUsedBarode", value); // لو لسه ما اتلاقاش يكمل
            }
            else if (colName == "dgvName")
                HandleProductSearch(dgv, e.RowIndex, "pName", value);
            else if (colName == "dgvShortcut")
                HandleProductSearch(dgv, e.RowIndex, "shorcut", value);
            try
            {



                // الخصم بالقيمة
                if (colName == "dgvPriceTotal")
                {
                    double price = Convert.ToDouble(dgv["dgvPriceTotal", e.RowIndex].Value);
                    double qty = Convert.ToDouble(dgv["dgvQty", e.RowIndex].Value);

                    double total = price * qty;

                    dgv["dgvTotal", e.RowIndex].Value = total;
                }


                // تحديث الإجمالي من الكمية
                if (colName == "dgvQty")
                {
                    double qty = Convert.ToDouble(dgv["dgvQty", e.RowIndex].Value);
                    double price = Convert.ToDouble(dgv["dgvPriceTotal", e.RowIndex].Value);
                    dgv["dgvTotal", e.RowIndex].Value = qty * price;
                }
            }
            catch
            {
                Notifier.ShowNotification("حدث خطأ أثناء معالجة البيانات. يرجى التحقق من المدخلات.", "خطأ");
            }

            isEditing = false;
        }


        private int numberU2;
        private int numberU3;
        private int idUniteDef;
        private int idUnite1;
        private int idUnite2;
        private int idUnite3;
        private bool isUsed = false;
        private double qty;

        private string status;
        private bool HandleProductSearch(DataGridView dgv, int rowIndex, string columnName, string searchValue)
        {
            try
            {
                // 🟢 تحديد حالة المنتج (مستعمل / جديد)
                isUsed = (columnName == "pUsedBarode");
                status = isUsed ? "مستعمل" : "جديد";

                if (string.IsNullOrWhiteSpace(searchValue))
                    return false;

                string query = @"
                SELECT p.*, c.catName, u.uName, ts.*
                FROM products p
                INNER JOIN category c ON c.catID = p.categoryID
                INNER JOIN untits u ON p.idUniteDef = u.uID
                INNER JOIN totalStor ts ON ts.pID = p.pID
                WHERE p." + columnName + " = @value";

                DataTable dt = new DataTable();

                // ✅ افتح اتصال جديد لكل عملية
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@value", searchValue);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                if (dt.Rows.Count == 0)
                    return false;

                DataRow row = dt.Rows[0];

                // 🟢 جلب بيانات الوحدات
                idUniteDef = Convert.ToInt32(row["idUniteDef"]);
                idUnite1 = Convert.ToInt32(row["idUnite1"]);
                idUnite2 = Convert.ToInt32(row["idUnite2"]);
                idUnite3 = Convert.ToInt32(row["idUnite3"]);

                numberU2 = Convert.ToInt32(row["numberU2"]);
                numberU3 = Convert.ToInt32(row["numberU3"]);

                // 🟢 تحديد الوحدة الحالية
                if (idUniteDef == idUnite3)
                {
                    currentUinte = idUnite3;
                    currentNumber = numberU2 * numberU3;
                    currentUnitePrice = isUsed ? Convert.ToDouble(row["priceU3Used"]) : Convert.ToDouble(row["priceU3"]);
                    currentPurchese = isUsed ? Convert.ToDouble(row["purUsedPrice"]) : Convert.ToDouble(row["purPrice"]);
                    qty = isUsed ? Convert.ToDouble(row["qtyUsedU3"]) : Convert.ToDouble(row["qtyU3"]);
                }
                else if (idUniteDef == idUnite2)
                {
                    currentUinte = idUnite2;
                    currentNumber = numberU2;
                    currentUnitePrice = isUsed ? Convert.ToDouble(row["priceU2Used"]) : Convert.ToDouble(row["priceU2"]);
                    currentPurchese = isUsed ? Convert.ToDouble(row["purUsedPrice"]) : Convert.ToDouble(row["purPrice"]);
                    qty = isUsed ? Convert.ToDouble(row["qtyUsedU2"]) : Convert.ToDouble(row["qtyU2"]);
                }
                else
                {
                    currentUinte = idUnite1;
                    currentNumber = 1;
                    currentUnitePrice = isUsed ? Convert.ToDouble(row["sellPriceUsed"]) : Convert.ToDouble(row["sellPrice"]);
                    currentPurchese = isUsed ? Convert.ToDouble(row["purUsedPrice"]) : Convert.ToDouble(row["purPrice"]);
                    qty = isUsed ? Convert.ToDouble(row["qtyUsedU1"]) : Convert.ToDouble(row["qtyU1"]);
                }

                // 🟢 أسعار الوحدات
                double unitSellPrice = currentUnitePrice;                 // سعر البيع للوحدة
                double unitPurchasePrice = currentPurchese / currentNumber; // تكلفة الشراء للوحدة

                string newName = row["pName"].ToString();
                string newCode = searchValue;

                // 🟢 البحث إذا المنتج موجود مسبقًا
                foreach (DataGridViewRow gridRow in dgv.Rows)
                {
                    if (!gridRow.IsNewRow &&
                        gridRow.Cells["dgvCode"].Value?.ToString() == newCode &&
                        !string.IsNullOrWhiteSpace(gridRow.Cells["dgvName"].Value?.ToString()))
                    {
                        // زيادة الكمية
                        double oldQty = Convert.ToDouble(gridRow.Cells["dgvQty"].Value);
                        double finalQty = oldQty + 1;

                        gridRow.Cells["dgvQty"].Value = finalQty;
                        gridRow.Cells["dgvTotal"].Value = finalQty * unitPurchasePrice;

                        // امسح الكود من الصف الحالي لو فارغ
                        if (rowIndex < dgv.Rows.Count && !dgv.Rows[rowIndex].IsNewRow)
                        {
                            if (dgv.Rows[rowIndex].Cells["dgvCode"].Value?.ToString() == newCode &&
                                string.IsNullOrWhiteSpace(dgv.Rows[rowIndex].Cells["dgvName"].Value?.ToString()))
                            {
                                dgv.Rows[rowIndex].Cells["dgvCode"].Value = null;
                                dgv.CurrentCell = dgv.Rows[rowIndex].Cells["dgvCode"];
                                dgv.BeginEdit(true);
                            }
                        }

                        return true;
                    }
                }

                // 🟢 إضافة المنتج للـ DataGridView
                dgv["dgvproID", rowIndex].Value = row["pID"];
                dgv["dgvBarCode", rowIndex].Value = false;
                dgv["dgvStatus", rowIndex].Value = status;
                dgv["dgvName", rowIndex].Value = newName;
                dgv["dgvUnite", rowIndex].Value = row["uName"];
                dgv["dgvPriceTotal", rowIndex].Value = unitPurchasePrice.ToString();
                dgv["dgvDp", rowIndex].Value = row["discountPro"];
                dgv["dgvPrice", rowIndex].Value = unitSellPrice.ToString();
                dgv["dgbCat", rowIndex].Value = row["catName"];
                dgv["dgvUniteID", rowIndex].Value = idUniteDef;
                dgv["dgvbonus", rowIndex].Value = "0";
                dgv["dgvQty", rowIndex].Value = "1";
                dgv["dgvTotal", rowIndex].Value = unitPurchasePrice.ToString(); ; // الكمية = 1
                dgv["dgvBalance", rowIndex].Value = qty;
                dgv["dgvBillid", rowIndex].Value = billID;

                // 🟢 حذف الصف الفاضي لو موجود
                CheckAndDeleteEmptyNameRow(dgv, rowIndex);

                // 🟢 إضافة صف جديد
                int rowIndexNew = dgv.Rows.Add();
                dgv.CurrentCell = dgv.Rows[rowIndexNew].Cells["dgvCode"];
                dgv.BeginEdit(true);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء البحث عن المنتج:\n" + ex.Message,
                                "خطأ",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }



        private void CheckAndDeleteEmptyNameRow(DataGridView dgv, int rowIndex)
        {
            if (dgv.Rows.Count > rowIndex && string.IsNullOrWhiteSpace(dgv.Rows[rowIndex].Cells["dgvName"].Value?.ToString()))
            {
                dgv.BeginInvoke(new Action(() =>
                {
                    dgv.Rows.RemoveAt(rowIndex);
                }));
            }

        }

        private void txtPriceTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.' || txtPriceTotal.Text.Contains(".")))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtPriceTotal.Text != "")
                {
                    if (!(txtPriceTotal.Text == "" || txtPriceTotal.Text == "0,00"))
                    {
                        string priceTotal = txtPriceTotal.Text;
                        priceTotal = priceTotal.Replace("٬", ".");

                        double price = Convert.ToDouble(priceTotal.Replace('٫', '.'), CultureInfo.InvariantCulture);

                        string PayPrice = txtPayPrice.Text;
                        PayPrice = PayPrice.Replace("٬", ".");

                        double pay = Convert.ToDouble(PayPrice.Replace('٫', '.'), CultureInfo.InvariantCulture);


                        double Price2 = pay - price;
                        txtProfit.Text = Price2.ToString("F2");
                    }

                }
                e.Handled = true;
            }
        }

        private void txtPriceTotal_Leave(object sender, EventArgs e)
        {
            if (txtPriceTotal.Text != "")
            {
                if (!(txtPriceTotal.Text == "" || txtPriceTotal.Text == "0,00"))
                {

                    string priceTotal = txtPriceTotal.Text;
                    priceTotal = priceTotal.Replace("٬", ".");

                    double price = Convert.ToDouble(priceTotal.Replace('٫', '.'), CultureInfo.InvariantCulture);

                    string PayPrice = txtPayPrice.Text;
                    PayPrice = PayPrice.Replace("٬", ".");

                    double pay = Convert.ToDouble(PayPrice.Replace('٫', '.'), CultureInfo.InvariantCulture);


                    double Price2 = pay - price;
                    txtProfit.Text = Price2.ToString("F2");
                }

            }



        }

        private void guna2DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            GetTotal();

            //if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            //{
            //    bool isSelected = dgvProductes.Rows[e.RowIndex].Selected;

            //    e.CellStyle.BackColor = isSelected ? checkedFillColor : backgroundPrimary;
            //    e.CellStyle.ForeColor = textColor;
            //}

        }

        private void GetTotal()
        {
            double purchPrice = 0;
            double sellPrice = 0;

            foreach (DataGridViewRow item in dgvProductes.Rows)
            {
                // سعر الشراء (العمود مخصص لسعر شراء الوحدة × الكمية)
                if (item.Cells["dgvPriceTotal"].Value != null && item.Cells["dgvQty"].Value != null)
                {
                    double buy = Convert.ToDouble(item.Cells["dgvPriceTotal"].Value.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                    double qty = Convert.ToDouble(item.Cells["dgvQty"].Value.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                    purchPrice += buy * qty;
                }

                // سعر البيع (سعر البيع × الكمية)
                if (item.Cells["dgvPrice"].Value != null && item.Cells["dgvQty"].Value != null)
                {
                    double sell = Convert.ToDouble(item.Cells["dgvPrice"].Value.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                    double qty = Convert.ToDouble(item.Cells["dgvQty"].Value.ToString().Replace('٫', '.'), CultureInfo.InvariantCulture);
                    sellPrice += sell * qty;
                }
            }

            txtPriceTotal.Text = purchPrice.ToString("F2");
            txtPayPrice.Text = sellPrice.ToString("F2");
            txtProfit.Text = (sellPrice - purchPrice).ToString("F2");

        }
        public event EventHandler ButtonCancel;

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected virtual void btnclse()
        {
            ButtonCancel?.Invoke(this, EventArgs.Empty);
        }

        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            textColor2 = MainClass.TextColor3;
            checkedFillColor = MainClass.CheckedFillColor;
            checkedForeColor = MainClass.CheckedForeColor;
        }
        //public void ThemeMode()
        //{
        //    ThemeColor();

        //    this.BackColor = backgroundPrimary;


        //    //Panels
        //    topPanel.BackColor = checkedFillColor;
        //    mainPanel.BackColor = backgroundPrimary;
        //    bottomPanel.BackColor = backgroundSecondary;


        //    //Text box
        //    txtPriceTotal.BackColor = backgroundPrimary;
        //    txtPriceTotal.ForeColor = textColor2;
        //    txtPriceTotal.BorderColor = checkedFillColor;
        //    txtPriceTotal.FillColor = backgroundPrimary;

        //    txtPayPrice.BackColor = backgroundPrimary;
        //    txtPayPrice.ForeColor = textColor2;
        //    txtPayPrice.BorderColor = checkedFillColor;
        //    txtPayPrice.FillColor = backgroundPrimary;

        //    txtProfit.BackColor = backgroundPrimary;
        //    txtProfit.ForeColor = textColor2;
        //    txtProfit.BorderColor = checkedFillColor;
        //    txtProfit.FillColor = backgroundPrimary;

        //    //labels 
        //    lblTitle.ForeColor = textColor2;
        //    lblPrice.ForeColor = textColor;
        //    lblPurchPrice.ForeColor = textColor;
        //    lblProfit.ForeColor = textColor;

        //    //-> datagride view 
        //    //dgvProductes.BackgroundColor = backgroundPrimary;
        //    //dgvProductes.GridColor = backgroundPrimary;

        //    //dgvProductes.DefaultCellStyle.BackColor = backgroundPrimary;
        //    //dgvProductes.DefaultCellStyle.ForeColor = textColor;
        //    //dgvProductes.DefaultCellStyle.SelectionBackColor = checkedFillColor;
        //    //dgvProductes.DefaultCellStyle.SelectionForeColor = textColor;

        //    //dgvProductes.ColumnHeadersDefaultCellStyle.BackColor = backgroundSecondary;
        //    //dgvProductes.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
        //    //dgvProductes.ColumnHeadersDefaultCellStyle.SelectionBackColor = checkedFillColor;

        //    //dgvProductes.RowsDefaultCellStyle.BackColor = backgroundPrimary;
        //    ////dgvProductes.AlternatingRowsDefaultCellStyle.BackColor = backgroundPrimary;
        //    //dgvProductes.RowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
        //    //dgvProductes.RowsDefaultCellStyle.ForeColor = textColor;
        //    //dgvProductes.RowsDefaultCellStyle.SelectionForeColor = textColor2;


        //    // Buttons
        //    btnPSave.FillColor = checkedFillColor;
        //    btnPSave.ForeColor = textColor2;

        //    btnExit.FillColor = Color.Red;
        //    btnExit.ForeColor = textColor;

        //    //GroupBox
        //    groupBox5.ForeColor = textColor;

        //}

        private void dgvProductes_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.BackColor = checkedFillColor;
            e.Control.ForeColor = textColor;
        }

        private void frmProductAdd2_SizeChanged(object sender, EventArgs e)
        {
            billPanel.Left = (mainPanel.Width - billPanel.Width) / 2;
            // billPanel.Top = (mainPanel.Height - billPanel.Height) / 2;
        }

        private void txtBillNumber_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBillNumber.Text) || string.IsNullOrEmpty(cbStore.Text) || string.IsNullOrEmpty(txtSupName.Text))
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;


            }
        }

       
        private void txtSupNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!int.TryParse(txtSupNumber.Text, out int storeNumber))
                {
                    txtSupNumber.Text = "0";
                    MessageBox.Show("لم يتم العثور على البيانات.");
                    txtSupName.Clear();
                    e.Handled = true;
                    return;
                }

                string qry = @"SELECT pName, pID FROM Parties WHERE supCode = @supCode";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@supCode", storeNumber);
                    DataTable dt2 = new DataTable();
                    using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                    {
                        da2.Fill(dt2);
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        txtSupName.Text = dt2.Rows[0]["pName"].ToString();
                        supplierID = Convert.ToInt32(dt2.Rows[0]["pID"]);

                        string qry2 = @"SELECT SUM(clear) AS TotalClear FROM billPrcheses WHERE supplierID = @supID";

                        using (SqlCommand cmd2 = new SqlCommand(qry2, con))
                        {
                            cmd2.Parameters.AddWithValue("@supID", supplierID);
                            DataTable dt3 = new DataTable();
                            using (SqlDataAdapter da3 = new SqlDataAdapter(cmd2))
                            {
                                da3.Fill(dt3);
                            }

                            if (dt3.Rows.Count > 0)
                                txtSumSupp.Text = dt3.Rows[0]["TotalClear"].ToString();
                            else
                                txtSumSupp.Text = "0";
                        }
                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على البيانات.");
                        txtSupName.Clear();
                    }
                }

                txtBillNumber.Focus();
                e.Handled = true;
            }
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            mainPanel.Enabled = true;
            btnAddProducts.Enabled = true;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;

            lastRowCheck();

            btnEdit1.Enabled = true;
            btnNext.Enabled = false;

            string query = @"
            SELECT billStatus
            FROM billPrcheses          
            WHERE billStatus = @value";

            using (SqlConnection con = MainClass.GetConnection()) // استخدام GetConnection
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@value", "UnderWork");

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                if (dt.Rows.Count == 0)
                {
                    btnDeleteBills.Enabled = false;
                    btnDeleteBills.Text = "لا يوجد فواتير غير مكتملة";
                }
                else
                {
                    btnDeleteBills.Enabled = true;
                    btnDeleteBills.Text = "حذف الفواتير الغير مكتملة" + " (" + dt.Rows.Count + ")";
                }
            }

        }
        private void lastRowCheck()
        {
            if (dgvProductes.Rows.Count > 0) // لو فيه صفوف
            {
                int lastRowIndex = dgvProductes.Rows.Count - 1; // آخر صف
                int colIndex = dgvProductes.Columns["dgvCode"].Index; // رقم العمود

                object val = dgvProductes.Rows[lastRowIndex].Cells["dgvproID"].Value;

                if (val != null && !string.IsNullOrWhiteSpace(val.ToString()))
                {
                    // لو الخلية فيها قيمة → أضف صف جديد
                    int newIndex = dgvProductes.Rows.Add();
                    dgvProductes.CurrentCell = dgvProductes.Rows[newIndex].Cells["dgvCode"];
                    dgvProductes.BeginEdit(true);
                }
                else
                {
                    // لو الخلية فاضية → افتح نفس الصف
                    dgvProductes.CurrentCell = dgvProductes.Rows[lastRowIndex].Cells[colIndex];
                    dgvProductes.BeginEdit(true);
                }
            }
            else // لو مفيش صفوف
            {
                int newIndex = dgvProductes.Rows.Add(); // أضف صف جديد
                dgvProductes.CurrentCell = dgvProductes.Rows[newIndex].Cells["dgvCode"]; // حط المؤشر على خلية الكود
                dgvProductes.BeginEdit(true); // افتح الخلية للكتابة
            }
        }
        private void btnEdit1_Click(object sender, EventArgs e)
        {
            mainPanel.Enabled = false;
            btnAddProducts.Enabled = false;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;

            btnEdit1.Enabled = false;
            btnNext.Enabled = true;
        }

        private void dgvProductes_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dgvProductes.Rows.Count > 0)
            {
                var firstRow = dgvProductes.Rows[0];

                if (firstRow.IsNewRow ||
                    firstRow.Cells["dgvName"].Value == null ||
                    string.IsNullOrWhiteSpace(firstRow.Cells["dgvName"].Value.ToString()))
                {
                    btnPSave.Enabled = false;
                }
                else
                {
                    btnPSave.Enabled = true;
                }
            }
            else
            {
                btnPSave.Enabled = false;
            }
        }


        private void dgvProductes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dgvProductes.Rows.Count > 0)
            {
                btnPSave.Enabled = true;
            }
            else
            {
                btnPSave.Enabled = false;
            }
        }
        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartyType", "%" + "مورد" + "%");

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);
                    foreach (DataRow row in dt2.Rows)
                    {
                        string name = row["pName"].ToString();
                        int id = Convert.ToInt32(row["pID"]);
                        dataSource.Add(name);
                        nameToID[name] = id;
                    }
                }

                txtSupName.AutoCompleteCustomSource = dataSource;
                txtSupName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtSupName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }

        }

        private void txtSupName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtSupName.Text))
            {
                selectedPartyID = nameToID[txtSupName.Text];
                btnEditParties.Enabled = true;

            }
            else
            {
                selectedPartyID = 0;
                btnEditParties.Enabled = false;
            }

            if (string.IsNullOrEmpty(txtBillNumber.Text) || string.IsNullOrEmpty(cbStore.Text) || string.IsNullOrEmpty(txtSupName.Text))
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;


            }
        }

        private void btnDeleteBills_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("هل أنت متأكد أنك تريد حذف جميع الفواتير الغير مكتملة؟",
                                                   "تأكيد الحذف",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string query = @"DELETE FROM billPrcheses WHERE billStatus = @status";

                using (SqlConnection con = MainClass.GetConnection()) // استخدام GetConnection
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = "UnderWork";

                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Notifier.ShowNotification("تم", "✅ تم حذف " + rowsAffected + " فاتورة غير مكتملة");
                            btnDeleteBills.Text = $"حذف الفواتير الغير مكتملة (0)";
                            btnDeleteBills.Enabled = false;
                        }
                        else
                        {
                            Notifier.ShowNotification("لا يوجد", "❌ لا توجد فواتير بحالة غير مكتملة");
                            btnDeleteBills.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Notifier.ShowNotification("خطأ", "حدث خطأ أثناء حذف الفواتير: " + ex.Message);
                    }
                }

            }
            else
            {
                Notifier.ShowNotification("إلغاء", "تم إلغاء عملية الحذف ❌");

            }

        }

        private void btnWithoutBarcode_Click(object sender, EventArgs e)
        {
            bool anyUnchecked = false;

            // أولاً نتحقق هل فيه أي صف غير محدد
            foreach (DataGridViewRow row in dgvProductes.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = row.Cells["dgvBarCode"].Value != null && (bool)row.Cells["dgvBarCode"].Value;
                if (!isChecked)
                {
                    anyUnchecked = true;
                    btnWithoutBarcode.Text = "الغاء تحديد الكل";

                    break;
                }
                btnWithoutBarcode.Text = "تحديد الكل";

            }

            // لو فيه أي صف غير محدد → نحدد كل الصفوف، وإلا نلغي التحديد
            bool newValue = anyUnchecked;

            foreach (DataGridViewRow row in dgvProductes.Rows)
            {
                if (row.IsNewRow) continue;

                row.Cells["dgvBarCode"].Value = newValue;
            }
        }

        private void btnAddParties_Click(object sender, EventArgs e)
        {
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.partyType = "مورد";
                frm.ShowDialog();

            }
            this.Show();
            this.Focus();
        }

        private void btnEditParties_Click(object sender, EventArgs e)
        {
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.pID = selectedPartyID; // Pass the selected party ID to the form
                frm.partyType = "مورد";
                frm.ShowDialog();

            }
            this.Show();
            this.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmPartesSearch frm = new frmPartesSearch(this);
            frm.type = "مورد";
            frm.ShowDialog();
            this.Focus();
        }
        public void resultSearch(string pName)
        {
            txtSupName.Text = pName;
        }

        private void dgvProductes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // نتحقق أنه هيدر فقط RowIndex = -1
            if (e.RowIndex == -1)
            {
                // لو العمود المحدد فقط
                if (dgvProductes.CurrentCell != null &&
                    e.ColumnIndex == dgvProductes.CurrentCell.ColumnIndex)
                {
                    e.Handled = true;

                    // ارسم خلفية الهيدر الافتراضية بدون أي تغيير
                    using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(0, 80, 80)))
                    {
                        e.Graphics.FillRectangle(backBrush, e.CellBounds);
                    }

                    // ارسم النص
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.FormattedValue?.ToString(),
                        new Font("Tahoma", 11, FontStyle.Bold),
                        e.CellBounds,
                        Color.FromArgb(204, 204, 204),  // لون الخط المحدد للهيدر
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    // ارسم الحدود
                    using (Pen p = new Pen(Color.DarkSlateGray))
                    {
                        e.Graphics.DrawRectangle(p, new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1));
                    }

                    return;
                }
            }
        }

        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.Visible = true;
            dgv.BringToFront();
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;

            // أحجام الخلايا والهيدر
            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 45;

            // الخطوط
            dgv.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

            // الألوان العادية للصفوف
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);

            // الصفوف المتبادلة (صف غامق وصف فاتح)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // الرمادي الفاتح
            dgv.RowsDefaultCellStyle.BackColor = Color.White;                              // الصف العادي

            // ألوان التحديد (Selection)
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // ✅ ألوان الهيدر (عادي + خط)
            dgv.EnableHeadersVisualStyles = false; // مهم عشان ألوانك تشتغل
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;                 // لون خط الهيدر

            // لون الهيدر وقت التحديد (لو حابب تخليه مختلف)
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

        }



        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            frmSearchProductToAdd frm = new frmSearchProductToAdd(this);
            frm.ShowDialog();
            this.Focus();
        }
        private void ProcessEnter(int rowIndex, string columnName)
        {
            // لو عندك لوجيك في CellEndEdit أو KeyDown مع Enter نفّذه هنا
            var cell = dgvProductes.Rows[rowIndex].Cells[columnName];

            // مثال: نرفع حدث CellEndEdit يدويًا
            guna2DataGridView2_CellEndEdit(dgvProductes, new DataGridViewCellEventArgs(cell.ColumnIndex, rowIndex));
        }
        public void resultSearchProduct(string code)
        {
            int rowIndex;

            // لو الـ DataGridView فاضي → نضيف أول صف
            if (dgvProductes.Rows.Count == 0)
                rowIndex = dgvProductes.Rows.Add();
            else
                rowIndex = dgvProductes.Rows.Count - 1; // آخر صف

            // حط الباركود الجديد أو المستعمل
            dgvProductes.Rows[rowIndex].Cells["dgvCode"].Value = code;

            // نفّذ نفس اللوجيك اللي بيشتغل مع Enter
            ProcessEnter(rowIndex, "dgvCode");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            dgvProductes.Rows.Clear();

            mainPanel.Enabled = false;
            btnAddProducts.Enabled = false;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;

            btnEdit1.Enabled = false;
            btnNext.Enabled = true;

            txtPriceTotal.Text = "0";
            txtPayPrice.Text = "0";
            txtProfit.Text = "0";
            cbStore.SelectedIndex = 0;
            txtSupName.Text = string.Empty;
            txtSupNumber.Text = string.Empty;
            txtSumSupp.Text = string.Empty;
            txtBillNumber.Text = string.Empty;
            txtNote.Text = string.Empty;

            dtPicker.Value = DateTime.Today;


            billID = 0;
        }
    }

}
