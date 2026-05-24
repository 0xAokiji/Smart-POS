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

namespace pos.Settings
{
    public partial class frmRequestAdvance : Form
    {
        public int staffID = 0;
        public string name = "";
        public string role = "";
        public int salary = 0;
        public int oldAdvance = 0;
        private int currntSalary = 0;
        public frmRequestAdvance()
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
        private void frmRequestAdvance_Load(object sender, EventArgs e)
        {
            LoadStaffData(staffID);

            if (HasEmployeeReceivedSalary(staffID))
            {
                btnPaySalary.Enabled = false;
                btnAdvance.Enabled = false;
                Notifier.ShowNotification("تنبية", "⚠️ هذا الموظف قد استلم راتبه لهذا الشهر بالفعل");
                lblInfo.Text = "⚠️ هذا الموظف قد استلم راتبه لهذا الشهر بالفعل";
                lblInfo.Visible = true;
            }
        }
        private void LoadStaffData(int staffID)
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                string qry = @"SELECT 
       s.sName,
       s.sRole,
       CAST(s.sSalary AS int) AS sSalary,
       CAST(ISNULL(a.TotalAdvancesThisMonth, 0) AS int) AS TotalAdvancesThisMonth
FROM staff s
LEFT JOIN (
    SELECT staffID, SUM(Amount) AS TotalAdvancesThisMonth
    FROM Advances
    WHERE MONTH(AdvanceDate) = MONTH(GETDATE())
      AND YEAR(AdvanceDate) = YEAR(GETDATE())
    GROUP BY staffID
) a ON s.staffID = a.staffID
WHERE s.staffID = @staffID";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["sName"].ToString();
                            txtRole.Text = reader["sRole"].ToString();
                            salary = Convert.ToInt32(reader["sSalary"]);
                            txtSalary.Text = salary.ToString("N0");
                            oldAdvance = Convert.ToInt32(reader["TotalAdvancesThisMonth"]);
                            txtOldAdvance.Text = oldAdvance.ToString("N0");
                            currntSalary = salary - oldAdvance;
                            txtCurrentSalary.Text = (currntSalary).ToString("N0");
                        }
                        else
                        {
                            MessageBox.Show("⚠️ لا يوجد موظف بهذا الـ ID");
                        }
                    }
                }
            }
        }

        public bool HasEmployeeReceivedSalary(int empID)
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                string qry = @"
            SELECT TOP 1 IsPaid 
            FROM Salaries
            WHERE staffID = @staffID
              AND SalaryMonth = MONTH(GETDATE())
              AND SalaryYear = YEAR(GETDATE())
              AND IsPaid = 1";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", empID);
                    con.Open();

                    var result = cmd.ExecuteScalar();

                    // لو رجع قيمة يبقى الموظف قبض
                    return result != null;
                }
            }
        }
        public decimal GetRemainingSalary(int staffID)
        {
            decimal salary = 0;
            decimal advance = 0;

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                string qry = @"SELECT sSalary, sAdvance 
                       FROM staff 
                       WHERE staffID = @staffID";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", staffID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            salary = reader["sSalary"] != DBNull.Value ? Convert.ToDecimal(reader["sSalary"]) : 0;
                            advance = reader["sAdvance"] != DBNull.Value ? Convert.ToDecimal(reader["sAdvance"]) : 0;
                        }
                    }
                }
            }

            return salary - advance;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPaySalary_Click(object sender, EventArgs e)
        {
            AddSalary(staffID, salary);
        }
        public void AddSalary(int empID, decimal salaryAmount)
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                string qry = @"
                IF NOT EXISTS (
                    SELECT 1 FROM Salaries
                    WHERE staffID = @staffID 
                      AND SalaryMonth = MONTH(GETDATE())
                      AND SalaryYear = YEAR(GETDATE())
                      AND IsPaid = 1
                )
                BEGIN
                    INSERT INTO Salaries (staffID, SalaryMonth, SalaryYear, SalaryAmount, IsPaid, Amount, PaidDate, shiftID)
                    VALUES (@staffID, MONTH(GETDATE()), YEAR(GETDATE()), @SalaryAmount, 1, @Amount, GETDATE(), @shiftID)
                END";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", empID);
                    cmd.Parameters.AddWithValue("@SalaryAmount", salaryAmount);
                    cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@Amount", currntSalary);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    lblInfo.Text = "⚠️ هذا الموظف قد استلم راتبه لهذا الشهر بالفعل";
                    lblInfo.Visible = true;

                    btnAdvance.Enabled = false;
                    btnPaySalary.Enabled = false;
                    Notifier.ShowNotification("نجاح", "✅ تم دفع الراتب بنجاح");
                }
            }
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            int advanceAmount = int.Parse(txtNewAdvance.Text == string.Empty ? "0" : txtNewAdvance.Text);

            bool canAdd = AddAdvance(staffID, advanceAmount);
            if (canAdd)
            {
                Notifier.ShowNotification("نجاح", "✅ تم إضافة السلفة بنجاح.");
                LoadStaffData(staffID); // تحديث البيانات
            }
            else
            {
                Notifier.ShowNotification("خطأ", "❌ لا يمكن إضافة السلفة. مجموع السلف لهذا الشهر يتجاوز الراتب");
            }
        }
        public bool AddAdvance(int empID, int amount)
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // 1️⃣ هات راتب الموظف
                string qrySalary = "SELECT sSalary FROM staff WHERE staffID = @staffID";
                decimal salary = 0;
                using (SqlCommand cmd = new SqlCommand(qrySalary, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", empID);
                    salary = Convert.ToDecimal(cmd.ExecuteScalar() ?? 0);
                }

                // 2️⃣ هات مجموع السلف السابقة للشهر الحالي
                string qryAdvance = @"
            SELECT ISNULL(SUM(Amount),0) 
            FROM Advances 
            WHERE staffID = @staffID 
              AND MONTH(AdvanceDate) = MONTH(GETDATE()) 
              AND YEAR(AdvanceDate) = YEAR(GETDATE())";

                decimal totalAdvance = 0;
                using (SqlCommand cmd = new SqlCommand(qryAdvance, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", empID);
                    totalAdvance = Convert.ToDecimal(cmd.ExecuteScalar() ?? 0);
                }

                // 3️⃣ تحقق إن السلف الحالية + الجديدة ≤ الراتب
                if (totalAdvance + amount <= salary)
                {
                    // مسموح الإضافة
                    string qryInsert = @"INSERT INTO Advances (staffID, Amount, AdvanceDate, shiftID) 
                                 VALUES (@staffID, @Amount, GETDATE(), @shiftID)";

                    using (SqlCommand cmd = new SqlCommand(qryInsert, con))
                    {
                        cmd.Parameters.AddWithValue("@staffID", empID);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);

                        cmd.ExecuteNonQuery();
                    }

                    return true; // تمت الإضافة
                }
                else
                {
                    return false; // مش مسموح
                }
                txtNewAdvance.Text = string.Empty;
            }
        }

        private void txtNewAdvance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
