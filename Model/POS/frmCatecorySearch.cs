using pos.GeneralForms.MainForm;
using pos.Model.Finance;
using pos.Model.Stor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace pos.Model.POS
{
    public partial class frmCatecorySearch : Form
    {
        private System.Windows.Forms.Timer inputTimer = new System.Windows.Forms.Timer();
        int pageSize = 20;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;
        private bool allLoaded = false;

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        // استدعاء API من user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        private frmCategoryCard frmCategoryCard;
        public frmCatecorySearch(frmCategoryCard frm)
        {
            InitializeComponent();
            this.Padding = new Padding(3); // مسافة عشان نسيب مكان للإطار
            this.ShowInTaskbar = false;
            // إعداد المؤقت مرة واحدة
            inputTimer.Interval = 500; // نص ثانية = 500 ملي ثانية
            inputTimer.Tick += InputTimer_Tick;

            frmCategoryCard = frm;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80; // WS_EX_TOOLWINDOW
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
        private async void frmCatecorySearch_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvProducts); // ✅ تطبيق التنسيق
            await search();
        }
        private async Task search(bool isNewSearch = false)
        {
            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {
                string qry = @"
        SELECT 
            c.catID, 
            c.catName
        FROM category c
        WHERE c.catName LIKE '%' + @catName + '%'
        ORDER BY c.catName ASC
        OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                int offset = currentPage * pageSize;

                SqlParameter[] parameters = {
            new SqlParameter("@catName", txtSearch.Text),
            new SqlParameter("@offset", offset),
            new SqlParameter("@limit", pageSize)
        };

                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                if (isNewSearch)
                {
                    dgvProducts.Rows.Clear();
                    currentPage = 0;
                    hasMoreData = true;
                }

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                int rowIndex = dgvProducts.Rows.Count + 1;

                foreach (DataRow row in dt.Rows)
                {
                    dgvProducts.Rows.Add(
                        rowIndex++,
                        row["catID"],
                        row["catName"]
                    );
                }

                foreach (DataGridViewColumn column in dgvProducts.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                currentPage++;
                dgvProducts.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
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
            await search(true); // ✅ نبدأ بحث جديد
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // نتأكد إن الضغط كان على صف فعلي مش على العناوين
            if (e.RowIndex < 0 || e.RowIndex >= dgvProducts.Rows.Count)
                return;

            try
            {
                // نحاول قراءة القيم بشكل آمن
                string name = dgvProducts.Rows[e.RowIndex].Cells["dgvName"].Value?.ToString() ?? string.Empty;
                object idValue = dgvProducts.Rows[e.RowIndex].Cells["dgvCatID"].Value;

                if (idValue == null || !int.TryParse(idValue.ToString(), out int catID))
                {
                    MessageBox.Show("لا يمكن قراءة رقم الفئة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // إرسال البيانات للفورم الهدف
                frmCategoryCard.resultSearch(name, catID);

                // إغلاق الفورم الحالي بعد الاختيار
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء معالجة الحدث:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dgv.AlternatingRowsDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
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

        private void frmCatecorySearch_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private async void dgvProducts_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvProducts.FirstDisplayedScrollingRowIndex + dgvProducts.DisplayedRowCount(false) >= dgvProducts.RowCount)
                {
                    await search(); // ✅ تحميل الصفحة التالية
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
