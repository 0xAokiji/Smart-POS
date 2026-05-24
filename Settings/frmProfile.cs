using DevExpress.Drawing;
using Guna.UI2.WinForms;
using pos.SystemApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace pos.Settings
{
    public partial class frmProfile : Form
    {
        //-> Dark Mode
        private Color backgroundPrmary = Color.FromArgb(32, 32, 32);
        private Color backgroundseconder = Color.FromArgb(38, 38, 38);
        private Color textColor = Color.FromArgb(204, 204, 204);
        private Color checkedFillColor = Color.FromArgb(1, 95, 95);
        private Color checkedForColor = Color.FromArgb(2, 2, 2);

        private bool passMach = false;
        private Byte[] imageByteArray;
        private frmAppSetting mainForm;
        public frmProfile(frmAppSetting frm)
        {
            InitializeComponent();
            mainForm = frm;

            ThemRefresh();
        }

        public void ThemRefresh()
        {
            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // معالجة صورة المستخدم
                System.Drawing.Image temp = new Bitmap(userImage.Image);
                MemoryStream ms = new MemoryStream();
                temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageByteArray = ms.ToArray();

                string qryUsers = @"UPDATE [dbo].[users] 
                            SET [userImage] = @image
                            WHERE [staffID] = @staffID";

                string qryStaff = @"UPDATE [dbo].[staff]
                            SET [sName] = @sName,
                                [sPhone] = @sPhone
                            WHERE [staffID] = @staffID";

                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    // تحديث جدول users
                    using (SqlCommand cmdUsers = new SqlCommand(qryUsers, con))
                    {
                        cmdUsers.Parameters.AddWithValue("@staffID", MainClass.UID);
                        cmdUsers.Parameters.AddWithValue("@image", imageByteArray);
                        cmdUsers.ExecuteNonQuery();
                    }

                    // تحديث جدول staff
                    using (SqlCommand cmdStaff = new SqlCommand(qryStaff, con))
                    {
                        cmdStaff.Parameters.AddWithValue("@staffID", MainClass.UID);
                        cmdStaff.Parameters.AddWithValue("@sName", txtName.Text);
                        cmdStaff.Parameters.AddWithValue("@sPhone", txtPhone.Text.Replace(" ", ""));
                        cmdStaff.ExecuteNonQuery();
                    }
                }

                // بعد الحفظ بنجاح
                MainClass.imageBytes = imageByteArray;
                MainClass.user = txtName.Text;
                MainClass.userphone = txtPhone.Text.Replace(" ", "");
                mainForm.updateImage();

                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                messageBox.Show("حدث خطأ أثناء حفظ البيانات", "خطأ");
            }
        }


        private void frmProfile_Load(object sender, EventArgs e)
        {
            if (MainClass.IMAGEBYTES != null)
            {
                using (MemoryStream stream = new MemoryStream(MainClass.IMAGEBYTES))
                {

                    userImage.Image = Image.FromStream(stream);
                }
            }
            imageByteArray = MainClass.IMAGEBYTES;

            txtName.Text = MainClass.USER;
            txtPhone.Text = MainClass.USERPHONE;
        }
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
            passPanel.FillColor = backgroundseconder;
            userPanel.FillColor = backgroundseconder;

            mainPanel.BackColor = backgroundPrmary;

            //-> Text Box 
            txtOldPass.BackColor = backgroundseconder;
            txtOldPass.ForeColor = textColor;
            txtOldPass.BorderColor = checkedFillColor;
            txtOldPass.FillColor = backgroundPrmary;
            txtOldPass.IconRight = Properties.Resources.showPass_light;

            txtPass.BackColor = backgroundseconder;
            txtPass.ForeColor = textColor;
            txtPass.BorderColor = checkedFillColor;
            txtPass.FillColor = backgroundPrmary;
            txtPass.IconRight = Properties.Resources.showPass_light;

            txtRepass.BackColor = backgroundseconder;
            txtRepass.ForeColor = textColor;
            txtRepass.BorderColor = checkedFillColor;
            txtRepass.FillColor = backgroundPrmary;
            txtRepass.IconRight = Properties.Resources.showPass_light;

            txtName.BackColor = backgroundseconder;
            txtName.ForeColor = textColor;
            txtName.BorderColor = checkedFillColor;
            txtName.FillColor = backgroundPrmary;

            txtPhone.BackColor = backgroundseconder;
            txtPhone.ForeColor = textColor;
            txtPhone.BorderColor = checkedFillColor;
            txtPhone.FillColor = backgroundPrmary;

            //-> Button
            btnChoseImage.BackColor = backgroundPrmary;
            btnChoseImage.ForeColor = textColor;
            btnChoseImage.FillColor = checkedFillColor;

            btnSave.BackColor = backgroundseconder;
            btnSave.ForeColor = textColor;
            btnSave.FillColor = checkedFillColor;

            btnSavePass.BackColor = backgroundseconder;
            btnSavePass.ForeColor = textColor;
            btnSavePass.FillColor = checkedFillColor;

        }

        private void txtOldPass_IconRightClick(object sender, EventArgs e)
        {
            txtOldPass.PasswordChar = '\0';
            txtOldPass.UseSystemPasswordChar = !txtOldPass.UseSystemPasswordChar;

            if (MainClass.ThemeMode == "dark")
            {
                txtOldPass.IconRight = txtOldPass.UseSystemPasswordChar
                        ? Properties.Resources.showPass_light
                        : Properties.Resources.showpassNo_light;
            }
            else
            {
                txtOldPass.IconRight = txtOldPass.UseSystemPasswordChar
                        ? Properties.Resources.showpass_dark
                        : Properties.Resources.showpassNo_dark;
            }
        }
        private void checkAllInput()
        {
            if (!string.IsNullOrWhiteSpace(txtPass.Text) && !string.IsNullOrWhiteSpace(txtOldPass.Text)
                    && !string.IsNullOrWhiteSpace(txtRepass.Text) && passMach)
            {
                btnSavePass.Enabled = true;
                if (MainClass.ThemeMode == "dark")
                {
                    btnSavePass.FillColor = checkedFillColor;
                    btnSavePass.BackColor = backgroundseconder;
                }
                else
                {
                    btnSavePass.FillColor = Color.FromArgb(136, 214, 218);
                    btnSavePass.BackColor = Color.FromArgb(243, 243, 243);
                }
            }
            else
            {
                btnSavePass.Enabled = false;
                btnSavePass.FillColor = Color.DimGray;
            }

        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private string FormatAsPhoneNumber(string input)
        {
            if (input.Length <= 3)
                return input;
            else if (input.Length <= 6)
                return input.Insert(3, " ");
            else if (input.Length <= 10)
                return input.Insert(3, " ").Insert(7, " ");
            else
                return input.Substring(0, 11).Insert(3, " ").Insert(7, " ");
        }
        private void txtPass_IconRightClick(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '\0';
            txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;

            if (MainClass.ThemeMode == "dark")
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

            if (MainClass.ThemeMode == "dark")
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

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                checkAllInput();
                txtPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtPass.Text[0];

            if (IsArabic(firstChar))
                txtPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            else
                txtPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;

           
            if (txtPass.Text.Length < 8)
            {
                lblPassMach.Visible = true;
                lblPassMach.Text = "كلمة المرور يجب أن تكون 8 أحرف على الأقل";
                lblPassMach.ForeColor = Color.Red;
                lblPassMach.Location = new Point(txtPass.Right - lblPassMach.PreferredWidth, txtPass.Bottom + 5 );

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
                txtRepass.Location = new Point(43, 105);
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
                txtRepass.Location = new Point(43, 105);
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

        private void txtOldPass_TextChanged(object sender, EventArgs e)
        {
            checkAllInput();

            if (string.IsNullOrWhiteSpace(txtOldPass.Text))
            {
                txtOldPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtOldPass.Text[0];

            if (IsArabic(firstChar))
                txtOldPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            else
                txtOldPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
           
        }

        private void txtRepass_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtRepass.Text))
            {
                checkAllInput();

                txtRepass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtRepass.Text[0];

            if (IsArabic(firstChar))
                txtRepass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            else
                txtRepass.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;

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
                txtRepass.Location = new Point(43, 105);
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
                txtRepass.Location = new Point(43, 105);
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

        private void userDataInputCheck()
        {
            bool isImageChanged = false;

            if (imageByteArray != null && MainClass.IMAGEBYTES != null)
            {
                isImageChanged = !imageByteArray.SequenceEqual(MainClass.IMAGEBYTES);
            }
            else if (imageByteArray != null || MainClass.IMAGEBYTES != null)
            {
                isImageChanged = true;
            }

            if (!string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (!(txtName.Text == MainClass.USER) || !(txtPhone.Text.Replace(" ", "") == MainClass.USERPHONE) || isImageChanged)
                {
                    btnSave.Enabled = true;
                    if (MainClass.ThemeMode == "dark")
                    {
                        btnSave.FillColor = checkedFillColor;
                        btnSave.BackColor = backgroundseconder;
                    }
                    else
                    {
                        btnSave.FillColor = Color.FromArgb(136, 214, 218);
                        btnSave.BackColor = Color.FromArgb(243, 243, 243);
                    }
                }
                else
                {
                    btnSave.Enabled = false;
                    btnSave.FillColor = Color.DimGray;
                }
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.FillColor = Color.DimGray;
            }
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            userDataInputCheck();

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtName.Text[0];

            if (IsArabic(firstChar))
                txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            else
                txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            userDataInputCheck();

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                txtPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                return;

            }
            else
                txtPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;

            Guna.UI2.WinForms.Guna2TextBox txt = sender as Guna.UI2.WinForms.Guna2TextBox;

            string numbersOnly = new string(txt.Text.Where(char.IsDigit).ToArray());

            int selectionStart = txt.SelectionStart;

            string formatted = FormatAsPhoneNumber(numbersOnly);

            txt.Text = formatted;

            txt.SelectionStart = txt.Text.Length;

           
        }

        private void btnSavePass_Click(object sender, EventArgs e)
        {
            if (!MainClass.ChangePass)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            try
            {
                // نجيب بيانات المستخدم الحالي من الجدول
                string qryGet = "SELECT userID, uername, uPass, staffID FROM users WHERE staffID = @staffID";
                DataTable dt = new DataTable();

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qryGet, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", MainClass.UID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    if (dt.Rows.Count == 0)
                    {
                        messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                        messageBox.Show("المستخدم غير موجود", "خطأ");
                        return;
                    }

                    string storedHash = dt.Rows[0]["uPass"].ToString();
                    int userId = Convert.ToInt32(dt.Rows[0]["userID"]);
                    string username = dt.Rows[0]["uername"].ToString();
                    int staffId = Convert.ToInt32(dt.Rows[0]["staffID"]);

                    // ✅ تحقق من كلمة المرور القديمة
                    if (!MainClass.VerifyPassword(txtOldPass.Text, storedHash))
                    {
                        messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                        messageBox.Show("كلمة المرور غير صحيحة", "تنبيه");
                        return;
                    }

                    // ✅ كلمة المرور الجديدة
                    string newPasswordHash = MainClass.HashPassword(txtPass.Text);

                    // 📌 حساب توقيع جديد بنفس القيم الأصلية
                    string signature = UserSigner.ComputeSignature(userId, username, newPasswordHash, staffId);

                    // ✅ تحديث
                    string qryUpdate = @"UPDATE [dbo].[users] 
                                 SET [uPass] = @uPass, [Signature] = @Signature 
                                 WHERE [userID] = @userID";

                    using (SqlCommand cmd2 = new SqlCommand(qryUpdate, con))
                    {
                        con.Open();
                        cmd2.Parameters.AddWithValue("@uPass", newPasswordHash);
                        cmd2.Parameters.AddWithValue("@Signature", signature);
                        cmd2.Parameters.AddWithValue("@userID", userId);

                        cmd2.ExecuteNonQuery();
                    }
                }

                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Show("تم تحديث كلمة المرور بنجاح", "نجاح");

                txtOldPass.Clear();
                txtPass.Clear();
                txtRepass.Clear();
            }
            catch (Exception ex)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                messageBox.Show("حدث خطأ أثناء تحديث كلمة المرور\n" + ex.Message, "خطأ");
            }
        }




        string filePath;
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|*.png;*.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                userImage.Image = new Bitmap(filePath);

                // معالجة صورة المستخدم
                System.Drawing.Image temp = new Bitmap(userImage.Image);
                MemoryStream ms = new MemoryStream();
                temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageByteArray = ms.ToArray();
                userDataInputCheck();
            }
        }
    }
}
