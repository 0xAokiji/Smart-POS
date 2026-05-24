using DevExpress.XtraMap.ItemEditor;
using DevExpress.XtraRichEdit.Utils;
using Guna.UI2.WinForms;
using pos.Classes;
using pos.GeneralForms;
using pos.View;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace pos.Model
{
    public partial class frmCloseSafe : Form
    {
        //Fields
        private float BorderRadius = 8f;
        private float BorderSize = 2f;
        private Color borderColor = Color.FromArgb(136, 214, 218);



        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedFillColor2;
        private Color checkedForColor;

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        private decimal billTotal;
        private decimal billTotalAll;
        private decimal purchaseTotal;
        private decimal clearTotal;
        private decimal previousTotal;
        private decimal comingTotal;
        private decimal comingTotalAll;
        private decimal returnTotalCustomer;
        private decimal returnTotalSuplieser;
        private decimal advanceTotal;
        private decimal salaryTotal;
        private decimal residualTotal;
        private decimal returnTotalCustomerOutShift;
        private decimal returnTotalSupplierOutShift;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        frmPOS fpos;
        public frmCloseSafe(frmPOS pos)
        {
            InitializeComponent();

            this.ShowInTaskbar = false;

            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

            fpos = pos;
            textSuggester();

            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();

        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCloseSafe_Load(object sender, EventArgs e)
        {

            getData();

            txtTotla.Text = SafeDecimal(billTotal, "N0");
            txtPrice.Text = SafeDecimal(billTotalAll, "N0");
            txtPruchase.Text = SafeDecimal(purchaseTotal, "N0");
            txtClear.Text = SafeDecimal(clearTotal, "N0");
            txtprevious.Text = SafeDecimal(previousTotal, "N0");
            txtRetuns.Text = SafeDecimal(returnTotalCustomer, "N0");
            txtRetunsSuplieser.Text = SafeDecimal(returnTotalSuplieser, "N0");
            txtadvance.Text = SafeDecimal(advanceTotal, "N0");
            txtSalary.Text = SafeDecimal(salaryTotal, "N0");
            txtComing.Text = SafeDecimal(comingTotal, "N0");
            txtComingTotal.Text = SafeDecimal(comingTotalAll, "N0");
            txtResidual.Text = SafeDecimal(residualTotal, "N0");

        }
        private string SafeDecimal(object value, string format = "N0")
        {
            if (value == DBNull.Value) return "0";
            decimal d = Convert.ToDecimal(value);

            var nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = ","  // هنا تحدد الفاصل بنفسك
            };

            return d.ToString("N0", nfi);
        }

        private void getData()
        {
            txtExstName.Text = MainClass.USER;

            string qry = @"SELECT Amount FROM shifts WHERE ID = (SELECT MAX(ID) FROM shifts WHERE ID < (SELECT MAX(ID) FROM shifts));
                 SELECT 
                     SUM(ISNULL(m.PaidAmount, 0)) AS PaidAmount,
                     SUM(ISNULL(m.priceClear, 0)) AS priceClear,
                     SUM(ISNULL(m.change, 0)) AS CreditBalance,

                     ISNULL((
                         SELECT SUM(ISNULL(d.priceAfterDes, 0))
                         FROM tblDetails d
                         WHERE d.DeleteFlag = 1 AND d.shiftDoUpdate = s.ID
                     ), 0) AS returnTotalCustomerInShift,

                     ISNULL((
                         SELECT SUM(ISNULL(d.priceAfterDes, 0))
                         FROM tblDetails d
                         INNER JOIN tblMain1 mm ON d.MainID = mm.MainID
                         WHERE d.DeleteFlag = 1 
                           AND d.shiftDoUpdate = s.ID
                           AND mm.shiftID <> s.ID
                     ), 0) AS returnTotalCustomerOutShift

                 FROM shifts s
                 LEFT JOIN tblMain1 m ON s.ID = m.shiftID 
                 WHERE s.ID = @shiftID 
                   AND (m.DeleteFlag IS NULL OR m.DeleteFlag = 0) 
                 GROUP BY s.ID, s.staffID, s.startTime, s.endTime;

            SELECT 
             SUM(ISNULL(b.clear, 0)) AS TotalSales, 
             SUM(ISNULL(b.PaidAmount, 0)) AS PaidAmount,
             SUM(ISNULL(b.priceClear, 0)) AS priceClear,
             SUM(ISNULL(b.clear, 0)) - SUM(ISNULL(b.PaidAmount, 0)) AS NetTotal,

             ISNULL((
                 SELECT SUM(ISNULL(d.priceAfterDes, 0))
                 FROM tblDetailsSupliser d
                 WHERE d.DeleteFlag = 1 AND d.shiftDoUpdate = s.ID
             ), 0) AS returnTotalSupplierInShift,

             ISNULL((
                 SELECT SUM(ISNULL(d.priceAfterDes, 0))
                 FROM tblDetailsSupliser d
                 INNER JOIN billPrcheses bb ON d.billPrchesesID = bb.bID
                 WHERE d.DeleteFlag = 1
                   AND d.shiftDoUpdate = s.ID
                   AND bb.shiftID <> s.ID
             ), 0) AS returnTotalSupplierOutShift

         FROM shifts s
         LEFT JOIN billPrcheses b ON s.ID = b.shiftID 
         WHERE s.ID = @shiftID 
           AND (b.DeleteFlag IS NULL OR b.DeleteFlag = 0) 
         GROUP BY s.ID, s.staffID, s.startTime, s.endTime;";

            string qry2 = @"SELECT m.MainID,d.proID,d.qty,m.status, m.total
                     FROM tblMain1 m 
                     INNER JOIN tblDetails d ON m.MainID = d.MainID 
                     INNER JOIN products p ON p.pID = d.proID
                     WHERE m.status LIKE N'%غير مكتمل%'";

            string qry3 = @"SELECT m.MainID, m.status, m.total, m.shiftID
                     FROM tblMain1 m
                     WHERE m.status LIKE N'%معلق%' AND m.shiftID = @shiftID";

            string qry4 = @"SELECT SUM(a.Amount) AS AdvanceTotal FROM Advances a WHERE a.shiftID = @shiftID;
                    SELECT SUM(s.Amount) AS SalaryTotal FROM Salaries s WHERE s.shiftID = @shiftID;";

            string qry5 = @"SELECT SUM(price) AS TotalPrice FROM purchases WHERE shiftID = @shiftID";


            // 🔹 Query 1
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    // Table 0 → previousTotal
                    previousTotal = (ds.Tables[0].Rows.Count > 0)
                        ? Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"])
                        : 0;

                    // Table 1 → المبيعات
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        billTotalAll = Convert.ToDecimal(ds.Tables[1].Rows[0]["priceClear"]);
                        billTotal = Convert.ToDecimal(ds.Tables[1].Rows[0]["PaidAmount"]);
                        residualTotal = Convert.ToDecimal(ds.Tables[1].Rows[0]["CreditBalance"]);
                        returnTotalCustomer = Convert.ToDecimal(ds.Tables[1].Rows[0]["returnTotalCustomerInShift"]);
                        returnTotalCustomerOutShift = Convert.ToDecimal(ds.Tables[1].Rows[0]["returnTotalCustomerOutShift"]);
                    }

                    // Table 2 → المشتريات
                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        comingTotalAll = Convert.ToDecimal(ds.Tables[2].Rows[0]["TotalSales"]);
                        comingTotal = Convert.ToDecimal(ds.Tables[2].Rows[0]["PaidAmount"]);
                        returnTotalSuplieser = Convert.ToDecimal(ds.Tables[2].Rows[0]["returnTotalSupplierInShift"]);
                        returnTotalSupplierOutShift = Convert.ToDecimal(ds.Tables[2].Rows[0]["returnTotalSupplierOutShift"]);
                    }
                }
            }

            // 🔹 Query 2
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd2 = new SqlCommand(qry2, con))
            using (SqlDataAdapter da2 = new SqlDataAdapter(cmd2))
            {
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                btnUnComplet.Text = dt2.Rows.Count.ToString();
            }

            // 🔹 Query 3
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd3 = new SqlCommand(qry3, con))
            {
                cmd3.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                using (SqlDataAdapter da3 = new SqlDataAdapter(cmd3))
                {
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    btnHold.Text = dt3.Rows.Count.ToString();
                }
            }

            // 🔹 Query 4
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd4 = new SqlCommand(qry4, con))
            {
                cmd4.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                using (SqlDataAdapter da4 = new SqlDataAdapter(cmd4))
                {
                    DataSet ds4 = new DataSet();
                    da4.Fill(ds4);

                    if (ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0 && ds4.Tables[0].Rows[0]["AdvanceTotal"] != DBNull.Value)
                        advanceTotal = Convert.ToDecimal(ds4.Tables[0].Rows[0]["AdvanceTotal"]);

                    if (ds4.Tables.Count > 1 && ds4.Tables[1].Rows.Count > 0 && ds4.Tables[1].Rows[0]["SalaryTotal"] != DBNull.Value)
                        salaryTotal = Convert.ToDecimal(ds4.Tables[1].Rows[0]["SalaryTotal"]);
                }
            }

            // 🔹 Enable/Disable btnRecive
            btnRecive.Enabled = (btnUnComplet.Text == "0" && btnHold.Text == "0");

            // 🔹 Query 5
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd5 = new SqlCommand(qry5, con))
            {
                cmd5.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                using (SqlDataAdapter da5 = new SqlDataAdapter(cmd5))
                {
                    DataTable dt5 = new DataTable();
                    da5.Fill(dt5);

                    if (dt5.Rows.Count > 0 && dt5.Rows[0]["TotalPrice"] != DBNull.Value)
                        purchaseTotal = Convert.ToDecimal(dt5.Rows[0]["TotalPrice"]);
                }
            }

            clearTotal = (billTotal + returnTotalSupplierOutShift)
                         - (purchaseTotal + comingTotal + salaryTotal + advanceTotal + returnTotalCustomerOutShift);
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(btnHold.Text) != 0 || Convert.ToInt32(btnUnComplet.Text) != 0)
                {
                    guna2MessageDialog1.Show("لا يمكن المتابعة، توجد فواتير غير مكتملة أو معلقة.");

                    return;
                }
                else
                {
                    Hashtable ht = new Hashtable();
                    string qry = @"UPDATE shifts SET endTime = @endTime, Amount = @billTotal, endDate = @endDate WHERE ID = (SELECT MAX(ID) FROM shifts)";
                    ht.Add("@endTime", Convert.ToString(DateTime.Now.ToShortTimeString()));
                    ht.Add("@billTotal", billTotal);
                    ht.Add("@endDate", DateTime.Now.Date);
                    MainClass.SQL(qry, ht);


                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "تنبيه";
                    notifyIcon1.BalloonTipText = "تم تسليم الوردية بنجاح";
                    notifyIcon1.ShowBalloonTip(2000);

                    //this.Close();
                }

                btnCansel.Enabled = false;
                nextUserPanel.Enabled = true;
                userPanel.Enabled = false;
                billsPanel.Enabled = false;
            }
            catch
            {
                guna2MessageDialog1.Show("حدث خطأ");

                return;

            }
        }



        private void btnHold_Click(object sender, EventArgs e)
        {

            frmBillList frm = new frmBillList();
            frm.btnHold.Visible = false;
            frm.btnEnd.Visible = false;
            frm.btnUnCom.Visible = false;
            frm.enter = "hold";
            frm.ShowDialog();
            this.Focus();
            getData();
        }

        private void btnUnComplet_Click(object sender, EventArgs e)
        {

            frmBillList frm = new frmBillList();
            frm.btnHold.Visible = false;
            frm.btnEnd.Visible = false;
            frm.btnUnCom.Visible = false;
            frm.enter = "un";
            frm.ShowDialog();
            this.Focus();
            getData();
        }

        private void txtExstName_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtExstName.Text, @"\p{IsArabic}"))
            {
                txtExstName.RightToLeft = RightToLeft.Yes;
            }
            else
            {
                txtExstName.RightToLeft = RightToLeft.No;
            }
        }

        private void txtNextName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNextName.Text))
            {
                txtNextName.TextAlign = HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtNextName.Text[0];

            if (IsArabic(firstChar))
                txtNextName.TextAlign = HorizontalAlignment.Right;
            else
                txtNextName.TextAlign = HorizontalAlignment.Left;
        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (MainClass.IsvalidUser(txtNextName.Text, txtPass.Text) == false)
            {
                guna2MessageDialog1.Show("اسم المستخدم او كلمة المرور غير صحح");
                txtPass.Focus();
                txtPass.SelectAll();
                return;
            }

            this.Close();
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void textSuggester()
        {
            string qry = @"SELECT uername FROM users";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                DataTable dt2 = new DataTable();

                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);
                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

                    foreach (DataRow row in dt2.Rows)
                    {
                        dataSource.Add(row["uername"].ToString());
                    }

                    txtNextName.AutoCompleteCustomSource = dataSource;
                }
            }

            txtNextName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtNextName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtNextName.RightToLeft = System.Windows.Forms.RightToLeft.No;
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            float r = BorderRadius;
            float d = r * 2;
            Rectangle rect = this.ClientRectangle;

            using (var pen = new Pen(borderColor, BorderSize))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // يمين
                e.Graphics.DrawLine(pen, rect.Right - BorderSize / 2, rect.Y, rect.Right - BorderSize / 2, rect.Bottom - r);

                // يسار
                e.Graphics.DrawLine(pen, rect.X + BorderSize / 2, rect.Bottom - r, rect.X + BorderSize / 2, rect.Y);
            }
        }

        private void LightMode()
        {
            backgroundPrmary = Color.FromArgb(243, 243, 243);
            backgroundseconder = Color.FromArgb(230, 230, 230);
            textColor2 = Color.White;
            textColor = Color.FromArgb(51, 51, 51);
            checkedFillColor2 = Color.FromArgb(136, 214, 218);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.FromArgb(250, 250, 20);

            //imgIcon.Image = Properties.Resources.money_light1;
            imgIcon.Image = Properties.Resources.money_dark1;

        }
        private void DarkMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor2 = Color.White;
            textColor = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedFillColor2 = Color.FromArgb(136, 214, 218);
            checkedForColor = Color.FromArgb(2, 2, 2);
            borderColor = checkedFillColor;

            imgIcon.Image = Properties.Resources.money_dark1;

        }
        private void ThemeMode()
        {
            this.BackColor = backgroundPrmary;
            //Panels
            mainPanel.BackColor = backgroundPrmary;
            userPanel.FillColor = backgroundPrmary;
            nextUserPanel.FillColor = backgroundPrmary;
            billsPanel.FillColor = backgroundPrmary;
            bottomPanel.BackColor = backgroundseconder;
            topPanel.BackColor = checkedFillColor;

            //Lables
            lblTitel.ForeColor = textColor2;
            lblCurrentUser.ForeColor = textColor;
            lblCurrentBalance.ForeColor = textColor;
            lblPay.ForeColor = textColor;
            lblBills.ForeColor = textColor;
            lblHold.ForeColor = textColor;
            lblUncomplite.ForeColor = textColor;
            lblNextUser.ForeColor = textColor;
            lblCleanBalance.ForeColor = textColor;

            //Text box
            txtExstName.BackColor = backgroundPrmary;
            txtExstName.ForeColor = textColor;
            txtExstName.BorderColor = checkedFillColor;
            txtExstName.FillColor = backgroundPrmary;

            txtTotla.BackColor = backgroundPrmary;
            txtTotla.ForeColor = textColor;
            txtTotla.BorderColor = checkedFillColor;
            txtTotla.FillColor = backgroundPrmary;



            txtprevious.BackColor = backgroundPrmary;
            txtprevious.ForeColor = textColor;
            txtprevious.BorderColor = checkedFillColor;
            txtprevious.FillColor = backgroundPrmary;

            txtPruchase.BackColor = backgroundPrmary;
            txtPruchase.ForeColor = textColor;
            txtPruchase.BorderColor = checkedFillColor;
            txtPruchase.FillColor = backgroundPrmary;

            txtClear.BackColor = backgroundPrmary;
            txtClear.ForeColor = textColor;
            txtClear.BorderColor = checkedFillColor;
            txtClear.FillColor = backgroundPrmary;

            txtNextName.BackColor = backgroundPrmary;
            txtNextName.ForeColor = textColor;
            txtNextName.BorderColor = checkedFillColor;
            txtNextName.FillColor = backgroundPrmary;

            txtPass.BackColor = backgroundPrmary;
            txtPass.ForeColor = textColor;
            txtPass.BorderColor = checkedFillColor;
            txtPass.FillColor = backgroundPrmary;

            //Buttons
            btnlogin.FillColor = checkedFillColor;
            btnlogin.ForeColor = textColor2;

            btnExit.FillColor = Color.Red;
            btnExit.ForeColor = textColor2;

            btnHold.FillColor = checkedFillColor2;
            btnHold.ForeColor = textColor2;

            btnUnComplet.FillColor = checkedFillColor2;
            btnUnComplet.ForeColor = textColor2;

            btnRecive.FillColor = checkedFillColor;
            btnRecive.ForeColor = textColor2;

            btnCansel.FillColor = Color.Red;
            btnCansel.ForeColor = textColor2;


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
        }

        private void txtPass_IconRightClick(object sender, EventArgs e)
        {
            txtPass.PasswordChar = '\0';
            txtPass.UseSystemPasswordChar = !txtPass.UseSystemPasswordChar;

            txtPass.IconRight = txtPass.UseSystemPasswordChar
                         ? Properties.Resources.showpass_dark
                         : Properties.Resources.showpassNo_dark;
        }
    }
}
