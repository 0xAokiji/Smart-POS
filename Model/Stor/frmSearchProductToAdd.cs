using pos.Classes;
using pos.GeneralForms.MainForm;
using pos.Model.Finance;
using pos.Model.POS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace pos.Model.Stor
{
    public partial class frmSearchProductToAdd : Form
    {
        private int currentPage;
        int pageSize = 30;
        bool hasMoreData = true;

        // 🔥 متغيرات Global لتخزين الفلاتر
        private string currentSearchValue = null;
        private bool isLoading;
        private string currentCategoryValue = null;
        private frmProductAdd2 frmProductAdd2;


        // تحرك الفروم من خلال سحب بنال
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        // استدعاء API من user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public frmSearchProductToAdd(frmProductAdd2 frmProductAdd2)
        {
            InitializeComponent();
            this.frmProductAdd2 = frmProductAdd2;
            textSuggester();
            this.ShowInTaskbar = false;

        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;       // WS_EX_TOOLWINDOW - لجعل الفورم لا يظهر في شريط المهام
                return cp;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // نرسم الإطار
            int borderWidth = 1; // سمك الإطار
            Color borderColor = Color.FromArgb(1, 95, 95); // لون الإطار    

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid,
                borderColor, borderWidth, ButtonBorderStyle.Solid);
        }

        private async void frmSearchProductToAdd_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvProducts); // ✅ تطبيق التنسيق
            await GetData(); // 🔥 جلب البيانات

            DataTable categories = new DataTable();
            string qry = "SELECT catID, catName FROM category";

            using (SqlConnection conn = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(categories);
            }

            // 🟢 إضافة صف جديد في الأول (ID = 0 مثلًا)
            DataRow newRow = categories.NewRow();
            newRow["catID"] = 0;
            newRow["catName"] = "الكل";
            categories.Rows.InsertAt(newRow, 0);

            // ✅ ربط البيانات بالكومبو بوكس
            cbCategory.DataSource = categories;
            cbCategory.DisplayMember = "catName";
            cbCategory.ValueMember = "catID";

            cbCategory.SelectedIndex = 0;

        }

        public async Task GetData(string categoryValue = null)
        {
            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {

                if (categoryValue != null)
                    currentCategoryValue = categoryValue;

                // ✅ لو أول صفحة امسح البيانات
                if (currentPage == 0)
                    dgvProducts.Rows.Clear();

                currentSearchValue = string.IsNullOrWhiteSpace(txtSearch.Text)
                                 ? null
                                 : txtSearch.Text.Trim();
                if (cbCategory.SelectedIndex == 0)
                    currentCategoryValue = null;

                string qry = @"
                SELECT 
                    p.pID, 
                    CASE WHEN p.pName IS NULL OR p.pName = '' THEN N'غير محدد' ELSE p.pName END AS pName,
                    CASE WHEN p.pNewBarode IS NULL OR p.pNewBarode = '' THEN N'غير محدد' ELSE p.pNewBarode END AS pNewBarode,
                    CASE WHEN p.pUsedBarode IS NULL OR p.pUsedBarode = '' THEN N'غير محدد' ELSE p.pUsedBarode END AS pUsedBarode
                FROM products p 
                JOIN category c ON p.categoryID = c.catID 
                JOIN totalStor ts ON ts.pID = p.pID 
                WHERE
                (@catName IS NULL OR @catName = '' OR c.catName LIKE '%' + @catName + '%')
                AND
                    (@searchName IS NULL OR @searchName = '' 
                     OR p.pName LIKE '%' + @searchName + '%'
                     OR p.pNewBarode LIKE '%' + @searchName + '%'
                     OR p.pUsedBarode LIKE '%' + @searchName + '%'
                    )
                ORDER BY p.pID
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

                int offset = currentPage * pageSize;

                SqlParameter[] parameters = {
                    new SqlParameter("@catName", (object)currentCategoryValue ?? DBNull.Value),
                    new SqlParameter("@searchName", (object)currentSearchValue ?? DBNull.Value),
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize)
                };

                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                int rowIndex = dgvProducts.Rows.Count + 1;

                foreach (DataRow row in dt.Rows)
                {
                    dgvProducts.Rows.Add(
                        rowIndex++,
                        row["pID"],
                        row["pName"],
                        row["pNewBarode"],
                        row["pUsedBarode"]
                    );
                }

                currentPage++;
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
            }
            finally
            {
                isLoading = false;
            }
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
        private async void dgvProducts_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvProducts.FirstDisplayedScrollingRowIndex + dgvProducts.DisplayedRowCount(false) >= dgvProducts.RowCount)
                {
                    await GetData(cbCategory.Text.ToString()); // 🔥 جلب البيانات
                }
            }
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
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

            // الألوان العادية للصفوف
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);

            // الصفوف المتبادلة (صف غامق وصف فاتح)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.Columns[e.ColumnIndex].Name != "btnUse")
            {
                string value = dgvProducts.Rows[e.RowIndex].Cells["dgvCode"].Value?.ToString();

                frmProductAdd2.resultSearchProduct(value);
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ✅ تأكد إنك مش على الهيدر
            {
                // لو الضغط كان على عمود btnUse
                if (dgvProducts.Columns[e.ColumnIndex].Name == "btnUse")
                {
                    string value = dgvProducts.Rows[e.RowIndex].Cells["dgvCodeUse"].Value?.ToString();

                    frmProductAdd2.resultSearchProduct(value);
                }

            }
        }

        private void frmSearchProductToAdd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private async void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtSearch.Text = string.Empty;
            dgvProducts.Rows.Clear();
            dgvProducts.DataSource = null;
            hasMoreData = true;
            isLoading = false;
            currentPage = 0;
            if (cbCategory.SelectedIndex == 0) // ✅ لو "الكل"
            {
                await GetData(); // جلب كل البيانات
            }
            else
            {
                // ✅ الأفضل تستخدم SelectedValue لو شغال بالـ ID
                await GetData(cbCategory.Text.ToString());
            }
        }
        private System.Windows.Forms.Timer searchTimer;

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // ✅ تحقق إذا التكست فاضي
            string searchText = txtSearch.Text.Trim();

            // 🔄 إعادة ضبط البيانات


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
                dgvProducts.Rows.Clear();
                dgvProducts.DataSource = null;

                hasMoreData = true;
                isLoading = false;
                currentPage = 0;

                if (string.IsNullOrEmpty(searchText))
                {
                    if (cbCategory.SelectedIndex == 0) // ✅ لو "الكل"
                    {
                        await GetData(); // جلب كل البيانات
                    }
                    else
                    {
                        // ✅ الأفضل تستخدم SelectedValue لو شغال بالـ ID
                        await GetData(cbCategory.Text.ToString());
                    }
                }
                else
                    await GetData(); // جلب كل البيانات

            };

            searchTimer.Start();
        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
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

                    txtSearch.AutoCompleteCustomSource = dataSource;
                    txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
        }
    }
}
