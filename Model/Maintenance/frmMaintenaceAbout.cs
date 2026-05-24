using pos.Classes;
using pos.Model.Stor;
using pos.View;
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
using System.Xml.Linq;

namespace pos.Model.Maintenance
{
    public partial class frmMaintenaceAbout : Form
    {
        public int taskID = 0;
        private string billCode;
        private int partyID = 0;
        public frmMaintenaceAbout()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

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
        private void frmMaintenaceAbout_Load(object sender, EventArgs e)
        {
            loadData();
            if (string.IsNullOrEmpty(billCode))
                btnShowBill.Enabled = false;
            else
                btnShowBill.Enabled = true;
        }
        private void loadData()
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qry = @"
                SELECT 
                    t.taskID,
                    t.paryID,
                    t.taskNumber,
                    t.partyNotes,
                    t.tecnicalID,
                    t.descriptionProblem,
                    t.PriorityName,
                    t.taskPrice,
                    t.status,
                    t.startDate,
                    t.startTime,
                    t.endDate,
                    t.endTime,
                    p.pName AS PartyName,
                    p.pPhone AS PartyPhone,
                    s.sName AS TecnicalName,
                    S.sPhone AS TecnicalPhone,
                    m.TotalWithInterest,
                    m.InvoiceCode AS BillCode
                FROM Task t
                INNER JOIN Parties p ON t.paryID = p.pID
                INNER JOIN staff s ON t.tecnicalID = s.staffID   
                LEFT JOIN tblMain1 m ON t.taskID = m.taskID
                WHERE t.taskID = @id";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@id", taskID);

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // تعبئة القيم في عناصر الفورم
                                //txtTaskNumber.Text = reader["taskNumber"].ToString();
                                txtCustomerNote.Text = reader["partyNotes"].ToString();
                                txtDescriptionProblem.Text = reader["descriptionProblem"].ToString();
                                txttaskPrice.Text = reader["taskPrice"].ToString();
                                txtParyName.Text = reader["PartyName"].ToString();
                                txtPartyPhone.Text = reader["PartyPhone"].ToString();
                                txtTecnicalName.Text = reader["TecnicalName"].ToString();
                                txtTecnicalPhone.Text = reader["TecnicalPhone"].ToString();
                                partyID = Convert.ToInt32(reader["paryID"]);
                                txtState.Text = reader["status"].ToString();
                                txtPriorityName.Text = reader["PriorityName"].ToString();
                                billCode = reader["BillCode"].ToString();

                                decimal totalWithInterest = reader["TotalWithInterest"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(reader["TotalWithInterest"]);

                                decimal taskPrice = reader["taskPrice"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(reader["taskPrice"]);

                                decimal total = totalWithInterest + taskPrice;
                                txttaskPrice.Text = taskPrice.ToString("N0");
                                txtBillPrice.Text = totalWithInterest.ToString("N2");
                                txtTotal.Text = total.ToString("N2");



                                // لو عايز تعرض الحالة أو التواريخ
                                // lblStatus.Text = reader["status"].ToString();
                                txtDate.Text = Convert.ToDateTime(reader["startDate"]).ToString("yyyy-MM-dd") + " : " + reader["startTime"].ToString();
                            }
                            else
                            {
                                Notifier.ShowNotification("تحذير ⚠️", "لم يتم العثور على البيانات المطلوبة");
                            }
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ أثناء تحميل البيانات");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShowBill_Click(object sender, EventArgs e)
        {
            using (frmBlackout frmblackout = new frmBlackout(this))
            {
                frmblackout.Show();
                frmAll_Bills frm = new frmAll_Bills(billCode, txtCustomerNote.Text, "عميل");
                frm.lblTitle.Text = "فاتورة عميل";
                frm.ShowDialog(this);
            }
        }
    }
}
