using pos.Model;
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

namespace pos.frmReports
{
    public partial class frmReportSalesView : Form
    {
        public frmReportSalesView()
        {
            InitializeComponent();
        }

        private void frmReportSalesView_Load(object sender, EventArgs e)
        {
            getData();
        }

        private void getData()
        {
            try
            {
                List<int> addedMainIDs = new List<int>();

                string qry1 = @"SELECT * FROM [dateTime] WHERE ID = (SELECT MAX(ID) FROM [dateTime])";

                using (SqlConnection con1 = MainClass.GetConnection())
                {
                    con1.Open();
                    using (SqlCommand cmd1 = new SqlCommand(qry1, con1))
                    {
                        DataTable dt1 = new DataTable();
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(dt1);

                        if (dt1.Rows.Count > 0)
                        {
                            DateTime stDate = Convert.ToDateTime(dt1.Rows[0]["startDate"]);
                            DateTime endDate = Convert.ToDateTime(dt1.Rows[0]["endDate"]);

                            string qry2 = @"SELECT *
                        FROM tblMain1 m 
                        INNER JOIN tblDetails d ON m.MainID = d.MainID 
                        INNER JOIN products p ON p.pID = d.proID
                        WHERE aDate BETWEEN @StartDate AND @EndDate";

                            using (SqlConnection con2 = MainClass.GetConnection())
                            {
                                con2.Open();
                                using (SqlCommand command = new SqlCommand(qry2, con2))
                                {
                                    command.Parameters.AddWithValue("@StartDate", stDate);
                                    command.Parameters.AddWithValue("@EndDate", endDate);

                                    DataTable dt2 = new DataTable();
                                    SqlDataAdapter da2 = new SqlDataAdapter(command);
                                    da2.Fill(dt2);

                                    // تتبع الـ MainID التي تم طباعتها
                                    HashSet<int> printedMainIDs = new HashSet<int>();

                                    if (dt2.Rows.Count > 0 && dt2.Columns.Contains("status"))
                                    {
                                        foreach (DataRow row in dt2.Rows)
                                        {
                                            string status = row["status"].ToString();
                                            int mainID = Convert.ToInt32(row["MainID"]);

                                            // تحقق إن الـ MainID مش مضاف قبل كده
                                            if (!addedMainIDs.Contains(mainID) && status == "مدفوع")
                                            {
                                                addedMainIDs.Add(mainID);

                                                // هنا بيكمل الكود الخاص بيك للمعالجة أو الطباعة
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ");
                return;
            }
        }




    }
}
