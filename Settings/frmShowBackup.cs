using DevExpress.XtraMap.ItemEditor;
using pos.Classes;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.GeneralForms.MainForm
{
    public partial class frmShowBackup : Form
    {
        private Stopwatch sw;
        private string backupFilePath;
        private string customBackupFolderPath;
        private int currentPercent = 0;
        private System.Windows.Forms.Timer uiTimer;
        private CancellationTokenSource spendCts;
        public string backupType; // ✅ نوع النسخة (Full/Differential)
        public bool showNotification = true;

        // ✅ الكونستركتور الافتراضي (Full)
        public frmShowBackup()
        {
            InitializeComponent();
            backupType = "FULL"; // افتراضي نسخة كاملة
        }

        // ✅ كونستركتور بمسار مخصص + نوع النسخة
        public frmShowBackup(string backupFolderPath) : this()
        {
            customBackupFolderPath = backupFolderPath;
        }

        private async void frmShowBackup_Load(object sender, EventArgs e)
        {
            DateTime? lastFullBackupDate = null;

            using (SqlConnection con = MainClass.GetConnection())
            {
                await con.OpenAsync();
                string qry = "SELECT TOP 1 LastFullBackupDate FROM settings";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                        lastFullBackupDate = Convert.ToDateTime(result);
                }
            }

            // 🔹 عرض آخر تاريخ أو رسالة افتراضية
            lblLastDate.Text = lastFullBackupDate.HasValue
                ? lastFullBackupDate.Value.ToString("dd-MM-yyyy HH:mm:ss")
                : "لا توجد نسخة احتياطية";

            // 🔹 حساب الأيام المتبقية (لو فيه تاريخ سابق)
            int remainingDays = 7;
            if (lastFullBackupDate.HasValue)
            {
                DateTime currentDate = GetServerDate().Date;
                int usedDays = (currentDate - lastFullBackupDate.Value.Date).Days;
                remainingDays = Math.Max(0, 7 - usedDays);
            }

            lblRemainingDays.Text = $"متبقي ({remainingDays} يوم)";

            // 🔹 حالة نسخة كاملة
            if (remainingDays == 0)
            {
                PrepareBackupUi();
                backupType = "FULL";
                await BackUpWithProgressAsync();
                return;
            }

            // 🔹 حالة نسخة تفاضلية
            if (backupType == "DIFFERENTIAL")
            {
                PrepareBackupUi();
                await BackUpWithProgressAsync();
            }
        }

        private void PrepareBackupUi()
        {
            btnDifferential.Visible = false;
            btnFull.Visible = false;
            btnExit.Visible = false;
            bottomPanel.BackColor = backupPanel.BackColor;
            backupPanel.Enabled = true;
        }

        private DateTime GetServerDate()
        {
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT GETDATE()", con))
            {
                con.Open();
                return Convert.ToDateTime(cmd.ExecuteScalar());
            }
        }
        private async Task BackUpWithProgressAsync()
        {
            string backupFolderPath = customBackupFolderPath;

            if (string.IsNullOrWhiteSpace(backupFolderPath))
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    await con.OpenAsync();
                    string qry = "SELECT TOP 1 backupPath FROM settings";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        object result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                            backupFolderPath = result.ToString();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(backupFolderPath))
            {
                Notifier.ShowNotification("تحذير", "أضف مسار النسخ الاحتياطي");
                this.Close();
                return;
            }

            if (!Directory.Exists(backupFolderPath))
                Directory.CreateDirectory(backupFolderPath);

            // اسم الملف حسب نوع النسخة
            string backupFileName = backupType == "DIFFERENTIAL"
                ? "DiffBackup.bak"
                : "FullBackup.bak";

            if(backupType == "FULL")
            {
                foreach (string file in Directory.GetFiles(backupFolderPath, "*.bak"))
                { 
                    try 
                    { 
                        File.Delete(file); 
                    } 
                    catch { } 
                }
            }

            backupFilePath = Path.Combine(backupFolderPath, backupFileName);

            sw = new Stopwatch();
            sw.Start();

            spendCts = new CancellationTokenSource();
            Task.Run(() => UpdateSpendTime(spendCts.Token));

            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 1000;
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();

            try
            {
                using (SqlConnection con2 = MainClass.GetConnection())
                {
                    con2.FireInfoMessageEventOnUserErrors = true;
                    con2.InfoMessage += (s, e) =>
                    {
                        foreach (SqlError err in e.Errors)
                        {
                            string msg = err.Message;
                            if (msg.Contains("processed"))
                            {
                                var match = Regex.Match(msg, @"(\d+)\spercent");
                                if (match.Success)
                                {
                                    int percent = int.Parse(match.Groups[1].Value);
                                    currentPercent = percent;
                                    pbBackup.Invoke(new Action(() => pbBackup.Value = percent));
                                }
                            }
                        }
                    };

                    await con2.OpenAsync();

                    // ✅ الاستعلام حسب نوع النسخة
                    string backupQuery = backupType == "DIFFERENTIAL"
                        ? @"BACKUP DATABASE [smartpos] TO DISK = @path WITH INIT, STATS = 1, DIFFERENTIAL;"
                        : @"BACKUP DATABASE [smartpos] TO DISK = @path WITH INIT, STATS = 1;";

                    using (SqlCommand cmd2 = new SqlCommand(backupQuery, con2))
                    {
                        cmd2.Parameters.AddWithValue("@path", backupFilePath);
                        cmd2.CommandTimeout = 0;
                        await cmd2.ExecuteNonQueryAsync();
                    }
                }

                sw.Stop();
                uiTimer.Stop();
                spendCts.Cancel();

                UpdateUi();

                if (backupType == "FULL")
                {
                    using (SqlConnection con = MainClass.GetConnection())
                    {
                        await con.OpenAsync();
                        string qry = "UPDATE settings SET LastFullBackupDate = @date";
                        using (SqlCommand cmd2 = new SqlCommand(qry, con))
                        {
                            cmd2.Parameters.AddWithValue("@date", DateTime.Now);
                            await cmd2.ExecuteNonQueryAsync();
                        }
                    }
                }
                if (showNotification)
                {
                    Notifier.ShowNotification(
                        "نجاح",
                        backupType == "DIFFERENTIAL"
                            ? $"تم إنشاء نسخة تفاضلية بنجاح ✅\nالمسار: {backupFilePath}"
                            : $"تم إنشاء نسخة كاملة بنجاح ✅\nالمسار: {backupFilePath}"
                    );
                }             

                this.Invoke(new Action(() => this.Close()));
            }
            catch (Exception ex)
            {
                uiTimer.Stop();
                spendCts.Cancel();
                MessageBox.Show("خطأ أثناء النسخ الاحتياطي:\n" + ex.Message);
            }
        }

        private void UpdateSpendTime(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        lblSpend.Text = $"{sw.Elapsed:hh\\:mm\\:ss}";
                    }));
                }
                catch { }
                Thread.Sleep(1000);
            }
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            UpdateUi();
        }

        private void UpdateUi()
        {
            long fileSize = 0;
            if (File.Exists(backupFilePath))
                fileSize = new FileInfo(backupFilePath).Length;
            double fileSizeMB = fileSize / (1024.0 * 1024.0);
            lblSize.Text = $"{fileSizeMB:F2} MB";

            double seconds = sw.Elapsed.TotalSeconds > 0 ? sw.Elapsed.TotalSeconds : 1;
            double speed = fileSizeMB / seconds;
            lblSpeed.Text = $"{speed:F2} MB/s";

            if (currentPercent > 0)
            {
                double estimatedTotalSeconds = (seconds / currentPercent) * 100;
                TimeSpan remaining = TimeSpan.FromSeconds(estimatedTotalSeconds - seconds);
                lblTackTime.Text = $"{remaining:hh\\:mm\\:ss}";
            }
        }

        private async void btnFull_Click(object sender, EventArgs e)
        {
            btnDifferential.Enabled = false;
            btnFull.Enabled = false;
            btnExit.Enabled = false;
            backupPanel.Enabled = true;
            lblLastDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            backupType = "FULL"; // تعيين نوع النسخة إلى كاملة
            await BackUpWithProgressAsync();

        }

        private async void btnDifferential_Click(object sender, EventArgs e)
        {
            btnDifferential.Enabled = false;
            btnFull.Enabled = false;
            btnExit.Enabled = false;
            backupPanel.Enabled = true;

            backupType = "DIFFERENTIAL"; // تعيين نوع النسخة إلى تفاضلية
            await BackUpWithProgressAsync();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
