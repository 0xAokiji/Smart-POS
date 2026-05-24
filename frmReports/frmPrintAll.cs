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
    public partial class frmPrintAll : Form
    {
        public frmPrintAll()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            //getData();
            this.Close();

        }

        //private void getData()
        //{
        //    try
        //    {
        //        List<int> addedMainIDs = new List<int>();

        //        using (SqlConnection con1 = MainClass.GetConnection())
        //        {
        //            con1.Open();
        //            string qry1 = @"SELECT * FROM [dateTime] WHERE ID = (SELECT MAX(ID) FROM [dateTime])";
        //            using (SqlCommand cmd1 = new SqlCommand(qry1, con1))
        //            {
        //                DataTable dt1 = new DataTable();
        //                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
        //                da1.Fill(dt1);

        //                if (dt1.Rows.Count > 0)
        //                {
        //                    DateTime stDate = Convert.ToDateTime(dt1.Rows[0]["startDate"]);
        //                    DateTime endDate = Convert.ToDateTime(dt1.Rows[0]["endDate"]);

        //                    using (SqlConnection con2 = MainClass.GetConnection())
        //                    {
        //                        con2.Open();
        //                        string qry2 = @"SELECT *
        //                    FROM tblMain1 m 
        //                    INNER JOIN tblDetails d ON m.MainID = d.MainID 
        //                    INNER JOIN products p ON p.pID = d.proID
        //                    WHERE aDate BETWEEN @StartDate AND @EndDate";

        //                        using (SqlCommand command = new SqlCommand(qry2, con2))
        //                        {
        //                            command.Parameters.AddWithValue("@StartDate", stDate);
        //                            command.Parameters.AddWithValue("@EndDate", endDate);

        //                            DataTable dt2 = new DataTable();
        //                            SqlDataAdapter da2 = new SqlDataAdapter(command);
        //                            da2.Fill(dt2);

        //                            HashSet<int> printedMainIDs = new HashSet<int>();

        //                            if (dt2.Rows.Count > 0 && dt2.Columns.Contains("status"))
        //                            {
        //                                foreach (DataRow row in dt2.Rows)
        //                                {
        //                                    string status = row["status"].ToString();
        //                                    int mainID = Convert.ToInt32(row["MainID"]);

        //                                    if (!addedMainIDs.Contains(mainID) && status == "مدفوع")
        //                                    {
        //                                        using (SqlConnection con3 = MainClass.GetConnection())
        //                                        {
        //                                            con3.Open();
        //                                            string qry3 = @"SELECT * FROM printer WHERE ID = (SELECT MAX(ID) FROM printer)";
        //                                            using (SqlCommand cmd3 = new SqlCommand(qry3, con3))
        //                                            {
        //                                                DataTable dt3 = new DataTable();
        //                                                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
        //                                                da3.Fill(dt3);

        //                                                addedMainIDs.Add(mainID);
        //                                                frmPOS frm = new frmPOS();
        //                                                Reports.XtraReport1 rpt = new Reports.XtraReport1();

        //                                                if (dt3.Rows.Count > 0)
        //                                                {
        //                                                    if (dt3.Rows[0]["posprinter"].ToString() != "Not Found")
        //                                                    {
        //                                                        rpt.PrinterName = dt3.Rows[0]["posprinter"].ToString();

        //                                                        if (!printedMainIDs.Contains(mainID))
        //                                                        {
        //                                                            printedMainIDs.Add(mainID);
        //                                                            frm.report(rpt, mainID);
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("المدة التي حددتها لا يوجد بها تقارير");
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("حدث خطأ");
        //        return;
        //    }
        //}


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
