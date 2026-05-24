using DevExpress.XtraScheduler.iCalendar.Components;
using DevExpress.XtraSpreadsheet.Import.Xls;
using Guna.UI2.WinForms;
using pos.Model;
using pos.SystemApp;
using pos.View;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Settings
{
    public partial class frmStaff : Form
    {
        //-> Dark Mode
        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor3;
        private Color checkedFillColor;
        private Color checkedForeColor;

        // قائمة لحفظ قيم عمود dgvid للصفوف المحددة
        private List<int> selectedRowIds = new List<int>();

        frmAppSetting mainForm;

        public frmStaff(frmAppSetting frm)
        {
            InitializeComponent();
            mainForm = frm;

            ThemeMode();
        }

        int currentPage = 0;     // الصفحة الحالية
        int pageSize = 20;       // عدد السجلات في كل صفحة
        bool hasMoreData = true; // للتحكم لو في صفحات تانية

        public async Task GetDataAsync(bool reset = false)
        {
            if (!MainClass.StaffDelete)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            try
            {
                if (reset) // ✅ لو بداية جديدة (بحث جديد مثلاً)
                {
                    currentPage = 0;
                    hasMoreData = true;
                    dgvStaff.Rows.Clear();
                }

                if (!hasMoreData) return; // ✅ مفيش صفحات تانية

                int offset = currentPage * pageSize;

                string qry = @"
            SELECT 
                s.staffID,
                s.sName,
                s.sPhone,
                s.sRole,
                CAST(s.sSalary AS int) AS sSalary,
                CAST(ISNULL(a.TotalAdvancesThisMonth, 0) AS int) AS TotalAdvancesThisMonth,
                CASE 
                    WHEN sal.IsPaid = 1 THEN N'مدفوع'
                    ELSE N'غير مدفوع'
                END AS حالة_المرتب
            FROM staff s
            LEFT JOIN (
                SELECT staffID, SUM(Amount) AS TotalAdvancesThisMonth
                FROM Advances
                WHERE MONTH(AdvanceDate) = MONTH(GETDATE())
                  AND YEAR(AdvanceDate) = YEAR(GETDATE())
                GROUP BY staffID
            ) a ON s.staffID = a.staffID
            LEFT JOIN Salaries sal 
                ON s.staffID = sal.staffID
               AND sal.SalaryMonth = MONTH(GETDATE())
               AND sal.SalaryYear = YEAR(GETDATE())
            WHERE s.sName LIKE N'%' + @search + N'%'
            ORDER BY s.staffID
            OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                DataTable dt = await Task.Run(() =>
                {
                    DataTable table = new DataTable();
                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@search", txtSearch.Text.Trim());
                        cmd.Parameters.AddWithValue("@offset", offset);
                        cmd.Parameters.AddWithValue("@limit", pageSize);

                        con.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    return table;
                });
                int rowIndex = dgvStaff.Rows.Count + 1;

                // ✅ إضافة بيانات للـ DataGridView بدون مسح القديم
                foreach (DataRow row in dt.Rows)
                {
                    dgvStaff.Rows.Add(
                        rowIndex++,
                        row["staffID"],
                        row["sName"],
                        row["sPhone"],
                        row["sRole"],
                        row["sSalary"],
                        row["TotalAdvancesThisMonth"],
                        row["حالة_المرتب"]
                    );
                }

                // ✅ لو عدد السجلات أقل من حجم الصفحة → مفيش بيانات تانية
                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                // ✅ زود رقم الصفحة
                currentPage++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ: " + ex.Message);
                return;
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
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // ✅ ألوان الهيدر (عادي + خط)
            dgv.EnableHeadersVisualStyles = false; // مهم عشان ألوانك تشتغل
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;                 // لون خط الهيدر

            // لون الهيدر وقت التحديد (لو حابب تخليه مختلف)
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {

                string columnName = dgvStaff.Columns[e.ColumnIndex].Name;

                if (dgvStaff.Columns["dgvSelect"].Visible)
                {
                    var value = dgvStaff.Rows[e.RowIndex].Cells["dgvSelect"].Value;

                    bool isChecked = value != null && (bool)value;
                    value = !isChecked;

                    dgvStaff.Rows[e.RowIndex].Cells["dgvSelect"].Value = value;
                }

                // تأكيد التحديث
                dgvStaff.EndEdit();
                checkdgvstate();


            }

        }

        private void btnEditing_Click(object sender, EventArgs e)
        {
            //if (MainClass.StaffAdd)
            //{
            //    frmStaffView mainForm = this.FindForm() as frmStaffView;

            //    frmBlackout frmBlackout = new frmBlackout();
            //    frmBlackout.Show(mainForm);
            //    frmStaffAdd frm = new frmStaffAdd();
            //    frm.ShowDialog(frmBlackout);
            //    frmBlackout.Close();
            //    GetData();
            //}
            //else
            //{
            //    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            //    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            //    guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
            //    guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
            //}
        }

        private void guna2DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //checkdgvstate();
            if (e.RowIndex >= 0 && !dgvStaff.Columns["dgvSelect"].Visible)
            {
                dgvStaff.Columns["dgvSelect"].Visible = true;
                dgvStaff.Rows[e.RowIndex].Cells["dgvSelect"].Value = true;
            }
        }

        private async void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            await GetDataAsync(true); // يبدأ من أول صفحة

        }

        private void btnAddStaff_Click(object sender, EventArgs e)
        {
            if (MainClass.StaffAdd)
            {
                mainForm.addPersone(0, false);

                selectedRowIds.Clear();
                checkdgvstate();
            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
            }
        }

        private async void frmStaff_Load(object sender, EventArgs e)
        {
            await GetDataAsync(true); // يبدأ من أول صفحة
            dgvStaff.ReadOnly = false;

            foreach (DataGridViewColumn col in dgvStaff.Columns)
            {
                if (col.Name != "dgvSelect")
                    col.ReadOnly = true;
            }
            ApplyGridStyle(dgvStaff);
        }

        private void dgvStaff_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStaff.Columns.Contains("dgvSelect"))
            {
                if (e.ColumnIndex == dgvStaff.Columns["dgvSelect"].Index)
                {
                    int rowId = Convert.ToInt32(dgvStaff.Rows[e.RowIndex].Cells["dgvid"].Value);

                    bool isChecked = Convert.ToBoolean(dgvStaff.Rows[e.RowIndex].Cells["dgvSelect"].Value);

                    if (isChecked)
                    {
                        if (!selectedRowIds.Contains(rowId))
                        {
                            selectedRowIds.Add(rowId);
                        }
                    }
                    else
                    {
                        if (selectedRowIds.Contains(rowId))
                        {
                            selectedRowIds.Remove(rowId);
                        }
                    }

                    checkdgvstate();
                }
            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (!MainClass.StaffEdite)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

                return;
            }
            int staffID = 0;
            if (selectedRowIds.Count > 0)
            {
                staffID = selectedRowIds[0];

                selectedRowIds.Clear();
                checkdgvstate();
            }

            mainForm.addPersone(staffID, true);

        }

        private void dgvStaff_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvStaff.IsCurrentCellDirty && dgvStaff.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvStaff.CommitEdit(DataGridViewDataErrorContexts.Commit); // نعتمد القيمة الجديدة
            }
        }
        void checkdgvstate()
        {
            if (selectedRowIds.Count == 0)
            {
                btnEdite.Enabled = false;
                btnSalary.Enabled = false;
                btnDelete.Enabled = false;
                if (dgvStaff.Columns.Contains("dgvSelect"))
                    dgvStaff.Columns["dgvSelect"].Visible = false;

            }
            else if (selectedRowIds.Count == 1)
            {
                btnEdite.Enabled = true;
                btnSalary.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdite.Enabled = false;
                btnSalary.Enabled = false;
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (true)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                // تعيين الـ Parent للنموذج الرئيسي
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;

                if (guna2MessageDialog1.Show(" هل تريد حذف هذا الصنف ") == DialogResult.Yes)
                {
                    if (selectedRowIds.Count > 0)
                    {
                        // 1. أولاً، نرسل استعلام الحذف لقاعدة البيانات
                        List<string> paramNames = new List<string>();
                        Hashtable ht = new Hashtable();

                        // إضافة القيم لـ selectedRowIds إلى استعلام الحذف
                        for (int i = 0; i < selectedRowIds.Count; i++)
                        {
                            string paramName = "@id" + i;
                            paramNames.Add(paramName);
                            ht.Add(paramName, selectedRowIds[i]);
                        }

                        string qryDeleteStaff = $"DELETE FROM staff WHERE staffID IN ({string.Join(",", paramNames)})";
                        MainClass.SQL(qryDeleteStaff, ht);

                        string qryDeleteUsers = $"DELETE FROM users WHERE staffID IN ({string.Join(",", paramNames)})";
                        MainClass.SQL(qryDeleteUsers, ht);

                        for (int i = dgvStaff.Rows.Count - 1; i >= 0; i--)
                        {
                            DataGridViewRow row = dgvStaff.Rows[i];
                            if (!row.IsNewRow)
                            {
                                int rowId = Convert.ToInt32(row.Cells["dgvid"].Value);

                                if (selectedRowIds.Contains(rowId))
                                {
                                    dgvStaff.Rows.RemoveAt(i);
                                }
                            }
                        }

                        selectedRowIds.Clear();
                        checkdgvstate();

                        //int num = numberStaffRow();
                        //if (num >= 15)
                        //    ClipControlRegion(this, "left", 17);
                        //else
                        //    ClipControlRegion(this, "left", 10);
                    }
                }
                else
                {
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                    guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                }
            }
        }

        private async void btnReload_Click(object sender, EventArgs e)
        {
            await GetDataAsync(true); // يبدأ من أول صفحة
            selectedRowIds.Clear();
            checkdgvstate();

        }

        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            textColor3 = MainClass.TextColor3;
            checkedFillColor = MainClass.CheckedFillColor;
            checkedForeColor = MainClass.CheckedForeColor;
        }
        public void ThemeMode()
        {
            ThemeColor();
            if (MainClass.ThemeMode == "light")
                dgvStaff.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 230);
            if (MainClass.ThemeMode == "dark")
            {
                btnReload.Image = Properties.Resources.refresh_white_dark;
                btnAddStaff.Image = Properties.Resources.add_dark3;
                btnEdite.Image = Properties.Resources.edit_dark2;
                txtSearch.IconRight = Properties.Resources.searching_dark;

            }

            btnReload.Image = Properties.Resources.refresh_white_dark;
            btnAddStaff.Image = Properties.Resources.add_dark3;
            btnEdite.Image = Properties.Resources.edit_dark2;
            txtSearch.IconRight = Properties.Resources.searching_dark;

            this.BackColor = backgroundPrimary;
            mainPanel.BackColor = backgroundPrimary;

            //-> button
            btnReload.BackColor = backgroundPrimary;
            btnReload.FillColor = checkedFillColor;
            btnReload.ForeColor = textColor;

            btnAddStaff.BackColor = backgroundPrimary;
            btnAddStaff.FillColor = checkedFillColor;
            btnAddStaff.ForeColor = textColor;

            btnEdite.BackColor = backgroundPrimary;
            btnEdite.FillColor = checkedFillColor;
            btnEdite.ForeColor = textColor;

            btnDelete.BackColor = backgroundPrimary;
            btnDelete.FillColor = checkedFillColor;
            btnDelete.ForeColor = textColor;

            //-> text box
            txtSearch.ForeColor = backgroundPrimary;
            txtSearch.ForeColor = textColor;
            txtSearch.BorderColor = checkedFillColor;
            txtSearch.FillColor = backgroundPrimary;

            //-> datagride view 
            dgvStaff.BackgroundColor = backgroundPrimary;
            dgvStaff.GridColor = backgroundPrimary;
            dgvStaff.AlternatingRowsDefaultCellStyle.BackColor = backgroundPrimary;
            dgvStaff.AlternatingRowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
            dgvStaff.AlternatingRowsDefaultCellStyle.ForeColor = textColor;
            dgvStaff.AlternatingRowsDefaultCellStyle.SelectionForeColor = textColor3;

            dgvStaff.ColumnHeadersDefaultCellStyle.BackColor = backgroundSecondary;
            dgvStaff.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            dgvStaff.ColumnHeadersDefaultCellStyle.SelectionBackColor = backgroundSecondary;

            dgvStaff.RowsDefaultCellStyle.BackColor = backgroundPrimary;
            dgvStaff.RowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
            dgvStaff.RowsDefaultCellStyle.ForeColor = textColor;
            dgvStaff.RowsDefaultCellStyle.SelectionForeColor = textColor3;
        }

        //private void ClipControlRegion(Control control, string direction, int cutSize)
        //{
        //    Rectangle rect = control.ClientRectangle;

        //    switch (direction.ToLower())
        //    {
        //        case "top":
        //            rect.Y += cutSize;
        //            rect.Height -= cutSize;
        //            break;
        //        case "bottom":
        //            rect.Height -= cutSize;
        //            break;
        //        case "left":
        //            rect.X += cutSize;
        //            rect.Width -= cutSize;
        //            break;
        //        case "right":
        //            rect.Width -= cutSize;
        //            break;
        //        default:
        //            throw new ArgumentException("Direction must be: top, bottom, left, or right.");
        //    }

        //    GraphicsPath path = new GraphicsPath();
        //    path.AddRectangle(rect);
        //    control.Region = new Region(path);
        //}

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
            //int num = numberStaffRow();
            //if (num >= 15)
            //    ClipControlRegion(this, "left", 17);
            //else
            //    ClipControlRegion(this, "left", 10);

        }
        private int numberStaffRow()
        {
            string query = "SELECT COUNT(*) FROM staff";
            int rowCount = 0;

            using (SqlConnection conn = MainClass.GetConnection()) // ✅ استخدم دالة الاتصال
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    rowCount = Convert.ToInt32(result);
                }
            }

            return rowCount;
        }

        private void brnSalary_Click(object sender, EventArgs e)
        {
            if (!MainClass.Salaries)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            int staffID = 0;
            if (selectedRowIds.Count > 0)
            {
                staffID = selectedRowIds[0];

                frmRequestAdvance frm = new frmRequestAdvance();
                frm.staffID = staffID;
                frm.ShowDialog();
            }

        }

        private async void dgvStaff_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvStaff.FirstDisplayedScrollingRowIndex + dgvStaff.DisplayedRowCount(false) >= dgvStaff.RowCount)
                {
                    await GetDataAsync(); // يجيب الصفحة اللي بعدها
                }
            }
        }

        private void dgvStaff_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvStaff.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvStaff.CurrentCell.ColumnIndex)
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
    }
}
