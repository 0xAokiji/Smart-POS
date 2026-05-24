using DevExpress.XtraBars.ViewInfo;
using pos.Classes;
using pos.View;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;

namespace pos.SystemApp
{
    public partial class frmTrialTime : Form
    {
        private string key = "";
        public bool expired = false;
        public frmTrialTime()
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateLicenseAndRetrieveOldValues();
        }
        private void UpdateLicenseAndRetrieveOldValues()
        {
            try
            {
                guna2MessageDialog1.Caption = "تنبيه";
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Dark;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;

                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    // 🔹 أولًا استرجع أول صف موجود وحفظ كل القيم في متغيرات
                    int idOld = 0;
                    string keyCurrentOld = null;
                    string activationKeyOld = null;
                    string hashActivationKeyOld = null;
                    string publicKeyOld = null;
                    bool isActivatedOld = false;
                    string hashValueOld = null;

                    using (SqlCommand selectCmd = new SqlCommand("SELECT TOP 1 * FROM [LicenseInfo]", con))
                    using (SqlDataReader dr = selectCmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            idOld = Convert.ToInt32(dr["ID"]);
                            keyCurrentOld = dr["key_Current"]?.ToString();
                            activationKeyOld = dr["Activation_Key"]?.ToString();
                            hashActivationKeyOld = dr["Hash_Activation_Key"]?.ToString(); // العمود الجديد
                            publicKeyOld = dr["PK_key"]?.ToString();
                            isActivatedOld = dr["Is_Activated"] != DBNull.Value && Convert.ToBoolean(dr["Is_Activated"]);
                            hashValueOld = dr["hash_Value"]?.ToString();

                            // إذا عندك أعمدة أخرى في الجدول، ضيفها هنا بنفس الطريقة
                            // string anotherColumnOld = dr["AnotherColumn"]?.ToString();
                        }
                        else
                        {
                            MessageBox.Show("⚠️ لا يوجد أي صف في جدول LicenseInfo.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // 🔹 جهز القيم الجديدة للتحديث
                    string publicKey = publicKeyOld; // ممكن تستخدم القديم أو تولد واحد جديد لو تحب
                    bool isActivated = true;

                    // 🔒 تشفير المفتاح العشوائي بالمفتاح العام
                    //string encryptedCurrentKey = EncryptWithPublicKey(txtKey.Text, publicKey);

                    // 🔹 تحقق من المفتاح المدخل
                    if (!LicenseSigner.VerifyHashActivationKey(txtKey.Text, hashActivationKeyOld))
                    {
                        MessageBox.Show("المفتاح الذي أدخلته غير صحيح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 🔐 إنشاء توقيع للتحقق من سلامة البيانات
                    string hashValue = LicenseSigner.ComputeSignature(
                        keyCurrent: activationKeyOld,
                        activationKey: activationKeyOld,
                        hashActivationKey: hashActivationKeyOld, // الآن نمرر هذا العمود
                        publicKey: publicKey,
                        isActivated: isActivated
                    );

                    // 🔹 تنفيذ UPDATE
                    using (SqlCommand updateCmd = new SqlCommand(@"
                    UPDATE [LicenseInfo]
                    SET key_Current = @key_Current,
                        Activation_Key = @Activation_Key,
                        Hash_Activation_Key = @Hash_Activation_Key,  -- العمود الجديد
                        PK_key = @PK_key,
                        Is_Activated = @Is_Activated,
                        hash_Value = @hash_Value
                    WHERE ID = @ID", con))
                    {
                        updateCmd.Parameters.AddWithValue("@key_Current", activationKeyOld);
                        updateCmd.Parameters.AddWithValue("@Activation_Key", activationKeyOld);
                        updateCmd.Parameters.AddWithValue("@Hash_Activation_Key", hashActivationKeyOld); // العمود الجديد
                        updateCmd.Parameters.AddWithValue("@PK_key", publicKeyOld);
                        updateCmd.Parameters.AddWithValue("@Is_Activated", isActivated);
                        updateCmd.Parameters.AddWithValue("@hash_Value", hashValue);
                        updateCmd.Parameters.AddWithValue("@ID", idOld);

                        updateCmd.ExecuteNonQuery();
                    }


                    Notifier.ShowNotification("Done ✅", "تم تحديث بيانات الترخيص بنجاح.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء التحديث: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DateTime? GetTrialStartDate()
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 1 startDate FROM [trialTime]", con))
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string rawDate = result.ToString();


                        string decrypted = DecryptDate(rawDate);

                        // إزالة BOM إذا موجود
                        if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                            decrypted = decrypted.Substring(1);

                        // إزالة أي whitespace إضافية
                        // ✅ نفترض إن DecryptDate يرجع DateTime
                        return Convert.ToDateTime(decrypted.Trim());
                    }

                    return null;
                }
            }
            catch
            {
                return null;
            }
        }




        public static string DecryptDate(string encryptedDate)
        {
            byte[] key = KeyManager.GetOrCreateKey();
            string plainText = AesEncryption.Decrypt(encryptedDate, key);

            // ✅ يرجع التاريخ فقط بدون وقت
            return plainText;
        }

        private void frmTrialTime_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime? startDate = GetTrialStartDate();

                if (startDate.HasValue)
                {
                    DateTime currentDate = GetServerDate().Date;

                    int usedDays = (currentDate - startDate.Value.Date).Days;
                    int remainingDays = Math.Max(0, 30 - usedDays);

                    if (usedDays <= 30)
                    {
                        btn_cont.Text = $"استمرار ({remainingDays} يوم)";
                        btn_cont.Visible = true;
                        btnClose.Visible = false;
                    }
                    else
                    {
                        btnClose.Text = "انتهت النسخة التجريبية";
                        btn_cont.Visible = false;
                        btnClose.Visible = true;
                    }
                }
                //else
                //{
                //    Notifier.ShowNotification("خطأ", "حدث.");
                //}
                if (expired)
                    btn_cont.Enabled = false;
                else
                    btn_cont.Enabled = true;
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("حدث خطأ", "خطأ أثناء تحميل البيانات: " + ex.Message);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_cont_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string trialText()
        {
            return key;
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            // افصل الحدث مؤقتًا علشان مانعملش Loop
            txtKey.TextChanged -= txtKey_TextChanged;

            // احسب مكان المؤشر
            int cursorPosition = txtKey.SelectionStart;
            int digitsBeforeCursor = txtKey.Text.Take(cursorPosition).Count(char.IsLetterOrDigit);

            // شيل أي رمز مش حرف أو رقم وخليها UpperCase
            string input = new string(txtKey.Text.Where(char.IsLetterOrDigit).ToArray()).ToUpper();

            // حدد أقصى طول 16 حرف
            if (input.Length > 16)
                input = input.Substring(0, 16);

            // قسم النص لأربع مقاطع
            var parts = Enumerable.Range(0, (input.Length + 3) / 4)
                                  .Select(i => input.Substring(i * 4, Math.Min(4, input.Length - i * 4)));

            string formatted = string.Join("-", parts);

            // حدد مكان المؤشر الجديد
            int newCursorPosition = 0, lettersCounted = 0;
            foreach (char c in formatted)
            {
                if (lettersCounted == digitsBeforeCursor) break;
                newCursorPosition++;
                if (char.IsLetterOrDigit(c)) lettersCounted++;
            }

            // حط النص الجديد ورجع المؤشر
            txtKey.Text = formatted;
            txtKey.SelectionStart = newCursorPosition;

            // رجع الحدث
            txtKey.TextChanged += txtKey_TextChanged;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
