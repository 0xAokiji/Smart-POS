using DevExpress.CodeParser;
using DevExpress.Xpo.DB;
using DevExpress.XtraCharts.Native;
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.View
{
    public partial class frmReportPurcheseView : SampleView
    {

        public frmReportPurcheseView()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

        }



        public void GetData()
        {
            try
            {
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

                            string qry = "SELECT * FROM purchases WHERE pname LIKE N'%" + txtSearch1.Text + "%' AND aDate BETWEEN @StartDate AND @EndDate";

                            using (SqlConnection con2 = MainClass.GetConnection())
                            {
                                con2.Open();
                                using (SqlCommand command = new SqlCommand(qry, con2))
                                {
                                    command.Parameters.AddWithValue("@StartDate", stDate);
                                    command.Parameters.AddWithValue("@EndDate", endDate);

                                    ListBox lb = new ListBox();
                                    lb.Items.Add(dgvid);
                                    lb.Items.Add(dgvName);
                                    lb.Items.Add(dgvPname);
                                    lb.Items.Add(dgvPrice);
                                    lb.Items.Add(dgvQty);
                                    lb.Items.Add(dgvTime);
                                    lb.Items.Add(dgvDate);

                                    MainClass.LoadData(command, guna2DataGridView2, lb);
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


        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }
      

        private void frm_Load(object sender, EventArgs e)
        {

            GetData();
        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            if (guna2DataGridView2.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;


                if (guna2MessageDialog1.Show(" هل تريد حذه هذا الصنف ") == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView2.CurrentRow.Cells["dgvid"].Value);
                    string qry = "Delete from purchases where pid = " + id + "";
                    Hashtable ht = new Hashtable();
                    MainClass.SQL(qry, ht);
                    guna2DataGridView2.Rows.RemoveAt(e.RowIndex);
                    Notifier.ShowNotification("Done ✅", "تم الحذف بنجاح");
                    GetData();
                }


            }
        }

       
    }
}
