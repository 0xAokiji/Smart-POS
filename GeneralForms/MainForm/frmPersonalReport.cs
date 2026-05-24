using pos.Classes;
using pos.Model.POS;
using pos.Model.Stor;
using pos.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.GeneralForms.MainForm
{
    public partial class frmPersonalReport : Form
    {
        public int partiesID = 0;
        public string partyType = "عميل";
        public int mainID = 0;
        public bool fromParties = false;
        public Dictionary<string, int> nameToID = new Dictionary<string, int>();

        private bool showAllReport = false;
        public frmPersonalReport()
        {
            InitializeComponent();
            textSuggester();

        }

        private void frmPersonalReport_SizeChanged(object sender, EventArgs e)
        {
            CenterPanel(panel2, mainPanel);

        }
        private async void frmPersonalReport_Load(object sender, EventArgs e)
        {
            dtPickerStart.Value = DateTime.Today;
            dtPickerEnd.Value = DateTime.Today;

            dtPickerStart.Format = DateTimePickerFormat.Custom;
            dtPickerStart.CustomFormat = "yyyy-MM-dd";

            dtPickerEnd.Format = DateTimePickerFormat.Custom;
            dtPickerEnd.CustomFormat = "yyyy-MM-dd";
            ApplyGridStyle(dgvDetainls);

            if (fromParties)
            {
                currentPage = 0;
                hasMoreData = true;
                await LoadDataAsync(true, false);
                showAllReport = true;
            }

        }
        private void CenterPanel(Panel panel, Panel mainPanel)
        {
            panel.Left = (mainPanel.Width - panel.Width) / 2;
        }

        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ الاتصال الآمن
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartyType", "%" + partyType + "%");

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


        private void cbPayWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChooseParyties.SelectedIndex == 0)
            {
                partyType = "عميل";
                textSuggester();
            }
            else
            {
                partyType = "مورد";
                textSuggester();

            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            hasMoreData = true;
            await LoadDataAsync(true, false);
            showAllReport = true;
        }

        int pageSize = 35;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;

        private async Task LoadDataAsync(bool isNewSearch = false, bool searchWithDate = true)
        {
            try
            {
                if (isLoading || !hasMoreData)
                    return;

                isLoading = true;

                // ✅ امسح القديم في أول صفحة
                if (isNewSearch)
                {
                    dgvDetainls.Rows.Clear();
                    currentPage = 0;
                    hasMoreData = true;
                }

                // ✅ الاستعلام الأساسي
                string qry = @"
            SELECT 
                pt.tID,
                pt.partiesID,
                pt.shiftID,
                pt.mainID,
                pt.transactionsInfo,
                pt.transactionsType,
                pt.previousDebitBalance,
                pt.currentDebitBalance,
                pt.aDate,
                pt.aTime,
                s.sName AS StaffName,
                m.InvoiceCode  
            FROM PartiesTransactions pt
            LEFT JOIN shifts sh ON pt.shiftID = sh.ID
            LEFT JOIN staff s ON sh.staffID = s.staffID
            LEFT JOIN tblMain1 m ON pt.mainID = m.MainID
            WHERE pt.partiesID = @PartyID
        ";

                // ✅ لو البحث بالتاريخ → نضيف شرط التاريخ
                if (searchWithDate)
                {
                    qry += " AND pt.aDate BETWEEN @StartDate AND @EndDate ";
                }

                // ✅ Pagination
                qry += @" ORDER BY pt.tID
                  OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                int offset = currentPage * pageSize;

                // ✅ تجهيز البراميترات
                List<SqlParameter> paramList = new List<SqlParameter>()
        {
            new SqlParameter("@PartyID", SqlDbType.Int) { Value = partiesID },
            new SqlParameter("@offset", SqlDbType.Int) { Value = offset },
            new SqlParameter("@limit", SqlDbType.Int) { Value = pageSize }
        };

                if (searchWithDate)
                {
                    paramList.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = dtPickerStart.Value.Date });
                    paramList.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = dtPickerEnd.Value.Date });
                }

                SqlParameter[] parameters = paramList.ToArray();

                // ✅ تحميل البيانات
                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                // ✅ لو أقل من حجم الصفحة → مفيش صفحات إضافية
                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                // ✅ تعبئة DataGridView
                int rowIndex = dgvDetainls.Rows.Count + 1;

                foreach (DataRow row in dt.Rows)
                {
                    int rowId = dgvDetainls.Rows.Add(
                        rowIndex++,
                        row["StaffName"],
                        row["InvoiceCode"],
                        row["transactionsType"],
                        row["transactionsInfo"],
                        Convert.ToDecimal(row["previousDebitBalance"]).ToString("N1", CultureInfo.InvariantCulture),
                        Convert.ToDecimal(row["currentDebitBalance"]).ToString("N1", CultureInfo.InvariantCulture),
                        Convert.ToDateTime(row["aDate"]).ToString("yyyy-MM-dd"),
                        row["aTime"]
                    );

                    // 🎨 تنسيق الصف حسب نوع العملية
                    DataGridViewRow dgvRow = dgvDetainls.Rows[rowId];
                    string type = row["transactionsType"].ToString();

                    switch (type)
                    {
                        case "سداد من الاجل":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#D4EDDA");
                            break;

                        case "فاتورة اجل":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#D1ECF1");
                            break;

                        case "مرتجعات":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFF3CD");
                            break;

                        case "حذف":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8D7DA");
                            break;

                        case "سحب":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E0D7F8");
                            break;

                        case "ايداع":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#D6F5E3");
                            break;

                        case "تعديل":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFE6CC");
                            break;

                        case "اضافة":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#CCE5FF");
                            break;

                        case "تعديل ايصال دفع":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E2E3E5");
                            break;

                        case "حذف العميل من الاجل":
                            dgvRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8D7DA");
                            break;

                        default:
                            dgvRow.DefaultCellStyle.BackColor = Color.White;
                            break;
                    }
                }

                // ✅ تفعيل زرار الطباعة
                btnPrint.Enabled = dgvDetainls.Rows.Count > 0;

                currentPage++;
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("حدث خطأ", ex.Message);
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
        private async void dgvDetainls_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvDetainls.FirstDisplayedScrollingRowIndex + dgvDetainls.DisplayedRowCount(false) >= dgvDetainls.RowCount)
                {
                    await LoadDataAsync(false, !showAllReport);
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtName.Text))
                partiesID = nameToID[txtName.Text];
            else
                partiesID = 0;
        }

        private void dgvDetainls_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var colName = dgvDetainls.Columns[e.ColumnIndex].Name;
            if (colName == "Column6" || colName == "Column5")
            {
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    if (decimal.TryParse(e.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    {
                        string s = d.ToString("N1", CultureInfo.InvariantCulture);
                        // بدّل الهايڤن بـ "علامة ناقص" U+2212 (اختياري لكنه يساعد)
                        s = s.Replace("-", "\u2212");
                        e.Value = "\u200E" + s;   // LRM قبل الرقم
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadDataAsync(true, false);

                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private void btnPartySearch_Click(object sender, EventArgs e)
        {
            if (cbChooseParyties.SelectedIndex == 0)
            {
                partyType = "عميل";
            }
            else
            {
                partyType = "مورد";
            }

            frmPartesSearch frm = new frmPartesSearch(this);
            frm.type = partyType;
            frm.ShowDialog();
            this.Focus();
        }
        public void resultSearch(string pName)
        {
            txtName.Text = pName;
        }


        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.Visible = true;
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
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

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

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            if(showAllReport)
            {
                await MainClass.PrintPartiesReportAsync2(
                null,     // StartDate = null
                null,     // EndDate = null
                partiesID,
                txtName.Text,
                cbChooseParyties.SelectedIndex == 1,
                true      // ✅ هنا بنقول للدالة: اطبع كل حاجة بدون تاريخ
                );
            }
            else
            {
                await MainClass.PrintPartiesReportAsync2(
                    dtPickerStart.Value.Date,
                    dtPickerEnd.Value.Date,
                    partiesID,
                    txtName.Text,
                    cbChooseParyties.SelectedIndex == 1,
                    false
                );
            }

        }

        private void dgvDetainls_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // نتأكد أن الضغط كان على صف فعلي وليس على رأس الأعمدة
            if (e.RowIndex >= 0)
            {
                // نجيب الصف اللي اتضغط عليه
                DataGridViewRow row = dgvDetainls.Rows[e.RowIndex];

                // نجيب قيمة الخلية حسب اسم العمود
                var value = row.Cells["dgvTransfareType"].Value;
                string transfareType = value != null ? value.ToString() : "";
                if (transfareType == "فاتورة اجل" || transfareType == "مرتجعات")
                {
                    string invoiceCode = row.Cells["dgvInvoiceCode"].Value.ToString();
                    using (frmBlackout frmblackout = new frmBlackout(this))
                    {
                        frmblackout.Show();
                        frmAll_Bills frm = new frmAll_Bills(invoiceCode, txtName.Text, cbChooseParyties.Text);
                        if (cbChooseParyties.SelectedIndex == 0)
                            frm.lblTitle.Text = "فاتورة عميل";
                        else if (cbChooseParyties.SelectedIndex == 1)
                            frm.lblTitle.Text = "فاتورة مورد";
                        frm.ShowDialog(this);
                    }

                }

            }
        }

        private async void btnSearchDate_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            hasMoreData = true;

            await LoadDataAsync(true, true);
            showAllReport = false;
        }

        private void frmPersonalReport_Resize(object sender, EventArgs e)
        {
            CenterPanel(panel2, mainPanel);

        }
    }
}
