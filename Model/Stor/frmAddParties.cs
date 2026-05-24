using pos.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace pos.Model.Stor
{
    public partial class frmAddParties : Form
    {
        public int pID = 0;
        private bool isUpdate = false;
        public string partyType = string.Empty;
        public frmAddParties()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;       // WS_EX_TOOLWINDOW - لجعل الفورم لا يظهر في شريط المهام
                return cp;
            }
        }

        private void frmAddCastomer_Load(object sender, EventArgs e)
        {
            if (partyType == "عميل")
            {
                rbCustomer.Checked = true;
                txtCode.Enabled = false;
            }
            else if (partyType == "مورد")
            {
                rbSupliser.Checked = true;
                txtCode.Enabled = true;
            }

            loadPartyNames();
            loadDataFromUpdate();

        }
        private void loadDataFromUpdate()
        {
            if (pID != 0)
            {
                isUpdate = true;

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand("Select * from Parties where pID = @pID", con))
                {
                    cmd.Parameters.AddWithValue("@pID", pID);

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtName.Text = dr["pName"].ToString();
                            txtPhone1.Text = dr["pPhone"].ToString();
                            txtPhone2.Text = dr["pPhone2"] == DBNull.Value ? string.Empty : dr["pPhone2"].ToString();
                            txtAdderess.Text = dr["pAdderss"] == DBNull.Value ? string.Empty : dr["pAdderss"].ToString();
                            LoadDataForUpdate(dr["supCode"] == DBNull.Value ? string.Empty : dr["supCode"].ToString());

                            if (dr["PartyType"].ToString() == "عميل")
                            {
                                rbCustomer.Checked = true;
                                rbSupliser.Checked = false;
                            }
                            else if (dr["PartyType"].ToString() == "مورد")
                            {
                                rbSupliser.Checked = true;
                                rbCustomer.Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string qry;
                if (pID == 0)
                {
                    qry = @"Insert into Parties Values (@name, @phone, @phone2, @address, @PartyType, @supCode)";
                }
                else
                {
                    qry = @"Update Parties 
                    Set 
                        pName = @name,
                        pPhone = @phone, 
                        pPhone2 = @phone2, 
                        pAdderss = @address, 
                        PartyType = @PartyType,
                        supCode = @supCode 
                    where pID = @pID";
                }

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@pID", pID);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone1.Text);
                    cmd.Parameters.AddWithValue("@phone2", string.IsNullOrWhiteSpace(txtPhone2.Text) ? DBNull.Value : (object)txtPhone2.Text);
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtAdderess.Text) ? DBNull.Value : (object)txtAdderess.Text);

                    if (rbCustomer.Checked)
                        partyType = "عميل";
                    else if (rbSupliser.Checked)
                        partyType = "مورد";

                    cmd.Parameters.AddWithValue("@PartyType", partyType);
                    cmd.Parameters.AddWithValue("@supCode", string.IsNullOrWhiteSpace(txtCode.Text) ? DBNull.Value : (object)txtCode.Text);

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                notify.Icon = SystemIcons.Information; // أو أيقونة مخصصة
                notify.Visible = true;
                notify.BalloonTipTitle = "عملية الحفظ";
                notify.BalloonTipText = "تم الحفظ بنجاح";
                notify.ShowBalloonTip(3000); // مدة الإظهار بالمللي ثانية
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء الحفظ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Close();
        }


        private void rbCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCustomer.Checked)
            {
                rbSupliser.Checked = false;
                txtCode.Enabled = false;

            }

            else
            {
                rbSupliser.Checked = true;
                txtCode.Enabled = true;

            }
        }

        private void rbSupliser_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSupliser.Checked)
            {
                rbCustomer.Checked = false;
                txtCode.Enabled = true;
            }
            else
            {
                rbCustomer.Checked = true;
                txtCode.Enabled = false;
            }
        }
        private List<string> partyNames = new List<string>();
        private List<string> supCode = new List<string>();

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                lblWarning.Text = "⚠️ الاسم فارغ";

                lblWarning.ForeColor = Color.Red;
                btn_Save.Enabled = false;

                txtName.HoverState.BorderColor = Color.Red;
                txtName.FocusedState.BorderColor = Color.Red;
                txtName.BorderColor = Color.Red;

                btn_Save.Enabled = false;
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAdderess.Text) || string.IsNullOrWhiteSpace(txtPhone1.Text))
            {
                btn_Save.Enabled = false;
            }
            else
            {
                btn_Save.Enabled = true;
            }

            string currentText = txtName.Text.Trim();

            if (partyNames.Any(n => n.Equals(currentText, StringComparison.OrdinalIgnoreCase)))
            {
                if(isUpdate && currentText.Equals(txtName.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    // لو بنحدث و الاسم مش متغير، ما تعملش حاجة
                    lblWarning.Visible = false;
                    btn_Save.Enabled = true;
                    txtName.HoverState.BorderColor = Color.Green;
                    txtName.FocusedState.BorderColor = Color.Green;
                    txtName.BorderColor = Color.Green;
                    return;
                }
                lblWarning.Text = "⚠️ الاسم موجود بالفعل";

                lblWarning.ForeColor = Color.Red;
                btn_Save.Enabled = false;

                txtName.HoverState.BorderColor = Color.Red;
                txtName.FocusedState.BorderColor = Color.Red;
                txtName.BorderColor = Color.Red;
            }
            else
            {
                lblWarning.Visible = true;
                lblWarning.Text = "✅ هذا الاسم متاح"; // يمسح التحذير لو الاسم مش مكرر
                lblWarning.ForeColor = Color.Green;

                txtName.HoverState.BorderColor = Color.Green;
                txtName.FocusedState.BorderColor = Color.Green;
                txtName.BorderColor = Color.Green;

            }
            lblWarning.Location = new Point(txtName.Right - lblWarning.PreferredWidth, txtName.Bottom + 5);


        }
        private void loadPartyNames()
        {
            partyNames.Clear();
            supCode.Clear(); // ✅ عدلت دي لأنك كنت بتعمل Clear مرتين لـ partyNames

            string query = "SELECT pName, supCode FROM Parties";

            using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدم GetConnection
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            partyNames.Add(reader["pName"].ToString().Trim());
                            supCode.Add(reader["supCode"].ToString().Trim());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Notifier.ShowNotification("خطأ", "❌ حدث خطأ أثناء تحميل الأسماء");
                }
            }
        }

        private void txtPhone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string oldCode = string.Empty;

        private void LoadDataForUpdate(string code)
        {
            oldCode = code;
            txtCode.Text = code;
            isUpdate = true;
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtCode.Text.Trim();

            // لو بتعمل تحديث والكود ما اتغيرش
            if (isUpdate && currentText.Equals(oldCode, StringComparison.OrdinalIgnoreCase))
            {
                lblWarning2.Visible = false;
                btn_Save.Enabled = true;

                txtCode.HoverState.BorderColor = Color.Green;
                txtCode.FocusedState.BorderColor = Color.Green;
                txtCode.BorderColor = Color.Green;
                return;
            }

            lblWarning2.Visible = true;

            if (supCode.Any(n => n.Equals(currentText, StringComparison.OrdinalIgnoreCase)))
            {
                // الكود موجود ومش هو القديم
                lblWarning2.Text = "⚠️ هذا الكود موجود بالفعل";
                lblWarning2.ForeColor = Color.Red;
                btn_Save.Enabled = false;

                txtCode.HoverState.BorderColor = Color.Red;
                txtCode.FocusedState.BorderColor = Color.Red;
                txtCode.BorderColor = Color.Red;
            }
            else
            {
                // الكود متاح
                lblWarning2.Text = "✅ هذا الكود متاح";
                lblWarning2.ForeColor = Color.Green;
                btn_Save.Enabled = true;

                txtCode.HoverState.BorderColor = Color.Green;
                txtCode.FocusedState.BorderColor = Color.Green;
                txtCode.BorderColor = Color.Green;
            }

            lblWarning2.Location = new Point(txtCode.Right - lblWarning2.PreferredWidth, txtCode.Bottom + 5);
        }


    }
}
