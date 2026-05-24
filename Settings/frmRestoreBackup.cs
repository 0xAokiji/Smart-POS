using DevExpress.XtraMap.ItemEditor;
using pos.Classes;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Settings
{
    public partial class frmRestoreBackup : Form
    {
        private Stopwatch sw;
        private System.Windows.Forms.Timer uiTimer;
        private int currentPercent = 0;
        private string fullPath;
        private string diffPath;
        private string dbName = "smartpos"; // ✏️ عدل اسم قاعدة البيانات
        public bool creatDatabase = false;
        public frmRestoreBackup()
        {
            InitializeComponent();
        }
        private async void frmRestoreBackup_Load(object sender, EventArgs e)
        {
            GetMasterConnectionString();
            if (creatDatabase)
            {
                btnDiffPath.Enabled = false;
                btnFullPath.Enabled = false;
                txtDiff.Enabled = false;
                txtFull.Enabled = false;
                txtDatabaseName.Enabled = false;
                fullPath =  Path.Combine(Application.StartupPath, "Database", "NewDatabase.bak");
                txtFull.Text = fullPath;
                backupPanel.Enabled = true;
                bottomPanel.Enabled = false;
                await RestoreDatabaseAsync();

            }
            else
            {
                btnDiffPath.Enabled = true;
                btnFullPath.Enabled = true;
                txtDiff.Enabled = true;
                txtFull.Enabled = true;
                txtDatabaseName.Enabled = true;
            }
            winProgressBar.Style = ProgressBarStyle.Marquee;
            winProgressBar.MarqueeAnimationSpeed = 10;
        }
        // ✅ اتصال مع master DB
        private string GetMasterConnectionString()
        {
            DBConfig config = DBConfig.Load();

            string serverName = config.Server;
            string databaseName = config.Database; // هتستخدمه لاحقاً مش في الكونكشن نفسه
            string dbUserName = config.User;
            string decrypted = DecryptText(config.Password);

            txtDatabaseName.Text = databaseName;

            // إزالة BOM إذا موجود
            if (!string.IsNullOrEmpty(decrypted) && decrypted[0] == '\uFEFF')
                decrypted = decrypted.Substring(1);

            // إزالة أي whitespace إضافية
            string dbPassword = decrypted.Trim();

            bool sqlAuthentication = config.sqlAuthentication;

            if (sqlAuthentication)
            {
                // SQL Auth → اتصل بـ master
                return $"Server={serverName};Database=master;User Id={dbUserName};Password={dbPassword};";
            }
            else
            {
                // Windows Auth
                return $"Server={serverName};Database=master;Integrated Security=True;";
            }
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

        private async Task RestoreDatabaseAsync()
        {
            if (string.IsNullOrWhiteSpace(dbName))
            {
                MessageBox.Show("❌ من فضلك أدخل اسم قاعدة البيانات أولاً");
                return;
            }

            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
            {
                MessageBox.Show("❌ من فضلك اختر ملف النسخة الكاملة أولاً");
                return;
            }

            if (!string.IsNullOrEmpty(diffPath) && !File.Exists(diffPath))
            {
                MessageBox.Show("❌ ملف النسخة التفاضلية غير موجود");
                return;
            }

            sw = new Stopwatch();
            sw.Start();

            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 1000;
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();

            try
            {
                using (SqlConnection con = new SqlConnection(GetMasterConnectionString()))
                {
                    con.FireInfoMessageEventOnUserErrors = true;
                    con.InfoMessage += (s, e) =>
                    {
                        foreach (SqlError err in e.Errors)
                        {
                            string msg = err.Message;
                            if (msg.Contains("percent"))
                            {
                                var parts = msg.Split(' ');
                                if (int.TryParse(parts[0], out int percent))
                                {
                                    currentPercent = percent;
                                    pbRestore.Invoke(new Action(() => pbRestore.Value = percent));
                                }
                            }
                        }
                    };

                    await con.OpenAsync();

                    // 🔹 استخراج أسماء الملفات من النسخة الاحتياطية
                    string logicalDataName = null;
                    string logicalLogName = null;

                    using (SqlCommand cmd = new SqlCommand("RESTORE FILELISTONLY FROM DISK = @full", con))
                    {
                        cmd.Parameters.AddWithValue("@full", fullPath);
                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            if (await dr.ReadAsync())
                            {
                                logicalDataName = dr["LogicalName"].ToString();
                                if (await dr.ReadAsync())
                                    logicalLogName = dr["LogicalName"].ToString();
                            }
                        }
                    }

                    // 🔹 المسار الافتراضي الآمن لمجلد SQL Data
                    string defaultDataPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        @"Microsoft\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA");

                    if (!Directory.Exists(defaultDataPath))
                        defaultDataPath = @"C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA";

                    string mdfPath = Path.Combine(defaultDataPath, $"{dbName}.mdf");
                    string ldfPath = Path.Combine(defaultDataPath, $"{dbName}_log.ldf");

                    // 1️⃣ قفل قاعدة البيانات لو موجودة
                    using (SqlCommand cmd = new SqlCommand($"IF EXISTS (SELECT name FROM sys.databases WHERE name = '{dbName}') ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", con))
                        await cmd.ExecuteNonQueryAsync();

                    // 2️⃣ استرجاع النسخة الكاملة مع MOVE
                    string restoreFull = $@"
                RESTORE DATABASE [{dbName}]
                FROM DISK = @full
                WITH 
                    MOVE N'{logicalDataName}' TO N'{mdfPath}',
                    MOVE N'{logicalLogName}' TO N'{ldfPath}',
                    REPLACE, STATS = 1, NORECOVERY;";

                    using (SqlCommand cmd = new SqlCommand(restoreFull, con))
                    {
                        cmd.Parameters.AddWithValue("@full", fullPath);
                        cmd.CommandTimeout = 0;
                        await cmd.ExecuteNonQueryAsync();
                    }

                    // 3️⃣ استرجاع النسخة التفاضلية (إن وجدت)
                    if (!string.IsNullOrEmpty(diffPath))
                    {
                        string restoreDiff = $@"
                    RESTORE DATABASE [{dbName}]
                    FROM DISK = @diff
                    WITH 
                        MOVE N'{logicalDataName}' TO N'{mdfPath}',
                        MOVE N'{logicalLogName}' TO N'{ldfPath}',
                        STATS = 1, NORECOVERY;";

                        using (SqlCommand cmd = new SqlCommand(restoreDiff, con))
                        {
                            cmd.Parameters.AddWithValue("@diff", diffPath);
                            cmd.CommandTimeout = 0;
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    // 4️⃣ إنهاء العملية واسترجاع القاعدة أونلاين
                    using (SqlCommand cmd = new SqlCommand($"RESTORE DATABASE [{dbName}] WITH RECOVERY;", con))
                        await cmd.ExecuteNonQueryAsync();

                    // 5️⃣ إعادة القاعدة للوضع العادي
                    using (SqlCommand cmd = new SqlCommand($"ALTER DATABASE [{dbName}] SET MULTI_USER;", con))
                        await cmd.ExecuteNonQueryAsync();
                }

                sw.Stop();
                uiTimer.Stop();
                UpdateUi();

                Notifier.ShowNotification("نجاح", "✅ تم استرجاع النسخة الاحتياطية بنجاح من أي جهاز");
                this.Close();
            }
            catch (Exception ex)
            {
                uiTimer.Stop();
                MessageBox.Show("❌ خطأ أثناء الاسترجاع:\n" + ex.Message);
            }
        }


        private void UiTimer_Tick(object sender, EventArgs e) => UpdateUi();

        private bool isMarqueeStarted = false; // تعريف في أعلى الكلاس

        private void UpdateUi()
        {
            long fileSize = 0;
            if (File.Exists(fullPath))
                fileSize += new FileInfo(fullPath).Length;
            if (!string.IsNullOrEmpty(diffPath) && File.Exists(diffPath))
                fileSize += new FileInfo(diffPath).Length;

            double fileSizeMB = fileSize / (1024.0 * 1024.0);
            lblSize.Text = $"{fileSizeMB:F2} MB";
            lblSpend.Text = $"{sw.Elapsed:hh\\:mm\\:ss}";

            if (currentPercent > 0 && currentPercent < 100)
            {
                pbRestore.Value = currentPercent;

                double seconds = sw.Elapsed.TotalSeconds > 0 ? sw.Elapsed.TotalSeconds : 1;
                double speed = fileSizeMB / seconds;
                lblSpeed.Text = $"{speed:F2} MB/s";

                double estimatedTotalSeconds = (seconds / currentPercent) * 100;
                TimeSpan remaining = TimeSpan.FromSeconds(estimatedTotalSeconds - seconds);
                lblTackTime.Text = $"{remaining:hh\\:mm\\:ss}";
            }
            else if (currentPercent >= 100)
            {
                if (!isMarqueeStarted) // ✅ نفذ مرة واحدة فقط
                {
                    // اخفاء Guna2ProgressBar
                    pbRestore.Visible = false;

                    // ضبط موقع وحجم winProgressBar مطابق للـ Guna2ProgressBar
                    winProgressBar.Location = pbRestore.Location;
                    winProgressBar.Size = pbRestore.Size;

                    // اظهار ProgressBar WinForms العادي
                    winProgressBar.Visible = true;

                    // نص الاسترجاع بدون نقاط
                    lblSpeed.RightToLeft = RightToLeft.No;
                    lblSpeed.Text = "🔄 جاري إنهاء عملية الاسترجاع";
                    lblSpeedName.Visible = false;
                    // صفر الوقت المتوقع
                    lblTackTime.Text = "00:00:00";

                    isMarqueeStarted = true; // ✅ تم تفعيل Marquee مرة واحدة
                }
            }


        }




        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private async void btnFull_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fullPath) && creatDatabase)
            {
                MessageBox.Show("❌ يجب اختيار نسخة كاملة أولاً");
                return;
            }

            btnFull.Enabled = false;
            btnClose.Enabled = false;
            btnDifferential.Enabled = false;
            backupPanel.Enabled = true;
            dbName = txtDatabaseName.Text.Trim();
            await RestoreDatabaseAsync();
        }

        private async void btnDifferential_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                MessageBox.Show("❌ يجب اختيار نسخة كاملة أولاً قبل استرجاع نسخة تفاضلية");
                return;
            }

            btnFull.Enabled = false;
            btnClose.Enabled = false;
            btnDifferential.Enabled = false;
            backupPanel.Enabled = true;
            dbName = txtDatabaseName.Text.Trim();
            await RestoreDatabaseAsync();
        }

        private void btnFullPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "اختر ملف النسخة الكاملة";
                dlg.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    fullPath = dlg.FileName;
                    txtFull.Text = fullPath;
                    txtFull.TextAlign = HorizontalAlignment.Left;

                    // ✅ جلب تاريخ النسخة الاحتياطية ووضعه في lblDate
                    DateTime? backupDate = GetFullBackupDate(fullPath);
                    if (backupDate.HasValue)
                        lblDate.Text = backupDate.Value.ToString("dd-MM-yyyy HH:mm:ss");
                    else
                        lblDate.Text = "❌ لم يتم التعرف على تاريخ النسخة";

                    btnDifferential.Enabled = true; // ✅ تفعيل زر التفاضلية بعد اختيار نسخة كاملة
                }
            }
        }


        private void btnDiffPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "اختر ملف النسخة التفاضلية";
                dlg.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    diffPath = dlg.FileName;
                    txtDiff.Text = diffPath;
                    txtDiff.TextAlign = HorizontalAlignment.Left;
                }
            }
        }

        // دالة لإرجاع تاريخ النسخة الاحتياطية من ملف bak
        private DateTime? GetFullBackupDate(string backupFile)
        {
            if (string.IsNullOrEmpty(backupFile) || !File.Exists(backupFile))
                return null;

            try
            {
                using (SqlConnection con = new SqlConnection(GetMasterConnectionString()))
                {
                    con.Open();
                    string query = "RESTORE HEADERONLY FROM DISK = @backupFile";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@backupFile", backupFile);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                // العمود BackupFinishDate يحتوي على تاريخ الانتهاء الفعلي للنسخة
                                return dr.GetDateTime(dr.GetOrdinal("BackupFinishDate"));
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

    }
}
