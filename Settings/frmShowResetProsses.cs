using DevExpress.XtraSpreadsheet.PrintLayoutEngine;
using DevExpress.XtraWaitForm;
using pos.Classes;
using pos.SystemApp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace pos.Settings
{
    public partial class frmShowResetProsses : Form
    {
        private Stopwatch sw;
        private System.Windows.Forms.Timer timer;
        private frmMain mainForm1 = (frmMain)Application.OpenForms["frmMain"];
        private frmAppSetting mainForm = (frmAppSetting)Application.OpenForms["frmAppSetting"];
        public frmShowResetProsses()
        {
            InitializeComponent();
        }

        private async void btnStartReset_Click(object sender, EventArgs e)
        {
            using (frmEnterpasskey frm = new frmEnterpasskey())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    btnExit.Enabled = false;
                    btnStartReset.Enabled = false;
                    resetPanel.Enabled = true;
                    await RunDatabaseResetAsync();
                }
            }

            
        }

        private async Task RunDatabaseResetAsync()
        {
            // 🔹 Start stopwatch and timer
            sw = new Stopwatch();
            sw.Start();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // update every 1 second
            timer.Tick += (s, e) =>
            {
                lblSpend.Text = $"{sw.Elapsed.TotalSeconds:F0} seconds";
            };
            timer.Start();

            lblProsseName.Text = "System Reinitialization";
            lblProsseName.Visible = true;
            lblTableName.Text = "";
            lblTableName.Visible = true;
            pbReset.Value = 0;

            using (SqlConnection con = MainClass.GetConnection())
            {
                await con.OpenAsync();

                // 1️⃣ Get all tables except excluded ones
                string getTables = @"
                SELECT name 
                FROM sys.tables
                WHERE name NOT IN (
                    'serialNumber',
                    'trialTime',
                    'LicenseInfo'
                )";

                DataTable tables = new DataTable();
                using (SqlCommand cmd = new SqlCommand(getTables, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(tables);

                int totalTables = tables.Rows.Count;
                int processed = 0;

                // Estimated count
                lblTackTime.Text = $"{totalTables}";

                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        // 2️⃣ Delete data from all non-excluded tables
                        foreach (DataRow row in tables.Rows)
                        {
                            string tableName = row["name"].ToString();
                            lblTableName.Text = $"Currently deleting data from: {tableName}";

                            // 🧹 حذف كل البيانات
                            string deleteSQL = $"DELETE FROM [{tableName}]";
                            using (SqlCommand del = new SqlCommand(deleteSQL, con, tran))
                                await del.ExecuteNonQueryAsync();

                            // 🔁 إعادة ترقيم الـ IDENTITY (إن وجد)
                            string reseedSQL = $"DBCC CHECKIDENT ('[{tableName}]', RESEED, 0)";
                            try
                            {
                                using (SqlCommand reseedCmd = new SqlCommand(reseedSQL, con, tran))
                                    await reseedCmd.ExecuteNonQueryAsync();
                            }
                            catch
                            {
                                // بعض الجداول ممكن ما يكونش فيها IDENTITY — نتجاهل الخطأ
                            }

                            processed++;
                            pbReset.Value = (int)((processed / (float)totalTables) * 100);

                            await Task.Delay(150); // للتوضيح البصري للتقدم
                        }
                      

                        // Commit transaction
                        tran.Commit();

                        lblTableName.Text = "Operation completed successfully ✅";
                        lblProsseName.Text = "System reset finished";

                        Notifier.ShowNotification("Done ✔", "System reinitialization completed successfully.");

                        btnExit.Enabled = true;
                        btnStartReset.Enabled = true;

                        DialogResult result2 = MessageBox.Show(
                           "سيتم إعادة تشغيل البرنامج لتطبيق التغييرات.",
                           "إعادة التشغيل",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);

                        if (result2 == DialogResult.OK)
                        {
                            Application.Restart(); // إعادة تشغيل التطبيق
                            Environment.Exit(0);   // إنهاء النسخة الحالية فورًا
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        lblTableName.Text = "An error occurred during the process ❌";
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // 🔹 Stop timer and stopwatch
            timer.Stop();
            sw.Stop();

            lblSpend.Text = $"{sw.Elapsed.TotalSeconds:F1} seconds (total time)";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
