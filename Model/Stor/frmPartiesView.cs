using Guna.UI2.WinForms;
using pos.Classes;
using pos.GeneralForms;
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

namespace pos.Model.Stor
{
    public partial class frmPartiesView : Form
    {
        frmMian2 frmMain;
        public frmPartiesView(frmMian2 frm)
        {
            InitializeComponent();
            frmMain = frm;
        }
        public frmPartiesView()
        {
            InitializeComponent();
        }

        private async void frmPartiesView_Load(object sender, EventArgs e)
        {
            await GetData(); // ✅ Async

        }
        int pageSize = 35;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;
        private bool allLoaded = false;
        public string type = "عميل";

        private async Task GetData()
        {
            try
            {
                if (isLoading || !hasMoreData)
                    return;

                isLoading = true;
                // ✅ لو أول صفحة، نمسح القديم
                if (currentPage == 0)
                    dgvCategory.Rows.Clear();

                string searchText = txtSearch.Text.Trim();
                string searchParam = "%" + searchText + "%";

                string qry = @"
                 SELECT 
                    p.*, 
                    ISNULL(r.currentDebitBalance, 0) AS currentDebitBalance
                FROM Parties p
                LEFT JOIN residualTable r ON p.pID = r.PartiesID
                WHERE p.PartyType LIKE @PartyType
                  AND p.pName LIKE '%' + @pName + '%'
                ORDER BY p.pName ASC
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                int offset = currentPage * pageSize;

                SqlParameter[] parameters =
                {
                    new SqlParameter("@pName", searchParam),
                    new SqlParameter("@PartyType", type + "%"),   // ✅ لضمان البحث الجزئي
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize)
                };

                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                int rowIndex = dgvCategory.Rows.Count + 1; // ✅ يبدأ من آخر رقم

                foreach (DataRow row in dt.Rows)
                {
                    dgvCategory.Rows.Add(
                        rowIndex++,
                        row["pID"],
                        row["pName"],
                        row["PartyType"],
                        row["pAdderss"],
                        row["pPhone"],
                        row["pPhone2"],
                        row["supCode"],
                        row["currentDebitBalance"]
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
        private void topPanel_SizeChanged(object sender, EventArgs e)
        {

        }

        private void dgvCategory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCategory.CurrentCell.OwningColumn.Name == "dgvName" || dgvCategory.CurrentCell.OwningColumn.Name == "dgvParties" || dgvCategory.CurrentCell.OwningColumn.Name == "dgvAddress" ||
                dgvCategory.CurrentCell.OwningColumn.Name == "dgvPhone1" || dgvCategory.CurrentCell.OwningColumn.Name == "dgvPhone2" || dgvCategory.CurrentCell.OwningColumn.Name == "dgvCode")
            {
                int pId = Convert.ToInt32(dgvCategory.CurrentRow.Cells["dgvid"].Value);
                string partyName = dgvCategory.CurrentRow.Cells["dgvName"].Value.ToString();

                frmMain.openBalanceFollow(pId, partyName, cbChooseParyties.SelectedIndex == 1);
            }
                if (dgvCategory.CurrentCell.OwningColumn.Name == "dgvEdite")
            {
                //frmBlackout frmBlackout1 = new frmBlackout(this, true);
                //frmBlackout1.Show();


                using (frmAddParties frm = new frmAddParties())
                {

                    frm.Owner = this;
                    frm.pID = Convert.ToInt32(dgvCategory.CurrentRow.Cells["dgvid"].Value);
                    frm.partyType = dgvCategory.CurrentRow.Cells["dgvParties"].Value.ToString();
                    frm.ShowDialog();

                }
                this.Focus();

                //frmBlackout1.Close();

                GetData();


            }
            else if (dgvCategory.CurrentCell.OwningColumn.Name == "dgvDelete")
            {
                int id = Convert.ToInt32(dgvCategory.CurrentRow.Cells["dgvid"].Value);
                string status = "";

                // جلب الحالة من قاعدة البيانات
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string checkQry = @"SELECT status FROM residualTable WHERE PartiesID = @ID";

                    using (SqlCommand cmd = new SqlCommand(checkQry, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        con.Close();

                        if (result != null)
                            status = result.ToString();
                    }
                }

                // التحقق من الحالة
                if (status == "مدين" || status == "دائن")
                {
                    MessageBox.Show(
                        $"لا يمكن حذف هذا الشخص لأنه {status}.",
                        "عملية مرفوضة",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // لو مسدد، نعرض رسالة تأكيد الحذف
                DialogResult confirm = MessageBox.Show(
                    "هل أنت متأكد أنك تريد حذف هذا الشخص من السجل؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    string deleteQry = @"DELETE FROM Parties WHERE pID = @ID;";

                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand(deleteQry, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    Notifier.ShowNotification("تم الحذف", $"✅ تم حذف شخص من السجل بنجاح");
                }
            }

        }

        private void frmPartiesView_SizeChanged(object sender, EventArgs e)
        {
            int panelWidth = topPanel.Width;

            // المسافات
            int spaceBetween = 20;

            // إجمالي عرض العنصرين + المسافة بينهم
            int totalWidth = cbChooseParyties.Width + spaceBetween + txtSearch.Width;

            // نحسب بداية المجموعة (X) بحيث تكون في النص
            int startX = (panelWidth - totalWidth) / 2;

            // نحدد أماكنهم
            cbChooseParyties.Location = new Point(startX, 5);
            txtSearch.Location = new Point(startX + cbChooseParyties.Width + spaceBetween, 5);


        }

        private async void dgvCategory_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvCategory.FirstDisplayedScrollingRowIndex + dgvCategory.DisplayedRowCount(false) >= dgvCategory.RowCount)
                {
                    await GetData(); // ✅ تحميل الصفحة التالية
                }
            }
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            currentPage = 0;
            hasMoreData = true;
            await GetData(); // ✅ تحميل الصفحة التالية

        }

        private void dgvCategory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "dgvBalance")
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

        private async void cbChooseParyties_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = cbChooseParyties.Text;
            currentPage = 0;
            hasMoreData = true;
            await GetData(); // ✅ Async

        }
    }
}
