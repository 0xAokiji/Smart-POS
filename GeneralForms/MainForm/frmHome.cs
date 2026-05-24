using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using pos.Analysis_Forms;
using pos.frmReports;
using pos.GeneralForms.MainForm;
using pos.Model;
using pos.View;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace pos
{
    public partial class frmHome : Form
    {
        public string place = "";
        public static frmHome Instance; // مرجع ثابت للفورم

        private int chabngForMe;
        private decimal totalBillCustomer;

        private int chabngForSupplieser;
        private decimal totalBillSupplieser;

        public frmHome()
        {
            InitializeComponent();
            Instance = this;

            this.ShowInTaskbar = false;

        }

        bool klick = false;
        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            try
            {
                place = "sales";
                frmRptDate frmRpt = new frmRptDate();
                frmRpt.ShowDialog();
                reports();
                klick = true;

                if (klick == true)
                {
                    string qry1 = @"SELECT * FROM [dateTime] WHERE ID = (SELECT MAX(ID) FROM [dateTime])";

                    using (SqlConnection con1 = MainClass.GetConnection())
                    {
                        con1.Open();
                        using (SqlCommand cmd1 = new SqlCommand(qry1, con1))
                        {
                            DataTable dt1 = new DataTable();
                            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                            da1.Fill(dt1);

                            if (dt1.Rows.Count > 0)
                            {
                                DateTime stDate = Convert.ToDateTime(dt1.Rows[0]["startDate"]);
                                DateTime endDate = Convert.ToDateTime(dt1.Rows[0]["endDate"]);

                                string qry = @"SELECT SUM(m.Total) AS TotalAmount 
                                       FROM tblMain1 m 
                                       INNER JOIN tblDetails d ON m.MainID = d.MainID
                                       INNER JOIN products p ON p.pID = d.proID
                                       WHERE m.aDate BETWEEN @StartDate AND @EndDate 
                                       AND m.status = @status";

                                using (SqlConnection con2 = MainClass.GetConnection())
                                {
                                    con2.Open();
                                    using (SqlCommand cmd = new SqlCommand(qry, con2))
                                    {
                                        cmd.Parameters.AddWithValue("@StartDate", stDate);
                                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                                        cmd.Parameters.AddWithValue("@status", "مدفوع");

                                        DataTable dt = new DataTable();
                                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                                        da.Fill(dt);

                                        if (dt.Rows.Count > 0 && dt.Columns.Contains("TotalAmount"))
                                        {
                                            object totalAmountObj = dt.Rows[0]["TotalAmount"];
                                            decimal totalAmount = totalAmountObj != DBNull.Value ? Convert.ToDecimal(totalAmountObj) : 0;

                                            // ممكن تضيف هنا أي كود للتعامل مع القيمة totalAmount
                                        }
                                        else
                                        {
                                            MessageBox.Show("لا توجد بيانات في الفترة المحددة");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تنفيذ العملية");
                return;
            }
        }

        public void reports()
        {
            rptPanel.Controls.Clear();

            if (place == "Purchese")
            {
                frmReportPurcheseView fR1 = new frmReportPurcheseView();
                fR1.Dock = DockStyle.Fill;
                fR1.TopLevel = false;
                rptPanel.Controls.Add(fR1);
                fR1.Show();
            }
            else if (place == "sales")
            {
                frmReportSalesView fR2 = new frmReportSalesView();
                fR2.Dock = DockStyle.Fill;
                fR2.TopLevel = false;
                rptPanel.Controls.Add(fR2);
                fR2.Show();
            }
            else if (place == "sheeft")
            {
                frmReportSheeftView fR3 = new frmReportSheeftView();
                fR3.Dock = DockStyle.Fill;
                fR3.TopLevel = false;
                rptPanel.Controls.Add(fR3);
                fR3.Show();
            }
        }



        private void frmHome_Leave(object sender, EventArgs e)
        {
            klick = false;

        }
        private void resduals()
        {
            string qry = @"
        SELECT 
            SUM(CASE WHEN p.PartyType = N'عميل' THEN r.currentDebitBalance ELSE 0 END) AS TotalForCustomer,
            SUM(CASE WHEN p.PartyType = N'مورد' THEN r.currentDebitBalance ELSE 0 END) AS TotalForSupplier
        FROM residualTable r
        JOIN Parties p ON r.PartiesID = p.pID;
    ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataRow row = dt2.Rows[0];

                        chabngForMe = row["TotalForCustomer"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalForCustomer"]))
                            : 0;

                        txtchangeForMe.Text = chabngForMe.ToString("N0");

                        chabngForSupplieser = row["TotalForSupplier"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalForSupplier"]))
                            : 0;

                        txtchangeForSupplieser.Text = chabngForSupplieser.ToString("N0");
                    }
                }
            }
        }

        private void showReportPaids()
        {
            string qry = @"
        SELECT 
            COUNT(*) AS TotalFinishedRows,
            SUM(TotalWithInterest) AS TotalWithInterestSum,
            SUM(CASE WHEN PaymentMethod = N'كاش' THEN 1 ELSE 0 END) AS CashCount,
            SUM(CASE WHEN PaymentMethod = N'اجل' THEN 1 ELSE 0 END) AS AglCount
        FROM tblMain1
        WHERE status = 'finshed';
    ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataRow row = dt2.Rows[0];

                        totalBillCustomer = row["TotalWithInterestSum"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalWithInterestSum"]))
                            : 0;

                        txtPaisAmountCustomer.Text = totalBillCustomer.ToString("N0");

                        txtPayBillNumer.Text = row["TotalFinishedRows"] != DBNull.Value
                            ? row["TotalFinishedRows"].ToString()
                            : "0";

                        txtCash.Text = row["CashCount"] != DBNull.Value
                            ? row["CashCount"].ToString()
                            : "0";

                        txtAglcount.Text = row["AglCount"] != DBNull.Value
                            ? row["AglCount"].ToString()
                            : "0";
                    }
                }
            }
        }

        private decimal GetTotalSalaries()
        {
            decimal totalSalaries = 0;

            string qry = @"
            SELECT SUM(SalaryAmount) AS TotalSalaries
            FROM Salaries;";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                con.Open();
                object result = cmd.ExecuteScalar();
                totalSalaries = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }

            return totalSalaries;
        }

        private void showReportPaisSuplieser()
        {
            string qry = @"
        SELECT 
            COUNT(*) AS TotalFinishedRows,
            SUM(clear) AS TotalWithInterestSum
        FROM billPrcheses
        WHERE billStatus = 'Finish';
    ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataRow row = dt2.Rows[0];

                        totalBillSupplieser = row["TotalWithInterestSum"] != DBNull.Value
                            ? Convert.ToInt32(Convert.ToDecimal(row["TotalWithInterestSum"]))
                            : 0;

                        txtPaisAmountSupplieser.Text = totalBillSupplieser.ToString("N0");

                        txtPayBillNumerSupplieser.Text = row["TotalFinishedRows"] != DBNull.Value
                            ? row["TotalFinishedRows"].ToString()
                            : "0";
                    }
                }
            }
        }

        private decimal purches()
        {
            decimal totalAmount = 0;

            string qry = @"
        SELECT SUM(ISNULL(price, 0)) AS Totalprice
        FROM purchases;
    ";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                con.Open();
                object result = cmd.ExecuteScalar();
                totalAmount = result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
            }

            return totalAmount;
        }

        private async void frmHome_Load(object sender, EventArgs e)
        {
            await LoadCompanyInfoAsync();

            resduals();
            showReportPaids();
            showReportPaisSuplieser();
            decimal totalSalaries = GetTotalSalaries();
            txtSalaries.Text = totalSalaries.ToString("N0");

            decimal pruches = purches();
            txtPurches.Text = pruches.ToString("N0");

            decimal totalExpenses = totalBillSupplieser + totalSalaries + pruches;
            txtTotalExpenses.Text = totalExpenses.ToString("N0");

            decimal profit = totalBillCustomer - totalExpenses;
            txtProfit.Text = profit.ToString("N0");
            txtPaid.Text = totalBillCustomer.ToString("N0");

        }
        private static void CenterLabelInPanel(Panel panel, Label label)
        {
            if (panel == null || label == null) return;

            // حساب موقع منتصف البانل
            int x = (panel.Width - label.Width) / 2;
            int y = (panel.Height - label.Height) / 2;

            label.Location = new Point(x, y);
        }

        private void guna2TileButton5_Click(object sender, EventArgs e)
        {

            AddControls(new frm_product_analysis());

        }

        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();

        public void AddControls(Form f)
        {
            // تأكد من إزالة أي نماذج أخرى من الـ Panel
            rptPanel.Controls.Clear();

            // إعداد النموذج الجديد بشكل آمن
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;
            f.AutoScaleMode = AutoScaleMode.None;

            rptPanel.Controls.Add(f);
            f.Show();
            f.BringToFront();

            // تخزين النموذج
            if (!openedForms.ContainsKey(f.Name))
                openedForms.Add(f.Name, f);
            else
                openedForms[f.Name] = f;
        }

        private void التقريرالماليالشاملToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ReportFinance)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            AddControls(new frmFinancialReports());

        }

        private void تتبعرصيدالطرافToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.PartiesBalance)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            AddControls(new frmPersonalReport());

        }

        private async void tsHome_Click(object sender, EventArgs e)
        {
            await LoadCompanyInfoAsync();

        }
        public async Task LoadCompanyInfoAsync()
        {
            rptPanel.Controls.Clear();
            rptPanel.Controls.Add(homePanel);

            await MainClass.LoadCompanyProfileAsync();

            // 🧩 تعبئة النصوص
            SetLabelTextAndVisibility(lblComName, namePanel, MainClass.CompanyName, "", true);
            SetLabelTextAndVisibility(lblComAddress, addressPanel, MainClass.CompanyAddress, "العنوان : ");
            SetLabelTextAndVisibility(lblComPhone1, phone1Panel, MainClass.Phone1, "رقم الهاتف : ");
            SetLabelTextAndVisibility(lblComPhone2, phone2Panel, MainClass.Phone2, "رقم الهاتف : ");

            // 🖼️ تعبئة الصور
            SetPictureFromBytes(picLogo, MainClass.CompanyLogo);
            SetPictureFromBytes(picBackground, MainClass.CompanyPic);

            // ✅ تحديث واجهة المستخدم
            rptPanel.Refresh();
            homePanel.Refresh();
            Application.DoEvents();
        }


        // 🧩 دالة مساعدة لتعبئة الليبلات
        private void SetLabelTextAndVisibility(Label lbl, Panel parentPanel, string value, string prefix = "", bool center = false)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                lbl.Text = prefix + value;
                lbl.Visible = true;
                parentPanel.Visible = true;

                // ✅ تحديث حجم الـ Label بناءً على النص
                lbl.AutoSize = true;

                // ✅ اجعل عرض البانل مساوي لعرض الليبل + هامش بسيط (اختياري)
                parentPanel.Width = lbl.Width + 70;

                // ✅ ضبط التمركز
                if (center)
                    CenterLabelInPanel(parentPanel, lbl);
                else
                    CenterLabelInPanel(parentPanel, lbl);
            }
            else
            {
                lbl.Visible = false;
                parentPanel.Visible = false;
            }
        }


        // 🖼️ دالة مساعدة لتعبئة الصور
        private void SetPictureFromBytes(PictureBox pic, byte[] imageBytes)
        {
            if (imageBytes != null && imageBytes.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                    pic.Image = Image.FromStream(ms);
                pic.Visible = true;
            }
            else
            {
                pic.Visible = false;
            }
        }
        private void CenterPanel(Panel inner, Panel outer)
        {
            inner.Left = (outer.Width - inner.Width) / 2;
            //inner.Top = (outer.Height - inner.Height) / 2;
        }
        private void frmHome_Resize(object sender, EventArgs e)
        {
            CenterPanel(profitPanel, homePanel);
        }
    }
}
