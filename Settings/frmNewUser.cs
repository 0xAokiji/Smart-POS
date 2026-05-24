using DevExpress.DataAccess.Sql;
using DevExpress.Drawing;
using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using pos.Classes;
using pos.Model;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace pos.AccountManagement
{
    public partial class frmNewUser : Form
    {
        //-> Dark Mode
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color checkedFillColor;
        private Color checkedForColor;

        private bool DarkState;
        private bool passMach = false;
        private string filePath;
        private Byte[] imageByteArray;
        private bool newTime = false;
        int staffID = 0;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public frmNewUser()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            ThemRefresh();
        }
        public frmNewUser(bool newUser)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Size = new Size(480, 563);
            namePanel.Visible = true;
            CenterLabelInPanel(namePanel,lblComName);
            ThemRefresh();
            newTime = true;
        }
        private static void CenterLabelInPanel(Panel panel, Label label)
        {
            if (panel == null || label == null) return;

            // حساب موقع منتصف البانل
            int x = (panel.Width - label.Width) / 2;
            int y = (panel.Height - label.Height) / 2;

            label.Location = new Point(x, y);
        }
        public void ThemRefresh()
        {
            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();
        }
        private void MyForm_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {

            if (InputLanguage.CurrentInputLanguage.Culture.TwoLetterISOLanguageName == "ar")
            {

                txtPass.RightToLeft = RightToLeft.Yes;
                txtRepass.RightToLeft = RightToLeft.Yes;
                txtUser.RightToLeft = RightToLeft.Yes;

            }
            else
            {

                txtPass.RightToLeft = RightToLeft.No;
                txtRepass.RightToLeft = RightToLeft.No;
                txtUser.RightToLeft = RightToLeft.No;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // تجهيز صورة المستخدم
                System.Drawing.Image temp = new Bitmap(userImage.Image);
                MemoryStream ms = new MemoryStream();
                temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageByteArray = ms.ToArray();

                try
                {
                    // فحص إذا كان اسم المستخدم موجود
                    string qry = @"SELECT [uername] FROM users";

                    DataTable dt = new DataTable();
                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    bool isUsernameExists = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["uername"].ToString() == txtUser.Text)
                        {
                            isUsernameExists = true;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(txtUser.Text) ||
                        string.IsNullOrEmpty(txtPass.Text) ||
                        string.IsNullOrEmpty(txtRepass.Text) ||
                        comboBoxUser.SelectedItem == null)
                    {
                        return;
                    }

                    if (isUsernameExists)
                    {
                        MessageBox.Show("⚠️ اسم المستخدم موجود بالفعل", "تنبيه",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (txtPass.Text != txtRepass.Text)
                        {
                            txtRepass.Text = null;
                            txtPass.Text = null;
                            txtPass.Focus();
                            return;
                        }
                        else
                        {
                            try
                            {
                                int newId = 0;
                                string passwordHash = MainClass.HashPassword(txtPass.Text);

                                using (SqlConnection con = MainClass.GetConnection())
                                {
                                    con.Open();

                                    // أول إدخال بدون التوقيع
                                    string insertQry = @"
                            INSERT INTO [dbo].[users] ([uername], [uPass], [staffID], [userImage], [Signature])  
                            VALUES (@uername, @uPass, @staffID, @image, '') ;
                            SELECT SCOPE_IDENTITY();";

                                    using (SqlCommand cmd2 = new SqlCommand(insertQry, con))
                                    {
                                        cmd2.Parameters.AddWithValue("@uername", txtUser.Text);
                                        cmd2.Parameters.AddWithValue("@uPass", passwordHash);
                                        cmd2.Parameters.AddWithValue("@staffID", staffID);
                                        cmd2.Parameters.AddWithValue("@image", imageByteArray);

                                        object result = cmd2.ExecuteScalar();
                                        if (result != null)
                                            newId = Convert.ToInt32(result);
                                    }

                                    // حساب التوقيع الآن باستخدام UserSigner
                                    string signature = UserSigner.ComputeSignature(newId, txtUser.Text, passwordHash, staffID);

                                    // تحديث العمود Signature
                                    string updateSigQry = "UPDATE [dbo].[users] SET [Signature] = @Signature WHERE [userID] = @userID";
                                    using (SqlCommand cmd3 = new SqlCommand(updateSigQry, con))
                                    {
                                        cmd3.Parameters.AddWithValue("@Signature", signature);
                                        cmd3.Parameters.AddWithValue("@userID", newId);
                                        cmd3.ExecuteNonQuery();
                                    }
                                }

                                // تنظيف الحقول
                                txtUser.Text = string.Empty;
                                txtPass.Text = string.Empty;
                                txtRepass.Text = string.Empty;
                                comboBoxUser.SelectedIndex = -1;
                                userImage.Image = Properties.Resources.user;

                                Notifier.ShowNotification("نجاح", "✅ تم إضافة المستخدم بنجاح");

                                if (newTime)
                                {
                                    SetResetPassword("SmartPOS_Reset@2025");
                                    frmUserPermissions frmUserPermissions = new frmUserPermissions();
                                    frmUserPermissions.FirstTime = true;
                                    frmUserPermissions.ShowDialog();
                                    this.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("❌ حدث خطأ أثناء الإضافة\n" + ex.Message,
                                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                txtPass.Text = null;
                                txtRepass.Text = null;
                                txtUser.Text = null;
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ خطأ أثناء التحقق من المستخدمين\n" + ex.Message,
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {

                return;
            }
        }

        public static void SetResetPassword(string password)
        {
            string hash = HashPassword(password);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // التحقق هل الجدول يحتوي على صفوف
                string checkQry = "SELECT COUNT(*) FROM settings";
                int rowCount;

                using (SqlCommand checkCmd = new SqlCommand(checkQry, con))
                {
                    rowCount = (int)checkCmd.ExecuteScalar();
                }

                if (rowCount == 0)
                {
                    // ✅ الجدول فاضي → السماح بالإدخال اليدوي لقيمة setID
                    using (SqlCommand enableCmd = new SqlCommand("SET IDENTITY_INSERT settings ON;", con))
                    {
                        enableCmd.ExecuteNonQuery();
                    }

                    string insertQry = @"
                INSERT INTO settings (setID, backupPath, resetPasswordHash, themMode)
                VALUES (1, @backupPath, @hash, @theme)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQry, con))
                    {
                        insertCmd.Parameters.AddWithValue("@hash", hash);
                        insertCmd.Parameters.AddWithValue("@theme", "Light");
                        insertCmd.Parameters.AddWithValue("@backupPath", "Not Found");

                        int rows = insertCmd.ExecuteNonQuery();

                        if (rows > 0)
                            Notifier.ShowNotification("Done ✔", "✅ تم إنشاء إعداد جديد وحفظ الباسورد بنجاح!");
                        else
                            Notifier.ShowNotification("Error ❌", "⚠️ لم يتم إدخال أي صف جديد.");
                    }

                    // ❌ مهم جدًا ترجع الإعداد الافتراضي
                    using (SqlCommand disableCmd = new SqlCommand("SET IDENTITY_INSERT settings OFF;", con))
                    {
                        disableCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // 🔁 الصف موجود → نحدث الباسورد فقط
                    string updateQry = "UPDATE settings SET ResetPasswordHash = @hash";

                    using (SqlCommand updateCmd = new SqlCommand(updateQry, con))
                    {
                        updateCmd.Parameters.AddWithValue("@hash", hash);
                        int rows = updateCmd.ExecuteNonQuery();

                        if (rows > 0)
                            Notifier.ShowNotification("Done ✔", "✅ تم تحديث باسورد الـ Reset بنجاح!");
                        else
                            Notifier.ShowNotification("Error ❌", "⚠️ لم يتم تحديث أي صف في الجدول.");
                    }
                }
            }
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
       
        private void frmNewUser_Load(object sender, EventArgs e)
        {
            string qry_Tec = @"SELECT staffID AS id, sName as name FROM staff WHERE staffID NOT IN (SELECT staffID FROM users)";
            MainClass.CBFill(qry_Tec, comboBoxUser);

            txtPass.UseSystemPasswordChar = true;

        }

        // Parameters (يمكن تعديلها لكن القيم هذي آمنة افتراضياً)

        private void comboBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUser.SelectedValue != null && comboBoxUser.SelectedIndex != -1)
            {
                try
                {
                    staffID = Convert.ToInt32(comboBoxUser.SelectedValue);
                    checkAllInput();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء تحويل القيمة: " + ex.Message);
                }
            }
        }

        //-> Dark Mode
        private void LightMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(243, 243, 243);
            backgroundseconder = Color.FromArgb(230, 230, 230);
            textColor = Color.FromArgb(51, 51, 51);
            checkedFillColor = Color.FromArgb(136, 214, 218);
            checkedForColor = Color.FromArgb(250, 250, 20);
        }
        private void DarkMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.FromArgb(2, 2, 2);
        }
        private void ThemeMode()
        {
            this.BackColor = backgroundPrmary;
            this.ForeColor = textColor;
            lblPassMach.ForeColor = Color.Red;
            lblStaff.ForeColor = textColor;

            userPanel.FillColor = backgroundseconder;


            //-> ComboBox
            comboBoxUser.BackColor = backgroundseconder;
            comboBoxUser.ForeColor = textColor;
            comboBoxUser.BorderColor = checkedFillColor;
            comboBoxUser.FillColor = backgroundPrmary;

            //-> Text Box
            txtUser.ForeColor = backgroundPrmary;
            txtUser.ForeColor = textColor;
            txtUser.BorderColor = checkedFillColor;
            txtUser.FillColor = backgroundPrmary;

            txtPass.ForeColor = backgroundPrmary;
            txtPass.ForeColor = textColor;
            txtPass.BorderColor = checkedFillColor;
            txtPass.FillColor = backgroundPrmary;
            txtPass.IconRight = Properties.Resources.showPass_light;

            txtRepass.ForeColor = backgroundPrmary;
            txtRepass.ForeColor = textColor;
            txtRepass.BorderColor = checkedFillColor;
            txtRepass.FillColor = backgroundPrmary;
            txtRepass.IconRight = Properties.Resources.showPass_light;

            //-> Button
            btnChoseImage.BackColor = backgroundseconder;
            btnChoseImage.ForeColor = textColor;
            btnChoseImage.FillColor = checkedFillColor;

            btnSave.BackColor = backgroundseconder;
            btnSave.ForeColor = textColor;
            btnSave.FillColor = checkedFillColor;


        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
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
            checkAllInput();
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                txtPass.TextAlign = HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtPass.Text[0];

            if (IsArabic(firstChar))
                txtPass.TextAlign = HorizontalAlignment.Right;
            else
                txtPass.TextAlign = HorizontalAlignment.Left;

            if (txtPass.Text.Length < 8)
            {
                lblPassMach.Visible = true;
                lblPassMach.Text = "كلمة المرور يجب أن تكون 8 أحرف على الأقل";
                lblPassMach.ForeColor = Color.Red;
                lblPassMach.Location = new Point(txtPass.Right - lblPassMach.PreferredWidth, txtPass.Bottom + 5);

                txtPass.HoverState.BorderColor = Color.Red;
                txtPass.FocusedState.BorderColor = Color.Red;
                txtPass.BorderColor = Color.Red;

                txtRepass.Location = new Point(txtPass.Left, lblPassMach.Bottom + 5);

                txtRepass.HoverState.BorderColor = Color.Red;
                txtRepass.FocusedState.BorderColor = Color.Red;
                txtRepass.BorderColor = Color.Red;

                passMach = false;
            }
            else if (txtRepass.Text != txtPass.Text)
            {
                txtRepass.Location = new Point(17, 246);
                txtPass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.BorderColor = checkedFillColor;

                lblPassMach.Visible = true;
                lblPassMach.Text = "كلمة المرور غير متطابقة";
                lblPassMach.ForeColor = Color.Red;
                lblPassMach.Location = new Point(txtRepass.Right - lblPassMach.PreferredWidth, txtRepass.Bottom + 5);

                txtRepass.HoverState.BorderColor = Color.Red;
                txtRepass.FocusedState.BorderColor = Color.Red;
                txtRepass.BorderColor = Color.Red;

                passMach = false;
            }
            else
            {
                txtRepass.Location = new Point(17, 246);
                txtPass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.BorderColor = checkedFillColor;

                lblPassMach.Visible = true;
                lblPassMach.ForeColor = Color.Green;
                lblPassMach.Text = "✓ كلمة المرور متطابقة";
                lblPassMach.Location = new Point(txtRepass.Right - lblPassMach.PreferredWidth, txtRepass.Bottom + 5);

                txtRepass.HoverState.BorderColor = Color.Green;
                txtRepass.FocusedState.BorderColor = Color.Green;
                txtRepass.BorderColor = Color.Green;

                passMach = true;
            }
            checkAllInput();
        }

        private void txtRepass_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRepass.Text))
            {
                txtRepass.TextAlign = HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtRepass.Text[0];

            if (IsArabic(firstChar))
                txtRepass.TextAlign = HorizontalAlignment.Right;
            else
                txtRepass.TextAlign = HorizontalAlignment.Left;

            if (txtPass.Text.Length < 8)
            {
                lblPassMach.Visible = true;
                lblPassMach.Text = "كلمة المرور يجب أن تكون 8 أحرف على الأقل";
                lblPassMach.ForeColor = Color.Red;
                lblPassMach.Location = new Point(txtPass.Right - lblPassMach.PreferredWidth, txtPass.Bottom + 5);

                txtPass.HoverState.BorderColor = Color.Red;
                txtPass.FocusedState.BorderColor = Color.Red;
                txtPass.BorderColor = Color.Red;

                txtRepass.Location = new Point(txtPass.Left, lblPassMach.Bottom + 5);

                txtRepass.HoverState.BorderColor = Color.Red;
                txtRepass.FocusedState.BorderColor = Color.Red;
                txtRepass.BorderColor = Color.Red;

                passMach = false;

                txtRepass.Text = string.Empty;
                txtPass.Focus();
            }
            else if (txtRepass.Text != txtPass.Text)
            {
                txtRepass.Location = new Point(17, 246);
                txtPass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.BorderColor = checkedFillColor;

                lblPassMach.Visible = true;
                lblPassMach.Text = "كلمة المرور غير متطابقة";
                lblPassMach.ForeColor = Color.Red;
                lblPassMach.Location = new Point(txtRepass.Right - lblPassMach.PreferredWidth, txtRepass.Bottom + 5);

                passMach = false;
            }
            else
            {
                txtRepass.Location = new Point(17, 246);
                txtPass.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                txtPass.BorderColor = checkedFillColor;

                lblPassMach.Visible = true;
                lblPassMach.ForeColor = Color.Green;
                lblPassMach.Text = "✓ كلمة المرور متطابقة";
                lblPassMach.Location = new Point(txtRepass.Right - lblPassMach.PreferredWidth, txtRepass.Bottom + 5);

                txtRepass.HoverState.BorderColor = Color.Green;
                txtRepass.FocusedState.BorderColor = Color.Green;
                txtRepass.BorderColor = Color.Green;

                passMach = true;

            }
            checkAllInput();

        }

        private void txtPass_IconRightClick(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '\0';
            txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;

            if (DarkState)
            {
                txtPass.IconRight = txtPass.UseSystemPasswordChar
                        ? Properties.Resources.showPass_light
                        : Properties.Resources.showpassNo_light;
            }
            else
            {
                txtPass.IconRight = txtPass.UseSystemPasswordChar
                        ? Properties.Resources.showpass_dark
                        : Properties.Resources.showpassNo_dark;
            }

        }

        private void txtRepass_IconRightClick(object sender, EventArgs e)
        {
            txtRepass.PasswordChar = '\0';
            txtRepass.UseSystemPasswordChar = !txtRepass.UseSystemPasswordChar;

            if (DarkState)
            {
                txtRepass.IconRight = txtRepass.UseSystemPasswordChar
                        ? Properties.Resources.showPass_light
                        : Properties.Resources.showpassNo_light;
            }
            else
            {
                txtRepass.IconRight = txtRepass.UseSystemPasswordChar
                        ? Properties.Resources.showpass_dark
                        : Properties.Resources.showpassNo_dark;
            }
        }

        private void checkAllInput()
        {
            if (comboBoxUser.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(txtPass.Text) && !string.IsNullOrWhiteSpace(txtPass.Text)
                    && !string.IsNullOrWhiteSpace(txtRepass.Text) && passMach)
            {
                btnSave.Enabled = true;
                if (DarkState)
                {
                    btnSave.FillColor = checkedFillColor;
                    btnSave.BackColor = backgroundseconder;
                }
                else
                {
                    btnSave.FillColor = Color.FromArgb(136, 214, 218);
                    btnSave.BackColor = Color.FromArgb(230, 230, 230);
                }
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.FillColor = Color.DimGray;
            }

        }

        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|*.png;*.jpg"; // تصحيح تصفية الصور
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                userImage.Image = new Bitmap(filePath); // عرض الصورة في PictureBox
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
