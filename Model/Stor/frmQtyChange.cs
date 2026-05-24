using pos.Test;
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
    public partial class frmQtyChange : Form
    {
        public int proID = 0;
        private int ID;
        private int PID;
        private decimal QtyU1;
        private decimal QtyU2;
        private decimal QtyU3;
        private decimal QtyUsedU1;
        private decimal QtyUsedU2;
        private decimal QtyUsedU3;
        private double qtyU3;
        private double qtyU2;
        private double qtyU1;
        private double qtyUsedU3;
        private double qtyUsedU2;
        private double qtyUsedU1;
        private System.Windows.Forms.Timer inputTimer = new System.Windows.Forms.Timer();

        public frmQtyChange()
        {
            InitializeComponent();
            inputTimer.Interval = 300; // 300ms يعني ينتظر 0.3 ثانية بعد آخر كتابة
        }

        private void frmQtyChange_Load(object sender, EventArgs e)
        {
            // في تحميل الفورم (Form_Load) أو المصمم (Designer)
            txtSmaillNew.MaxLength = 9;  // يسمح بـ 9 أرقام فقط
            txtSmaillUsed.MaxLength = 9; // يسمح بـ 9 أرقام فقط

            GetProductByPID();
        }

        private void GetProductByPID()
        {

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string query = @"SELECT [ID],[pID],[qtyU1],[qtyU2],[qtyU3],
                                        [qtyUsedU1],[qtyUsedU2],[qtyUsedU3]
                                 FROM [smartpos].[dbo].[totalStor]
                                 WHERE pID = @pID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pID", proID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0;
                            PID = reader["pID"] != DBNull.Value ? Convert.ToInt32(reader["pID"]) : 0;
                            QtyU1 = reader["qtyU1"] != DBNull.Value ? Convert.ToDecimal(reader["qtyU1"]) : 0;
                            QtyU2 = reader["qtyU2"] != DBNull.Value ? Convert.ToDecimal(reader["qtyU2"]) : 0;
                            QtyU3 = reader["qtyU3"] != DBNull.Value ? Convert.ToDecimal(reader["qtyU3"]) : 0;
                            QtyUsedU1 = reader["qtyUsedU1"] != DBNull.Value ? Convert.ToDecimal(reader["qtyUsedU1"]) : 0;
                            QtyUsedU2 = reader["qtyUsedU2"] != DBNull.Value ? Convert.ToDecimal(reader["qtyUsedU2"]) : 0;
                            QtyUsedU3 = reader["qtyUsedU3"] != DBNull.Value ? Convert.ToDecimal(reader["qtyUsedU3"]) : 0;
                        }
                    }
                }
            }
            txtSmaillNew.Text = QtyU3.ToString();
            txtMiduamNew.Text = QtyU2.ToString();
            txtLargNew.Text = QtyU1.ToString();

            txtSmaillUsed.Text = QtyUsedU3.ToString();
            txtMiduamUsed.Text = QtyUsedU2.ToString();
            txtLargUsed.Text = QtyUsedU1.ToString();

        }

        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetProductUnitInfo(double newQty = 0)
        {
            string query = @"
                        SELECT p.*, c.*, u.uName, ts.*
                        FROM products p
                        INNER JOIN category c ON c.catID = p.categoryID
                        INNER JOIN untits u ON p.idUniteDef = u.uID
                        INNER JOIN totalStor ts ON ts.pID = p.pID
                        WHERE p.pID = @value";

            using (SqlConnection con = MainClass.GetConnection()) // ✅ بدل new SqlConnection
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@value", proID);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                con.Open();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                    return; // لو مفيش بيانات للمنتج ده

                DataRow row = dt.Rows[0];

                int idUnite1 = Convert.ToInt32(row["idUnite1"]);
                int idUnite2 = Convert.ToInt32(row["idUnite2"]);
                int idUnite3 = Convert.ToInt32(row["idUnite3"]);

                int numberU2 = Convert.ToInt32(row["numberU2"]); // كم وحدة U3 في U2
                int numberU3 = Convert.ToInt32(row["numberU3"]); // كم U2 في U1

                qtyU3 = newQty;

                // 2️⃣ حساب الكميات بوحدات مختلفة
                qtyU2 = qtyU3 / numberU3; // كام وحدة U2
                qtyU1 = qtyU2 / numberU2; // كام وحدة U1
            }

        }
        private void UpdateProduct()
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string query = @"
                    UPDATE [smartpos].[dbo].[totalStor]
                    SET qtyU1 = @qtyU1,
                        qtyU2 = @qtyU2,
                        qtyU3 = @qtyU3,
                        qtyUsedU1 = @qtyUsedU1,
                        qtyUsedU2 = @qtyUsedU2,
                        qtyUsedU3 = @qtyUsedU3
                    WHERE pID = @pID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@pID", proID);
                    cmd.Parameters.AddWithValue("@qtyU1", Convert.ToDecimal(txtLargNew.Text == string.Empty ? "0" : txtLargNew.Text));
                    cmd.Parameters.AddWithValue("@qtyU2", Convert.ToDecimal(txtMiduamNew.Text == string.Empty ? "0" : txtMiduamNew.Text));
                    cmd.Parameters.AddWithValue("@qtyU3", Convert.ToDecimal(txtSmaillNew.Text == string.Empty ? "0" : txtSmaillNew.Text));
                    cmd.Parameters.AddWithValue("@qtyUsedU1", Convert.ToDecimal(txtLargUsed.Text == string.Empty ? "0" : txtLargUsed.Text));
                    cmd.Parameters.AddWithValue("@qtyUsedU2", Convert.ToDecimal(txtMiduamUsed.Text == string.Empty ? "0" : txtMiduamUsed.Text));
                    cmd.Parameters.AddWithValue("@qtyUsedU3", Convert.ToDecimal(txtSmaillUsed.Text == string.Empty ? "0" : txtSmaillUsed.Text));

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnRecive_Click(object sender, EventArgs e)
        {
            UpdateProduct();
            this.Close();
        }

        private void txtSmaillNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtSmaillUsed_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtSmaillNew_TextChanged(object sender, EventArgs e)
        {
            inputTimer.Tick -= InputTimerUsed_Tick; // ربط الحدث

            inputTimer.Tick += InputTimerNew_Tick; // ربط الحدث
            inputTimer.Stop();  // كل مرة المستخدم يكتب، نوقف المؤقت
            inputTimer.Start(); // ونشغله من جديد
        }

        private void txtSmaillUsed_TextChanged(object sender, EventArgs e)
        {
            inputTimer.Tick -= InputTimerNew_Tick; // ربط الحدث

            inputTimer.Tick += InputTimerUsed_Tick; // ربط الحدث
            inputTimer.Stop();  // كل مرة المستخدم يكتب، نوقف المؤقت
            inputTimer.Start(); // ونشغله من جديد
        }

        private void InputTimerUsed_Tick(object sender, EventArgs e)
        {
            inputTimer.Stop(); // وقف المؤقت لأنه خلص

            // 📥 قراءة الرقم بأمان
            int qty = 0;
            int.TryParse(txtSmaillUsed.Text, out qty);

            // 🔢 حساب الوحدات
            SetProductUnitInfo(qty);
            txtMiduamUsed.Text = qtyU2.ToString("F1");
            txtLargUsed.Text = qtyU1.ToString("F1");
        }
        private void InputTimerNew_Tick(object sender, EventArgs e)
        {
            inputTimer.Stop(); // وقف المؤقت لأنه خلص

            // 📥 قراءة الرقم بأمان
            int qty = 0;
            int.TryParse(txtSmaillNew.Text, out qty);

            // 🔢 حساب الوحدات
            SetProductUnitInfo(qty);
            txtMiduamNew.Text = qtyU2.ToString("F1");
            txtLargNew.Text = qtyU1.ToString("F1");
        }
    }
}

