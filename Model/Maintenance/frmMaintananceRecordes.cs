using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace pos.Model.Maintenance
{
    public partial class frmMaintananceRecordes : Form
    {
        public frmMaintananceRecordes()
        {
            InitializeComponent();
            // إعداد المؤقت مرة واحدة
            inputTimer.Interval = 500; // نص ثانية = 500 ملي ثانية
            inputTimer.Tick += InputTimer_Tick;
        }

        private async void frmMaintananceRecordes_Load(object sender, EventArgs e)
        {
            dtPickerStart.Value = DateTime.Today;
            dtPickerEnd.Value = DateTime.Today;

            dtPickerStart.Format = DateTimePickerFormat.Custom;
            dtPickerStart.CustomFormat = "yyyy-MM-dd";

            dtPickerEnd.Format = DateTimePickerFormat.Custom;
            dtPickerEnd.CustomFormat = "yyyy-MM-dd";

            ApplyGridStyle(dgvTasks); // ✅ تطبيق التنسيق
            await loadData();          // نفذ البحث
        }
        private System.Windows.Forms.Timer inputTimer = new System.Windows.Forms.Timer();
        int pageSize = 30;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;
        private bool allLoaded = false;
        private async Task loadData(bool isNewSearch = false, bool searchWithDate = false)
        {
            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {
                // ✅ امسح الصفحة الأولى قبل تحميل البيانات
                if (isNewSearch)
                {
                    dgvTasks.Rows.Clear();
                    currentPage = 0;
                    hasMoreData = true;
                }

                string qry = @"
SELECT 
    t.taskID,
    t.paryID,
    t.taskNumber,
    t.partyNotes,
    t.tecnicalID,
    t.descriptionProblem,
    t.Priority,
    t.PriorityName,
    t.taskPrice,
    t.status,
    t.paymentStatus,
    t.startDate,
    t.startTime,
    t.endDate,
    t.endTime,
    p.pName AS PartyName,
    p.pPhone AS PartyPhone,
    s.sName AS TecnicalName,
    s.sPhone AS TecnicalPhone,
    m.TotalWithInterest,
    m.InvoiceCode AS BillCode
FROM Task t
INNER JOIN Parties p ON t.paryID = p.pID
INNER JOIN staff s ON t.tecnicalID = s.staffID
LEFT JOIN tblMain1 m ON t.taskID = m.taskID
WHERE 1 = 1 
";

                // ✅ إضافة شروط البحث لو فيه نص
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    qry += @"
AND (
        t.taskNumber LIKE '%' + @SearchText + '%'
     OR p.pName LIKE '%' + @SearchText + '%'
     OR s.sName LIKE '%' + @SearchText + '%'
    )";
                }

                // ✅ إضافة شرط التاريخ إن كان مفعّلاً
                if (searchWithDate)
                {
                    qry += " AND t.endDate BETWEEN @StartDate AND @EndDate ";
                }

                qry += @"
ORDER BY t.endDate DESC
OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                int offset = currentPage * pageSize;

                // ✅ إنشاء الباراميترات
                List<SqlParameter> paramList = new List<SqlParameter>
        {
            new SqlParameter("@offset", offset),
            new SqlParameter("@limit", pageSize)
        };

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    paramList.Add(new SqlParameter("@SearchText", txtSearch.Text));
                }

                if (searchWithDate)
                {
                    paramList.Add(new SqlParameter("@StartDate", dtPickerStart.Value.Date));
                    paramList.Add(new SqlParameter("@EndDate", dtPickerEnd.Value.Date));
                }

                DataTable dt = await Task.Run(() => LoadDataReturn(qry, paramList.ToArray()));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                int rowIndex = dgvTasks.Rows.Count + 1;

                decimal totalTaskPrice = 0; // 🆕 لتجميع قيم taskPrice

                foreach (DataRow row in dt.Rows)
                {
                    decimal taskPrice = row["taskPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(row["taskPrice"]);
                    decimal totalWithInterest = row["TotalWithInterest"] == DBNull.Value ? 0 : Convert.ToDecimal(row["TotalWithInterest"]);


                    dgvTasks.Rows.Add(
                        rowIndex++,
                        false,
                        row["taskID"],
                        row["PartyName"]?.ToString() ?? "غير معروف",
                        row["PartyPhone"]?.ToString() ?? "غير معروف",
                        row["TecnicalName"]?.ToString() ?? "غير معروف",
                        row["TecnicalPhone"]?.ToString() ?? "غير معروف",
                        row["status"]?.ToString() ?? "",
                        row["paymentStatus"]?.ToString() ?? "",
                        totalWithInterest.ToString("N0"),
                        taskPrice.ToString("N0"),
                        (totalWithInterest + taskPrice).ToString("N0"),
                        row["startDate"] == DBNull.Value ? "غير معروف" : Convert.ToDateTime(row["startDate"]).ToString("yyyy-MM-dd"),
                        row["endDate"] == DBNull.Value ? "غير معروف" : Convert.ToDateTime(row["endDate"]).ToString("yyyy-MM-dd")
                    );
                }

                // 🆕 عرض مجموع taskPrice في TextBox
                txtPrice.Text = totalTaskPrice.ToString("N0");

                currentPage++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message);
            }
            finally
            {
                isLoading = false;
                UpdateTotals();
            }
        }

        private void UpdateTotals()
        {
            decimal totalTaskPrice = 0;
            decimal totalBillPrice = 0;

            foreach (DataGridViewRow row in dgvTasks.Rows)
            {
                // جمع TaskPrice
                if (row.Cells["dgvTaskPrice"].Value != null &&
                    decimal.TryParse(row.Cells["dgvTaskPrice"].Value.ToString(), out decimal taskValue))
                {
                    totalTaskPrice += taskValue;
                }

                // جمع BillPrice
                if (row.Cells["dgvBillPrice"].Value != null &&
                    decimal.TryParse(row.Cells["dgvBillPrice"].Value.ToString(), out decimal billValue))
                {
                    totalBillPrice += billValue;
                }
            }

            txtPrice.Text = totalTaskPrice.ToString("N0");
            txtBillsTotal.Text = totalBillPrice.ToString("N0");
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                txtSearch.TextAlign = HorizontalAlignment.Left;
            }
            else
            {
                // ✅ تحقق من أول حرف بطريقة آمنة
                char firstChar = searchText[0];
                txtSearch.TextAlign = IsArabic(firstChar) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }

            inputTimer.Stop();  // كل ما يكتب المستخدم نوقف المؤقت
            inputTimer.Start(); // ونعيد تشغيله
        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private async void InputTimer_Tick(object sender, EventArgs e)
        {
            inputTimer.Stop(); // وقف المؤقت بعد ما يخلص
            currentPage = 0;
            hasMoreData = true;
            await loadData(true); // ✅ نبدأ بحث جديد
        }
        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.Visible = true;
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
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);

            // الألوان العادية للصفوف
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);

            // الصفوف المتبادلة (صف غامق وصف فاتح)
            dgv.AlternatingRowsDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // الرمادي الفاتح
            dgv.RowsDefaultCellStyle.BackColor = Color.White;                              // الصف العادي

            // ألوان التحديد (Selection)
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(204, 204, 204);

            // ✅ ألوان الهيدر (عادي + خط)
            dgv.EnableHeadersVisualStyles = false; // مهم عشان ألوانك تشتغل
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;                 // لون خط الهيدر

            // لون الهيدر وقت التحديد (لو حابب تخليه مختلف)
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(1, 95, 95);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        }

        private async void dgvTasks_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvTasks.FirstDisplayedScrollingRowIndex + dgvTasks.DisplayedRowCount(false) >= dgvTasks.RowCount)
                {
                    await loadData(false, withDate); // ✅ تحميل الصفحة التالية
                }
            }
        }

        private void dgvTasks_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvTasks.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvTasks.CurrentCell.ColumnIndex)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, true);

                    // ارسم النص بلون مختلف للهيدر المحدد
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.FormattedValue?.ToString(),
                        new Font("Tahoma", 10, FontStyle.Bold),
                        e.CellBounds,
                        Color.FromArgb(204, 204, 204),              // ← لون خط الهيدر المحدد
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    // ارسم حدود الخلية
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                }
            }
        }

        bool withDate = false;
        private async void btnSearchDate_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            hasMoreData = true;
            await loadData(isNewSearch: true, searchWithDate: true);
            withDate = true;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            hasMoreData = true;
            await loadData(isNewSearch: true, searchWithDate: false);
            withDate = false;
        }
    }
}
