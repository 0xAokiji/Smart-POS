using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using pos.AccountManagement;
using pos.Classes;
using pos.GeneralForms;
using pos.GeneralForms.MainForm;
using pos.Model;
using pos.Settings;
using pos.View;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.SystemApp
{
    public partial class frmAppSetting : Form
    {
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color checkedFillColor;
        private Color checkedForColor;

        //Fields
        private int bordarRadius = 15;
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(32, 32, 32);

        frmMian2 frmParaint;
        public frmAppSetting(frmMian2 frm)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.ShowInTaskbar = false;

            frmParaint = frm;

            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();
        }
        public frmAppSetting()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize);
            this.ShowInTaskbar = false;


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
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;       // WS_EX_TOOLWINDOW - لجعل الفورم لا يظهر في شريط المهام
                return cp;
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private GraphicsPath GetRoundedPath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        private void FormRegionAndBorder(Form form, float radius, Graphics graph, Color borderColor, float borderSize)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                using (GraphicsPath roundPath = GetRoundedPath(form.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    form.Region = new Region(roundPath);
                    if (borderSize >= 1)
                    {
                        Rectangle rect = form.ClientRectangle;
                        float scaleX = 1.0F - ((borderSize + 1) / rect.Width);
                        float scaleY = 1.0F - ((borderSize + 1) / rect.Height);
                        transform.Scale(scaleX, scaleY);
                        transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                        graph.Transform = transform;
                        graph.DrawPath(penBorder, roundPath);
                    }
                }
            }
        }
        private void ControlRegionAndBorder(Control control, float radius, Graphics graph, Color borderColor)
        {
            using (GraphicsPath roundPath = GetRoundedPath(control.ClientRectangle, radius))
            using (Pen penBorder = new Pen(borderColor, 1))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                control.Region = new Region(roundPath);
                graph.DrawPath(penBorder, roundPath);
            }
        }
        private void DrawPath(Rectangle rect, Graphics graph, Color color)
        {
            using (GraphicsPath roundPath = GetRoundedPath(rect, bordarRadius))
            using (Pen penBorder = new Pen(color, 3))
            {
                graph.DrawPath(penBorder, roundPath);
            }
        }
        private struct FormBoundsColors
        {
            public Color TopLeftColor;
            public Color TopRightColor;
            public Color BottomLeftColor;
            public Color BottomRightColor;
        }
        private FormBoundsColors GetFormBoundsColors()
        {
            var fbColor = new FormBoundsColors();
            using (var bmp = new Bitmap(1, 1))
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle rectBmp = new Rectangle(0, 0, 1, 1);
                //Top Left
                rectBmp.X = this.Bounds.X - 1;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopLeftColor = bmp.GetPixel(0, 0);
                //Top Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopRightColor = bmp.GetPixel(0, 0);
                //Bottom Left
                rectBmp.X = this.Bounds.X;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomLeftColor = bmp.GetPixel(0, 0);
                //Bottom Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomRightColor = bmp.GetPixel(0, 0);
            }
            return fbColor;
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //-> SMOOTH OUTER DORDER
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle recForm = this.ClientRectangle;
            int mWight = recForm.Width / 2;
            int mHight = recForm.Height / 2;
            var fbColors = GetFormBoundsColors();

            //Top Left
            DrawPath(recForm, e.Graphics, fbColors.TopLeftColor);

            //Top Right
            Rectangle recTopRight = new Rectangle(mWight, recForm.Y, mWight, mHight);
            DrawPath(recTopRight, e.Graphics, fbColors.TopRightColor);

            //Bottom Left
            Rectangle recBottomLeft = new Rectangle(recForm.X, recForm.X + mHight, mWight, mHight);
            DrawPath(recBottomLeft, e.Graphics, fbColors.BottomLeftColor);

            //Bottom Right
            Rectangle recBottomRight = new Rectangle(mWight, recForm.Y + mHight, mWight, mHight);
            DrawPath(recBottomRight, e.Graphics, fbColors.BottomRightColor);
        }
        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void frmGeneralSetting_Paint(object sender, PaintEventArgs e)
        {
            FormRegionAndBorder(this, bordarRadius, e.Graphics, borderColor, borderSize);
        }
        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            ControlRegionAndBorder(panelContainer, bordarRadius - (borderSize / 2), e.Graphics, borderColor);

        }
        private void frmGeneralSetting_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void frmGeneralSetting_Activated(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void frmGeneralSetting_ResizeEnd(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void frmGeneralSetting_Load(object sender, EventArgs e)
        {
            updateImage();

            lblUserName.Text = MainClass.USER;
            lblUserName.Left = (userImgPanel.Width - lblUserName.Width) / 2;

            UpdateSubTitelLable("الملف الشخصي");
            AddControlsSenter(new frmProfile(this), false);

        }
        public void updateImage()
        {
            if (MainClass.IMAGEBYTES != null)
            {
                using (MemoryStream stream = new MemoryStream(MainClass.IMAGEBYTES))
                {

                    userImage.Image = Image.FromStream(stream);
                }
            }
        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!MainClass.AddUser)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (btnAddUser.Checked == false)
            {
                btnAddUser.Checked = true;
                btnprofile.Checked = false;
                btnGsetting1.Checked = false;
                btnLogout.Checked = false;
                btnPath.Checked = false;
                btnPermission.Checked = false;
                btnRecovery.Checked = false;
                btnSaveBackup.Checked = false;
                btnStaff.Checked = false;

                mainPanel.Visible = true;

                UpdateSubTitelLable("اضافة مستخدم جديد");
                AddControlsSenter(new frmNewUser(), false);
            }

        }
        void UpdateSubTitelLable(string txt)
        {
            lblSubTittel.Text = txt;

            //lblSubTittel.Left = subTiltePanel.Width - lblSubTittel.Width;

            CenterLabelInPanel(subTiltePanel, lblSubTittel);

        }
        private void btnChangePass_Click(object sender, EventArgs e)
        {
            if (btnprofile.Checked == false)
            {
                btnAddUser.Checked = false;
                btnprofile.Checked = true;
                btnGsetting1.Checked = false;
                btnLogout.Checked = false;
                btnPath.Checked = false;
                btnPermission.Checked = false;
                btnRecovery.Checked = false;
                btnSaveBackup.Checked = false;
                btnStaff.Checked = false;

                mainPanel.Visible = true;

                UpdateSubTitelLable("الملف الشخصي");
                AddControlsSenter(new frmProfile(this), false);
            }


        }

        private void btnPermission_Click(object sender, EventArgs e)
        {
            if (!MainClass.UserPermission)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (btnPermission.Checked == false)
            {
                //btn setting
                btnAddUser.Checked = false;
                btnprofile.Checked = false;
                btnGsetting1.Checked = false;
                btnLogout.Checked = false;
                btnPath.Checked = false;
                btnPermission.Checked = true;
                btnRecovery.Checked = false;
                btnSaveBackup.Checked = false;
                btnStaff.Checked = false;
                UpdateSubTitelLable("صلاحيات المستخدمين");

                mainPanel.Visible = true;

                AddControlsFull(new frmUserPermissions());

            }

        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            if (MainClass.StaffShow)
            {
                if (btnStaff.Checked == false)
                {
                    btnAddUser.Checked = false;
                    btnprofile.Checked = false;
                    btnGsetting1.Checked = false;
                    btnLogout.Checked = false;
                    btnPath.Checked = false;
                    btnPermission.Checked = false;
                    btnRecovery.Checked = false;
                    btnSaveBackup.Checked = false;
                    btnStaff.Checked = true;

                    mainPanel.Visible = true;

                    UpdateSubTitelLable("اعدادات طاقم العمل");
                    AddControlsFull(new frmStaff(this));

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
        private void btnSaveBackup_Click(object sender, EventArgs e)
        {

            if (MainClass.SaveBackup)
            {
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;

                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

                if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    string folderPath = folderBrowserDialog1.SelectedPath;

                    // تحذير: لا يُفضل استخدام قرص C
                    if (folderPath.StartsWith("C:", StringComparison.OrdinalIgnoreCase))
                    {
                        messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                        messageBox.Show("يُفضل اختيار مسار خارج القرص C لأمان أكثر.");
                        return;
                    }
                    frmShowBackup frmShowBackup = new frmShowBackup(folderPath);
                    frmShowBackup.ShowDialog(this);

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

        private void btnRecovery_Click(object sender, EventArgs e)
        {
            frmRestoreBackup frmRestoreBackup = new frmRestoreBackup();
            if (MainClass.BackupPath)
            {
                frmRestoreBackup.ShowDialog(this);
            }
            else
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
            }

        }

        private void btnPath_Click(object sender, EventArgs e)
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


        private void btnLogout_Click(object sender, EventArgs e)
        {
            frmMessageBox frmMessage = new frmMessageBox("تنبية", "هل تريد تسجيل خروج المستخدم الحالي ؟", "W");
            if (frmMessage.ShowDialog() == DialogResult.OK)
            {
                frmParaint.Hide();
                this.Hide();

                frmLogin frmLogin = new frmLogin();
                frmLogin.ShowDialog();
                this.Close();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void LightMode()
        {
            backgroundPrmary = Color.FromArgb(243, 243, 243);
            backgroundseconder = Color.FromArgb(230, 230, 230);
            textColor = Color.FromArgb(51, 51, 51);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.FromArgb(250, 250, 20);

            btnStaff.Image = Properties.Resources.staff_dark;
            btnSaveBackup.Image = Properties.Resources.save_backup_darck;
            btnRecovery.Image = Properties.Resources.recovery_backup_darck;
            btnPermission.Image = Properties.Resources.permission_dark;
            btnPath.Image = Properties.Resources.folder_darck;
            btnLogout.Image = Properties.Resources.logout_darck;
            btnGsetting1.Image = Properties.Resources.setting_dark;
            btnprofile.Image = Properties.Resources.change_password_dark;
            btnAddUser.Image = Properties.Resources.add_user_dark;


        }
        private void DarkMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.FromArgb(2, 2, 2);
            borderColor = checkedFillColor;

            btnAddUser.Image = Properties.Resources.add_user_light;
            btnprofile.Image = Properties.Resources.profile_white;
            btnGsetting1.Image = Properties.Resources.setting_light;
            btnLogout.Image = Properties.Resources.logout_light;
            btnPath.Image = Properties.Resources.folder_light;
            btnPermission.Image = Properties.Resources.permission_light;
            btnRecovery.Image = Properties.Resources.recovery_backup_light;
            btnSaveBackup.Image = Properties.Resources.save_backup_light;
            btnStaff.Image = Properties.Resources.staff_light;
            btnClose.Image = Properties.Resources.close_light2;
            btnMinimum.Image = Properties.Resources.minimumm_light2;

        }
        private void ThemeMode()
        {
            this.BackColor = backgroundPrmary;
            controlPanel.BackColor = backgroundPrmary;
            topPanel.BackColor = backgroundPrmary;
            userImgPanel.FillColor = backgroundPrmary;
            userImgPanel.ShadowColor = backgroundPrmary;
            lblUserName.ForeColor = textColor;
            lblTitel.ForeColor = textColor;
            SLine.FillColor = Color.Gray;
            SLine.BackColor = backgroundPrmary;

            lblSubTittel.ForeColor = textColor;
            subTiltePanel.BackColor = backgroundPrmary;

            btnAddUser.FillColor = backgroundPrmary;
            btnAddUser.ForeColor = textColor;
            btnAddUser.CheckedState.FillColor = checkedFillColor;
            //btnAddUser.CheckedState.ForeColor = checkedForColor;

            btnprofile.FillColor = backgroundPrmary;
            btnprofile.ForeColor = textColor;
            btnprofile.CheckedState.FillColor = checkedFillColor;
            //btnChangePass.CheckedState.ForeColor = checkedForColor;

            btnGsetting1.FillColor = backgroundPrmary;
            btnGsetting1.ForeColor = textColor;
            btnGsetting1.CheckedState.FillColor = checkedFillColor;
            //btnGsetting1.CheckedState.ForeColor = checkedForColor;

            btnLogout.FillColor = backgroundPrmary;
            btnLogout.ForeColor = textColor;
            btnLogout.CheckedState.FillColor = checkedFillColor;
            //btnLogout.CheckedState.ForeColor = checkedForColor;

            btnPath.FillColor = backgroundPrmary;
            btnPath.ForeColor = textColor;
            btnPath.CheckedState.FillColor = checkedFillColor;
            // btnPath.CheckedState.ForeColor = checkedForColor;

            btnPermission.FillColor = backgroundPrmary;
            btnPermission.ForeColor = textColor;
            btnPermission.CheckedState.FillColor = checkedFillColor;
            //btnPermission.CheckedState.ForeColor = checkedForColor;

            btnRecovery.FillColor = backgroundPrmary;
            btnRecovery.ForeColor = textColor;
            btnRecovery.CheckedState.FillColor = checkedFillColor;
            //btnRecovery.CheckedState.ForeColor = checkedForColor;

            btnSaveBackup.FillColor = backgroundPrmary;
            btnSaveBackup.ForeColor = textColor;
            btnSaveBackup.CheckedState.FillColor = checkedFillColor;
            //btnSaveBackup.CheckedState.ForeColor = checkedForColor;

            btnStaff.FillColor = backgroundPrmary;
            btnStaff.ForeColor = textColor;
            btnStaff.CheckedState.FillColor = checkedFillColor;
            //btnStaff.CheckedState.ForeColor = checkedForColor;

            btnClose.FillColor = backgroundPrmary;
            btnClose.HoverState.FillColor = backgroundPrmary;

            btnMinimum.FillColor = backgroundPrmary;
            btnMinimum.HoverState.FillColor = backgroundPrmary;

        }
        public void themRefresh()
        {
            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();

            frmParaint.themRefresh();

            if (openedForms.ContainsKey("frmNewUser"))
            {
                var form = openedForms["frmNewUser"] as frmNewUser;
                form?.ThemRefresh();
            }

            if (openedForms.ContainsKey("frmProfile"))
            {
                var form = openedForms["frmProfile"] as frmProfile;
                form?.ThemRefresh();
            }
            if (openedForms.ContainsKey("frmStaff"))
            {
                var form = openedForms["frmStaff"] as frmStaff;
                form?.ThemeMode();
            }
            if (openedForms.ContainsKey("frmAddPersone"))
            {
                var form = openedForms["frmAddPersone"] as frmAddPersone;
                form?.ThemRefresh();
            }
            if (openedForms.ContainsKey("frmUserPermissions"))
            {
                var form = openedForms["frmUserPermissions"] as frmUserPermissions;
                form?.ThemRefresh();
            }
        }
        // Add Form To panel
        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();

        public void AddControlsFull(Form f)
        {
            // التحقق مما إذا كان النموذج موجوداً بالفعل في المجموعة
            if (openedForms.ContainsKey(f.Name))
            {
                var existingForm = openedForms[f.Name];

                if (existingForm.IsDisposed)
                {
                    existingForm = CreateNewFormInstance(f.Name);
                    openedForms[f.Name] = existingForm;
                }

                mainPanel.Controls.Clear();
                mainPanel.Size = new Size(747, 657);
                mainPanel.Location = new Point(12, 60);

                existingForm.Dock = DockStyle.Fill;
                existingForm.TopLevel = false;
                mainPanel.Controls.Add(existingForm);
                existingForm.Show();
                existingForm.BringToFront();
            }
            else
            {
                mainPanel.Controls.Clear();

                f.Dock = DockStyle.Fill;
                f.TopLevel = false;
                mainPanel.Controls.Add(f);
                f.Show();

                openedForms.Add(f.Name, f);
            }
        }

        //-> task paramelter to open new form 
        public void AddControlsSenter(Form f, bool task)
        {
            Form formToShow;

            if (task)
            {
                // إذا الفورم موجود مسبقًا نحذفه
                if (openedForms.ContainsKey(f.Name))
                {
                    var existingForm = openedForms[f.Name];

                    if (!existingForm.IsDisposed)
                        existingForm.Close();  // إغلاق النموذج القديم

                    openedForms.Remove(f.Name);
                }

                // إضافة النموذج الجديد
                formToShow = f;
                openedForms.Add(f.Name, f);
            }
            else
            {
                if (openedForms.ContainsKey(f.Name))
                {
                    formToShow = openedForms[f.Name];

                    if (formToShow.IsDisposed)
                    {
                        formToShow = CreateNewFormInstance(f.Name);
                        openedForms[f.Name] = formToShow;
                    }
                }
                else
                {
                    formToShow = f;
                    openedForms.Add(f.Name, f);
                }
            }

            // ضبط وإظهار النموذج داخل mainPanel
            mainPanel.Controls.Clear();
            mainPanel.Size = new Size(747, 657);
            mainPanel.Location = new Point(12, 60);

            formToShow.Dock = DockStyle.None;
            formToShow.TopLevel = false;

            // توسيط النموذج أفقياً
            int centerX = (mainPanel.Width - formToShow.Width) / 2;
            formToShow.Location = new Point(centerX, formToShow.Location.Y);

            mainPanel.Controls.Add(formToShow);
            formToShow.Show();
            formToShow.BringToFront();

        }

        private Form CreateNewFormInstance(string formName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

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

        private void btnMinimum_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnGsetting1_Click(object sender, EventArgs e)
        {
            UpdateSubTitelLable("الاعدادات العامة");
            AddControlsSenter(new frmGeneralSettings(this), false);

            btnAddUser.Checked = false;
            btnprofile.Checked = false;
            btnGsetting1.Checked = true;
            btnLogout.Checked = false;
            btnPath.Checked = false;
            btnPermission.Checked = false;
            btnRecovery.Checked = false;
            btnSaveBackup.Checked = false;
            btnStaff.Checked = false;



        }
        public void addPersone(int id, bool task)
        {
            UpdateSubTitelLable("اضافة موظف جديد");
            AddControlsSenter(new frmAddPersone(this, id), task);
        }
        public void frmStaffBack()
        {
            UpdateSubTitelLable("اعدادات طاقم العمل");
            AddControlsFull(new frmStaff(this));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (!MainClass.CanResetSystem)
            {
                messageBox.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                messageBox.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                messageBox.Parent = (Form)this.TopLevelControl;
                messageBox.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            else
            {
                using (frmBlackout frmblackout = new frmBlackout(this))
                {
                    frmShowResetProsses frm = new frmShowResetProsses();
                    frm.ShowDialog(this);
                }

            }

        }

        private void mainPanel_SizeChanged(object sender, EventArgs e)
        {
            CenterLabelInPanel(subTiltePanel, lblSubTittel);
        }
        private static void CenterLabelInPanel(Panel panel, Label label)
        {
            if (panel == null || label == null) return;

            // حساب موقع منتصف البانل
            int x = (panel.Width - label.Width) / 2;
            int y = (panel.Height - label.Height) / 2;

            label.Location = new Point(x, y);
        }
    }
}
