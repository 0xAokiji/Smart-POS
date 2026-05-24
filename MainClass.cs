using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Utils;
using pos.Classes;
using pos.GeneralForms.MainForm;
using pos.Model.POS;
using pos.Reports;
using pos.Settings;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace pos
{
    internal class MainClass
    {
        private static string serverName;
        private static string databaseName;
        private static string dbUserName;
        private static string dbPassword;

        
        public static string machineName = Environment.MachineName;
        /// <summary>
        /// </summary>
        //public static readonly string con_string = "server=AOKIJI; database=smartpos; user id=sa; password=aokiji; MultipleActiveResultSets=true;";// My PC

        //public static readonly string con_string = "server=DESKTOP-RE7FF65; database=smartpos; user id=sa; password=aokiji; MultipleActiveResultSets=true;"; // Ali Elsayed PC

        //public static SqlConnection con;

        private static bool nousers;
        public static int shiftid;
        private static int staffid;
        public static string user;
        public static int uid;
        public static string userphone;
        public static string password;
        public static byte[] imageBytes;
        public static string thememode;

        private static string mainPrinter;
        private static string barcodePrinter;
        private static string cashierPrinter1;
        private static string cashierPrinter2;

        public static bool NoUsers { get { return nousers; } set { nousers = value; } }

        public static int staffID { get { return staffid; } set { staffid = value; } }

        public static int shiftID { get { return shiftid; } set { shiftid = value; } }
        public static string PASSWORD { get { return password; } private set { password = value; } }
        public static string DatabaseName { get { return databaseName; } private set { databaseName = value; } }

        public static string USERPHONE { get { return userphone; } private set { userphone = value; } }
        public static string USER { get { return user; } private set { user = value; } }
        public static int UID { get { return uid; } private set { uid = value; } }
        public static byte[] IMAGEBYTES { get { return imageBytes; } private set { imageBytes = value; } }
        public static string ThemeMode { get { return thememode; } private set { thememode = value; } }



        public static string MainPrinter { get { return mainPrinter; } set { mainPrinter = value; } }
        public static string BarcodePrinter { get { return barcodePrinter; } set { barcodePrinter = value; } }
        public static string CashierPrinter1 { get { return cashierPrinter1; } set { cashierPrinter1 = value; } }
        public static string CashierPrinter2 { get { return cashierPrinter2; } set { cashierPrinter2 = value; } }

        private static Color backgroundPrimary;
        private static Color backgroundSecondary;
        private static Color textColor;
        private static Color textColor2;
        private static Color textColor3;
        private static Color checkedFillColor;
        private static Color checkedFillColor2;
        private static Color checkedForeColor;

        public static Color BackgroundPrimary { get => backgroundPrimary; set => backgroundPrimary = value; }
        public static Color BackgroundSecondary { get => backgroundSecondary; set => backgroundSecondary = value; }
        public static Color TextColor { get => textColor; set => textColor = value; }
        public static Color TextColor2 { get => textColor2; set => textColor2 = value; }
        public static Color TextColor3 { get => textColor3; set => textColor3 = value; }

        public static Color CheckedFillColor { get => checkedFillColor; set => checkedFillColor = value; }
        public static Color CheckedFillColor2 { get => checkedFillColor2; set => checkedFillColor2 = value; }

        public static Color CheckedForeColor { get => checkedForeColor; set => checkedForeColor = value; }

        // Permission properties variable
        private static bool rebortMaint, billMaint, editeDevice, deleteDevice, increasePrice;
        private static bool proCardAdd, proCardEdite, proCardDetete;
        private static bool staffShow, staffAdd, staffEdite, staffDelete;
        private static bool addUser, changePass, userPermission, saveBackup, reBackup, backupPath;
        private static bool reportFinance, partiesBalance, wholeSale, halfWholeSale, showReturns;
        private static bool addSupplierBill, showSupplierBills, showDeletedSupBills, showCustomerBills;
        private static bool showDeletedCusBills, showStoreBalance, showSuppliers, addStore, openStore;
        private static bool financePage, editDebtorBalance, withdrawCreditor, addCreditCustomer, payCredit;
        private static bool showShortages, showPurchases, addExpenses, salaries, showCategories;
        private static bool canResetSystem, installmentPos, deferredPos, editCompanyInfo;

        public static bool RebortMaint { get { return rebortMaint; } set { rebortMaint = value; } }
        public static bool BillMaint { get { return billMaint; } set { billMaint = value; } }
        public static bool EditeDevice { get { return editeDevice; } set { editeDevice = value; } }
        public static bool DeleteDevice { get { return deleteDevice; } set { deleteDevice = value; } }
        public static bool IncreasePrice { get { return increasePrice; } set { increasePrice = value; } }
        public static bool ProCardAdd { get { return proCardAdd; } set { proCardAdd = value; } }
        public static bool ProCardEdite { get { return proCardEdite; } set { proCardEdite = value; } }
        public static bool ProCardDetete { get { return proCardDetete; } set { proCardDetete = value; } }
        public static bool StaffShow { get { return staffShow; } set { staffShow = value; } }
        public static bool StaffAdd { get { return staffAdd; } set { staffAdd = value; } }
        public static bool StaffEdite { get { return staffEdite; } set { staffEdite = value; } }
        public static bool StaffDelete { get { return staffDelete; } set { staffDelete = value; } }
        public static bool AddUser { get { return addUser; } set { addUser = value; } }
        public static bool ChangePass { get { return changePass; } set { changePass = value; } }
        public static bool UserPermission { get { return userPermission; } set { userPermission = value; } }
        public static bool SaveBackup { get { return saveBackup; } set { saveBackup = value; } }
        public static bool ReBackup { get { return reBackup; } set { reBackup = value; } }
        public static bool BackupPath { get { return backupPath; } set { backupPath = value; } }
        public static bool ReportFinance { get { return reportFinance; } set { reportFinance = value; } }
        public static bool PartiesBalance { get { return partiesBalance; } set { partiesBalance = value; } }
        public static bool WholeSale { get { return wholeSale; } set { wholeSale = value; } }
        public static bool HalfWholeSale { get { return halfWholeSale; } set { halfWholeSale = value; } }
        public static bool ShowReturns { get { return showReturns; } set { showReturns = value; } }
        public static bool AddSupplierBill { get { return addSupplierBill; } set { addSupplierBill = value; } }
        public static bool ShowSupplierBills { get { return showSupplierBills; } set { showSupplierBills = value; } }
        public static bool ShowDeletedSupBills { get { return showDeletedSupBills; } set { showDeletedSupBills = value; } }
        public static bool ShowCustomerBills { get { return showCustomerBills; } set { showCustomerBills = value; } }
        public static bool ShowDeletedCusBills { get { return showDeletedCusBills; } set { showDeletedCusBills = value; } }
        public static bool ShowStoreBalance { get { return showStoreBalance; } set { showStoreBalance = value; } }
        public static bool ShowSuppliers { get { return showSuppliers; } set { showSuppliers = value; } }
        public static bool AddStore { get { return addStore; } set { addStore = value; } }
        public static bool OpenStore { get { return openStore; } set { openStore = value; } }
        public static bool FinancePage { get { return financePage; } set { financePage = value; } }
        public static bool EditDebtorBalance { get { return editDebtorBalance; } set { editDebtorBalance = value; } }
        public static bool WithdrawCreditor { get { return withdrawCreditor; } set { withdrawCreditor = value; } }
        public static bool AddCreditCustomer { get { return addCreditCustomer; } set { addCreditCustomer = value; } }
        public static bool PayCredit { get { return payCredit; } set { payCredit = value; } }
        public static bool ShowShortages { get { return showShortages; } set { showShortages = value; } }
        public static bool ShowPurchases { get { return showPurchases; } set { showPurchases = value; } }
        public static bool AddExpenses { get { return addExpenses; } set { addExpenses = value; } }
        public static bool Salaries { get { return salaries; } set { salaries = value; } }
        public static bool ShowCategories { get { return showCategories; } set { showCategories = value; } }
        public static bool CanResetSystem { get { return canResetSystem; } set { canResetSystem = value; } }
        public static bool InstallmentPos { get { return installmentPos; } set { installmentPos = value; } }
        public static bool DeferredPos { get { return deferredPos; } set { deferredPos = value; } }
        public static bool EditCompanyInfo { get { return editCompanyInfo; } set { editCompanyInfo = value; } }



        private static int companyID;
        private static string companyName;
        private static string companyAddress;
        private static string phone1;
        private static string phone2;
        private static byte[] companyPic;
        private static byte[] companyLogo;
        private static byte[] companyQRCodeInfo;

        public static int CompanyID { get { return companyID; } set { companyID = value; } }
        public static string CompanyName { get { return companyName; } set { companyName = value; } }
        public static string CompanyAddress { get { return companyAddress; } set { companyAddress = value; } }
        public static string Phone1 { get { return phone1; } set { phone1 = value; } }
        public static string Phone2 { get { return phone2; } set { phone2 = value; } }
        public static byte[] CompanyPic { get { return companyPic; } set { companyPic = value; } }
        public static byte[] CompanyLogo { get { return companyLogo; } set { companyLogo = value; } }
        public static byte[] CompanyQRCodeInfo { get { return companyQRCodeInfo; } set { companyQRCodeInfo = value; } }

        static MainClass()
        {
            try
            {
                con_string = BuildConnectionString();
            }
            catch (Exception ex)
            {
            }
        }

        private const int SaltSize = 16; // bytes
        private const int HashSize = 32; // bytes (256-bit)
        private const int Iterations = 100_000; // عدد التكرارات
        public static string HashPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);
                    // صيغة التخزين: iterations.salt.hash
                    return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
                }
            }
        }
        public static string EncryptText(string plainText)
        {
            byte[] key = KeyManager.GetOrCreateKey();
            return AesEncryption.Encrypt(plainText, key);
        }

        public static string DecryptText(string encryptedText)
        {
            byte[] key = KeyManager.GetOrCreateKey();
            string decrypted = AesEncryption.Decrypt(encryptedText, key);

            // إزالة BOM إذا موجود
            if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                decrypted = decrypted.Substring(1);

            // إزالة أي whitespace إضافية
            decrypted = decrypted.Trim();

            return decrypted;
        }

        private static string con_string ;

        private static string BuildConnectionString()
        {
            DBConfig config = DBConfig.Load();

            serverName = config.Server;
            databaseName = config.Database;
            dbUserName = config.User;
            string decrypted = DecryptText(config.Password);

            // إزالة BOM إذا موجود
            if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                decrypted = decrypted.Substring(1);

            // إزالة أي whitespace إضافية
            dbPassword = decrypted.Trim();

            bool sqlAuthentication = config.sqlAuthentication;
            if (sqlAuthentication)
                return $"server={serverName}; database={databaseName}; user id={dbUserName}; password={dbPassword}; MultipleActiveResultSets=true;";
            else
                return $"server={serverName}; database={databaseName}; Integrated Security=True; MultipleActiveResultSets=true;";

        }
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(con_string);
        }
        public static string GetMasterConnectionString()
        {
            DBConfig config = DBConfig.Load();
            string serverName = config.Server;
            string dbUserName = config.User;
            string decrypted = DecryptText(config.Password);
            // إزالة BOM إذا موجود
            if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                decrypted = decrypted.Substring(1);

            // إزالة أي whitespace إضافية
            string dbPassword = decrypted.Trim();
            bool sqlAuthentication = config.sqlAuthentication;

            if (sqlAuthentication)
                return $"Server={serverName};Database=master;User Id={dbUserName};Password={dbPassword};";
            else
                return $"Server={serverName};Database=master;Integrated Security=True;";
        }


        public static bool VerifyPassword(string password, string stored)
        {
            try
            {
                var parts = stored.Split('.');
                if (parts.Length != 3) return false;

                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] storedHash = Convert.FromBase64String(parts[2]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] computed = pbkdf2.GetBytes(storedHash.Length);
                    return CryptographicOperations.FixedTimeEquals(computed, storedHash);
                }
            }
            catch
            {
                return false;
            }
        }
        public  static bool IsvalidUser(string user, string pass)
        {
            password = pass;
            bool isValid = false;

            using (SqlConnection con = MainClass.GetConnection())
            {
                // ✅ نجيب بيانات اليوزر فقط بالاسم
                string qry = @"SELECT * FROM users WHERE uername = @user";
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@user", user);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            // ✅ التحقق من الباسورد بالهاش المخزن
                            string storedHash = dt.Rows[0]["uPass"].ToString();
                            string storedSignature = dt.Rows[0]["Signature"].ToString();

                            if (VerifyPassword(pass, storedHash))
                            {
                                // 📌 تحقق من سلامة البيانات باستخدام التوقيع
                                int userId = Convert.ToInt32(dt.Rows[0]["userID"]);   // 👈 هات userID من الجدول
                                int staffId = Convert.ToInt32(dt.Rows[0]["staffID"]);
                                string username = dt.Rows[0]["uername"].ToString();
                                string passHash = storedHash;

                                nousers = false;

                                string expectedSig = UserSigner.ComputeSignature(
                                    userId, username, passHash, staffId
                                );

                                if (expectedSig != storedSignature)
                                {
                                    // 🚨 في تلاعب بالبيانات
                                    MessageBox.Show("❌ تم اكتشاف تلاعب في بيانات المستخدم\nسيتم إغلاق البرنامج",
                                                    "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Environment.Exit(0); // قفل البرنامج
                                    return false;
                                }

                                isValid = true;

                                if (dt.Rows[0]["userImage"] != DBNull.Value)
                                    imageBytes = (byte[])dt.Rows[0]["userImage"];

                                staffID = staffId;

                                // 🔹 جلب بيانات الموظف وآخر شيفت
                                string qry2 = @"SELECT * FROM staff WHERE staffID = @staffID;
                                            SELECT TOP 1 ID, staffID, endTime 
                                            FROM shifts 
                                            ORDER BY ID DESC;";

                                using (SqlCommand cmd2 = new SqlCommand(qry2, con))
                                {
                                    cmd2.Parameters.AddWithValue("@staffID", staffID);
                                    using (SqlDataAdapter da2 = new SqlDataAdapter(cmd2))
                                    {
                                        DataSet ds2 = new DataSet();
                                        da2.Fill(ds2);

                                        // ✅ لو الجدول staff فاضي نضيف صف جديد
                                        if (ds2.Tables[0].Rows.Count == 0)
                                        {
                                            string insertStaffQry = @"INSERT INTO staff (staffID, sName, sPhone, sRole, sStatus)
                                                VALUES (@staffID, @name, @phone, @role, @status);";

                                            using (SqlCommand insertCmd = new SqlCommand(insertStaffQry, con))
                                            {
                                                insertCmd.Parameters.AddWithValue("@staffID", staffID);
                                                insertCmd.Parameters.AddWithValue("@name", "User " + staffID);
                                                insertCmd.Parameters.AddWithValue("@phone", "0000000000");
                                                insertCmd.Parameters.AddWithValue("@role", "موظف");
                                                insertCmd.Parameters.AddWithValue("@status", "نشط");

                                                if (con.State != ConnectionState.Open)
                                                    con.Open();

                                                insertCmd.ExecuteNonQuery();
                                            }

                                            // ✅ بعد الإدخال نعيد تحميل البيانات من جديد
                                            using (SqlCommand reloadCmd = new SqlCommand(qry2, con))
                                            {
                                                reloadCmd.Parameters.AddWithValue("@staffID", staffID);
                                                using (SqlDataAdapter reloadDa = new SqlDataAdapter(reloadCmd))
                                                {
                                                    ds2 = new DataSet();
                                                    reloadDa.Fill(ds2);
                                                }
                                            }
                                        }

                                        if (ds2.Tables[0].Rows.Count > 0)
                                        {
                                            USER = ds2.Tables[0].Rows[0]["sName"].ToString();
                                            userphone = ds2.Tables[0].Rows[0]["sPhone"].ToString();
                                            UID = staffID;

                                            LoadUserPermissions(staffID);
                                            LoadCompanyProfileAsync();
                                            themeMode();

                                            bool isEndTimeNull = true;
                                            bool sameID = true;

                                            // ✅ التحقق إذا كان جدول الشيفت فاضي
                                            if (ds2.Tables.Count < 2 || ds2.Tables[1].Rows.Count == 0)
                                            {
                                                // 📌 مفيش شيفتات → نضيف شيفت جديد
                                                string insertShiftQry = @"
                                                INSERT INTO shifts (Amount, staffID, aDate, startTime, endTime)
                                                VALUES (@zero, @staffID, @aDate, @startTime, @endTime);
                                                SELECT SCOPE_IDENTITY();";

                                                using (SqlCommand cmd3 = new SqlCommand(insertShiftQry, con))
                                                {
                                                    cmd3.Parameters.AddWithValue("@zero", 0);
                                                    cmd3.Parameters.AddWithValue("@staffID", staffID);
                                                    cmd3.Parameters.AddWithValue("@aDate", DateTime.Now.Date);
                                                    cmd3.Parameters.AddWithValue("@startTime", DateTime.Now.ToShortTimeString());
                                                    cmd3.Parameters.AddWithValue("@endTime", DBNull.Value);

                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();

                                                    object result = cmd3.ExecuteScalar();
                                                    shiftID = Convert.ToInt32(result);
                                                }
                                            }
                                            else
                                            {
                                                // ✅ جدول الشيفت موجود وفيه صفوف
                                                object endTimeValue = ds2.Tables[1].Rows[0]["endTime"];
                                                isEndTimeNull = (endTimeValue == DBNull.Value || string.IsNullOrWhiteSpace(endTimeValue.ToString()));

                                                int shiftStaffID = Convert.ToInt32(ds2.Tables[1].Rows[0]["staffID"]);
                                                sameID = staffID == shiftStaffID;

                                                if (!isEndTimeNull || !sameID)
                                                {
                                                    if (!sameID)
                                                    {
                                                        string qryUpdate = @"UPDATE shifts 
                                                         SET endTime = @endTime, endDate = @endDate 
                                                         WHERE ID = (SELECT MAX(ID) FROM shifts)";

                                                        Hashtable ht = new Hashtable
                                                        {
                                                            { "@endTime", DateTime.Now.ToShortTimeString() },
                                                            { "@endDate", DateTime.Now.Date }
                                                        };

                                                        SQL(qryUpdate, ht);
                                                    }

                                                    string qry3 = @"INSERT INTO shifts (Amount, staffID, aDate, startTime, endTime)
                                                    VALUES (@zero, @staffID, @aDate, @startTime, @endTime);
                                                    SELECT SCOPE_IDENTITY();";

                                                    using (SqlCommand cmd3 = new SqlCommand(qry3, con))
                                                    {
                                                        cmd3.Parameters.AddWithValue("@zero", 0);
                                                        cmd3.Parameters.AddWithValue("@staffID", staffID);
                                                        cmd3.Parameters.AddWithValue("@aDate", DateTime.Now.Date);
                                                        cmd3.Parameters.AddWithValue("@startTime", DateTime.Now.ToShortTimeString());
                                                        cmd3.Parameters.AddWithValue("@endTime", DBNull.Value);

                                                        if (con.State != ConnectionState.Open)
                                                            con.Open();

                                                        object result = cmd3.ExecuteScalar();
                                                        shiftID = Convert.ToInt32(result);
                                                    }
                                                }
                                                else
                                                {
                                                    shiftID = Convert.ToInt32(ds2.Tables[1].Rows[0]["ID"]);
                                                }
                                            }

                                            setPrinterName();
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            nousers = true;                           
                        }
                    }
                }
            }

            return isValid;
        }




        //Methord for curd operation
        public static int SQL(string qry, Hashtable ht)
        {
            int res = 0;
            try
            {
                using (SqlConnection con = MainClass.GetConnection()) // الاتصال من الدالة
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    foreach (DictionaryEntry item in ht)
                    {
                        cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                    }

                    con.Open(); // هنا يكفي
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return res;

        }

        // for loading data from database
        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {

            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            try
            {
                using (SqlConnection con = MainClass.GetConnection()) // ✅ الاتصال من الدالة
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    for (int i = 0; i < lb.Items.Count; i++)
                    {
                        string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                        if (gv.Columns[colNam1] != null)
                        {
                            gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();
                        }
                    }

                    gv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public static void LoadData(string qry, DataGridView gv, ListBox lb, SqlParameter[] parameters = null)
        {
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            try
            {
                using (SqlConnection con = MainClass.GetConnection()) // ✅ جايب الاتصال من الدالة
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    for (int i = 0; i < lb.Items.Count; i++)
                    {
                        string colName = ((DataGridViewColumn)lb.Items[i]).Name;
                        gv.Columns[colName].DataPropertyName = dt.Columns[i].ColumnName;
                    }

                    gv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        public static void LoadData(SqlCommand cmd, DataGridView gv, ListBox lb)
        {
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);

            try
            {
                using (SqlConnection con = GetConnection()) // ✅ الاتصال من الدالة
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    for (int i = 0; i < lb.Items.Count; i++)
                    {
                        string colName = ((DataGridViewColumn)lb.Items[i]).Name;
                        gv.Columns[colName].DataPropertyName = dt.Columns[i].ColumnName; // ✅ اسم العمود الصحيح
                    }

                    gv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public static void LoadData(string qry, DataGridView gv, List<string> columnNames)
        {
            if (columnNames == null || columnNames.Count == 0)
            {
                throw new ArgumentNullException(nameof(columnNames), "Column names cannot be null or empty.");
            }

            // علشان ما نكررش إضافة نفس الـ EventHandler أكتر من مرة
            gv.CellFormatting -= new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);

            try
            {
                using (SqlConnection con = GetConnection()) // ✅ فتح الاتصال بالطريقة المعيارية
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gv.AutoGenerateColumns = false;

                    // Clear existing columns
                    gv.Columns.Clear();

                    // Add columns based on columnNames
                    foreach (string colName in columnNames)
                    {
                        DataGridViewTextBoxColumn newCol = new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = colName,
                            HeaderText = colName,
                            Name = colName
                        };
                        gv.Columns.Add(newCol);
                    }

                    gv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message);
            }
        }

        public static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        public static void BlureBackground(Form Model)
        {
            Form Background = new Form();
            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmMain.Instance.Size;
                Background.Location = frmMain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Model.Dispose();
            }
        }

        // اضافه الصن الي combox
        public static void CBFill(string qry, ComboBox cb, string displayMember = "name", string valueMember = "id")
        {
            try
            {
                using (SqlConnection con = GetConnection()) // ✅ الاتصال من الدالة
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cb.DisplayMember = displayMember;
                    cb.ValueMember = valueMember;
                    cb.DataSource = dt;
                    cb.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات في ComboBox: " + ex.Message);
            }
        }


        public static void UpdateAppSetting(string key, string value)
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.config");

            if (!File.Exists(configFilePath))
            {
                var defaultConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                <configuration>
                  <appSettings>
                  </appSettings>
                </configuration>";
                File.WriteAllText(configFilePath, defaultConfig);
            }

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            if (config.AppSettings.Settings[key] != null)
                config.AppSettings.Settings[key].Value = value;
            else
                config.AppSettings.Settings.Add(key, value);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        // ✅ تحميل الصلاحيات والتحقق من سلامتها
        public static void LoadUserPermissions(int staffID)
        {
            string query = @"
            SELECT RebortMaint, BillMaint, EditeDevice, DeleteDevice, IncreasePrice,
                   ProCardAdd, ProCardEdite, ProCardDetete,
                   StaffShow, StaffAdd, StaffEdite, StaffDelete,
                   AddUser, ChangePass, UserPermission, SaveBackup, ReBackup, BackupPath,
                   ReportFinance, PartiesBalance, WholeSale, HalfWholeSale, ShowReturns,
                   AddSupplierBill, ShowSupplierBills, ShowDeletedSupBills, ShowCustomerBills,
                   ShowDeletedCusBills, ShowStoreBalance, ShowSuppliers, AddStore, OpenStore,
                   FinancePage, EditDebtorBalance, WithdrawCreditor, AddCreditCustomer, PayCredit,
                   ShowShortages, ShowPurchases, AddExpenses, Salaries, ShowCategories, CanResetSystem,
                   Signature, InstallmentPos, DeferredPos,deitCompanyInfo
            FROM userPermissions
            WHERE staffID = @staffID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@staffID", staffID);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // نخزن القيم في Array
                        bool[] flags =
                        {
                        Convert.ToBoolean(dr["RebortMaint"]),
                        Convert.ToBoolean(dr["BillMaint"]),
                        Convert.ToBoolean(dr["EditeDevice"]),
                        Convert.ToBoolean(dr["DeleteDevice"]),
                        Convert.ToBoolean(dr["IncreasePrice"]),
                        Convert.ToBoolean(dr["ProCardAdd"]),
                        Convert.ToBoolean(dr["ProCardEdite"]),
                        Convert.ToBoolean(dr["ProCardDetete"]),
                        Convert.ToBoolean(dr["StaffShow"]),
                        Convert.ToBoolean(dr["StaffAdd"]),
                        Convert.ToBoolean(dr["StaffEdite"]),
                        Convert.ToBoolean(dr["StaffDelete"]),
                        Convert.ToBoolean(dr["AddUser"]),
                        Convert.ToBoolean(dr["ChangePass"]),
                        Convert.ToBoolean(dr["UserPermission"]),
                        Convert.ToBoolean(dr["SaveBackup"]),
                        Convert.ToBoolean(dr["ReBackup"]),
                        Convert.ToBoolean(dr["BackupPath"]),
                        Convert.ToBoolean(dr["ReportFinance"]),
                        Convert.ToBoolean(dr["PartiesBalance"]),
                        Convert.ToBoolean(dr["WholeSale"]),
                        Convert.ToBoolean(dr["HalfWholeSale"]),
                        Convert.ToBoolean(dr["ShowReturns"]),
                        Convert.ToBoolean(dr["AddSupplierBill"]),
                        Convert.ToBoolean(dr["ShowSupplierBills"]),
                        Convert.ToBoolean(dr["ShowDeletedSupBills"]),
                        Convert.ToBoolean(dr["ShowCustomerBills"]),
                        Convert.ToBoolean(dr["ShowDeletedCusBills"]),
                        Convert.ToBoolean(dr["ShowStoreBalance"]),
                        Convert.ToBoolean(dr["ShowSuppliers"]),
                        Convert.ToBoolean(dr["AddStore"]),
                        Convert.ToBoolean(dr["OpenStore"]),
                        Convert.ToBoolean(dr["FinancePage"]),
                        Convert.ToBoolean(dr["EditDebtorBalance"]),
                        Convert.ToBoolean(dr["WithdrawCreditor"]),
                        Convert.ToBoolean(dr["AddCreditCustomer"]),
                        Convert.ToBoolean(dr["PayCredit"]),
                        Convert.ToBoolean(dr["ShowShortages"]),
                        Convert.ToBoolean(dr["ShowPurchases"]),
                        Convert.ToBoolean(dr["AddExpenses"]),
                        Convert.ToBoolean(dr["Salaries"]),
                        Convert.ToBoolean(dr["ShowCategories"]),
                        Convert.ToBoolean(dr["CanResetSystem"]),
                        Convert.ToBoolean(dr["InstallmentPos"]),
                        Convert.ToBoolean(dr["DeferredPos"]),
                        Convert.ToBoolean(dr["deitCompanyInfo"])


                    };

                        string storedSignature = dr["Signature"].ToString();

                        // احسب التوقيع
                        string computedSignature = PermissionSigner.ComputeSignature(staffID, flags);

                        if (storedSignature != computedSignature)
                        {
                            // لو فيه تلاعب → كل الصلاحيات False
                            RebortMaint = BillMaint = EditeDevice = DeleteDevice = IncreasePrice =
                            ProCardAdd = ProCardEdite = ProCardDetete =
                            StaffShow = StaffAdd = StaffEdite = StaffDelete =
                            AddUser = ChangePass = UserPermission =
                            SaveBackup = ReBackup = BackupPath =
                            ReportFinance = PartiesBalance = WholeSale = HalfWholeSale = ShowReturns =
                            AddSupplierBill = ShowSupplierBills = ShowDeletedSupBills = ShowCustomerBills =
                            ShowDeletedCusBills = ShowStoreBalance = ShowSuppliers = AddStore = OpenStore =
                            FinancePage = EditDebtorBalance = WithdrawCreditor = AddCreditCustomer = PayCredit =
                            ShowShortages = ShowPurchases = AddExpenses = Salaries = ShowCategories = InstallmentPos = 
                            CanResetSystem = DeferredPos = EditCompanyInfo = false;

                            MessageBox.Show(
                                "⚠️ تم اكتشاف تلاعب في صلاحيات المستخدم داخل قاعدة البيانات!\n\n" +
                                "تم تعطيل جميع الصلاحيات لحماية النظام.",
                                "تحذير أمني",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }
                        else
                        {
                            // ✅ التوقيع سليم → نخزن القيم
                            RebortMaint = flags[0];
                            BillMaint = flags[1];
                            EditeDevice = flags[2];
                            DeleteDevice = flags[3];
                            IncreasePrice = flags[4];
                            ProCardAdd = flags[5];
                            ProCardEdite = flags[6];
                            ProCardDetete = flags[7];
                            StaffShow = flags[8];
                            StaffAdd = flags[9];
                            StaffEdite = flags[10];
                            StaffDelete = flags[11];
                            AddUser = flags[12];
                            ChangePass = flags[13];
                            UserPermission = flags[14];
                            SaveBackup = flags[15];
                            ReBackup = flags[16];
                            BackupPath = flags[17];
                            ReportFinance = flags[18];
                            PartiesBalance = flags[19];
                            WholeSale = flags[20];
                            HalfWholeSale = flags[21];
                            ShowReturns = flags[22];
                            AddSupplierBill = flags[23];
                            ShowSupplierBills = flags[24];
                            ShowDeletedSupBills = flags[25];
                            ShowCustomerBills = flags[26];
                            ShowDeletedCusBills = flags[27];
                            ShowStoreBalance = flags[28];
                            ShowSuppliers = flags[29];
                            AddStore = flags[30];
                            OpenStore = flags[31];
                            FinancePage = flags[32];
                            EditDebtorBalance = flags[33];
                            WithdrawCreditor = flags[34];
                            AddCreditCustomer = flags[35];
                            PayCredit = flags[36];
                            ShowShortages = flags[37];
                            ShowPurchases = flags[38];
                            AddExpenses = flags[39];
                            Salaries = flags[40];
                            ShowCategories = flags[41];
                            CanResetSystem = flags[42];
                            InstallmentPos = flags[43];
                            DeferredPos = flags[44];
                            EditCompanyInfo = flags[45];
                        }
                    }
                }
            }
        }


        public static async Task<int> SQLAsync(string query, Hashtable parameters)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدام GetConnection
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // إضافة المعلمات لو موجودة
                        if (parameters != null)
                        {
                            foreach (DictionaryEntry param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key.ToString(), param.Value ?? DBNull.Value);
                            }
                        }

                        // تنفيذ الاستعلام بشكل غير متزامن
                        rowsAffected = await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message);
            }

            return rowsAffected; // ✅ رجع عدد الصفوف المتأثرة
        }

        public static void themeMode()
        {
            string configFilePath = Path.Combine(Application.StartupPath, @"..\..\..\Settings\Settings.config");

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            if (settings["ThemeMode"] != null && !string.IsNullOrWhiteSpace(settings["ThemeMode"].Value) && settings["ThemeMode"].Value.ToLower() != "null")
                thememode = settings["ThemeMode"].Value;
            else
                thememode = "light";

            if (MainClass.thememode == "dark")
                DarkMode();
            else if (MainClass.thememode == "light")
                LightMode();
        }
        private static void LightMode()
        {
            //-> Light Mode
            backgroundPrimary = Color.FromArgb(243, 243, 243);
            backgroundSecondary = Color.FromArgb(230, 230, 230);
            textColor = Color.FromArgb(51, 51, 51);
            textColor2 = Color.FromArgb(89, 89, 89);
            textColor3 = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedFillColor2 = Color.FromArgb(136, 214, 218);
            checkedForeColor = Color.FromArgb(250, 250, 250);
        }
        private static void DarkMode()
        {
            //-> Dark Mode
            backgroundPrimary = Color.FromArgb(32, 32, 32);
            backgroundSecondary = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            textColor2 = Color.LightSkyBlue;
            textColor3 = Color.FromArgb(51, 51, 51);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedFillColor2 = Color.FromArgb(136, 214, 218);
            checkedForeColor = Color.FromArgb(2, 2, 2);

        }

        public static T GetSingleValue<T>(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection()) // ✅ اتصال جديد
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return default; // يرجع القيمة الافتراضية لنوع T (مثلاً 0 للـ int أو null للـ string)

                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return default;
            }
        }

        public static void SafeInvoke(Action action)
        {
            if (Application.OpenForms.Count > 0)
            {
                var form = Application.OpenForms[0];
                if (form.InvokeRequired)
                    form.Invoke(action);
                else
                    action();
            }
            else
            {
                action();
            }
        }
        public static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        public static async Task PrintInvoiceAsync(int mainID, bool print = false, string billStatments = "فاتورة مبيعات", double total = 0)
        {
            // 🟢 1. جلب البيانات فقط في الخلفية
            DataSet ds = await Task.Run(() =>
            {
                return pos.Classes.setDataInDataSet.GetBillData(mainID, billStatments, total);
            });

            // 🟢 2. إنشاء التقرير على الـ UI Thread
            XtraReportCutomerA5 report = new XtraReportCutomerA5
            {
                DataSource = ds,
                DataMember = "Bill_Info"
            };

            if (report.Bands["DetailReport"] is DetailReportBand detailReportBand)
            {
                detailReportBand.DataSource = ds;
                detailReportBand.DataMember = "Bill_Details";
            }

            // 🟢 3. تحميل شعار الشركة على الخلفية
            await MainClass.LoadCompanyProfileAsync();

            if (MainClass.CompanyLogo != null)
            {
                using MemoryStream msLogo = new MemoryStream(MainClass.CompanyLogo);
                report.CompanyLogo = Image.FromStream(msLogo);
            }
            if (MainClass.companyQRCodeInfo != null)
            {
                using MemoryStream msQRCode = new MemoryStream(MainClass.companyQRCodeInfo);
                report.CompanyQRCode = Image.FromStream(msQRCode);
            }

            // 🟢 4. تعبئة البيانات العامة
            report.CompanyName = MainClass.CompanyName;
            report.CompanyAddress = MainClass.CompanyAddress;
            report.Phone1 = MainClass.Phone1;
            report.Phone2 = MainClass.Phone2;

            // 🟢 5. الطباعة أو المعاينة
            if (print)
            {
                PrinterSettings printerSettings = new PrinterSettings
                {
                    PrinterName = mainPrinter
                };

                if (printerSettings.IsValid)
                    report.Print(printerSettings.PrinterName);
                else
                    report.Print();
            }
            else
            {
                ReportPrintTool printTool = new ReportPrintTool(report);

                printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                    DevExpress.XtraPrinting.PrintingSystemCommand.ExportPdf,
                    DevExpress.XtraPrinting.CommandVisibility.All
                );
                printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                    DevExpress.XtraPrinting.PrintingSystemCommand.ExportXlsx,
                    DevExpress.XtraPrinting.CommandVisibility.All
                );
                printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                    DevExpress.XtraPrinting.PrintingSystemCommand.ExportDocx,
                    DevExpress.XtraPrinting.CommandVisibility.All
                );

                printTool.ShowPreviewDialog();
            }
        }




        public static async Task PrintPartiesReportAsync(DateTime? startDate, DateTime? endDate, int partyID, string partyName, bool isSupplier, bool showAll)
        {
            // ✅ 1. تنفيذ جلب البيانات وإعداد التقرير في الخلفية
            var report = await Task.Run(async () =>
            {
                DataSet ds = pos.Classes.Data_Set.partiesRport.GetBillData(startDate, endDate, partyID, partyName, isSupplier, showAll);

                XtraPartiesReport rpt = new XtraPartiesReport
                {
                    DataSource = ds,
                    DataMember = "parteisRepert"
                };

                // ✅ تعيين صورة اللوجو من MainClass
                await MainClass.LoadCompanyProfileAsync();
                if (MainClass.CompanyLogo != null)
                {
                    using (MemoryStream msLogo = new MemoryStream(MainClass.CompanyLogo))
                    {
                        rpt.CompanyLogo = Image.FromStream(msLogo);
                    }
                }
                if (MainClass.companyQRCodeInfo != null)
                {
                    using (MemoryStream msQRCode = new MemoryStream(MainClass.companyQRCodeInfo))
                    {
                        rpt.CompanyQRCode = Image.FromStream(msQRCode);
                    }
                }
                return rpt;
            });

            // ✅ 2. عرض التقرير على واجهة المستخدم
            await Task.Yield(); // للتأكد من العودة إلى UI thread

            ReportPrintTool printTool = new ReportPrintTool(report);

            report.CompanyName = MainClass.CompanyName;
            report.CompanyAddress = MainClass.CompanyAddress;
            report.Phone1 = MainClass.Phone1;
            report.Phone2 = MainClass.Phone2;
            // 🔹 إظهار خيارات التصدير
            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportPdf,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportXlsx,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportDocx,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            // ✅ 3. عرض نافذة المعاينة
            printTool.ShowPreviewDialog();
        }
        public static async Task PrintPartiesReportAsync2(DateTime? startDate, DateTime? endDate, int partyID, string partyName, bool isSupplier, bool showAll)
        {
            // ✅ 1. تنفيذ جلب البيانات وإعداد التقرير في الخلفية
            var report = await Task.Run(async () =>
            {
                DataSet ds = pos.Classes.Data_Set.partiesReport2.GetBillData(startDate, endDate, partyID, partyName, isSupplier, showAll);

                XtraPartiesReport rpt = new XtraPartiesReport
                {
                    DataSource = ds,
                    DataMember = "parteisRepert"
                };

                // ✅ تعيين صورة اللوجو من MainClass
                await MainClass.LoadCompanyProfileAsync();
                if (MainClass.CompanyLogo != null)
                {
                    using (MemoryStream msLogo = new MemoryStream(MainClass.CompanyLogo))
                    {
                        rpt.CompanyLogo = Image.FromStream(msLogo);
                    }
                }
                if (MainClass.companyQRCodeInfo != null)
                {
                    using (MemoryStream msQRCode = new MemoryStream(MainClass.companyQRCodeInfo))
                    {
                        rpt.CompanyQRCode = Image.FromStream(msQRCode);
                    }
                }
                return rpt;
            });

            // ✅ 2. عرض التقرير على واجهة المستخدم
            await Task.Yield(); // للتأكد من العودة إلى UI thread

            ReportPrintTool printTool = new ReportPrintTool(report);

            report.CompanyName = MainClass.CompanyName;
            report.CompanyAddress = MainClass.CompanyAddress;
            report.Phone1 = MainClass.Phone1;
            report.Phone2 = MainClass.Phone2;
            // 🔹 إظهار خيارات التصدير
            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportPdf,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportXlsx,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportDocx,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            // ✅ 3. عرض نافذة المعاينة
            printTool.ShowPreviewDialog();
        }


        public static async Task PrintOrderCardAsync(
            int mainID = 0,
            int pariesID = 0,
            bool breakable = false,
            bool show = false)
        {
            // 1️⃣ جلب البيانات في الخلفية
            var report = await Task.Run(async () =>
            {
                DataSet ds = pos.Classes.setDataInDataSet.GetBillData(
                    mainID, "", 0, 0, 0, 0, "", "", pariesID, breakable);

                XtraOrderCard rpt = new XtraOrderCard
                {
                    DataSource = ds,
                    DataMember = "Bill_Info"
                };

                if (rpt.Bands["DetailReport"] is DetailReportBand detailReportBand)
                {
                    detailReportBand.DataSource = ds;
                    detailReportBand.DataMember = "Bill_Details";
                }
                // ✅ تعيين صورة اللوجو من MainClass
                await MainClass.LoadCompanyProfileAsync();
                if (MainClass.CompanyLogo != null)
                {
                    using (MemoryStream msLogo = new MemoryStream(MainClass.CompanyLogo))
                    {
                        rpt.CompanyLogo = Image.FromStream(msLogo);
                    }
                }
                if (MainClass.companyQRCodeInfo != null)
                {
                    using (MemoryStream msQRCode = new MemoryStream(MainClass.companyQRCodeInfo))
                    {
                        rpt.CompanyQRCode = Image.FromStream(msQRCode);
                    }
                }
                return rpt;
            });

            // 2️⃣ الطباعة أو العرض على UI Thread
            await Task.Yield(); // يضمن الرجوع إلى الـ UI context

            report.CompanyName = MainClass.CompanyName;
            report.CompanyAddress = MainClass.CompanyAddress;
            report.Phone1 = MainClass.Phone1;
            report.Phone2 = MainClass.Phone2;

            if (!show)
            {


                PrinterSettings printerSettings = new PrinterSettings
                {
                    PrinterName = mainPrinter
                };

                if (printerSettings.IsValid)
                {
                    report.Print(printerSettings.PrinterName);
                }
                else
                {
                    if (Application.OpenForms.Count > 0)
                    {
                        Application.OpenForms[0].BeginInvoke(new Action(() =>
                        {
                            Notifier.ShowNotification(
                                "تنبيه",
                                $"الطابعة '{mainPrinter}' غير موجودة. سيتم استخدام الطابعة الافتراضية.");
                        }));
                    }

                    report.Print(); // fallback
                }
            }
            else
            {
                // 🟢 فتح نافذة Print Preview على UI Thread
                ReportPrintTool printTool = new ReportPrintTool(report);

                printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                    DevExpress.XtraPrinting.PrintingSystemCommand.ExportPdf,
                    DevExpress.XtraPrinting.CommandVisibility.All
                );
                printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                    DevExpress.XtraPrinting.PrintingSystemCommand.ExportXlsx,
                    DevExpress.XtraPrinting.CommandVisibility.All
                );
                printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                    DevExpress.XtraPrinting.PrintingSystemCommand.ExportDocx,
                    DevExpress.XtraPrinting.CommandVisibility.All
                );

                printTool.ShowPreviewDialog();
            }
        }



        public static async Task BillStatmentPrintAsync(
     int mainID,
     double change = 0,
     double current = 0,
     double previous = 0,
     int paritesID = 0,
     string billStatments = "فاتورة مبيعات",
     string parties_From = "",
     string parties_To = "",
     int total = 0,
     string date = "",
     string time = "")
        {
            // 🟢 1️⃣ جلب البيانات وبناء التقرير في الخلفية
            var report = await Task.Run(async () =>
            {
                DataSet ds = pos.Classes.setDataInDataSet.GetBillData(
                    mainID, billStatments, 0, change, current, previous,
                    parties_From, parties_To, paritesID, false, date, time);

                XtraPaymentReceipt rpt = new XtraPaymentReceipt
                {
                    DataSource = ds,
                    DataMember = "Bill_Info"
                };

                if (rpt.Bands["DetailReport"] is DetailReportBand detailReportBand)
                {
                    detailReportBand.DataSource = ds;
                    detailReportBand.DataMember = "Bill_Details";
                }

                // ✅ تعيين صورة اللوجو من MainClass
                await MainClass.LoadCompanyProfileAsync();
                // ✅ تحميل شعار الشركة
                if (MainClass.CompanyLogo != null && MainClass.CompanyLogo.Length > 0)
                {
                    try
                    {
                        using (MemoryStream msLogo = new MemoryStream(MainClass.CompanyLogo))
                            rpt.CompanyLogo = Image.FromStream(msLogo);
                    }
                    catch
                    {
                        rpt.CompanyLogo = null;
                    }
                }
                else
                {
                    rpt.CompanyLogo = null;
                }

                // ✅ تحميل كود QR للشركة
                if (MainClass.companyQRCodeInfo != null && MainClass.companyQRCodeInfo.Length > 0)
                {
                    try
                    {
                        using (MemoryStream msQRCode = new MemoryStream(MainClass.companyQRCodeInfo))
                            rpt.CompanyQRCode = Image.FromStream(msQRCode);
                    }
                    catch
                    {
                        rpt.CompanyQRCode = null;
                    }
                }
                else
                {
                    rpt.CompanyQRCode = null;
                }

                return rpt;
            });

            // 🟢 2️⃣ بعد الانتهاء، ارجع إلى UI Thread
            await Task.Yield();

            // 3️⃣ إعداد أداة المعاينة على UI Thread
            ReportPrintTool printTool = new ReportPrintTool(report);

            report.CompanyName = MainClass.CompanyName;
            report.CompanyAddress = MainClass.CompanyAddress;
            report.Phone1 = MainClass.Phone1;
            report.Phone2 = MainClass.Phone2;

            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportPdf,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportXlsx,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            printTool.PreviewForm.PrintControl.PrintingSystem.SetCommandVisibility(
                DevExpress.XtraPrinting.PrintingSystemCommand.ExportDocx,
                DevExpress.XtraPrinting.CommandVisibility.All
            );

            // 🟢 4️⃣ عرض نافذة Print Preview على الـ UI Thread
            printTool.ShowPreviewDialog();
        }



        public static void GetPrinterNames(
            out string mainPrinter,
            out string barcodePrinter,
            out string cashierPrinter1,
            out string cashierPrinter2)
        {
            // قيم افتراضية
            mainPrinter = "";
            barcodePrinter = "";
            cashierPrinter1 = "";
            cashierPrinter2 = "";

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string qry = "SELECT TOP 1 mainPrinter, barcodePrinter, cashierPrinter1, cashierPrinter2 FROM printer";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        mainPrinter = dr["mainPrinter"] != DBNull.Value ? dr["mainPrinter"].ToString() : "";
                        barcodePrinter = dr["barcodePrinter"] != DBNull.Value ? dr["barcodePrinter"].ToString() : "";
                        cashierPrinter1 = dr["cashierPrinter1"] != DBNull.Value ? dr["cashierPrinter1"].ToString() : "";
                        cashierPrinter2 = dr["cashierPrinter2"] != DBNull.Value ? dr["cashierPrinter2"].ToString() : "";
                    }
                }
            }
        }
        public static void setPrinterName()
        {
            // ✅ تحميل الطابعات
            GetPrinterNames(out string mainP, out string barcodeP, out string cashier1, out string cashier2);
            mainPrinter = mainP;
            barcodePrinter = barcodeP;
            cashierPrinter1 = cashier1;
            cashierPrinter2 = cashier2;

        }
        public static async Task BackUpWithoutSpinnerAsync()
        {
            string backupFolderPath = "";

            using (SqlConnection con = MainClass.GetConnection())
            {
                await con.OpenAsync();

                // 🔹 جلب مسار النسخ الاحتياطي
                string qry = "SELECT TOP 1 backupPath FROM settings";
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                        backupFolderPath = result.ToString();
                }

                if (string.IsNullOrWhiteSpace(backupFolderPath))
                {
                    Notifier.ShowNotification("تحذير", "أضف مسار النسخ الاحتياطي");
                    return;
                }

                if (!Directory.Exists(backupFolderPath))
                    Directory.CreateDirectory(backupFolderPath);


                string backupFileName = $"DiffBackup.bak";
                string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

                try
                {
                    // 🟢 تنفيذ النسخ الاحتياطي التفاضلي
                    string backupQuery = @"
                BACKUP DATABASE [smartpos] 
                TO DISK = @path 
                WITH DIFFERENTIAL, INIT, NOFORMAT, 
                     STATS = 10;
            ";

                    await Task.Run(() =>
                    {
                        using (SqlCommand cmd2 = new SqlCommand(backupQuery, con))
                        {
                            cmd2.Parameters.AddWithValue("@path", backupFilePath);
                            cmd2.CommandTimeout = 0;
                            cmd2.ExecuteNonQuery();
                        }
                    });

                    Notifier.ShowNotification("نجاح", "تم إنشاء النسخة الاحتياطية التفاضلية بنجاح ✅");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ أثناء النسخ الاحتياطي:\n" + ex.Message);
                }
            }
        }

        public static async Task LoadCompanyProfileAsync()
        {
            string query = "SELECT TOP 1 * FROM CompanyProfile";

            using (SqlConnection con = MainClass.GetConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        CompanyID = reader["CompanyID"] != DBNull.Value ? Convert.ToInt32(reader["CompanyID"]) : 0;
                        CompanyName = reader["CompanyName"] != DBNull.Value ? reader["CompanyName"].ToString() : "";
                        CompanyAddress = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";
                        Phone1 = reader["Phone1"] != DBNull.Value ? reader["Phone1"].ToString() : "";
                        Phone2 = reader["Phone2"] != DBNull.Value ? reader["Phone2"].ToString() : "";

                        // ✅ معالجة الصور بدون صور افتراضية
                        CompanyPic = reader["CompanyPic"] != DBNull.Value
                            ? (byte[])reader["CompanyPic"]
                            : null;

                        CompanyLogo = reader["CompanyLogo"] != DBNull.Value
                            ? (byte[])reader["CompanyLogo"]
                            : null;

                        CompanyQRCodeInfo = reader["CompanyQRCodeInfo"] != DBNull.Value
                            ? (byte[])reader["CompanyQRCodeInfo"]
                            : null;

                    }
                }
            }
        }
        private static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

    }
}

