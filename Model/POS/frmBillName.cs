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
    public partial class frmBillName : Form
    {
        private int mainID = 0;
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        public int selectedPartyID = 0;
        public frmBillName(int id)
        {
            InitializeComponent();
            textSuggester(); // Initialize text suggester for party names
            mainID = id;
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
        private void frmBillName_Load(object sender, EventArgs e)
        {

        }
        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartyType", "%عميل%");

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
                btnEditParties.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                btnEditParties.Enabled = false;
                btnSave.Enabled = false;

                selectedPartyID = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveBillCustomer();

            // 3️⃣ خلص → رجّع النتيجة وقفّل الفورم
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void saveBillCustomer()
        {
            try
            {
                if (mainID <= 0 || selectedPartyID <= 0)
                {
                    MessageBox.Show("برجاء اختيار فاتورة وعميل قبل الحفظ.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string qry = @"
                UPDATE tblMain1
                SET partiesID = @partiesID,
                    shiftID = @shiftID
                WHERE MainID = @ID";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = mainID;
                    cmd.Parameters.Add("@shiftID", SqlDbType.Int).Value = MainClass.shiftID;
                    cmd.Parameters.Add("@partiesID", SqlDbType.Int).Value = selectedPartyID;

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        MessageBox.Show("لم يتم تحديث أي فاتورة. تأكد من رقم الفاتورة.", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAddParties_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.partyType = "عميل";
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
                frm.partyType = "عميل";
                frm.ShowDialog();

            }
            this.Show();
            this.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
