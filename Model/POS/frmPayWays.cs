using DevExpress.CodeParser;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.Sql;
using DevExpress.PivotGrid.OLAP.Mdx;
using DevExpress.Utils.Animation;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using pos.Classes;
using pos.Model.Stor;
using pos.Reports;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pos.Model.POS
{
    public partial class frmPayWays : Form
    {
        public int mainID = 0;
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        public int selectedPartyID = 0;
        private int benefitValue = 0;
        public string partyType = "";
        public decimal total = 0;
        public decimal totalClean = 0;
        public decimal discountValue = 0;
        private decimal CreditBalance = 0;
        private decimal previousBebitBalance;
        public string status = "new";
        private bool unknown = false;
        public string partyName = "";
        public string invoiceCode;
        private bool paritesIsFind = false;
        public bool fromRetuned = false;
        private decimal oldBalance;
        private decimal newBalance;
        private string personName;

        public bool isTaskBill = false;
        public frmPayWays()
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
        private void frmPayWays_Load(object sender, EventArgs e)
        {
            textSuggester(); // Initialize text suggester for party names

            txtPriceTotal.Text = (total).ToString();
            txtDV.Text = (discountValue).ToString();

            if(isTaskBill)
                txtClean.Text = totalClean.ToString();
            else
                txtClean.Text = (total - discountValue).ToString();

            txtName.Text = partyName;
            txtName.Focus();


            if (status == "update")
            {
                updateBillDataFromDB();

                if (fromRetuned)
                {
                    btnNext1.Enabled = true;
                    btnEdit1.Visible = false;
                    btnUnknow.Visible = false;
                    btnPremium.Enabled = true;
                    btnRsidual.Enabled = true;
                    gbPartiesData.Enabled = false;
                    gbPay.Enabled = false;
                    gbData.Enabled = false;

                }

                if (cbPayWay.SelectedIndex == 0 || cbPayWay.SelectedIndex == 2) // Assuming 0 is the index for "كاش"
                {
                    gbPartiesData.Enabled = true;
                    txtNumPremium.Visible = false;
                    lblNumPremium.Visible = false;
                    txtValuePremium.Visible = false;
                    lblValPremium.Visible = false;
                }
            }
            else
            {
                btnEdit1.Enabled = false;
                btnPremium.Enabled = false;
                btnRsidual.Enabled = false;
            }
        }
        private void cbPayWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidatePayment();

            if (cbPayWay.SelectedIndex == 0) // Assuming 0 is the index for "كاش"
            {

                gbPartiesData.Enabled = true;
                txtNumPremium.Visible = false;
                lblNumPremium.Visible = false;
                txtValuePremium.Visible = false;
                lblValPremium.Visible = false;

                txtBenefits.Enabled = false;

                txtCurrentDebitBalance.Text = Convert.ToString(previousBebitBalance);

            }
            else if (cbPayWay.SelectedIndex == 2)
            {
                gbPartiesData.Enabled = true;
                txtNumPremium.Visible = false;
                lblNumPremium.Visible = false;
                txtValuePremium.Visible = false;
                lblValPremium.Visible = false;

                txtBenefits.Enabled = true;


            }
            else
            {
                gbPartiesData.Enabled = false;
                txtNumPremium.Visible = true;
                lblNumPremium.Visible = true;
                txtValuePremium.Visible = true;
                lblValPremium.Visible = true;
                txtBenefits.Enabled = true;


            }
        }

        private async void btnPSave_Click(object sender, EventArgs e)
        {
            try
            {
                // خزن قيم UI في متغيرات محلية
                bool isClient = (partyType == "عميل");
                bool printInvoice = cbPrint.Checked;
                bool printOrderCard = cbPrintOrderCard.Checked;
                bool breakable = cbBreakable.Checked;
                int payWay = cbPayWay.SelectedIndex;
                int id = mainID;

                // 1️⃣ شغل منطق الحفظ مباشرة (بدون Threads)
                if (isClient)
                    saveBillCustomer();
                else
                    saveBillSupliser();

                if (payWay == 1)
                    PremiumBillCustomer();
                else if (payWay == 2)
                    residualBillCustomer();

                // 2️⃣ أي UI أو طباعة
                if (isClient)
                {
                    if (printInvoice)
                        await MainClass.PrintInvoiceAsync(id, true);

                    if (printOrderCard)
                        await MainClass.PrintOrderCardAsync(id, 0, breakable);
                }

                // 3️⃣ خلص → رجّع النتيجة وقفّل الفورم
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void saveBillCustomer()
        {
            try
            {
                string qry = @"
            UPDATE tblMain1
            SET partiesID = @partiesID,
                TotalWithInterest = @TotalWithInterest,
                shiftID = @shiftID,
                priceClear = @TotalWithInterest,
                InterestAmount = @InterestAmount,
                PaidAmount = @PaidAmount,
                CreditBalance = @CreditBalance,
                [status] = @status,
                change = @change,
                PaymentMethod = @PaymentMethod,
                previousDebitBalance = @previousDebitBalance,
                total = @total,
                descountValue = @descountValue,
                currentDebitBalance = @currentDebitBalance,
                InvoiceIssuanceValue = @TotalWithInterest,
                latePayTax = @latePayTax
            WHERE MainID = @ID";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@ID", mainID);
                    cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@partiesID", selectedPartyID);

                    if (isTaskBill)
                    {
                        double billPrice = Convert.ToDouble(string.IsNullOrWhiteSpace(txtTotalAfterBenefit.Text) ? "0" : txtTotalAfterBenefit.Text);
                        double task = Convert.ToDouble(string.IsNullOrWhiteSpace(txtBenefits.Text) ? "0" : txtBenefits.Text);
                        double total = billPrice - task;

                        cmd.Parameters.AddWithValue("@TotalWithInterest", total);

                    }
                    else
                        cmd.Parameters.AddWithValue("@TotalWithInterest", Convert.ToDouble(string.IsNullOrWhiteSpace(txtTotalAfterBenefit.Text) ? "0" : txtTotalAfterBenefit.Text));
                    cmd.Parameters.AddWithValue("@total", Convert.ToDouble(string.IsNullOrWhiteSpace(txtPriceTotal.Text) ? "0" : txtPriceTotal.Text));
                    cmd.Parameters.AddWithValue("@descountValue", Convert.ToDouble(string.IsNullOrWhiteSpace(txtDV.Text) ? "0" : txtDV.Text));

                    double change = Convert.ToDouble(string.IsNullOrWhiteSpace(txtChange.Text) ? "0" : txtChange.Text);
                    if (cbPayWay.SelectedIndex == 0)
                        change = 0;
                    cmd.Parameters.AddWithValue("@change", change);

                    cmd.Parameters.AddWithValue("@InterestAmount", Convert.ToDouble(string.IsNullOrWhiteSpace(txtBenefits.Text) ? "0" : txtBenefits.Text));
                    cmd.Parameters.AddWithValue("@PaidAmount", Convert.ToDouble(string.IsNullOrWhiteSpace(txtPay2.Text) ? "0" : txtPay2.Text));
                    cmd.Parameters.AddWithValue("@CreditBalance", CreditBalance);
                    cmd.Parameters.AddWithValue("@PaymentMethod", cbPayWay.Text);
                    cmd.Parameters.AddWithValue("@status", "finshed");
                    cmd.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDouble(string.IsNullOrWhiteSpace(txtPreviousBebitBalance.Text) ? "0" : txtPreviousBebitBalance.Text));
                    cmd.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDouble(string.IsNullOrWhiteSpace(txtCurrentDebitBalance.Text) ? "0" : txtCurrentDebitBalance.Text));
                    cmd.Parameters.AddWithValue("@latePayTax", Convert.ToDouble(string.IsNullOrWhiteSpace(txtBenefits.Text) ? "0" : txtBenefits.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void saveBillSupliser()
        {
            try
            {
                string qry = @"
                UPDATE billPrcheses SET 
                    supplierID = @supplierID,
                    shiftID = @shiftID,
                    payWay = @payWay, 
                    total = @total, 
                    clear = @clear,
                    priceClear =  @clear,
                    InvoiceIssuanceValue = @clear,
                    change = @change,
                    billStatus = @billStatus,
                    PaidAmount = @PaidAmount,
                    previousDebitBalance = @previousDebitBalance,
                    currentDebitBalance = @currentDebitBalance
                WHERE bID = @ID";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@ID", mainID);
                    cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@supplierID", selectedPartyID);
                    cmd.Parameters.AddWithValue("@change", Convert.ToDouble(string.IsNullOrWhiteSpace(txtChange.Text) ? "0" : txtChange.Text));
                    cmd.Parameters.AddWithValue("@total", Convert.ToDouble(string.IsNullOrWhiteSpace(txtPriceTotal.Text) ? "0" : txtPriceTotal.Text));
                    cmd.Parameters.AddWithValue("@clear", Convert.ToDouble(string.IsNullOrWhiteSpace(txtClean.Text) ? "0" : txtClean.Text));
                    cmd.Parameters.AddWithValue("@payWay", cbPayWay.Text);
                    cmd.Parameters.AddWithValue("@billStatus", "Finish");
                    cmd.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDouble(string.IsNullOrWhiteSpace(txtPreviousBebitBalance.Text) ? "0" : txtPreviousBebitBalance.Text));
                    cmd.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDouble(string.IsNullOrWhiteSpace(txtCurrentDebitBalance.Text) ? "0" : txtCurrentDebitBalance.Text));
                    cmd.Parameters.AddWithValue("@PaidAmount", Convert.ToDouble(string.IsNullOrWhiteSpace(txtPay2.Text) ? "0" : txtPay2.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void residualBillCustomer()
        {
            string qry = string.Empty;

            if (!paritesIsFind)
            {
                qry = @"
                INSERT INTO residualTable 
                (PartiesID, [status], isCustomer, totalPaid, totalTransaction, previousDebitBalance, currentDebitBalance) 
                VALUES (@PartiesID, @status, @isCustomer, @totalPaid, @totalTransaction, @previousDebitBalance, @currentDebitBalance);
                SELECT SCOPE_IDENTITY();";

            }
            else
            {
                qry = @"
                UPDATE residualTable 
                SET
                    [status] = @status, 
                    isCustomer = @isCustomer, 
                    totalPaid = ISNULL(totalPaid, 0) + @totalPaid,
                    totalTransaction = ISNULL(totalTransaction, 0) + @totalTransaction,
                    previousDebitBalance = @previousDebitBalance, 
                    currentDebitBalance = ISNULL(currentDebitBalance, 0) + @currentDebitBalance
                WHERE PartiesID = @PartiesID;";


            }

            string qtyTransaction = @"
        INSERT INTO PartiesTransactions
            (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
        VALUES
            (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID, 
            CAST(GETDATE() AS DATE), @aTime);";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        // أولاً: PartiesTransactions
                        using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                        {
                            cmdTransaction.Parameters.AddWithValue("@partiesID", selectedPartyID);
                            cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "فاتورة اجل");
                            cmdTransaction.Parameters.AddWithValue("@mainID", mainID);
                            cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                              $"تم إضافة فاتورة أجل والباقي  {Convert.ToDecimal(txtChange.Text).ToString("N0")}");
                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance",
                                Convert.ToDouble(string.IsNullOrWhiteSpace(txtPreviousBebitBalance.Text) ? "0" : txtPreviousBebitBalance.Text));
                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance",
                                Convert.ToDouble(string.IsNullOrWhiteSpace(txtCurrentDebitBalance.Text) ? "0" : txtCurrentDebitBalance.Text));
                            cmdTransaction.ExecuteNonQuery();
                        }

                        // ثانياً: residualTable
                        using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@PartiesID", selectedPartyID);
                            cmd.Parameters.AddWithValue("@status", "مدين");
                            cmd.Parameters.AddWithValue("@isCustomer", (partyType == "عميل"));
                            cmd.Parameters.AddWithValue("@totalPaid", Convert.ToDouble(txtPay2.Text == string.Empty ? "0" : txtPay2.Text));
                            cmd.Parameters.AddWithValue("@totalTransaction", Convert.ToDouble(txtTotalAfterBenefit.Text == string.Empty ? "0" : txtTotalAfterBenefit.Text));
                            cmd.Parameters.AddWithValue("@previousDebitBalance", CreditBalance);
                            cmd.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDouble(txtChange.Text == string.Empty ? "0" : txtChange.Text));

                            cmd.ExecuteNonQuery();
                        }


                        // نجاح → نثبت التغييرات
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        // فشل → نلغي كل حاجة
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }



        private void PremiumBillCustomer()
        {
            string qry = string.Empty;
            if (status == "new")
            {
                qry = @"INSERT INTO premiumAddTable 
        (mainID, PartiesID, premiumValue, premiumNumber, startDate, time, endDate, status, ChargNumber) 
        VALUES(@mainID, @PartiesID, @premiumValue, @premiumNumber, @startDate, @time, @endDate, @status, @ChargNumber)";
            }
            else
            {
                qry = @"UPDATE premiumAddTable 
        SET PartiesID = @PartiesID, premiumValue = @premiumValue, premiumNumber = @premiumNumber, startDate = @startDate, time = @time, endDate = @endDate, status = @status, ChargNumber = @ChargNumber 
        WHERE mainID = @mainID;";
            }

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd1 = new SqlCommand(qry, con))
            {
                cmd1.Parameters.AddWithValue("@mainID", mainID);
                cmd1.Parameters.AddWithValue("@PartiesID", selectedPartyID); // أو قيمتك من الكود
                cmd1.Parameters.AddWithValue("@premiumValue", Convert.ToDouble(txtValuePremium.Text == string.Empty ? "0" : txtValuePremium.Text));
                cmd1.Parameters.AddWithValue("@premiumNumber", Convert.ToDouble(txtNumPremium.Text == string.Empty ? "0" : txtNumPremium.Text));
                cmd1.Parameters.AddWithValue("@startDate", DateTime.Now.Date);
                cmd1.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
                cmd1.Parameters.AddWithValue("@endDate", DBNull.Value); // خالي
                cmd1.Parameters.AddWithValue("@status", "لم يتم الدفع");
                cmd1.Parameters.AddWithValue("@ChargNumber", 0); // خالي أو قيمتك

                if (con.State == ConnectionState.Closed)
                    con.Open();

                cmd1.ExecuteNonQuery();
            }
        }



        private void updateBillDataFromDB()
        {
            string qry = @"
SET NOCOUNT ON;
SELECT m.MainID, 
       m.partiesID, 
       m.total, 
       m.priceClear, 
       m.descount, 
       m.PaidAmount, 
       m.PaymentMethod, 
       m.CreditBalance,
       m.TotalWithInterest,
       m.InterestAmount,
       m.change,
       p.pName, 
       p.pPhone, 
       p.pAdderss
FROM tblMain1 AS m
INNER JOIN Parties AS p
    ON m.partiesID = p.pID
WHERE m.MainID = @mainID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@mainID", mainID);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // قراءة القيم في متغيرات
                        int partiesID = reader["partiesID"] != DBNull.Value ? Convert.ToInt32(reader["partiesID"]) : 0;
                        string totalStr = reader["total"] != DBNull.Value ? reader["total"].ToString() : "0";
                        string priceClearStr = reader["priceClear"] != DBNull.Value ? reader["priceClear"].ToString() : "0";
                        int descount = reader["descount"] != DBNull.Value ? Convert.ToInt32(reader["descount"]) : 0;
                        string paidAmountStr = reader["PaidAmount"] != DBNull.Value ? reader["PaidAmount"].ToString() : "0";
                        string totalWithInterestStr = reader["TotalWithInterest"] != DBNull.Value ? reader["TotalWithInterest"].ToString() : "0";
                        string paymentMethod = reader["PaymentMethod"] != DBNull.Value ? reader["PaymentMethod"].ToString() : "";
                        int creditBalance = reader["CreditBalance"] != DBNull.Value ? Convert.ToInt32(reader["CreditBalance"]) : 0;
                        string changeStr = reader["change"] != DBNull.Value ? reader["change"].ToString() : "0";
                        string interestAmountStr = reader["InterestAmount"] != DBNull.Value ? reader["InterestAmount"].ToString() : "0";
                        string pName = reader["pName"] != DBNull.Value ? reader["pName"].ToString() : "";
                        string pPhone = reader["pPhone"] != DBNull.Value ? reader["pPhone"].ToString() : "";
                        string pAddress = reader["pAdderss"] != DBNull.Value ? reader["pAdderss"].ToString() : "";

                        // مثال لو عايز تحسب قيمة الخصم على مبلغ
                        decimal total = Convert.ToDecimal(reader["total"]);
                        decimal discountValueD = total * (descount / 100m);
                        int discountValue = (int)discountValueD; // تحويل القيمة إلى int

                        // التعيين للـ TextBox بعد ما القيم اتقرت كلها
                        selectedPartyID = partiesID;
                        txtPriceTotal.Text = totalStr;
                        txtClean.Text = priceClearStr;
                        txtDV.Text = discountValue.ToString();
                        txtPay1.Text = paidAmountStr;
                        txtPay2.Text = paidAmountStr;
                        txtTotalAfterBenefit.Text = totalWithInterestStr;
                        cbPayWay.SelectedItem = paymentMethod;
                        CreditBalance = creditBalance;
                        txtCreditorBalance.Text = CreditBalance.ToString();
                        txtChange.Text = changeStr;
                        txtBenefits.Text = interestAmountStr;
                        txtName.Text = pName;
                        txtPhone.Text = pPhone;
                        txtAddress.Text = pAddress;

                        // العمليات الحسابية
                        if (int.TryParse(totalStr, out int total1) && int.TryParse(priceClearStr, out int priceClear))
                        {
                            txtDV.Text = (total1 - priceClear).ToString();
                        }
                    }
                }
            }
        }

        private void btnCreditorBalance_Click(object sender, EventArgs e)
        {
            CreditBalance = int.Parse(txtCreditorBalance.Text == string.Empty ? "0" : txtCreditorBalance.Text);
            txtChange.Text = "0";

        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private void gbPartiesData_Enter(object sender, EventArgs e)
        {
            this.ActiveControl = null;

        }

        private void btnAddParties_Click(object sender, EventArgs e)
        {

            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.partyType = partyType;
                frm.ShowDialog(this);

            }
            this.Focus();
            textSuggester(); // Initialize text suggester for party names

        }



        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ الاتصال الصحيح
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@PartyType", "%" + partyType + "%");

                    con.Open(); // ✅ افتح الاتصال

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
                }
            }

            txtName.AutoCompleteCustomSource = dataSource;
            txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }


        private void txtName_Leave(object sender, EventArgs e)
        {


        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtName.Text))
            {
                selectedPartyID = nameToID[txtName.Text];
                unknown = false;
            }
            else
            {
                selectedPartyID = 0;
                unknown = true;
            }

            if (selectedPartyID > 0)
            {
                string qry = @"SELECT pPhone, pAdderss FROM Parties WHERE pID = @pID";

                using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدم اتصال جاهز
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@pID", selectedPartyID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtPhone.Text = reader["pPhone"].ToString();
                                txtAddress.Text = reader["pAdderss"] != DBNull.Value ? reader["pAdderss"].ToString() : string.Empty;

                                lblValedatName.Visible = false;
                                btnNext1.Enabled = true;
                                btnEditParties.Enabled = true;

                                txtName.HoverState.BorderColor = Color.FromArgb(136, 214, 218);
                                txtName.FocusedState.BorderColor = Color.FromArgb(136, 214, 218);
                                txtName.BorderColor = Color.FromArgb(136, 214, 218);
                            }
                        }
                    }
                }
            }
            else if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.TextAlign = HorizontalAlignment.Right;

                txtPhone.Text = string.Empty;
                txtAddress.Text = string.Empty;
                lblValedatName.Visible = false;
                btnNext1.Enabled = false;
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

                lblValedatName.Visible = true;
                lblValedatName.Text = "هذا الاسم غير موجود";
                lblValedatName.ForeColor = Color.Red;

                btnNext1.Enabled = false;
                btnEditParties.Enabled = false;
            }

            if (!string.IsNullOrEmpty(txtName.Text))
            {
                char firstChar = txtName.Text[0];
                txtName.TextAlign = IsArabic(firstChar)
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left;
            }
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }

        private void btnNext1_Click(object sender, EventArgs e)
        {
            next1();
            cbPayWay.SelectedIndex = 0;
            cbPayWay.Enabled = true;
        }
        private void next1()
        {
            gbData.Enabled = false;
            gbPartiesData.Enabled = false;
            gbPay.Enabled = true;
            btnNext1.Enabled = false;
            btnEdit1.Enabled = true;
            btnEdit2.Enabled = false;
            btnPremium.Enabled = true;
            btnRsidual.Enabled = true;

            txtPay1.Focus();
            if (cbPayWay.SelectedIndex == 0 || cbPayWay.SelectedIndex == 2) // Assuming 0 is the index for "كاش"
            {
                gbPartiesData.Enabled = true;
                txtNumPremium.Visible = false;
                lblNumPremium.Visible = false;
                txtValuePremium.Visible = false;
                lblValPremium.Visible = false;
            }
            else
                gbPartiesData.Enabled = false;

            (previousBebitBalance, CreditBalance) = GetTotalChangeForActiveInvoices(selectedPartyID);
            txtPreviousBebitBalance.Text = Convert.ToString(CreditBalance);
            txtCurrentDebitBalance.Text = Convert.ToString(CreditBalance + totalClean);

        }
        private (decimal previousDebitBalance, decimal currentDebitBalance) GetTotalChangeForActiveInvoices(int partiesID)
        {
            // ✅ تحقق من ID صالح
            if (partiesID <= 0)
            {
                paritesIsFind = false;
                return (0, 0);
            }

            const string qry = @"
            SELECT TOP 1 
                ISNULL(previousDebitBalance, 0) AS previousDebitBalance,
                ISNULL(currentDebitBalance, 0) AS currentDebitBalance
            FROM residualTable
            WHERE PartiesID = @PartiesID
            ORDER BY id DESC;"; // ✅ دا بيجيب أحدث صف للطرف

            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.Add("@PartiesID", SqlDbType.Int).Value = partiesID;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            decimal previous = 0;
                            decimal current = 0;

                            if (reader["previousDebitBalance"] != DBNull.Value)
                                previous = Convert.ToDecimal(reader["previousDebitBalance"]);

                            if (reader["currentDebitBalance"] != DBNull.Value)
                                current = Convert.ToDecimal(reader["currentDebitBalance"]);

                            paritesIsFind = true;
                            return (previous, current);
                        }
                        else
                        {
                            // ✅ الطرف مش موجود في الجدول
                            paritesIsFind = false;
                            return (0, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // ✅ في حالة أي خطأ غير متوقع، نرجع صفر ونبلغ المستخدم (اختياري)
                MessageBox.Show($"Error reading balances: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                paritesIsFind = false;
                return (0, 0);
            }
        }


        private void btnEdit1_Click(object sender, EventArgs e)
        {
            gbData.Enabled = true;
            gbPay.Enabled = false;
            btnNext1.Enabled = true;
            btnEdit1.Enabled = false;
            gbPartiesData.Enabled = false;
            btnPremium.Enabled = false;
            btnRsidual.Enabled = false;

            btnSave.Enabled = false;
            unknown = false;

            txtName.Focus();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.pID = selectedPartyID; // Pass the selected party ID to the form
                frm.partyType = partyType;
                frm.ShowDialog(this);

            }
            this.Focus();
            textSuggester(); // Initialize text suggester for party names

        }

        private void txtPay1_Leave(object sender, EventArgs e)
        {
            ValidatePayment();

        }

        private void txtPay1_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
        }

        private void btnNext2_Click(object sender, EventArgs e)
        {
            int benefit;
            if (isTaskBill)
                benefit = 0;
            else
                benefit = int.Parse(txtBenefits.Text == string.Empty ? "0" : txtBenefits.Text);
            decimal priceTotal = Convert.ToDecimal(txtClean.Text == string.Empty ? "0" : txtClean.Text);
            decimal pay = Convert.ToDecimal(txtPay1.Text == string.Empty ? "0" : txtPay1.Text);
            decimal pay2 = Convert.ToDecimal(txtPay2.Text == string.Empty ? "0" : txtPay2.Text);

            decimal totalAfterBenefit = priceTotal + benefit;
            decimal change2 = totalAfterBenefit - pay2;

            benefitValue = benefit;

            if (cbPayWay.SelectedIndex == 0) // كاش
            {
                txtValuePremium.Enabled = false;
                btnSave.Enabled = true;

                txtNumPremium.Text = "0";
                txtValuePremium.Text = "0";

                gbPartiesData.Text = "حساب الفاتورة";

                gbPartiesData.Enabled = true;
                txtNumPremium.Visible = false;
                lblNumPremium.Visible = false;
                txtValuePremium.Visible = false;
                lblValPremium.Visible = false;

            }
            else if (cbPayWay.SelectedIndex == 1)
            {
                gbPartiesData.Enabled = true;
                txtNumPremium.Visible = true;
                lblNumPremium.Visible = true;
                txtValuePremium.Visible = true;
                lblValPremium.Visible = true;

                txtNumPremium.Enabled = true;
                txtValuePremium.Enabled = true;
                txtNumPremium.Focus();

                gbPartiesData.Text = "بيانات القسط";

                txtTotalAfterBenefit.Text = totalAfterBenefit.ToString();
                txtCurrentDebitBalance.Text = Convert.ToString(CreditBalance + change2);
            }
            else
            {
                txtNumPremium.Text = "0";
                txtValuePremium.Text = "0";

                gbPartiesData.Enabled = true;
                txtNumPremium.Visible = false;
                lblNumPremium.Visible = false;
                txtValuePremium.Visible = false;
                lblValPremium.Visible = false;

                txtNumPremium.Enabled = false;
                txtValuePremium.Enabled = false;
                btnSave.Enabled = true;

                gbPartiesData.Text = "بيانات  الأجل";

                txtTotalAfterBenefit.Text = totalAfterBenefit.ToString();
                txtCurrentDebitBalance.Text = Convert.ToString(CreditBalance + change2);
            }


            gbPay.Enabled = false;
            btnNext2.Enabled = false;
            btnEdit2.Enabled = true;
            btnNext1.Enabled = false;


            txtChange.Text = change2.ToString();





        }

        private void btnEdit2_Click(object sender, EventArgs e)
        {
            gbData.Enabled = false;
            gbPay.Enabled = true;
            btnNext2.Enabled = true;
            btnEdit2.Enabled = false;
            btnSave.Enabled = false;
            btnCreditorBalance.Enabled = false; // Disable creditor balance button if needed          

            txtNumPremium.Enabled = false;
            txtValuePremium.Enabled = false;

            txtBenefits.Text = benefitValue.ToString();
            txtPay1.Text = txtPay2.Text;

            txtPay1.Focus();

            if (cbPayWay.SelectedIndex == 0 || cbPayWay.SelectedIndex == 2) // Assuming 0 is the index for "كاش"
                gbPartiesData.Enabled = true;
            else
                gbPartiesData.Enabled = false;
        }

        int RoundUpToNearestFive(double amount)
        {
            return (int)(Math.Ceiling(amount / 5) * 5);
        }


        private void cbViewBenefit_CheckedChanged(object sender, EventArgs e)
        {
            if (cbViewBenefit.Checked)
            {

                txtValuePremium.ReadOnly = false;

                txtValuePremium.FillColor = txtNumPremium.FillColor;
                txtValuePremium.Enter -= gbPartiesData_Enter;
                txtValuePremium.TabStop = true; // Disable tab stop to prevent focus

            }
            else
            {

                txtValuePremium.ReadOnly = true;

                txtValuePremium.FillColor = txtChange.FillColor;
                txtValuePremium.Enter += gbPartiesData_Enter;
                txtValuePremium.TabStop = false; // Disable tab stop to prevent focus

            }
        }

        private void txtValuePremium_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
            if (e.KeyChar == (char)Keys.Enter)
            {

                calculatedBenefits();

            }
        }

        private void txtNumPremium_Leave(object sender, EventArgs e)
        {
            //calculatedBenefits();
        }
        private void calculatedBenefits()
        {
            int totalAmount = int.Parse(txtTotalAfterBenefit.Text == string.Empty ? "0" : txtTotalAfterBenefit.Text);      // المبلغ المتبقي

            int change = int.Parse(txtChange.Text == string.Empty ? "0" : txtChange.Text);      // المبلغ المتبقي
            int installmentCount = int.Parse(txtNumPremium.Text == string.Empty ? "0" : txtNumPremium.Text);       // عدد الأقساط

            if (installmentCount > 0)
            {
                double rawInstallment = change / installmentCount;

                int finalInstallment = RoundUpToNearestFive(rawInstallment);

                int totalAfterRounding = finalInstallment * installmentCount;

                int calculatedBenefits = totalAfterRounding - change;

                int newTotal = totalAmount + calculatedBenefits;
                int newChange = change + calculatedBenefits;


                int newBenefits = benefitValue + calculatedBenefits;

                txtTotalAfterBenefit.Text = newTotal.ToString();
                txtChange.Text = newChange.ToString();
                txtBenefits.Text = newBenefits.ToString();

                txtValuePremium.Text = finalInstallment.ToString(); // عرض القسط النهائي

                btnSave.Enabled = true;
            }
            else
            {
                txtNumPremium.Focus();
                btnSave.Enabled = false;
                return;
            }
        }

        private void txtPay1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام + و - فقط
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '+' && e.KeyChar != '-')
            {
                e.Handled = true;
                return;
            }

        }


        private void txtPay1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Guna.UI2.WinForms.Guna2TextBox txt = sender as Guna.UI2.WinForms.Guna2TextBox;
                    string input = txt.Text.Trim();

                    if (IsValidExpression(input))
                    {
                        try
                        {
                            DataTable dt = new DataTable();
                            var result = dt.Compute(input, "");
                            txt.Text = result.ToString();
                        }
                        catch (DivideByZeroException)
                        {
                            MessageBox.Show("لا يمكن القسمة على صفر!");
                            txt.Clear();
                        }
                        catch
                        {
                            MessageBox.Show("التعبير غير صحيح!");
                            txt.Clear();
                        }
                    }
                    else
                    {
                        txt.Clear();
                    }

                    ValidatePayment();

                    if (unknown)
                    {
                        txtPay1.Text = txtClean.Text;
                        txtPay2.Text = txtPay1.Text;
                        txtCurrentDebitBalance.Text = "0";
                        txtPreviousBebitBalance.Text = "0";
                        btnSave.Enabled = true;
                        return;

                    }

                    if (cbPayWay.SelectedIndex == 0) // Assuming 0 is the index for "كاش"
                    {

                        txtCurrentDebitBalance.Text = Convert.ToString(previousBebitBalance);
                    }

                    txtPay2.Text = txtPay1.Text;

                    decimal pay1 = string.IsNullOrWhiteSpace(txtPay1.Text) ? 0 : Convert.ToDecimal(txtPay1.Text);
                    decimal clean = string.IsNullOrWhiteSpace(txtClean.Text) ? 0 : Convert.ToDecimal(txtClean.Text);
                   

                    decimal change = pay1 - clean;

                    if (selectedPartyID == 0) // لو المبلغ المدفوع أقل من أو يساوي قيمة الفاتورة أو مفيش حد متحدد
                    {
                        txtCurrentDebitBalance.Text = Convert.ToString(previousBebitBalance - change);
                        return;
                    }
                    if (change == 0)
                    {
                        txtCurrentDebitBalance.Text = CreditBalance.ToString();
                        txtPreviousBebitBalance.Text = previousBebitBalance.ToString();
                        return;
                    }
                    else if (change < 0) // المبلغ المدفوع أقل من الفاتورة
                    {
                        decimal newDebit = Math.Abs(change) + CreditBalance; // نضيف النقص على الرصيد
                        txtCurrentDebitBalance.Text = newDebit.ToString();
                        txtPreviousBebitBalance.Text = CreditBalance.ToString();
                        return;
                    }

                    DialogResult result2 = MessageBox.Show(
                   $"هل تريد خصم الرصيد الباقي من حساب المدين السابق ؟\n\nالرصيد الباقي: {change:N1}",
                   "تأكيد السحب",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning);
                    if (result2 == DialogResult.Yes)
                    {
                        residual(change);
                        transactions(change, newBalance, oldBalance);
                        Rpay(change, newBalance);
                        txtPay2.Text = txtPay1.Text;
                        btnExit.Enabled = false;

                    }
                    else
                    {
                        txtCurrentDebitBalance.Text = Convert.ToString(previousBebitBalance);
                        txtPay2.Text = txtClean.Text;
                        txtPay1.Text = txtChange.Text;
                        txtChange.Text = "0";


                    }
                }
                catch
                {
                    MessageBox.Show("خطأ في الصيغة الحسابية");
                }

                // منع تنفيذ أي شيء آخر للزر Enter
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void Rpay(decimal change, decimal currentBalance)
        {
            personName = txtName.Text;

            if (change <= 0)
            {
                MessageBox.Show("المبلغ المدفوع يجب أن يكون أكبر من الصفر.");
                return;
            }
            if (selectedPartyID == 0)
            {
                MessageBox.Show("يرجى اختيار اسم الطرف أولاً.");
                return;
            }

            string qry;
            if (partyType == "عميل")
            {
                qry = @"INSERT INTO chargeResidual 
            ([partiesID], [name], [shiftId], [recipt], [change], [date], [time])
            VALUES (@partiesID, @name, @shiftId, @recipt, @change, @Date, @Time);
            SELECT SCOPE_IDENTITY();";
            }
            else
            {
                qry = @"INSERT INTO chargeResidualSuplieser 
            ([partiesID], [name], [shiftId], [recipt], [change], [date], [time])
            VALUES (@partiesID, @name, @shiftId, @recipt, @change, @Date, @Time);
            SELECT SCOPE_IDENTITY();";
            }


            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@partiesID", selectedPartyID);
                cmd.Parameters.AddWithValue("@name", personName);
                cmd.Parameters.AddWithValue("@shiftId", MainClass.shiftID);
                cmd.Parameters.AddWithValue("@recipt", change);
                cmd.Parameters.AddWithValue("@change", currentBalance);
                cmd.Parameters.AddWithValue("@Time", Convert.ToString(DateTime.Now.ToShortTimeString()));
                cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(DateTime.Now.Date));

                if (con.State == ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();
            }
            // دفع المبلغ الجزئي
        }

        private void residual(decimal change)
        {


            string queryCheck = "SELECT COUNT(*) FROM residualTable WHERE PartiesID = @partiesID";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // 🔍 التأكد من وجود PartiesID
                int isCustomerValue = (partyType == "عميل") ? 1 : 0;

                using (SqlCommand checkCmd = new SqlCommand(queryCheck, con))
                {
                    checkCmd.Parameters.AddWithValue("@partiesID", selectedPartyID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        // 📝 PartiesID مش موجود → نضيف صف جديد
                        string insertQuery = @"
                        INSERT INTO residualTable
                        (PartiesID, status, isCustomer, totalPaid, totalTransaction, previousDebitBalance, currentDebitBalance)
                        VALUES
                        (@partiesID, N'دائن', @isCustomer, 0, 0, 0, 0);";

                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                        {
                            insertCmd.Parameters.AddWithValue("@partiesID", selectedPartyID);
                            insertCmd.Parameters.AddWithValue("@isCustomer", isCustomerValue);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                // ✏️ تنفيذ UPDATE وجلب الرصيدين
                string query = @"
                UPDATE residualTable
                SET 
                    previousDebitBalance = ISNULL(currentDebitBalance, 0), -- حفظ القديم
                    currentDebitBalance = ISNULL(currentDebitBalance, 0) - @deductValue -- تعديل الجديد
                OUTPUT INSERTED.previousDebitBalance, INSERTED.currentDebitBalance
                WHERE PartiesID = @partiesID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@deductValue", change);
                    cmd.Parameters.AddWithValue("@partiesID", selectedPartyID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            oldBalance = Convert.ToDecimal(reader[0]);
                            newBalance = Convert.ToDecimal(reader[1]);
                        }
                    }
                }

                con.Close();
                txtCurrentDebitBalance.Text = newBalance.ToString();

            }

            // 🔍 معالجة حالة الرصيد السالب
            if (newBalance < 0)
            {
                DialogResult result2 = MessageBox.Show(
                    $"هل تريد سحب رصيد الدائن الحالي؟\n\nالرصيد السابق: {oldBalance:N1}\nالرصيد الحالي: {newBalance:N1}",
                    "تأكيد السحب",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                string newStatus = (result2 == DialogResult.Yes) ? "مسدد" : "دائن";
                string currentDebitBalance = (result2 == DialogResult.Yes) ? "0" : newBalance.ToString();
                txtCurrentDebitBalance.Text = currentDebitBalance;


                string query2 = @"
                UPDATE residualTable
                SET currentDebitBalance = CASE WHEN @status = N'مسدد' THEN 0 ELSE currentDebitBalance END,
                    status = @status
                WHERE PartiesID = @partiesID;";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query2, con))
                {
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@partiesID", selectedPartyID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        private void transactions(decimal amoutPaied, decimal currentBalance, decimal prevBalance)
        {
            string qtyTransaction = @"
        INSERT INTO PartiesTransactions
            (partiesID, shiftID, transactionsInfo, transactionsType, previousDebitBalance, currentDebitBalance, mainID, aDate, aTime)
        VALUES
            (@partiesID, @shiftID, @transactionsInfo, @transactionsType, @previousDebitBalance, @currentDebitBalance, @mainID, 
            CAST(GETDATE() AS DATE), @aTime);";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        // أولاً: PartiesTransactions
                        using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                        {
                            cmdTransaction.Parameters.AddWithValue("@partiesID", selectedPartyID);
                            cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "سداد من الاجل");
                            cmdTransaction.Parameters.AddWithValue("@mainID", DBNull.Value);
                            cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());

                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                              $"تم سداد دفعة بقيمة  {Convert.ToDecimal(amoutPaied).ToString("N0")}");
                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", prevBalance);
                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", currentBalance);
                            cmdTransaction.ExecuteNonQuery();
                        }
                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        // فشل → نلغي كل حاجة
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        private void SetBorderColor(Color color)
        {
            txtPay1.HoverState.BorderColor = color;
            txtPay1.FocusedState.BorderColor = color;
            txtPay1.BorderColor = color;
        }

        private void ValidatePayment()
        {
            int pay = ParseTextToInt(txtPay1.Text);
            int priceTotal = ParseTextToInt(txtClean.Text);


            if (cbPayWay.SelectedIndex == 0) // كاش
            {
                if (unknown)
                {
                    SuccessPayment();
                    //txtPay1.Text = txtClean.Text;
                    txtCurrentDebitBalance.Text = "0";
                    return;
                }
                if (pay < priceTotal)
                {
                    SetBorderColor(Color.Red);
                    lblValedatPay.Visible = true;
                    lblViewChange.Visible = false;
                    lblValedatPay.Text = "المبلغ المدفوع أقل من قيمة الفاتورة، تم تحويلها للأجل";
                    lblValedatPay.ForeColor = Color.Red;
                    cbPayWay.SelectedIndex = 2;
                    btnNext2.Enabled = true;

                }
                else if (pay == priceTotal)
                {
                    SuccessPayment();
                }
                else
                {
                    OverPayment(pay, priceTotal);
                    //txtPay1.Text = txtClean.Text;

                }
            }
            else if (cbPayWay.SelectedIndex == 1) // دفعة قسط
            {
                if (pay < priceTotal)
                {
                    lblValedatPay.Visible = true;
                    lblValedatPay.Text = "انت الان علي نظام الدفع بالقسط ,اضف فائدة اذا اردت";
                    lblValedatPay.ForeColor = Color.Green;
                    SetBorderColor(Color.FromArgb(136, 214, 218));
                    btnNext2.Enabled = true;
                }
                else if (pay == priceTotal)
                {
                    SuccessPayment();
                    cbPayWay.SelectedIndex = 0;
                }
                else
                {
                    OverPayment(pay, priceTotal);
                    cbPayWay.SelectedIndex = 0;

                }
            }
            else if (cbPayWay.SelectedIndex == 2) // أجل
            {
                if (pay == 0)
                {
                    lblValedatPay.Visible = true;
                    lblValedatPay.Text = "المبلغ فارغ، انت الان علي نظام الدفع بالأجل أضف فائدة إذا أردت";
                    lblValedatPay.ForeColor = Color.Green;
                    SetBorderColor(Color.FromArgb(136, 214, 218));
                    btnNext2.Enabled = true;

                }
                else if (pay < priceTotal)
                {
                    lblValedatPay.Visible = true;
                    lblValedatPay.Text = "انت الان علي نظام الدفع بالأجل, اضف فائدة اذا اردت";
                    lblValedatPay.ForeColor = Color.Green;
                    SetBorderColor(Color.FromArgb(136, 214, 218));
                    btnNext2.Enabled = true;
                }
                else if (pay == priceTotal)
                {
                    SuccessPayment();
                    cbPayWay.SelectedIndex = 0;
                }
                else
                {
                    OverPayment(pay, priceTotal);
                    cbPayWay.SelectedIndex = 0;

                }
            }
        }

        // 🔹 دالة لمعالجة الدفع الزائد
        private void OverPayment(int pay, int priceTotal)
        {
            lblValedatPay.Visible = true;
            lblViewChange.Visible = true;

            lblValedatPay.Text = "المبلغ المدفوع أكبر من قيمة الفاتورة، تم تحويل الفرق إلى رصيد دائن";
            int change = pay - priceTotal;

            txtPay2.Text = txtPay1.Text;

            decimal clean = string.IsNullOrWhiteSpace(txtClean.Text) ? 0 : Convert.ToDecimal(txtClean.Text);
           

            txtTotalAfterBenefit.Text = txtClean.Text;
            txtChange.Text = change.ToString();
            lblViewChange.Text = change.ToString();
            txtCreditorBalance.Text = change.ToString();

            lblValedatPay.ForeColor = Color.Green;
            lblViewChange.ForeColor = Color.Green;

            SetBorderColor(Color.FromArgb(136, 214, 218));
            btnSave.Enabled = true;
            // btnNext2.Enabled = true;
            btnCreditorBalance.Enabled = true;

            gbPartiesData.Text = "حساب الفاتورة";


        }

        // 🔹 دالة لمعالجة الدفع المطابق
        private void SuccessPayment()
        {
            decimal clean = string.IsNullOrWhiteSpace(txtClean.Text) ? 0 : Convert.ToDecimal(txtClean.Text);

            

            txtTotalAfterBenefit.Text = clean.ToString();
            SetBorderColor(Color.FromArgb(136, 214, 218));
            btnSave.Enabled = true;
            //btnNext2.Enabled = true;
            lblValedatPay.Visible = false;
            lblViewChange.Visible = false;

            gbPartiesData.Text = "حساب الفاتورة";

        }

        // 🔹 تحويل النص لرقم
        private int ParseTextToInt(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            try
            {
                // تقييم العمليات الحسابية لو فيه + أو -
                DataTable dt = new DataTable();
                var result = dt.Compute(text, "");
                return Convert.ToInt32(result);
            }
            catch
            {
                return 0;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            unknown = true;
            next1();
            selectedPartyID = 0;
            cbPayWay.Enabled = false;
            txtPay1.Focus();
            cbPayWay.SelectedIndex = 0;
            btnNext2.Enabled = false;
            txtCreditorBalance.Text = "0";  
            txtCurrentDebitBalance.Text = "0";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        }

        private void txtDV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtDV_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    decimal dv = Math.Max(
                        Convert.ToDecimal(string.IsNullOrWhiteSpace(txtDV.Text) ? "0" : txtDV.Text),
                        discountValue
                    );
                    benefitValue = Convert.ToInt32(string.IsNullOrWhiteSpace(txtBenefits.Text) ? "0" : txtBenefits.Text);
                    decimal finalDis;
                    if (isTaskBill)
                        finalDis = dv;
                    else
                        finalDis = dv + benefitValue;

                    decimal newPriceClear = total - finalDis;
                    txtClean.Text = newPriceClear.ToString("F1");
                    txtDV.Text = (dv).ToString("F1");
                    txtTotalAfterBenefit.Text = newPriceClear.ToString("F1");

                    if (unknown)
                    {
                        txtPay1.Text = newPriceClear.ToString("F1");
                        txtPay2.Text = newPriceClear.ToString("F1");
                        btnSave.Enabled = true;

                    }
                }
                catch
                {
                    MessageBox.Show("خطأ في الصيغة الحسابية");
                }
                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private void cbPrintOrderCard_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPrintOrderCard.Checked == true)
                cbBreakable.Enabled = true;
            else
                cbBreakable.Enabled = false;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmPartesSearch frm = new frmPartesSearch(this);
            frm.type = partyType;
            frm.ShowDialog(this);
            this.Focus();
        }
        public void resultSearch(string value)
        {
            txtName.Text = value;
        }
        private static bool IsValidExpression(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (Regex.IsMatch(input, @"[\+\-\*/]{2,}"))
                return false;

            if (!Regex.IsMatch(input, @"^\d+(\.\d+)?([\+\-\*/]\d+(\.\d+)?)*$"))
                return false;

            if (Regex.IsMatch(input, @"/0+(\D|$)"))
                return false;

            return true;
        }
        public static void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox txt = sender as Guna.UI2.WinForms.Guna2TextBox;
            string text = txt.Text;

            // السماح بالأرقام و Backspace
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                return;

            // السماح بعلامات العمليات لمرة واحدة بين الأرقام فقط
            if (e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '*' || e.KeyChar == '/')
            {
                if (string.IsNullOrEmpty(text) || Regex.IsMatch(text, @"[\+\-\*/]$"))
                {
                    e.Handled = true;
                    return;
                }
                return;
            }

            // السماح بالنقطة العشرية لمرة واحدة
            if (e.KeyChar == '.')
            {
                if (text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }
                return;
            }

            // منع أي رموز أخرى
            e.Handled = true;
        }

        // دالة KeyDown عامة لأي TextBox
        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Guna.UI2.WinForms.Guna2TextBox txt = sender as Guna.UI2.WinForms.Guna2TextBox;
                string input = txt.Text.Trim();

                if (IsValidExpression(input))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        var result = dt.Compute(input, "");
                        txt.Text = result.ToString();
                    }
                    catch (DivideByZeroException)
                    {
                        MessageBox.Show("لا يمكن القسمة على صفر!");
                        txt.Clear();
                    }
                    catch
                    {
                        MessageBox.Show("التعبير غير صحيح!");
                        txt.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("التعبير يحتوي على أخطاء!");
                    txt.Clear();
                }
            }
        }
    }
}

