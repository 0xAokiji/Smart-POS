using pos.Classes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Settings
{
    public partial class frmOwnerUnlock : Form
    {
        public frmOwnerUnlock()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string input = txtPassword.Text ?? "";

            if (VerifyOwnerPassword(input))
            {
                this.DialogResult = DialogResult.OK;
                Notifier.ShowNotification("Done ✔", "✔ كلمة المرور صحيحة");
                this.Close();
            }
            else
            {
                Notifier.ShowNotification("Error ❌", "❌ كلمة المرور غير صحيحة");
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private bool VerifyOwnerPassword(string inputPassword)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    string qry = "SELECT TOP 1 OwnerPassHash FROM OwnerPass";
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            Notifier.ShowNotification("⚠️ تنبيه", "لم يتم تعيين كلمة مرور للمالك بعد!");
                            return false;
                        }

                        string storedHash = result.ToString();

                        // ✅ تحقق من كلمة المرور المدخلة
                        return MainClass.VerifyPassword(inputPassword, storedHash);
                    }
                }
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("Error ❌", $"حدث خطأ أثناء التحقق:\n{ex.Message}");
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
