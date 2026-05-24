using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace pos.SystemApp
{
    public partial class frmSettingPOS : Form
    {
        public frmSettingPOS()
        {
            InitializeComponent();
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        int change = 0;
        int pX = 0;
        private void frmSettingPOS_Load(object sender, EventArgs e)
        {
            txtX.Focus();
            this.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int radius = 12; // تقليل قطر الدائرة لجعل الحواف أقل دائرية

                // أركان النافذة
                Rectangle corner1 = new Rectangle(0, 0, radius * 2, radius * 2);
                Rectangle corner2 = new Rectangle(this.Width - radius * 2, 0, radius * 2, radius * 2);
                Rectangle corner3 = new Rectangle(0, this.Height - radius * 2, radius * 2, radius * 2);
                Rectangle corner4 = new Rectangle(this.Width - radius * 2, this.Height - radius * 2, radius * 2, radius * 2);

                path.AddArc(corner1, 180, 90);
                path.AddArc(corner2, 270, 90);
                path.AddArc(corner4, 0, 90);
                path.AddArc(corner3, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            };
            string appBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configFilePath = Path.Combine(Application.StartupPath, @"..\..\..\Settings\Settings.config");

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(Directory.GetCurrentDirectory(), "Settings.config") };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            if (settings["ucProdact_X"] != null && settings["ucProdact_Y"] != null)
            {
                txtX.Text = settings["ucProdact_X"].Value;
                txtY.Text = settings["ucProdact_Y"].Value;
                dgvX.Text = settings["panel1Size_X"].Value;
                pX = int.Parse(settings["panel1Size_X"].Value);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateSetting("ucProdact_X", txtX.Text);
            UpdateSetting("ucProdact_Y", txtY.Text);
            UpdateSetting("panel1Size_X", dgvX.Text);

            change = int.Parse(dgvX.Text) - pX;
            UpdateSetting("cPanel1Size_X", change.ToString());
            Application.Restart();
            Environment.Exit(0);
        }

        public void UpdateSetting(string key, string value)
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.config");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFilePath);

            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");
            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes != null)
                {
                    XmlAttribute attribute = childNode.Attributes["key"];
                    if (attribute != null && attribute.Value == key)
                    {
                        childNode.Attributes["value"].Value = value;
                        break;
                    }
                }
            }

            xmlDoc.Save(configFilePath);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // يمكنك استخدام أي قيمة من DialogResult تراها مناسبة

            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            txtX.Text = "220";
            txtY.Text = "180";
            dgvX.Text = "348";
            change = 0;
            UpdateSetting("ucProdact_X", txtX.Text);
            UpdateSetting("ucProdact_Y", txtY.Text);
            UpdateSetting("panel1Size_X", dgvX.Text);
            UpdateSetting("cPanel1Size_X", change.ToString());
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
