using DevExpress.CodeParser;
using DevExpress.Drawing.Internal.Fonts.Interop;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraMap.ItemEditor;
using DevExpress.XtraRichEdit.Fields;
using DevExpress.XtraRichEdit.Model;
using pos.Classes;
using pos.GeneralForms;
using pos.Model.POS;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Model.Stor
{
    public partial class frmAll_Bills : Form
    {
        private int mainID = 0;
        public string partyType;
        private int partiesID = 0;
        public bool pos = false;
        public string nvoiceCode;
        public string invoiceCode;
        public bool isFinancial = true;
        private bool showDeletedProducts = false; // لتحديد ما إذا كنت تريد عرض المنتجات المحذوفة أم لا
        private bool fromTax = false;
        public bool isDeleted = false;
        private decimal finalTotal;

        private bool typeInvoice = false; // true = فاتورة مبيعات ,false = فاتورة مشتريات
        private int searchType = 0; // 1 = بحث برقم الفاتورة , 2 = بحث بالتاريخ 
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        private bool showTax = false;
        frmPOS frmpos;
        private bool formAsBox = false;

        public frmAll_Bills(frmPOS frm)
        {
            InitializeComponent();
            btnSearchParty.Image = Properties.Resources.magnifying_glass;
            this.ShowInTaskbar = false;

            this.frmpos = frm;
        }

        public frmAll_Bills()
        {
            InitializeComponent();
            btnSearchParty.Image = Properties.Resources.magnifying_glass;
            this.ShowInTaskbar = false;

        }
        public frmAll_Bills(string invoce, string name, string partytype)
        {
            InitializeComponent();
            btnClose.Visible = true;
            btnShowAndHide.Visible = false;
            btnEditParties.Visible = false;
            btnDelete.Visible = false;
            showTax = true;
            this.ShowInTaskbar = false;
            formAsBox = true;

            gbSearch.Enabled = false;
            btnSearchParty.Image = Properties.Resources.magnifying_glass;
            invoiceCode = invoce;
            txtBillNumber.Text = invoce;
            //txtName.Text = name;

            // إخفاء عمود dgvDelet
            if (dgvProducts.Columns.Contains("dgvDelet"))
                dgvProducts.Columns["dgvDelet"].Visible = false;

            if (partytype == "عميل")
            {
                GetBillDataFromDB(invoiceCode, false);
            }
            else if (partytype == "مورد")
            {
                GetBillDataFromDB(invoiceCode, true);
            }
        }
        public frmAll_Bills(string partyName, int partyID, string partytype)
        {
            InitializeComponent();
            btnSearchParty.Image = Properties.Resources.magnifying_glass;
            btnClose.Visible = true;
            this.ShowInTaskbar = false;

            // إخفاء عمود dgvDelet
            if (dgvProducts.Columns.Contains("dgvDelet"))
                dgvProducts.Columns["dgvDelet"].Visible = false;
            txtName.Text = partyName;
            txtPartiesName.Text = partyName;
            partiesID = partyID;
            partyType = partytype;
            if (partytype == "عميل")
            {
                DisplayBillsAsync(1, false, isDeleted);
            }
            else if (partytype == "مورد")
            {
                DisplayBillsAsync(1, true, isDeleted);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                //cp.ExStyle |= 0x02000000;
                cp.ExStyle |= 0x80; // WS_EX_TOOLWINDOW
                return cp;
            }
        }

        private void frmAll_Bills_Load(object sender, EventArgs e)
        {
            dgvDelet.Image = Properties.Resources.delete_Red;
            btnEditParties.Image = Properties.Resources.edit_white;

            dtPickerStart.Value = DateTime.Today;
            dtPickerEnd.Value = DateTime.Today;

            dtPickerStart.Format = DateTimePickerFormat.Custom;
            dtPickerStart.CustomFormat = "yyyy-MM-dd";

            dtPickerEnd.Format = DateTimePickerFormat.Custom;
            dtPickerEnd.CustomFormat = "yyyy-MM-dd";
            txtBillNumber.Focus();
            ApplyGridStyle(dgvBills);
            ApplyGridStyle(dgvProducts);

            textSuggester();

            if (pos)
            {
                btnTax.Visible = false;
                btnEditParties.Visible = false;
                dgvProducts.Columns["dgvDelet"].Visible = true;
                txtStoreName.Enabled = false;
                lblStore.Enabled = false;
                txtNote.Enabled = false;
                lblNote.Enabled = false;

                return;
            }
            else if (showTax)
            {
                btnTax.Visible = false;
                dgvProducts.Columns["dgvDelet"].Visible = false;
            }
            else
            {
                btnTax.Visible = true;
                //btnEditParties.Visible = true;
                dgvProducts.Columns["dgvDelet"].Visible = false;

            }
            //if (isFinancial)
            //    GetBillDataFromDB(invoiceCode, false);

            if (partyType == "عميل")
            {
                txtStoreName.Enabled = false;
                lblStore.Enabled = false;
                txtNote.Enabled = false;
                lblNote.Enabled = false;

            }
            else if (partyType == "مورد")
            {
                txtStoreName.Enabled = true;
                lblStore.Enabled = true;
                txtNote.Enabled = true;
                lblNote.Enabled = true;

                btnEditParties.Visible = false;
                btnTax.Visible = false;

            }
            if (isDeleted)
            {
                btnTax.Visible = false;
                btnEditParties.Visible = false;
                btnPrint.Visible = false;
                btnPos.Visible = true;
                btnPos.Text = "حذف الكل";
            }
        }
        private void textSuggester()
        {
            string qry = @"SELECT pID, pName 
               FROM Parties 
               WHERE PartyType LIKE @PartyType 
               AND pName LIKE @keyword";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@PartyType", "%" + partyType + "%");
                    cmd.Parameters.AddWithValue("@keyword", "%" + txtName.Text + "%");
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

                    txtName.AutoCompleteCustomSource = dataSource;
                    txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
            }



        }
        private void CenterPanel(Panel panel, Panel mainPanel)
        {
            panel.Left = (mainPanel.Width - panel.Width) / 2;
            // myGroupBox.Top = (panel.Height - myGroupBox.Height) / 2;
        }




        private void btnEditParties_Click(object sender, EventArgs e)
        {

            frmBlackout frmblackout = new frmBlackout(this);
            frmblackout.Show();
            frmblackout.Owner = this;

            using (frmPayWays frm = new frmPayWays())
            {
                frm.mainID = this.mainID;
                frm.partyType = partyType;
                frm.status = "update";

                frm.Owner = this;
                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {


                }
                else
                {

                }
            }
            this.Focus();
            frmblackout.Close();
        }

        private void frmAll_Bills_SizeChanged(object sender, EventArgs e)
        {
            CenterPanel(panel1, detellPanel);
            CenterPanel(panel2, billPanel);

        }

        private async void txtBillNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                invoiceCode = txtBillNumber.Text;

                if (partyType == "عميل")
                {
                    await GetBillDataFromDB(invoiceCode, false);

                }
                else if (partyType == "مورد")
                {
                    await GetBillDataFromDB(invoiceCode, true);

                }
            }

        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            lblNotefi.Visible = false;

            if (partyType == "عميل")
            {
                if (partiesID > 0)
                {
                    currentPage = 0;
                    hasMoreData = true;
                    isLoading = false;
                    await DisplayBillsAsync(1, false, isDeleted);
                    searchType = 1;
                    typeInvoice = false;
                }
                else if (partyType == "مورد")
                {
                    await GetBillDataFromDB(invoiceCode, false);
                    invoiceCode = txtBillNumber.Text;
                }

            }
            else
            {
                if (partiesID > 0)
                {
                    currentPage = 0;
                    hasMoreData = true;
                    isLoading = false;
                    await DisplayBillsAsync(1, true, isDeleted);
                    searchType = 1;
                    typeInvoice = true;
                }
                else
                {
                    await GetBillDataFromDB(invoiceCode, true);
                    invoiceCode = txtBillNumber.Text;
                }
            }

            if (isDeleted)
            {
                btnPos.Text = "حذف الكل";
                btnPos.Enabled = true;
                btnPos.FillColor = Color.Red;
            }

        }

        private async void btnBillSearch_Click(object sender, EventArgs e)
        {
            if (partyType == "عميل")
            {
                currentPage = 0;
                hasMoreData = true;
                isLoading = false;
                await DisplayBillsAsync(2, false, isDeleted);
                searchType = 2;
                typeInvoice = false;
            }
            else if (partyType == "مورد")
            {
                currentPage = 0;
                hasMoreData = true;
                isLoading = false;
                await DisplayBillsAsync(2, true, isDeleted);
                searchType = 2;
                typeInvoice = true;
            }
            lblNotefi.Visible = false;


            if (isDeleted)
            {
                btnPos.Text = "حذف الكل";
                btnPos.Enabled = true;
                btnPos.FillColor = Color.Red;
            }

        }

        private void txtClean_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;

        }

        private async Task GetBillDataFromDB(string InvoiceCode, bool isSupplier)
        {
            dgvProducts.Visible = true;
            dgvBills.Visible = false;
            btnPrint.Enabled = false;
            btnShowAndHide.Enabled = false;

            btnDelete.Enabled = true;
            if (pos)
            {
                btnTax.Visible = false;
            }
            else
            {
                btnTax.Enabled = true;
            }

            detellPanel.Height = 462;
            panel1.Height = 443;
            billPanel.Dock = DockStyle.None;
            billPanel.Top = detellPanel.Bottom + 5;
            billPanel.Height = this.mainPanel.Height - 490;
            panel2.Height = billPanel.Height;

            dgvProducts.Width = 1426;
            dgvProducts.Height = panel2.Height - 48;
            dgvProducts.ScrollBars = ScrollBars.Vertical;
            int x = (panel2.Width - dgvProducts.Width) / 2;
            int y = 0;
            dgvProducts.Location = new Point(x, y);

            // اختيار الاستعلام حسب نوع الفاتورة (مورد او عميل)
            string qry;
            if (!isSupplier)
            {
                qry = @"
            SELECT m.MainID, m.partiesID, m.InvoiceCode, m.total, m.priceClear, m.descount, m.descountValue,
                   m.PaidAmount, m.PaymentMethod, m.CreditBalance, m.TotalWithInterest, m.InterestAmount,
                   m.aTime, m.aDate, m.change, m.previousDebitBalance, m.currentDebitBalance,
                   m.updateDate, m.updateTime, m.latePayTax, m.descountForBill,
                   p.pName, p.pPhone, p.pAdderss,
                   s1.sName AS CreatedByStaffName, s2.sName AS UpdatedByStaffName
            FROM tblMain1 AS m
            LEFT JOIN Parties AS p ON m.partiesID = p.pID
            LEFT JOIN shifts sh1 ON m.shiftID = sh1.ID
            LEFT JOIN staff s1 ON sh1.staffID = s1.staffID
            LEFT JOIN shifts sh2 ON m.shiftDoUpdate = sh2.ID
            LEFT JOIN staff s2 ON sh2.staffID = s2.staffID
            WHERE m.InvoiceCode = @InvoiceCode;";
            }
            else
            {
                qry = @"
            SELECT b.bID AS MainID, b.supplierID AS partiesID, a.storeName, b.notes,b.InvoiceCode,
                   b.total, b.clear AS priceClear, 0 AS descount, 0 AS descountValue,
                   0 AS PaidAmount, b.payWay AS PaymentMethod, 0 AS CreditBalance,
                   b.total AS TotalWithInterest, 0 AS InterestAmount,
                   b.Time AS aTime, b.date AS aDate, b.change, 
				   b.currentDebitBalance, b.previousDebitBalance,
                   NULL AS updateDate, NULL AS updateTime, 
                   0 AS latePayTax, 0 AS descountForBill,
                   p.pName AS pName, '' AS pPhone, '' AS pAdderss,
                   s1.sName AS CreatedByStaffName, '' AS UpdatedByStaffName
            FROM billPrcheses b
            LEFT JOIN shifts sh1 ON b.shiftID = sh1.ID
            LEFT JOIN staff s1 ON sh1.staffID = s1.staffID
            LEFT JOIN Parties AS p ON b.supplierID = p.pID
            INNER JOIN addStore a ON a.storeID = b.storeID
            WHERE b.InvoiceCode = @InvoiceCode;";
            }

            using (SqlConnection con = MainClass.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@InvoiceCode", invoiceCode); // ← هنا تحط قيمة المتغير

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.mainID = reader["MainID"] != DBNull.Value ? Convert.ToInt32(reader["MainID"]) : 0;
                            partiesID = reader["partiesID"] != DBNull.Value ? Convert.ToInt32(reader["partiesID"]) : 0;

                            txtPriceTotal.Text = SafeDecimal(reader["total"]);
                            txtClean.Text = SafeDecimal(reader["TotalWithInterest"]);
                            txtDV.Text = SafeDecimal(reader["descountValue"]);
                            txtPayWay.Text = SafeString(reader["PaymentMethod"]);
                            txtPartiesName.Text = SafeString(reader["pName"]);
                            txtDate.Text = SafeDate(reader["aDate"]);
                            txtTime.Text = SafeString(reader["aTime"]);
                            txtChange.Text = SafeDecimal(reader["change"]);
                            txtCurrentDebitBalance.Text = SafeDecimal(reader["currentDebitBalance"]);
                            txtPreviousBebitBalance.Text = SafeDecimal(reader["previousDebitBalance"]);
                            txtStaffName.Text = SafeString(reader["CreatedByStaffName"]);
                            invoiceCode = SafeString(reader["InvoiceCode"]);
                            if (isSupplier)
                            {
                                txtStoreName.Text = SafeString(reader["storeName"]);
                                txtNote.Text = SafeString(reader["notes"]);
                                txtPay2.Text = SafeDecimal(reader["total"]);
                            }
                            else
                            {
                                txtPay2.Text = SafeDecimal(reader["PaidAmount"]);
                                txtTaxValue.Text = SafeDecimal(reader["latePayTax"]);
                                txtLastUpdateName.Text = SafeString(reader["UpdatedByStaffName"]);
                                txtLastUpdateDate.Text = SafeDate(reader["updateDate"]);
                                txtLastUpdateTime.Text = SafeString(reader["updateTime"]);
                            }

                        }
                    }
                }
            }


            dgvProducts.Rows.Clear();
            cachedProductsTable.Clear();
            await LoadProductsPagedAsync(isSupplier);
        }


        private string SafeDecimal(object value, string format = "N0")
        {
            return value != DBNull.Value ? Convert.ToDecimal(value).ToString(format) : "0";
        }

        private string SafeString(object value)
        {
            return value != DBNull.Value ? value.ToString() : "";
        }

        private string SafeDate(object value, string format = "yyyy-MM-dd")
        {
            return value != DBNull.Value ? Convert.ToDateTime(value).ToString(format) : "";
        }
        // 📌 عدد الصفوف في الصفحة
        private const int PageSizeProducts = 15;

        // 📌 المتغيرات الخاصة بالتحميل على أجزاء
        private int currentPageProducts = 0;
        private int totalRowsProducts = 0;
        private DataTable cachedProductsTable = new DataTable();

        private async Task LoadProductsPagedAsync(bool isSupplier = false)
        {
            try
            {
                // 🟢 تحميل البيانات كاملة مرة واحدة من SQL لكن نخزنها في DataTable
                if (cachedProductsTable.Rows.Count == 0)
                {
                    string qry;
                    if (!isSupplier)
                    {
                        qry = @"SELECT 
                     d.DetailID,
                     CASE 
                         WHEN d.proID = 0 THEN d.proName
                         ELSE p.pName
                     END AS pName,
                     p.pID,
                     c.catName,
                     d.unite,
                     d.uniteID,
                     d.qty,
                     d.returnQty,
                     d.priceUnDis AS price,
                     d.amount,
                     d.pDescount,
                     d.vDescount,
                     d.DeleteFlag,
                     d.pTax,
                     d.vTax,
                     d.isUsed,
                     d.priceAfterDes
                 FROM tblMain1 m
                 INNER JOIN tblDetails d ON m.MainID = d.MainID
                 LEFT JOIN products p ON p.pID = d.proID
                 LEFT JOIN category c ON c.catID = p.categoryID
                 WHERE m.MainID = @mainID";
                    }
                    else
                    {
                        qry = @"SELECT 
                     d.DetailID, 
                     p.pName, 
                     p.pID, 
                     c.catName, 
                     d.unite,
                     d.uniteID, 
                     d.qty, 
                     d.returnQty, 
                     d.price, 
                     d.amount,
                     d.pDescount, 
                     d.vDescount, 
                     d.DeleteFlag, 
                     d.pTax, 
                     d.vTax,
                     d.isUsed, 
                     d.priceAfterDes
                 FROM billPrcheses b
                 INNER JOIN tblDetailsSupliser d ON b.bID = d.billPrchesesID
                 INNER JOIN products p ON p.pID = d.proID
                 INNER JOIN category c ON c.catID = p.categoryID
                 WHERE b.bID = @mainID";
                    }

                    SqlParameter[] parameters = { new SqlParameter("@mainID", this.mainID) };
                    cachedProductsTable = await Task.Run(() => LoadDataReturn(qry, parameters));
                    totalRowsProducts = cachedProductsTable.Rows.Count;
                    currentPageProducts = 0; // reset
                }

                // 🟢 تحميل الصفحة المطلوبة
                int startIndex = currentPageProducts * PageSizeProducts;
                int endIndex = Math.Min(startIndex + PageSizeProducts, totalRowsProducts);
                // ✅ يجب تنفيذه قبل اللوب وليس داخله
                dgvProducts.Columns["dgvReturnQty"].ReadOnly = !pos;

                // ✅ قبل إضافة الصفوف
                dgvProducts.SuspendLayout();

                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow row = cachedProductsTable.Rows[i];

                    string status = row["isUsed"] != DBNull.Value && (bool)row["isUsed"] ? "مستعمل" : "جديد";
                    bool isDelete = row["DeleteFlag"] != DBNull.Value && (bool)row["DeleteFlag"];

                    double returnQty = row["returnQty"] == DBNull.Value ? 0 : Convert.ToDouble(row["returnQty"]);

                    int idx = dgvProducts.Rows.Add(
                        dgvProducts.Rows.Count + 1,
                        row["DetailID"],
                        row["pID"],
                        row["pName"],
                        status,
                        row["catName"],
                        row["unite"],
                        row["qty"],
                        returnQty,
                        row["price"],
                        row["amount"],
                        row["pDescount"],
                        row["vDescount"],
                        row["pTax"],
                        row["vTax"],
                        row["priceAfterDes"],
                        isDelete,
                        row["uniteID"],
                        row["isUsed"],
                        returnQty
                    );

                    if (isDelete)
                        dgvProducts.Rows[idx].Visible = false;
                }

                // ✅ بعد الإضافة
                dgvProducts.ResumeLayout();

                // 🟢 إيقاف الفرز
                foreach (DataGridViewColumn column in dgvProducts.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // ✅ شرط عدد الصفوف
                if (dgvProducts.Rows.Count > 0)
                {
                    btnPrint.Enabled = true;
                    btnShowAndHide.Enabled = true;
                }
                else
                {
                    btnPrint.Enabled = false; // لو عايز تعكس الحالة لما يكون الجدول فاضي
                    btnShowAndHide.Enabled = false;
                }

                currentPageProducts++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل المنتجات: " + ex.Message);
            }
        }

        private int currentPage = 0;
        private int pageSize = 20;   // عدد السجلات في كل صفحة
        private bool hasMoreData = true;
        private bool isLoading = false;


        private async Task DisplayBillsAsync
            (
             int searchMode,
             bool isPurchase, // false = مبيعات , true = مشتريات
             bool isDeletedFlag
            )
        {
            if (isLoading || !hasMoreData) return; // ✅ منع التكرار

            isLoading = true;

            dgvProducts.Visible = false;
            dgvBills.Visible = true;
            btnDelete.Enabled = btnTax.Enabled = btnShowAndHide.Enabled = false;
            btnPrint.Enabled = false;
            btnShowAndHide.Enabled = false;

            detellPanel.Height = 231;
            panel1.Height = 231;
            billPanel.Dock = DockStyle.None;
            billPanel.Top = detellPanel.Bottom + 5;
            billPanel.Height = this.mainPanel.Height - 260;
            panel2.Height = billPanel.Height;

            dgvBills.Width = 1426;
            dgvBills.Height = panel2.Height - 48;
            dgvBills.Location = new Point((panel2.Width - dgvBills.Width) / 2, 0);

            if (currentPage == 0) // 🧹 امسح القديم أول مرة بس
            {
                dgvBills.Rows.Clear();
                dgvBills.Refresh();
            }



            try
            {
                string qry;
                List<SqlParameter> parameters = new List<SqlParameter>();

                int offset = currentPage * pageSize;

                if (isPurchase) // ✅ مشتريات
                {
                    qry = @"
            SELECT     
                b.bID AS MainID,
                b.InvoiceCode,
                b.payWay AS PaymentMethod,
                b.total,
                0 AS descount,
                0 AS descountValue,
                0 AS priceClear,
                0 AS InterestAmount,
                0 AS TotalWithInterest,
                b.[date] AS aDate,
                b.[time] AS aTime,
                b.updateDate,
                ISNULL(p.pName, N'غير محدد') AS pName,
                p.pID
            FROM billPrcheses b
            LEFT JOIN Parties p ON p.pID = b.supplierID
            WHERE billStatus = 'Finish' AND ";

                    qry += (!isDeletedFlag) ? "(b.DeleteFlag IS NULL OR b.DeleteFlag = 0)" : "b.DeleteFlag = 1";

                    if (searchMode == 1 && partiesID != 0)
                    {
                        qry += " AND b.supplierID = @partiesID";
                        parameters.Add(new SqlParameter("@partiesID", partiesID));
                        dgvBills.Columns["dgvNameParties"].Visible = false;
                    }
                    else if (searchMode == 2)
                    {
                        qry += isDeletedFlag ?
                               " AND (b.updateDate BETWEEN @startDate AND @endDate)" :
                               " AND (b.[date] BETWEEN @startDate AND @endDate)";
                        parameters.Add(new SqlParameter("@startDate", dtPickerStart.Value.Date));
                        parameters.Add(new SqlParameter("@endDate", dtPickerEnd.Value.Date.AddDays(1).AddSeconds(-1)));
                        dgvBills.Columns["dgvNameParties"].Visible = true;
                    }
                    else if (searchMode == 3 && partiesID != 0)
                    {
                        qry += " AND b.supplierID = @partiesID";
                        qry += isDeletedFlag ?
                               " AND b.updateDate BETWEEN @startDate AND @endDate" :
                               " AND b.[date] BETWEEN @startDate AND @endDate";
                        parameters.Add(new SqlParameter("@partiesID", partiesID));
                        parameters.Add(new SqlParameter("@startDate", dtPickerStart.Value.Date));
                        parameters.Add(new SqlParameter("@endDate", dtPickerEnd.Value.Date.AddDays(1).AddSeconds(-1)));
                        dgvBills.Columns["dgvNameParties"].Visible = false;
                    }

                    qry += isDeletedFlag ? " ORDER BY b.updateDate" : " ORDER BY b.[date]";

                }
                else // ✅ مبيعات
                {
                    qry = @"
            SELECT     
                m.MainID,
                m.InvoiceCode,
                m.PaymentMethod,
                m.total,
                m.descount,
                m.descountValue,
                m.priceClear,
                m.InterestAmount,
                m.TotalWithInterest,
                m.aDate,
                m.aTime,
                m.updateDate,
                ISNULL(p.pName, N'غير محدد') AS pName,
                p.pID
            FROM tblMain1 m
            LEFT JOIN Parties p ON p.pID = m.partiesID
            WHERE m.status LIKE 'finshed' AND ";

                    qry += (!isDeletedFlag) ? "(m.DeleteFlag IS NULL OR m.DeleteFlag = 0)" : "m.DeleteFlag = 1";

                    if (searchMode == 1 && partiesID != 0)
                    {
                        qry += " AND m.partiesID = @partiesID";
                        parameters.Add(new SqlParameter("@partiesID", partiesID));
                        dgvBills.Columns["dgvNameParties"].Visible = false;
                    }
                    else if (searchMode == 2)
                    {
                        qry += isDeletedFlag ?
                               " AND (m.updateDate BETWEEN @startDate AND @endDate)" :
                               " AND (m.aDate BETWEEN @startDate AND @endDate)";
                        parameters.Add(new SqlParameter("@startDate", dtPickerStart.Value.Date));
                        parameters.Add(new SqlParameter("@endDate", dtPickerEnd.Value.Date.AddDays(1).AddSeconds(-1)));
                        dgvBills.Columns["dgvNameParties"].Visible = true;
                    }
                    else if (searchMode == 3 && partiesID != 0)
                    {
                        qry += " AND m.partiesID = @partiesID";
                        qry += isDeletedFlag ?
                               " AND m.updateDate BETWEEN @startDate AND @endDate" :
                               " AND m.aDate BETWEEN @startDate AND @endDate";
                        parameters.Add(new SqlParameter("@partiesID", partiesID));
                        parameters.Add(new SqlParameter("@startDate", dtPickerStart.Value.Date));
                        parameters.Add(new SqlParameter("@endDate", dtPickerEnd.Value.Date.AddDays(1).AddSeconds(-1)));
                        dgvBills.Columns["dgvNameParties"].Visible = false;
                    }

                    qry += isDeletedFlag ? " ORDER BY m.updateDate" : " ORDER BY m.aDate";
                }

                // ✅ إضافة التحميل على أجزاء
                qry += " OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";
                parameters.Add(new SqlParameter("@offset", offset));
                parameters.Add(new SqlParameter("@limit", pageSize));

                DataTable dt = await Task.Run(() =>
                {
                    DataTable table = new DataTable();
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                            con.Open();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(table);
                            }
                        }
                    }

                    return table;
                });

                int rowIndex = dgvBills.Rows.Count + 1;
                foreach (DataRow row in dt.Rows)
                {
                    dgvBills.Rows.Add(
                        rowIndex++,
                        row["MainID"],
                        row["pID"],
                        row["pName"],
                        row["InvoiceCode"],
                        row["PaymentMethod"],
                        row["total"],
                        row["descount"],
                        row["descountValue"],
                        row["priceClear"],
                        row["InterestAmount"],
                        row["TotalWithInterest"],
                        row["aDate"] == DBNull.Value ? "" : Convert.ToDateTime(row["aDate"]).ToString("yyyy-MM-dd"),
                        row["aTime"]
                    );
                }

                if (dt.Rows.Count < pageSize) // ✅ لو أقل من حجم الصفحة → مفيش صفحات تانية
                    hasMoreData = false;

                currentPage++; // ✅ جاهز يجيب الصفحة اللي بعدها

                foreach (DataGridViewColumn column in dgvBills.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل البيانات\n" + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }


        public static DataTable LoadDataReturn(string qry, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }




        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtName.Text))
                partiesID = nameToID[txtName.Text];
            else
                partiesID = 0;
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private async void txtName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (partyType == "عميل")
                {
                    currentPage = 0;
                    hasMoreData = true;
                    isLoading = false;
                    await DisplayBillsAsync(1, false, isDeleted);
                    searchType = 1;
                    typeInvoice = false;
                }
                else if (partyType == "مورد")
                {
                    currentPage = 0;
                    hasMoreData = true;
                    isLoading = false;
                    await DisplayBillsAsync(3, true, isDeleted);
                    searchType = 3;
                    typeInvoice = true;
                }

                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private async void dgvBills_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cellValue = dgvBills.Rows[e.RowIndex].Cells["dgvBillNumber"].Value.ToString();
                var cellValue2 = dgvBills.Rows[e.RowIndex].Cells["dgvPartiesID"].Value;

                if (cellValue2 == null || string.IsNullOrWhiteSpace(cellValue2.ToString()))
                {
                    partiesID = 0; // قيمة افتراضية لو الخلية فاضية

                }
                else
                    partiesID = Convert.ToInt32(cellValue2);


                invoiceCode = cellValue;

                if (partyType == "عميل")
                    await GetBillDataFromDB(cellValue, false);
                else if (partyType == "مورد")
                    await GetBillDataFromDB(cellValue, true);

            }

        }

        private async void btnSearchByNameAndDate_Click(object sender, EventArgs e)
        {
            lblNotefi.Visible = false;
            if (partiesID > 0)
            {
                if (partyType == "عميل")
                {
                    currentPage = 0;
                    hasMoreData = true;
                    isLoading = false;
                    await DisplayBillsAsync(3, false, isDeleted);
                    searchType = 3;
                    typeInvoice = false;
                }
                else if (partyType == "مورد")
                {
                    currentPage = 0;
                    hasMoreData = true;
                    isLoading = false;
                    await DisplayBillsAsync(3, true, isDeleted);
                    searchType = 3;
                    typeInvoice = true;
                }
            }



            if (isDeleted)
            {
                btnPos.Text = "حذف الكل";
                btnPos.Enabled = true;
                btnPos.FillColor = Color.Red;
            }

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            double paid = double.TryParse(txtPay2.Text, out var parsed) ? parsed : 0;
            double total = double.TryParse(txtClean.Text, out var parsed2) ? parsed2 : 0;
            double oldBalance = double.TryParse(txtPreviousBebitBalance.Text, out var parsed3) ? parsed3 : 0;


            if (isDeleted)
            {
                DialogResult result = MessageBox.Show(
                                                     "هل أنت متأكد أنك تريد حذف الفاتورة؟",
                                                     "تأكيد الحذف",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string qry1;
                    if (partyType == "عميل")
                        qry1 = @"DELETE FROM tblMain1 WHERE MainID = @ID;
                 DELETE FROM tblDetails WHERE MainID = @ID;";
                    else
                        qry1 = @"DELETE FROM billPrcheses WHERE bID = @ID;
                 DELETE FROM tblDetailsSupliser WHERE billPrchesesID = @ID;";

                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand(qry1, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", mainID);

                            if (con.State == ConnectionState.Closed)
                                con.Open();

                            cmd.ExecuteNonQuery();

                            if (con.State == ConnectionState.Open)
                                con.Close();
                        }
                    }

                    frmCleanData();

                    Notifier.ShowNotification("حذف", "تم حذف الفاتورة بنجاح ✅");
                }
                else
                {

                }

            }
            else
            {
                DialogResult result = MessageBox.Show(
                                                     "هل أنت متأكد أنك تريد حذف الفاتورة؟",
                                                     "تأكيد الحذف",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    double change = double.TryParse(txtPay2.Text, out var parsedPrev) ? parsedPrev : 0;
                    await transactionStore();



                    if (partyType == "عميل")
                    {
                        await billFlagDeleteCustomerAsync(true);
                        await residualFullReturnAsync(mainID, false);

                    }
                    else if (partyType == "مورد")
                    {
                        await billFlagDeleteSuplieserAsync(true);
                        await residualFullReturnSuppliersAsync(mainID, false);


                    }




                    Notifier.ShowNotification("حذف", "تم نقل الفاتوره الي الفواتير المحذوفة ✅");
                }
                else
                {
                }
            }

        }

        private async Task residualAsync(double change, double paid, double total, double OldBalance)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                await Task.Run(async () =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();

                        int isCustomerValue = (partyType == "عميل") ? 1 : 0;
                        double previousBalance = 0;
                        double currentBalance = 0;

                        // 🔹 1. التحقق من وجود PartiesID
                        string queryCheck = "SELECT COUNT(*) FROM residualTable WHERE PartiesID = @partiesID";
                        using (SqlCommand checkCmd = new SqlCommand(queryCheck, con))
                        {
                            checkCmd.Parameters.AddWithValue("@partiesID", partiesID);
                            int count = (int)checkCmd.ExecuteScalar();

                            if (count == 0)
                            {
                                string insertQuery = @"
                            INSERT INTO residualTable
                            (PartiesID, status, isCustomer, totalPaid, totalTransaction, previousDebitBalance, currentDebitBalance)
                            VALUES
                            (@partiesID, N'دائن', @isCustomer, @totalPaid, 0, 0, 0);";

                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                                {
                                    insertCmd.Parameters.AddWithValue("@partiesID", partiesID);
                                    insertCmd.Parameters.AddWithValue("@isCustomer", isCustomerValue);
                                    insertCmd.Parameters.AddWithValue("@totalPaid", paid);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // 🔹 2. تحديث الرصيد والحصول على الرصيدين
                        string queryResidual = @"
                    UPDATE residualTable
                    SET 
                        previousDebitBalance = ISNULL(currentDebitBalance, 0),
                        currentDebitBalance = ISNULL(currentDebitBalance, 0) - @deductValue
                    OUTPUT 
                        DELETED.currentDebitBalance AS previousBalance,
                        INSERTED.currentDebitBalance AS currentBalance
                    WHERE PartiesID = @partiesID;
                ";

                        using (SqlCommand cmd1 = new SqlCommand(queryResidual, con))
                        {
                            cmd1.Parameters.AddWithValue("@deductValue", change);
                            cmd1.Parameters.AddWithValue("@partiesID", partiesID);

                            using (SqlDataReader reader = cmd1.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    previousBalance = Convert.ToDouble(reader["previousBalance"]);
                                    currentBalance = Convert.ToDouble(reader["currentBalance"]);
                                }
                            }
                        }

                        // 🔹 3. تحديث tblMain1 بالرصد الجديد
                        string queryMain;
                        if (partyType == "عميل")
                        {
                            queryMain = @"
                            UPDATE tblMain1
                            SET currentDebitBalance = @currentBalance
                            WHERE MainID = @mainID;";
                        }
                        else
                        {
                            queryMain = @"
                            UPDATE billPrcheses
                            SET currentDebitBalance = @currentBalance
                            WHERE bID = @mainID;";
                        }

                        using (SqlCommand cmd2 = new SqlCommand(queryMain, con))
                        {
                            cmd2.Parameters.AddWithValue("@currentBalance", currentBalance);
                            cmd2.Parameters.AddWithValue("@mainID", mainID);
                            cmd2.ExecuteNonQuery();
                        }

                        double remainderBill = OldBalance + total - paid - change;

                        // ⚙️ تسجيل عملية partiesTransfare (خارج UI)
                        await partiesTransfareAsync(change, currentBalance, previousBalance, false);

                        // 🔹 4. معالجة الرصيد السالب داخل نفس السياق
                        //if (remainderBill < 0 && currentBalance < 0)
                        if (currentBalance < 0)
                        {
                            // ⚠️ نجهّز الرسالة خارج الـ Task (نرسلها لاحقًا للـ UI)
                            string message = $"هل تريد سحب رصيد الدائن الحالي؟\n\nالرصيد السابق: {previousBalance:N1}\nالرصيد الحالي: {currentBalance:N1}";
                            string newStatus = "دائن";
                            bool withdraw = false;

                            // ❗ إظهار الرسالة لازم يكون في UI Thread
                            System.Windows.Forms.DialogResult result2 = DialogResult.None;
                            MainClass.SafeInvoke(() =>
                            {
                                result2 = MessageBox.Show(message, "تأكيد السحب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            });

                            if (result2 == DialogResult.Yes)
                            {
                                newStatus = "مسدد";
                                withdraw = true;

                                // 🔹 3. تحديث tblMain1 بالرصد الجديد

                                using (SqlCommand cmd2 = new SqlCommand(queryMain, con))
                                {
                                    cmd2.Parameters.AddWithValue("@currentBalance", 0);
                                    cmd2.Parameters.AddWithValue("@mainID", mainID);
                                    cmd2.ExecuteNonQuery();
                                }
                            }

                            using (SqlTransaction tran = con.BeginTransaction())
                            {
                                try
                                {
                                    // 🔸 تحديث الحالة في residualTable
                                    string query2 = @"
                                UPDATE residualTable
                                SET currentDebitBalance = CASE WHEN @status = N'مسدد' THEN 0 ELSE currentDebitBalance END,
                                    status = @status
                                WHERE PartiesID = @partiesID;";

                                    using (SqlCommand cmdUpdate = new SqlCommand(query2, con, tran))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@status", newStatus);
                                        cmdUpdate.Parameters.AddWithValue("@partiesID", partiesID);
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    // 🔸 تسجيل الحركة
                                    string qtyTransaction = @"
                                INSERT INTO PartiesTransactions
                                    (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
                                VALUES
                                    (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID, 
                                    CAST(GETDATE() AS DATE), @aTime);";

                                    using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                                    {
                                        cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                                        cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                                        cmdTransaction.Parameters.AddWithValue("@mainID", mainID);
                                        cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                                        if (withdraw)
                                        {
                                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "سحب");
                                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo", $"تم سحب كل رصيد الدائن بقيمة {currentBalance:N0}");
                                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", currentBalance);
                                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", 0.0);
                                        }
                                        else
                                        {
                                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "ايداع");
                                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo", $"تم ايداع رصيد بقيمة {change:N0}");
                                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", previousBalance);
                                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", currentBalance);
                                        }

                                        cmdTransaction.ExecuteNonQuery();
                                    }

                                    tran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tran.Rollback();
                                    MainClass.SafeInvoke(() =>
                                    {
                                        MessageBox.Show("حدث خطأ أثناء تسجيل الحركة: " + ex.Message);
                                    });
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ في دالة residual:\n" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }



        private async Task residualFullReturnAsync(int mainID, bool Retruns)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();

                        double invoiceIssuanceValue = 0;   // إجمالي الفاتورة وقت الإصدار
                        double currentTotal = 0;           // الإجمالي الحالي بعد المرتجعات الجزئية
                        double paidAmount = 0;             // المبلغ المدفوع
                        double previousBalance = 0;
                        double currentBalance = 0;

                        // 🟢 1. قراءة بيانات الفاتورة
                        using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        ISNULL(InvoiceIssuanceValue, 0) AS InvoiceIssuanceValue,
                        ISNULL(TotalWithInterest, 0) AS currentTotal,
                        ISNULL(PaidAmount, 0) AS PaidAmount
                    FROM tblMain1
                    WHERE MainID = @mainID;", con))
                        {
                            cmd.Parameters.AddWithValue("@mainID", mainID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    invoiceIssuanceValue = Convert.ToDouble(reader["InvoiceIssuanceValue"]);
                                    currentTotal = Convert.ToDouble(reader["currentTotal"]);
                                    paidAmount = Convert.ToDouble(reader["PaidAmount"]);
                                }
                            }
                        }

                        // 🧮 2. حساب الخصم الصحيح (المتبقي بعد المدفوعات والمرتجعات)
                        double alreadyDeducted = invoiceIssuanceValue - currentTotal;
                        double remainingDebt = (invoiceIssuanceValue - paidAmount) - alreadyDeducted;
                        double amountToDeduct = remainingDebt + paidAmount;


                        // 🟢 3. تحديث residualTable وخصم المبلغ
                        string updateResidual = @"
                    UPDATE residualTable
                    SET 
                        previousDebitBalance = ISNULL(currentDebitBalance, 0),
                        currentDebitBalance = ISNULL(currentDebitBalance, 0) - @deductValue
                    OUTPUT 
                        DELETED.currentDebitBalance AS previousBalance,
                        INSERTED.currentDebitBalance AS currentBalance
                    WHERE PartiesID = @partiesID;
                ";

                        using (SqlCommand cmd = new SqlCommand(updateResidual, con))
                        {
                            cmd.Parameters.AddWithValue("@deductValue", amountToDeduct);
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    previousBalance = Convert.ToDouble(reader["previousBalance"]);
                                    currentBalance = Convert.ToDouble(reader["currentBalance"]);
                                }
                            }
                        }

                        // 🟢 4. تحديث الرصيد في tblMain1
                        using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE tblMain1
                    SET currentDebitBalance = @currentBalance
                    WHERE MainID = @mainID;", con))
                        {
                            cmd.Parameters.AddWithValue("@currentBalance", currentBalance);
                            cmd.Parameters.AddWithValue("@mainID", mainID);
                            cmd.ExecuteNonQuery();
                        }

                        // 🟢 5. معالجة حالة الرصيد السالب (دائن)
                        if (currentBalance < 0)
                        {
                            string message = $"هل تريد سحب رصيد الدائن الحالي؟\n\nالرصيد السابق: {previousBalance:N1}\nالرصيد الحالي: {currentBalance:N1}";
                            string newStatus = "دائن";
                            bool withdraw = false;

                            DialogResult result = DialogResult.None;
                            MainClass.SafeInvoke(() =>
                            {
                                result = MessageBox.Show(message, "تأكيد السحب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            });

                            if (result == DialogResult.Yes)
                            {
                                newStatus = "مسدد";
                                withdraw = true;
                                currentBalance = 0; // تصفير الرصيد في حالة السحب
                            }

                            using (SqlTransaction tran = con.BeginTransaction())
                            {
                                try
                                {
                                    // تحديث الحالة في residualTable
                                    string queryUpdate = @"
                                UPDATE residualTable
                                SET 
                                    previousDebitBalance = ISNULL(currentDebitBalance, 0),
                                    currentDebitBalance = @currentBalance,
                                    status = @status
                                WHERE PartiesID = @partiesID;";

                                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con, tran))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@status", newStatus);
                                        cmdUpdate.Parameters.AddWithValue("@currentBalance", currentBalance);
                                        cmdUpdate.Parameters.AddWithValue("@partiesID", partiesID);
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    // تسجيل العملية
                                    string queryTrans = @"
                                INSERT INTO PartiesTransactions
                                    (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
                                VALUES
                                    (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID,
                                     CAST(GETDATE() AS DATE), @aTime);";

                                    using (SqlCommand cmdTransaction = new SqlCommand(queryTrans, con, tran))
                                    {
                                        cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                                        cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                                        cmdTransaction.Parameters.AddWithValue("@mainID", mainID);
                                        cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                                        if (withdraw)
                                        {
                                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "سحب");
                                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo", $"تم سحب كل رصيد الدائن بقيمة {Math.Abs(currentBalance):N0}");
                                        }
                                        else
                                        {
                                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "إيداع");
                                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo", $"تم إيداع رصيد بقيمة {Math.Abs(currentBalance):N0}");
                                        }

                                        cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", previousBalance);
                                        cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", currentBalance);
                                        cmdTransaction.ExecuteNonQuery();
                                    }

                                    tran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tran.Rollback();
                                    MainClass.SafeInvoke(() =>
                                    {
                                        MessageBox.Show("حدث خطأ أثناء تسجيل الحركة: " + ex.Message);
                                    });
                                }
                            }
                        }

                        // 🟢 6. تسجيل عملية المرتجع العام
                        using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO PartiesTransactions
                        (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
                    VALUES
                        (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID,
                         CAST(GETDATE() AS DATE), @aTime);", con))
                        {
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);
                            cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmd.Parameters.AddWithValue("@transactionsType", "مرتجعات");
                            if (Retruns == true)
                                cmd.Parameters.AddWithValue("@transactionsInfo", $"تم إرجاع الفاتورة من الفواتير المحذوفة بقيمة {amountToDeduct:N0}");
                            else
                                cmd.Parameters.AddWithValue("@transactionsInfo", $"تم إرجاع الفاتورة بالكامل بقيمة {amountToDeduct:N0}"); cmd.Parameters.AddWithValue("@previousDebitBalance", previousBalance);
                            cmd.Parameters.AddWithValue("@currentDebitBalance", currentBalance);
                            cmd.Parameters.AddWithValue("@mainID", mainID);
                            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                });

                MainClass.SafeInvoke(this, () =>
                {
                    MessageBox.Show("✅ تم إرجاع الفاتورة بالكامل وتحديث الرصيد بنجاح.",
                                    "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء إرجاع الفاتورة بالكامل:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async Task residualFullReturnSuppliersAsync(int mainID, bool Retruns)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();

                        double invoiceIssuanceValue = 0;   // إجمالي الفاتورة وقت الإصدار
                        double currentTotal = 0;           // الإجمالي الحالي بعد المرتجعات الجزئية
                        double paidAmount = 0;             // المبلغ المدفوع
                        double previousBalance = 0;
                        double currentBalance = 0;

                        // 🟢 1. قراءة بيانات الفاتورة
                        using (SqlCommand cmd = new SqlCommand(@"
                        SELECT 
                            ISNULL(InvoiceIssuanceValue, 0) AS InvoiceIssuanceValue,
                            ISNULL(clear, 0) AS currentTotal,
                            ISNULL(PaidAmount, 0) AS PaidAmount
                        FROM billPrcheses
                        WHERE bID = @mainID;", con))
                        {
                            cmd.Parameters.AddWithValue("@mainID", mainID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    invoiceIssuanceValue = Convert.ToDouble(reader["InvoiceIssuanceValue"]);
                                    currentTotal = Convert.ToDouble(reader["currentTotal"]);
                                    paidAmount = Convert.ToDouble(reader["PaidAmount"]);
                                }
                            }
                        }

                        // 🧮 2. حساب الخصم الصحيح (المتبقي بعد المدفوعات والمرتجعات)
                        double alreadyDeducted = invoiceIssuanceValue - currentTotal;
                        double remainingDebt = (invoiceIssuanceValue - paidAmount) - alreadyDeducted;
                        double amountToDeduct = remainingDebt + paidAmount;


                        // 🟢 3. تحديث residualTable وخصم المبلغ
                        string updateResidual = @"
                    UPDATE residualTable
                    SET 
                        previousDebitBalance = ISNULL(currentDebitBalance, 0),
                        currentDebitBalance = ISNULL(currentDebitBalance, 0) - @deductValue
                    OUTPUT 
                        DELETED.currentDebitBalance AS previousBalance,
                        INSERTED.currentDebitBalance AS currentBalance
                    WHERE PartiesID = @partiesID;
                ";

                        using (SqlCommand cmd = new SqlCommand(updateResidual, con))
                        {
                            cmd.Parameters.AddWithValue("@deductValue", amountToDeduct);
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    previousBalance = Convert.ToDouble(reader["previousBalance"]);
                                    currentBalance = Convert.ToDouble(reader["currentBalance"]);
                                }
                            }
                        }

                        // 🟢 4. تحديث الرصيد في tblMain1
                        using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE billPrcheses
                    SET currentDebitBalance = @currentBalance
                    WHERE bID = @mainID;", con))
                        {
                            cmd.Parameters.AddWithValue("@currentBalance", currentBalance);
                            cmd.Parameters.AddWithValue("@mainID", mainID);
                            cmd.ExecuteNonQuery();
                        }

                        // 🟢 5. معالجة حالة الرصيد السالب (دائن)
                        if (currentBalance < 0)
                        {
                            string message = $"هل تريد سحب رصيد الدائن الحالي؟\n\nالرصيد السابق: {previousBalance:N1}\nالرصيد الحالي: {currentBalance:N1}";
                            string newStatus = "دائن";
                            bool withdraw = false;

                            DialogResult result = DialogResult.None;
                            MainClass.SafeInvoke(() =>
                            {
                                result = MessageBox.Show(message, "تأكيد السحب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            });

                            if (result == DialogResult.Yes)
                            {
                                newStatus = "مسدد";
                                withdraw = true;
                                currentBalance = 0; // تصفير الرصيد في حالة السحب
                            }

                            using (SqlTransaction tran = con.BeginTransaction())
                            {
                                try
                                {
                                    // تحديث الحالة في residualTable
                                    string queryUpdate = @"
                                UPDATE residualTable
                                SET 
                                    previousDebitBalance = ISNULL(currentDebitBalance, 0),
                                    currentDebitBalance = @currentBalance,
                                    status = @status
                                WHERE PartiesID = @partiesID;";

                                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con, tran))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@status", newStatus);
                                        cmdUpdate.Parameters.AddWithValue("@currentBalance", currentBalance);
                                        cmdUpdate.Parameters.AddWithValue("@partiesID", partiesID);
                                        cmdUpdate.ExecuteNonQuery();
                                    }

                                    // تسجيل العملية
                                    string queryTrans = @"
                                INSERT INTO PartiesTransactions
                                    (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
                                VALUES
                                    (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID,
                                     CAST(GETDATE() AS DATE), @aTime);";

                                    using (SqlCommand cmdTransaction = new SqlCommand(queryTrans, con, tran))
                                    {
                                        cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                                        cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                                        cmdTransaction.Parameters.AddWithValue("@mainID", mainID);
                                        cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                                        if (withdraw)
                                        {
                                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "سحب");
                                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo", $"تم سحب كل رصيد الدائن بقيمة {Math.Abs(currentBalance):N0}");
                                        }
                                        else
                                        {
                                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "إيداع");
                                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo", $"تم إيداع رصيد بقيمة {Math.Abs(currentBalance):N0}");
                                        }

                                        cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", previousBalance);
                                        cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", currentBalance);
                                        cmdTransaction.ExecuteNonQuery();
                                    }

                                    tran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tran.Rollback();
                                    MainClass.SafeInvoke(() =>
                                    {
                                        MessageBox.Show("حدث خطأ أثناء تسجيل الحركة: " + ex.Message);
                                    });
                                }
                            }
                        }

                        // 🟢 6. تسجيل عملية المرتجع العام
                        using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO PartiesTransactions
                        (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
                    VALUES
                        (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID,
                         CAST(GETDATE() AS DATE), @aTime);", con))
                        {
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);
                            cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmd.Parameters.AddWithValue("@transactionsType", "مرتجعات");
                            if (Retruns == true)
                                cmd.Parameters.AddWithValue("@transactionsInfo", $"تم إرجاع الفاتورة من الفواتير المحذوفة بقيمة {amountToDeduct:N0}");
                            else
                                cmd.Parameters.AddWithValue("@transactionsInfo", $"تم إرجاع الفاتورة بالكامل بقيمة {amountToDeduct:N0}");

                            cmd.Parameters.AddWithValue("@previousDebitBalance", previousBalance);
                            cmd.Parameters.AddWithValue("@currentDebitBalance", currentBalance);
                            cmd.Parameters.AddWithValue("@mainID", mainID);
                            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                });

                MainClass.SafeInvoke(this, () =>
                {
                    MessageBox.Show("✅ تم إرجاع الفاتورة بالكامل وتحديث الرصيد بنجاح.",
                                    "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء إرجاع الفاتورة بالكامل:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }



        private async Task transactionStore()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                // تخطي الصفوف الجديدة أو الفارغة
                if (row.IsNewRow) continue;

                // التحقق من عمود DeleteFlag
                if (row.Cells["DeleteFlag"].Value != null &&
                    (row.Cells["DeleteFlag"].Value.ToString() == "1" ||
                     row.Cells["DeleteFlag"].Value.ToString().ToLower() == "true"))
                {
                    continue; // تخطي الصف المحذوف
                }

                int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);
                int uid = Convert.ToInt32(row.Cells["dgvUnitID"].Value);
                bool isuse = Convert.ToBoolean(row.Cells["dgvIsUse"].Value);
                int detailID = Convert.ToInt32(row.Cells["dgvDetainlID"].Value);
                int pID = row.Cells["dgvPID"].Value is DBNull or null
                    ? 0
                    : Convert.ToInt32(row.Cells["dgvPID"].Value);

                if (partyType == "عميل")
                {
                    await qtyStoreAddAsync(pID, uid, isuse, qty);
                }
                else if (partyType == "مورد")
                {
                    await qtyStoreSubAsync(pID, uid, isuse, qty);
                }
            }
        }

        private async Task transactionStoreRerurn()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                // تخطي الصفوف الجديدة أو الفارغة
                if (row.IsNewRow) continue;

                // التحقق من عمود DeleteFlag
                if (row.Cells["DeleteFlag"].Value != null &&
                    (row.Cells["DeleteFlag"].Value.ToString() == "1" ||
                     row.Cells["DeleteFlag"].Value.ToString().ToLower() == "true"))
                {
                    continue; // تخطي الصف المحذوف
                }

                int qty = Convert.ToInt32(row.Cells["dgvQty"].Value);
                int uid = Convert.ToInt32(row.Cells["dgvUnitID"].Value);
                bool isuse = Convert.ToBoolean(row.Cells["dgvIsUse"].Value);
                int detailID = Convert.ToInt32(row.Cells["dgvDetainlID"].Value);
                int pID = row.Cells["dgvPID"].Value is DBNull or null
                    ? 0
                    : Convert.ToInt32(row.Cells["dgvPID"].Value);

                if (partyType == "عميل")
                {
                    await qtyStoreSubAsync(pID, uid, isuse, qty);

                }
                else if (partyType == "مورد")
                {
                    await qtyStoreAddAsync(pID, uid, isuse, qty);

                }
            }
        }

        private async Task billFlagDeleteCustomerAsync(bool DeleteFlag)
        {
            string qry3 = @"
        UPDATE tblMain1
        SET 
            DeleteFlag = @DeleteFlag,
            shiftDoUpdate = @shiftDoUpdate,
            updateDate = @updateDate,
            updateTime = @updateTime
        WHERE MainID = @ID";

            try
            {
                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(qry3, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", mainID);
                        cmd.Parameters.AddWithValue("@DeleteFlag", DeleteFlag);
                        cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftid);
                        cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                });

                // ✅ تنظيف البيانات بعد نجاح العملية على UI Thread
                MainClass.SafeInvoke(this, () => frmCleanData());
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء حذف الفاتورة:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async Task billFlagDeleteSuplieserAsync(bool DeleteFlag)
        {
            string qry3 = @"
        UPDATE billPrcheses
        SET 
            DeleteFlag = @DeleteFlag,
            shiftDoUpdate = @shiftDoUpdate,
            updateDate = @updateDate,
            updateTime = @updateTime
        WHERE bID = @ID";

            try
            {
                // تنفيذ عملية الحذف في الخلفية لتفادي تجمد الواجهة
                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(qry3, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", mainID);
                        cmd.Parameters.AddWithValue("@DeleteFlag", DeleteFlag);
                        cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftid);
                        cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                });

                // تنظيف البيانات بعد الحذف على الـ UI Thread
                MainClass.SafeInvoke(this, () => frmCleanData());
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء حذف فاتورة المورد:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnReturns_Click(object sender, EventArgs e)
        {
            if (isDeleted)
            {
                await deletAllAsync();
                frmCleanData();

                return;
            }
            if (partyType == "عميل")
            {
                //frmpos.showPOS(mainID);

            }
            else if (partyType == "مورد")
            {
                dgvProducts.Columns["dgvDelet"].Visible = true;

            }
        }
        private async Task deletAllAsync()
        {
            DialogResult result = MessageBox.Show(
                "هل أنت متأكد أنك تريد حذف جميع الفواتير المعروضة؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();

                        using (SqlTransaction tran = con.BeginTransaction())
                        {
                            try
                            {
                                foreach (DataGridViewRow row in dgvBills.Rows)
                                {
                                    if (row.Cells["dgvBill_id"].Value == null)
                                        continue;

                                    int billID = Convert.ToInt32(row.Cells["dgvBill_id"].Value);
                                    string qry;

                                    if (partyType == "عميل")
                                    {
                                        qry = @"
                                    DELETE FROM tblDetails WHERE MainID = @ID;
                                    DELETE FROM tblMain1 WHERE MainID = @ID;";
                                    }
                                    else
                                    {
                                        qry = @"
                                    DELETE FROM tblDetailsSupliser WHERE billPrchesesID = @ID;
                                    DELETE FROM billPrcheses WHERE bID = @ID;";
                                    }

                                    using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                                    {
                                        cmd.Parameters.AddWithValue("@ID", billID);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                tran.Commit();
                            }
                            catch
                            {
                                tran.Rollback();
                                throw;
                            }
                        }
                    }
                });

                // ✅ تنظيف البيانات وإشعار المستخدم بعد نجاح الحذف (على UI Thread)
                MainClass.SafeInvoke(this, () =>
                {
                    frmCleanData();
                    Notifier.ShowNotification("حذف", "تم حذف جميع الفواتير بنجاح ✅");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ حدث خطأ أثناء حذف الفواتير:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        public void frmCleanData()
        {
            dgvProducts.Rows.Clear();
            dgvBills.Rows.Clear();

            btnDelete.Enabled = false;
            btnTax.Enabled = false;
            btnShowAndHide.Enabled = false;

            txtName.Text = String.Empty;
            txtBillNumber.Text = string.Empty;
            txtLastUpdateDate.Text = string.Empty;
            txtLastUpdateTime.Text = string.Empty;
            txtLastUpdateName.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPartiesName.Text = string.Empty;
            txtStoreName.Text = string.Empty;
            txtPriceTotal.Text = string.Empty;
            txtDV.Text = string.Empty;
            txtChange.Text = string.Empty;
            txtClean.Text = string.Empty;
            txtDate.Text = string.Empty;
            txtTime.Text = string.Empty;
            txtPay2.Text = string.Empty;
            txtPayWay.Text = string.Empty;
            txtPreviousBebitBalance.Text = string.Empty;
            txtCurrentDebitBalance.Text = string.Empty;
            txtNote.Text = string.Empty;

            dtPickerStart.Value = DateTime.Today;
            dtPickerEnd.Value = DateTime.Today;

            dtPickerStart.Format = DateTimePickerFormat.Custom;
            dtPickerStart.CustomFormat = "yyyy-MM-dd";

            dtPickerEnd.Format = DateTimePickerFormat.Custom;
            dtPickerEnd.CustomFormat = "yyyy-MM-dd";

            partiesID = 0;
            mainID = 0;
        }
        double totalAfterDes;
        double totalBeforDes;
        double desValue;
        private async void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            double paid = double.TryParse(txtPay2.Text, out var parsed) ? parsed : 0;
            double total = double.TryParse(txtClean.Text, out var parsed2) ? parsed2 : 0;
            double oldBalance = double.TryParse(txtPreviousBebitBalance.Text, out var parsed3) ? parsed3 : 0;

            if (e.ColumnIndex == dgvProducts.Columns["dgvDelet"].Index && e.RowIndex >= 0)
            {
                totalAfterDes = double.Parse(
                    dgvProducts.Rows[e.RowIndex].Cells["dgvTotalDiscount"].Value.ToString().Replace('٫', '.'),
                    CultureInfo.InvariantCulture
                );

                totalBeforDes = double.Parse(
                    dgvProducts.Rows[e.RowIndex].Cells["dgvTotal"].Value.ToString().Replace('٫', '.'),
                    CultureInfo.InvariantCulture
                );

                desValue = double.Parse(
                    dgvProducts.Rows[e.RowIndex].Cells["dgvDiscountV"].Value.ToString().Replace('٫', '.'),
                    CultureInfo.InvariantCulture
                );

                var cellValue = dgvProducts.Rows[e.RowIndex].Cells["DeleteFlag"].Value;

                bool isDeleted = false;
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    string val = cellValue.ToString().Trim();
                    isDeleted = (val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase));
                }

                if (isDeleted)
                    return;

                // 1️⃣ جلب DetailID من العمود
                int detailID = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["dgvDetainlID"].Value);

                object cellValue2 = dgvProducts.Rows[e.RowIndex].Cells["dgvpID"].Value;

                int pID = 0; // قيمة افتراضية
                if (cellValue2 != null && cellValue2 != DBNull.Value)
                {
                    pID = Convert.ToInt32(cellValue2);
                }
                int qty = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["dgvQty"].Value);
                int uid = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["dgvUnitID"].Value);
                bool isuse = Convert.ToBoolean(dgvProducts.Rows[e.RowIndex].Cells["dgvIsUse"].Value);

                if (partyType == "عميل")
                {
                    await qtyStoreAddAsync(pID, uid, isuse, qty);

                    await deteteBillCustomerAsync(detailID);
                }
                else if (partyType == "مورد")
                {
                    await qtyStoreSubAsync(pID, uid, isuse, qty);

                    await deteteBillSuplieserAsync(detailID);

                }
                await residualAsync(totalAfterDes, paid, total, oldBalance);

                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = Color.Red;
                row.DefaultCellStyle.ForeColor = Color.White;

                // مسح الصورة من عمود dgvDelet
                if (dgvProducts.Columns.Contains("dgvDelet"))
                {
                    row.Cells["dgvDelet"].Value = DBNull.Value;
                    row.Cells["dgvDelet"].Style.NullValue = null; // منع عرض أي صورة افتراضية
                }

                int actualRows = dgvProducts.AllowUserToAddRows
                 ? dgvProducts.Rows.Count - 1
                 : dgvProducts.Rows.Count;

                row.Cells["DeleteFlag"].Value = true; // يخليها True لو 1 غير كده False


                if (actualRows <= 1)
                    dgvProducts.Columns["dgvDelet"].Visible = false;
                else
                    dgvProducts.Columns["dgvDelet"].Visible = true;

                // ✅ دلوقتي العمود كله Boolean والسورت هيشتغل
                dgvProducts.Sort(dgvProducts.Columns["DeleteFlag"], ListSortDirection.Ascending);


                // ✅ التحقق إذا كل الصفوف DeleteFlag = true
                bool allDeleted = true;

                for (int i = 0; i < dgvProducts.Rows.Count; i++)
                {
                    DataGridViewRow row2 = dgvProducts.Rows[i];
                    if (row2.IsNewRow)
                        continue;

                    var val = row2.Cells["DeleteFlag"].Value;
                    bool isDeleted2 = false;

                    if (val != null && val != DBNull.Value)
                    {
                        string strVal = val.ToString().Trim();
                        if (strVal == "1" || strVal.Equals("true", StringComparison.OrdinalIgnoreCase))
                            isDeleted2 = true;
                    }

                    // لو لقيت صف مش متشال، يبقى مش كله True
                    if (!isDeleted2)
                    {
                        allDeleted = false;
                        break;
                    }
                }

                if (allDeleted)
                {
                    if (partyType == "عميل")
                        await billFlagDeleteCustomerAsync(true);
                    else if (partyType == "مورد")
                        await billFlagDeleteSuplieserAsync(true);
                }
                else if (partyType == "مورد")
                {
                    //dgvProducts.Columns["dgvDelet"].Visible = false;

                }
                if (partyType == "عميل")
                    await GetBillDataFromDB(invoiceCode, false);

                else if (partyType == "مورد")
                    await GetBillDataFromDB(invoiceCode, true);
            }
        }
        private async Task deteteBillCustomerAsync(int detailID)
        {
            string qry1 = @"
        UPDATE tblDetails 
        SET 
            DeleteFlag = @DeleteFlag,
            shiftDoUpdate = @shiftDoUpdate,
            updateDate = @updateDate,
            updateTime = @updateTime
        WHERE DetailID = @ID";

            string qry3 = @"
        UPDATE tblMain1
        SET 
            shiftDoUpdate = @shiftDoUpdate,
            updateDate = @updateDate,
            updateTime = @updateTime
        WHERE MainID = @ID";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // ✅ تحديث تفاصيل الفاتورة
                using (SqlCommand cmd = new SqlCommand(qry1, con))
                {
                    cmd.Parameters.AddWithValue("@DeleteFlag", true);
                    cmd.Parameters.AddWithValue("@ID", detailID);
                    cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());

                    cmd.ExecuteNonQuery();
                }

                // ✅ تحديث بيانات الفاتورة الرئيسية
                using (SqlCommand cmd = new SqlCommand(qry3, con))
                {
                    cmd.Parameters.AddWithValue("@ID", mainID);
                    cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());

                    cmd.ExecuteNonQuery();
                }
            }

            // ✅ تحديث واجهة العميل بعد التعديل
            await BillUpdateCustomerAsync();
        }

        private async Task deteteBillSuplieserAsync(int detailID)
        {
            string qry1 = @"
        UPDATE tblDetailsSupliser 
        SET 
            DeleteFlag = @DeleteFlag, 
            shiftDoUpdate = @shiftDoUpdate,
            updateDate = @updateDate,
            updateTime = @updateTime
        WHERE DetailID = @ID";

            string qry3 = @"
        UPDATE billPrcheses
        SET 
            shiftDoUpdate = @shiftDoUpdate,
            updateDate = @updateDate,
            updateTime = @updateTime
        WHERE bID = @ID";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // ✅ تحديث تفاصيل المورد
                using (SqlCommand cmd = new SqlCommand(qry1, con))
                {
                    cmd.Parameters.AddWithValue("@DeleteFlag", true);
                    cmd.Parameters.AddWithValue("@ID", detailID);
                    cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());

                    cmd.ExecuteNonQuery();
                }

                // ✅ تحديث بيانات الفاتورة الرئيسية
                using (SqlCommand cmd = new SqlCommand(qry3, con))
                {
                    cmd.Parameters.AddWithValue("@ID", mainID);
                    cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());

                    cmd.ExecuteNonQuery();
                }
            }

            // ✅ تحديث واجهة المورد بعد التعديل
            await BillUpdateSuplieserAsync();
        }

        private double qtyU1, qtyU2, qtyU3;
        private async Task qtyStoreAddAsync(int pid, int uid, bool isuse, double qty)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // ✅ احسب الكميات (يُنفذ مباشرة لأنه مجرد عملية حساب داخل الذاكرة)
                await SetProductUnitInfoAddAsync(pid, isuse, uid, qty);

                await Task.Run(() =>
                {
                    string qry;

                    if (isuse)
                    {
                        qry = @"UPDATE totalStor 
                        SET qtyUsedU1 = @qtyU1,
                            qtyUsedU2 = @qtyU2,
                            qtyUsedU3 = @qtyU3
                        WHERE pID = @pID";
                    }
                    else
                    {
                        qry = @"UPDATE totalStor 
                        SET qtyU1 = @qtyU1,
                            qtyU2 = @qtyU2,
                            qtyU3 = @qtyU3
                        WHERE pID = @pID";
                    }

                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddWithValue("@pID", pid);
                            cmd.Parameters.AddWithValue("@qtyU1", qtyU1);
                            cmd.Parameters.AddWithValue("@qtyU2", qtyU2);
                            cmd.Parameters.AddWithValue("@qtyU3", qtyU3);
                            cmd.ExecuteNonQuery();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء تحديث الكميات في المخزون:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private async Task SetProductUnitInfoAddAsync(int pID, bool isUsed, int currentUinte, double extraQtyU = 0)
        {
            try
            {
                // 🧮 تحميل البيانات في الخلفية
                DataTable dt = await Task.Run(() =>
                {
                    string query = @"
                SELECT p.*, c.*, u.uName, ts.*
                FROM products p
                INNER JOIN category c ON c.catID = p.categoryID
                INNER JOIN untits u ON p.idUniteDef = u.uID
                INNER JOIN totalStor ts ON ts.pID = p.pID
                WHERE p.pID = @value";

                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@value", pID);

                        DataTable table = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(table); // تشغيل في Thread الخلفية
                        return table;
                    }
                });

                if (dt.Rows.Count == 0)
                    return;

                // 🔹 كل الحسابات دي خفيفة — نعملها على UI Thread بأمان
                DataRow row = dt.Rows[0];

                int idUnite1 = row["idUnite1"] != DBNull.Value ? Convert.ToInt32(row["idUnite1"]) : 0;
                int idUnite2 = row["idUnite2"] != DBNull.Value ? Convert.ToInt32(row["idUnite2"]) : 0;
                int idUnite3 = row["idUnite3"] != DBNull.Value ? Convert.ToInt32(row["idUnite3"]) : 0;

                int numberU2 = row["numberU2"] != DBNull.Value ? Convert.ToInt32(row["numberU2"]) : 1;
                int numberU3 = row["numberU3"] != DBNull.Value ? Convert.ToInt32(row["numberU3"]) : 1;

                // 1️⃣ تحديد الكمية حسب الوحدة المستخدمة
                if (currentUinte == idUnite3)
                {
                    qtyU3 = Convert.ToDouble(isUsed ? row["qtyUsedU3"] : row["qtyU3"]);
                    qtyU3 += extraQtyU;
                }
                else if (currentUinte == idUnite2)
                {
                    double baseQty = Convert.ToDouble(isUsed ? row["qtyUsedU2"] : row["qtyU2"]);
                    baseQty += extraQtyU;
                    qtyU3 = baseQty * numberU3;
                }
                else
                {
                    double baseQty = Convert.ToDouble(isUsed ? row["qtyUsedU1"] : row["qtyU1"]);
                    baseQty += extraQtyU;
                    qtyU3 = baseQty * numberU2 * numberU3;
                }

                // 2️⃣ التحويل إلى باقي الوحدات
                qtyU2 = qtyU3 / numberU3;
                qtyU1 = qtyU2 / numberU2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء تحميل بيانات الوحدة:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async Task qtyStoreSubAsync(int pid, int uid, bool isuse, double qty)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // ✅ احسب الكميات المطلوبة (داخل الذاكرة، سريع جدًا)
                await SetProductUnitInfoSubAsync(pid, isuse, uid, qty);

                // ✅ نفّذ تحديث قاعدة البيانات في الخلفية
                await Task.Run(() =>
                {
                    string qry;

                    if (isuse)
                    {
                        qry = @"UPDATE totalStor 
                        SET qtyUsedU1 = @qtyU1,
                            qtyUsedU2 = @qtyU2,
                            qtyUsedU3 = @qtyU3
                        WHERE pID = @pID";
                    }
                    else
                    {
                        qry = @"UPDATE totalStor 
                        SET qtyU1 = @qtyU1,
                            qtyU2 = @qtyU2,
                            qtyU3 = @qtyU3
                        WHERE pID = @pID";
                    }

                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddWithValue("@pID", pid);
                            cmd.Parameters.AddWithValue("@qtyU1", qtyU1);
                            cmd.Parameters.AddWithValue("@qtyU2", qtyU2);
                            cmd.Parameters.AddWithValue("@qtyU3", qtyU3);
                            cmd.ExecuteNonQuery();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء خصم الكمية من المخزون:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private async Task SetProductUnitInfoSubAsync(int pID, bool isUsed, int currentUinte, double extraQtyU = 0)
        {
            try
            {
                // 🧠 تحميل بيانات المنتج في الخلفية بدون تجميد الواجهة
                DataTable dt = await Task.Run(() =>
                {
                    string query = @"
                SELECT p.*, c.*, u.uName, ts.*
                FROM products p
                INNER JOIN category c ON c.catID = p.categoryID
                INNER JOIN untits u ON p.idUniteDef = u.uID
                INNER JOIN totalStor ts ON ts.pID = p.pID
                WHERE p.pID = @value";

                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@value", pID);

                        DataTable table = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(table); // يتم تنفيذها في Thread الخلفية
                        return table;
                    }
                });

                if (dt.Rows.Count == 0)
                    return;

                // 🔹 العمليات الحسابية تتم بعد جلب البيانات
                DataRow row = dt.Rows[0];

                int idUnite1 = row["idUnite1"] != DBNull.Value ? Convert.ToInt32(row["idUnite1"]) : 0;
                int idUnite2 = row["idUnite2"] != DBNull.Value ? Convert.ToInt32(row["idUnite2"]) : 0;
                int idUnite3 = row["idUnite3"] != DBNull.Value ? Convert.ToInt32(row["idUnite3"]) : 0;

                int numberU2 = row["numberU2"] != DBNull.Value ? Convert.ToInt32(row["numberU2"]) : 1;
                int numberU3 = row["numberU3"] != DBNull.Value ? Convert.ToInt32(row["numberU3"]) : 1;

                // 1️⃣ حساب الكمية بعد الخصم حسب الوحدة الحالية
                if (currentUinte == idUnite3)
                {
                    qtyU3 = Convert.ToDouble(isUsed ? row["qtyUsedU3"] : row["qtyU3"]);
                    qtyU3 -= extraQtyU;
                }
                else if (currentUinte == idUnite2)
                {
                    double baseQty = Convert.ToDouble(isUsed ? row["qtyUsedU2"] : row["qtyU2"]);
                    baseQty -= extraQtyU;
                    qtyU3 = baseQty * numberU3;
                }
                else
                {
                    double baseQty = Convert.ToDouble(isUsed ? row["qtyUsedU1"] : row["qtyU1"]);
                    baseQty -= extraQtyU;
                    qtyU3 = baseQty * numberU2 * numberU3;
                }

                // 2️⃣ التحويل لباقي الوحدات
                qtyU2 = qtyU3 / numberU3;
                qtyU1 = qtyU2 / numberU2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء تحميل بيانات الوحدة:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async Task BillUpdateCustomerAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string qry1 = @"
            UPDATE tblMain1
            SET 
                total = CAST(ROUND(total - @total, 0) AS INT),
                descountValue = CAST(ROUND(descountValue - @descountValue, 0) AS INT),
                priceClear = CAST(ROUND(priceClear - @priceClear, 0) AS INT),
                TotalWithInterest = CAST(ROUND(TotalWithInterest - @TotalWithInterest, 0) AS INT),

                change = CASE 
                            WHEN PaymentMethod = N'اجل' THEN 
                                CASE 
                                    WHEN CAST(ROUND(change - @change, 0) AS INT) < 0 THEN 0
                                    ELSE CAST(ROUND(change - @change, 0) AS INT)
                                END
                            ELSE change
                         END
            WHERE MainID = @ID;

            SELECT PaymentMethod,
                   CAST(ROUND(total, 0) AS INT) AS total,
                   CAST(ROUND(descountValue, 0) AS INT) AS descountValue,
                   CAST(ROUND(change, 0) AS INT) AS change,
                   CAST(ROUND(TotalWithInterest, 0) AS INT) AS TotalWithInterest,
                   CAST(ROUND(currentDebitBalance, 0) AS INT) AS currentDebitBalance,
                   CAST(ROUND(PaidAmount, 0) AS INT) AS PaidAmount
            FROM tblMain1
            WHERE MainID = @ID;
        ";

                // 🧮 شغل العمليات الثقيلة (SQL) في الخلفية
                var result = await Task.Run(() =>
                {
                    var data = new Dictionary<string, object>();

                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(qry1, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", mainID);
                            cmd.Parameters.AddWithValue("@total", totalBeforDes);
                            cmd.Parameters.AddWithValue("@priceClear", totalAfterDes);
                            cmd.Parameters.AddWithValue("@TotalWithInterest", totalAfterDes);
                            cmd.Parameters.AddWithValue("@descountValue", desValue);
                            cmd.Parameters.AddWithValue("@change", totalAfterDes);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    data["total"] = reader["total"];
                                    data["descountValue"] = reader["descountValue"];
                                    data["change"] = reader["change"];
                                    data["TotalWithInterest"] = reader["TotalWithInterest"];
                                    data["currentDebitBalance"] = reader["currentDebitBalance"];
                                    data["PaidAmount"] = reader["PaidAmount"];
                                    data["PaymentMethod"] = reader["PaymentMethod"];
                                }
                            }
                        }
                    }

                    return data;
                });

                // 🔹 بعد رجوع البيانات: حدّث الواجهة فقط (UI Thread)
                if (result.Count > 0)
                {
                    txtPriceTotal.Text = Convert.ToDecimal(result["total"]).ToString("N0");
                    txtDV.Text = Convert.ToDecimal(result["descountValue"]).ToString("N0");
                    txtChange.Text = Convert.ToDecimal(result["change"]).ToString("N0");
                    txtClean.Text = Convert.ToDecimal(result["TotalWithInterest"]).ToString("N0");
                    txtCurrentDebitBalance.Text = Convert.ToDecimal(result["currentDebitBalance"]).ToString("N0");
                    txtPay2.Text = Convert.ToDecimal(result["PaidAmount"]).ToString("N0");

                    lblNotefi.Text = "تم تحديث الفاتورة وعرض البيانات الجديدة";
                    lblNotefi.Visible = true;
                    lblNotefi.ForeColor = Color.Green;
                }
                else
                {
                    lblNotefi.Text = "⚠️ لم يتم العثور على بيانات محدثة";
                    lblNotefi.Visible = true;
                    lblNotefi.ForeColor = Color.OrangeRed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث الفاتورة:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private async Task BillUpdateSuplieserAsync()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string qry1 = @"
            UPDATE billPrcheses
            SET 
                total = CAST(ROUND(total - @total, 0) AS INT),
                clear = CAST(ROUND(clear - @priceClear, 0) AS INT),

                change = CASE 
                            WHEN payWay = N'اجل' THEN 
                                CASE 
                                    WHEN CAST(ROUND(change - @change, 0) AS INT) < 0 THEN 0
                                    ELSE CAST(ROUND(change - @change, 0) AS INT)
                                END
                            ELSE change
                         END

            WHERE bID = @ID;

            SELECT payWay,
                   CAST(ROUND(total, 0) AS INT) AS total, 
                   CAST(ROUND(clear, 0) AS INT) AS clear, 
                   CAST(ROUND(change, 0) AS INT) AS change,
                   CAST(ROUND(currentDebitBalance, 0) AS INT) AS currentDebitBalance,
                   CAST(ROUND(PaidAmount, 0) AS INT) AS PaidAmount
            FROM billPrcheses
            WHERE bID = @ID;
        ";

                // 🧮 شغل العملية الثقيلة (SQL) في الخلفية
                var result = await Task.Run(() =>
                {
                    var data = new Dictionary<string, object>();
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(qry1, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", mainID);
                            cmd.Parameters.AddWithValue("@total", totalBeforDes);
                            cmd.Parameters.AddWithValue("@priceClear", totalAfterDes);
                            cmd.Parameters.AddWithValue("@change", totalAfterDes);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    data["total"] = reader["total"];
                                    data["clear"] = reader["clear"];
                                    data["change"] = reader["change"];
                                    data["currentDebitBalance"] = reader["currentDebitBalance"];
                                    data["PaidAmount"] = reader["PaidAmount"];
                                    data["payWay"] = reader["payWay"];
                                }
                            }
                        }
                    }
                    return data;
                });

                // 🔹 بعد انتهاء الخلفية: تحديث الواجهة فقط
                if (result.Count > 0)
                {
                    txtPriceTotal.Text = Convert.ToDecimal(result["total"]).ToString("N0");
                    txtChange.Text = Convert.ToDecimal(result["change"]).ToString("N0");
                    txtClean.Text = Convert.ToDecimal(result["clear"]).ToString("N0");
                    txtCurrentDebitBalance.Text = Convert.ToDecimal(result["currentDebitBalance"]).ToString("N0");
                    txtPay2.Text = Convert.ToDecimal(result["PaidAmount"]).ToString("N0");

                    lblNotefi.Text = "تم تحديث الفاتورة وعرض البيانات الجديدة";
                    lblNotefi.Visible = true;
                    lblNotefi.ForeColor = Color.Green;
                }
                else
                {
                    lblNotefi.Text = "⚠️ لم يتم العثور على بيانات محدثة";
                    lblNotefi.Visible = true;
                    lblNotefi.ForeColor = Color.OrangeRed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحديث الفاتورة:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void btnShowAndHide_Click(object sender, EventArgs e)
        {
            if (showDeletedProducts)
            {
                showDeletedProducts = false;
                btnShowAndHide.Text = "إظهار المنتجات المحذوفة";

                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    if (row.Cells["DeleteFlag"].Value != DBNull.Value)
                    {
                        bool isDeleted = ConvertToBool(row.Cells["DeleteFlag"].Value);
                        if (isDeleted)
                            row.Visible = false;
                    }
                }
            }
            else
            {
                showDeletedProducts = true;
                btnShowAndHide.Text = "إخفاء المنتجات المحذوفة";

                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    if (row.Cells["DeleteFlag"].Value != DBNull.Value)
                    {
                        bool isDeleted = ConvertToBool(row.Cells["DeleteFlag"].Value);
                        if (isDeleted)
                        {
                            row.Visible = true;
                            if (dgvProducts.Columns.Contains("dgvDelet"))
                            {
                                row.Selected = false;
                                row.Cells["dgvDelet"].Value = DBNull.Value;
                                row.Cells["dgvDelet"].Style.NullValue = null; // منع عرض أي صورة افتراضية
                                row.DefaultCellStyle.BackColor = Color.Red;
                                row.DefaultCellStyle.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }

            dgvProducts.Sort(dgvProducts.Columns["DeleteFlag"], ListSortDirection.Ascending);
        }

        /// <summary>
        /// يحول أي قيمة (0/1, true/false, int, string) إلى Boolean
        /// </summary>
        private bool ConvertToBool(object value)
        {
            if (value == null || value == DBNull.Value)
                return false;

            string val = value.ToString().Trim();

            if (val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;

            if (val == "0" || val.Equals("false", StringComparison.OrdinalIgnoreCase))
                return false;

            // fallback
            return false;
        }



        private void smoothPanel_BottomCorner1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDescount_Click(object sender, EventArgs e)
        {
            if (isFinancial)
            {
                panel3.Controls.Add(gbFinance);
                gbFinance.Location = gbSearch.Location;
                btnTax.Text = "الرجوع الي طرق البحث والعرض";
                gbSearch.Visible = false;
                gbFinance.Visible = true;
                isFinancial = false;

            }
            else
            {
                isFinancial = true;
                btnTax.Text = "دارة الضرائب والرسوم";
                gbSearch.Visible = true;
                gbFinance.Visible = false;
            }

        }

        private async void btnAddPercenTax_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAddPercenTax.Text, out decimal percent))
            {
                Notifier.ShowNotification("النسبة غير صحيحة", "❌ من فضلك أدخل نسبة صحيحة");
                return;
            }
            string Current = txtCurrentDebitBalance.Text;

            decimal currentDebit = string.IsNullOrWhiteSpace(txtCurrentDebitBalance.Text)
                    ? 0
                    : decimal.Parse(txtChange.Text,
        System.Globalization.NumberStyles.AllowThousands);

            decimal taxValue = Math.Round(currentDebit * percent / 100, 2);

            string qry = @"
            UPDATE tblMain1
            SET 
                shiftDoUpdate = @shiftDoUpdate,
                updateDate = @updateDate,
                updateTime = @updateTime,
                currentDebitBalance = ISNULL(currentDebitBalance, 0) + @currentDebitBalance,
                latePayTax = ISNULL(latePayTax, 0) + @latePayTax
            WHERE MainID = @ID;";


            string qtyTransaction = @"
            INSERT INTO PartiesTransactions
                (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance,mainID, aDate, aTime)
            VALUES
                (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID,
                 CAST(GETDATE() AS DATE), @aTime);";



            try
            {
                await resdualAddTaxAsync(taxValue);

                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {

                            // 2️⃣ تحديث tblMain1
                            using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                            {
                                cmd.Parameters.AddWithValue("@ID", mainID);
                                cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftid);
                                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                                cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());
                                cmd.Parameters.AddWithValue("@currentDebitBalance", taxValue);
                                cmd.Parameters.AddWithValue("@latePayTax", taxValue);

                                cmd.ExecuteNonQuery();
                            }



                            // 4️⃣ إدخال سجل في PartiesTransactions
                            using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                            {
                                cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                                cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                                cmdTransaction.Parameters.AddWithValue("@mainID", mainID);
                                cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                                cmdTransaction.Parameters.AddWithValue("@transactionsType", "اضافة ضريبة");
                                cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                                        $"تم إضافة ضريبة بنسبة {percent}% بقيمة {taxValue.ToString("N0")}");

                                cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", oldBalance);
                                cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", newBalance);

                                cmdTransaction.ExecuteNonQuery();
                            }

                            tran.Commit();
                            Notifier.ShowNotification("تم", "✅ تم اضافة الضريبة بنجاح");

                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("خطأ في العملية: " + ex.Message);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الاتصال: " + ex.Message);
            }

            if (partyType == "عميل")
                await GetBillDataFromDB(invoiceCode, false);
            else if (partyType == "مورد")
                await GetBillDataFromDB(invoiceCode, true);

            txtAddPercenTax.Text = String.Empty;
        }
        private double oldBalance = 0;
        private double newBalance = 0;
        private async Task resdualAddTaxAsync(decimal taxValue)
        {
            try
            {
                await Task.Run(() =>
                {
                    string queryCheck = "SELECT COUNT(*) FROM residualTable WHERE PartiesID = @partiesID";

                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();

                        int isCustomerValue = (partyType == "عميل") ? 1 : 0;

                        // 🔍 التأكد من وجود PartiesID
                        using (SqlCommand checkCmd = new SqlCommand(queryCheck, con))
                        {
                            checkCmd.Parameters.AddWithValue("@partiesID", partiesID);
                            int count = (int)checkCmd.ExecuteScalar();

                            if (count == 0)
                            {
                                string insertQuery = @"
                            INSERT INTO residualTable
                            (PartiesID, status, isCustomer, totalPaid, totalTransaction, previousDebitBalance, currentDebitBalance)
                            VALUES
                            (@partiesID, N'مدين', @isCustomer, 0, 0, 0, 0);";

                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                                {
                                    insertCmd.Parameters.AddWithValue("@partiesID", partiesID);
                                    insertCmd.Parameters.AddWithValue("@isCustomer", isCustomerValue);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // ✏️ تحديث الرصيد وإرجاع القديم والجديد
                        string query = @"
                    UPDATE residualTable
                    SET 
                        previousDebitBalance = ISNULL(currentDebitBalance, 0),
                        currentDebitBalance = ISNULL(currentDebitBalance, 0) + @deductValue
                    OUTPUT 
                        DELETED.currentDebitBalance AS OldBalance,
                        INSERTED.currentDebitBalance AS NewBalance
                    WHERE PartiesID = @partiesID;";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@deductValue", taxValue);
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    oldBalance = reader["OldBalance"] != DBNull.Value ? Convert.ToDouble(reader["OldBalance"]) : 0;
                                    newBalance = reader["NewBalance"] != DBNull.Value ? Convert.ToDouble(reader["NewBalance"]) : 0;
                                }
                            }
                        }

                        con.Close();
                    }
                });

                // ✅ بعد اكتمال العملية بنجاح (تحديث الـ UI)
                MainClass.SafeInvoke(this, () =>
                {
                    Console.WriteLine($"Old Balance: {oldBalance}, New Balance: {newBalance}");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء تحديث الرصيد بالضريبة:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task resdualAddDisAsync(double change)
        {
            try
            {
                await Task.Run(() =>
                {
                    string queryCheck = "SELECT COUNT(*) FROM residualTable WHERE PartiesID = @partiesID";

                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();

                        int isCustomerValue = (partyType == "عميل") ? 1 : 0;

                        // 🔍 التأكد من وجود PartiesID
                        using (SqlCommand checkCmd = new SqlCommand(queryCheck, con))
                        {
                            checkCmd.Parameters.AddWithValue("@partiesID", partiesID);
                            int count = (int)checkCmd.ExecuteScalar();

                            if (count == 0)
                            {
                                // 📝 PartiesID مش موجود → نضيف صف جديد
                                string insertQuery = @"
                            INSERT INTO residualTable
                            (PartiesID, status, isCustomer, totalPaid, totalTransaction, previousDebitBalance, currentDebitBalance)
                            VALUES
                            (@partiesID, N'دائن', @isCustomer, 0, 0, 0, 0);";

                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                                {
                                    insertCmd.Parameters.AddWithValue("@partiesID", partiesID);
                                    insertCmd.Parameters.AddWithValue("@isCustomer", isCustomerValue);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // ✏️ تنفيذ UPDATE وجلب الرصيدين
                        string query = @"
                    UPDATE residualTable
                    SET 
                        previousDebitBalance = ISNULL(currentDebitBalance, 0),
                        currentDebitBalance = ISNULL(currentDebitBalance, 0) - @deductValue
                    OUTPUT 
                        DELETED.currentDebitBalance AS OldBalance,
                        INSERTED.currentDebitBalance AS NewBalance
                    WHERE PartiesID = @partiesID;";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@deductValue", change);
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    oldBalance = reader["OldBalance"] != DBNull.Value ? Convert.ToDouble(reader["OldBalance"]) : 0;
                                    newBalance = reader["NewBalance"] != DBNull.Value ? Convert.ToDouble(reader["NewBalance"]) : 0;
                                }
                            }
                        }

                        con.Close();
                    }
                });

                // 🔍 معالجة حالة الرصيد السالب (UI Thread)
                if (newBalance < 0)
                {
                    DialogResult result2 = MessageBox.Show(
                        $"هل تريد سحب رصيد الدائن الحالي؟\n\nالرصيد السابق: {oldBalance:N1}\nالرصيد الحالي: {newBalance:N1}",
                        "تأكيد السحب",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    string newStatus = (result2 == DialogResult.Yes) ? "مسدد" : "دائن";

                    await Task.Run(() =>
                    {
                        string query2 = @"
                    UPDATE residualTable
                    SET currentDebitBalance = CASE WHEN @status = N'مسدد' THEN 0 ELSE currentDebitBalance END,
                        status = @status
                    WHERE PartiesID = @partiesID;";

                        using (SqlConnection con = MainClass.GetConnection())
                        using (SqlCommand cmd = new SqlCommand(query2, con))
                        {
                            cmd.Parameters.AddWithValue("@status", newStatus);
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء تعديل رصيد الخصم:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtAddPercenTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null) return; // لو مش TextBox يخرج

            // السماح بالأرقام، Backspace، والنقطة العشرية
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // منع إدخال أكثر من نقطة عشرية
            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
            }

            // لو ضغط Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // نفذ الإجراء المطلوب هنا
            }
        }

        private async void btnAddValueTax_Click(object sender, EventArgs e)
        {

            if (!decimal.TryParse(txtAddValueTax.Text, out decimal taxValue))
            {
                Notifier.ShowNotification("القيمة غير صحيحة", "❌ من فضلك أدخل قيمة صحيحة");
                return;
            }

            string qry = @"
            UPDATE tblMain1
            SET 
                shiftDoUpdate = @shiftDoUpdate,
                updateDate = @updateDate,
                updateTime = @updateTime,
                currentDebitBalance = ISNULL(currentDebitBalance, 0) + @currentDebitBalance,
                latePayTax = ISNULL(latePayTax, 0) + @latePayTax
            WHERE MainID = @ID";


            string qtyTransaction = @"
            INSERT INTO PartiesTransactions
                (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance,mainID, aDate, aTime)
            VALUES
                (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID,
                 CAST(GETDATE() AS DATE), @aTime);";

            try
            {
                await resdualAddTaxAsync(taxValue);

                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {

                            // 2️⃣ تحديث tblMain1
                            using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                            {
                                cmd.Parameters.AddWithValue("@ID", mainID);
                                cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftid);
                                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                                cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());
                                cmd.Parameters.AddWithValue("@currentDebitBalance", taxValue);
                                cmd.Parameters.AddWithValue("@latePayTax", taxValue);

                                cmd.ExecuteNonQuery();
                            }



                            // 4️⃣ إدخال سجل في PartiesTransactions
                            using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                            {
                                cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                                cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                                cmdTransaction.Parameters.AddWithValue("@mainID", mainID);
                                cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                                cmdTransaction.Parameters.AddWithValue("@transactionsType", "اضافة ضريبة");
                                cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                                        $"تم إضافة ضريبة بقيمة {taxValue.ToString("N0")}");

                                cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", oldBalance);
                                cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", newBalance);

                                cmdTransaction.ExecuteNonQuery();
                            }

                            tran.Commit();
                            Notifier.ShowNotification("تم", "✅ تم اضافة الضريبة بنجاح");


                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("خطأ في العملية: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الاتصال: " + ex.Message);
            }

            if (partyType == "عميل")
                await GetBillDataFromDB(invoiceCode, false);
            else if (partyType == "مورد")
                await GetBillDataFromDB(invoiceCode, true);

            txtAddValueTax.Text = String.Empty;
        }


        private void txtAddValueTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }

        }

        private async void btnAddBillDescount_Click(object sender, EventArgs e)
        {


            if (!double.TryParse(txtAddBillDescount.Text, out double taxValue))
            {
                Notifier.ShowNotification("القيمة غير صحيحة", "❌ من فضلك أدخل قيمة صحيحة");
                return;
            }


            string qry = @"
                    UPDATE tblMain1
                    SET 
                        shiftDoUpdate = @shiftDoUpdate,
                        updateDate = @updateDate,
                        updateTime = @updateTime,
                        currentDebitBalance = ISNULL(currentDebitBalance, 0) - @currentDebitBalance,
                        latePayTax = ISNULL(latePayTax, 0) - @latePayTax
                    WHERE MainID = @ID";


            string qtyTransaction = @"
                    INSERT INTO PartiesTransactions
                        (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance,mainID, aDate, aTime)
                    VALUES
                        (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID,
                         CAST(GETDATE() AS DATE), @aTime);";

            try
            {
                await resdualAddDisAsync(taxValue);
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {

                            // 2️⃣ تحديث tblMain1
                            using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                            {
                                cmd.Parameters.AddWithValue("@ID", mainID);
                                cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftid);
                                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                                cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());
                                cmd.Parameters.AddWithValue("@currentDebitBalance", taxValue);
                                cmd.Parameters.AddWithValue("@latePayTax", taxValue);

                                cmd.ExecuteNonQuery();
                            }

                            // 4️⃣ إدخال سجل في PartiesTransactions
                            using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                            {
                                cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                                cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                                cmdTransaction.Parameters.AddWithValue("@mainID", mainID);
                                cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                                cmdTransaction.Parameters.AddWithValue("@transactionsType", "خصم علي الفاتورة");
                                cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                                        $"تم  خصم مبلغ {taxValue.ToString("N0")} من الفاتورة");

                                cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", oldBalance);
                                cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", newBalance);

                                cmdTransaction.ExecuteNonQuery();
                            }

                            tran.Commit();
                            Notifier.ShowNotification("تم", "✅ تم اضافة خصم علي الفاتورة بنجاح");
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("خطأ في العملية: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الاتصال: " + ex.Message);
            }

            if (partyType == "عميل")
                await GetBillDataFromDB(invoiceCode, false);
            else if (partyType == "مورد")
                await GetBillDataFromDB(invoiceCode, true);

            txtAddBillDescount.Text = String.Empty;
        }

        private void txtAddBillDescount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }

        }

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Columns.Contains("DeleteFlag"))
            {
                var row = dgvProducts.Rows[e.RowIndex];
                bool deleted = row.Cells["DeleteFlag"].Value?.ToString() == "1";

                if (deleted)
                {
                    // غير لون الصف
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.White;

                    // شيل الصورة من العمود
                    if (dgvProducts.Columns.Contains("dgvDelet"))
                    {
                        row.Cells["dgvDelet"].Value = null;
                    }
                }
            }
        }

        private void dgvProducts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvProducts.Rows[e.RowIndex];
            bool isDeleted = ConvertToBool(row.Cells["DeleteFlag"].Value);

            if (isDeleted)
            {
                // نخلي لون التحديد نفس اللون العادي علشان مايبانش إنه اتحدد
                row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor;
                row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;
            }
            else
            {
                // نرجع الألوان العادية للصفوف السليمة
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
                row.DefaultCellStyle.SelectionForeColor = Color.Wheat;
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvProducts.SelectedRows)
            {
                bool isDeleted = ConvertToBool(row.Cells["DeleteFlag"].Value);
                if (isDeleted)
                {
                    row.Selected = false; // هنا نلغي التحديد فقط للصفوف المحذوفة
                }
            }
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            await MainClass.PrintInvoiceAsync(mainID);

        }

        private async void dgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double paid = double.TryParse(txtPay2.Text, out var parsed) ? parsed : 0;
            double total = double.TryParse(txtClean.Text, out var parsed2) ? parsed2 : 0;
            double oldBalance = double.TryParse(txtPreviousBebitBalance.Text, out var parsed3) ? parsed3 : 0;

            var dgv = sender as DataGridView;
            if (dgv == null) return;

            if (dgv.Columns[e.ColumnIndex].Name == "dgvReturnQty")
            {
                int proID = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["dgvDetainlID"].Value);

                object cellValue2 = dgvProducts.Rows[e.RowIndex].Cells["dgvpID"].Value;

                int pID = 0; // قيمة افتراضية
                if (cellValue2 != null && cellValue2 != DBNull.Value)
                {
                    pID = Convert.ToInt32(cellValue2);
                }
                int qty = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["dgvQty"].Value);
                int uid = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["dgvUnitID"].Value);
                bool isuse = Convert.ToBoolean(dgvProducts.Rows[e.RowIndex].Cells["dgvIsUse"].Value);

                var currentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                object value = currentCell.Value;

                if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                    return;

                double returnQty = Convert.ToDouble(value);

                // 🔹 جلب الكمية الأصلية
                double originalQty = Convert.ToDouble(dgv.Rows[e.RowIndex].Cells["dgvQty"].Value);
                double oldReturn = Convert.ToDouble(dgv.Rows[e.RowIndex].Cells["dgvOrignalQty"].Value);
                double price = Convert.ToDouble(dgv.Rows[e.RowIndex].Cells["dgvPrice"].Value);

                // 🔹 المقارنة
                if (returnQty >= originalQty)
                {
                    MessageBox.Show(
                        "⚠ لا يمكن إرجاع كمية أكبر أو تساوي الكمية الأصلية.\nإذا كنت تريد إزالة المنتج بالكامل من الفاتورة، استخدم خيار الحذف.",
                        "تنبيه",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    currentCell.Value = originalQty;
                    dgv.Rows[e.RowIndex].Cells["dgvQty"].Value = originalQty;
                    dgv.Rows[e.RowIndex].Cells["dgvReturnQty"].Value = 0;
                    return;
                }

                // 🔹 رسالة تأكيد قبل التعديل
                var result = MessageBox.Show(
                    $"هل أنت متأكد أنك تريد إرجاع {returnQty} من هذا المنتج؟",
                    "تأكيد الإرجاع",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    // رجع القيمة زي ما كانت
                    currentCell.Value = 0;
                    dgv.Rows[e.RowIndex].Cells["dgvQty"].Value = originalQty;
                    return;
                }

                // 🔹 تعديل القيم
                dgv.Rows[e.RowIndex].Cells["dgvQty"].Value = originalQty - returnQty;

                double change = price * returnQty;
                change = Math.Round(change, 1);
                await residualAsync(change, paid, total, oldBalance);
                await updateDBilsAsync(invoiceCode, change);
                await updateDetailsBillsAsync(proID, change, originalQty - returnQty, returnQty + oldReturn);



                if (partyType == "عميل")
                {
                    await qtyStoreAddAsync(pID, uid, isuse, returnQty);

                    //deteteBillCustomer(proID);

                }
                else if (partyType == "مورد")
                {
                    await qtyStoreSubAsync(pID, uid, isuse, returnQty);

                    //deteteBillSuplieser(proID);
                }
            }
        }

        private async Task updateDBilsAsync(string InvoiceCode, double change)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = con;

                            if (partyType == "عميل")
                            {
                                cmd.CommandText = @"
                            UPDATE tblMain1
                            SET 
                                TotalWithInterest = TotalWithInterest - @val,
                                change = change - @val,
                                total = total - @val
                            WHERE InvoiceCode = @code";
                            }
                            else
                            {
                                cmd.CommandText = @"
                            UPDATE billPrcheses 
                            SET 
                                total = total - @val, 
                                clear = clear - @val,
                                change = change - @val
                            WHERE InvoiceCode = @code";
                            }

                            cmd.Parameters.Add("@val", SqlDbType.Decimal).Value = change;
                            cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = InvoiceCode;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error updating bill:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private async Task updateDetailsBillsAsync(int proID, double change, double qty, double returnQty)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();

                        string updateQry = partyType == "عميل"
                            ? @"UPDATE tblDetails 
                       SET 
                           qty = @qty, 
                           returnQty = @returnQty,
                           cleanPrice = cleanPrice - @cleanPrice,
                           amount = amount - @amount,
                           priceAfterDes = priceAfterDes - @priceAfterDes,
                           shiftDoUpdate = @shiftDoUpdate,
                           updateDate = @updateDate,
                           updateTime = @updateTime
                       WHERE detailID = @detailID"
                            :
                            @"UPDATE tblDetailsSupliser 
                       SET 
                           qty = @qty, 
                           returnQty = @returnQty,
                           cleanPrice = cleanPrice - @cleanPrice,
                           amount = amount - @amount,
                           priceAfterDes = priceAfterDes - @priceAfterDes,
                           shiftDoUpdate = @shiftDoUpdate,
                           updateDate = @updateDate,
                           updateTime = @updateTime
                       WHERE DetailID = @detailID";

                        using (SqlCommand cmd = new SqlCommand(updateQry, con))
                        {
                            cmd.Parameters.AddWithValue("@detailID", proID);
                            cmd.Parameters.AddWithValue("@qty", qty);
                            cmd.Parameters.AddWithValue("@returnQty", returnQty);
                            cmd.Parameters.AddWithValue("@cleanPrice", change);
                            cmd.Parameters.AddWithValue("@amount", change);
                            cmd.Parameters.AddWithValue("@priceAfterDes", change);
                            cmd.Parameters.AddWithValue("@shiftDoUpdate", MainClass.shiftID);
                            cmd.Parameters.AddWithValue("@updateDate", DateTime.Now.Date);
                            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now.ToShortTimeString());

                            cmd.ExecuteNonQuery();
                        }
                    }
                });

                // 🔹 بعد تحديث البيانات، نعيد تحميل الفاتورة في الخلفية
                if (partyType == "عميل")
                    await GetBillDataFromDB(invoiceCode, false);
                else if (partyType == "مورد")
                    await GetBillDataFromDB(invoiceCode, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error updating details:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private async Task partiesTransfareAsync(double change, double newBalance, double oldBalance, bool isDelete)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();
                        using (SqlTransaction tran = con.BeginTransaction())
                        {
                            try
                            {
                                string insertQuery = @"
                            INSERT INTO PartiesTransactions
                                (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
                            VALUES
                                (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID, 
                                CAST(GETDATE() AS DATE), @aTime);";

                                using (SqlCommand cmd = new SqlCommand(insertQuery, con, tran))
                                {
                                    cmd.Parameters.AddWithValue("@partiesID", partiesID);
                                    cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                                    cmd.Parameters.AddWithValue("@mainID", mainID);
                                    cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                                    // 🟢 تحديد نوع العملية والمعلومات
                                    if (isDelete)
                                    {
                                        cmd.Parameters.AddWithValue("@transactionsType", "حذف");
                                        cmd.Parameters.AddWithValue("@transactionsInfo",
                                            $"تم حذف فاتورة بقيمة {change:N0}");
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@transactionsType", "مرتجعات");
                                        cmd.Parameters.AddWithValue("@transactionsInfo",
                                            $"تم إرجاع منتجات بقيمة {change:N0}");
                                    }

                                    cmd.Parameters.AddWithValue("@previousDebitBalance", oldBalance);
                                    cmd.Parameters.AddWithValue("@currentDebitBalance", newBalance);

                                    cmd.ExecuteNonQuery();
                                }

                                tran.Commit();
                            }
                            catch
                            {
                                tran.Rollback();
                                throw;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ خطأ أثناء تسجيل المعاملة:\n" + ex.Message,
                                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSearchParty_Click(object sender, EventArgs e)
        {
            frmPartesSearch frm = new frmPartesSearch(this);
            frm.type = partyType;
            frm.ShowDialog();
            this.Focus();
        }
        public void resultSearch(string pName)
        {
            txtName.Text = pName;
        }

        private async void dgvBills_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvBills.RowCount == 0 || dgvBills.FirstDisplayedScrollingRowIndex < 0)
                    return;

                if (dgvBills.FirstDisplayedScrollingRowIndex + dgvBills.DisplayedRowCount(false) >= dgvBills.RowCount)
                {
                    // تحميل الصفحة التالية
                    await DisplayBillsAsync(searchType, typeInvoice, isDeleted);
                }
            }
        }

        private void dgvBills_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvBills.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvBills.CurrentCell.ColumnIndex)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, true);

                    // ارسم النص بلون مختلف للهيدر المحدد
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.FormattedValue?.ToString(),
                        new Font("Tahoma", 11, FontStyle.Bold),
                        e.CellBounds,
                        Color.FromArgb(204, 204, 204),              // ← لون خط الهيدر المحدد
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    // ارسم حدود الخلية
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                }
            }
        }
        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.BringToFront();
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            //dgv.ReadOnly = true;
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
        }

        private void dgvProducts_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvProducts.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvProducts.CurrentCell.ColumnIndex)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, true);

                    // ارسم النص بلون مختلف للهيدر المحدد
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.FormattedValue?.ToString(),
                        new Font("Tahoma", 11, FontStyle.Bold),
                        e.CellBounds,
                        Color.FromArgb(204, 204, 204),              // ← لون خط الهيدر المحدد
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    // ارسم حدود الخلية
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                }
            }
        }

        private async void dgvProducts_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll &&
                e.NewValue + dgvProducts.DisplayedRowCount(false) >= dgvProducts.Rows.Count)
            {
                await LoadProductsPagedAsync();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void detellPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void billPanel_Paint(object sender, PaintEventArgs e)
        {
            //ControlPaint.DrawBorder(e.Graphics,
            //    billPanel.ClientRectangle,
            //    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid,   // يسار
            //    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid,   // أعلى
            //    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid,   // يمين
            //    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvProducts_Paint(object sender, PaintEventArgs e)
        {
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
            if (showTax)
            {
                ControlPaint.DrawBorder(e.Graphics,
                    mainPanel.ClientRectangle,
                    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid,   // يسار
                    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid,   // أعلى
                    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid,   // يمين
                    Color.FromArgb(1, 95, 95), 1, ButtonBorderStyle.Solid);
            }

        }

        private async void btnRestrunsBill_Click(object sender, EventArgs e)
        {
            await transactionStoreRerurn();

            if (partyType == "عميل")
                await billFlagDeleteCustomerAsync(false);
            else if (partyType == "مورد")
                await billFlagDeleteSuplieserAsync(false);
        }

        private void topPanel_Resize(object sender, EventArgs e)
        {

        }

        private void frmAll_Bills_Resize(object sender, EventArgs e)
        {
            if (!formAsBox)
            {
                lblTitle.Location = new Point(
                    (topPanel.Width - lblTitle.Width) / 2,
                    (topPanel.Height - lblTitle.Height) / 2
                );
            }
        }
    }
}
