using DevExpress.XtraBars;
using Microsoft.VisualBasic.Devices;
using pos.SystemApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.View
{
    public partial class frmGraphicalinterFace : Form
    {
        private bool trus = true;
        private int progress = 0;
        private bool oneTimeCheck;


        public frmGraphicalinterFace(bool oneTime)
        {
            InitializeComponent();
            this.timer1.Interval = 60;
            this.timer1.Start();

            oneTimeCheck = oneTime;

        }

        private bool checkedOnce = false;

        private async void timer1_Tick(object sender, EventArgs e)
        {
            progress += 2;

            if (!checkedOnce)
            {
                checkedOnce = true;

                Exception dbException = null;

                bool ok = await Task.Run(() =>
                {
                    try
                    {
                        if (!File.Exists("config.json"))
                            throw new FileNotFoundException("⚠️ Configuration file (config.json) is missing! Please create it first.");

                        DBConfig config = DBConfig.Load();
                        if (config == null || string.IsNullOrEmpty(config.Database))
                            throw new Exception("⚠️ Database configuration is missing or invalid inside config.json.");

                        string actualSerial = "";
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            actualSerial = obj["SerialNumber"].ToString();
                            break;
                        }

                        if (!DatabaseExists(config.Database))
                            throw new Exception("❌ قاعدة البيانات غير موجودة!");

                        using (SqlConnection con = MainClass.GetConnection())
                        {
                            con.Open();

                            string dbSerial = "";
                            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 serial FROM serialNumber", con))
                            {
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                    dbSerial = result.ToString();
                            }

                            if (!string.Equals(actualSerial, dbSerial, StringComparison.OrdinalIgnoreCase))
                                return false;
                        }

                        CheckDate();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbException = ex;
                        return false;
                    }
                });

                if (!ok)
                {
                    timer1.Stop();

                    if (dbException != null)
                    {
                        if (this.IsHandleCreated)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                ShowDbError(dbException);
                            });
                        }
                        else
                        {
                            ShowDbError(dbException);
                        }
                    }
                    else
                    {
                        guna2MessageDialog1.Show("❌ البرنامج غير متوفر لهذا الجهاز أو تم التلاعب في البيانات");
                        Application.Exit();
                    }

                    return;
                }
            }

            // ✅ بدل شريط التحميل القديم بالـ ProgressBar
            progressBar.Value = Math.Min(progress, 100);

            if (progress == 100)
            {
                timer1.Stop();

                if (trus)
                {
                    if (oneTimeCheck) return;
                    if (!Application.OpenForms.OfType<frmLogin>().Any())
                    {
                        frmLogin frmLogin = new frmLogin();
                        frmLogin.FormClosed += (s, args) =>
                        {
                            timer1.Stop();
                        };
                        this.Hide();
                        frmLogin.Show();
                    }

                    oneTimeCheck = true;
                }
            }
        }



        private void ShowDbError(Exception ex)
        {
            string errorMsg = "Database Error:\n" + ex.Message;

            guna2MessageDialog1.Caption = "Error";
            guna2MessageDialog1.Text = errorMsg;
            guna2MessageDialog1.Show();

            // ✅ بعد عرض الرسالة، نفتح صفحة الإعدادات
            if (!Application.OpenForms.OfType<FormInishialSettings>().Any())
            {
                FormInishialSettings frmError = new FormInishialSettings
                {
                    createDatabase = true
                };

                this.Hide();
                frmError.ShowDialog();
            }

            // بعد إغلاق صفحة الإعدادات، نغلق التطبيق
            Application.Exit();
        }



        private void frmGraphicalinterFace_Load(object sender, EventArgs e)
        {

        }

        private bool DatabaseExists(string dbName)
        {
            using (SqlConnection con = new SqlConnection(MainClass.GetMasterConnectionString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM sys.databases WHERE name = @dbName", con))
                {
                    cmd.Parameters.AddWithValue("@dbName", dbName);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool VerifyFromDatabase(SqlConnection con)
        {
            string query = "SELECT TOP 1 serial, isFirstTime, Signature FROM serialNumber";
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string serial = reader.GetString(0);
                    bool isFirstTime = reader.GetBoolean(1);
                    string dbSignature = reader.GetString(2);

                    string expectedSignature = SerailSigner.ComputeSignature(serial, isFirstTime);

                    return dbSignature == expectedSignature;
                }
            }
            return false;
        }

        private static DateTime GetNetworkTime(string ntpServer = "pool.ntp.org")
        {
            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                socket.ReceiveTimeout = 3000;
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            const byte serverReplyTime = 40;
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            DateTime networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }

        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        private void CheckDate()
        {
            bool state = false; 
            try
            {
                DateTime networkTime = GetNetworkTime("pool.ntp.org"); 
                DateTime localTime = DateTime.Now;

                if (networkTime.Date == localTime.Date)
                {
                    state = false;
                }
                else
                {

                    trus = false;
                    state = true; 
                    guna2MessageDialog1.Show("الرجاء ضبط التاريخ");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
            }


        }


    }
}
