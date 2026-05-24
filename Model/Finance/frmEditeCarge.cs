using pos.Classes;
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
    public partial class frmEditeCarge : Form
    {
        public int chargeID = 0;
        public double charge = 0;
        public double currentCharge = 0;
        public int partiesID = 0;
        private double amountPaid;
        public string partyType = "عميل";

        public frmEditeCarge()
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
        private void frmEditeCarge_Load(object sender, EventArgs e)
        {
            txtcharge.Text = charge.ToString();
            txtCurrentBalance.Text = (charge + currentCharge).ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Rpay();
            this.Close();
        }
        private void Rpay()
        {
            if (!string.IsNullOrEmpty(txtcharge.Text))
            {
                amountPaid = Convert.ToInt32(txtcharge.Text);
                double oldBalance = currentCharge + charge;
                double newBalance = oldBalance - amountPaid;

                if (amountPaid <= 0)
                {
                    MessageBox.Show("المبلغ المدفوع يجب أن يكون أكبر من الصفر.");
                    return;
                }

                string qry;
                if (partyType == "عميل")
                {
                    qry = @"
            UPDATE chargeResidual
            SET 
                [shiftId] = @posName,
                [recipt] = @recipt,
                [change] = @change
            WHERE id = @id";
                }
                else
                {
                    qry = @"
            UPDATE chargeResidualSuplieser
            SET 
                [shiftId] = @posName,
                [recipt] = @recipt,
                [change] = @change
            WHERE id = @id";
                }

                using (SqlConnection con = MainClass.GetConnection()) // ✅ الطريقة الآمنة
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@id", chargeID);
                    cmd.Parameters.AddWithValue("@posName", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@recipt", amountPaid);
                    cmd.Parameters.AddWithValue("@change", newBalance);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                // ✅ دفع المبلغ الجزئي
                PayPartialAmount(amountPaid, newBalance, oldBalance);

                // ✅ عرض إشعار النجاح
                Notifier.ShowNotification("تم الدفع", "تم دفع المبلغ بنجاح");

                // ✅ تسجيل العملية في السجل
                transactions(amountPaid, newBalance, oldBalance);
            }
            else
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة.");
            }
        }

        private void transactions(double amoutPaied, double currentBalance, double prevBalance)
        {
            string qtyTransaction = @"
        INSERT INTO PartiesTransactions
            (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
        VALUES
            (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID, 
            CAST(GETDATE() AS DATE), @aTime);";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        // أولاً: PartiesTransactions
                        using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                        {
                            cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                            cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "تعديل ايصال دفع");
                            cmdTransaction.Parameters.AddWithValue("@mainID", DBNull.Value);
                            cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                              $"تم تعديل هذاالايصال الي  {Convert.ToDecimal(amoutPaied).ToString("N0")}");
                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", prevBalance);
                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", currentBalance);
                            cmdTransaction.ExecuteNonQuery();
                        }
                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        // فشل → نلغي كل حاجة
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        private void PayPartialAmount(double amountPaid, double newBalance, double prevBalance)
        {
            string qry = @"
        UPDATE residualTable 
        SET
            [status] = @status, 
            previousDebitBalance = @previousDebitBalance, 
            currentDebitBalance = @newBalance
        WHERE PartiesID = @PartiesID;";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {


                        // تحديد الحالة
                        string status = "مدين";
                        if (newBalance == 0) status = "مسدد";
                        else if (newBalance < 0) status = "دائن";

                        using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@PartiesID", partiesID);
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@previousDebitBalance", prevBalance);
                            cmd.Parameters.AddWithValue("@newBalance", newBalance);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void btnExite_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtcharge_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }
    }
}
