using pos.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Settings
{
    public partial class frmEnterpasskey : Form
    {
        public frmEnterpasskey()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            //string password = txtPasskey.Text.Trim();

            //if (string.IsNullOrEmpty(password))
            //{
            //    MessageBox.Show("⚠️ أدخل كلمة مرور أولاً.");
            //    return;
            //}

            //SetResetPassword(password);
            //return;

            using (SqlConnection con = MainClass.GetConnection())
            {
                string qry = @"SELECT ResetPasswordHash FROM settings";
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            // ✅ جلب الهاش المخزن في قاعدة البيانات
                            string storedHash = dt.Rows[0]["ResetPasswordHash"].ToString();

                            // ✅ التحقق من كلمة المرور المدخلة باستخدام دالة التحقق
                            if (MainClass.VerifyPassword(txtPasskey.Text, storedHash))
                            {
                                Notifier.ShowNotification("Done ✔", "✔ كلمة المرور صحيحة، يمكنك المتابعة في عملية Reset.");
                                // تابع العملية هنا
                                this.DialogResult = DialogResult.OK; // ← ترجع قيمة نجاح
                                this.Close();
                            }
                            else
                            {
                                Notifier.ShowNotification("Error ❌", "❌ كلمة المرور غير صحيحة!");
                            }
                        }
                        else
                        {
                            Notifier.ShowNotification("Error ❌", "⚠️ لم يتم العثور على بيانات في جدول الإعدادات!");
                        }
                    }
                }
            }

        }
        public static void SetResetPassword(string password)
        {
            // توليد الهاش
            string hash = HashPassword(password);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // تحديث العمود في جدول settings
                string qry = "UPDATE settings SET ResetPasswordHash = @hash";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@hash", hash);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        Notifier.ShowNotification("Done ✔", "✅ تم حفظ باسورد الـ Reset بنجاح!");
                    else
                        Notifier.ShowNotification("Error ❌", "⚠️ لم يتم تحديث أي صف في الجدول.");
                }
            }
        }

        // 🔸 دالة إنشاء الهاش بنفس نمط VerifyPassword
        private static string HashPassword(string password)
        {
            int iterations = 10000; // عدد الدورات
            byte[] salt = RandomNumberGenerator.GetBytes(16); // إنشاء Salt عشوائي

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // توليد الهاش بطول 32 بايت (SHA256)
                                                   // دمجهم معًا بالشكل: iterations.salt.hash
                return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
            }
        }
        private void SetPasskey()
        {
            // SmartPOS_Reset@2025
            string password = txtPasskey.Text.Trim();

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("⚠️ أدخل كلمة مرور أولاً.");
                return;
            }

            SetResetPassword(password);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
