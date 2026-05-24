using DevExpress.XtraBars.Customization;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Utils;
using Guna.UI2.WinForms;
using pos.Classes;
using pos.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pos.View
{
    public partial class frmShortcomings : SampleView
    {
        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;


        int pageSize = 30;
        bool hasMoreData = true;

        // 🔥 متغيرات Global لتخزين الفلاتر
        private string currentCategoryValue = null;
        private string currentSearchValue = null;
        private int currentPage;
        private bool isLoading;
        private bool? filterShowInShortcomming = true;

        public frmShortcomings()
        {
            InitializeComponent();
            textSuggester();
            //ThemeMode();
        }
        private async void frmShortcomings_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvProductes);


            // تحميل البيانات
            await GetData();
            AddCategory();
        }
        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.Visible = true;
            dgv.Dock = DockStyle.Fill;
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
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(1, 95, 95);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(204, 204, 204);

            // ✅ ألوان الهيدر (عادي + خط)
            dgv.EnableHeadersVisualStyles = false; // مهم عشان ألوانك تشتغل
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;                 // لون خط الهيدر

            // لون الهيدر وقت التحديد (لو حابب تخليه مختلف)
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(1, 95, 95);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        }


        public async Task GetData(string categoryValue = null)
        {
            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {
                // ✅ لو فيه categoryValue جديدة، حدث المتغير
                if (categoryValue != null)
                    currentCategoryValue = categoryValue;

                // ✅ دايمًا خزن البحث الحالي
                currentSearchValue = string.IsNullOrWhiteSpace(txtSearch1.Text) ? null : txtSearch1.Text.Trim();

                string qry = @"
                SELECT q.*, 
                       CASE WHEN u.uName IS NULL OR u.uName = '' 
                            THEN N'غير محدد' 
                            ELSE u.uName 
                       END AS uniteName
                FROM (
                    SELECT 
                        MIN(p.pID) AS pID, 
                        CASE WHEN MIN(p.pName) IS NULL OR MIN(p.pName) = '' 
                             THEN N'غير محدد' ELSE MIN(p.pName) END AS pName,
                        CASE WHEN MIN(p.pCode) IS NULL OR MIN(p.pCode) = '' 
                             THEN N'غير محدد' ELSE MIN(p.pCode) END AS pCode,
                        CASE WHEN MIN(c.catName) IS NULL OR MIN(c.catName) = '' 
                             THEN N'غير محدد' ELSE MIN(c.catName) END AS catName,
                        CASE WHEN MIN(p.compName) IS NULL OR MIN(p.compName) = '' 
                             THEN N'غير محدد' ELSE MIN(p.compName) END AS compName,

                        CASE WHEN ts.qtyU1 IS NULL OR ts.qtyU1 = 0 
                             THEN N'غير متوفر' ELSE CAST(ts.qtyU1 AS NVARCHAR) END AS TotalQtyNew,
                        CASE WHEN ts.qtyUsedU1 IS NULL OR ts.qtyUsedU1 = 0 
                             THEN N'غير متوفر' ELSE CAST(ts.qtyUsedU1 AS NVARCHAR) END AS TotalQtyUsed,

                        p.idUnite1,
                        p.showInShortcomming
                    FROM 
                        products p 
                    JOIN 
                        category c ON p.categoryID = c.catID 
                    JOIN 
                        totalStor ts ON ts.pID = p.pID 
                    WHERE
                        (@catName IS NULL OR @catName = '' OR c.catName LIKE '%' + @catName + '%')
                        AND
                        (@searchName IS NULL OR @searchName = '' 
                            OR p.pName LIKE '%' + @searchName + '%'
                            OR p.pNewBarode LIKE '%' + @searchName + '%'
                            OR p.pUsedBarode LIKE '%' + @searchName + '%')
                        AND (
                               (ts.qtyU1 < p.minimumP AND p.sellPrice IS NOT NULL AND p.sellPrice <> 0)
                            OR (ts.qtyUsedU1 < p.minimumP AND p.sellPriceUsed IS NOT NULL AND p.sellPriceUsed <> 0)
                        )                        AND (@showInShortcomming IS NULL OR p.showInShortcomming = @showInShortcomming)
                    GROUP BY 
                        p.pName, ts.qtyU1, ts.qtyUsedU1, p.idUnite1, p.showInShortcomming
                ) q
                LEFT JOIN untits u ON u.uID = q.idUnite1
                ORDER BY q.pID
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";


                int offset = currentPage * pageSize;

                SqlParameter[] parameters = {
                    new SqlParameter("@catName", (object)currentCategoryValue ?? DBNull.Value),
                    new SqlParameter("@searchName", (object)currentSearchValue ?? DBNull.Value),
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize),
                    new SqlParameter("@showInShortcomming", (object)filterShowInShortcomming ?? DBNull.Value)
                };


                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                int rowIndex = dgvProductes.Rows.Count + 1;

                foreach (DataRow row in dt.Rows)
                {

                    dgvProductes.Rows.Add(
                        rowIndex++,
                        row["pID"],
                        row["pName"],
                        row["catName"],
                        row["compName"],
                        row["pCode"],
                        row["uniteName"],
                        row["TotalQtyNew"],
                        row["TotalQtyUsed"],
                        !(row["showInShortcomming"] != DBNull.Value && Convert.ToBoolean(row["showInShortcomming"])) // ✅ عكس القيمة
                    );
                }

                currentPage++;
                SetupDataGridView();

            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء جلب البيانات.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
            }
        }
        private void ResetData()
        {
            dgvProductes.Rows.Clear();
            dgvProductes.DataSource = null;
            currentPage = 0;
            isLoading = false;
            hasMoreData = true;
        }
        private static string selectedCategory = "الكل";

        private void AddCategory()
        {
            try
            {
                string qry = "SELECT catID, catName FROM v_CategoryArabicSorted ORDER BY SortOrder, catName;";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    CategoryPanel.Controls.Clear();

                    // 🔹 زر "الكل"
                    Guna.UI2.WinForms.Guna2Button allCategoriesButton = new Guna.UI2.WinForms.Guna2Button();
                    allCategoriesButton.Size = new Size(160, 45);
                    allCategoriesButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    allCategoriesButton.Text = "الكل";
                    allCategoriesButton.CheckedState.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
                    allCategoriesButton.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    allCategoriesButton.AutoRoundedCorners = false;
                    allCategoriesButton.BorderRadius = 8;
                    allCategoriesButton.BorderThickness = 1;

                    if (MainClass.ThemeMode == "dark")
                    {
                        allCategoriesButton.CheckedState.FillColor = Color.FromArgb(1, 95, 95);
                        allCategoriesButton.CheckedState.BorderColor = Color.FromArgb(136, 214, 218);
                        allCategoriesButton.ForeColor = Color.FromArgb(51, 51, 51);
                        allCategoriesButton.FillColor = Color.FromArgb(136, 214, 218);
                        allCategoriesButton.CheckedState.ForeColor = Color.White;
                        allCategoriesButton.BorderColor = Color.FromArgb(1, 95, 95);
                    }
                    else
                    {
                        allCategoriesButton.FillColor = Color.FromArgb(1, 95, 95);
                        allCategoriesButton.ForeColor = Color.White;
                        allCategoriesButton.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
                        allCategoriesButton.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
                        allCategoriesButton.CheckedState.ForeColor = Color.FromArgb(51, 51, 51);
                        allCategoriesButton.BorderColor = Color.Gray;
                    }

                    allCategoriesButton.Checked = (selectedCategory == "الكل");
                    allCategoriesButton.AutoRoundedCorners = true;
                    allCategoriesButton.Click += (s, e) =>
                    {
                        selectedCategory = "الكل";
                        b_clik(s, e);
                    };

                    Guna2Separator separator = new Guna2Separator();
                    separator.Size = new Size(160, 12);
                    separator.FillThickness = 3;

                    CategoryPanel.Controls.Add(allCategoriesButton);
                    CategoryPanel.Controls.Add(separator);

                    // 🔹 إضافة باقي الفئات
                    foreach (DataRow row in dt.Rows)
                    {
                        string catName = row["catName"].ToString();
                        Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                        b.Size = new Size(160, 35);
                        b.AutoRoundedCorners = true;
                        b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                        b.Text = catName;
                        b.BorderThickness = 1;
                        b.CheckedState.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
                        b.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                        b.AutoRoundedCorners = false;
                        b.BorderRadius = 8;

                        if (MainClass.ThemeMode == "dark")
                        {
                            b.CheckedState.FillColor = Color.FromArgb(1, 95, 95);
                            b.CheckedState.BorderColor = Color.FromArgb(136, 214, 218);
                            b.ForeColor = Color.FromArgb(51, 51, 51);
                            b.FillColor = Color.FromArgb(136, 214, 218);
                            b.CheckedState.ForeColor = Color.White;
                            b.BorderColor = Color.FromArgb(1, 95, 95);
                        }
                        else
                        {
                            b.FillColor = Color.FromArgb(1, 95, 95);
                            b.ForeColor = Color.White;
                            b.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
                            b.CheckedState.ForeColor = Color.FromArgb(51, 51, 51);
                            b.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
                            b.BorderColor = Color.Gray;
                        }

                        b.Checked = (selectedCategory == catName);

                        b.Click += (s, e) =>
                        {
                            selectedCategory = catName;
                            b_clik(s, e);
                        };

                        CategoryPanel.Controls.Add(b);
                    }
                }
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
            }
        }

        private string lastSelectedCategory = string.Empty;
        private string selectedCategoryName;
        private async void b_clik(object? sender, EventArgs e)
        {
            var b = (Guna.UI2.WinForms.Guna2Button)sender;
            txtSearch1.Text = string.Empty;

            selectedCategoryName = (b.Text == "الكل") ? string.Empty : b.Text;

            // إذا ضغط على نفس الفئة، لا نفعل شيئاً
            if (selectedCategoryName == lastSelectedCategory)
                return;

            // حدّث الفئة الأخيرة
            lastSelectedCategory = selectedCategoryName;

            ResetData();
            dgvProductes.Rows.Clear();
            dgvProductes.DataSource = null;


            await GetData(selectedCategoryName); // 🔥 استخدم await مباشر

        }
        public static DataTable LoadDataReturn(string qry, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدام GetConnection
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
        private void SetupDataGridView()
        {
            // 🔹 إعدادات الصفوف والعناوين
            dgvProductes.RowTemplate.Height = 35;
            dgvProductes.ColumnHeadersHeight = 45;

            dgvProductes.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgvProductes.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

            // 🔹 الأعمدة: حجم ثابت + منع الترتيب
            foreach (DataGridViewColumn col in dgvProductes.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                col.SortMode = DataGridViewColumnSortMode.NotSortable; // 🔥 منع الترتيب
            }

            // ✅ لف النص في العمود المطلوب
            dgvProductes.Columns["dgvName2"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            // ✅ تحسين مظهر النص
            dgvProductes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private System.Windows.Forms.Timer searchTimer;
        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            // ✅ تحقق إذا التكست فاضي
            string searchText = txtSearch1.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                txtSearch1.TextAlign = HorizontalAlignment.Left;
            }
            else
            {
                // ✅ تحقق من أول حرف بطريقة آمنة
                char firstChar = searchText[0];
                txtSearch1.TextAlign = IsArabic(firstChar) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }

            // ✅ أوقف المؤقت القديم لو شغال
            if (searchTimer != null)
            {
                searchTimer.Stop();
                searchTimer.Dispose();
            }

            // ✅ إعداد مؤقت جديد
            searchTimer = new System.Windows.Forms.Timer
            {
                Interval = 500 // نصف ثانية
            };

            searchTimer.Tick += async (s, args) =>
            {
                searchTimer.Stop();
                searchTimer.Dispose();

                // 🔄 إعادة ضبط البيانات
                ResetData();
                dgvProductes.Rows.Clear();
                dgvProductes.DataSource = null;



                if (string.IsNullOrEmpty(searchText))
                    await GetData(lastSelectedCategory);
                else
                    await GetData(string.Empty);

            };

            searchTimer.Start();

        }
        private void textSuggester()
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

                    txtSearch1.AutoCompleteCustomSource = dataSource;
                    txtSearch1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtSearch1.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }

        public void ReloadData()
        {

            ThemeMode();


            if (storPanel != null)
            {
                foreach (System.Windows.Forms.Control c in storPanel.Controls)
                {
                    if (c is ucStore item)
                    {
                        item.themRefresh();
                    }
                }
            }
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
                txtSearch1.IconRight = Properties.Resources.search_Dark;

            }
            else if (MainClass.ThemeMode == "light")
            {

                txtSearch1.IconRight = Properties.Resources.search_ligh;
            }

            ThemeColor();

            this.BackColor = backgroundPrimary;

            //Panels
            storPanel.BackColor = backgroundPrimary;
            topPanel.BackColor = checkedFillColor;

        }

        private void dgvProductes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void dgvProductes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvProductes.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvProductes.CurrentCell.ColumnIndex)
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

        private async void dgvProductes_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvProductes.FirstDisplayedScrollingRowIndex + dgvProductes.DisplayedRowCount(false) >= dgvProductes.RowCount)
                {
                    await GetData(selectedCategoryName); // 🔥 استخدم await مباشر
                }
            }
        }

        private async void dgvProductes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dgvProductes.Columns[e.ColumnIndex].Name;

                if (columnName == "dgvShow")
                {
                    // الخلية اللي اتضغط عليها
                    var cell = dgvProductes.Rows[e.RowIndex].Cells["dgvShow"];
                    bool isChecked = cell.Value != null && (bool)cell.Value;

                    // القيمة الجديدة بعد الضغط (عكس القديمة)
                    bool newValue = !isChecked;

                    if (newValue == true) // المستخدم اختار إخفاء المنتج
                    {
                        DialogResult result = MessageBox.Show(
                            "هل تريد إخفاء المنتج من النواقص؟",
                            "تأكيد",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.OK)
                        {
                            // ✅ نفذ الكود اللي انت عايزه هنا
                            object idValue = dgvProductes.Rows[e.RowIndex].Cells["dgvproID"].Value;
                            if (idValue != null)
                            {
                                int productId = Convert.ToInt32(idValue);
                                string productName = dgvProductes.Rows[e.RowIndex].Cells["dgvName2"].Value.ToString();
                                HideFromShortcoming(productId, productName, false);
                                ResetData();
                                await GetData(selectedCategoryName); // 🔥 استخدم await مباشر

                            }


                            cell.Value = true; // ثبت القيمة
                        }
                        else
                        {
                            // ❌ رجع القيمة القديمة (ما تغيرش)
                            cell.Value = false;
                        }
                    }
                    else
                    {
                        // لو المستخدم رجعها False (يعني يظهر المنتج في النواقص)
                        cell.Value = false;
                        DialogResult result = MessageBox.Show(
                            "هل تريد اظهار هذا المنتج في النواقص؟",
                            "تأكيد",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.OK)
                        {
                            // ✅ نفذ الكود اللي انت عايزه هنا
                            object idValue = dgvProductes.Rows[e.RowIndex].Cells["dgvproID"].Value;
                            if (idValue != null)
                            {
                                int productId = Convert.ToInt32(idValue);
                                string productName = dgvProductes.Rows[e.RowIndex].Cells["dgvName2"].Value.ToString();
                                HideFromShortcoming(productId, productName, true);
                                ResetData();
                                await GetData(selectedCategoryName); // 🔥 استخدم await مباشر

                            }


                            cell.Value = true; // ثبت القيمة
                        }
                        else
                        {
                            // ❌ رجع القيمة القديمة (ما تغيرش)
                            cell.Value = false;
                        }
                    }
                }
            }
        }
        public static void HideFromShortcoming(int productId, string productName, bool show)
        {
            string query = "UPDATE products SET showInShortcomming = @show WHERE pID = @ProductID";

            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@show", show);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        if (show)
                        {
                            Notifier.ShowNotification("تم", $"تم اظهار المنتج  {productName} في النواقص ✅");

                        }
                        else
                            Notifier.ShowNotification("تم", $"تم إخفاء المنتج  {productName} من النواقص ✅");

                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على المنتج ❌", "خطأ",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ: " + ex.Message, "خطأ",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvProductes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // ✅ لو دبل كليك على الهيدر (RowIndex = -1) اخرج
            if (e.RowIndex < 0) return;

            // ✅ لو العمود هو dgvShow اخرج
            if (dgvProductes.Columns[e.ColumnIndex].Name == "dgvShow") return;

            // ✅ هات قيمة ID
            object idValue = dgvProductes.Rows[e.RowIndex].Cells["dgvproID"].Value;

            if (idValue != null && int.TryParse(idValue.ToString(), out int id))
            {
                frmCategoryCard frm = new frmCategoryCard();
                frm.Owner = this;
                frm.id = id;
                frm.ShowDialog();
                this.Focus();
            }
        }

        private async void cbChooseShowWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChooseShowWay.SelectedIndex == 0)
            {
                filterShowInShortcomming = true;
            }
            else if (cbChooseShowWay.SelectedIndex == 1)
            {
                filterShowInShortcomming = false; // يعرض المخفي
            }
            else
            {
                filterShowInShortcomming = null; // يعرض الكل
            }
            ResetData();
            await GetData(selectedCategoryName); // 🔥 استخدم await مباشر
        }

        private void frmShortcomings_SizeChanged(object sender, EventArgs e)
        {
            // توسيط العناصر أفقياً
            int formWidth = this.ClientSize.Width;

            int searchBoxWidth = txtSearch1.Width;
            int headerWidth = lblHeader.Width;

            int searchX = (formWidth - searchBoxWidth) / 2;
            int headerX = (formWidth - headerWidth) / 2;

            txtSearch1.Location = new Point(searchX, 55);
            lblHeader.Location = new Point(headerX, 11);
        }
    }
}
