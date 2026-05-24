using Microsoft.Data.Sql;
using pos.Classes;
using pos.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace pos.SystemApp
{
    public partial class FormInishialSettings : Form
    {
        public bool createDatabase = false;
        public FormInishialSettings()
        {
            InitializeComponent();
            
        }

        private async void FormInishialSettings_Load(object sender, EventArgs e)
        {
            if (createDatabase)
                btnCreateDatabase.Visible = true;
            else
                btnCreateDatabase.Visible = false;

            DBConfig config = DBConfig.Load();

            txtSqlServerName.Text = config.Server;
            txtDatabase.Text = config.Database;
            txtUserName.Text = config.User;
            cbLogin.Checked = config.sqlAuthentication;
        }


        private void cbLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLogin.Checked)
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
            else
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                txtUserName.Text = String.Empty;
                txtPassword.Text = String.Empty;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string encryptedPassword = MainClass.EncryptText(txtPassword.Text);

            DBConfig config = new DBConfig
            {
                Server = txtSqlServerName.Text,
                Database = txtDatabase.Text,
                sqlAuthentication = cbLogin.Checked,
                User = txtUserName.Text,
                Password = encryptedPassword

            };

            config.Save();
            Notifier.ShowNotification("تم", "✅ تم حفظ الإعدادات");
            this.Close();
        }

        private void btnDifferential_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmRestoreBackup frm = new frmRestoreBackup();
            frm.creatDatabase = true;
            frm.ShowDialog(this);
            this.Show();
        }
    }
}
