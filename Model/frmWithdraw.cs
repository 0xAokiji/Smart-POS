using pos.View;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;


namespace pos.Model
{
    public partial class frmWithdraw : SampleAdd
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public frmWithdraw()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            // تغيير خصائص النافذة لمنع ظهورها في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

            textSuggester_Name();
            textSuggester_Pruchase();
        }


        private void frmWithdraw_Load(object sender, EventArgs e)
        {
            

            txtName.Text = MainClass.USER;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Parent = (Form)this.TopLevelControl;

            try
            {
                string qry = @"SELECT SUM(ISNULL(m.total, 0)) AS Amount 
                    FROM shifts s 
                    LEFT JOIN tblMain1 m ON s.ID = m.shiftID 
                    WHERE s.ID = (SELECT MAX(ID) FROM shifts)
                    GROUP BY s.ID, s.staffID, s.startTime, s.endTime;";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt); // تنفيذ الاستعلام وملء DataTable بالنتائج

                    
                }

                //Hashtable ht = new Hashtable();
                //string qry1 = @"UPDATE shifts SET Amount = Amount - @IncrementValue WHERE ID = (SELECT MAX(ID) FROM shifts)";
                //ht.Add("@IncrementValue", int.Parse(price.Text));
                //MainClass.SQL(qry1, ht);

                Hashtable ht2 = new Hashtable();
                string qry2 = @"INSERT INTO purchases (shiftID, name, pname, price, amount, aTime, aDate) VALUES 
                                              (@shiftID,@name, @pname, @price, @amount, @aTime, @aDate)";
                ht2.Add("@shiftID", MainClass.shiftID);
                ht2.Add("@name", txtName.Text);
                ht2.Add("@pname", txtPurpose.Text);
                ht2.Add("@price", int.Parse(price.Text));
                ht2.Add("@amount", int.Parse(amount.Text));
                ht2.Add("@aTime", Convert.ToString(DateTime.Now.ToShortTimeString()));
                ht2.Add("@aDate", Convert.ToDateTime(DateTime.Now.Date));
                MainClass.SQL(qry2, ht2);

                this.Close();
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Show("حدث خطأ");
                return;
            }
        }


        private void price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }

        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void price_Click(object sender, EventArgs e)
        {
            price.SelectAll();

        }

        private void amount_Click(object sender, EventArgs e)
        {
            amount.SelectAll();
        }
        private void textSuggester_Pruchase()
        {
            string qry = @"SELECT pname FROM purchases";

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
                    this.txtPurpose.AutoCompleteCustomSource = dataSource;
                }
            }

            this.txtPurpose.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtPurpose.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtPurpose.RightToLeft = System.Windows.Forms.RightToLeft.No;
        }

        private void textSuggester_Name()
        {
            string qry = @"SELECT sName FROM staff";

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
                    this.txtName.AutoCompleteCustomSource = dataSource;
                }
            }

            this.txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No;
        }

    }
}
