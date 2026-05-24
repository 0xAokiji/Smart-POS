using DevExpress.Pdf.Xmp;
using DevExpress.XtraEditors;
using pos.SystemApp;
using pos.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace pos.Settings
{
    public partial class frmAddPersone : Form
    {
        //-> Dark Mode
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color checkedFillColor;
        private Color checkedForColor;

        public int Id;
        frmAppSetting mainForm;

        public frmAddPersone(frmAppSetting frm, int id)
        {
            InitializeComponent();

            Id = id;
            mainForm = frm;

            ThemRefresh();

            ThemeMode();
            textSuggester();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtRole.Text = string.Empty;

            mainForm.frmStaffBack();
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
            string qry = string.Empty;
            Hashtable ht = new Hashtable();
            if (Id == 0)
            {
                qry = "Insert into staff Values (@Name ,@phone,@role, @salary, @advance)";
                ht.Add("@Name", txtName.Text);
                ht.Add("@phone", txtPhone.Text.Replace(" ", ""));
                ht.Add("@role", txtRole.Text);
                ht.Add("@salary", int.Parse(txtSalary.Text == string.Empty ? "0" : txtSalary.Text));
                ht.Add("@advance", 0);

            }
            else
            {
                qry = "Update staff Set sName = @Name ,sPhone = @phone ,sRole = @role, sSalary = @salary where staffID = @id";

                ht.Add("@Name", txtName.Text);
                ht.Add("@phone", txtPhone.Text.Replace(" ", ""));
                ht.Add("@role", txtRole.Text);
                ht.Add("@salary", int.Parse(txtSalary.Text == string.Empty ? "0" : txtSalary.Text));
                ht.Add("@id", Id);

            }

            if (MainClass.SQL(qry, ht) > 0) ;

            if (chbMulti.Checked == true)
            {
                txtName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtRole.Text = string.Empty;
            }
            else
            {
                txtName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtRole.Text = string.Empty;
                mainForm.frmStaffBack();

            }

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

            //-> Panel
            secondPanel.FillColor = backgroundseconder;
            mainPanel.BackColor = backgroundPrmary;

            //-> Text Box
            txtName.ForeColor = backgroundPrmary;
            txtName.ForeColor = textColor;
            txtName.BorderColor = checkedFillColor;
            txtName.FillColor = backgroundPrmary;

            txtPhone.ForeColor = backgroundPrmary;
            txtPhone.ForeColor = textColor;
            txtPhone.BorderColor = checkedFillColor;
            txtPhone.FillColor = backgroundPrmary;

            txtRole.ForeColor = backgroundPrmary;
            txtRole.ForeColor = textColor;
            txtRole.BorderColor = checkedFillColor;
            txtRole.FillColor = backgroundPrmary;

            //-> Button
            btnSave.BackColor = backgroundseconder;
            btnSave.FillColor = checkedFillColor;
            btnSave.ForeColor = textColor;

            btnBack.BackColor = backgroundseconder;
            btnBack.FillColor = Color.Red;
            btnBack.ForeColor = textColor;

            //-> Check Box
            chbMulti.ForeColor = textColor;
            chbMulti.CheckedState.FillColor = checkedFillColor;

        }

        private void frmAddPersone_Load(object sender, EventArgs e)
        {
            if (Id > 0)
            {
                string qry = "SELECT sName, sPhone, sRole FROM staff WHERE staffID = @id";

                try
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(qry, con))
                        {
                            command.Parameters.AddWithValue("@id", Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtName.Text = reader["sName"].ToString();
                                    txtPhone.Text = reader["sPhone"].ToString();
                                    txtRole.Text = reader["sRole"].ToString();
                                    btnSave.Enabled = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            chekNullInput();

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.TextAlign = HorizontalAlignment.Right;
                return;

            }

            char firstChar = txtName.Text[0];

            if (IsArabic(firstChar))
                txtName.TextAlign = HorizontalAlignment.Right;
            else
                txtName.TextAlign = HorizontalAlignment.Left;

        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }

        private void txtRole_TextChanged(object sender, EventArgs e)
        {
            chekNullInput();

            if (string.IsNullOrWhiteSpace(txtRole.Text))
            {
                txtRole.TextAlign = HorizontalAlignment.Right;
                return;

            }

            char firstChar = txtRole.Text[0];

            if (IsArabic(firstChar))
                txtRole.TextAlign = HorizontalAlignment.Right;
            else
                txtRole.TextAlign = HorizontalAlignment.Left;

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            chekNullInput();

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                txtPhone.TextAlign = HorizontalAlignment.Right;
                return;

            }
            else
                txtPhone.TextAlign = HorizontalAlignment.Left;

            Guna.UI2.WinForms.Guna2TextBox txt = sender as Guna.UI2.WinForms.Guna2TextBox;

            string numbersOnly = new string(txt.Text.Where(char.IsDigit).ToArray());

            int selectionStart = txt.SelectionStart;

            string formatted = FormatAsPhoneNumber(numbersOnly);

            txt.Text = formatted;

            txt.SelectionStart = txt.Text.Length;

        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
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
        private void chekNullInput()
        {
            btnSave.Enabled =
                !string.IsNullOrWhiteSpace(txtName.Text) &&
                !string.IsNullOrWhiteSpace(txtPhone.Text) &&
                !string.IsNullOrWhiteSpace(txtRole.Text) &&
                !string.IsNullOrWhiteSpace(txtSalary.Text);

        }

        private void textSuggester()
        {
            string qry = @"SELECT DISTINCT sRole FROM staff";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                DataTable dt2 = new DataTable();

                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);
                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        dataSource.Add(dt2.Rows[i][0].ToString());
                    }
                    this.txtRole.AutoCompleteCustomSource = dataSource;
                }
            }

            this.txtRole.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtRole.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtRole.RightToLeft = System.Windows.Forms.RightToLeft.No;
        }


        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {
            chekNullInput();

        }

        private void cbTechnical_CheckedChanged(object sender, EventArgs e)
        {
            if(cbTechnical.Checked == true) 
                txtRole.Text = "فني";
        }
    }
}
