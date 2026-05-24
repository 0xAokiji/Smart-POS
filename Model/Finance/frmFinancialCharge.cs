using DevExpress.XtraEditors.Controls;
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

namespace pos.Model.Finance
{
    public partial class frmFinancialCharge : Form
    {
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        private int partiesID = 0;
        private string partyType = "عميل"; // أو "مورد" حسب الحاجة
        public frmFinancialCharge()
        {
            InitializeComponent();
            textSuggester();

        }
        private void frmFinancialCharge_Load(object sender, EventArgs e)
        {
            partyType = "عميل"; // تعيين نوع الطرف الافتراضي
            txtName.Focus();
            dtPickerStart.Value = DateTime.Today;
            dtPickerEnd.Value = DateTime.Today;

            dtPickerStart.Format = DateTimePickerFormat.Custom;
            dtPickerStart.CustomFormat = "yyyy-MM-dd";

            dtPickerEnd.Format = DateTimePickerFormat.Custom;
            dtPickerEnd.CustomFormat = "yyyy-MM-dd";
        }

        private void frmFinancialCharge_SizeChanged(object sender, EventArgs e)
        {
            CenterPanelInForm(panel1);
            CenterPanelInForm(panel2);
        }
        private void CenterPanelInForm(Panel panel)
        {
            panel.Left = (this.ClientSize.Width - panel.Width) / 2;
            // لو حابب تـوسّط عموديًا:
            // panel.Top = (this.ClientSize.Height - panel.Height) / 2;
        }
        private async void displayBillsByPartiesName(int searchMode)
        {


            dgvProducts.Width = 1430;
            dgvProducts.Height = panel2.Height - 48;
            dgvProducts.Location = new Point((panel2.Width - dgvProducts.Width) / 2, 0);
            dgvProducts.Rows.Clear();
            try
            {
                string qry;

                if (partyType == "عميل")
                {
                    qry = @"
                        SELECT 
                            cr.[id],
                            cr.[partiesID],
                            cr.[name],
                            cr.[shiftId],
                            cr.[recipt],
                            cr.[change],
                            cr.[date],
                            cr.[time],
                            s.[sName] AS StaffName
                        FROM [chargeResidual] cr
                        INNER JOIN [shifts] sh
                            ON cr.[shiftId] = sh.[ID]
                        INNER JOIN [staff] s
                            ON sh.[staffID] = s.[staffID]
                        WHERE 1=1
                    ";
                }
                else
                {
                    qry = @"
                        SELECT 
                            cr.[id],
                            cr.[partiesID],
                            cr.[name],
                            cr.[shiftId],
                            cr.[recipt],
                            cr.[change],
                            cr.[date],
                            cr.[time],
                            s.[sName] AS StaffName
                        FROM [chargeResidualSuplieser] cr
                        INNER JOIN [shifts] sh
                            ON cr.[shiftId] = sh.[ID]
                        INNER JOIN [staff] s
                            ON sh.[staffID] = s.[staffID]
                        WHERE 1=1
                    ";
                }


                List<SqlParameter> parameters = new List<SqlParameter>();

                if (searchMode == 1 && partiesID != 0)
                {
                    qry += " AND cr.[partiesID] = @partiesID";
                    parameters.Add(new SqlParameter("@partiesID", partiesID));
                }
                else if (searchMode == 2)
                {
                    qry += " AND cr.[partiesID] = @partiesID";
                    qry += " AND cr.[date] BETWEEN @startDate AND @endDate";
                    parameters.Add(new SqlParameter("@partiesID", partiesID));
                    parameters.Add(new SqlParameter("@startDate", dtPickerStart.Value.Date));
                    parameters.Add(new SqlParameter("@endDate", dtPickerEnd.Value.Date.AddDays(1).AddSeconds(-1)));
                }
                else
                    return;
                qry += " ORDER BY cr.[date]";


                DataTable dt = await LoadDataAsync(qry, parameters.ToArray());

                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    dgvProducts.Rows.Add(
                        rowIndex++,
                        row["id"],
                        row["name"],
                        row["StaffName"],
                        row["recipt"],
                        row["change"],
                        Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd"),
                        row["time"]
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message);
            }
        }
        private async Task<DataTable> LoadDataAsync(string qry, SqlParameter[] parameters)
        {
            using (SqlConnection con = MainClass.GetConnection()) // ✅ الاتصال المؤقت والآمن
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    await Task.Run(() =>
                    {
                        con.Open(); // ✅ فتح الاتصال داخل الـ Task
                        da.Fill(dt);
                    });

                    return dt;
                }
            }
        }

        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ اتصال آمن ومستقل
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartyType", "%" + partyType + "%");

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    con.Open(); // ✅ فتح الاتصال داخل using لضمان الأمان
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


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtName.Text))
            {
                partiesID = nameToID[txtName.Text];
            }
            else
            {
                partiesID = 0;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            displayBillsByPartiesName(1);
        }

        private void btnSearchByNameAndDate_Click(object sender, EventArgs e)
        {
            displayBillsByPartiesName(2);
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                displayBillsByPartiesName(1);
                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private void rbCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCustomer.Checked)
            {
                rbSupliser.Checked = false;
                partyType = "عميل";

                textSuggester();
            }
        }

        private void rbSupliser_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSupliser.Checked)
            {
                rbCustomer.Checked = false;
                partyType = "مورد";
                textSuggester();
            }
        }

        private async void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProducts.Columns["dgvPrint"].Index && e.RowIndex >= 0)
            {
                var row = dgvProducts.Rows[e.RowIndex];

                // Parse ID
                if (row.Cells["dgvID"].Value != null)
                    partiesID = Convert.ToInt32(row.Cells["dgvID"].Value);
                // Balances
                double newBalance = Convert.ToDouble(row.Cells["dgvQty"].Value ?? 0);
                double amountPaid = Convert.ToDouble(row.Cells["dgvUnit"].Value ?? 0);
                double prevBalance = newBalance + amountPaid;

                // Strings
                string delivery = row.Cells["dgvName"].Value?.ToString();
                string parties = row.Cells["dgvCategory"].Value?.ToString();
                string time = row.Cells["dgvTime"].Value?.ToString();
                string date = row.Cells["dgvDate"].Value?.ToString();
                // Call printer
                await MainClass.BillStatmentPrintAsync(0, amountPaid, prevBalance, newBalance, partiesID, "", delivery, parties, 0, date, time);

            }
        }
    }
}
