using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.GeneralForms.MainForm
{
    public partial class frmFinancialReports : Form
    {
        private int chabngForMe;
        private decimal totalBillCustomer;

        private int chabngForSupplieser;
        private decimal totalBillSupplieser;

        private int type;
        public frmFinancialReports()
        {
            InitializeComponent();
        }

        private void frmFinancialReports_Load(object sender, EventArgs e)
        {
            dtPickerStart.Value = DateTime.Today;
            dtPickerEnd.Value = DateTime.Today;

            dtPickerStart.Format = DateTimePickerFormat.Custom;
            dtPickerStart.CustomFormat = "yyyy-MM-dd";

            dtPickerEnd.Format = DateTimePickerFormat.Custom;
            dtPickerEnd.CustomFormat = "yyyy-MM-dd";

            ApplyGridStyle(dgvDetainls);
        }

        private void btnReportSearch_Click(object sender, EventArgs e)
        {
            resduals();
            showReportPaids();
            showReportPaisSuplieser();
            decimal totalSalaries = GetTotalSalaries();
            txtSalaries.Text = totalSalaries.ToString("N0");

            decimal pruches = purches();
            txtPurches.Text = pruches.ToString("N0");

            decimal totalExpenses = totalBillSupplieser + totalSalaries + pruches;
            txtTotalExpenses.Text = totalExpenses.ToString("N0");

            decimal profit = totalBillCustomer - totalExpenses;
            txtProfit.Text = profit.ToString("N0");
            txtPaid.Text = totalBillCustomer.ToString("N0");
        }
        private void showReportPaids()
        {
            string qry = @"
                   SELECT 
                    COUNT(*) AS TotalFinishedRows,
                    SUM(TotalWithInterest) AS TotalWithInterestSum,
                    SUM(CASE WHEN PaymentMethod = N'كاش' THEN 1 ELSE 0 END) AS CashCount,
                    SUM(CASE WHEN PaymentMethod = N'اجل' THEN 1 ELSE 0 END) AS AglCount
                FROM tblMain1
                WHERE status = 'finshed'
                  AND aDate BETWEEN @StartDate AND @EndDate;
                        ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                // 🟢 تمرير قيم التاريخ
                cmd.Parameters.AddWithValue("@StartDate", dtPickerStart.Value.Date);
                cmd.Parameters.AddWithValue("@EndDate", dtPickerEnd.Value.Date);

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataRow row = dt2.Rows[0];

                        // 🆕 متغيرات int


                        totalBillCustomer = row["TotalWithInterestSum"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalWithInterestSum"]))
                            : 0;

                        txtPaisAmountCustomer.Text = totalBillCustomer.ToString("N0");

                        txtPayBillNumer.Text = row["TotalFinishedRows"] != DBNull.Value
                            ? Convert.ToInt32(row["TotalFinishedRows"]).ToString()
                            : "0";

                        txtCash.Text = row["CashCount"] != DBNull.Value
                            ? Convert.ToInt32(row["CashCount"]).ToString()
                            : "0";

                        txtAglcount.Text = row["AglCount"] != DBNull.Value
                            ? Convert.ToInt32(row["AglCount"]).ToString()
                            : "0";
                    }
                }
            }


        }
        private void showReportPaisSuplieser()
        {
            string qry = @"
                      SELECT 
                        COUNT(*) AS TotalFinishedRows,
                        SUM(clear) AS TotalWithInterestSum
                    FROM billPrcheses
                    WHERE billStatus = 'Finish'
                      AND [date] BETWEEN @StartDate AND @EndDate;
                    ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                // 🟢 تمرير قيم التاريخ
                cmd.Parameters.AddWithValue("@StartDate", dtPickerStart.Value.Date);
                cmd.Parameters.AddWithValue("@EndDate", dtPickerEnd.Value.Date);

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataRow row = dt2.Rows[0];

                        // 🆕 متغيرات int


                        totalBillSupplieser = row["TotalWithInterestSum"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalWithInterestSum"]))
                            : 0;

                        txtPaisAmountSupplieser.Text = totalBillSupplieser.ToString("N0");

                        txtPayBillNumerSupplieser.Text = row["TotalFinishedRows"] != DBNull.Value
                            ? Convert.ToInt32(row["TotalFinishedRows"]).ToString()
                            : "0";

                    }
                }
            }
        }
        private void CenterPanel(Panel panel, Panel mainPanel)
        {
            panel.Left = (mainPanel.Width - panel.Width) / 2;
            // myGroupBox.Top = (panel.Height - myGroupBox.Height) / 2;
        }
        private void frmFinancialReports_SizeChanged(object sender, EventArgs e)
        {
            CenterPanel(panel2, mainPanel);

        }

        private decimal GetTotalSalaries()
        {
            decimal totalSalaries = 0;

            // ناخد التواريخ من الـ DateTimePickers
            DateTime startDate = dtPickerStart.Value.Date;
            DateTime endDate = dtPickerEnd.Value.Date;

            string qry = @"
        SELECT SUM(SalaryAmount) AS TotalSalaries
        FROM Salaries
        WHERE 
            CAST(CAST(SalaryYear AS VARCHAR(4)) + '-' + 
                 RIGHT('0' + CAST(SalaryMonth AS VARCHAR(2)), 2) + '-01' AS DATE)
            BETWEEN @StartDate AND @EndDate;
    ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                con.Open();
                object result = cmd.ExecuteScalar();
                totalSalaries = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }

            return totalSalaries;
        }

        private void resduals()
        {
            string qry = @"
                    SELECT 
                        SUM(CASE WHEN p.PartyType = N'عميل' THEN r.currentDebitBalance ELSE 0 END) AS TotalForCustomer,
                        SUM(CASE WHEN p.PartyType = N'مورد' THEN r.currentDebitBalance ELSE 0 END) AS TotalForSupplier
                    FROM residualTable r
                    JOIN Parties p ON r.PartiesID = p.pID;
                    ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataRow row = dt2.Rows[0];

                        // 🆕 متغيرات int


                        chabngForMe = row["TotalForCustomer"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalForCustomer"]))
                            : 0;

                        txtchangeForMe.Text = chabngForMe.ToString("N0");

                        chabngForSupplieser = row["TotalForSupplier"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalForSupplier"]))
                            : 0;

                        txtchangeForSupplieser.Text = chabngForSupplieser.ToString("N0");

                    }
                }
            }
        }
        private decimal purches()
        {
            decimal totalAmount = 0;
            // ناخد التواريخ من الـ DateTimePickers
            DateTime startDate = dtPickerStart.Value.Date;
            DateTime endDate = dtPickerEnd.Value.Date;

            string qry = @"
                SELECT SUM(ISNULL(price, 0)) AS Totalprice
                FROM purchases
                WHERE aDate >= @startDate AND aDate <= @endDate
                ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                totalAmount = (result != DBNull.Value) ? Convert.ToDecimal(result) : 0m;
            }

            return totalAmount;
        }
        private async void btnDetailPaid_Click(object sender, EventArgs e)
        {
            if (btnDetailPaid.Checked)
                return;

            btnDetainlsSuplieser.Checked = false;
            btnDetailPaid.Checked = true;

            dgvDetainls.Visible = true;
            currentPage = 0;
            hasMoreData = true;

            await LoadDataAsync(1);
            type = 1;
        }

        int pageSize = 20;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;
        private async Task LoadDataAsync(int type)
        {
            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {
                // ✅ امسح القديم بس في أول صفحة
                if (currentPage == 0)
                    dgvDetainls.Rows.Clear();

                // 📝 حساب الـ offset
                int offset = currentPage * pageSize;

                // 📝 تجهيز باراميترات التاريخ و الـ pagination
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = dtPickerStart.Value.Date },
                    new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = dtPickerEnd.Value.Date },
                    new SqlParameter("@offset", SqlDbType.Int) { Value = offset },
                    new SqlParameter("@limit", SqlDbType.Int) { Value = pageSize }
                };

                // 📝 الاستعلام حسب النوع
                string qry = "";

                if (type == 1) // ✅ مبيعات
                {
                    qry = @"
                    SELECT 
                        d.unite AS UnitName,
                        d.uniteID,
                        d.proName AS ProductName,
                        CASE 
                            WHEN d.isUsed = 0 THEN N'جديد'
                            WHEN d.isUsed = 1 THEN N'مستعمل'
                        END AS ProductStatus,
                        SUM(d.qty) AS TotalQty,
                        SUM(d.qty * d.price) AS TotalAmount,
                        MIN(m.aDate) AS FirstDate,
                        MAX(m.aDate) AS LastDate
                    FROM tblDetails d
                    INNER JOIN tblMain1 m ON d.MainID = m.MainID
                    WHERE 
                        (d.DeleteFlag IS NULL OR d.DeleteFlag = 0) 
                        AND (m.DeleteFlag IS NULL OR m.DeleteFlag = 0)
                        AND m.aDate BETWEEN @StartDate AND @EndDate
                    GROUP BY 
                        d.unite, d.uniteID, d.proName, d.isUsed
                    ORDER BY 
                        d.uniteID, ProductStatus, ProductName
                    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";
                }
                else if (type == 2) // ✅ مشتريات
                {
                    qry = @"
                    SELECT 
                        d.unite AS UnitName,                      
                        d.uniteID,                                
                        d.proID,                                  
                        p.pName AS ProductName,                   
                        CASE 
                            WHEN d.isUsed = 0 THEN N'جديد'
                            WHEN d.isUsed = 1 THEN N'مستعمل'
                        END AS ProductStatus,                     
                        SUM(d.qty) AS TotalQty,                   
                        SUM(d.amount) AS TotalAmount,             
                        MIN(b.date) AS FirstDate,                 
                        MAX(b.date) AS LastDate                   
                    FROM tblDetailsSupliser d
                    INNER JOIN billPrcheses b 
                        ON d.billPrchesesID = b.bID
                    INNER JOIN products p 
                        ON d.proID = p.pID                        
                    WHERE 
                        (d.DeleteFlag IS NULL OR d.DeleteFlag = 0) 
                        AND (b.DeleteFlag IS NULL OR b.DeleteFlag = 0)
                        AND b.date BETWEEN @StartDate AND @EndDate
                    GROUP BY 
                        d.unite, d.uniteID, d.proID, p.pName, d.isUsed
                    ORDER BY 
                        d.uniteID, ProductStatus, d.proID
                    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";
                }

                // 🔍 تحميل البيانات Asynchronous
                DataTable dt = await Task.Run(() =>
                {
                    DataTable table = new DataTable();
                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        con.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    return table;
                });


                int rowIndex = dgvDetainls.Rows.Count + 1;
                foreach (DataRow row in dt.Rows)
                {
                    dgvDetainls.Rows.Add(
                        rowIndex++,
                        row["ProductName"],
                        row["TotalQty"],
                        row["ProductStatus"],
                        row["UnitName"],
                        Convert.ToDecimal(row["TotalAmount"]).ToString("N0")
                    );
                }

                // ✅ لو البيانات أقل من حجم الصفحة → مفيش صفحات تانية
                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                // زيادة الصفحة
                currentPage++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل البيانات:\n" + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
            }
        }



        private async void btnDetainlsSuplieser_Click(object sender, EventArgs e)
        {
            if (btnDetainlsSuplieser.Checked)
                return;
            btnDetailPaid.Checked = false;
            btnDetainlsSuplieser.Checked = true;

            dgvDetainls.Visible = true;
            currentPage = 0;
            hasMoreData = true;
            await LoadDataAsync(2);
            type = 2;

        }

        private async void dgvDetainls_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvDetainls.FirstDisplayedScrollingRowIndex + dgvDetainls.DisplayedRowCount(false) >= dgvDetainls.RowCount)
                {
                    await LoadDataAsync(type); // ✅ تحميل الصفحة التالية
                }
            }
        }

        private void dgvDetainls_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvDetainls.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvDetainls.CurrentCell.ColumnIndex)
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
            //dgv.Visible = true;
            //dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
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

    }
}
