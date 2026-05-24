using DevExpress.XtraMap.ItemEditor;
using DevExpress.XtraRichEdit.Model;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.View
{
    public partial class frmpurchaseView : SampleView
    {

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color textColor3;
        private Color checkedFillColor;
        private Color checkedFillColor2;
        private Color checkedForeColor;

        public frmpurchaseView()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.InputLanguageChanged += new InputLanguageChangedEventHandler(MyForm_InputLanguageChanged);

            string qry = @"SELECT pname FROM Addpurchases";

            using (SqlConnection con = MainClass.GetConnection())
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

                this.txtSearch2.AutoCompleteCustomSource = dataSource;
                this.txtSearch2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtSearch2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.txtSearch2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            }

            //ThemeMode();
        }


        private void MyForm_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {

            if (InputLanguage.CurrentInputLanguage.Culture.TwoLetterISOLanguageName == "ar")
            {
                txtSearch2.RightToLeft = RightToLeft.No;
            }
            else
            {
                txtSearch2.RightToLeft = RightToLeft.Yes;
            }
        }



        public override void btnAdd_Click(object sender, EventArgs e)
        {

        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
        }


        private async void frmpurchaseView_Load(object sender, EventArgs e)
        {
            btnAdd.Checked = false;
            AddPruch.Checked = false;
            ApplyGridStyle(dgvPrucchase);

            int x = this.Size.Width;
            int x2 = txtSearch2.Size.Width;
            int z = (x - x2) / 2;
            txtSearch2.Location = new Point(z, 15);

            mainPanel.Controls.Clear();

            txtSearch2.Visible = true;
            dgvPrucchase.Visible = true;
            dgvPrucchase.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(dgvPrucchase);

            await GetPruchaseData();
        }

        private void typePruch_Click(object sender, EventArgs e)
        {
            //btnAdd.Checked = false;
            //AddPruch.Checked = false;


            //frmBlackout frmBlackout = new frmBlackout(this);
            //frmBlackout.Show();
            //frmBlackout.Owner = this;
            //frmpurchase frmpurchase = new frmpurchase();
            //frmpurchase.Owner = this;
            //frmpurchase.ShowDialog();
            //frmBlackout.Close();


        }

        private void showPruch_Click(object sender, EventArgs e)
        {
            btnAdd.Checked = false;
            AddPruch.Checked = false;


            mainPanel.Controls.Clear();

            txtSearch2.Visible = true;
            dgvPrucchase.Visible = false;
        }

        private async void AddPruch_Click(object sender, EventArgs e)
        {
            if (!MainClass.AddExpenses)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            // الحصول على الفورم الرئيسية من أي عنصر داخلها (مثل زر داخل UserControl)
            Form parentForm = this.FindForm();

            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = parentForm;

            frmWithdraw frm = new frmWithdraw();
            frm.ShowDialog(parentForm);

            frmBlackout.Close();

            // إعادة تركيز الفورم الرئيسية بعد الإغلاق
            parentForm.Activate();


            AddPruch.Checked = false;

            currentPage = 0;
            hasMoreData = true;
            await GetPruchaseData(); // ✅ تحميل الصفحة التالية

        }

        private async void showpurRep_Click(object sender, EventArgs e)
        {
            btnAdd.Checked = false;
            AddPruch.Checked = false;


            mainPanel.Controls.Clear();

            txtSearch2.Visible = true;
            dgvPrucchase.Visible = true;
            dgvPrucchase.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(dgvPrucchase);

            currentPage = 0;
            hasMoreData = true;
            await GetPruchaseData(); // ✅ تحميل الصفحة التالية
        }

        int pageSize = 35;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;
        private bool allLoaded = false;

        private async Task GetPruchaseData()
        {
            try
            {
                if (isLoading || !hasMoreData)
                    return;

                isLoading = true;

                // ✅ لو أول صفحة، نمسح القديم
                if (currentPage == 0)
                    dgvPrucchase.Rows.Clear();

                string qry = @"
                SELECT 
                  p.pid,
                  p.name,
                  s.sName,
                  p.pname,
                  p.price,
                  p.amount,
                  p.aTime,
                  p.aDate
                FROM 
                    purchases p
                       INNER JOIN 
                          shifts sh ON p.shiftID = sh.ID
                       INNER JOIN 
                          staff s ON sh.staffID = s.staffID
                WHERE 
                   p.pname LIKE N'%' + @search + '%'
                ORDER BY p.name ASC
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                int offset = currentPage * pageSize;

                SqlParameter[] parameters =
                {
                    new SqlParameter("@search", txtSearch2.Text.Trim()),
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize)
                };

                // ✅ تحميل البيانات في Thread منفصل
                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                int rowIndex = dgvPrucchase.Rows.Count + 1; // ✅ يبدأ من آخر رقم

                foreach (DataRow row in dt.Rows)
                {
                    dgvPrucchase.Rows.Add(
                        rowIndex++,
                        row["pid"],
                        row["name"],
                        row["sName"],
                        row["pname"],
                        row["price"],
                        row["amount"],
                        row["aTime"],
                        Convert.ToDateTime(row["aDate"]).ToString("dd-MM-yyyy")
                    );
                }

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
        public static DataTable LoadDataReturn(string qry, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = MainClass.GetConnection())
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

        private void dgvPrucchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPrucchase.Columns[e.ColumnIndex].Name == "dgvDel2")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;

                if (guna2MessageDialog1.Show("هل تريد حذف هذا الصنف؟") == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvPrucchase.Rows[e.RowIndex].Cells["dgvID2"].Value);
                    string qry = "DELETE FROM purchases WHERE pid = " + id;
                    Hashtable ht = new Hashtable();
                    MainClass.SQL(qry, ht);

                    dgvPrucchase.Rows.RemoveAt(e.RowIndex);

                    //frmNotify frmNotify = new frmNotify();
                    //frmNotify.showAlert("تم الحذف بنجاح");
                }
            }
        }
        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            textColor2 = MainClass.TextColor2;
            textColor3 = MainClass.TextColor3;
            checkedFillColor = MainClass.CheckedFillColor;
            checkedFillColor2 = MainClass.CheckedFillColor2;
            checkedForeColor = MainClass.CheckedForeColor;
        }
        public void ThemeMode()
        {
            ThemeColor();

            this.BackColor = backgroundPrimary;


            //Panels
            topPanel.BackColor = checkedFillColor;
            mainPanel.BackColor = backgroundPrimary;


            //Text box
            txtSearch2.BackColor = backgroundPrimary;
            txtSearch2.ForeColor = textColor2;
            txtSearch2.BorderColor = checkedFillColor;
            txtSearch2.FillColor = backgroundPrimary;

            //-> datagride view 
            dgvPrucchase.BackgroundColor = backgroundPrimary;
            dgvPrucchase.GridColor = backgroundPrimary;

            dgvPrucchase.DefaultCellStyle.BackColor = backgroundPrimary;
            dgvPrucchase.DefaultCellStyle.ForeColor = textColor;
            dgvPrucchase.DefaultCellStyle.SelectionBackColor = checkedFillColor;
            dgvPrucchase.DefaultCellStyle.SelectionForeColor = textColor;

            dgvPrucchase.ColumnHeadersDefaultCellStyle.BackColor = backgroundSecondary;
            dgvPrucchase.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            dgvPrucchase.ColumnHeadersDefaultCellStyle.SelectionBackColor = checkedFillColor;

            dgvPrucchase.RowsDefaultCellStyle.BackColor = backgroundPrimary;
            dgvPrucchase.AlternatingRowsDefaultCellStyle.BackColor = backgroundPrimary;
            dgvPrucchase.RowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
            dgvPrucchase.RowsDefaultCellStyle.ForeColor = textColor;
            dgvPrucchase.RowsDefaultCellStyle.SelectionForeColor = textColor;

            dgvPrucchase.CellBorderStyle = DataGridViewCellBorderStyle.Single;


            // Buttons
            AddPruch.FillColor = checkedFillColor2;
            AddPruch.ForeColor = textColor3;



        }



        private void dgvPrucchase_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvPrucchase.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvPrucchase.CurrentCell.ColumnIndex)
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

        private void frmpurchaseView_SizeChanged(object sender, EventArgs e)
        {
            int formWidth = this.ClientSize.Width;

            int searchBoxWidth = txtSearch2.Width;

            int searchX = (formWidth - searchBoxWidth) / 2;

            txtSearch2.Location = new Point(searchX, 46);
        }

        private async void txtSearch2_TextChanged(object sender, EventArgs e)
        {
            currentPage = 0;
            hasMoreData = true;
            await GetPruchaseData(); // ✅ تحميل الصفحة التالية
        }

        private async void dgvPrucchase_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvPrucchase.FirstDisplayedScrollingRowIndex + dgvPrucchase.DisplayedRowCount(false) >= dgvPrucchase.RowCount)
                {
                    await GetPruchaseData(); // ✅ تحميل الصفحة التالية
                }
            }
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
