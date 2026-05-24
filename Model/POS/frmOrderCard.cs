using pos.Model.Stor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Model.POS
{
    public partial class frmOrderCard : Form
    {
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        public int selectedPartyID = 0;
        public string partyType = "";

        public frmOrderCard()
        {
            InitializeComponent();
            textSuggester();
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
        private void btnAddParties_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.partyType = partyType;

                frm.ShowDialog();

            }
            this.Show();
            this.Focus();
            textSuggester(); // Initialize text suggester for party names
        }

        private void btnEditParties_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.pID = selectedPartyID; // Pass the selected party ID to the form
                frm.partyType = partyType;
                frm.ShowDialog();

            }
            this.Show();
            this.Focus();
        }

        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartyType", "%" + partyType + "%");

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);
                    foreach (DataRow row in dt2.Rows)
                    {
                        string name = row["pName"].ToString();
                        int id = Convert.ToInt32(row["pID"]);
                        dataSource.Add(name);
                        nameToID[name] = id;
                    }
                }

                txtName.AutoCompleteCustomSource = dataSource;
                txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtName.Text))
            {
                selectedPartyID = nameToID[txtName.Text];
            }
            else
            {
                selectedPartyID = 0;
            }

            if (selectedPartyID > 0)
            {
                string qry = @"SELECT pPhone, pAdderss FROM Parties WHERE pID = @pID";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@pID", selectedPartyID);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtPhone.Text = reader["pPhone"].ToString();
                            txtAddress.Text = reader["pAdderss"] != DBNull.Value ? reader["pAdderss"].ToString() : string.Empty;

                            btnEditParties.Enabled = true;

                            txtName.HoverState.BorderColor = Color.FromArgb(136, 214, 218);
                            txtName.FocusedState.BorderColor = Color.FromArgb(136, 214, 218);
                            txtName.BorderColor = Color.FromArgb(136, 214, 218);
                        }
                    }
                }
            }
            else if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.TextAlign = HorizontalAlignment.Right;

                txtPhone.Text = string.Empty;
                txtAddress.Text = string.Empty;

                btnEditParties.Enabled = false;

                txtName.HoverState.BorderColor = Color.FromArgb(136, 214, 218);
                txtName.FocusedState.BorderColor = Color.FromArgb(136, 214, 218);
                txtName.BorderColor = Color.FromArgb(136, 214, 218);

                return;
            }
            else
            {
                txtPhone.Text = string.Empty;
                txtPhone.PlaceholderText = "بيانات الاتصال فارغة";

                txtAddress.Text = string.Empty;
                txtAddress.PlaceholderText = "العنوان فارغ";

                txtName.HoverState.BorderColor = Color.Red;
                txtName.FocusedState.BorderColor = Color.Red;
                txtName.BorderColor = Color.Red;
            }

            if (!string.IsNullOrEmpty(txtName.Text))
            {
                char firstChar = txtName.Text[0];

                if (IsArabic(firstChar))
                    txtName.TextAlign = HorizontalAlignment.Right;
                else
                    txtName.TextAlign = HorizontalAlignment.Left;
            }
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }

        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btn_Save_Click(object sender, EventArgs e)
        {
            await MainClass.PrintOrderCardAsync(0, selectedPartyID, cbBreakable.Checked);

        }

        private async void btnShow_Click(object sender, EventArgs e)
        {
            await MainClass.PrintOrderCardAsync(0, selectedPartyID, cbBreakable.Checked, true);

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmPartesSearch frm = new frmPartesSearch(this);
            frm.ShowDialog();
            this.Focus();
            this.Show();
        }
        public void resultSearch(string pName)
        {
            txtName.Text = pName;
        }
    }
}
