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

namespace pos.SystemApp
{
    public partial class frmResetUserPassword : Form
    {
        private Color backgroundPrmary = Color.FromArgb(32, 32, 32);
        private Color backgroundseconder = Color.FromArgb(38, 38, 38);
        private Color textColor = Color.FromArgb(204, 204, 204);
        private Color checkedFillColor = Color.FromArgb(1, 95, 95);
        private Color checkedForColor = Color.FromArgb(2, 2, 2);
        private bool passMach = false;
        public string userName = "";

        public frmResetUserPassword()
        {
            InitializeComponent();
        }

        private void frmResetUserPassword_Load(object sender, EventArgs e)
        {

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
                txtRepass.Location = new Point(26, 59);
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
                txtRepass.Location = new Point(26, 59);
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
        private void checkAllInput()
        {
            if (!string.IsNullOrWhiteSpace(txtPass.Text)
                    && !string.IsNullOrWhiteSpace(txtRepass.Text) && passMach)
            {
                btnSavePass.Enabled = true;

            }
            else
            {
                btnSavePass.Enabled = false;
            }

        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
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
                txtRepass.Location = new Point(26, 59);
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
                txtRepass.Location = new Point(26, 59);
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

        private void btnSavePass_Click(object sender, EventArgs e)
        {
            try
            {
                // نجيب بيانات المستخدم الحالي من الجدول
                string qryGet = "SELECT userID, uername, uPass, staffID FROM users WHERE uername = @uername";
                DataTable dt = new DataTable();

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qryGet, con))
                {
                    cmd.Parameters.AddWithValue("@uername", userName);
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

                txtPass.Clear();
                txtRepass.Clear();
                this.Close();
            }
            catch (Exception ex)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                messageBox.Show("حدث خطأ أثناء تحديث كلمة المرور\n" + ex.Message, "خطأ");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
