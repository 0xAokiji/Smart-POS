using pos.Model.POS;
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

namespace pos.Model.Stor
{
    public partial class frmShowAllReturns : Form
    {
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        public int selectedPartyID = 0;

        private System.Windows.Forms.Timer inputTimer = new System.Windows.Forms.Timer();
        int pageSize = 20;
        bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;
        private bool allLoaded = false;
        public string partyType = "عميل";
        public frmShowAllReturns()
        {
            InitializeComponent();
            textSuggester();
        }

        private void frmShowAllReturns_Load(object sender, EventArgs e)
        {
            dtPickerStart.Value = DateTime.Today;
            dtPickerEnd.Value = DateTime.Today;

            dtPickerStart.Format = DateTimePickerFormat.Custom;
            dtPickerStart.CustomFormat = "yyyy-MM-dd";

            dtPickerEnd.Format = DateTimePickerFormat.Custom;
            dtPickerEnd.CustomFormat = "yyyy-MM-dd";
        }
        private void CenterPanel(Panel inner, Panel outer)
        {
            inner.Left = (outer.Width - inner.Width) / 2;
            inner.Top = (outer.Height - inner.Height) / 2;
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

        private async Task search(bool isNewSearch = false, bool isSupplier = false, bool filterByDate = false)
        {
            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {
                string qry;

                // ✅ استعلام المورد
                if (isSupplier)
                {
                    qry = @"
                    SELECT 
                        d.DetailID,
                        d.proID,
                        pr.pName AS proName,
                        c.catName,
                        d.price,
                        d.isUsed,
                        d.unite,
                        m.bID AS MainID,

                        CASE WHEN d.returnQty IS NOT NULL AND d.returnQty <> 0 
                             THEN d.returnQty 
                             ELSE d.qty 
                        END AS qty_display,

                        CASE WHEN d.returnQty IS NOT NULL AND d.returnQty <> 0 
                             THEN d.returnQty * d.price 
                             ELSE d.qty * d.price 
                        END AS amount,

                        d.DeleteFlag AS DetailDeleteFlag,
                        p.pName AS PartyName,
                        m.DeleteFlag AS MainDeleteFlag,

                        CASE WHEN m.DeleteFlag = 1 THEN m.updateDate ELSE d.updateDate END AS updateDate,
                        CASE WHEN m.DeleteFlag = 1 THEN m.updateTime ELSE d.updateTime END AS updateTime,

                        s.sName AS UpdatedByName

                    FROM tblDetailsSupliser d
                    INNER JOIN billPrcheses m ON d.billPrchesesID = m.bID
                    INNER JOIN Parties p ON m.supplierID = p.pID

                    LEFT JOIN products pr ON pr.pID = d.proID
                    LEFT JOIN category c ON c.catID = pr.categoryID

                    LEFT JOIN shifts sh ON d.shiftDoUpdate = sh.ID
                    LEFT JOIN staff s ON sh.staffID = s.staffID

                    WHERE 
                        m.supplierID = @PartyID
                        AND (m.DeleteFlag = 1 OR d.DeleteFlag = 1 OR (d.returnQty IS NOT NULL AND d.returnQty <> 0))
                        " + (filterByDate ? "AND m.date BETWEEN @dateStart AND @dateEnd" : "") + @"

                    ORDER BY m.bID, d.DetailID
                    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";
                }
                else
                {
                    // ✅ استعلام العميل
                    qry = @"
                    SELECT 
                        d.DetailID,
                        d.MainID,
                        d.proName,
                        c.catName,
                        d.isUsed,
                        d.unite,

                        CASE WHEN d.returnQty IS NOT NULL AND d.returnQty <> 0 
                             THEN d.returnQty 
                             ELSE d.qty 
                        END AS qty,

                        d.price,

                        CASE WHEN d.returnQty IS NOT NULL AND d.returnQty <> 0 
                             THEN d.returnQty * d.price
                             ELSE d.qty * d.price
                        END AS amount,

                        d.DeleteFlag AS DetailDeleteFlag,
                        p.pName AS PartyName,
                        m.DeleteFlag AS MainDeleteFlag,

                        CASE WHEN m.DeleteFlag = 1 THEN m.updateDate ELSE d.updateDate END AS updateDate,
                        CASE WHEN m.DeleteFlag = 1 THEN m.updateTime ELSE d.updateTime END AS updateTime,

                        s.sName AS UpdatedByName

                    FROM tblDetails d
                    INNER JOIN tblMain1 m ON d.MainID = m.MainID
                    INNER JOIN Parties p ON m.partiesID = p.pID

                    LEFT JOIN products pr ON pr.pID = d.proID
                    LEFT JOIN category c ON c.catID = pr.categoryID

                    LEFT JOIN shifts sh ON d.shiftDoUpdate = sh.ID
                    LEFT JOIN staff s ON sh.staffID = s.staffID

                    WHERE 
                        m.partiesID = @PartyID
                        AND (m.DeleteFlag = 1 OR d.DeleteFlag = 1 OR (d.returnQty IS NOT NULL AND d.returnQty <> 0))
                        " + (filterByDate ? "AND m.aDate BETWEEN @dateStart AND @dateEnd" : "") + @"
                    ORDER BY m.MainID, d.DetailID
                    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";
                }

                int offset = currentPage * pageSize;

                List<SqlParameter> paramList = new List<SqlParameter>
        {
            new SqlParameter("@PartyID", selectedPartyID),
            new SqlParameter("@offset", offset),
            new SqlParameter("@limit", pageSize)
        };

                if (filterByDate)
                {
                    paramList.Add(new SqlParameter("@dateStart", dtPickerStart.Value.Date));
                    paramList.Add(new SqlParameter("@dateEnd", dtPickerEnd.Value.Date));
                }

                DataTable dt = await Task.Run(() => LoadDataReturn(qry, paramList.ToArray()));

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
                    string usedText = "جديد";
                    if (row.Table.Columns.Contains("isUsed"))
                    {
                        usedText = row["isUsed"] != DBNull.Value && Convert.ToInt32(row["isUsed"]) == 1
                                   ? "مستعمل"
                                   : "جديد";
                    }

                    dgvProducts.Rows.Add(
                        rowIndex++,
                        row["MainID"],
                        row["proName"],
                        usedText,
                        row["catName"],       // ✅ اسم الفئة
                        row["unite"],
                        row.Table.Columns.Contains("qty_display") ? row["qty_display"] : row["qty"],
                        row["price"],
                        row["amount"],
                        row["updateDate"] == DBNull.Value
                            ? ""
                            : Convert.ToDateTime(row["updateDate"]).ToString("yyyy-MM-dd"),
                        row["updateTime"],
                        row["UpdatedByName"]
                    );
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

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtName.Text))
            {
                selectedPartyID = nameToID[txtName.Text];
            }
            else
            {
                selectedPartyID = 0;
            }

            if (!string.IsNullOrEmpty(txtName.Text))
            {
                char firstChar = txtName.Text[0];
                txtName.TextAlign = IsArabic(firstChar)
                    ? HorizontalAlignment.Left
                    : HorizontalAlignment.Right;
            }
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ الاتصال الصحيح
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {

                    con.Open(); // ✅ افتح الاتصال

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
                }
            }

            txtName.AutoCompleteCustomSource = dataSource;
            txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
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

        private void btnSearchParties_Click(object sender, EventArgs e)
        {
            frmPartesSearch frm = new frmPartesSearch(this);
            frm.type = partyType;
            frm.ShowDialog(this);
            this.Focus();
        }
        public void resultSearch(string pName)
        {
            txtName.Text = pName;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            hasMoreData = true;
            currentPage = 0;

            await search(
                isNewSearch: true,
                isSupplier: (partyType != "عميل"),
                filterByDate: false
            );
        }

        private async void btnSearchDate_Click(object sender, EventArgs e)
        {
            hasMoreData = true;
            currentPage = 0;

            await search(
                isNewSearch: true,
                isSupplier: (partyType != "عميل"),
                filterByDate: true
            );

        }

        private void topPanel_Resize(object sender, EventArgs e)
        {
            CenterPanel(contenerPanel, topPanel);

            lblTitel.Location = new Point(
                (titelPanel.Width - lblTitel.Width) / 2,
                (titelPanel.Height - lblTitel.Height) / 2
            );
        }
    }
}
