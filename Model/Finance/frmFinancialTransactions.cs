using DevExpress.CodeParser;
using DevExpress.DataAccess.Sql;
using Guna.UI2.WinForms;
using pos.Classes;
using pos.GeneralForms.MainForm;
using pos.Model.POS;
using pos.Model.Stor;
using pos.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static pos.GeneralForms.frmMian2;

namespace pos.Model.Finance
{
    public partial class frmFinancialTransactions : Form
    {
        private int partiesID = 0;
        public string partyType = "عميل";
        public int mainID = 0;
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();
        // متغيرات لتخزين القيم الأصلية
        private Point panel1OriginalLocation;
        private Size panel1OriginalSize;
        private Point panel2OriginalLocation;
        private Size panel2OriginalSize;
        private int oldChange;

        private int type;
        private string InvoiceCode = string.Empty;
        public frmFinancialTransactions()
        {
            InitializeComponent();
            textSuggester();
        }

        private void frmFinancialTransactions_Load(object sender, EventArgs e)
        {
            partyType = "عميل"; // تعيين نوع الطرف الافتراضي
            txtName.Focus();
            ApplyGridStyle(dgvBills);
            ApplyGridStyle(dgvProducts);

        }

        private void frmFinancialTransactions_SizeChanged(object sender, EventArgs e)
        {
            CenterPanelInForm(panel1);
            CenterPanelInForm(panel2);
        }
        private void CenterPanelInForm(Panel panel)
        {
            panel.Left = (this.ClientSize.Width - panel.Width) / 2;
            // لو حابب تـوسّط عموديًا:
            // panel.Top = (this.ClientSize.Height - panel.Height) / 2;
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                click();
                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }
        private async Task<DataTable> LoadDataAsync(string qry, SqlParameter[] parameters)
        {
            using (SqlConnection con = MainClass.GetConnection()) // ✅ كل استعلام له اتصال خاص به
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    await Task.Run(() =>
                    {
                        con.Open(); // ✅ فتح الاتصال داخل مهمة مستقلة
                        da.Fill(dt);
                    });
                    return dt;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            click();

        }
        private void click()
        {

            hasMoreData = true;
            currentPage = 0;
            displayPartiesResdule(1, true);
            type = 1;
            gbDataFinance.Enabled = false;

        }
        private void btnShowAll_Click(object sender, EventArgs e)
        {

            hasMoreData = true;
            currentPage = 0;
            displayPartiesResdule(2, true);
            type = 2;
            gbDataFinance.Enabled = false;

            btnShowFinancailData.Enabled = false;
            btnBills.Enabled = false;
            btnTransfarePrint.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            btnEditeBalance.Enabled = false;
            btnWithdraw.Enabled = false;
            txtAdd.Enabled = false;
            txtEditeBalance.Enabled = false;
            txtWithdraw.Enabled = false;
        }

        private async void dgvBills_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            partiesID = int.Parse(dgvBills.Rows[e.RowIndex].Cells["dgvPartiesID"].Value.ToString());

            billsHasMoreData = true;
            billsPageNumber = 0;
            await displayBillsByPartiesName(partiesID, true);


            txtPreviousBebitBalance.Text = dgvBills.Rows[e.RowIndex].Cells["Column3"].Value.ToString();
            txtCurrentDebitBalance.Text = dgvBills.Rows[e.RowIndex].Cells["Column4"].Value.ToString();
            txtDelivery.Text = dgvBills.Rows[e.RowIndex].Cells["dgvNameParties"].Value.ToString();
            txtTotalPaid.Text = dgvBills.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
            txtTotalTransaction.Text = dgvBills.Rows[e.RowIndex].Cells["Column5"].Value.ToString();

            double current = Convert.ToDouble(txtCurrentDebitBalance.Text == string.Empty ? "0" : txtCurrentDebitBalance.Text);
            if (current < 0)
            {
                btnWithdraw.Enabled = true;
                txtWithdraw.Enabled = true;
            }
            else
            {
                btnWithdraw.Enabled = false;
                txtWithdraw.Enabled = false;
            }

            btnBillDetails.Enabled = true;
            btnShowFinancailData.Enabled = true;
            btnBills.Enabled = true;
            btnTransfarePrint.Enabled = true;
            btnDelete.Enabled = true;
            btnAdd.Enabled = true;
            btnEditeBalance.Enabled = true;
            txtAdd.Enabled = true;
            txtEditeBalance.Enabled = true;
        }

        int pageSize = 14;      // عدد الصفوف في كل صفحة
        int currentPage = 0;    // الصفحة الحالية
        bool isLoading = false;
        bool hasMoreData = true;

        private async Task displayPartiesResdule(int searchMode, bool isNewSearch)
        {
            if (isNewSearch)
                clearFields();
            if (searchMode == 2)
                txtName.Text = string.Empty;

            dgvProducts.Visible = false;
            dgvBills.Visible = true;
            btnBillDetails.Enabled = false;

            dgvBills.Width = 1430;
            dgvBills.Height = panel2.Height - 48;
            dgvBills.Location = new Point((panel2.Width - dgvBills.Width) / 2, 0);


            if (partiesIsFound())
            {
                btnDelete.Enabled = true;
                btnAdd.Enabled = true;
                btnEditeBalance.Enabled = true;
                txtAdd.Enabled = true;
                txtEditeBalance.Enabled = true;
            }

            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {
                if (currentPage == 0) // 🧹 امسح القديم بس في أول تحميل
                    dgvBills.Rows.Clear();

                int offset = currentPage * pageSize;

                string qry = @"
                 -- 1. تحديث الـ status مباشرة في قاعدة البيانات
                 UPDATE r
                 SET r.[status] = CASE
                     WHEN r.currentDebitBalance = 0 THEN N'مسدد'
                     WHEN r.currentDebitBalance > 0 THEN N'مدين'
                     ELSE N'دائن'
                 END
                 FROM residualTable r
                 JOIN Parties p ON r.PartiesID = p.pID
                 WHERE p.PartyType LIKE @PartyType";

                if (searchMode == 1 && partiesID != 0)
                    qry += " AND r.PartiesID = @partiesID";

                qry += @"
                 -- 2. بعد التحديث، عرض القيم بعد التغيير
                 SELECT r.PartiesID,
                        p.pName,
                        r.[status],
                        r.totalPaid,
                        r.totalTransaction,
                        r.previousDebitBalance,
                        r.currentDebitBalance
                 FROM residualTable r
                 JOIN Parties p ON r.PartiesID = p.pID
                 WHERE p.PartyType LIKE @PartyType";

                if (searchMode == 1 && partiesID != 0)
                    qry += " AND r.PartiesID = @partiesID";

                qry += @"
                 ORDER BY p.pName
                 OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                List<SqlParameter> parameters = new List<SqlParameter>
                 {
                     new SqlParameter("@PartyType", partyType),
                     new SqlParameter("@offset", offset),
                     new SqlParameter("@limit", pageSize)
                 };

                if (searchMode == 1 && partiesID != 0)
                    parameters.Add(new SqlParameter("@partiesID", partiesID));

                DataTable dt = await LoadDataAsync(qry, parameters.ToArray());

                int rowIndex = dgvBills.Rows.Count + 1;

                foreach (DataRow row in dt.Rows)
                {
                    dgvBills.Rows.Add(
                        rowIndex++,
                        row["PartiesID"],
                        row["pName"],
                        row["status"],
                        row["totalPaid"] == DBNull.Value ? "0" : Convert.ToDecimal(row["totalPaid"]).ToString("N1"),
                        row["totalTransaction"] == DBNull.Value ? "0" : Convert.ToDecimal(row["totalTransaction"]).ToString("N1"),
                        row["previousDebitBalance"] == DBNull.Value ? "0" : Convert.ToDecimal(row["previousDebitBalance"]).ToString("N1"),
                        row["currentDebitBalance"] == DBNull.Value ? "0" : Convert.ToDecimal(row["currentDebitBalance"]).ToString("N1")
                    );

                    // 🎨 ألوان حسب الحالة
                    var status = row["status"].ToString();
                    DataGridViewRow addedRow = dgvBills.Rows[dgvBills.Rows.Count - 1]; // ✅ آخر صف

                    switch (status)
                    {
                        case "مسدد":
                            addedRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E6F4EA");
                            addedRow.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#0A3D1C");
                            break;

                        case "مدين":
                            addedRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FDECEA");
                            addedRow.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#7A0000");
                            break;

                        case "دائن":
                            addedRow.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#E3F2FD");
                            addedRow.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#002855");
                            break;

                        default:
                            addedRow.DefaultCellStyle.BackColor = Color.White;
                            addedRow.DefaultCellStyle.ForeColor = Color.Black;
                            break;
                    }

                    addedRow.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                }

                // ✅ لو عدد الصفوف أقل من حجم الصفحة → مفيش صفحات تانية
                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                currentPage++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void clearFields()
        {
            txtPreviousBebitBalance.Text = string.Empty;
            txtCurrentDebitBalance.Text = string.Empty;
            txtRPay.Text = string.Empty;
            txtNote.Text = string.Empty;
            txtDelivery.Text = string.Empty;
            txtTotalPaid.Text = string.Empty;
            txtTotalTransaction.Text = string.Empty;
            txtAdd.Text = string.Empty;
            txtEditeBalance.Text = string.Empty;
            txtWithdraw.Text = string.Empty;
            
        }

        private int currentPartiesId = 0;
        private int billsPageNumber = 0;
        private bool billsHasMoreData = true;
        private bool isBillsLoading = false;
        private int billsPageSize = 15; // عدد الصفوف في كل صفحة

        private async Task displayBillsByPartiesName(int partiesid, bool reset = false)
        {
            currentPartiesId = partiesid;

            if (reset)
            {
                dgvProducts.Visible = true;
                dgvBills.Visible = false;

                dgvProducts.Width = 1426;
                dgvProducts.Height = panel2.Height - 48;
                dgvProducts.ScrollBars = ScrollBars.Vertical;

                int x = (panel2.Width - dgvProducts.Width) / 2;
                int y = 0;
                dgvProducts.Location = new Point(x, y);

                dgvProducts.Rows.Clear();

                billsPageNumber = 0;
                billsHasMoreData = true;
            }

            if (!billsHasMoreData || partiesid == 0)
                return;

            try
            {
                string qry;

                if (partyType == "عميل")
                {
                    qry = @"
                SELECT 
                    cr.[id],
                    cr.[partiesID],
                    cr.[name],
                    cr.[shiftId],
                    cr.[recipt],
                    cr.[previousDebitBalance],
                    cr.[change],
                    cr.[date],
                    cr.[time],
                    cr.[note],
                    s.[sName] AS StaffName
                FROM [chargeResidual] cr
                INNER JOIN [shifts] sh ON cr.[shiftId] = sh.[ID]
                INNER JOIN [staff] s ON sh.[staffID] = s.[staffID]
                WHERE cr.[partiesID] = @partiesID
                ORDER BY cr.[date]
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
            ";
                }
                else
                {
                    qry = @"
                SELECT 
                    cr.[id],
                    cr.[partiesID],
                    cr.[name],
                    cr.[shiftId],
                    cr.[recipt],
                    cr.[previousDebitBalance],
                    cr.[change],
                    cr.[date],
                    cr.[time],
                    cr.[note],
                    s.[sName] AS StaffName
                FROM [chargeResidualSuplieser] cr
                INNER JOIN [shifts] sh ON cr.[shiftId] = sh.[ID]
                INNER JOIN [staff] s ON sh.[staffID] = s.[staffID]
                WHERE cr.[partiesID] = @partiesID
                ORDER BY cr.[date]
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
            ";
                }

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@partiesID", partiesid),
            new SqlParameter("@offset", billsPageNumber * billsPageSize),
            new SqlParameter("@limit", billsPageSize)
        };

                DataTable dt = await LoadDataAsync(qry, parameters.ToArray());

                int rowIndex = dgvProducts.Rows.Count + 1;
                foreach (DataRow row in dt.Rows)
                {
                    dgvProducts.Rows.Add(
                        rowIndex++,
                        row["id"],
                        row["name"],
                        row["StaffName"],
                        row["recipt"],
                        row["previousDebitBalance"],
                        row["change"],
                        Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd"),
                        row["time"],
                        row["note"]
                    );
                }

                // لو البيانات أقل من حجم الصفحة → مفيش بيانات تانية
                if (dt.Rows.Count < billsPageSize)
                    billsHasMoreData = false;

                billsPageNumber++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message);
            }
        }



        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtName.Text))
            {
                partiesID = nameToID[txtName.Text];
                btnShowFinancailData.Enabled = true;
                btnBills.Enabled = true;
                btnTransfarePrint.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = true;
                btnEditeBalance.Enabled = true;
                txtAdd.Enabled = true;
                txtEditeBalance.Enabled = true;
                txtDelivery.Text = txtName.Text; // تعيين اسم الطرف في حقل التسليم
            }
            else
            {
                partiesID = 0;
                btnShowFinancailData.Enabled = false;
                btnBills.Enabled = false;
                btnTransfarePrint.Enabled = false;
                btnTransfarePrint.Enabled = false;
                btnTransfarePrint.Enabled = false;
                btnDelete.Enabled = false;
                btnAdd.Enabled = false;
                btnEditeBalance.Enabled = false;
                btnWithdraw.Enabled = false;
                txtAdd.Enabled = false;
                txtEditeBalance.Enabled = false;
                txtWithdraw.Enabled = false;
            }
        }
        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ اتصال آمن
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartyType", "%" + partyType + "%");

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    con.Open(); // ✅ فتح الاتصال فقط داخل using
                    da2.Fill(dt2);
                }

                foreach (DataRow row in dt2.Rows)
                {
                    string name = row["pName"].ToString();
                    int id = Convert.ToInt32(row["pID"]);
                    dataSource.Add(name);
                    nameToID[name] = id;
                }

                txtName.AutoCompleteCustomSource = dataSource;
                txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }





        private void btnHome_Click(object sender, EventArgs e)
        {
            if (!btnHome.Checked)
            {
                mainPanel.Controls.Clear();
                mainPanel.Controls.Add(panel1);
                mainPanel.Controls.Add(panel2);

                // رجع panel1 لمكانها وحجمها الأصلي
                panel1.Location = panel1OriginalLocation;
                panel1.Size = panel1OriginalSize;

                // رجع panel2 لمكانها وعرضها القديم
                panel2.Location = panel2OriginalLocation;
                panel2.Width = panel2OriginalSize.Width;

                // خلي ارتفاعها يكمل لآخر الـ mainPanel
                panel2.Height = mainPanel.Height - panel2.Location.Y - 5;


                CenterPanelInForm(panel1);
                CenterPanelInForm(panel2);
            }

            btnHome.Checked = true;
            btnDetainls.Checked = false;



        }

        private void btnDetainls_Click(object sender, EventArgs e)
        {
            panel1OriginalLocation = panel1.Location;
            panel1OriginalSize = panel1.Size;

            panel2OriginalLocation = panel2.Location;
            panel2OriginalSize = panel2.Size;

            btnHome.Checked = false;
            btnDetainls.Checked = true;

            //mainPanel.Controls.Clear();
            openedForms.Remove("frmFinancialCharge");

            AddControls(new frmFinancialCharge());
        }

        private void btnFinsh_Click(object sender, EventArgs e)
        {
            panel1OriginalLocation = panel1.Location;
            panel1OriginalSize = panel1.Size;

            panel2OriginalLocation = panel2.Location;
            panel2OriginalSize = panel2.Size;

            btnHome.Checked = false;
            btnDetainls.Checked = false;
        }

        private void btnBillDetails_Click(object sender, EventArgs e)
        {
            panel1OriginalLocation = panel1.Location;
            panel1OriginalSize = panel1.Size;

            panel2OriginalLocation = panel2.Location;
            panel2OriginalSize = panel2.Size;

            mainPanel.Controls.Clear();
            openedForms.Remove("frmAll_Bills");

            frmAll_Bills frm = new frmAll_Bills
            {
                pos = false,
                isFinancial = true,
                invoiceCode = InvoiceCode,
                partyType = this.partyType
            };
            AddControls(frm);
        }
        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();

        public void AddControls(Form f)
        {
            // إخفاء أي فورم معروضة حاليًا
            foreach (var frm in openedForms.Values)
            {
                frm.Hide();
            }

            if (openedForms.ContainsKey(f.Name))
            {
                var existingForm = openedForms[f.Name];

                if (existingForm.IsDisposed)
                {
                    existingForm = CreateNewFormInstance(f.Name);
                    openedForms[f.Name] = existingForm;
                    PrepareForm(existingForm);
                }

                existingForm.Show();
                existingForm.BringToFront();

                // تنفيذ الحدث لو الفورم تدعم IRefreshableForm
                if (existingForm is IRefreshableForm refreshable)
                {
                    refreshable.OnFormShownAgain();
                }
            }
            else
            {
                PrepareForm(f);
                openedForms.Add(f.Name, f);
                f.Show();
            }
        }
        private Form CreateNewFormInstance(string formName)
        {
            // الحصول على مجمع البرنامج الحالي حيث يتم تعريف النموذج
            Assembly assembly = Assembly.GetExecutingAssembly();

            // محاولة إنشاء مثيل للنموذج باستخدام اسمه
            // يُفترض أن اسم النموذج يتطابق مع الاسم الكامل للصنف بما في ذلك مساحة الاسم
            Type formType = assembly.GetType(formName);

            if (formType == null)
            {
                throw new ArgumentException($"No form found with the name {formName}.");
            }

            object formInstance = Activator.CreateInstance(formType);
            if (formInstance == null || !(formInstance is Form))
            {
                throw new ArgumentException($"The type {formName} is not a Form.");
            }

            return (Form)formInstance;
        }
        private void PrepareForm(Form frm)
        {
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(frm);
        }



        private void btnBack_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(panel1);
            mainPanel.Controls.Add(panel2);

            panel1.Location = panel1OriginalLocation;
            panel1.Size = panel1OriginalSize;

            panel2.Location = panel2OriginalLocation;
            panel2.Size = panel2OriginalSize;

            CenterPanelInForm(panel1);
            CenterPanelInForm(panel2);
        }

        private void showFinancailData_Click(object sender, EventArgs e)
        {
            if (!MainClass.PayCredit)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            txtNote.Text = string.Empty;
            txtNote.PlaceholderText = "اكتب الملاحظات قبل الدفع";

            gbDataFinance.Enabled = true;
            txtNote.Enabled = true;
            txtNote.ReadOnly = false;
            txtNote.Focus();
        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            if (!MainClass.PayCredit)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            Rpay();
            frmShowBackup frmshowBackup = new frmShowBackup();
            frmshowBackup.backupType = "DIFFERENTIAL";
            frmshowBackup.showNotification = false;
            frmshowBackup.ShowDialog(this);
        }
        private double amountPaid;
        private string personName;
        private double prevBalance;
        private double newBalance;

        private void Rpay()
        {
            // ✅ تحقق من إدخال المستخدم
            if (string.IsNullOrWhiteSpace(txtRPay.Text) || string.IsNullOrWhiteSpace(txtDelivery.Text))
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة.");
                return;
            }

            // ✅ قراءة القيم بأمان
            if (!double.TryParse(txtRPay.Text, out double amountPaid))
            {
                MessageBox.Show("برجاء إدخال رقم صحيح للمبلغ.");
                return;
            }

            if (amountPaid <= 0)
            {
                MessageBox.Show("المبلغ المدفوع يجب أن يكون أكبر من الصفر.");
                return;
            }

            if (partiesID == 0)
            {
                MessageBox.Show("يرجى اختيار اسم الطرف أولاً.");
                return;
            }

            personName = txtDelivery.Text;

            // ✅ دفع المبلغ الجزئي وحساب الرصيد الجديد
            double prevBalance = 0, newBalance = 0;
            (prevBalance, newBalance) = PayPartialAmount(partiesID, amountPaid);

            // ✅ بعد التحقق من الحساب نبدأ إدخال سجل الدفع
            string qry;

            if (partyType == "عميل")
            {
                qry = @"
                INSERT INTO chargeResidual 
                ([partiesID], [name], [shiftId], [recipt], [change], [date], [time], [note], [previousDebitBalance])
                VALUES (@partiesID, @name, @shiftId, @recipt, @change, @date, @time, @note, @previousDebitBalance);";
            }
            else
            {
                qry = @"
                INSERT INTO chargeResidualSuplieser 
                ([partiesID], [name], [shiftId], [recipt], [change], [date], [time], [note],[previousDebitBalance])
                VALUES (@partiesID, @name, @shiftId, @recipt, @change, @date, @time, @previousDebitBalance);";
            }


            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);
                            cmd.Parameters.AddWithValue("@name", personName);
                            cmd.Parameters.AddWithValue("@shiftId", MainClass.shiftID);
                            cmd.Parameters.AddWithValue("@recipt", amountPaid);
                            cmd.Parameters.AddWithValue("@previousDebitBalance", prevBalance);
                            cmd.Parameters.AddWithValue("@change", newBalance);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
                            cmd.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
                            cmd.Parameters.AddWithValue("@note", (object)txtNote.Text ?? DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error أثناء الحفظ: " + ex.Message);
                        return;
                    }
                }
            }

            // ✅ تحديث واجهة المستخدم
            gbDataFinance.Enabled = false;

            txtPreviousBebitBalance.Text = prevBalance.ToString("N2");
            txtCurrentDebitBalance.Text = newBalance.ToString("N2");

            txtRPay.Text = string.Empty;
            txtNote.Text = string.Empty;
            txtNote.PlaceholderText = "ملاحظات";

            Notifier.ShowNotification("تم الدفع", "تم دفع المبلغ بنجاح");
            transactions(amountPaid, newBalance, prevBalance);
        }

        private (double previousBalance, double newBalance) PayPartialAmount(int partiesID, double amountPaid)
        {
            double previousBalance = 0;
            double newBalance = 0;
            string status = "";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ نجيب الرصيد الحالي
                        string getQuery = "SELECT currentDebitBalance FROM residualTable WHERE PartiesID = @PartiesID";
                        using (SqlCommand getCmd = new SqlCommand(getQuery, con, tran))
                        {
                            getCmd.Parameters.AddWithValue("@PartiesID", partiesID);
                            object result = getCmd.ExecuteScalar();
                            if (result == null)
                                throw new Exception("PartiesID not found in residualTable.");

                            previousBalance = Convert.ToDouble(result);
                        }

                        // 2️⃣ نحسب الرصيد الجديد
                        newBalance = previousBalance - amountPaid;

                        // 3️⃣ نحدد الحالة
                        if (newBalance == 0)
                            status = "مسدد";
                        else if (newBalance < 0)
                            status = "دائن";
                        else
                            status = "مدين";

                        // 4️⃣ نحدث القيم في الجدول
                        string updateQuery = @"
                    UPDATE residualTable
                    SET 
                        status = @status,
                        previousDebitBalance = @previous,
                        currentDebitBalance = @current
                    WHERE PartiesID = @PartiesID;
                ";

                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, con, tran))
                        {
                            updateCmd.Parameters.AddWithValue("@status", status);
                            updateCmd.Parameters.AddWithValue("@previous", previousBalance);
                            updateCmd.Parameters.AddWithValue("@current", newBalance);
                            updateCmd.Parameters.AddWithValue("@PartiesID", partiesID);
                            updateCmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

            return (previousBalance, newBalance);
        }


        private void transactions(double amoutPaied, double currentBalance, double prevBalance)
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
                            cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
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

        private void txtDelivery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtRPay.Focus();
                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private void txtRPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void txtRPay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Rpay();

                e.Handled = true; // يمنع التصرف الافتراضي
            }
        }

        private void cbSearchMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChooseParyties.SelectedIndex == 0)
            {
                partyType = "عميل";

                textSuggester();
            }
            else
            {
                partyType = "مورد";
                textSuggester();
            }
        }

        private async void btnPrint_Click_1(object sender, EventArgs e)
        {
            await MainClass.BillStatmentPrintAsync(mainID, amountPaid, prevBalance, newBalance, partiesID, "", txtDelivery.Text, MainClass.USER);
        }

        private void dgvBills_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var colName = dgvBills.Columns[e.ColumnIndex].Name;
            if (colName == "Column4" || colName == "Column3")
            {
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    if (decimal.TryParse(e.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    {
                        string s = d.ToString("N1", CultureInfo.InvariantCulture);
                        // بدّل الهايڤن بـ "علامة ناقص" U+2212 (اختياري لكنه يساعد)
                        s = s.Replace("-", "\u2212");
                        e.Value = "\u200E" + s;   // LRM قبل الرقم
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string colName = dgvProducts.Columns[e.ColumnIndex].Name;

            // ✅ تنسيق عمود الكمية
            if (colName == "dgvQty")
            {
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    if (decimal.TryParse(e.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    {
                        string s = d.ToString("N1", CultureInfo.InvariantCulture);
                        s = s.Replace("-", "\u2212"); // استبدال الهايڤن بعلامة ناقص فعلية
                        e.Value = "\u200E" + s;       // إضافة LRM لمنع انعكاس الأرقام بالعربية
                        e.FormattingApplied = true;
                    }
                }
            }

            // ✅ إخفاء أيقونة "Edit" في الصفوف غير الأخيرة
            if (dgvProducts.Columns.Contains("dgvEdit"))
            {
                int lastRowIndex = dgvProducts.Rows.Count - 1;

                if (e.ColumnIndex == dgvProducts.Columns["dgvEdit"].Index)
                {
                    if (e.RowIndex != lastRowIndex)
                    {
                        // ✨ تأكد إن العمود فعلاً ImageColumn قبل ما تمسح الصورة
                        if (dgvProducts.Columns["dgvEdit"] is DataGridViewImageColumn)
                        {
                            e.Value = new Bitmap(1, 1); // صورة فاضية صغيرة بدل null (تمنع الخطأ)
                            e.FormattingApplied = true;
                        }
                    }
                }
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!MainClass.AddCreditCustomer)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            // 🔹 تأكيد الحذف
            DialogResult result = MessageBox.Show(
                "هل أنت متأكد أنك تريد حذف هذا العميل من قائمة الدفع بالأجل؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No)
                return;

            string qry = @"DELETE FROM residualTable WHERE PartiesID = @PartiesID;
                   DELETE FROM chargeResidual WHERE partiesID = @PartiesID;";

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
                        // ✅ تمرير المعاملة هنا
                        using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@PartiesID", partiesID);
                            cmd.ExecuteNonQuery();
                        }

                        decimal prev = 0, curr = 0;
                        decimal.TryParse(txtPreviousBebitBalance.Text.Replace(",", ""), out prev);
                        decimal.TryParse(txtCurrentDebitBalance.Text.Replace(",", ""), out curr);

                        using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                        {
                            cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                            cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "حذف العميل من الاجل");
                            cmdTransaction.Parameters.AddWithValue("@mainID", DBNull.Value);
                            cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo", "تم حذف هذا العميل من قائمة الدفع بالاجل");
                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", prev);
                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", curr);
                            cmdTransaction.ExecuteNonQuery();
                        }

                        // ✅ تأكيد التغييرات
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

            // 🔹 إشعار بالنجاح
            Notifier.ShowNotification("تم الحذف", "تم حذف العميل من قائمة الدفع بالأجل بنجاح ✅");
        }

        private void btnPartySearch_Click(object sender, EventArgs e)
        {
            if (cbChooseParyties.SelectedIndex == 0)
            {
                partyType = "عميل";
            }
            else
            {
                partyType = "مورد";
            }

            frmPartesSearch frm = new frmPartesSearch(this);
            frm.type = partyType;
            frm.ShowDialog();
            this.Focus();
        }
        public void resultSearch(string pName, int partyID)
        {
            txtName.Text = pName;
            partiesID = partyID;
            click();
        }

        private async void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProducts.Columns["dgvPrint"].Index && e.RowIndex >= 0)
            {
                var row = dgvProducts.Rows[e.RowIndex];

                // Parse ID
                //if (row.Cells["dgvID"].Value != null)
                //    partiesID = Convert.ToInt32(row.Cells["dgvID"].Value);
                // Balances
                newBalance = Convert.ToDouble(row.Cells["dgvQty"].Value ?? 0);
                amountPaid = Convert.ToDouble(row.Cells["dgvUnit"].Value ?? 0);
                prevBalance = newBalance + amountPaid;

                // Strings
                string delivery = row.Cells["dgvName"].Value?.ToString();
                string parties = row.Cells["dgvCategory"].Value?.ToString();
                string time = row.Cells["dgvTime"].Value?.ToString();
                string date = row.Cells["dgvDate"].Value?.ToString();
                // Call printer
                await MainClass.BillStatmentPrintAsync(0, amountPaid, prevBalance, newBalance, partiesID, "", delivery, parties, 0, date, time);

            }
            else if (e.ColumnIndex == dgvProducts.Columns["dgvEdit"].Index && e.RowIndex >= 0)
            {
                if (!MainClass.EditDebtorBalance)
                {
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                    guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                    return;
                }
                var row = dgvProducts.Rows[e.RowIndex];

                frmEditeCarge frm = new frmEditeCarge();
                frm.partyType = partyType;
                frm.partiesID = partiesID;
                frm.chargeID = Convert.ToInt32(row.Cells["dgvID"].Value);
                frm.charge = Convert.ToDouble(row.Cells["dgvUnit"].Value ?? 0);
                frm.currentCharge = Convert.ToDouble(row.Cells["dgvQty"].Value ?? 0);
                frm.ShowDialog();
                this.Focus();
                click();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!MainClass.AddCreditCustomer)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (txtAdd.Text == string.Empty || Convert.ToDouble(txtAdd.Text) == 0)
            {
                Notifier.ShowNotification("تنبية", $"الرجاء ادخال رصيد صحيح");
                txtAdd.Text = string.Empty;
                txtAdd.Focus();
                return;
            }
            residualBillCustomer();
        }
        private bool partiesIsFound()
        {
            string qry = @"
            SELECT 1
            FROM residualTable 
            WHERE PartiesID = @PartiesID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartiesID", partiesID);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // لو فيه صف => الشخص موجود => رجع true
                    return reader.Read();
                }
            }
        }


        private void residualBillCustomer()
        {
            string qry = string.Empty;
            if (!partiesIsFound())
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
                        double current = Convert.ToDouble(txtCurrentDebitBalance.Text == string.Empty ? "0" : txtCurrentDebitBalance.Text);
                        double add = Convert.ToDouble(txtAdd.Text == string.Empty ? "0" : txtAdd.Text);
                        double newCurrent = current + add;

                        // أولاً: PartiesTransactions
                        using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                        {
                            cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                            cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "اضافة");
                            cmdTransaction.Parameters.AddWithValue("@mainID", DBNull.Value);
                            cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                              $"تم إضافة مبلغ أجل الي هذا العميل بقيمة  {Convert.ToDecimal(add).ToString("N0")}");
                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDecimal(current.ToString("N0")));
                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDecimal(newCurrent.ToString("N0")));
                            cmdTransaction.ExecuteNonQuery();
                        }

                        // ثانياً: residualTable
                        using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                        {

                            cmd.Parameters.AddWithValue("@PartiesID", partiesID);
                            cmd.Parameters.AddWithValue("@status", "مدين");
                            cmd.Parameters.AddWithValue("@isCustomer", (partyType == "عميل"));
                            cmd.Parameters.AddWithValue("@totalPaid", 0);
                            cmd.Parameters.AddWithValue("@totalTransaction", Convert.ToDecimal(add.ToString("N0")));
                            cmd.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDecimal(current.ToString("N0")));

                            cmd.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDecimal(add.ToString("N0")));

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
            click();
            Notifier.ShowNotification("تم", $" ✅ تم اضافة مبلغ الي قائمة الاجل لشخص بنجاح");
            txtAdd.Text = string.Empty;
        }

        private void btnEditeBalance_Click(object sender, EventArgs e)
        {
            if (!MainClass.EditDebtorBalance)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (txtEditeBalance.Text == string.Empty || Convert.ToDouble(txtEditeBalance.Text) == 0)
            {
                Notifier.ShowNotification("تنبية", $"الرجاء ادخال رصيد صحيح");
                txtEditeBalance.Text = string.Empty;
                txtEditeBalance.Focus();
                return;
            }
            string qry = string.Empty;
            if (partiesIsFound())
            {
                qry = @"
                UPDATE residualTable 
                SET
                    [status] = @status, 
                    currentDebitBalance = @currentDebitBalance
                WHERE PartiesID = @PartiesID;";

            }
            else
            {

                Notifier.ShowNotification("تنبية", $"هذا الشخص غير موجود في قائمة الاجل");

                return;
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
                        double current = Convert.ToDouble(txtCurrentDebitBalance.Text == string.Empty ? "0" : txtCurrentDebitBalance.Text);
                        double newCurrent = Convert.ToDouble(txtEditeBalance.Text == string.Empty ? "0" : txtEditeBalance.Text);

                        // أولاً: PartiesTransactions
                        using (SqlCommand cmdTransaction = new SqlCommand(qtyTransaction, con, tran))
                        {
                            cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                            cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "تعديل");
                            cmdTransaction.Parameters.AddWithValue("@mainID", DBNull.Value);
                            cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                              $"تم تعديل رصيد المدين الحلي لهذا العميل الي قيمة  {Convert.ToDecimal(newCurrent).ToString("N0")}");
                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDecimal(current.ToString("N0")));
                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDecimal(newCurrent.ToString("N0")));
                            cmdTransaction.ExecuteNonQuery();
                        }

                        // ثانياً: residualTable
                        using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                        {

                            cmd.Parameters.AddWithValue("@PartiesID", partiesID);
                            cmd.Parameters.AddWithValue("@status", "مدين");
                            cmd.Parameters.AddWithValue("@isCustomer", (partyType == "عميل"));
                            cmd.Parameters.AddWithValue("@totalPaid", 0);
                            cmd.Parameters.AddWithValue("@totalTransaction", Convert.ToDecimal(newCurrent.ToString("N0")));
                            cmd.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDecimal(current.ToString("N0")));

                            cmd.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDecimal(newCurrent.ToString("N0")));

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
            click();
            Notifier.ShowNotification("تم", $" ✅ تم تعديل رصيد المدين الحالي لهذا الشخص بنجاح");
            txtEditeBalance.Text = string.Empty;

        }

        private void txtEditeBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام
            if (char.IsDigit(e.KeyChar))
                return;

            // السماح بالنقطة العشرية (مرة واحدة فقط)
            if (e.KeyChar == '.' && !txtEditeBalance.Text.Contains("."))
                return;

            // السماح بالسالب في البداية فقط
            if (e.KeyChar == '-' && txtEditeBalance.SelectionStart == 0 && !txtEditeBalance.Text.Contains("-"))
                return;

            // السماح بمفتاح الحذف Backspace
            if (e.KeyChar == (char)Keys.Back)
                return;

            // إذا لم يكن أي مما سبق، نمنع الإدخال
            e.Handled = true;
        }

        private void txtAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام
            if (char.IsDigit(e.KeyChar))
                return;

            // السماح بالنقطة العشرية (مرة واحدة فقط)
            if (e.KeyChar == '.' && !txtAdd.Text.Contains("."))
                return;

            // السماح بالسالب في البداية فقط
            if (e.KeyChar == '-' && txtAdd.SelectionStart == 0 && !txtAdd.Text.Contains("-"))
                return;

            // السماح بمفتاح الحذف Backspace
            if (e.KeyChar == (char)Keys.Back)
                return;

            // إذا لم يكن أي مما سبق، نمنع الإدخال
            e.Handled = true;
        }

        private void txtWithdraw_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (!MainClass.WithdrawCreditor)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            if (txtWithdraw.Text == string.Empty || Convert.ToDouble(txtWithdraw.Text) <= 0)
            {
                Notifier.ShowNotification("تنبية", $"الرجاء ادخال رصيد صحيح");
                txtWithdraw.Text = string.Empty;
                txtWithdraw.Focus();
                return;
            }
            if (!partiesIsFound())
            {
                Notifier.ShowNotification("تنبيه", $"هذا الشخص غير موجود في قائمة الأجل");
                return;
            }

            double current = Convert.ToDouble(txtCurrentDebitBalance.Text == string.Empty ? "0" : txtCurrentDebitBalance.Text);
            double withdrawAmount = Convert.ToDouble(txtWithdraw.Text == string.Empty ? "0" : txtWithdraw.Text);
            double newCurrent = current + withdrawAmount; // لأن الرصيد بالسالب ممكن يكون current < 0

            // تحقق من السحب مقابل الرصيد المتاح
            if (newCurrent > 0)
            {
                Notifier.ShowNotification("تنبيه", $"الرصيد غير كافي، لا يمكن السحب أكثر من: {Math.Abs(current):N0}");
                return;
            }

            string qry = @"
            UPDATE residualTable 
            SET
                [status] = @status, 
                isCustomer = @isCustomer, 
                totalPaid = ISNULL(totalPaid, 0) + @totalPaid,
                totalTransaction = ISNULL(totalTransaction, 0) + @totalTransaction,
                previousDebitBalance = @previousDebitBalance, 
                currentDebitBalance = ISNULL(currentDebitBalance, 0) + @currentDebitBalance
            WHERE PartiesID = @PartiesID;";

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
                            cmdTransaction.Parameters.AddWithValue("@partiesID", partiesID);
                            cmdTransaction.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmdTransaction.Parameters.AddWithValue("@transactionsType", "سحب");
                            cmdTransaction.Parameters.AddWithValue("@mainID", DBNull.Value);
                            cmdTransaction.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                            cmdTransaction.Parameters.AddWithValue("@transactionsInfo",
                              $"تم سحب مبلغ: {withdrawAmount:N0}");
                            cmdTransaction.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDecimal(current.ToString("N0")));
                            cmdTransaction.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDecimal(newCurrent.ToString("N0")));
                            cmdTransaction.ExecuteNonQuery();
                        }

                        // ثانياً: residualTable
                        using (SqlCommand cmd = new SqlCommand(qry, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@PartiesID", partiesID);
                            cmd.Parameters.AddWithValue("@status", "مدين");
                            cmd.Parameters.AddWithValue("@isCustomer", (partyType == "عميل"));
                            cmd.Parameters.AddWithValue("@totalPaid", 0);
                            cmd.Parameters.AddWithValue("@totalTransaction", Convert.ToDecimal(withdrawAmount.ToString("N0")));
                            cmd.Parameters.AddWithValue("@previousDebitBalance", Convert.ToDecimal(current.ToString("N0")));
                            cmd.Parameters.AddWithValue("@currentDebitBalance", Convert.ToDecimal(withdrawAmount.ToString("N0")));
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

            click();
            Notifier.ShowNotification("تم", $" ✅ تم سحب هذا المبلغ بنجاح");
            txtWithdraw.Text = string.Empty;
        }

        private void dgvProducts_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.Columns[e.ColumnIndex].Name == "dgvEdit")
            {
                int lastRowIndex = dgvProducts.Rows.Count - 1;

                if (e.RowIndex != lastRowIndex) // أي صف غير الأخير
                {
                    e.PaintBackground(e.CellBounds, true); // يرسم الخلفية بس
                    e.Handled = true; // يمنع رسم الصورة
                }
            }
            if (e.RowIndex == -1 && dgvProducts.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvProducts.CurrentCell.ColumnIndex)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, true);

                    // ارسم النص بلون مختلف للهيدر المحدد
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.FormattedValue?.ToString(),
                        new Font("Tahoma", 11, FontStyle.Bold),
                        e.CellBounds,
                        Color.FromArgb(204, 204, 204),              // ← لون خط الهيدر المحدد
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    // ارسم حدود الخلية
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                }
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // تجاهل الضغط على الهيدر أو أي صف غير صالح
            if (e.RowIndex < 0)
                return;

            var row = dgvProducts.Rows[e.RowIndex];

            // Balances
            string note = row.Cells["dgvNote"].Value?.ToString() ?? string.Empty;
            txtNote.Text = note;
        }

        private void dgvBills_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvBills.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvBills.CurrentCell.ColumnIndex)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, true);

                    // ارسم النص بلون مختلف للهيدر المحدد
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.FormattedValue?.ToString(),
                        new Font("Tahoma", 11, FontStyle.Bold),
                        e.CellBounds,
                        Color.FromArgb(204, 204, 204),              // ← لون خط الهيدر المحدد
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    // ارسم حدود الخلية
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                }
            }
        }
        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.Visible = true;
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;

            // أحجام الخلايا والهيدر
            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 45;

            // الخطوط
            dgv.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

            // الألوان العادية للصفوف
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);

            // الصفوف المتبادلة (صف غامق وصف فاتح)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // الرمادي الفاتح
            dgv.RowsDefaultCellStyle.BackColor = Color.White;                              // الصف العادي

            // ألوان التحديد (Selection)
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // ✅ ألوان الهيدر (عادي + خط)
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // ✅ ضبط حجم الأعمدة تلقائياً
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (dgv == dgvProducts)
                {
                    if (col.Name.Equals("dgvQty", StringComparison.OrdinalIgnoreCase))
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;  // العمود الخاص بالكمية يملأ المساحة
                    else
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // باقي الأعمدة تتناسب مع محتواها
                }
                else if (dgv == dgvProducts)
                {
                    if (col.Name.Equals("Column5", StringComparison.OrdinalIgnoreCase))
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;  // العمود الخاص بالكمية يملأ المساحة
                    else
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // باقي الأعمدة تتناسب مع محتواها
                }

            }
        }


        private async void dgvBills_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                // ✅ تأكد إن فيه صفوف متعرضة
                if (dgvBills.RowCount == 0 || dgvBills.FirstDisplayedScrollingRowIndex < 0)
                    return;

                if (dgvBills.FirstDisplayedScrollingRowIndex + dgvBills.DisplayedRowCount(false) >= dgvBills.RowCount)
                {
                    await displayPartiesResdule(type, false); // يجيب الصفحة اللي بعدها
                }
            }
        }

        private async void dgvProducts_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvProducts.RowCount == 0 || dgvProducts.FirstDisplayedScrollingRowIndex < 0)
                    return;

                if (dgvProducts.FirstDisplayedScrollingRowIndex + dgvProducts.DisplayedRowCount(false) >= dgvProducts.RowCount)
                {
                    // تحميل الصفحة التالية
                    await displayBillsByPartiesName(currentPartiesId, false);
                }
            }
        }

        private async void btnTransfarePrint_Click(object sender, EventArgs e)
        {
            await MainClass.PrintPartiesReportAsync2(
                new DateTime(1753, 1, 1),
                DateTime.MaxValue,
                partiesID,
                txtName.Text,
                cbChooseParyties.SelectedIndex == 1,
                showAll: true
            );
        }

        private void btnBills_Click(object sender, EventArgs e)
        {
            using (frmBlackout frmblackout = new frmBlackout(this))
            {
                frmblackout.Show();
                frmAll_Bills frm = new frmAll_Bills(txtName.Text, partiesID, cbChooseParyties.Text);
                frm.ShowDialog();
            }

        }
    }
}
