using DevExpress.Drawing;
using DevExpress.XtraBars.Customization;
using DevExpress.XtraReports.UI;
using pos.Classes;
using pos.GeneralForms;
using pos.Model;
using pos.SystemApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace pos.Settings
{
    public partial class frmGeneralSettings : Form
    {
        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;

        frmAppSetting frmParaint;

        public frmGeneralSettings(frmAppSetting frm)
        {
            InitializeComponent();
            //ThemeMode();
            frmParaint = frm; // ✅ حفظ المرجع
        }
        private async void frmGeneralSettings_Load(object sender, EventArgs e)
        {
            LoadPrintersToComboBox(cbBarcodePrinter);
            LoadPrintersToComboBox(cbBillPrinter);

            using (SqlConnection con = MainClass.GetConnection())
            {
                await con.OpenAsync();
                string qry = "SELECT TOP 1 backupPath FROM settings";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                    {
                        txtPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
                        txtPath.Text = result.ToString();

                    }
                }
            }

            printerLoad();

            await check();
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
        private async Task check()
        {
            await MainClass.LoadCompanyProfileAsync();

            // تعبئة النصوص
            txtComName.Text = MainClass.CompanyName;
            txtComAddress.Text = MainClass.CompanyAddress;
            txtComPhone1.Text = MainClass.Phone1;
            txtComPhone2.Text = MainClass.Phone2;

            // تعبئة الصور
            if (MainClass.CompanyLogo != null && MainClass.CompanyLogo.Length > 0)
            {
                using (MemoryStream msLogo = new MemoryStream(MainClass.CompanyLogo))
                    picLogo.Image = Image.FromStream(msLogo);
            }
            else
            {
                picLogo.Image = null;
            }

            if (MainClass.CompanyPic != null && MainClass.CompanyPic.Length > 0)
            {
                using (MemoryStream msBackground = new MemoryStream(MainClass.CompanyPic))
                    picBackground.Image = Image.FromStream(msBackground);
            }
            else
            {
                picBackground.Image = null;
            }

            if (MainClass.CompanyQRCodeInfo != null && MainClass.CompanyQRCodeInfo.Length > 0)
            {
                using (MemoryStream msQR = new MemoryStream(MainClass.CompanyQRCodeInfo))
                    picQRCode.Image = Image.FromStream(msQR);
            }
            else
            {
                picQRCode.Image = null;
            }


            // إعداد الحالة الأصلية للتغيير
            SetupOriginalState();

            // ربط الأحداث لمراقبة أي تغيير
            SetupChangeDetection();

            // تحديث حالة الزر لأول مرة
            UpdateSaveButtonState();

            if (frmHome.Instance != null && !frmHome.Instance.IsDisposed)
            {
                await frmHome.Instance.LoadCompanyInfoAsync();
            }

        }

        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            textColor2 = MainClass.TextColor2;
            checkedFillColor = MainClass.CheckedFillColor;
            checkedForeColor = MainClass.CheckedForeColor;
        }

        private void ThemeMode()
        {
            ThemeColor();

            this.BackColor = backgroundPrimary;
            ContenerPanel.FillColor = backgroundPrimary;
            ContenerPanel.BackColor = backgroundPrimary;
            ContenerPanel.ForeColor = textColor;

            mainPanel.FillColor = backgroundPrimary;

            posPanel.FillColor = backgroundSecondary;
            themPanel.FillColor = backgroundSecondary;

            cbLightThem.CheckedState.FillColor = checkedFillColor;
            cbDarkThem.CheckedState.FillColor = checkedFillColor;

            cbCardPos.CheckedState.FillColor = checkedFillColor;
            cbTabelPos.CheckedState.FillColor = checkedFillColor;


        }

        private void cbTabelPos_CheckedChanged(object sender, EventArgs e)
        {
            cbCardPos.Checked = !cbTabelPos.Checked;
            if (cbTabelPos.Checked)
            {
                UpdateAppSetting("DisplayModePos", "dgv");
            }
        }

        private void cbCardPos_CheckedChanged(object sender, EventArgs e)
        {
            cbTabelPos.Checked = !cbCardPos.Checked;
            if (cbCardPos.Checked)
            {
                UpdateAppSetting("DisplayModePos", "card");
            }
        }

        private void cbTabelStore_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbCardStore_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbLightThem_CheckedChanged(object sender, EventArgs e)
        {
            cbDarkThem.Checked = !cbLightThem.Checked;
            if (cbLightThem.Checked)
            {
                //UpdateAppSetting("ThemeMode", "light");
                //MainClass.themeMode();
                //ThemeMode();
                //frmParaint.themRefresh();
            }
        }

        private void cbDarkThem_CheckedChanged(object sender, EventArgs e)
        {
            cbLightThem.Checked = !cbDarkThem.Checked;
            if (cbDarkThem.Checked)
            {
                //UpdateAppSetting("ThemeMode", "dark");
                //MainClass.themeMode();
                //ThemeMode();
                //frmParaint.themRefresh();
            }
        }


        // 🔥 دالة الحفظ هنا
        private void UpdateAppSetting(string key, string value)
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.config");
            configFilePath = Path.GetFullPath(configFilePath);

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            if (config.AppSettings.Settings[key] == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void LoadPrintersToComboBox(ComboBox comboBox)
        {
            try
            {
                comboBox.Items.Clear();

                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    comboBox.Items.Add(printer);
                }

                // ✅ في حالة عدم وجود طابعات
                if (comboBox.Items.Count == 0)
                {
                    comboBox.Items.Add("لا توجد طابعات متاحة");
                    comboBox.SelectedIndex = 0;
                }
                else
                {
                    // ✅ تعيين الطابعة الافتراضية لو موجودة
                    string defaultPrinter = new PrinterSettings().PrinterName;
                    if (comboBox.Items.Contains(defaultPrinter))
                        comboBox.SelectedItem = defaultPrinter;
                    else
                        comboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                // 🛡️ منع توقف البرنامج + تسجيل الخطأ (اختياري)
                comboBox.Items.Clear();
                comboBox.Items.Add("خطأ في تحميل الطابعات");
                comboBox.SelectedIndex = 0;

                // لو عندك Notifier أو سجل أخطاء داخلي استخدمه هنا بدل MessageBox
                Console.WriteLine("LoadPrintersToComboBox error: " + ex.Message);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string qry = @"
                IF EXISTS (SELECT 1 FROM printer)
                    UPDATE printer
                    SET 
                        mainPrinter = @mainPrinter,
                        barcodePrinter = @barcodePrinter,
                        cashierPrinter1 = @cashierPrinter1,
                        cashierPrinter2 = @cashierPrinter2,
                        indexMainPrinter = @indexMainPrinter,
                        indexBarcodePrinter = @indexBarcodePrinter,
                        indexCashierPrinter1 = @indexCashierPrinter1,
                        indexCashierPrinter2 = @indexCashierPrinter2
                    WHERE ID = (SELECT TOP 1 ID FROM printer)
                ELSE
                    INSERT INTO printer
                        (mainPrinter, barcodePrinter, cashierPrinter1, cashierPrinter2,
                         indexMainPrinter, indexBarcodePrinter, indexCashierPrinter1, indexCashierPrinter2)
                    VALUES
                        (@mainPrinter, @barcodePrinter, @cashierPrinter1, @cashierPrinter2,
                         @indexMainPrinter, @indexBarcodePrinter, @indexCashierPrinter1, @indexCashierPrinter2)";


                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@mainPrinter", cbBillPrinter.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@barcodePrinter", cbBarcodePrinter.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@cashierPrinter1", DBNull.Value);
                    cmd.Parameters.AddWithValue("@cashierPrinter2", DBNull.Value);
                    cmd.Parameters.AddWithValue("@indexMainPrinter", cbBillPrinter.SelectedIndex);
                    cmd.Parameters.AddWithValue("@indexBarcodePrinter", cbBarcodePrinter.SelectedIndex);
                    cmd.Parameters.AddWithValue("@indexCashierPrinter1", DBNull.Value);
                    cmd.Parameters.AddWithValue("@indexCashierPrinter2", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }

            Notifier.ShowNotification("الإعدادات", "✅ تم حفظ الإعدادات بنجاح");
            MainClass.setPrinterName();
        }

        private void printerLoad()
        {
            // 1️⃣ حمّل أسماء الطابعات في ComboBox
            LoadPrintersToComboBox(cbBillPrinter);
            LoadPrintersToComboBox(cbBarcodePrinter);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string qry = "SELECT TOP 1 * FROM printer";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        string mainPrinterName = dr["mainPrinter"]?.ToString();
                        string barcodePrinterName = dr["barcodePrinter"]?.ToString();

                        // ✅ حدد الطابعة بناءً على الاسم
                        if (!string.IsNullOrEmpty(mainPrinterName))
                        {
                            int idx = cbBillPrinter.Items.IndexOf(mainPrinterName);
                            cbBillPrinter.SelectedIndex = idx >= 0 ? idx : -1;
                        }

                        if (!string.IsNullOrEmpty(barcodePrinterName))
                        {
                            int idx = cbBarcodePrinter.Items.IndexOf(barcodePrinterName);
                            cbBarcodePrinter.SelectedIndex = idx >= 0 ? idx : -1;
                        }
                    }
                }
            }
        }

        Byte[] imageByteArrayLogo;
        Byte[] imageByteArrayBackground;
        Byte[] imageByteArrayQRCode;
        private void btnAddLogo_Click(object sender, EventArgs e)
        {
            ChangeImage(picLogo);

        }

        private void btnAddBackground_Click(object sender, EventArgs e)
        {
            ChangeImage(picBackground);

        }

        private void btnAddQrCode_Click(object sender, EventArgs e)
        {
            ChangeImage(picQRCode);

        }

        private async void btnComSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!MainClass.EditCompanyInfo)
                {
                    messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    messageBox.Parent = (Form)this.TopLevelControl;
                    messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                    return;
                }

                // ✅ تحقق من الاسم والعنوان ورقم الهاتف
                if (string.IsNullOrWhiteSpace(txtComName.Text))
                {
                    Notifier.ShowNotification("⚠️ Missing Data", "يرجى إدخال اسم الشركة.");
                    txtComName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtComAddress.Text))
                {
                    Notifier.ShowNotification("⚠️ Missing Data", "يرجى إدخال عنوان الشركة.");
                    txtComAddress.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtComPhone1.Text))
                {
                    Notifier.ShowNotification("⚠️ Missing Data", "يرجى إدخال رقم هاتف الشركة.");
                    txtComPhone1.Focus();
                    return;
                }

                // ✅ تحويل الصور إلى بايتات (مع دعم null)
                byte[] imageByteArrayLogo = null;
                byte[] imageByteArrayBackground = null;
                byte[] imageByteArrayQRCode = null;

                if (picLogo.Image != null)
                {
                    using (MemoryStream msLogo = new MemoryStream())
                    {
                        picLogo.Image.Save(msLogo, System.Drawing.Imaging.ImageFormat.Png);
                        imageByteArrayLogo = msLogo.ToArray();
                    }
                }

                if (picBackground.Image != null)
                {
                    using (MemoryStream msBackground = new MemoryStream())
                    {
                        picBackground.Image.Save(msBackground, System.Drawing.Imaging.ImageFormat.Png);
                        imageByteArrayBackground = msBackground.ToArray();
                    }
                }

                if (picQRCode.Image != null)
                {
                    using (MemoryStream msQrCode = new MemoryStream())
                    {
                        picQRCode.Image.Save(msQrCode, System.Drawing.Imaging.ImageFormat.Png);
                        imageByteArrayQRCode = msQrCode.ToArray();
                    }
                }

                // ✅ حفظ البيانات
                await SaveCompanyProfileAsync(
                    txtComName.Text.Trim(),
                    txtComAddress.Text.Trim(),
                    txtComPhone1.Text.Trim(),
                    txtComPhone2.Text.Trim(),
                    imageByteArrayBackground,  // ممكن تكون null
                    imageByteArrayLogo,        // ممكن تكون null
                    imageByteArrayQRCode       // ممكن تكون null
                );

                await check();
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("❌ Error", $"حدث خطأ أثناء عملية الحفظ:\n{ex.Message}");
            }
        }


        public static async Task SaveCompanyProfileAsync(
            string companyName,
            string address,
            string phone1,
            string phone2,
            byte[] companyPic,
            byte[] companyLogo,
            byte[] companyQRCodeInfo)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    await con.OpenAsync();

                    // 🔍 تحقق إن كان هناك سجل بالفعل
                    string checkQuery = "SELECT COUNT(*) FROM CompanyProfile";
                    int count;
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, con))
                    {
                        count = (int)await cmdCheck.ExecuteScalarAsync();
                    }

                    // ✅ إنشاء أو تحديث
                    string sqlQuery = count == 0
                        ? @"INSERT INTO CompanyProfile 
                    (CompanyName, Address, Phone1, Phone2, CompanyPic, CompanyLogo, CompanyQRCodeInfo)
                    VALUES (@CompanyName, @Address, @Phone1, @Phone2, @CompanyPic, @CompanyLogo, @CompanyQRCodeInfo)"
                        : @"UPDATE CompanyProfile
                    SET CompanyName = @CompanyName,
                        Address = @Address,
                        Phone1 = @Phone1,
                        Phone2 = @Phone2,
                        CompanyPic = @CompanyPic,
                        CompanyLogo = @CompanyLogo,
                        CompanyQRCodeInfo = @CompanyQRCodeInfo
                    WHERE CompanyID = (SELECT TOP 1 CompanyID FROM CompanyProfile)";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        // 🟢 تحديد أنواع البيانات صراحة لتجنب الخطأ
                        cmd.Parameters.AddWithValue("@CompanyName", companyName);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Phone1", phone1);
                        cmd.Parameters.AddWithValue("@Phone2", phone2);

                        // 🖼️ صور (قد تكون NULL)
                        var paramPic = cmd.Parameters.Add("@CompanyPic", SqlDbType.Image);
                        paramPic.Value = (object)companyPic ?? DBNull.Value;

                        var paramLogo = cmd.Parameters.Add("@CompanyLogo", SqlDbType.Image);
                        paramLogo.Value = (object)companyLogo ?? DBNull.Value;

                        var paramQR = cmd.Parameters.Add("@CompanyQRCodeInfo", SqlDbType.Image);
                        paramQR.Value = (object)companyQRCodeInfo ?? DBNull.Value;

                        // ✅ تنفيذ
                        int result = await cmd.ExecuteNonQueryAsync();

                        if (result > 0)
                        {
                            if (count == 0)
                                Notifier.ShowNotification("✔ Done", "تم حفظ بيانات المتجر بنجاح.");
                            else
                                Notifier.ShowNotification("✔ Updated", "تم تحديث بيانات المتجر بنجاح.");
                        }
                        else
                        {
                            Notifier.ShowNotification("⚠️ لم يتم الحفظ", "لم يتم تعديل أي بيانات.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Notifier.ShowNotification("❌ Database Error", $"حدث خطأ في قاعدة البيانات:\n{sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("❌ Error", $"حدث خطأ أثناء حفظ بيانات الشركة:\n{ex.Message}");
            }
        }

        // بعد تحميل البيانات من MainClass
        private void SetupOriginalState()
        {
            txtComName.Tag = MainClass.CompanyName;
            txtComAddress.Tag = MainClass.CompanyAddress;
            txtComPhone1.Tag = MainClass.Phone1;
            txtComPhone2.Tag = MainClass.Phone2;

            picLogo.Tag = MainClass.CompanyLogo;         // نخزن byte[] الأصلية
            picBackground.Tag = MainClass.CompanyPic;
            picQRCode.Tag = MainClass.CompanyQRCodeInfo;
        }

        // دالة للتحقق إذا البيانات اتغيرت
        private bool IsCompanyProfileChanged()
        {
            if ((string)txtComName.Tag != txtComName.Text) return true;
            if ((string)txtComAddress.Tag != txtComAddress.Text) return true;
            if ((string)txtComPhone1.Tag != txtComPhone1.Text) return true;
            if ((string)txtComPhone2.Tag != txtComPhone2.Text) return true;

            // مقارنة الصور باستخدام الـ Tag فقط
            if (!AreImagesEqual(picLogo.Image, picLogo.Tag as byte[])) return true;
            if (!AreImagesEqual(picBackground.Image, picBackground.Tag as byte[])) return true;
            if (!AreImagesEqual(picQRCode.Image, picQRCode.Tag as byte[])) return true;

            return false;
        }
        private bool AreImagesEqual(Image img, byte[] originalBytes)
        {
            if (img == null && (originalBytes == null || originalBytes.Length == 0)) return true;
            if (img == null || originalBytes == null) return false;

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // PNG ثابت
                byte[] currentBytes = ms.ToArray();

                using (var md5 = MD5.Create())
                {
                    byte[] hashCurrent = md5.ComputeHash(currentBytes);
                    byte[] hashOriginal = md5.ComputeHash(originalBytes);
                    return hashCurrent.SequenceEqual(hashOriginal);
                }
            }
        }

        // تحديث حالة الزر
        private void UpdateSaveButtonState()
        {
            btnComSave.Enabled = IsCompanyProfileChanged();
        }

        // ربط أحداث TextBoxes وPictureBox
        private void SetupChangeDetection()
        {
            txtComName.TextChanged += (s, e) => UpdateSaveButtonState();
            txtComAddress.TextChanged += (s, e) => UpdateSaveButtonState();
            txtComPhone1.TextChanged += (s, e) => UpdateSaveButtonState();
            txtComPhone2.TextChanged += (s, e) => UpdateSaveButtonState();
        }

        // عند تغيير صورة جديدة
        private void ChangeImage(PictureBox pic)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pic.Image = Image.FromFile(ofd.FileName);
                    UpdateSaveButtonState();
                }
            }
        }

        private void btnResetSystemPass_Click(object sender, EventArgs e)
        {
            if (!MainClass.CanResetSystem)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            string password = txtNewPassSystem.Text.Trim();

            if (string.IsNullOrEmpty(password))
            {
                messageBox.Show("⚠️ أدخل كلمة مرور أولاً.");
                return;
            }

            SetResetPassword(password);
            txtNewPassSystem.Clear();
            txtOldPassSystem.Clear();
        }

        private void btnOwnPass_Click(object sender, EventArgs e)
        {
            if (!MainClass.CanResetSystem)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            string oldPass = txtOldOwnPass.Text?.Trim() ?? "";
            string newPass = txtNewOwnPass.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(newPass))
            {
                Notifier.ShowNotification("⚠️ تنبيه", "أدخل كلمة مرور جديدة أولاً!");
                return;
            }

            SetOwnerPassword(oldPass, newPass);
            txtNewOwnPass.Clear();
            txtOldOwnPass.Clear();
        }

        private void SetResetPassword(string password)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qry = @"SELECT ResetPasswordHash FROM settings";
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                // ✅ جلب الهاش المخزن في قاعدة البيانات
                                string storedHash = dt.Rows[0]["ResetPasswordHash"].ToString();

                                // ✅ التحقق من كلمة المرور المدخلة باستخدام دالة التحقق
                                if (MainClass.VerifyPassword(txtOldPassSystem.Text, storedHash))
                                {

                                    if (string.IsNullOrEmpty(password))
                                    {
                                        messageBox.Show("⚠️ أدخل كلمة مرور أولاً.");
                                        return;
                                    }

                                    SetResetPasswordDone(password);
                                }
                                else
                                {
                                    Notifier.ShowNotification("Error ❌", "❌ كلمة المرور غير صحيحة!");
                                }
                            }
                            else
                            {
                                Notifier.ShowNotification("Error ❌", "⚠️ لم يتم العثور على بيانات في جدول الإعدادات!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("Error ❌", $"❌ حدث خطأ أثناء تعيين كلمة المرور:\n{ex.Message}");
            }
        }
        public static void SetResetPasswordDone(string password)
        {
            // توليد الهاش
            string hash = HashPassword(password);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // تحديث العمود في جدول settings
                string qry = "UPDATE settings SET ResetPasswordHash = @hash";

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@hash", hash);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        Notifier.ShowNotification("Done ✔", "✅ تم حفظ باسورد الـ Reset بنجاح!");
                    else
                        Notifier.ShowNotification("Error ❌", "⚠️ لم يتم تحديث أي صف في الجدول.");
                }
            }
        }
        // ✅ دالة التحقق وتغيير كلمة المرور
        private void SetOwnerPassword(string oldPassword, string newPassword)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qry = @"SELECT TOP 1 OwnerPassHash FROM OwnerPass";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                string storedHash = dt.Rows[0]["OwnerPassHash"].ToString();

                                if (MainClass.VerifyPassword(oldPassword, storedHash))
                                {
                                    UpdateOwnerPassword(newPassword);
                                }
                                else
                                {
                                    Notifier.ShowNotification("Error ❌", "❌ كلمة المرور القديمة غير صحيحة!");
                                }
                            }
                            else
                            {
                                // أول مرة، بنعمل إدخال جديد بدل التحديث
                                InsertOwnerPassword(newPassword);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("Error ❌", $"حدث خطأ أثناء تحديث كلمة المرور:\n{ex.Message}");
            }
        }

        // ✅ إدخال أول مرة
        private void InsertOwnerPassword(string password)
        {
            string hash = HashPassword(password);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string qry = "INSERT INTO OwnerPass (OwnerPassHash) VALUES (@hash)";
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@hash", hash);
                    cmd.ExecuteNonQuery();
                }
            }

            Notifier.ShowNotification("Done ✔", "✅ تم تعيين كلمة المرور بنجاح!");
        }

        // ✅ تحديث كلمة المرور في الجدول
        private void UpdateOwnerPassword(string password)
        {
            string hash = HashPassword(password);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string qry = "UPDATE OwnerPass SET OwnerPassHash = @hash";
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@hash", hash);
                    cmd.ExecuteNonQuery();
                }
            }

            Notifier.ShowNotification("Done ✔", "✅ تم تحديث كلمة المرور بنجاح!");
        }
        // 🔸 دالة إنشاء الهاش بنفس نمط VerifyPassword
        private static string HashPassword(string password)
        {
            int iterations = 10000; // عدد الدورات
            byte[] salt = RandomNumberGenerator.GetBytes(16); // إنشاء Salt عشوائي

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // توليد الهاش بطول 32 بايت (SHA256)
                                                   // دمجهم معًا بالشكل: iterations.salt.hash
                return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (MainClass.BackupPath)
            {
                using (FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string folderPath = folderBrowserDialog1.SelectedPath;

                        using (SqlConnection con = MainClass.GetConnection())
                        {
                            con.Open();

                            string qry = @"
                            IF EXISTS (SELECT 1 FROM settings)
                                UPDATE settings
                                SET backupPath = @backupPath
                                WHERE setID = (SELECT TOP 1 setID FROM settings)
                            ELSE
                                INSERT INTO settings (backupPath, themMode)
                                VALUES (@backupPath, @themMode)";

                            using (SqlCommand cmd = new SqlCommand(qry, con))
                            {
                                cmd.Parameters.AddWithValue("@backupPath", folderPath);
                                cmd.Parameters.AddWithValue("@themMode", "Light"); // ممكن تغيرها حسب الثيم الحالي

                                cmd.ExecuteNonQuery();
                            }
                        }

                        Notifier.ShowNotification("عملية ناجحة", "تم حفظ مسار النسخ الاحتياطي بنجاح:\n" + folderPath);
                        txtPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
                        txtPath.Text = folderPath;
                    }
                }
            }
            else
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
            }
        }

        private void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            picLogo.Image.Dispose();
            picLogo.Image = null;
            btnComSave.Enabled = true;
        }

        private void btnDeleteBackground_Click(object sender, EventArgs e)
        {
            picBackground.Image.Dispose();
            picBackground.Image = null;
            btnComSave.Enabled = true;

        }

        private void btnDeleteQrCode_Click(object sender, EventArgs e)
        {
            picQRCode.Image.Dispose();
            picQRCode.Image = null;
            btnComSave.Enabled = true;

        }
    }
}
