using pos.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Model.Stor
{
    public partial class frmShowStors : Form
    {
        public frmShowStors()
        {
            InitializeComponent();
        }

        private async void frmShowStors_Load(object sender, EventArgs e)
        {
            await GetData(); // ✅ Async

        }
        int pageSize = 35;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;
        private bool allLoaded = false;

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

                // ✅ تجهيز النص مع Wildcards
                string searchText = txtSearch.Text.Trim();
                string searchParam = "%" + searchText + "%";

                // ✅ الاستعلام
                string qry = @"
                SELECT * FROM addStore 
                WHERE storeName LIKE @SearchText
                ORDER BY storeName ASC
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";
                int offset = currentPage * pageSize;

                SqlParameter[] parameters =
                {
                    new SqlParameter("@SearchText", searchParam),
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize)
                };

                // ✅ تحميل البيانات في Thread منفصل
                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));
                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;
                int rowIndex = dgvCategory.Rows.Count + 1; // ✅ يكمل الترقيم

                foreach (DataRow row in dt.Rows)
                {
                    dgvCategory.Rows.Add(
                        rowIndex++,
                        row["storeID"],
                        row["storeName"],
                        row["storeNumber"]
                    );
                }
                currentPage++;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Notifier.ShowNotification("حدث خطأ", message);
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

        private System.Windows.Forms.Timer searchTimer;
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                txtSearch.TextAlign = HorizontalAlignment.Left;
            }
            else
            {
                char firstChar = searchText[0];
                txtSearch.TextAlign = IsArabic(firstChar) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }

            // أوقف المؤقت القديم لو شغال
            if (searchTimer != null)
            {
                searchTimer.Stop();
                searchTimer.Dispose();
            }

            searchTimer = new System.Windows.Forms.Timer
            {
                Interval = 500 // نصف ثانية
            };

            searchTimer.Tick += async (s, args) =>
            {
                searchTimer.Stop();
                searchTimer.Dispose();

                currentPage = 0;
                hasMoreData = true;
                await GetData(); // 🔥 استدعاء Async
            };

            searchTimer.Start();
        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }

        private async void dgvCategory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCategory.CurrentCell.OwningColumn.Name == "dgvName" || dgvCategory.CurrentCell.OwningColumn.Name == "dgvNum")
            {
                //frmBlackout frmBlackout1 = new frmBlackout(this);
                //frmBlackout1.StartPosition = FormStartPosition.Manual;
                //frmBlackout1.Location = this.Location;
                //frmBlackout1.Size = this.Size;
                //frmBlackout1.Show(this);

                frmAddStore frm = new frmAddStore();
                frm.id = Convert.ToInt32(dgvCategory.CurrentRow.Cells["dgvId"].Value);
                frm.txtName.Text = Convert.ToString(dgvCategory.CurrentRow.Cells["dgvName"].Value);
                frm.txtCode.Text = Convert.ToString(dgvCategory.CurrentRow.Cells["dgvNum"].Value);

                frm.ShowDialog(this);

                //frmBlackout1.Close();

                await GetData();


            }
            else if (dgvCategory.CurrentCell.OwningColumn.Name == "dgvDelete")
            {
                int id = Convert.ToInt32(dgvCategory.CurrentRow.Cells["dgvId"].Value);


                // لو مسدد، نعرض رسالة تأكيد الحذف
                DialogResult confirm = MessageBox.Show(
                    "هل أنت متأكد أنك تريد حذف هذا المخزن؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    string deleteQry = @"DELETE FROM addStore WHERE storeID = @ID;";

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
                    currentPage = 0;
                    hasMoreData = true;
                    await GetData(); // 🔥 استدعاء Async
                    Notifier.ShowNotification("تم الحذف", $"✅ تم حذف المخزن من السجل بنجاح");
                }
            }
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            int panelSize = panel1.Size.Width;
            int txtSearchSize = txtSearch.Size.Width;
            int z = (panelSize - txtSearchSize) / 2;
            txtSearch.Location = new Point(z, 5);
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
    }
}
