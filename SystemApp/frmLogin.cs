using Guna.UI2.WinForms;
using pos.AccountManagement;
using pos.Classes;
using pos.GeneralForms;
using pos.GeneralForms.MainForm;
using pos.Model.POS;
using pos.Settings;
using pos.SystemApp;
using pos.UserControls;
using pos.View;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace pos
{
    public partial class frmLogin : Form
    {
        int span = 0;

        // دوال من User32.dll
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        public frmLogin()
        {
            InitializeComponent();

            frmGraphicalinterFace frmGraphicalinterFace = new frmGraphicalinterFace(true);
            this.Icon = frmGraphicalinterFace.Icon;
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {


            logIn(txtUser.Text, txtPassword.Text);


        }

        private async void logIn(string userName, string password)
        {
            if (MainClass.IsvalidUser(userName, password) == false)
            {
                if (MainClass.NoUsers)
                {
                    await InsertDefaultAdminAsync();

                    guna2MessageDialog1.Icon = MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = MessageDialogButtons.OK;
                    guna2MessageDialog1.Caption = "تنبيه";
                    guna2MessageDialog1.Text = "لا يوجد أي مستخدمين حالياً في النظام.\n" +
                                                "سيتم تحويلك الآن إلى شاشة خاصة لإدخال كلمة مرور المالك,\n " +
                                                "وذالك لأجل إنشاء مستخدم\n" +
                                                "(Administrator).";

                    guna2MessageDialog1.Show();
                    using (frmBlackout frmblackout = new frmBlackout(this))
                    {
                        frmblackout.Show();
                        InsertOwnerPassword("Shop_Owner_Password@2025");
                        using (frmOwnerUnlock frm = new frmOwnerUnlock())
                        {

                            DialogResult result = frm.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                frmNewUser frmUser = new frmNewUser(true);
                                frmUser.ShowDialog();
                                DialogResult result2 = MessageBox.Show(
                               "سيتم إعادة تشغيل البرنامج لتطبيق التغييرات.",
                               "إعادة التشغيل",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);

                                if (result2 == DialogResult.OK)
                                {
                                    Application.Restart(); // إعادة تشغيل التطبيق
                                    Environment.Exit(0);   // إنهاء النسخة الحالية فورًا
                                }
                            }
                        }
                    }

                }
                else
                {
                    guna2MessageDialog1.Show("اسم المستخدم او كلمة المرور غير صحيحة");
                    txtPassword.Focus();
                    return;
                }

            }
            else
            {
                if (cbRememberMe.Checked == true)
                {
                    SaveLogin(userName, password);
                }
                
                if(ValidateOrInitializeLicense() == false)
                {
                    Application.Exit();
                    return;
                }
                frmMain existingForm = Application.OpenForms.OfType<frmMain>().FirstOrDefault();

                if (existingForm != null)  // إذا كانت الفورم موجودة ومفتوحة
                {
                    if (!existingForm.Visible)  // إذا كانت مخفية
                    {
                        existingForm.Show();  // عرض الفورم
                    }
                }
                else  // إذا لم تكن موجودة
                {
                    // فتح نسخة جديدة من frmMain
                    frmMian2 frmMain = new frmMian2();
                    frmMain.Show();
                    Notifier.ShowNotification(
                          "تسجيل الدخول",
                          $"مرحباً {MainClass.USER} ✅\nتم تسجيل الدخول بنجاح"
                     );
                }
                this.Close();

            }

        }
        private void InsertOwnerPassword(string password)
        {
            string hash = HashPassword(password);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // ✅ أولاً: التحقق هل الجدول يحتوي على صفوف
                string checkQry = "SELECT COUNT(*) FROM OwnerPass";
                using (SqlCommand checkCmd = new SqlCommand(checkQry, con))
                {
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        //Notifier.ShowNotification("⚠️ تنبيه", "يوجد كلمة مرور مسبقًا! لا يمكن إضافة أكثر من واحدة.");
                        return;
                    }
                }

                // ✅ إذا الجدول فاضي، يتم الإدخال
                string insertQry = "INSERT INTO OwnerPass (OwnerPassHash) VALUES (@hash)";
                using (SqlCommand cmd = new SqlCommand(insertQry, con))
                {
                    cmd.Parameters.AddWithValue("@hash", hash);
                    cmd.ExecuteNonQuery();
                }
            }

            //Notifier.ShowNotification("Done ✔", "✅ تم تعيين كلمة المرور بنجاح!");
        }
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
        public static async Task InsertDefaultAdminAsync()
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    await con.OpenAsync();

                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            // 🔹 تحقق من وجود صفوف في جدول staff
                            string checkStaffCount = "SELECT COUNT(*) FROM staff";
                            using (SqlCommand cmdCheck = new SqlCommand(checkStaffCount, con, tran))
                            {
                                int count = Convert.ToInt32(await cmdCheck.ExecuteScalarAsync());

                                if (count == 0)
                                {
                                    // 🔹 إدخال المستخدم الافتراضي Administrator
                                    string insertStaff = @"
                            INSERT INTO staff (sName, sPhone, sRole, sSalary, sAdvance)
                            VALUES ('Administrator', 'Phone Number', 'admin', 0, 0)";

                                    using (SqlCommand cmdInsert = new SqlCommand(insertStaff, con, tran))
                                    {
                                        await cmdInsert.ExecuteNonQueryAsync();
                                    }

                                    tran.Commit();
                                    Notifier.ShowNotification("تم", "✅ تم إنشاء حساب Administrator الافتراضي بنجاح");
                                }
                                else
                                {
                                    Notifier.ShowNotification("تنبيه ⚠️", "يوجد بالفعل موظفون في الجدول، لم يتم إنشاء حساب جديد.");
                                    tran.Rollback();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("حدث خطأ أثناء إنشاء حساب Administrator:\n" + ex.Message,
                                            "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر الاتصال بقاعدة البيانات:\n" + ex.Message,
                                "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DateTime GetServerDate()
        {
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT GETDATE()", con))
            {
                con.Open();
                return Convert.ToDateTime(cmd.ExecuteScalar());
            }
        }

        private bool ValidateOrInitializeLicense()
        {

            try
            {
                // إعدادات عامة للـ dialog
                guna2MessageDialog1.Caption = "تنبيه";
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Dark;

                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    // 🔍 التحقق من وجود صف واحد على الأقل
                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM [LicenseInfo]", con))
                    {
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        // 🟡 إنشاء صف جديد لو الجدول فاضي
                        if (count == 0)
                        {
                            string publicKey =
                            "MIIBCgKCAQEAoRTC6BGTByHMGYZW3UpE4hmiIwUNApW+nRHIQ/7lLWXBhxHJjNMMjiPTs7X1O7iSeAHYAg3IdziABbd+N93n2boZxO9nutFJnxwJqwMsDy8KKw5Q/eBt7KVrCQcWcV/cRnkfnhKN446d4WBE7cQteQ4F2vhETJniMuXB7uFnOwFrY8agjHEsOk0DLLXenf0XBQT35H6SdbZjKmm7XSRILUdQtK/U21DGNLK3+KJZM1fjA1HK3EGroVuu1Hw8trpn3b2Gcc/1GREoXeAZMwx5HrWOkk+J6eEfEcOd2TajgULdKP+pqmbhT5H1XOtxT62fD2s1Cms3ngIQgTFkW9L3mQIDAQAB";


                            string randomKey = GenerateRandomKey();
                            bool isActivated = false;

                            // 🔒 تشفير المفتاح العشوائي بالمفتاح العام
                            string encryptedKey = EncryptWithPublicKey(randomKey, publicKey);

                            // 🔐 حساب هاش المفتاح المشفر
                            string hashActivationKey = LicenseSigner.ComputeHashActivationKey(randomKey);

                            // 🔐 إنشاء توقيع للتحقق من سلامة البيانات باستخدام كل القيم
                            string hashValue = LicenseSigner.ComputeSignature(
                                keyCurrent: "",
                                activationKey: encryptedKey,
                                hashActivationKey: hashActivationKey, // الآن نمرر هذا العمود
                                publicKey: publicKey,
                                isActivated: isActivated
                            );


                            // 🧱 إدخال البيانات
                            using (SqlCommand insertCmd = new SqlCommand(@"
                            INSERT INTO [LicenseInfo] 
                                (key_Current, Activation_Key, Hash_Activation_Key, PK_key, Is_Activated, hash_Value)
                            VALUES 
                                (@key_Current, @Activation_Key, @Hash_Activation_Key, @PK_key, @Is_Activated, @hash_Value)", con))
                            {
                                insertCmd.Parameters.AddWithValue("@key_Current", "");
                                insertCmd.Parameters.AddWithValue("@Activation_Key", encryptedKey);
                                insertCmd.Parameters.AddWithValue("@Hash_Activation_Key", hashActivationKey); // العمود الجديد
                                insertCmd.Parameters.AddWithValue("@PK_key", publicKey);
                                insertCmd.Parameters.AddWithValue("@Is_Activated", isActivated);
                                insertCmd.Parameters.AddWithValue("@hash_Value", hashValue);

                                insertCmd.ExecuteNonQuery();
                            }

                        }
                    }

                    // 🔹 استرجاع بيانات الترخيص الحالية
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM [LicenseInfo]", con))
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string keyCurrent = dr["key_Current"]?.ToString();
                            string activationKey = dr["Activation_Key"]?.ToString();
                            string hashActivationKey = dr["Hash_Activation_Key"]?.ToString(); // العمود الجديد
                            string publicKey = dr["PK_key"]?.ToString();
                            bool isActivated = dr["Is_Activated"] != DBNull.Value && Convert.ToBoolean(dr["Is_Activated"]);
                            string hashValue = dr["hash_Value"]?.ToString();

                            // ✅ التحقق من سلامة البيانات مع أخذ Hash_Activation_Key في الاعتبار
                            bool valid = LicenseSigner.VerifySignature(
                                keyCurrent,
                                activationKey,
                                hashActivationKey, // نمرره هنا
                                publicKey,
                                isActivated,
                                hashValue
                            );

                            if (!valid)
                            {
                                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                guna2MessageDialog1.Show("⚠️ Data integrity check failed for [LicenseInfo].", "Error");
                                return false;
                            }

                            // ✅ تحليل حالة الترخيص
                            if (keyCurrent == activationKey && isActivated)
                            {
                                return true; // مرخص دائمًا
                            }
                            else if (string.IsNullOrEmpty(keyCurrent) && !isActivated)
                            {
                                var trialInfo = GetTrialInfo();
                                if (trialInfo != null)
                                {
                                    DateTime currentDate = GetServerDate().Date;
                                    DateTime startDate = trialInfo.Value;
                                    int daysUsed = (currentDate - startDate).Days;
                                    span = 15 - daysUsed;

                                    if (daysUsed <= 15)
                                    {
                                        this.Hide();
                                        frmTrialTime frm = new frmTrialTime();
                                        frm.ShowDialog();
                                        return true;
                                    }
                                    else
                                    {


                                        // عرض الرسالة
                                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                                        var result = guna2MessageDialog1.Show(
                                            "انتهت الفترة التجريبية.\nللاستمرار في استخدام البرنامج، يرجى تفعيل الترخيص.\nهل ترغب في تفعيل البرنامج الآن؟"
                                        );

                                        if (result == DialogResult.Yes)
                                        {
                                            frmTrialTime frmTrialTime = new frmTrialTime();
                                            frmTrialTime.expired = true;
                                            frmTrialTime.ShowDialog();
                                            this.Close();
                                        }
                                        else if (result == DialogResult.No)
                                        {
                                            return false;

                                        }
                                        this.Hide();
                                        frmTrialTime frm = new frmTrialTime();
                                        frm.ShowDialog();
                                        return false;
                                    }
                                }
                            }
                            else if (keyCurrent != activationKey && !isActivated)
                            {
                                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                guna2MessageDialog1.Show("❌ Invalid license key.", "License Error");
                                this.Hide();
                                frmTrialTime frm = new frmTrialTime();
                                frm.expired = true;
                                frm.ShowDialog();
                                return false;
                            }
                            else if (keyCurrent != activationKey && isActivated)
                            {
                                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                guna2MessageDialog1.Show("⚠️ License mismatch detected.", "License Warning");
                                this.Hide();
                                frmTrialTime frm = new frmTrialTime();
                                frm.expired = true;
                                frm.ShowDialog();
                                return false;
                            }
                        }
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Error: " + ex.Message, "Error");
                return false;
            }
        }

        private string EncryptWithPublicKey(string plainText, string base64PublicKey)
        {
            byte[] publicKeyBytes = Convert.FromBase64String(base64PublicKey);

            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.ImportRSAPublicKey(publicKeyBytes, out _); // لو ممكن تستخدم .NET 5+, وإلا استورد المفتاح بالطريقة القديمة
                }
                catch
                {
                    throw new Exception("❌ المفتاح العام غير صالح.");
                }

                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);

                // هنا false يعني تشفير بدون padding → ثابت لكل مرة
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, false);

                return Convert.ToBase64String(encryptedData);
            }
        }




        private string GenerateRandomKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 16; i++)
            {
                sb.Append(chars[rnd.Next(chars.Length)]);
                if ((i + 1) % 4 == 0 && i != 15)
                    sb.Append("-");
            }

            return sb.ToString(); // مثال: "ABCD-EFGH-IJKL-MNOP"
        }
        public static bool VerifyLicenseData()
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM [LicenseInfo]", con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string keyCurrent = dr["key_Current"].ToString();
                            string activationKey = dr["Activation_Key"].ToString();
                            string hashActivationKey = dr["Hash_Activation_Key"].ToString(); // العمود الجديد
                            string publicKey = dr["PK_key"].ToString();
                            bool isActivated = Convert.ToBoolean(dr["Is_Activated"]);
                            string hashValue = dr["hash_Value"].ToString();

                            bool valid = LicenseSigner.VerifySignature(
                                keyCurrent,
                                activationKey,
                                hashActivationKey, // نمرره هنا
                                publicKey,
                                isActivated,
                                hashValue
                            );

                            return valid; // ✅ ترجع true أو false حسب النتيجة
                        }
                        else
                        {
                            // ⚠️ الجدول فارغ
                            return false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error verifying license: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private DateTime? GetTrialInfo()
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    // ✅ أولاً: حاول تجيب startDate من جدول trialTime
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 1 startDate FROM [trialTime]", con))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return DecryptDate(result.ToString()); // ✅ بيرجع DateTime
                        }
                        else
                        {
                            DateTime today = DateTime.Now.Date;

                            // 1️⃣ استعلام قيمة isFirstTime من جدول serialNumber
                            bool isFirstTime = false;
                            string dbSerial = "";

                            using (SqlCommand checkCmd = new SqlCommand(
                                "SELECT TOP 1 isFirstTime, serial FROM serialNumber", con))
                            {
                                using (SqlDataReader reader = checkCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        isFirstTime = Convert.ToBoolean(reader["isFirstTime"]);
                                        dbSerial = reader["serial"].ToString();
                                    }
                                }
                            }

                            // 2️⃣ لو isFirstTime = true → Insert في trialTime
                            if (isFirstTime)
                            {
                                using (SqlCommand insertCmd = new SqlCommand(
                                    "INSERT INTO [trialTime] (startDate) VALUES (@StartDate)", con))
                                {
                                    insertCmd.Parameters.AddWithValue("@StartDate", EncryptDate(today));
                                    insertCmd.ExecuteNonQuery();
                                }


                                SerailSigner.UpdateSignature(con, dbSerial, false);

                            }
                            else
                            {
                                guna2MessageDialog1.Show("❌  البرنامج غير قادر علي جلب بيانات الفترة التجريبية");
                                Application.Exit();
                            }

                            return today;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء قراءة/إدخال بيانات الفترة التجريبية: " + ex.Message);
                return null;
            }
        }



        public static string EncryptDate(DateTime date)
        {
            byte[] key = KeyManager.GetOrCreateKey();
            string dateText = date.ToString("yyyy-MM-dd");
            return AesEncryption.Encrypt(dateText, key);
        }
        public static DateTime DecryptDate(string encryptedDate)
        {
            byte[] key = KeyManager.GetOrCreateKey();
            string plainText = AesEncryption.Decrypt(encryptedDate, key);

            // إزالة BOM وأي مسافات أو أحرف غير مطبوعة
            plainText = plainText.TrimStart('\uFEFF').Trim();

            return DateTime.ParseExact(plainText, "yyyy-MM-dd", null).Date;
        }

        public static string Decryptstring(string encryptedText)
        {
            byte[] key = KeyManager.GetOrCreateKey();
            string decrypted = AesEncryption.Decrypt(encryptedText, key);

            // إزالة BOM إذا موجود
            if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                decrypted = decrypted.Substring(1);

            // إزالة أي whitespace إضافية
            decrypted = decrypted.Trim();

            return decrypted;
        }



        private void frmLogin_Load(object sender, EventArgs e)
        {
            GetUsersPage();
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPassword.Text) && !string.IsNullOrWhiteSpace(txtUser.Text))
                btnLogin.Enabled = true; // 🔵 تفعيل الزر لما يكون فيهم بيانات

            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                txtUser.TextAlign = HorizontalAlignment.Right;

                return;

            }
            char firstChar = txtUser.Text[0];

            if (IsArabic(firstChar))
                txtUser.TextAlign = HorizontalAlignment.Right;
            else
                txtUser.TextAlign = HorizontalAlignment.Left;
        }

        private void txtPassword_IconRightClick(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '\0';
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;

            txtPassword.IconRight = txtPassword.UseSystemPasswordChar
                         ? Properties.Resources.showpass_dark
                         : Properties.Resources.showpassNo_dark;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPassword.Text) && !string.IsNullOrWhiteSpace(txtUser.Text))
                btnLogin.Enabled = true; // 🔵 تفعيل الزر لما يكون فيهم بيانات
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.TextAlign = HorizontalAlignment.Right;
                txtPassword.IconRight = null; // 🔹 يخلي الأيقونة تختفي
                return;

            }
            else
            {
                txtPassword.IconRight = txtPassword.UseSystemPasswordChar
                   ? Properties.Resources.showpass_dark
                   : Properties.Resources.showpassNo_dark;
            }
            char firstChar = txtPassword.Text[0];

            if (IsArabic(firstChar))
                txtPassword.TextAlign = HorizontalAlignment.Right;
            else
                txtPassword.TextAlign = HorizontalAlignment.Left;
        }

        private void smoothPanelTopConrner1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        // حفظ بيانات تسجيل الدخول (عند تفعيل "تذكرني")
        public static void SaveLogin(string username, string plainPassword)
        {
            // تشفير الباسورد باستخدام المفتاح المحفوظ بـ DPAPI
            byte[] key = KeyManager.GetOrCreateKey();
            string encryptedPass = AesEncryption.Encrypt(plainPassword, key);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string qry = @"
            IF EXISTS (SELECT 1 FROM SavedLogins WHERE Username = @user)
                UPDATE SavedLogins 
                SET PasswordEncrypted = @pass, LastLogin = @lastlogin
                WHERE Username = @user;
            ELSE
                INSERT INTO SavedLogins (staffID, Username, PasswordEncrypted, LastLogin)
                VALUES (@staffID, @user, @pass, @lastlogin);";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    DateTime today = DateTime.Now.Date;

                    cmd.Parameters.AddWithValue("@staffID", MainClass.staffID);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", encryptedPass);
                    cmd.Parameters.AddWithValue("@lastlogin", EncryptDate(today));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // حذف حفظ المستخدم لو عمل "خروج" وعايز تلغي حفظه
        public static void RemoveSavedLogin(string username)
        {
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM SavedLogins WHERE Username = @user", con))
            {
                cmd.Parameters.AddWithValue("@user", username);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void GetUsersPage()
        {
            try
            {
                string qry = @"
        SELECT 
            sl.Username,
            sl.PasswordEncrypted,
            sl.LastLogin,
            s.sName,
            u.userImage
        FROM SavedLogins sl
        INNER JOIN staff s ON sl.staffID = s.staffID
        INNER JOIN users u ON sl.staffID = u.staffID;";

                DataTable dt = new DataTable();

                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }

                // 🔹 عرض البيانات
                foreach (DataRow item in dt.Rows)
                {
                    Image img = null;

                    if (item["userImage"] != DBNull.Value)
                    {
                        byte[] imagearray = (byte[])item["userImage"];
                        using (var ms = new MemoryStream(imagearray))
                        {
                            img = Image.FromStream(ms);
                        }
                    }

                    AddItems(
                        item["sName"].ToString(),                // الاسم (sName)
                        img,                                     // الصورة (userImage)
                        item["PasswordEncrypted"].ToString(),    // الباسورد المشفر
                        item["Username"].ToString(),             // اليوزرنيم
                        DecryptDate(item["LastLogin"].ToString())
                    );
                }
            }
            catch (SqlException dbException)
            {
                //string errorMsg = "Database Error:\n" + dbException.Message;

                //Action showErrorAction1 = () =>
                //{
                //    guna2MessageDialog1.Caption = "Error";   // العنوان فوق
                //    guna2MessageDialog1.Text = errorMsg;     // نص الرسالة
                //    guna2MessageDialog1.Show();

                //    // أو تفتح فورم الإعدادات وتعرض الرسالة فيها
                //    if (!Application.OpenForms.OfType<FormInishialSettings>().Any())
                //    {
                //        FormInishialSettings frmError = new FormInishialSettings();
                //        this.Hide();
                //        frmError.ShowDialog();
                //        Application.Exit();
                //    }
                //};

                //if (this.IsHandleCreated)
                //{
                //    this.Invoke(showErrorAction1);
                //}
                //else
                //{
                //    // لو الفورم لسه مفيهوش Handle → نفذ الكود عادي
                //    showErrorAction1();
                //}
            }
            catch (Exception ex)
            {
                // 🟡 أي خطأ غير SQL
                MessageBox.Show("Unexpected Error:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private Queue<saveLogingIcon> recycledProducts = new Queue<saveLogingIcon>();

        private void AddItems(string name, Image pimage, string password, string username, DateTime lastTime)
        {
            saveLogingIcon s;
            if (recycledProducts.Count > 0)
            {
                // إعادة استخدام عنصر قديم
                s = recycledProducts.Dequeue();
                s.Size = new Size(113, 112);
                s.Visible = true;
            }
            else
            {
                // إنشاء عنصر جديد
                s = new saveLogingIcon();
                s.Size = new Size(113, 112);

                // ربط الأحداث (مرّة واحدة عند الإنشاء)
                s.onClick += S_onClick;
                s.onDelete += S_onDelete;
            }

            // تحديث بيانات العنصر
            s.password = password;
            s.PName = name;       // اسم العنصر في الفورم
            s.PImage = pimage;
            s.username = username;
            s.lastLogin = lastTime.ToString();
            // إضافة للوحة إذا غير موجود بالفعل
            if (!currentFlowPanelPerson.Controls.Contains(s))
                currentFlowPanelPerson.Controls.Add(s);
        }
        private void S_onClick(object ss, EventArgs ee)
        {
            var wdg = (saveLogingIcon)ss;

            // ✅ تحقق من وجود آخر تسجيل دخول
            if (!string.IsNullOrEmpty(wdg.lastLogin))
            {
                DateTime lastTime;
                if (DateTime.TryParse(wdg.lastLogin, out lastTime))
                {
                    DateTime currentDate = GetServerDate();

                    // مدة صلاحية التوكن (هنا يوم واحد)
                    TimeSpan tokenLifetime = TimeSpan.FromDays(10);

                    if (currentDate - lastTime <= tokenLifetime)
                    {
                        // ✅ فك تشفير الباسورد وتسجيل الدخول عادي
                        byte[] key = KeyManager.GetOrCreateKey();
                        string passwrd = Decryptstring(wdg.password);

                        logIn(wdg.username, passwrd);
                    }
                    else
                    {
                        guna2MessageDialog1.Caption = "رفض الدخول";
                        guna2MessageDialog1.Text = $"❌ التوكن منتهي الصلاحية\nآخر تسجيل دخول: {lastTime:yyyy/MM/dd}";
                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;

                        guna2MessageDialog1.Show();

                        RemoveSavedLogin(wdg.username);
                        currentFlowPanelPerson.Controls.Clear();
                        GetUsersPage();
                    }
                }
            }

        }


        private void S_onDelete(object ss, EventArgs ee)
        {
            var wdg = (saveLogingIcon)ss;

            DialogResult result = MessageBox.Show(
                $"هل أنت متأكد أنك تريد حذف المستخدم: {wdg.username}؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2 // الافتراضي يكون "لا"
            );

            if (result == DialogResult.Yes)
            {
                RemoveSavedLogin(wdg.username);
            }
            currentFlowPanelPerson.Controls.Clear();
            GetUsersPage();
        }

        bool isShow = false;
        private void picArrow_Click(object sender, EventArgs e)
        {
            if (isShow)
            {
                this.Size = new Size(390, 499); //مخفي
                picArrow.Image = Properties.Resources.down;
                spLine.Location = new Point(17, 158);

                isShow = false;
            }
            else
            {
                this.Size = new Size(390, 685); //ظاهر
                picArrow.Image = Properties.Resources.up;
                spLine.Location = new Point(17, 167);
                isShow = true;
            }


        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            if (!File.Exists("config.json"))
            {
                new DBConfig().Save(); // ينشئ ملف فاضي
            }

            this.Hide();
            FormInishialSettings formInishialSettings = new FormInishialSettings();
            formInishialSettings.ShowDialog();
            this.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmShowBackup formShowBackup = new frmShowBackup();
            formShowBackup.ShowDialog();
        }

        private void lblForgetPass_Click(object sender, EventArgs e)
        {
            try
            {
                string targetUsername = txtUser.Text; // ✏️ غيّر الاسم هنا للمستخدم اللي عاوز تتأكد منه
                bool userExists = false;

                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();
                    string qry = "SELECT COUNT(*) FROM users WHERE uername = @username";
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@username", targetUsername);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        userExists = count > 0;
                    }
                }

                if (!userExists)
                {
                    Notifier.ShowNotification("⚠️ تنبيه", $"المستخدم '{targetUsername}' غير موجود في النظام!");
                    return;
                }

                // ✅ لو المستخدم موجود → افتح شاشة التحقق من المالك
                using (frmBlackout frmblackout = new frmBlackout(this))
                {
                    frmblackout.Show();

                    using (frmOwnerUnlock frm = new frmOwnerUnlock())
                    {
                        DialogResult result = frm.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            frmResetUserPassword frmResetUserPassword = new frmResetUserPassword();
                            frmResetUserPassword.userName = txtUser.Text;
                            frmResetUserPassword.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ أثناء التحقق من المستخدم:\n" + ex.Message);
            }
        }

    }
}
