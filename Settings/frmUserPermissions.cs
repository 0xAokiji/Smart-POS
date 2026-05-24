using Guna.UI2.WinForms;
using pos.Classes;
using pos.SystemApp;
using pos.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;

namespace pos.AccountManagement
{
    public partial class frmUserPermissions : Form
    {
        //-> Dark Mode
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color checkedFillColor;
        private Color checkedForColor;

        public bool FirstTime = false;
        public frmUserPermissions()
        {
            InitializeComponent();
           
            ThemRefresh();
        }
        public frmUserPermissions(bool newTime)
        {
            InitializeComponent();


            ThemRefresh();
        }

        public void ThemRefresh()
        {
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
        int staffID = 0;
        int count = 0;
        string qry = string.Empty;
        bool isHandlingCheckedChanged = false;

        bool role;
        private void btnSavex_Click(object sender, EventArgs e)
        {
            // 1️⃣ جمع القيم من CheckBoxes
            bool rebortMaint = cbRebortMaint.Checked;
            bool billMaint = cbBillMaint.Checked;
            bool editeDevice = cbEditeDevice.Checked;
            bool deleteDevice = cbDeleteDevice.Checked;
            bool increasePrice = cbIncreasePrice.Checked;
            bool proCardAdd = cbProCardAdd.Checked;
            bool proCardEdite = cbProCardEdite.Checked;
            bool proCardDetete = cbProCardDetete.Checked;
            bool staffShow = cbStaffShow.Checked;
            bool staffAdd = cbStaffAdd.Checked;
            bool staffEdite = cbStaffEdite.Checked;
            bool staffDelete = cbStaffDelete.Checked;
            bool addUser = cbAddUser.Checked;
            bool changePass = cbChangePass.Checked;
            bool userPermission = cbUserPermission.Checked;
            bool saveBackup = cbSaveBackup.Checked;
            bool reBackup = cbReBackup.Checked;
            bool backupPath = cbBackupPath.Checked;
            bool reportFinance = cbReportFinance.Checked;
            bool partiesBalance = cbPartiesBalance.Checked;
            bool wholeSale = cbWholeSale.Checked;
            bool halfWholeSale = cbHalfWholeSale.Checked;
            bool showReturns = cbShowReturns.Checked;
            bool addSupplierBill = cbAddSupplierBill.Checked;
            bool showSupplierBills = cbShowSupplierBills.Checked;
            bool showDeletedSupBills = cbShowDeletedSupBills.Checked;
            bool showCustomerBills = cbShowCustomerBills.Checked;
            bool showDeletedCusBills = cbShowDeletedCusBills.Checked;
            bool showStoreBalance = cbShowStoreBalance.Checked;
            bool showSuppliers = cbShowSuppliers.Checked;
            bool addStore = cbAddStore.Checked;
            bool openStore = cbOpenStore.Checked;
            bool financePage = cbFinancePage.Checked;
            bool editDebtorBalance = cbEditDebtorBalance.Checked;
            bool withdrawCreditor = cbWithdrawCreditor.Checked;
            bool addCreditCustomer = cbAddCreditCustomer.Checked;
            bool payCredit = cbPayCredit.Checked;
            bool showShortages = cbShowShortages.Checked;
            bool showPurchases = cbShowPurchases.Checked;
            bool addExpenses = cbAddExpenses.Checked;
            bool salaries = cbSalaries.Checked;
            bool showCategories = cbShowCategories.Checked;
            bool editCompanyInfo = cbEditCompanyInfo.Checked;

            // ✅ الإضافات الجديدة
            bool installmentPos = cbInstallmentPos.Checked;
            bool deferredPos = cbDeferredPos.Checked;
            bool canResetSystem = cbCanResetSystem.Checked;

            // 2️⃣ التوقيع
            string signature = PermissionSigner.ComputeSignature(
                staffID,
                rebortMaint, billMaint, editeDevice, deleteDevice, increasePrice,
                proCardAdd, proCardEdite, proCardDetete,
                staffShow, staffAdd, staffEdite, staffDelete,
                addUser, changePass, userPermission,
                saveBackup, reBackup, backupPath,
                reportFinance, partiesBalance, wholeSale, halfWholeSale,
                showReturns, addSupplierBill, showSupplierBills, showDeletedSupBills,
                showCustomerBills, showDeletedCusBills, showStoreBalance, showSuppliers,
                addStore, openStore, financePage, editDebtorBalance, withdrawCreditor,
                addCreditCustomer, payCredit, showShortages, showPurchases,
                addExpenses, salaries, showCategories, canResetSystem, installmentPos,
                deferredPos, editCompanyInfo
            );

            using (SqlConnection con = MainClass.GetConnection())
            {
                string qryCheck = "SELECT staffID FROM userPermissions WHERE staffID = @staffID";
                int count = 0;
                using (SqlCommand cmdCheck = new SqlCommand(qryCheck, con))
                {
                    cmdCheck.Parameters.AddWithValue("@staffID", staffID);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmdCheck);
                    da.Fill(dt);
                    count = dt.Rows.Count;
                }

                string qry;
                if (count == 0)
                {
                    qry = @"INSERT INTO [dbo].[userPermissions]
            (
                staffID, InstallmentPos, DeferredPos, RebortMaint, BillMaint, EditeDevice, DeleteDevice, IncreasePrice,
                ProCardAdd, ProCardEdite, ProCardDetete,
                StaffShow, StaffAdd, StaffEdite, StaffDelete, AddUser, ChangePass,
                UserPermission, SaveBackup, ReBackup, BackupPath, Signature, ReportFinance, PartiesBalance, WholeSale,
                HalfWholeSale, ShowReturns, AddSupplierBill, ShowSupplierBills, ShowDeletedSupBills, ShowCustomerBills,
                ShowDeletedCusBills, ShowStoreBalance, ShowSuppliers, AddStore, OpenStore, FinancePage, EditDebtorBalance,
                WithdrawCreditor, AddCreditCustomer, PayCredit, ShowShortages, ShowPurchases, AddExpenses, Salaries,
                ShowCategories, CanResetSystem, deitCompanyInfo
            )
            VALUES (
                @staffID, @InstallmentPos, @DeferredPos, @RebortMaint, @BillMaint, @EditeDevice, @DeleteDevice, @IncreasePrice,
                @ProCardAdd, @ProCardEdite, @ProCardDetete,
                @StaffShow, @StaffAdd, @StaffEdite, @StaffDelete, @AddUser, @ChangePass,
                @UserPermission, @SaveBackup, @ReBackup, @BackupPath, @Signature, @ReportFinance, @PartiesBalance, @WholeSale,
                @HalfWholeSale, @ShowReturns, @AddSupplierBill, @ShowSupplierBills, @ShowDeletedSupBills, @ShowCustomerBills,
                @ShowDeletedCusBills, @ShowStoreBalance, @ShowSuppliers, @AddStore, @OpenStore, @FinancePage, @EditDebtorBalance,
                @WithdrawCreditor, @AddCreditCustomer, @PayCredit, @ShowShortages, @ShowPurchases, @AddExpenses, @Salaries,
                @ShowCategories, @CanResetSystem, @editCompanyInfo
            )";
                }
                else
                {
                    qry = @"UPDATE [dbo].[userPermissions] SET
                    InstallmentPos = @InstallmentPos,
                    DeferredPos = @DeferredPos,
                    RebortMaint = @RebortMaint,
                    BillMaint = @BillMaint,
                    EditeDevice = @EditeDevice,
                    DeleteDevice = @DeleteDevice,
                    IncreasePrice = @IncreasePrice,
                    ProCardAdd = @ProCardAdd,
                    ProCardEdite = @ProCardEdite,
                    ProCardDetete = @ProCardDetete,
                    StaffShow = @StaffShow,
                    StaffAdd = @StaffAdd,
                    StaffEdite = @StaffEdite,
                    StaffDelete = @StaffDelete,
                    AddUser = @AddUser,
                    ChangePass = @ChangePass,
                    UserPermission = @UserPermission,
                    SaveBackup = @SaveBackup,
                    ReBackup = @ReBackup,
                    BackupPath = @BackupPath,
                    ReportFinance = @ReportFinance,
                    PartiesBalance = @PartiesBalance,
                    WholeSale = @WholeSale,
                    HalfWholeSale = @HalfWholeSale,
                    ShowReturns = @ShowReturns,
                    AddSupplierBill = @AddSupplierBill,
                    ShowSupplierBills = @ShowSupplierBills,
                    ShowDeletedSupBills = @ShowDeletedSupBills,
                    ShowCustomerBills = @ShowCustomerBills,
                    ShowDeletedCusBills = @ShowDeletedCusBills,
                    ShowStoreBalance = @ShowStoreBalance,
                    ShowSuppliers = @ShowSuppliers,
                    AddStore = @AddStore,
                    OpenStore = @OpenStore,
                    FinancePage = @FinancePage,
                    EditDebtorBalance = @EditDebtorBalance,
                    WithdrawCreditor = @WithdrawCreditor,
                    AddCreditCustomer = @AddCreditCustomer,
                    PayCredit = @PayCredit,
                    ShowShortages = @ShowShortages,
                    ShowPurchases = @ShowPurchases,
                    AddExpenses = @AddExpenses,
                    Salaries = @Salaries,
                    ShowCategories = @ShowCategories,
                    CanResetSystem = @CanResetSystem,
                    deitCompanyInfo = @editCompanyInfo,
                    Signature = @Signature
                WHERE staffID = @staffID";
                }

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.Parameters.AddWithValue("@InstallmentPos", installmentPos);
                    cmd.Parameters.AddWithValue("@DeferredPos", deferredPos);
                    cmd.Parameters.AddWithValue("@RebortMaint", rebortMaint);
                    cmd.Parameters.AddWithValue("@BillMaint", billMaint);
                    cmd.Parameters.AddWithValue("@EditeDevice", editeDevice);
                    cmd.Parameters.AddWithValue("@DeleteDevice", deleteDevice);
                    cmd.Parameters.AddWithValue("@IncreasePrice", increasePrice);
                    cmd.Parameters.AddWithValue("@ProCardAdd", proCardAdd);
                    cmd.Parameters.AddWithValue("@ProCardEdite", proCardEdite);
                    cmd.Parameters.AddWithValue("@ProCardDetete", proCardDetete);
                    cmd.Parameters.AddWithValue("@StaffShow", staffShow);
                    cmd.Parameters.AddWithValue("@StaffAdd", staffAdd);
                    cmd.Parameters.AddWithValue("@StaffEdite", staffEdite);
                    cmd.Parameters.AddWithValue("@StaffDelete", staffDelete);
                    cmd.Parameters.AddWithValue("@AddUser", addUser);
                    cmd.Parameters.AddWithValue("@ChangePass", changePass);
                    cmd.Parameters.AddWithValue("@UserPermission", userPermission);
                    cmd.Parameters.AddWithValue("@SaveBackup", saveBackup);
                    cmd.Parameters.AddWithValue("@ReBackup", reBackup);
                    cmd.Parameters.AddWithValue("@BackupPath", backupPath);
                    cmd.Parameters.AddWithValue("@ReportFinance", reportFinance);
                    cmd.Parameters.AddWithValue("@PartiesBalance", partiesBalance);
                    cmd.Parameters.AddWithValue("@WholeSale", wholeSale);
                    cmd.Parameters.AddWithValue("@HalfWholeSale", halfWholeSale);
                    cmd.Parameters.AddWithValue("@ShowReturns", showReturns);
                    cmd.Parameters.AddWithValue("@AddSupplierBill", addSupplierBill);
                    cmd.Parameters.AddWithValue("@ShowSupplierBills", showSupplierBills);
                    cmd.Parameters.AddWithValue("@ShowDeletedSupBills", showDeletedSupBills);
                    cmd.Parameters.AddWithValue("@ShowCustomerBills", showCustomerBills);
                    cmd.Parameters.AddWithValue("@ShowDeletedCusBills", showDeletedCusBills);
                    cmd.Parameters.AddWithValue("@ShowStoreBalance", showStoreBalance);
                    cmd.Parameters.AddWithValue("@ShowSuppliers", showSuppliers);
                    cmd.Parameters.AddWithValue("@AddStore", addStore);
                    cmd.Parameters.AddWithValue("@OpenStore", openStore);
                    cmd.Parameters.AddWithValue("@FinancePage", financePage);
                    cmd.Parameters.AddWithValue("@EditDebtorBalance", editDebtorBalance);
                    cmd.Parameters.AddWithValue("@WithdrawCreditor", withdrawCreditor);
                    cmd.Parameters.AddWithValue("@AddCreditCustomer", addCreditCustomer);
                    cmd.Parameters.AddWithValue("@PayCredit", payCredit);
                    cmd.Parameters.AddWithValue("@ShowShortages", showShortages);
                    cmd.Parameters.AddWithValue("@ShowPurchases", showPurchases);
                    cmd.Parameters.AddWithValue("@AddExpenses", addExpenses);
                    cmd.Parameters.AddWithValue("@Salaries", salaries);
                    cmd.Parameters.AddWithValue("@ShowCategories", showCategories);
                    cmd.Parameters.AddWithValue("@CanResetSystem", canResetSystem);
                    cmd.Parameters.AddWithValue("@editCompanyInfo", editCompanyInfo);
                    cmd.Parameters.AddWithValue("@Signature", signature);

                    if (con.State == ConnectionState.Closed) con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            Notifier.ShowNotification("تم", $" ✅  تم حفظ الصلاحيات بنجاح");
            MainClass.LoadUserPermissions(MainClass.UID);
            CheckAllGuna2CheckBoxes(corntrolPanel, false);
            cbUserPermission.Enabled = true;
            cbUserPermission.Checked = false;
            if (FirstTime)
                this.Close();
        }


        private void frmUserPermissions_Load(object sender, EventArgs e)
        {
            if (FirstTime)
            {
                btnClose.Visible = true;
                btnAdmin.Checked = true;
                CheckAllGuna2CheckBoxes(corntrolPanel, true);
            }

            string qry_Tec = "SELECT staff.staffID AS 'id', staff.sName AS 'name' FROM staff INNER JOIN users ON staff.staffID = users.staffID";
            MainClass.CBFill(qry_Tec, comboBoxUser);

            cbUserPermission.Enabled = true;
            cbUserPermission.Checked = false;

            comboBoxUser.SelectedIndex = 0;
        }


        private void comboBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUser.SelectedValue != null && comboBoxUser.SelectedIndex != -1)
            {
                try
                {
                    staffID = Convert.ToInt32(comboBoxUser.SelectedValue);

                    // Desable permission check box form admin
                    string qry = @"SELECT sRole FROM [staff] WHERE staffID = @staffID";

                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@staffID", staffID);
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["sRole"].ToString() == "admin")
                            {
                                cbUserPermission.Checked = true;
                                cbUserPermission.Enabled = false;
                                role = true;
                            }
                            else
                            {
                                cbUserPermission.Enabled = true;
                                role = false;
                            }
                        }
                        else
                        {
                            cbUserPermission.Checked = false;
                            cbUserPermission.Enabled = true;
                        }
                    }

                    CheckAllGuna2CheckBoxes(corntrolPanel, false);
                    loadPermsission(staffID);

                    btnAdmin.Checked = false;
                    btnUser.Checked = false;
                    btnNoPermission.Checked = false;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء تحويل القيمة: " + ex.Message);
                }
            }
            CheckSpecificTextBoxes();
        }

        private void CheckSpecificTextBoxes()
        {
            // افحص تيكست بوكسات معينة
            if (comboBoxUser.SelectedIndex != -1)
            {
                btnSavex2.Enabled = true;
                if (MainClass.ThemeMode == "dark")
                {
                    btnSavex2.FillColor = checkedFillColor;
                    btnSavex2.BackColor = backgroundPrmary;
                }
                else
                {
                    btnSavex2.FillColor = Color.FromArgb(136, 214, 218);
                    btnSavex2.BackColor = Color.FromArgb(243, 243, 243);
                }


            }
            else
            {
                btnSavex2.Enabled = false;
                //btnSavex2.BackColor = Color.Gainsboro;
                btnSavex2.FillColor = Color.DimGray;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CheckAllGuna2CheckBoxes(Control parent, bool state)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CheckBox checkBox)
                {
                    checkBox.Checked = state;
                    Console.WriteLine("✅ Changed: " + checkBox.Name);
                }
                else if (ctrl.HasChildren)
                {
                    CheckAllGuna2CheckBoxes(ctrl, state);
                }
            }

            // 🛑 جملة الـ override دي هي اللي ممكن تلغي كل حاجة
            // لو مش محتاجها إحذفها
            if (role)
            {
                cbUserPermission.Checked = true;
            }
        }


        private void loadPermsission(int sID)
        {
            string query = @"SELECT 
            RebortMaint, BillMaint, EditeDevice, DeleteDevice, IncreasePrice, 
            ProCardAdd, ProCardEdite, ProCardDetete, 
            StaffShow, StaffAdd, StaffEdite, StaffDelete, 
            AddUser, ChangePass, UserPermission, SaveBackup, ReBackup, BackupPath,
            ReportFinance, PartiesBalance, WholeSale, HalfWholeSale, ShowReturns,
            AddSupplierBill, ShowSupplierBills, ShowDeletedSupBills, ShowCustomerBills,
            ShowDeletedCusBills, ShowStoreBalance, ShowSuppliers, AddStore, OpenStore,
            FinancePage, EditDebtorBalance, WithdrawCreditor, AddCreditCustomer, PayCredit,
            ShowShortages, ShowPurchases, AddExpenses, Salaries, ShowCategories, CanResetSystem,
            DeferredPos, InstallmentPos, deitCompanyInfo
        FROM userPermissions
        WHERE staffID = @staffID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@staffID", sID);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // ✅ الأعمدة القديمة
                        cbRebortMaint.Checked = dr.GetBoolean(0);
                        cbBillMaint.Checked = dr.GetBoolean(1);
                        cbEditeDevice.Checked = dr.GetBoolean(2);
                        cbDeleteDevice.Checked = dr.GetBoolean(3);
                        cbIncreasePrice.Checked = dr.GetBoolean(4);
                        cbProCardAdd.Checked = dr.GetBoolean(5);
                        cbProCardEdite.Checked = dr.GetBoolean(6);
                        cbProCardDetete.Checked = dr.GetBoolean(7);
                        cbStaffShow.Checked = dr.GetBoolean(8);
                        cbStaffAdd.Checked = dr.GetBoolean(9);
                        cbStaffEdite.Checked = dr.GetBoolean(10);
                        cbStaffDelete.Checked = dr.GetBoolean(11);
                        cbAddUser.Checked = dr.GetBoolean(12);
                        cbChangePass.Checked = dr.GetBoolean(13);
                        cbUserPermission.Checked = dr.GetBoolean(14);
                        cbSaveBackup.Checked = dr.GetBoolean(15);
                        cbReBackup.Checked = dr.GetBoolean(16);
                        cbBackupPath.Checked = dr.GetBoolean(17);

                        // ✅ الأعمدة الجديدة
                        cbReportFinance.Checked = dr.GetBoolean(18);
                        cbPartiesBalance.Checked = dr.GetBoolean(19);
                        cbWholeSale.Checked = dr.GetBoolean(20);
                        cbHalfWholeSale.Checked = dr.GetBoolean(21);
                        cbShowReturns.Checked = dr.GetBoolean(22);
                        cbAddSupplierBill.Checked = dr.GetBoolean(23);
                        cbShowSupplierBills.Checked = dr.GetBoolean(24);
                        cbShowDeletedSupBills.Checked = dr.GetBoolean(25);
                        cbShowCustomerBills.Checked = dr.GetBoolean(26);
                        cbShowDeletedCusBills.Checked = dr.GetBoolean(27);
                        cbShowStoreBalance.Checked = dr.GetBoolean(28);
                        cbShowSuppliers.Checked = dr.GetBoolean(29);
                        cbAddStore.Checked = dr.GetBoolean(30);
                        cbOpenStore.Checked = dr.GetBoolean(31);
                        cbFinancePage.Checked = dr.GetBoolean(32);
                        cbEditDebtorBalance.Checked = dr.GetBoolean(33);
                        cbWithdrawCreditor.Checked = dr.GetBoolean(34);
                        cbAddCreditCustomer.Checked = dr.GetBoolean(35);
                        cbPayCredit.Checked = dr.GetBoolean(36);
                        cbShowShortages.Checked = dr.GetBoolean(37);
                        cbShowPurchases.Checked = dr.GetBoolean(38);
                        cbAddExpenses.Checked = dr.GetBoolean(39);
                        cbSalaries.Checked = dr.GetBoolean(40);
                        cbShowCategories.Checked = dr.GetBoolean(41);
                        cbCanResetSystem.Checked = dr.GetBoolean(42);
                        cbDeferredPos.Checked = dr.GetBoolean(43);
                        cbInstallmentPos.Checked = dr.GetBoolean(44);
                        cbEditCompanyInfo.Checked = dr.GetBoolean(45);
                    }
                }

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void userPermission()
        {
            CheckAllGuna2CheckBoxes(corntrolPanel, false);

            cbEditeDevice.Checked = true;
            cbDeleteDevice.Checked = true;
            cbProCardAdd.Checked = true;
            cbProCardEdite.Checked = true;
            cbStaffShow.Checked = true;
            cbSaveBackup.Checked = true;
            cbChangePass.Checked = true;
        }

        private void btnAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (isHandlingCheckedChanged) return; // لو في تعديل تجاهل

            isHandlingCheckedChanged = true; // نبدأ منع التكرار

            CheckAllGuna2CheckBoxes(corntrolPanel, true);

            btnAdmin.Checked = true;
            btnUser.Checked = false;
            btnNoPermission.Checked = false;

            isHandlingCheckedChanged = false; // نرجع نسمح بالتعامل
        }

        private void btnUser_CheckedChanged(object sender, EventArgs e)
        {
            if (isHandlingCheckedChanged) return;

            isHandlingCheckedChanged = true;

            userPermission();

            btnAdmin.Checked = false;
            btnUser.Checked = true;
            btnNoPermission.Checked = false;

            isHandlingCheckedChanged = false;
        }

        private void btnNoPermission_CheckedChanged(object sender, EventArgs e)
        {
            if (isHandlingCheckedChanged) return;

            isHandlingCheckedChanged = true;

            CheckAllGuna2CheckBoxes(corntrolPanel, false);

            btnAdmin.Checked = false;
            btnUser.Checked = false;
            btnNoPermission.Checked = true;

            isHandlingCheckedChanged = false;
        }

        private void cbUserPermission_CheckedChanged(object sender, EventArgs e)
        {
            if (isHandlingCheckedChanged) return;

            isHandlingCheckedChanged = true;

            btnAdmin.Checked = false;
            btnUser.Checked = false;
            btnNoPermission.Checked = false;

            isHandlingCheckedChanged = false;
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
            //-> Light Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.FromArgb(2, 2, 2);
        }
        private void ThemeMode()
        {
            this.BackColor = backgroundPrmary;
            corntrolPanel.BackColor = backgroundPrmary;
            corntrolPanel.ForeColor = textColor;

            comboBoxUser.BackColor = backgroundPrmary;
            comboBoxUser.ForeColor = textColor;
            comboBoxUser.BorderColor = checkedFillColor;
            comboBoxUser.FillColor = backgroundPrmary;

            userPanel.FillColor = backgroundseconder;
            posPanel.FillColor = backgroundseconder;
            maintanancePanel.FillColor = backgroundseconder;
            storPanel.FillColor = backgroundseconder;
            installmentPanel.FillColor = backgroundseconder;
            staffPanel.FillColor = backgroundseconder;
            settingPanel.FillColor = backgroundseconder;
            reportesPanel.FillColor = backgroundseconder;
            otherPanel.FillColor = backgroundseconder;

            cbRebortMaint.CheckedState.FillColor = checkedFillColor;
            cbBillMaint.CheckedState.FillColor = checkedFillColor;
            cbEditeDevice.CheckedState.FillColor = checkedFillColor;
            cbDeleteDevice.CheckedState.FillColor = checkedFillColor;
            cbIncreasePrice.CheckedState.FillColor = checkedFillColor;
            cbProCardAdd.CheckedState.FillColor = checkedFillColor;
            cbProCardEdite.CheckedState.FillColor = checkedFillColor;
            cbProCardDetete.CheckedState.FillColor = checkedFillColor;
            cbStaffShow.CheckedState.FillColor = checkedFillColor;
            cbStaffAdd.CheckedState.FillColor = checkedFillColor;
            cbStaffEdite.CheckedState.FillColor = checkedFillColor;
            cbStaffDelete.CheckedState.FillColor = checkedFillColor;
            cbAddUser.CheckedState.FillColor = checkedFillColor;
            cbChangePass.CheckedState.FillColor = checkedFillColor;
            cbUserPermission.CheckedState.FillColor = checkedFillColor;
            cbSaveBackup.CheckedState.FillColor = checkedFillColor;
            cbReBackup.CheckedState.FillColor = checkedFillColor;
            cbBackupPath.CheckedState.FillColor = checkedFillColor;

            btnAdmin.CheckedState.FillColor = checkedFillColor;
            btnAdmin.CheckedState.BorderColor = checkedFillColor;
            btnUser.CheckedState.FillColor = checkedFillColor;
            btnUser.CheckedState.BorderColor = checkedFillColor;
            btnNoPermission.CheckedState.FillColor = checkedFillColor;
            btnNoPermission.CheckedState.BorderColor = checkedFillColor;
        }


        private void frmUserPermissions_VisibleChanged(object sender, EventArgs e)
        {
            string qry_Tec = "SELECT staff.staffID AS 'id', staff.sName AS 'name' FROM staff INNER JOIN users ON staff.staffID = users.staffID";
            MainClass.CBFill(qry_Tec, comboBoxUser);
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
