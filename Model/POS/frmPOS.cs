using Guna.UI2.WinForms;
using pos.Classes;
using pos.GeneralForms;
using pos.GeneralForms.MainForm;
using pos.Model.Maintenance;
using pos.Model.POS;
using pos.Model.Stor;
using pos.SystemApp;
using pos.UserControls;
using pos.View;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static pos.GeneralForms.frmMian2;


namespace pos.Model
{
    public partial class frmPOS : Form, IRefreshableForm
    {
        private Color backgroundPrmary;
        private Color backgroundseconder;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForColor;
        private Color borderColor;

        public int MainID = 0;
        public int DetailID = 0;
        public string orderType = string.Empty;
        public int driverID = 0;
        private bool isTaskbill = false;
        private int receivedNumber;
        private int receivedNumberprice;
        private int taskID = 0;

        int currentPage = -1;
        bool isLoading = false;
        bool allLoaded = false;
        private string display;
        private bool fromReturnsBill = false;
        private bool isReturn = false;
        private FlowLayoutPanel currentFlowPanelProduct;
        private string Name = "";
        private int partiesID = 0;
        private bool isRetuned = false;
        [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        private System.Windows.Forms.Timer scrollTimer = new System.Windows.Forms.Timer();

        private Queue<ucProduct2> recycledProducts = new Queue<ucProduct2>();

        frmMian2 frmParaint;

        public frmPOS(frmMian2 frm)
        {

            InitializeComponent();
            this.frmParaint = frm;

            loadInitial();


        }
        public frmPOS()
        {

            InitializeComponent();

            loadInitial();


        }
        private void loadInitial()
        {
            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            ThemeMode();
            txtSugesterCat();
            this.ShowInTaskbar = false;

            string qry = @"SELECT pName FROM products";

            // ✅ الاتصال الجديد
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd);

                con.Open(); // افتح الاتصال قبل التنفيذ
                da2.Fill(dt2);

                AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    dataSource.Add(dt2.Rows[i][0].ToString());
                }

                this.txtSearch.AutoCompleteCustomSource = dataSource;
                this.txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
                this.InputLanguageChanged += new InputLanguageChangedEventHandler(MyForm_InputLanguageChanged);
            }



            timer1.Interval = 500;
            timer1.Start();
        }

        public void OnFormShownAgain()
        {

        }
        private void txtSugesterCat()
        {
            string qry = @"SELECT catName  FROM v_CategoryArabicSorted ";

            // ✅ الاتصال الجديد
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd);

                con.Open(); // افتح الاتصال قبل التنفيذ
                da2.Fill(dt2);

                AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    dataSource.Add(dt2.Rows[i][0].ToString());
                }

                this.txtCatSearch.AutoCompleteCustomSource = dataSource;
                this.txtCatSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtCatSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }
        public void ReloadData()
        {

            if (MainClass.ThemeMode == "dark")
                DarkMode();
            else if (MainClass.ThemeMode == "light")
                LightMode();

            //ThemeMode();

            if (currentFlowPanelProduct != null)
            {
                foreach (Control c in currentFlowPanelProduct.Controls)
                {
                    if (c is ucProduct2 item)
                    {
                        item.themRefresh();
                    }
                }
            }

        }
        private async void frmPOS_Load(object sender, EventArgs e)
        {
            if (MainID > 0)
            {
                ReloadInvoiceToPOS(MainID);
            }
            invoiceCode = GenerateUniqueInvoiceCode();

            MainID = await CreateInvoiceAsync(invoiceCode);


            int rowIndexNew = dgvMain.Rows.Add();
            dgvMain.CurrentCell = dgvMain.Rows[rowIndexNew].Cells["dgv2Name"];
            dgvMain.BeginEdit(true);

            tsMode.Visible = true;

            ApplyGridStyle(dgvMain);

            classicPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            classicPanel.Height = 247;

            classicPanel.Location = new Point(0, (this.ClientSize.Height - classicPanel.Height) - 50);

            classicPanel.Width = this.ClientSize.Width;
            setFlowPanelPro();



            await Task.Run(() =>
            {
                GetData();
                setting();
                sellCheckState();
            });

            AddCategory(txtCatSearch.Text);
            RecycleAllProducts();

            await LoadNextPageAsync(18, string.Empty, string.Empty);
            scrollTimer.Interval = 20;
            scrollTimer.Tick += ScrollTimer_Tick;
            scrollTimer.Start();
        }
        private string DisplayMode()
        {
            // نخلي الملف في نفس مسار البرنامج
            string configFilePath = Path.Combine(Application.StartupPath, "Settings.config");

            // لو الملف مش موجود، ممكن نعمله إنشاء افتراضي
            if (!File.Exists(configFilePath))
            {
                // إنشاء ملف افتراضي لو مش موجود
                var defaultConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                    <configuration>
                      <appSettings>
                        <add key=""DisplayModePos"" value=""dgv"" />
                      </appSettings>
                    </configuration>";
                File.WriteAllText(configFilePath, defaultConfig);
            }

            string displaymode;

            try
            {
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
                var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;

                if (settings["DisplayModePos"] != null &&
                    !string.IsNullOrWhiteSpace(settings["DisplayModePos"].Value) &&
                    settings["DisplayModePos"].Value.ToLower() != "null")
                {
                    displaymode = settings["DisplayModePos"].Value;
                }
                else
                {
                    displaymode = "dgv";
                }
            }
            catch
            {
                displaymode = "dgv";
            }

            return displaymode;
        }

        private void MyForm_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {

            if (InputLanguage.CurrentInputLanguage.Culture.TwoLetterISOLanguageName == "ar")
            {
                txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            }
            else
            {
                txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            }
        }

        public static void SuspendDrawing(Control target)
        {
            if (target != null && !target.IsDisposed)
                SendMessage(target.Handle, WM_SETREDRAW, 0, 0);
        }

        public static void ResumeDrawing(Control target, bool redraw)
        {
            if (target != null && !target.IsDisposed)
            {
                SendMessage(target.Handle, WM_SETREDRAW, 1, 0);

                if (redraw)
                    target.Refresh();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }





        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            if (currentFlowPanelProduct == null)
                return; // لو مش جاهز، اخرج من الدالة

            int currentValue = currentFlowPanelProduct.VerticalScroll.Value;

            int coutProRow = CalculateItemsPerRow();
            if (!isLoading && !allLoaded &&
                currentValue + currentFlowPanelProduct.ClientSize.Height >= currentFlowPanelProduct.VerticalScroll.Maximum - 10)
            {
                _ = LoadNextPageAsync(coutProRow, selectedCategoryName, SearchName);
            }
        }


        private void ClipControlRegion(Control control, string direction, int cutSize)
        {
            Rectangle rect = control.ClientRectangle;

            switch (direction.ToLower())
            {
                case "top":
                    rect.Y += cutSize;
                    rect.Height -= cutSize;
                    break;
                case "bottom":
                    rect.Height -= cutSize;
                    break;
                case "left":
                    rect.X += cutSize;
                    rect.Width -= cutSize;
                    break;
                case "right":
                    rect.Width -= cutSize;
                    break;
                default:
                    throw new ArgumentException("Direction must be: top, bottom, left, or right.");
            }

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);
            control.Region = new Region(path);
        }
        private void setting()
        {
            string appBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(Directory.GetCurrentDirectory(), "Settings.config") };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            if (settings["ucProdact_X"] != null && settings["ucProdact_Y"] != null)
            {
                int sX = int.TryParse(settings["panel1Size_X"].Value, out sX) ? sX : 0;
                int lX = int.TryParse(settings["panel1Location_X"].Value, out lX) ? lX : 0;
                int lY = int.TryParse(settings["panel1Location_Y"].Value, out lY) ? lY : 0;



                int csX = int.TryParse(settings["cPanel1Size_X"].Value, out csX) ? csX : 0;

                if (csX > 0)
                {
                    currentFlowPanelProduct.Left += csX;
                    currentFlowPanelProduct.Width -= csX;
                }
                else
                {
                    int positiveNumber = Math.Abs(csX);
                    currentFlowPanelProduct.Left -= positiveNumber;
                    currentFlowPanelProduct.Width += positiveNumber;
                }
            }
        }
        private void GetData()
        {
            try
            {
                guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
                guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                //AddCategory();

                if (MainID != 0)
                {
                    string qry2 = @"SELECT m.MainID, p.pName, d.qty, d.price, d.amount
                            FROM tblMain1 m 
                            INNER JOIN tblDetails d ON m.MainID = d.MainID 
                            INNER JOIN products p ON p.pID = d.proID
                            WHERE m.MainID = @ID ";

                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd2 = new SqlCommand(qry2, con))
                    {
                        cmd2.Parameters.AddWithValue("@ID", MainID);
                        cmd2.CommandType = CommandType.Text;

                        DataTable dt2 = new DataTable();
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);

                        con.Open();
                        da2.Fill(dt2);

                        ListBox lb = new ListBox();

                        try
                        {
                            for (int i = 0; i < lb.Items.Count; i++)
                            {
                                string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
                return;
            }
        }


        private string GenerateUniqueInvoiceCode()
        {
            const string digits = "0123456789";
            Random random = new Random();
            string code;
            bool exists;

            do
            {
                // 👈 مثال: 8 أرقام عشوائية
                code = new string(Enumerable.Repeat(digits, 14)
                                 .Select(s => s[random.Next(s.Length)]).ToArray());

                // ✅ التأكد من أنه غير موجود مسبقاً في قاعدة البيانات
                using (SqlConnection con = MainClass.GetConnection()) // ✅ بدل new SqlConnection
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tblMain1 WHERE InvoiceCode = @code", con))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        exists = (int)cmd.ExecuteScalar() > 0;
                    }
                }


            } while (exists);

            return code + "55";
        }




        public int ReceivedNumber
        {
            get { return receivedNumber; }
        }
        public int ReceivedNumberprice
        {
            get { return receivedNumberprice; }
        }

        private void AddCategory(string catName)
        {
            try
            {
                // امسح أي أزرار موجودة قبل ما تضيف الجديد
                CategoryPanel.Controls.Clear();

                // زر "الكل"
                Guna.UI2.WinForms.Guna2Button allCategoriesButton = new Guna.UI2.WinForms.Guna2Button();
                allCategoriesButton.Size = new Size(160, 45);
                allCategoriesButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                allCategoriesButton.Text = "الكل";
                allCategoriesButton.CheckedState.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
                allCategoriesButton.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                allCategoriesButton.AutoRoundedCorners = false;
                allCategoriesButton.BorderRadius = 8;
                allCategoriesButton.BorderThickness = 1;

                if (MainClass.ThemeMode == "dark")
                {
                    allCategoriesButton.CheckedState.FillColor = Color.FromArgb(1, 95, 95);
                    allCategoriesButton.CheckedState.BorderColor = Color.FromArgb(136, 214, 218);
                    allCategoriesButton.ForeColor = Color.FromArgb(51, 51, 51);
                    allCategoriesButton.FillColor = Color.FromArgb(136, 214, 218);
                    allCategoriesButton.CheckedState.ForeColor = Color.White;
                    allCategoriesButton.BorderColor = Color.FromArgb(1, 95, 95);
                }
                else if (MainClass.ThemeMode == "light")
                {
                    allCategoriesButton.FillColor = Color.FromArgb(1, 95, 95);
                    allCategoriesButton.ForeColor = Color.White;
                    allCategoriesButton.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
                    allCategoriesButton.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
                    allCategoriesButton.CheckedState.ForeColor = Color.FromArgb(51, 51, 51);
                    allCategoriesButton.BorderColor = Color.Gray;
                }

                allCategoriesButton.Checked = true;
                allCategoriesButton.AutoRoundedCorners = true;
                allCategoriesButton.Click += new EventHandler(b_clik);

                // فاصل بعد زر الكل
                Guna2Separator separator = new Guna2Separator();
                separator.Size = new Size(160, 12);
                separator.FillThickness = 3;

                CategoryPanel.Controls.Add(allCategoriesButton);
                CategoryPanel.Controls.Add(separator);

                // جلب الأصناف من قاعدة البيانات
                string qry = "SELECT catID, catName FROM v_CategoryArabicSorted WHERE catName LIKE '%' + @catName + '%' ORDER BY SortOrder, catName;";
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@catName", catName);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                                    b.FillColor = textColor;
                                    b.Size = new Size(160, 35);
                                    b.AutoRoundedCorners = true;
                                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                                    b.Text = row["catName"].ToString();
                                    b.BorderThickness = 1;
                                    b.CheckedState.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
                                    b.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                                    b.AutoRoundedCorners = false;
                                    b.BorderRadius = 8;

                                    if (MainClass.ThemeMode == "dark")
                                    {
                                        b.CheckedState.FillColor = Color.FromArgb(1, 95, 95);
                                        b.CheckedState.BorderColor = Color.FromArgb(136, 214, 218);
                                        b.ForeColor = Color.FromArgb(51, 51, 51);
                                        b.FillColor = Color.FromArgb(136, 214, 218);
                                        b.CheckedState.ForeColor = Color.White;
                                        b.BorderColor = Color.FromArgb(1, 95, 95);
                                    }
                                    else if (MainClass.ThemeMode == "light")
                                    {
                                        b.FillColor = Color.FromArgb(1, 95, 95);
                                        b.ForeColor = Color.White;
                                        b.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
                                        b.CheckedState.ForeColor = Color.FromArgb(51, 51, 51);
                                        b.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
                                        b.BorderColor = Color.Gray;
                                    }

                                    b.Click += new EventHandler(b_clik);

                                    CategoryPanel.Controls.Add(b);
                                }
                            }
                        }
                    }
                }


            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ أثناء تحميل الأصناف");
                return;
            }
        }


        private System.Windows.Forms.Timer picHideTimer = new System.Windows.Forms.Timer();
        // 1. حقل لتخزين الفئة الحالية
        private string lastSelectedCategory = string.Empty;
        private string selectedCategoryName;

        private async void b_clik(object? sender, EventArgs e)
        {
            var b = (Guna.UI2.WinForms.Guna2Button)sender;
            txtSearch.Text = string.Empty;

            selectedCategoryName = (b.Text == "الكل") ? string.Empty : b.Text;

            // إذا ضغط على نفس الفئة، لا نفعل شيئاً
            if (selectedCategoryName == lastSelectedCategory)
                return;

            // حدّث الفئة الأخيرة
            lastSelectedCategory = selectedCategoryName;

            setFlowPanelPro();

            currentPage = 0;
            firstNumber = 0;
            isFirstTime = true;
            allLoaded = false;

            currentFlowPanelProduct.Controls.Clear();
            currentFlowPanelProduct.Refresh();

            // تحميل أول دفعة
            await LoadNextPageAsync(18, selectedCategoryName, string.Empty);

        }


        private int number = 0;
        private void AddItems(string id, string proID, string name, string cat, string qty, string qtyUse, string price, Image pimage, string pshortFall, string pCode, string pCodeUse, string place)
        {
            ucProduct2 w;
            if (recycledProducts.Count > 0)
            {
                // إعادة استخدام عنصر قديم
                w = recycledProducts.Dequeue();
                w.Size = new Size(230, 200);
                w.Visible = true;
            }
            else
            {
                // إنشاء عنصر جديد
                w = new ucProduct2();
                w.Size = new Size(230, 200);

                // ربط الأحداث (مرّة واحدة عند الإنشاء)
                w.onSelect += W_onSelect;
                w.onSelect2 += W_onSelect2;
                w.onEdite += W_onEdite;
                w.onAbout += W_onAbout;
                w.showImg += W_showImag;

                w.Size = new Size(230, 200);
            }

            // تحديث بيانات العنصر المعاد استخدامه أو الجديد
            w.PName = name;
            w.pprice = price;
            w.PCategory = cat;
            w.PImage = pimage;
            w.pQty = qty;
            w.id = Convert.ToInt32(proID);
            w.pshortFall = pshortFall;
            w.barCode = pCode;
            w.barCodeUse = pCodeUse;
            w.pPlace = place;

            //if(qty == "0")
            //{
            //    w.onSelect -= W_onSelect;
            //    w.onSelect2 -= W_onSelect2;
            //}
            if (qtyUse == "0")
            {
                w.btnUse.Visible = false;
            }

            // إضافة للوحة إذا غير موجود بالفعل
            if (!currentFlowPanelProduct.Controls.Contains(w))
                currentFlowPanelProduct.Controls.Add(w);
        }



        private void setFlowPanelPro()
        {
            if (currentFlowPanelProduct != null)
            {
                viewPanel.Controls.Remove(currentFlowPanelProduct);
                currentFlowPanelProduct.Dispose();
                currentFlowPanelProduct = null;
            }

            currentFlowPanelProduct = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight, // ✅ خلي الاتجاه عادي
                Padding = Padding.Empty
            };

            viewPanel.Controls.Add(currentFlowPanelProduct);

            const int itemWidth = 230;   // عرض الكارت
            const int itemMargin = 5;   // مسافة ثابتة بين الكروت

            currentFlowPanelProduct.Resize += (s, e) =>
            {
                AdjustPanelPadding(currentFlowPanelProduct, itemWidth, itemMargin);
            };

            currentFlowPanelProduct.ControlAdded += (s, ev) =>
            {
                if (ev.Control is ucProduct2 card)
                    card.Margin = new Padding(itemMargin / 2, 2, itemMargin / 2, 2);

                AdjustPanelPadding(currentFlowPanelProduct, itemWidth, itemMargin);
            };
        }

        private void AdjustPanelPadding(FlowLayoutPanel panel, int itemWidth, int itemMargin)
        {
            int panelWidth = panel.ClientSize.Width;
            if (panel.Controls.Count == 0) return;

            // 🔹 كام عنصر في الصف
            int itemsPerRow = Math.Max(1, (panelWidth + itemMargin) / (itemWidth + itemMargin));

            // 🔹 حساب العرض الإجمالي
            int totalRowWidth = itemsPerRow * itemWidth + (itemsPerRow - 1) * itemMargin;

            // 🔹 المساحة الفاضية للتوسيط
            int leftover = Math.Max(0, panelWidth - totalRowWidth);
            int sidePadding = leftover / 2;

            // 🔹 ضبط البادينج
            panel.Padding = new Padding(sidePadding, 5, sidePadding, 5);

            // 🔹 ضبط المارجن للكروت
            foreach (Control c in panel.Controls)
            {
                if (c is ucProduct2 card)
                    card.Margin = new Padding(itemMargin / 2, 5, itemMargin / 2, 5);
            }
        }







        // إزالة العناصر غير المستخدمة (مثلاً لما تغير الفلتر أو تعيد تحميل)
        private void RecycleAllProducts()
        {
            foreach (ucProduct2 ctrl in currentFlowPanelProduct.Controls.OfType<ucProduct2>())
            {
                ctrl.Visible = false;
                recycledProducts.Enqueue(ctrl);
            }
            currentFlowPanelProduct.Controls.Clear();
        }


        private void W_onSelect(object ss, EventArgs ee)
        {
            var wdg = (ucProduct2)ss;
            AddProductToPurchase(wdg, false);

        }

        private void W_onSelect2(object ss, EventArgs ee)
        {
            var wdg = (ucProduct2)ss;
            AddProductToPurchase(wdg, true);

        }

        private void AddProductToPurchase(ucProduct2 wdg, bool isUsed)
        {
            int rowIndex;

            // لو الـ DataGridView فاضي → نضيف أول صف
            if (dgvMain.Rows.Count == 0)
                rowIndex = dgvMain.Rows.Add();
            else
                rowIndex = dgvMain.Rows.Count - 1; // آخر صف

            // حط الباركود الجديد أو المستعمل
            dgvMain.Rows[rowIndex].Cells["dgv2Name"].Value = isUsed ? wdg.barCodeUse : wdg.barCode;

            // نفّذ نفس اللوجيك اللي بيشتغل مع Enter
            ProcessEnter(rowIndex, "dgv2Name");
        }


        // دالة عامة لتشغيل لوجيك الـ Enter
        private void ProcessEnter(int rowIndex, string columnName)
        {
            // لو عندك لوجيك في CellEndEdit أو KeyDown مع Enter نفّذه هنا
            var cell = dgvMain.Rows[rowIndex].Cells[columnName];

            // مثال: نرفع حدث CellEndEdit يدويًا
            guna2DataGridView3_CellEndEdit(dgvMain, new DataGridViewCellEventArgs(cell.ColumnIndex, rowIndex));
        }


        private void W_onEdite(object ss, EventArgs ee)
        {
            if (MainClass.ProCardEdite)
            {
                frmBlackout frmBlackout1 = new frmBlackout(this);
                frmBlackout1.Show();
                frmBlackout1.Owner = this;
                frmCategoryCard frm = new frmCategoryCard();
                frm.Owner = this;
                frm.id = ((ucProduct2)ss).id;
                frm.ShowDialog();
                this.Focus();
                frmBlackout1.Close();
            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
            }
        }

        private void W_onAbout(object ss, EventArgs ee)
        {
            var wdg = (ucProduct2)ss;
            double qty = 0;
            double qtyU = 0;

            string qry = @"SELECT qtyU1, qtyUsedU1 FROM totalStor WHERE pID = @ID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@ID", wdg.id);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    con.Open();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        qty = Convert.ToDouble(dt.Rows[0]["qtyU1"]);
                        qtyU = Convert.ToDouble(dt.Rows[0]["qtyUsedU1"]);
                    }
                }
            }

            wdg.pQty = qty.ToString();
            wdg.pQtyUse = qtyU.ToString();
        }

        private void W_showImag(object? sender, EventArgs e)
        {
            var wdg = (ucProduct2)sender;

            Image img = wdg.OriginalImage;
            if (img != null)
            {
                frmShowProductImg frm = new frmShowProductImg(img);
                frm.ShowDialog();
            }
        }
        private async void frmCountAdd_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        static int firstNumber = 0;
        static bool isFirstTime = true;
        private string SearchName;
        private List<DataRow> GetProductPage(int page, int proNum, string selectedCategory, string searchName)
        {
            if (page == -1)
                page = 0;
            int nextpro = page * proNum + firstNumber;
            string qry = @"
                        SELECT 
                            MIN(p.pID) AS pID, 
                            p.pName, 
                            p.pNewBarode,
                            p.pUsedBarode,
                            MIN(p.shorcut) AS shorcut,
                            MIN(p.pCode) AS pCode, 
                            MIN(p.categoryID) AS categoryID, 
                            MIN(c.catName) AS catName, 
                            MIN(p.sellPrice) AS sellPrice, 
                            MAX(CONVERT(VARBINARY(MAX), p.pImage)) AS pImage, 
                            ts.qtyU1 AS TotalQty, 
                            ts.qtyUsedU1 AS TotalUseQty, 
                            MIN(p.requestP) AS requestP 
                        FROM 
                            products p 
                        JOIN 
                            category c ON p.categoryID = c.catID 
                        JOIN 
                            totalStor ts ON ts.pID = p.pID 
                        WHERE
                            (@catName    IS NULL OR c.catName  LIKE '%' + @catName   + '%')
                        AND (@searchName IS NULL OR p.pName    LIKE '%' + @searchName + '%')
                        GROUP BY 
                            p.pName, ts.qtyU1, ts.qtyUsedU1, p.pNewBarode, p.pUsedBarode
                        HAVING 
                            ts.qtyU1 > 0 OR ts.qtyUsedU1 > 0
                        ORDER BY 
                            MIN(p.pID)
                        OFFSET @nextpro ROWS FETCH NEXT @proNum ROWS ONLY;";

            DataTable dt = new DataTable();

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    if (string.IsNullOrWhiteSpace(selectedCategory))
                        cmd.Parameters.AddWithValue("@catName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@catName", selectedCategory);

                    if (string.IsNullOrWhiteSpace(searchName))
                        cmd.Parameters.AddWithValue("@searchName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@searchName", searchName);

                    cmd.Parameters.AddWithValue("@nextpro", nextpro);
                    cmd.Parameters.AddWithValue("@proNum", proNum);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }


            if (isFirstTime)
            {
                firstNumber = proNum;
                isFirstTime = false;
            }

            return dt.Rows.Cast<DataRow>().ToList();
        }

        private async Task LoadNextPageAsync(int proNum, string selectedCategory, string searchName)
        {
            if (isLoading || allLoaded) return;

            isLoading = true;
            Debug.WriteLine($"Loading page: {currentPage}, Items per page: {proNum}");

            var rows = await Task.Run(() => GetProductPage(currentPage, proNum, selectedCategory, searchName));

            if (rows.Count == 0)
            {
                allLoaded = true;
                isLoading = false;
                Debug.WriteLine("All products loaded.");
                return;
            }

            currentFlowPanelProduct.SuspendLayout();
            currentFlowPanelProduct.PerformLayout();
            currentFlowPanelProduct.Invalidate();

            foreach (var item in rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                Image img = Image.FromStream(new MemoryStream(imagearray));

                AddItems("0",
                         item["pID"].ToString(),
                         item["pName"].ToString(),
                         item["catName"].ToString(),
                         item["TotalQty"].ToString(),
                         item["TotalUseQty"].ToString(),
                         item["sellPrice"].ToString(),
                         img,
                         item["requestP"].ToString(),
                         item["pNewBarode"].ToString(),
                         item["pUsedBarode"].ToString(),
                         item["shorcut"].ToString());
            }

            currentFlowPanelProduct.ResumeLayout();

            currentPage++;
            isLoading = false;
        }

        private async void btnNew_Click(object sender, EventArgs e)
        {

            dgvMain.Rows.Clear();
            txtClean.Text = string.Empty;
            txtDV.Text = string.Empty;
            txtDP.Text = string.Empty;
            txtPriceTotal.Text = string.Empty;

            MainID = 0;
            invoiceCode = GenerateUniqueInvoiceCode();
            MainID = await CreateInvoiceAsync(invoiceCode);

            int rowIndexNew2 = dgvMain.Rows.Add();
            dgvMain.CurrentCell = dgvMain.Rows[rowIndexNew2].Cells["dgv2Name"];
            dgvMain.BeginEdit(true);

        }

        public int id = 0;
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = this;

            frmBillList frm = new frmBillList(this);
            frm.Owner = this;
            frm.ShowDialog();

            if (frm.MainID > 0)
            {
                id = frm.MainID;
                MainID = frm.MainID;
            }
            this.Focus();
            frmBlackout.Close();
        }

        private async void btnCheckout_Click(object sender, EventArgs e)
        {
            bool isOk = false;
            // === تنفيذ الفاتورة ===
            if (MainID <= 0)
            {
                invoiceCode = GenerateUniqueInvoiceCode();
                MainID = await CreateInvoiceAsync(invoiceCode);
            }
            else
            {
                await UpdateInvoiceAsync(MainID, "underwork");
            }

            await SyncInvoiceDetailsAsync();

            using (frmBlackout frmBlackout = new frmBlackout(this))
            {
                frmBlackout.Show();

                using (frmPayWays frm = new frmPayWays())
                {
                    frm.mainID = MainID;
                    frm.partyType = "عميل";

                    decimal.TryParse(txtClean.Text.Replace('٫', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal cleanValue);
                    decimal.TryParse(txtPriceTotal.Text.Replace('٫', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal totalValue);
                    decimal.TryParse(txtDV.Text.Replace('٫', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal discountValue);

                    frm.totalClean = cleanValue;
                    frm.total = totalValue;
                    frm.discountValue = discountValue;
                    frm.invoiceCode = invoiceCode;

                    frm.status = isRetuned ? "update" : "new";

                    frm.Owner = this;
                    DialogResult result = frm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        qtyStore();
                        if (fromReturnsBill) SaveInvoiceAndUpdateStock();

                        Notifier.ShowNotification("تم الحفظ", "تم حفظ الفاتورة ,والمنتجات بنجاح ✅");
                        dgvMain.Rows.Clear();
                        txtPriceTotal.Text = "0";
                        txtClean.Text = "0";
                        txtDP.Text = "0";
                        txtDV.Text = "0";

                        invoiceCode = GenerateUniqueInvoiceCode();
                        MainID = await CreateInvoiceAsync(invoiceCode);
                        fromReturnsBill = false;

                        frmShowBackup frmshowBackup = new frmShowBackup();
                        frmshowBackup.backupType = "DIFFERENTIAL";
                        frmshowBackup.showNotification = false;
                        frmshowBackup.ShowDialog(this);
                    }
                }
            }
            if (isOk)
            {

            }
            this.Focus();

            // === النسخ الاحتياطي مع تقدير ProgressBar حسب حجم الملف ===
            isRetuned = false;
        }


        string invoiceCode;
        private async void btnHold_Click(object sender, EventArgs e)
        {
            using (frmBlackout frmBlackout = new frmBlackout(this))
            {
                frmBlackout.Show();

                if (MainID == 0)
                {
                    MainID = await CreateInvoiceAsync(invoiceCode);
                }
                else
                {
                    await UpdateInvoiceAsync(MainID, "pending");
                }
                await SyncInvoiceDetailsAsync();

                using (frmBillName frm = new frmBillName(MainID))
                {


                    frm.Owner = this;
                    DialogResult result = frm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        dgvMain.Rows.Clear();
                        txtPriceTotal.Text = "0";
                        txtClean.Text = "0";
                        txtDP.Text = "0";
                        txtDV.Text = "0";

                        invoiceCode = GenerateUniqueInvoiceCode();
                        MainID = await CreateInvoiceAsync(invoiceCode);

                        fromReturnsBill = false;

                        int rowIndexNew2 = dgvMain.Rows.Add();
                        dgvMain.CurrentCell = dgvMain.Rows[rowIndexNew2].Cells["dgv2Name"];
                        dgvMain.BeginEdit(true);

                    }
                    else
                    {

                    }
                }
            }

            this.Focus();
        }
        // إنشاء فاتورة جديدة
        private async Task<int> CreateInvoiceAsync(string invoiceCode)
        {
            string qry = @"
            INSERT INTO tblMain1 (InvoiceCode, shiftID, aDate, aTime, status)
            OUTPUT INSERTED.MainID
            VALUES (@InvoiceCode, @shiftID, @aDate, @aTime, @status)";

            using (SqlConnection con = MainClass.GetConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@InvoiceCode", invoiceCode);
                    cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                    cmd.Parameters.AddWithValue("@aDate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@status", "underwork");

                    int newId = (int)await cmd.ExecuteScalarAsync();
                    return newId;
                }
            }
        }

        // تحديث فاتورة
        private async Task UpdateInvoiceAsync(int MainID, string statusBill)
        {
            string qry1 = @"
              UPDATE tblMain1
              SET partiesID = @partiesID,
                  taskID = @taskID,
                  shiftID = @shiftID,
                  aDate = @aDate,
                  aTime = @aTime,
                  status = @status,
                  total = @total,
                  received = @received,
                  change = @change,
                  descount = @descount,
                  descountValue = @descountValue,
                  priceClear = @priceClear,
                  TotalWithInterest = @TotalWithInterest,
                  InterestAmount = @InterestAmount,
                  PaidAmount = @PaidAmount,
                  CreditBalance = @CreditBalance,
                  PaymentMethod = @PaymentMethod,
                  InvoiceIssuanceValue = @InvoiceIssuanceValue
              WHERE MainID = @ID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry1, con))
            {
                cmd.Parameters.AddWithValue("@ID", MainID);
                cmd.Parameters.AddWithValue("@partiesID", partiesID);
                cmd.Parameters.AddWithValue("@taskID", taskID);
                cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                cmd.Parameters.AddWithValue("@aDate", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
                cmd.Parameters.AddWithValue("@status", statusBill);

                decimal total = decimal.Parse(txtPriceTotal.Text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                cmd.Parameters.AddWithValue("@total", total);

                cmd.Parameters.AddWithValue("@received", 0);
                cmd.Parameters.AddWithValue("@change", 0);
                cmd.Parameters.AddWithValue("@priceClear", Convert.ToDouble(txtClean.Text));
                cmd.Parameters.AddWithValue("@descount", Convert.ToDouble(string.IsNullOrEmpty(txtDP.Text) ? "0" : txtDP.Text));

                string raw2 = txtDV.Text?.Trim();
                double descountValue = 0;
                if (!string.IsNullOrWhiteSpace(raw2))
                {
                    raw2 = raw2.Replace('٫', '.'); // لو الأرقام عربية
                    double.TryParse(raw2, NumberStyles.Any, CultureInfo.InvariantCulture, out descountValue);
                }
                cmd.Parameters.AddWithValue("@descountValue", descountValue);

                cmd.Parameters.AddWithValue("@TotalWithInterest", 0);
                cmd.Parameters.AddWithValue("@InvoiceIssuanceValue", 0);
                cmd.Parameters.AddWithValue("@InterestAmount", 0);
                cmd.Parameters.AddWithValue("@PaidAmount", 0);
                cmd.Parameters.AddWithValue("@CreditBalance", 0);
                cmd.Parameters.AddWithValue("@PaymentMethod", DBNull.Value);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            } // ✅ الاتصال بيتقفل تلقائي هنا
        }



        private async Task SyncInvoiceDetailsAsync()
        {
            using (SqlConnection con = MainClass.GetConnection())
            {
                await con.OpenAsync();

                // 🔹 IDs الموجودة في قاعدة البيانات
                var existingIds = new List<int>();
                using (SqlCommand getCmd = new SqlCommand("SELECT detailID FROM tblDetails WHERE MainID=@MainID", con))
                {
                    getCmd.Parameters.AddWithValue("@MainID", MainID);
                    using (var reader = await getCmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                            existingIds.Add(reader.GetInt32(0));
                    }
                }

                var currentIds = new List<int>();

                foreach (DataGridViewRow row in dgvMain.Rows)
                {
                    if (row.IsNewRow) continue;

                    int proID = Convert.ToInt32(row.Cells["dgv2proID"].Value ?? 0);
                    if (row.Cells["dgv2Name"].Value == null || string.IsNullOrWhiteSpace(row.Cells["dgv2Name"].Value.ToString())
                        || row.Cells["dgv2TotalDes"].Value == null || string.IsNullOrWhiteSpace(row.Cells["dgv2TotalDes"].Value.ToString()))
                        continue;

                    bool isUsed = Convert.ToBoolean(row.Cells["dgv2IsUsed"].Value ?? true);

                    int detailID = Convert.ToInt32(row.Cells["dgvDetailsId"].Value ?? 0);
                    SqlCommand cmd;

                    if (detailID == 0) // INSERT
                    {
                        string insertQry = @"
                    INSERT INTO tblDetails 
                    (MainID, proID, proName, qty, price, priceUnDis, cleanPrice, amount, unite, pDescount, vDescount, priceAfterDes, isUsed, pBarcode, catName, uniteID) 
                    VALUES 
                    (@MainID, @proID, @proName, @qty, @price, @priceUnDis, @cleanPrice, @amount, @unite, @pDescount, @vDescount, @priceAfterDes, @isUsed, @pBarcode, @catName, @uniteID);
                    SELECT SCOPE_IDENTITY()";

                        cmd = new SqlCommand(insertQry, con);
                    }
                    else // UPDATE
                    {
                        string updateQry = @"
                    UPDATE tblDetails 
                    SET 
                        MainID=@MainID, 
                        proID=@proID, 
                        qty=@qty, 
                        price=@price,
                        priceUnDis=@priceUnDis, 
                        isUsed=@isUsed,
                        cleanPrice=@cleanPrice,
                        amount=@amount,
                        unite=@unite,
                        pDescount=@pDescount, 
                        vDescount=@vDescount,
                        priceAfterDes=@priceAfterDes,
                        catName=@catName,
                        uniteID=@uniteID
                    WHERE detailID=@detailID";

                        cmd = new SqlCommand(updateQry, con);
                        cmd.Parameters.AddWithValue("@detailID", detailID);
                        currentIds.Add(detailID);
                    }

                    // 🔹 باراميترات مشتركة
                    cmd.Parameters.AddWithValue("@MainID", MainID);
                    cmd.Parameters.AddWithValue("@proID", proID);
                    cmd.Parameters.AddWithValue("@pBarcode", row.Cells["dgv2Code"].Value ?? "");
                    cmd.Parameters.AddWithValue("@proName", row.Cells["dgv2Name"].Value ?? "");
                    cmd.Parameters.AddWithValue("@qty", Convert.ToDouble(row.Cells["dgv2Qty"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgv2UnitPrice"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@priceUnDis", Convert.ToDouble(row.Cells["dgv2UnitPriceDis"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@cleanPrice", Convert.ToDouble(row.Cells["dgv2TotalDes"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgv2Total"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@unite", row.Cells["dgv2Unite"].Value ?? "");
                    cmd.Parameters.AddWithValue("@uniteID", Convert.ToInt32(row.Cells["dgv2UniteID"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@catName", row.Cells["dgbCat"].Value ?? "");
                    cmd.Parameters.AddWithValue("@pDescount", Convert.ToDouble(row.Cells["dgv2Dp"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@vDescount", Convert.ToDouble(row.Cells["dgv2Dv"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@priceAfterDes", Convert.ToDouble(row.Cells["dgv2TotalDes"].Value ?? 0));
                    cmd.Parameters.AddWithValue("@isUsed", isUsed);

                    if (detailID == 0)
                    {
                        object newId = await cmd.ExecuteScalarAsync();
                        int insertedId = Convert.ToInt32(newId);
                        row.Cells["dgvDetailsId"].Value = insertedId;
                        currentIds.Add(insertedId);
                    }
                    else
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // 🔹 حذف أي ID مش موجود في DataGridView
                var toDelete = existingIds.Except(currentIds).ToList();
                foreach (int id in toDelete)
                {
                    using (SqlCommand delCmd = new SqlCommand("DELETE FROM tblDetails WHERE detailID=@id", con))
                    {
                        delCmd.Parameters.AddWithValue("@id", id);
                        await delCmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }


        private async void deteteRowDB(int deletedId)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    await con.OpenAsync();

                    if (deletedId > 0)
                    {
                        string delQry = "DELETE FROM tblDetails WHERE detailID=@detailID";
                        using (SqlCommand delCmd = new SqlCommand(delQry, con))
                        {
                            delCmd.Parameters.Add("@detailID", SqlDbType.Int).Value = deletedId;
                            int rowsAffected = await delCmd.ExecuteNonQueryAsync();

                            if (rowsAffected > 0)
                                Console.WriteLine("Row deleted successfully.");
                            else
                                Console.WriteLine("No row found with the given ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting row: {ex.Message}");
            }
        }

        public DataSet ds;

        
        private void btnCloseThis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = this;
            frmSettingPOS frmSettingPOS = new frmSettingPOS();
            frmSettingPOS.Owner = this;
            DialogResult result = frmSettingPOS.ShowDialog();
            if (result == DialogResult.OK)
            {
                GetData();
                setting();
            }

            frmBlackout.Close();
        }

        private async void تحديثToolStripMenuItem_Click(object sender, EventArgs e)
        {
            display = DisplayMode();

            currentPage = 0;
            firstNumber = 0;
            allLoaded = false;
            isLoading = false;

            if (display == "dgv")
            {
                panelSize = classicPanel.Size;
                panelLocation = classicPanel.Location;
                classicPanel.Dock = DockStyle.Fill;
                viewPanel.Visible = false;
                tsMode.Visible = false;

            }
            else
            {
                txtSearch.Visible = true;
                viewPanel.Visible = true;
                tsMode.Visible = true;

                classicPanel.Size = panelSize;
                classicPanel.Location = panelLocation;
                classicPanel.Dock = DockStyle.None;
                viewPanel.Visible = true;

                classicPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                classicPanel.Height = 247;

                classicPanel.Location = new Point(0, (this.ClientSize.Height - classicPanel.Height) - 50);

                classicPanel.Width = this.ClientSize.Width;

                setFlowPanelPro();

            }


            await Task.Run(() =>
            {
                GetData();
                setting();
                sellCheckState();
            });

            AddCategory(txtCatSearch.Text);
            RecycleAllProducts();

            await LoadNextPageAsync(18, string.Empty, string.Empty);


        }

        private void اضافةمنتججديدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainClass.ProCardAdd)
            {

                frmCategoryCard frm = new frmCategoryCard();
                frm.Owner = this;
                frm.ShowDialog();
                this.Focus();
            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        private void guna2DataGridView3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dgvMain.Controls.OfType<VScrollBar>().Any(s => s.Visible))
            {
                ClipControlRegion(dgvMain, "left", 17);
            }

            if (dgvMain.Rows.Count > 0)
            {
                if (dgvMain.IsHandleCreated)
                {
                    dgvMain.BeginInvoke(new MethodInvoker(() =>
                    {
                        int columnIndex = dgvMain.Columns["dgv2Name"].Index;
                        dgvMain.CurrentCell = dgvMain.Rows[dgvMain.Rows.Count - 1].Cells[columnIndex];
                        dgvMain.BeginEdit(true);
                    }));
                }
                else
                {
                    // إنشاء الـ Handle بالقوة
                    var handle = dgvMain.Handle; // هذا يضمن إنشاء الـ Handle
                    dgvMain.BeginInvoke(new MethodInvoker(() =>
                    {
                        int columnIndex = dgvMain.Columns["dgv2Name"].Index;
                        dgvMain.CurrentCell = dgvMain.Rows[dgvMain.Rows.Count - 1].Cells[columnIndex];
                        dgvMain.BeginEdit(true);
                    }));
                }
            }
            bool hasValidRow = dgvMain.Rows
                             .Cast<DataGridViewRow>()
                             .Any(r => r.Cells["dgv2Name"].Value != null &&
                                       !string.IsNullOrWhiteSpace(r.Cells["dgv2Name"].Value.ToString()) &&
                                       !r.IsNewRow); // تجاهل الصف الفاضي الأخير الافتراضي

            if (hasValidRow)
            {
                btnCheckout.Enabled = true;
                btnEnd.Enabled = true;
                btnPrint.Enabled = true;
                txtDP.Enabled = true;
                txtDV.Enabled = true;
            }
            else
            {
                btnCheckout.Enabled = false;
                btnEnd.Enabled = false;
                btnPrint.Enabled = false;
                txtDP.Enabled = false;
                txtDV.Enabled = false;
            }


        }

        private bool sortAscending = true;
        private void guna2DataGridView3_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SortDataGridView(dgvMain.Columns[e.ColumnIndex].Name, sortAscending);
            sortAscending = !sortAscending;
        }
        private void SortDataGridView(string columnName, bool ascending)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>(dgvMain.Rows.Cast<DataGridViewRow>());

            rows = rows.Where(r => !r.IsNewRow).ToList();

            if (ascending)
            {
                rows = rows.OrderBy(r => int.TryParse(r.Cells[columnName].Value?.ToString(), out int tempVal) ? tempVal : int.MinValue).ToList();
            }
            else
            {
                rows = rows.OrderByDescending(r => int.TryParse(r.Cells[columnName].Value?.ToString(), out int tempVal) ? tempVal : int.MaxValue).ToList();
            }

            dgvMain.Rows.Clear();
            foreach (var row in rows)
            {
                dgvMain.Rows.Add(row);
            }
        }

        private void ProcessEnter(DataGridView dgv, int rowIndex, string columnName)
        {
            var cell = dgv.Rows[rowIndex].Cells[columnName];
            // If you want to reuse logic without recursion, factor logic into a helper.
            // For now, calling the same handler is fine:
            guna2DataGridView3_CellEndEdit(dgv, new DataGridViewCellEventArgs(cell.ColumnIndex, rowIndex));
        }
        public void UpdateRow(DataGridViewRow row, double discount)
        {
            // ✅ تحقق من صلاحية الصف
            if (row == null || row.IsNewRow || row.Cells["dgv2proID"].Value == null)
                return;

            // 🔹 دالة مساعدة لتحويل القيم بأمان
            double SafeParse(object value)
            {
                return (value != null && double.TryParse(value.ToString(), out double result)) ? result : 0;
            }

            double price = SafeParse(row.Cells["dgv2UnitPrice"].Value);
            double qty = SafeParse(row.Cells["dgv2Qty"].Value);

            // 🔹 أقل سعر مسموح
            double lowestPrice = SafeParse(row.Cells["dgv2lowestPriceRounded"].Value);
            if (lowestPrice == 0)
            {
                lowestPrice = SafeParse(row.Cells["dgv2PurPrice"].Value);
                row.Cells["dgv2lowestPriceRounded"].Value = lowestPrice.ToString("F0", CultureInfo.InvariantCulture);
            }

            // 🔹 حساب سعر الوحدة بعد الخصم
            double discountedUnitPrice = price * (1 - discount / 100.0);

            // 🔹 تصحيح نسبة الخصم إذا السعر أقل من الحد الأدنى
            if (discountedUnitPrice < lowestPrice && price > 0)
            {
                discount = (1 - (lowestPrice / price)) * 100;
                discountedUnitPrice = lowestPrice;
            }

            // 🔹 الحسابات
            double totalPrice = price * qty;
            double discountedTotal = discountedUnitPrice * qty;
            double discountValue = totalPrice - discountedTotal;

            // 🔹 تقريب الأرقام
            discountedUnitPrice = discountedUnitPrice;
            discountedTotal = discountedTotal;
            discountValue = discountValue;

            // 🔹 تحديث الأعمدة
            row.Cells["dgv2UnitPriceDis"].Value = discountedUnitPrice.ToString("F1", CultureInfo.InvariantCulture);
            row.Cells["dgv2TotalDes"].Value = discountedTotal.ToString("F1", CultureInfo.InvariantCulture);
            row.Cells["dgv2Dp"].Value = discount.ToString("F2", CultureInfo.InvariantCulture);
            row.Cells["dgv2Dv"].Value = discountValue.ToString("F1", CultureInfo.InvariantCulture);
        }

        public void UpdateRowWithDiscountValue(DataGridViewRow row, double totalDiscountValue)
        {
            // ✅ تحقق من صلاحية الصف
            if (row == null || row.IsNewRow || row.Cells["dgv2proID"].Value == null)
                return;

            double SafeParse(object value)
            {
                return (value != null && double.TryParse(value.ToString(), out double result)) ? result : 0;
            }

            double price = SafeParse(row.Cells["dgv2UnitPrice"].Value);
            double qty = SafeParse(row.Cells["dgv2Qty"].Value);

            if (qty == 0 || price == 0)
                return;

            // 🔹 أقل سعر مسموح
            double lowestPrice = SafeParse(row.Cells["dgv2lowestPriceRounded"].Value);
            if (lowestPrice == 0)
            {
                lowestPrice = SafeParse(row.Cells["dgv2PurPrice"].Value);
                row.Cells["dgv2lowestPriceRounded"].Value = lowestPrice.ToString("F0", CultureInfo.InvariantCulture);
            }

            // 🔹 إجمالي السعر قبل الخصم
            double totalPrice = price * qty;

            // 🔹 حساب الخصم كنسبة
            double discountPercentage = (totalDiscountValue / totalPrice) * 100;

            // 🔹 إعادة استخدام دالة الخصم بالنسبة
            UpdateRow(row, discountPercentage);
        }

        private async void guna2DataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            var dgv = sender as DataGridView;
            if (dgv == null) return;
            if (isReturn) return;

            string columnName = dgv.Columns[e.ColumnIndex].Name;
            var currentCell = dgv[e.ColumnIndex, e.RowIndex];
            string cellValue = currentCell.Value?.ToString() ?? "";

            // لو الخلية فاضية يوقف
            if (string.IsNullOrWhiteSpace(cellValue)) return;

            string searchColumn = null;

            if (columnName == "dgv2Name")
            {
                rowIndex = e.RowIndex;
                dgv.Rows[rowIndex].Cells["dgv2Code"].Value = cellValue;

                ProcessEnter(dgv, rowIndex, "dgv2Code");
                return; // 🔧 important: avoid running the rest of the handler
            }
            else if (columnName == "dgv2Code")
            {
                var productData = GetProductByAnyCode(cellValue);

                if (productData != null && productData.Rows.Count > 0)
                {
                    UnitsFunction(productData.Rows[0]); // تحديث الوحدات

                    double balance = GetBalance(productData.Rows[0]["pID"].ToString(), qtyColumn);
                    string barcode = await UpdateOrAddRow(dgv, productData, e.RowIndex, balance, lowPrice, currentWholesale, currentSemiWholesale, currentSellPrice, currentHiDes);

                    // ✅ تحديث dgv2Code بالكود الفعلي من قاعدة البيانات
                    dgv.Rows[e.RowIndex].Cells["dgv2Code"].Value = barcode;


                    productInfo = productData;
                    rowIndex = e.RowIndex;
                }
            }
            else if (columnName == "dgv2Dp")
            {
                DataGridViewRow currentRow = dgvMain.Rows[e.RowIndex];
                double discount = 0;

                if (!double.TryParse(currentRow.Cells["dgv2Dp"].Value?.ToString(), out discount))
                    discount = 0;

                UpdateRow(currentRow, discount);
            }
            else if (columnName == "dgv2Dv")
            {
                DataGridViewRow currentRow = dgvMain.Rows[e.RowIndex];
                double discount = 0;

                if (!double.TryParse(currentRow.Cells["dgv2Dv"].Value?.ToString(), out discount))
                    discount = 0;

                UpdateRowWithDiscountValue(currentRow, discount);
            }
            else if (columnName == "dgv2UnitPrice")
            {
                await SyncInvoiceDetailsAsync();
                DataGridViewRow row = dgvMain.Rows[e.RowIndex];

                // دالة مساعدة لتحويل القيم بأمان
                double SafeParse(object value)
                {
                    double result;
                    if (value == null || !double.TryParse(value.ToString(), out result))
                        return 0;
                    return result;
                }

                double price = SafeParse(row.Cells["dgv2UnitPrice"].Value);

                // أقل سعر مسموح
                double lowestPrice = SafeParse(row.Cells["dgv2lowestPriceRounded"].Value);
                if (lowestPrice == 0)
                {
                    lowestPrice = SafeParse(row.Cells["dgv2PurPrice"].Value);
                    row.Cells["dgv2lowestPriceRounded"].Value = lowestPrice.ToString("F1", CultureInfo.InvariantCulture);
                }


                // إذا سعر الوحدة بعد الخصم أقل من الحد الأدنى
                if (price < lowestPrice)
                {
                    row.Cells["dgv2UnitPrice"].Value = lowestPrice;
                    row.Cells["dgv2UnitPriceDis"].Value = lowestPrice;

                }
                else
                    row.Cells["dgv2UnitPriceDis"].Value = price;


            }
            else if (columnName == "dgv2Status")
            {
                DataGridViewRow row = dgvMain.Rows[e.RowIndex];
                string status = Convert.ToString(row.Cells["dgv2Status"].Value);
                if (status == "مستعمل")
                    row.Cells["dgv2IsUsed"].Value = true;
                else
                    row.Cells["dgv2IsUsed"].Value = false;
            }

            if (searchColumn != null)
            {
                var productData = GetProductBy(searchColumn, cellValue);

                if (productData != null)
                {
                    double balance = GetBalance(productData.Rows[0]["pID"].ToString(), qtyColumn);
                    string barcode = await UpdateOrAddRow(dgv, productData, e.RowIndex, balance, lowPrice, currentWholesale, currentSemiWholesale, currentSellPrice, currentHiDes);
                    productInfo = productData;

                    // ✅ تحديث dgv2Code بالكود الفعلي من قاعدة البيانات
                    dgv.Rows[e.RowIndex].Cells["dgv2Code"].Value = barcode;
                    rowIndex = e.RowIndex;
                }
            }
            else if (dgv != null && dgv.Columns[e.ColumnIndex].Name == "dgv2Qty")
            {

            }

            CheckAndDeleteEmptyNameRow(dgv, e.RowIndex);
        }


        private DataTable GetProductByAnyCode(string code)
        {
            DataTable dt = null;
            isUsed = false;

            // أولًا البحث في pNewBarode
            dt = GetProductBy("pNewBarode", code);
            if (dt != null && dt.Rows.Count > 0)
            {
                isUsed = false;
                return dt;
            }

            // لو مفيش → البحث في pUsedBarode
            dt = GetProductBy("pUsedBarode", code);
            if (dt != null && dt.Rows.Count > 0)
            {
                isUsed = true;
                return dt;
            }

            // لو لسه مفيش → البحث في الأعمدة barcode1…barcode5
            int pid = GetProductIDByBarcodeColumns(code);
            dt = GetProductBy("p.pID", pid.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                // لو المنتج وجد هنا ممكن نعتبره مستعمل أو حسب منطقك
                isUsed = false; // أو true حسب ما تحب
                return dt;
            }

            // لو مفيش أي نتائج → ترجع فارغ
            return null;
        }

        // دالة بحث في جدول internationalBarcode في الأعمدة barcode1…barcode5
        private int GetProductIDByBarcodeColumns(string barcode)
        {
            string qry = @"
             SELECT TOP 1 pID 
             FROM internationalBarcode
             WHERE barcode1 = @barcode
                OR barcode2 = @barcode
                OR barcode3 = @barcode
                OR barcode4 = @barcode
                OR barcode5 = @barcode";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@barcode", barcode);

                con.Open();
                object result = cmd.ExecuteScalar(); // ترجع أول قيمة pID أو null

                if (result != null && int.TryParse(result.ToString(), out int pID))
                {
                    return pID;
                }
                else
                {
                    return 0; // لم يتم العثور
                }
            }
        }



        private DataTable GetProductBy(string column, string value)
        {
            string qry = $@"
             SELECT * 
             FROM products p
             INNER JOIN category c ON c.catID = p.categoryID
             INNER JOIN untits u ON p.idUniteDef = u.uID
             INNER JOIN totalStor ts ON ts.pID = p.pID
             WHERE {column} = @value";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.Add("@value", SqlDbType.NVarChar).Value = value;

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                return dt.Rows.Count > 0 ? dt : null;
            }
        }



        // دالة تجيب الرصيد من المخزن
        private double GetBalance(string pID, string qtyColumn)
        {
            string qry = $"SELECT {qtyColumn} AS TotalQty FROM totalStor WHERE pID = @pID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@pID", Convert.ToInt32(pID));

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                if (dt.Rows.Count > 0 && dt.Rows[0]["TotalQty"] != DBNull.Value)
                    return Convert.ToDouble(dt.Rows[0]["TotalQty"]);

                return 0;
            }
        }


        bool enter = false;
        int detailID = 0;
        private DataTable productInfo;
        private int rowIndex;
        private bool newrow = true;


        private async Task<string> UpdateOrAddRow(DataGridView dgv, DataTable productInfo, int rowIndex, double balance, double lowprice, double currentWholesale,
            double currentSemiWholesale, double currentSellPrice, double hiDes)
        {
            invoiceCode = GenerateUniqueInvoiceCode();

            if (MainID == 0)
                MainID = await CreateInvoiceAsync(invoiceCode);// add New Bill

            int pID = Convert.ToInt32(productInfo.Rows[0]["pID"]);
            DataTable dt = GetProductInfo(pID);

            if (dt.Rows.Count == 0)
            {

                Notifier.ShowNotification("الكمية", "الكمية الموجودة غير كافية ❌");
                return "";
            }

            string newName = productInfo.Rows[0]["pName"].ToString();
            string newCode;
            if (isUsed)
            {
                newCode = productInfo.Rows[0]["pUsedBarode"].ToString();
                double qtyUse = Convert.ToDouble(dt.Rows[0]["qtyUsedU1"].ToString());
                if (qtyUse <= 0)
                {
                    Notifier.ShowNotification("الكمية", "الكمية المستعملة غير كافية ❌");
                    return "";
                }
            }
            else
            {
                newCode = productInfo.Rows[0]["pNewBarode"].ToString();
                double qtyNew = Convert.ToDouble(dt.Rows[0]["qtyU1"].ToString());
                if (qtyNew <= 0)
                {
                    Notifier.ShowNotification("الكمية", "الكمية الجديدة غير كافية ❌");
                    return "";
                }
            }

            // ✅ تحقق لو المنتج موجود بالفعل في الـ Grid
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string oldName = row.Cells["dgv2Name"].Value?.ToString();
                string oldCode = row.Cells["dgv2Code"].Value?.ToString();

                if (oldName == newName &&
                    oldCode == newCode)
                {
                    // ✅ تحديث الصف الحالي
                    double discount = Convert.ToDouble(txtDP.Text == string.Empty ? "0" : txtDP.Text);
                    UpdateExistingRow(row, 1, discount);

                    // ✅ روح للصف الجديد اللي تحت
                    int newRowIndex = dgv.Rows.Count - 1; // آخر صف هو الصف الجديد

                    this.BeginInvoke(new Action(() =>
                    {
                        if (dgv.Rows.Count > 0)
                        {
                            dgv.CurrentCell = dgv.Rows[newRowIndex].Cells["dgv2Name"];

                            // ✅ افتح الخلية في وضع الكتابة
                            dgv.BeginEdit(true);

                            if (dgv.EditingControl is TextBox tb)
                            {
                                tb.Clear(); // مسح النص تلقائياً
                            }
                        }
                    }));


                    return newCode;
                }
            }


            // ✅ لو مش موجود → ضيفه
            int storId = 0; // تقدر تجيبها من الاستعلام لو محتاج

            isReturn = true;
            AddNewRow(dgv, productInfo.Rows[0], rowIndex, balance, storId, newCode, lowprice, currentWholesale, currentSemiWholesale, currentSellPrice, hiDes);
            isReturn = false;

            int rowIndexNew2 = dgv.Rows.Add();
            dgv.CurrentCell = dgv.Rows[rowIndexNew2].Cells["dgv2Name"];
            dgv.BeginEdit(true);

            return newCode;
        }


        private DataTable GetProductInfo(int pID)
        {
            string qry = @"
             SELECT p.pName, p.sellPrice, ts.*
             FROM totalStor ts
             INNER JOIN products p ON ts.pID = p.pID
             WHERE ts.pID = @pID AND (ts.qtyU1 > 0 OR ts.qtyUsedU1 > 0)";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@pID", pID);

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                return dt;
            }
        }



        private async void AddNewRow(DataGridView dgv, DataRow productRow, int rowIndex, double balance, int storId, string barcode,
            double lowprice, double currentWholesale, double currentSemiWholesale, double currentSellPrice, double hiDes)
        {
            // لو الصف المستهدف غير صالح → نضيف صف جديد
            if (rowIndex < 0 || rowIndex >= dgv.Rows.Count || dgv.Rows[rowIndex].IsNewRow)
            {
                rowIndex = dgv.Rows.Add();
            }

            if (dgv.Columns.Contains("dgv2Status"))
            {
                dgv["dgv2Status", rowIndex].Value = isUsed ? "مستعمل" : "جديد";
                dgv["dgv2Status", rowIndex].ReadOnly = true; // 🔒 جعل الخلية للعرض فقط
            }
            dgv["dgv2Name", rowIndex].ReadOnly = true; // 🔒 جعل الخلية للعرض فقط

            dgv["dgv2IsUsed", rowIndex].Value = isUsed;
            dgv["dgv2Code", rowIndex].Value = barcode;
            dgv["dgv2proID", rowIndex].Value = productRow["pID"];
            dgv["dgv2Name", rowIndex].Value = productRow["pName"];
            dgv["dgv2Shortcut", rowIndex].Value = productRow["compName"];

            dgv["dgv2Unite", rowIndex].Value = productRow["uName"];
            dgv["dgv2UnitPrice", rowIndex].Value = currentUnitePrice;
            dgv["dgv2UnitPriceDis", rowIndex].Value = currentUnitePrice;
            dgv["dgv2Dp", rowIndex].Value = 0;
            dgv["dgbCat", rowIndex].Value = productRow["catName"];
            dgv["dgv2CatID", rowIndex].Value = productRow["categoryID"];
            dgv["dgv2Qty", rowIndex].Value = 1;
            dgv["dgv2Total", rowIndex].Value = currentUnitePrice * 1;
            dgv["dgv2TotalDes", rowIndex].Value = currentUnitePrice * 1;
            dgv["dgv2Balance", rowIndex].Value = balance - 1;
            dgv["dgv2StockQty", rowIndex].Value = balance;
            dgv["dgvStorID", rowIndex].Value = storId;
            dgv["dgv2UniteID", rowIndex].Value = productRow["idUniteDef"];
            dgv["dgv2lowestPriceRounded", rowIndex].Value = lowprice;
            dgv["dgv2PurPrice", rowIndex].Value = currentPurchese;
            dgv["dgv2Hpd", rowIndex].Value = hiDes;
            dgv["dgvWholesale", rowIndex].Value = currentWholesale;
            dgv["dgvsemiWholesale", rowIndex].Value = currentSemiWholesale;

            await SyncInvoiceDetailsAsync(); // Add or update Rows to DataBase


        }

        private int numberU2;
        private int numberU3;
        private int idUniteDef;
        private int idUnite1;
        private int idUnite2;
        private int idUnite3;
        private bool isUsed;
        private double qty;

        private int currentUinte;
        private int currentNumber;
        private double currentUnitePrice;
        private double currentPurchese;
        private double currentUnitePurchese;
        private string qtyColumn;
        private double lowPrice;

        private double currentSellPrice;       // سعر البيع الحالي
        private double currentWholesale;       // سعر الجملة الحالي
        private double currentSemiWholesale;   // سعر نص الجملة الحالي
        private double currentLowPrice;
        private double currentHiDes;


        // 🔥 متغيرات جديدة للأسعار لكل وحدة
        private double priceU1, priceU2, priceU3;              // أسعار البيع لكل وحدة
        private double priceUsedU1, priceUsedU2, priceUsedU3;  // أسعار البيع المستعمل لكل وحدة
        private double purPriceU1, purPriceU2, purPriceU3;     // أسعار الشراء لكل وحدة
        private double purPriceUsedU1, purPriceUsedU2, purPriceUsedU3; // شراء مستعمل لكل وحدة
        private double wholesaleU1, wholesaleU2, wholesaleU3;  // أسعار الجملة لكل وحدة
        private double wholesaleUseU1, wholesaleUseU2, wholesaleUseU3; // جملة مستعمل
        private double semiWholesaleU1, semiWholesaleU2, semiWholesaleU3; // نص جملة
        private double semiWholesaleUseU1, semiWholesaleUseU2, semiWholesaleUseU3; // نص جملة مستعمل
        private double lowPriceU1, lowPriceU2, lowPriceU3;     // أقل سعر لكل وحدة
        private double lowPriceUseU1, lowPriceUseU2, lowPriceUseU3; // أقل سعر مستعمل
        private double hiDesU1, hiDesU2, hiDesU3; // أقل سعر مستعمل
        private double hiDesUseU1, hiDesUseU2, hiDesUseU3; // أقل سعر مستعمل


        private void UnitsFunction(DataRow productRow, int idUniteDefNow = 0)
        {
            // 🟢 جلب بيانات الوحدات
            if (idUniteDefNow == 0)
                idUniteDef = Convert.ToInt32(productRow["idUniteDef"]);
            else
                idUniteDef = idUniteDefNow;

            idUnite1 = Convert.ToInt32(productRow["idUnite1"]);
            idUnite2 = Convert.ToInt32(productRow["idUnite2"]);
            idUnite3 = Convert.ToInt32(productRow["idUnite3"]);

            numberU2 = Convert.ToInt32(productRow["numberU2"]);
            numberU3 = Convert.ToInt32(productRow["numberU3"]);

            // 🔥 جلب كل الأسعار مرة واحدة
            priceU1 = SafeToDouble(productRow["sellPrice"]);
            priceU2 = SafeToDouble(productRow["priceU2"]);
            priceU3 = SafeToDouble(productRow["priceU3"]);

            priceUsedU1 = SafeToDouble(productRow["sellPriceUsed"]);
            priceUsedU2 = SafeToDouble(productRow["priceU2Used"]);
            priceUsedU3 = SafeToDouble(productRow["priceU3Used"]);

            purPriceU1 = SafeToDouble(productRow["purPrice"]);
            purPriceU2 = SafeToDouble(productRow["purPriceUnit2"]);
            purPriceU3 = SafeToDouble(productRow["purPriceUnit3"]);

            purPriceUsedU1 = SafeToDouble(productRow["purUsedPrice"]);
            purPriceUsedU2 = SafeToDouble(productRow["purUsedPriceUnit2"]);
            purPriceUsedU3 = SafeToDouble(productRow["purUsedPriceUnit3"]);

            wholesaleU1 = SafeToDouble(productRow["wholesale"]);
            wholesaleU2 = SafeToDouble(productRow["wholesaleUnit2"]);
            wholesaleU3 = SafeToDouble(productRow["wholesaleUnit3"]);

            wholesaleUseU1 = SafeToDouble(productRow["wholesaleUse"]);
            wholesaleUseU2 = SafeToDouble(productRow["wholesaleUseUnit2"]);
            wholesaleUseU3 = SafeToDouble(productRow["wholesaleUseUnit3"]);

            semiWholesaleU1 = SafeToDouble(productRow["semiWholesale"]);
            semiWholesaleU2 = SafeToDouble(productRow["semiWholesaleUnit2"]);
            semiWholesaleU3 = SafeToDouble(productRow["semiWholesaleUnit3"]);

            semiWholesaleUseU1 = SafeToDouble(productRow["semiWholesaleUse"]);
            semiWholesaleUseU2 = SafeToDouble(productRow["semiWholesaleUseUnit2"]);
            semiWholesaleUseU3 = SafeToDouble(productRow["semiWholesaleUseUnit3"]);

            lowPriceU1 = SafeToDouble(productRow["lowestSellingPrice"]);
            lowPriceU2 = SafeToDouble(productRow["lowestSellingPriceUnit2"]);
            lowPriceU3 = SafeToDouble(productRow["lowestSellingPriceUnit3"]);

            lowPriceUseU1 = SafeToDouble(productRow["lowestSellingPriceUse"]);
            lowPriceUseU2 = SafeToDouble(productRow["lowestSellingPriceUseUnit2"]);
            lowPriceUseU3 = SafeToDouble(productRow["lowestSellingPriceUseUnit3"]);

            hiDesU1 = SafeToDouble(productRow["hDiscountPro"]);
            hiDesU2 = SafeToDouble(productRow["hDiscountProU2"]);
            hiDesU3 = SafeToDouble(productRow["hDiscountProU3"]);

            hiDesUseU1 = SafeToDouble(productRow["hDiscountProUse"]);
            hiDesUseU2 = SafeToDouble(productRow["hDiscountProUseU2"]);
            hiDesUseU3 = SafeToDouble(productRow["hDiscountProUseU3"]);

            // 🔥 تحديد الأسعار بناءً على الوحدة الافتراضية
            if (idUniteDef == idUnite1)
            {
                SetUnitData(idUnite1, 1, isUsed ? priceUsedU1 : priceU1, isUsed ? purPriceUsedU1 : purPriceU1,
                            isUsed ? productRow["qtyUsedU1"] : productRow["qtyU1"],
                            isUsed ? "qtyUsedU1" : "qtyU1", isUsed ? lowPriceUseU1 : lowPriceU1);
            }
            else if (idUniteDef == idUnite2)
            {
                SetUnitData(idUnite2, numberU2, isUsed ? priceUsedU2 : priceU2, isUsed ? purPriceUsedU2 : purPriceU2,
                            isUsed ? productRow["qtyUsedU2"] : productRow["qtyU2"],
                            isUsed ? "qtyUsedU2" : "qtyU2", isUsed ? lowPriceUseU2 : lowPriceU2);
            }
            else if (idUniteDef == idUnite3)
            {
                SetUnitData(idUnite3, numberU2 * numberU3, isUsed ? priceUsedU3 : priceU3, isUsed ? purPriceUsedU3 : purPriceU3,
                            isUsed ? productRow["qtyUsedU3"] : productRow["qtyU3"],
                            isUsed ? "qtyUsedU3" : "qtyU3", isUsed ? lowPriceUseU3 : lowPriceU3);
            }
            else
            {
                // Default للوحدة الأولى
                SetUnitData(idUnite1, 1, isUsed ? priceUsedU1 : priceU1, isUsed ? purPriceUsedU1 : purPriceU1,
                            isUsed ? productRow["qtyUsedU1"] : productRow["qtyU1"],
                            isUsed ? "qtyUsedU1" : "qtyU1", isUsed ? lowPriceUseU1 : lowPriceU1);
            }
        }

        // 🔥 دالة لتسهيل التعيين
        // 🔥 دالة لتسهيل التعيين
        private void SetUnitData(int unitId, int unitNumber, double sellPrice, double purchasePrice, object qtyVal, string qtyCol, double minPrice)
        {
            currentUinte = unitId;
            currentNumber = unitNumber;
            currentUnitePrice = sellPrice;
            currentPurchese = purchasePrice;
            qty = SafeToDouble2(qtyVal);
            qtyColumn = qtyCol;
            lowPrice = minPrice;
            currentUnitePurchese = purchasePrice; // تكلفة الوحدة

            // 🟢 تحديث المتغيرات الجديدة
            currentSellPrice = currentUnitePrice;
            currentLowPrice = minPrice;

            // 🔥 تحديد الجملة ونص الجملة والخصم الأعلى للوحدة المختارة
            if (unitId == idUnite1)
            {
                currentWholesale = isUsed ? wholesaleUseU1 : wholesaleU1;
                currentSemiWholesale = isUsed ? semiWholesaleUseU1 : semiWholesaleU1;
                currentHiDes = isUsed ? hiDesUseU1 : hiDesU1;
            }
            else if (unitId == idUnite2)
            {
                currentWholesale = isUsed ? wholesaleUseU2 : wholesaleU2;
                currentSemiWholesale = isUsed ? semiWholesaleUseU2 : semiWholesaleU2;
                currentHiDes = isUsed ? hiDesUseU2 : hiDesU2;
            }
            else if (unitId == idUnite3)
            {
                currentWholesale = isUsed ? wholesaleUseU3 : wholesaleU3;
                currentSemiWholesale = isUsed ? semiWholesaleUseU3 : semiWholesaleU3;
                currentHiDes = isUsed ? hiDesUseU3 : hiDesU3;
            }
        }


        // 🔥 دالة لتحويل آمن
        private double SafeToDouble2(object value)
        {
            double.TryParse(value?.ToString(), out double result);
            return result;
        }

        private double SafeToDouble(object value)
        {
            return value == DBNull.Value || value == null ? 0.0 : Convert.ToDouble(value);
        }

        private async void UpdateExistingRow(DataGridViewRow row, double addQty, double discountPercent)
        {
            // 👇 التعامل مع الخلايا الفاضية أو null
            double currentQty = 0;
            double price = 0;

            if (row.Cells["dgv2Qty"].Value != null &&
                !string.IsNullOrWhiteSpace(row.Cells["dgv2Qty"].Value.ToString()))
            {
                currentQty = Convert.ToDouble(row.Cells["dgv2Qty"].Value);
            }

            if (row.Cells["dgv2UnitPrice"].Value != null &&
                !string.IsNullOrWhiteSpace(row.Cells["dgv2UnitPrice"].Value.ToString()))
            {
                price = Convert.ToDouble(row.Cells["dgv2UnitPrice"].Value);
            }

            // ✅ تحديث الكمية في المخزن



            double qty = currentQty + addQty;
            double total = qty * price;



            double discountValue = (discountPercent / 100.0) * total;
            double finalPrice = total - discountValue;

            // ✅ تحديث قيم الفاتورة
            row.Cells["dgv2Qty"].Value = qty;
            row.Cells["dgv2Total"].Value = total.ToString("F2", CultureInfo.InvariantCulture);
            row.Cells["dgv2Dv"].Value = discountValue.ToString("F2", CultureInfo.InvariantCulture);
            row.Cells["dgv2TotalDes"].Value = finalPrice.ToString("F2", CultureInfo.InvariantCulture);

            // ✅ مزامنة مع قاعدة البيانات
            await SyncInvoiceDetailsAsync();
        }





        private void CheckAndDeleteEmptyNameRow(DataGridView dgv, int rowIndex)
        {
            if (dgv.Rows.Count > rowIndex && string.IsNullOrWhiteSpace(dgv.Rows[rowIndex].Cells["dgv2Name"].Value?.ToString()))
            {
                dgv.BeginInvoke(new System.Action(() =>
                {
                    dgv.Rows.RemoveAt(rowIndex);
                }));
            }

        }

        private void guna2DataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            GetTotal2();
            int count = 0;
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                bool isSelected = dgvMain.Rows[e.RowIndex].Selected;

                e.CellStyle.BackColor = isSelected ? checkedFillColor : backgroundPrmary;
                e.CellStyle.ForeColor = textColor;
            }

        }

        private void GetTotal2()
        {
            // دالة لتحويل القيم Double بأمان
            double SafeParse(object value)
            {
                if (value == null) return 0;
                double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result);
                return result;
            }

            // حساب txtPriceTotal زي الأول (من dgv2Total)
            double purchPrice = dgvMain.Rows
                .Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Sum(row => SafeParse(row.Cells["dgv2Total"].Value));

            txtPriceTotal.Text = purchPrice.ToString("N2");

            // حساب txtClean.Text من تجميع dgv2TotalDes
            double cleanTotal = dgvMain.Rows
                .Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow)
                .Sum(row => SafeParse(row.Cells["dgv2TotalDes"].Value));

            txtClean.Text = cleanTotal.ToString("N2");
        }





        private void txtPriceTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != '.' || txtPriceTotal.Text.Contains(".")))
            {
                e.Handled = true;
            }
        }

        private void dgvMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null || e.RowIndex < 0) return;

            var row = dgv.Rows[e.RowIndex];

            // دالة لتحديث السعر والخصم للصف
            void UpdateRowTotals(DataGridViewRow r)
            {
                if (!int.TryParse(r.Cells["dgv2Qty"].Value?.ToString(), out int qty)) qty = 1;
                if (!double.TryParse(r.Cells["dgv2UnitPrice"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double price)) price = 0;
                if (!double.TryParse(r.Cells["dgv2Dp"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double discountPercent)) discountPercent = 0;

                double total = qty * price;
                double discountValue = (discountPercent / 100.0) * total;
                double finalPrice = total - discountValue;

                r.Cells["dgv2Total"].Value = total.ToString("F1", CultureInfo.InvariantCulture);
                r.Cells["dgv2Dv"].Value = discountValue.ToString("F1", CultureInfo.InvariantCulture);
                r.Cells["dgv2TotalDes"].Value = finalPrice.ToString("F1", CultureInfo.InvariantCulture);
            }

            if (e.ColumnIndex == dgv.Columns["dgv2Qty"].Index ||
                e.ColumnIndex == dgv.Columns["dgv2UnitPrice"].Index ||
                e.ColumnIndex == dgv.Columns["dgv2Dp"].Index)
            {
                UpdateRowTotals(row);
                UpdateTotalsSummary(); // دالة لتحديث النصوص مثل txtPriceTotal و txtDV و txtClean
                UpdateBalance(row); // دالة للتحقق من الرصيد
            }

            if (e.ColumnIndex == dgv.Columns["dgv2Balance"].Index)
            {
                checkProductview();
            }
        }

        // مثال دالة لتحديث إجمالي الفاتورة
        private void UpdateTotalsSummary()
        {
            double totalPrice = 0;
            double totalPriceD = 0;
            double totalDiscount = 0;

            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (row.IsNewRow) continue;

                if (double.TryParse(row.Cells["dgv2Total"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double t))
                    totalPrice += t;

                if (double.TryParse(row.Cells["dgv2Dv"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                    totalDiscount += d;

                if (double.TryParse(row.Cells["dgv2TotalDes"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double td))
                    totalPriceD += td;
            }

            // 🔥 عرض النتائج مع تقريب عادي
            txtPriceTotal.Text = totalPrice.ToString("F1", CultureInfo.InvariantCulture);
            txtDV.Text = totalDiscount.ToString("F1", CultureInfo.InvariantCulture);
            txtClean.Text = totalPriceD.ToString("F1", CultureInfo.InvariantCulture);
        }

        // مثال دالة لتحديث الرصيد
        private void UpdateBalance(DataGridViewRow row)
        {
            int pID = 0;
            double stockQty = 0;
            if (row.Cells["dgv2proID"].Value != null &&
                 !string.IsNullOrWhiteSpace(row.Cells["dgv2proID"].Value.ToString()))
            {
                pID = Convert.ToInt32(row.Cells["dgv2proID"].Value);
            }

            if (row.Cells["dgv2StockQty"].Value != null &&
                !string.IsNullOrWhiteSpace(row.Cells["dgv2StockQty"].Value.ToString()))
            {
                stockQty = Convert.ToDouble(row.Cells["dgv2StockQty"].Value, CultureInfo.InvariantCulture);

                if (double.TryParse(row.Cells["dgv2Qty"].Value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double qty))
                {
                    if (qty > stockQty)
                    {
                        if (pID == 0)
                            return;
                        row.Cells["dgv2Qty"].Value = stockQty.ToString(CultureInfo.InvariantCulture);
                        row.Cells["dgv2Balance"].Value = "0";
                    }
                    else
                    {
                        double diff = stockQty - qty;

                        // 🔹 الشرط: لو الفرق بين 0 و 1 (يعني كسر عشري أصغر من الواحد)
                        if (diff > 0 && diff < 1)
                        {
                            //MessageBox.Show("الفرق أقل من واحد (كسر عشري)!");
                        }

                        row.Cells["dgv2Balance"].Value = diff.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        // Delete product if qty = 0
        private void checkProductview()
        {
            string query = @"
                SELECT p.pID AS Pid, ts.qtyU1 AS Qty, ts.qtyUsedU1 
                FROM products p 
                JOIN totalStor ts ON p.pID = ts.pID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                foreach (DataRow row in dt.Rows)
                {
                    int pID = Convert.ToInt32(row["Pid"]);
                    int qty = Convert.ToInt32(row["Qty"]);
                    int qtyUse = Convert.ToInt32(row["qtyUsedU1"]);

                    // 👇 هنا ممكن تخليها <= بدل == لو عايز تحدث لما المخزون يخلص أو ينقص
                    if (qty <= 0)
                    {
                        DecreaseProductQty(pID, qty, qtyUse);
                    }
                }
            }
        }

        private void DecreaseProductQty(int productId, int qtySold, int qtyUse)
        {
            // أولا، ابحث عن الـ UserControl الذي يحتوي على هذا المنتج بناءً على الـ productId
            foreach (var control in currentFlowPanelProduct.Controls)
            {
                if (control is ucProduct2 productControl)
                {
                    if (productControl.id == productId)
                    {
                        // إذا كانت الكمية صفر، احذف الـ UserControl من الـ FlowLayoutPanel
                        if (qtySold <= 0 && qtyUse <= 0)
                        {
                            currentFlowPanelProduct.Controls.Remove(productControl);
                        }
                        break;  // لا حاجة للبحث أكثر
                    }
                }
            }
        }
        private void guna2DataGridView3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;

        }

        private void guna2DataGridView3_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;

        }

        private double currentQty = 0;
        private async void guna2DataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                try
                {
                    // ✅ لو مفيش أي صفوف أصلاً → أضف أول صف
                    if (dgvMain.Rows.Count == 0)
                    {
                        int newIndex = dgvMain.Rows.Add();
                        dgvMain.CurrentCell = dgvMain.Rows[newIndex].Cells["dgv2Name"];
                        e.Handled = true;
                        return;
                    }

                    if (dgvMain.CurrentCell == null)
                        return;

                    // لو أنا في آخر صف
                    if (dgvMain.CurrentCell.RowIndex == dgvMain.Rows.Count - 1)
                    {
                        int currentRow = dgvMain.CurrentCell.RowIndex;

                        var nameCellValue = dgvMain.Rows[currentRow].Cells["dgv2Name"].Value?.ToString();
                        var qty = dgvMain.Rows[currentRow].Cells["dgv2Qty"].Value?.ToString();
                        var total = dgvMain.Rows[currentRow].Cells["dgv2TotalDes"].Value?.ToString();

                        if (!string.IsNullOrWhiteSpace(nameCellValue))
                        {
                            int newIndex = dgvMain.Rows.Add();
                            dgvMain.CurrentCell = dgvMain.Rows[newIndex].Cells["dgv2Name"];
                            e.Handled = true;
                        }
                        if (!string.IsNullOrWhiteSpace(nameCellValue) &&
                            !string.IsNullOrWhiteSpace(qty) &&
                            !string.IsNullOrWhiteSpace(total))
                        {
                            GetTotal2();
                            await SyncInvoiceDetailsAsync();
                        }
                    }
                }
                catch
                {
                    int newIndex = dgvMain.Rows.Add();
                    dgvMain.CurrentCell = dgvMain.Rows[newIndex].Cells["dgv2Name"];
                    e.Handled = true;
                }
                return;
            }

            if (dgvMain.SelectedRows.Count == 0) return;

            int pID = Convert.ToInt32(dgvMain.CurrentRow.Cells["dgv2proID"].Value);
            string cellValue = Convert.ToString(dgvMain.CurrentRow?.Cells["dgv2Code"].Value);

            string qry = @"
        SELECT p.purPrice,
               p.idUnite1,
               p.idUnite2,
               p.idUnite3,
               p.sellPrice,
               p.priceU2,
               p.priceU3,
               p.sellPriceUsed,
               p.priceU2Used,
               p.priceU3Used,
               p.purPrice,
               p.purUsedPrice,
               p.numberU2,
               p.numberU3,
               p.lowestSellingPrice,
               c.*,
               ts.*
        FROM products p 
        INNER JOIN category c ON c.catID = p.categoryID 
        INNER JOIN totalStor ts ON ts.pID = p.pID 
        WHERE p.pID = @pID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@pID", pID);
                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                if (dt.Rows.Count == 0) return;

                DataRow product = dt.Rows[0];
                string status1 = dgvMain.CurrentRow.Cells["dgv2Status"].Value.ToString();

                isUsed = (status1 == "مستعمل");

                if (e.KeyCode == Keys.F6)
                {
                    currentQty = Convert.ToDouble(product[isUsed ? "qtyUsedU1" : "qtyU1"] ?? 0);
                    var productData = GetProductByAnyCode(cellValue);
                    if (productData != null && productData.Rows.Count > 0)
                        UnitsFunction(productData.Rows[0], Convert.ToInt32(product["idUnite1"]));

                    UpdateRows(dgvMain.SelectedRows,
                               Convert.ToInt32(product["idUnite1"]),
                               currentSellPrice,
                               currentQty,
                               currentLowPrice);
                }
                else if (e.KeyCode == Keys.F5)
                {
                    currentQty = Convert.ToDouble(product[isUsed ? "qtyUsedU2" : "qtyU2"] ?? 0);
                    var productData = GetProductByAnyCode(cellValue);
                    if (productData != null && productData.Rows.Count > 0)
                        UnitsFunction(productData.Rows[0], Convert.ToInt32(product["idUnite2"]));

                    UpdateRows(dgvMain.SelectedRows,
                               Convert.ToInt32(product["idUnite2"]),
                               currentSellPrice,
                               currentQty,
                               currentLowPrice);
                }
                else if (e.KeyCode == Keys.F4)
                {
                    currentQty = Convert.ToDouble(product[isUsed ? "qtyUsedU3" : "qtyU3"] ?? 0);
                    var productData = GetProductByAnyCode(cellValue);
                    if (productData != null && productData.Rows.Count > 0)
                        UnitsFunction(productData.Rows[0], Convert.ToInt32(product["idUnite3"]));

                    UpdateRows(dgvMain.SelectedRows,
                               Convert.ToInt32(product["idUnite3"]),
                               currentSellPrice,
                               currentQty,
                               currentLowPrice);
                }
                else if (e.KeyCode == Keys.F3)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void dgvMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var dgv = sender as DataGridView;

            if (dgv.CurrentCell == null) return;

            int rowIndex = dgv.CurrentCell.RowIndex;

            // 👇 لو ضغط سهم يمين
            if (e.KeyCode == Keys.Right)
            {
                if (dgv.CurrentCell.OwningColumn.Name == "dgv2Name")
                {
                    dgv.CurrentCell = dgv.Rows[rowIndex].Cells["dgv2Qty"];
                    e.IsInputKey = true;
                }
                else if (dgv.CurrentCell.OwningColumn.Name == "dgv2Qty")
                {
                    dgv.CurrentCell = dgv.Rows[rowIndex].Cells["dgv2UnitPrice"];
                    e.IsInputKey = true;
                }
            }

            // 👇 لو ضغط سهم تحت
            else if (e.KeyCode == Keys.Down)
            {
                // تقدر تحدد هنا العمود اللي عاوز ينط عليه في الصف اللي بعده
                if (dgv.CurrentCell.OwningColumn.Name == "dgv2UnitPrice")
                {
                    // لو أنا في آخر صف أزود صف جديد
                    if (rowIndex == dgv.Rows.Count - 1)
                    {
                        int newIndex = dgv.Rows.Add();
                        dgv.CurrentCell = dgv.Rows[newIndex].Cells["dgv2Name"];
                    }
                    else
                    {
                        dgv.CurrentCell = dgv.Rows[rowIndex + 1].Cells["dgv2Name"];
                    }

                    e.IsInputKey = true;
                }
            }
        }


        private void UpdateRows(DataGridViewSelectedRowCollection rows, int unitId, double sellPrice, double qty, double lowPrice)
        {
            string qry = "SELECT uName FROM untits WHERE uID = @uID";
            string uName = "";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@uID", unitId);
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                    uName = dt.Rows[0]["uName"].ToString();
            }

            foreach (DataGridViewRow row in rows)
            {
                if (row.IsNewRow) continue; // تجاهل الصف الجديد

                // قراءة القيمة بأمان
                double currentQty = 0;
                object cellValue = row.Cells["dgv2Qty"].Value;

                if (cellValue != null && double.TryParse(cellValue.ToString(), out double parsedQty))
                {
                    currentQty = parsedQty;
                }

                // ✅ تحديث القيم في DataGridView
                row.Cells["dgv2Unite"].Value = uName;
                row.Cells["dgv2UnitPrice"].Value = sellPrice;          // سعر البيع
                row.Cells["dgv2Balance"].Value = qty - currentQty;     // الرصيد بعد الطرح
                row.Cells["dgv2UniteID"].Value = unitId;
                row.Cells["dgv2lowestPriceRounded"].Value = lowPrice;
                row.Cells["dgv2HighValueDis"].Value = currentHiDes;
                row.Cells["dgv2PurPrice"].Value = currentPurchese;
                row.Cells["dgv2UnitPriceDis"].Value = sellPrice;
                row.Cells["dgvsemiWholesale"].Value = currentSemiWholesale;
                row.Cells["dgvWholesale"].Value = currentWholesale;
            }
        }


        private void guna2DataGridView3_CellintClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvMain.Columns["dgv2del"].Index && e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgvMain.CurrentRow.Cells["dgvDetailsId"].Value);
                deteteRowDB(id); // حذف من قاعدة البيانات
                dgvMain.Rows.RemoveAt(e.RowIndex);

                // ✅ تأكد أن فيه صفوف بعد الحذف
                if (dgvMain.Rows.Count > 0)
                {
                    int newIndex = Math.Min(e.RowIndex, dgvMain.Rows.Count - 1);
                    dgvMain.CurrentCell = dgvMain.Rows[newIndex].Cells["dgv2Name"];
                }
                else
                {
                    // ✅ الجدول فاضي -> أضف صف جديد وحدد أول خلية
                    int newIndex = dgvMain.Rows.Add();
                    dgvMain.CurrentCell = dgvMain.Rows[newIndex].Cells["dgv2Name"];
                }
            }

            if (e.ColumnIndex == dgvMain.Columns["dgv2Edit"].Index && e.RowIndex >= 0)
            {
                if (MainClass.ProCardEdite)
                {
                    frmBlackout frmBlackout1 = new frmBlackout(this);
                    frmBlackout1.Show();
                    frmBlackout1.Owner = this;
                    frmCategoryCard frm = new frmCategoryCard();
                    frm.Owner = this;
                    frm.id = Convert.ToInt32(dgvMain.CurrentRow.Cells["dgv2proID"].Value);
                    frm.ShowDialog();
                    this.Focus();
                    frmBlackout1.Close();
                }
                else
                {
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                    guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

                }
            }
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = this;
            frmCloseSafe frm = new frmCloseSafe(this);
            frm.ShowDialog();
            this.Focus();
            frmBlackout.Close();
        }

        private Point dgvLocation;
        private Point gboxLocation;
        private Point panelLocation;
        private Size panelSize;
        private Size dgvSize;
        private Size gboxSize;

        bool returns = true;
        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowReturns)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (!returns)
            {
                returns = true;

                btnReverse.Text = "مرتجعات";
                txtSearch.Visible = true;
                txtCatSearch.Visible = true;
                btnGeneralBill.Visible = true;
                btnTaskBill.Visible = true;
                showPanel.Visible = isTaskbill;
                tsMode.Visible = true;

                classicPanel.Dock = DockStyle.None;
                bottomPanel.Visible = true;

                if (tsMode.Checked == true)
                    viewPanel.Visible = false;
                else
                    viewPanel.Visible = true;
                classicPanel.Controls.Clear();
                classicPanel.Controls.Add(dgvMain);
                classicPanel.Controls.Add(groupBox);

                classicPanel.Size = panelSize;
                classicPanel.Location = panelLocation;

                dgvMain.Location = dgvLocation;
                dgvMain.Size = dgvSize;

                groupBox.Location = gboxLocation;
                groupBox.Size = gboxSize;
            }
            else
            {
                returns = false;
                tsMode.Visible = false;
                txtSearch.Visible = false;
                txtCatSearch.Visible = false;
                showPanel.Visible = false;
                btnGeneralBill.Visible = false;
                btnTaskBill.Visible = false;

                btnReverse.Text = "نقاط البيع";
                panelSize = classicPanel.Size;
                panelLocation = classicPanel.Location;

                dgvLocation = dgvMain.Location;
                gboxLocation = groupBox.Location;

                gboxSize = groupBox.Size;
                dgvSize = dgvMain.Size;

                classicPanel.Dock = DockStyle.Fill;
                viewPanel.Visible = false;
                bottomPanel.Visible = false;
                classicPanel.Controls.Clear();

                frmAll_Bills frm = new frmAll_Bills(this);
                frm.pos = true;
                frm.partyType = "عميل";
                openedForms.Remove("frmAll_Bills");
                AddControls(frm);
            }

        }
        public void showPOS(int mid)
        {
            classicPanel.Dock = DockStyle.None;
            viewPanel.Visible = true;
            bottomPanel.Visible = true;

            classicPanel.Controls.Clear();
            classicPanel.Controls.Add(dgvMain);
            classicPanel.Controls.Add(groupBox);

            classicPanel.Size = panelSize;
            classicPanel.Location = panelLocation;

            dgvMain.Location = dgvLocation;
            dgvMain.Size = dgvSize;

            groupBox.Location = gboxLocation;
            groupBox.Size = gboxSize;

            if (mid == 0)
                return;

            btnReverse.Text = "مرتجعات";
            returns = true;
            fromReturnsBill = true;
            ReloadInvoiceToPOS(mid);


        }
        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();

        public void AddControls(Form f)
        {
            // إخفاء أي فورم معروضة حاليًا
            foreach (var frm in openedForms.Values)
            {
                frm.Hide();
            }

            if (openedForms.ContainsKey(f.Name))
            {
                var existingForm = openedForms[f.Name];

                if (existingForm.IsDisposed)
                {
                    existingForm = CreateNewFormInstance(f.Name);
                    openedForms[f.Name] = existingForm;
                    PrepareForm(existingForm);
                }

                existingForm.Show();
                existingForm.BringToFront();

                // تنفيذ الحدث لو الفورم تدعم IRefreshableForm
                if (existingForm is IRefreshableForm refreshable)
                {
                    refreshable.OnFormShownAgain();
                }
            }
            else
            {
                PrepareForm(f);
                openedForms.Add(f.Name, f);
                f.Show();
            }
        }
        private void PrepareForm(Form frm)
        {
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            classicPanel.Controls.Add(frm);
        }
        private Form CreateNewFormInstance(string formName)
        {
            // الحصول على مجمع البرنامج الحالي حيث يتم تعريف النموذج
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // محاولة إنشاء مثيل للنموذج باستخدام اسمه
            // يُفترض أن اسم النموذج يتطابق مع الاسم الكامل للصنف بما في ذلك مساحة الاسم
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
        private void guna2DataGridView3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dgvMain.CurrentCell.OwningColumn.Name == "dgv2Qty")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }

                if ((e.KeyChar == '.' || e.KeyChar == ',') && ((sender as TextBox).Text.Contains(".") || (sender as TextBox).Text.Contains(",")))
                {
                    e.Handled = true;
                }
            }
            else if (dgvMain.CurrentCell.OwningColumn.Name == "dgv2Dv")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }

                if ((e.KeyChar == '.' || e.KeyChar == ',') && ((sender as TextBox).Text.Contains(".") || (sender as TextBox).Text.Contains(",")))
                {
                    e.Handled = true;
                }
            }
            else if (dgvMain.CurrentCell.OwningColumn.Name == "dgv2Dp")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }

                if ((e.KeyChar == '.' || e.KeyChar == ',') && ((sender as TextBox).Text.Contains(".") || (sender as TextBox).Text.Contains(",")))
                {
                    e.Handled = true;
                }
            }
            else if (dgvMain.CurrentCell.OwningColumn.Name == "dgv2UnitPrice")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }

                if ((e.KeyChar == '.' || e.KeyChar == ',') && ((sender as TextBox).Text.Contains(".") || (sender as TextBox).Text.Contains(",")))
                {
                    e.Handled = true;
                }
            }
            else if (dgvMain.CurrentCell.OwningColumn.Name == "dgv2Status")
            {
                // الحصول على TextBox اللي جوه الخلية
                if (dgvMain.EditingControl is TextBox tb)
                {
                    if (e.KeyChar == 'م')
                    {
                        tb.Text = "مستعمل";
                        e.Handled = true; // منع الكتابة العادية
                    }
                    else if (e.KeyChar == 'ج')
                    {
                        tb.Text = "جديد";
                        e.Handled = true; // منع الكتابة العادية
                    }
                    else if (!char.IsControl(e.KeyChar))
                    {
                        e.Handled = true; // منع أي كتابة أخرى
                    }
                }
            }


        }

        private void guna2DataGridView3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl editingControl = e.Control as DataGridViewTextBoxEditingControl;
            if (editingControl != null)
            {
                editingControl.KeyPress -= guna2DataGridView3_KeyPress;
                editingControl.KeyPress += guna2DataGridView3_KeyPress;
            }
        }

        private void اضافةموردToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Owner = this;

            frmBlackout.Show();
            using (frmSupplier frm = new frmSupplier())
            {

                frm.Owner = this;
                frm.ShowDialog();

            }
            frmBlackout.Close();
            this.Focus();
        }

        private void اضافةصنفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Owner = this;

            frmBlackout.Show();
            using (frmCategoryAdd frm = new frmCategoryAdd())
            {

                frm.Owner = this;
                frm.ShowDialog();
            }
            frmBlackout.Close();
            this.Focus();
        }

        private void اضافةمخزنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = this;
            frmAddStore frmAddStore = new frmAddStore();
            frmAddStore.Owner = this;
            frmAddStore.ShowDialog();
            frmBlackout.Close();
        }

        private void txtDV_Leave(object sender, EventArgs e)
        {
            if (txtDV.Text.Length <= 0)
            {
                txtDV.Text = "0";
            }

        }

        private void txtDV_TextChanged(object sender, EventArgs e)
        {
            if (txtDV.Text.Length <= 0)
            {
                txtDV.Text = "0";
            }
            {
                if (!(txtPriceTotal.Text == "" || txtPriceTotal.Text == "0,00" || txtPriceTotal.Text == "0,0" || txtPriceTotal.Text == "0," || txtPriceTotal.Text == "0,00"))
                {

                    string pv = txtDV.Text;

                    double price = Convert.ToDouble(pv, CultureInfo.InvariantCulture);

                    string PayPrice = txtPriceTotal.Text;

                    double pay = Convert.ToDouble(PayPrice, CultureInfo.InvariantCulture);


                    double Price2 = pay - price;
                    txtClean.Text = Price2.ToString("F1");
                }
            }

        }
        // حذف بيانات بعد العوده من صفحه الفواتير
        public void clearData()
        {
            dgvMain.Rows.Clear();
            txtClean.Text = string.Empty;
            txtDV.Text = string.Empty;
            txtDP.Text = string.Empty;
            txtPriceTotal.Text = string.Empty;

            MainID = 0;


        }
        private Dictionary<(int pID, int uID, bool isUsed), double> tempStock = new();

        public async Task ReloadInvoiceToPOS(int oldMainID, bool updateStock = false)
        {
            if (oldMainID == 0) return;

            isRetuned = fromReturnsBill;
            isReturn = true;

            dgvMain.Rows.Clear();
            txtClean.Text = txtDV.Text = txtDP.Text = txtPriceTotal.Text = string.Empty;

            MainID = oldMainID;

            await billBack(); // تحميل بيانات الفاتورة الأساسية

            string qry = @"
            SELECT 
                d.DetailID,
                d.MainID,
                d.proID,
                CASE WHEN d.proID = 0 THEN d.proName ELSE p.pName END AS ProductName,
                d.qty,
                d.price,
                d.amount,
                d.pDescount,
                d.vDescount,
                d.priceAfterDes,
                d.isUsed,
                d.pBarcode,
                d.catName,
                d.unite,
                d.uniteID
            FROM tblDetails d
            LEFT JOIN products p ON d.proID = p.pID
            WHERE d.MainID = @mainID";

            DataTable dt = new DataTable();
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@mainID", oldMainID);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("الفاتورة فارغة أو غير موجودة");
                return;
            }

            tempStock.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                int rowIndex = dgvMain.Rows.Add();
                DataGridViewRow row = dgvMain.Rows[rowIndex];

                bool isUsed = dr["isUsed"] != DBNull.Value && Convert.ToInt32(dr["isUsed"]) == 1;
                string status = isUsed ? "مستعمل" : "جديد";

                int pID = dr["proID"] != DBNull.Value ? Convert.ToInt32(dr["proID"]) : 0;
                int uID = dr["uniteID"] != DBNull.Value ? Convert.ToInt32(dr["uniteID"]) : 0;

                double qty = dr["qty"] != DBNull.Value ? Convert.ToDouble(dr["qty"]) : 0;

                double balance = GetQuantityByUnit(pID, uID, isUsed);

                if (updateStock)
                    AddQuantityToTempStock(pID, uID, qty, isUsed);
                else
                    tempStock[(pID, uID, isUsed)] = qty;

                // تعبئة DataGridView
                row.Cells["dgv2IsUsed"].Value = isUsed ? 1 : 0;
                row.Cells["dgv2Status"].Value = status;
                row.Cells["dgbCat"].Value = dr.Table.Columns.Contains("catName") ? dr["catName"] : null;
                row.Cells["dgv2Code"].Value = dr["pBarcode"] != DBNull.Value ? dr["pBarcode"] : null;
                row.Cells["dgv2proID"].Value = pID;
                row.Cells["dgv2Shortcut"].Value = dr.Table.Columns.Contains("shorcut") ? dr["shorcut"] : null;
                row.Cells["dgv2Name"].Value = dr["ProductName"] != DBNull.Value ? dr["ProductName"] : null;
                row.Cells["dgv2Unite"].Value = dr.Table.Columns.Contains("unite") ? dr["unite"] : null;
                row.Cells["dgv2UnitPrice"].Value = dr["price"] != DBNull.Value ? dr["price"] : null;
                row.Cells["dgv2Dp"].Value = dr["pDescount"] != DBNull.Value ? dr["pDescount"] : null;
                row.Cells["dgv2CatID"].Value = dr.Table.Columns.Contains("categoryID") ? dr["categoryID"] : null;
                row.Cells["dgv2Qty"].Value = qty;
                row.Cells["dgv2Total"].Value = dr["amount"] != DBNull.Value ? dr["amount"] : null;
                row.Cells["dgv2TotalDes"].Value = dr["priceAfterDes"] != DBNull.Value ? dr["priceAfterDes"] : null;
                row.Cells["dgv2Balance"].Value = balance - qty;
                row.Cells["dgv2StockQty"].Value = balance;
                row.Cells["dgv2Hpd"].Value = dr.Table.Columns.Contains("hDiscountPro") ? dr["hDiscountPro"] : null;
                row.Cells["dgvStorID"].Value = dr.Table.Columns.Contains("storId") ? dr["storId"] : null;
                row.Cells["dgv2UniteID"].Value = uID;
            }

            if (fromReturnsBill)
                await LoadOldInvoiceProductsAsync();

            // إضافة صف جديد فارغ
            int rowIndexNew2 = dgvMain.Rows.Add();
            dgvMain.CurrentCell = dgvMain.Rows[rowIndexNew2].Cells["dgv2Name"];
            dgvMain.BeginEdit(true);

            isReturn = false;
        }

        private void AddQuantityToTempStock(int pID, int uID, double qty, bool isUsed)
        {
            if (tempStock.ContainsKey((pID, uID, isUsed)))
                tempStock[(pID, uID, isUsed)] += qty;
            else
                tempStock[(pID, uID, isUsed)] = qty;
        }

        // تحميل بيانات الفاتورة الأساسية
        private string adderss = "";
        private string phone = "";
        private async Task billBack()
        {
            string qry = @"
            SELECT m.total, 
                   m.priceClear, 
                   m.descount, 
                   m.partiesID, 
                   p.pName,
                   p.pAdderss,
                   p.pPhone
            FROM tblMain1 m
            INNER JOIN Parties p ON m.partiesID = p.pID
            WHERE m.MainID = @MainID;";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@MainID", MainID);

                await con.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        txtPriceTotal.Text = dr["total"]?.ToString();
                        txtClean.Text = dr["priceClear"]?.ToString();
                        txtDP.Text = dr["descount"]?.ToString();

                        Name = dr["pName"]?.ToString();
                        phone = dr["pPhone"]?.ToString();
                        adderss = dr["pAdderss"]?.ToString();

                        partiesID = dr["partiesID"] != DBNull.Value ? Convert.ToInt32(dr["partiesID"]) : 0;
                    }
                }
            }
        }



        // مكان عام علشان نخزن النسخة القديمة
        private List<(int pID, int uID, bool isUsed, double qty)> oldProductsList;

        // دالة تحميل قديمة لكن Asynchronous
        private async Task LoadOldInvoiceProductsAsync()
        {
            string qryOld = @"SELECT proID, uniteID, qty, isUsed FROM tblDetails WHERE MainID = @MainID";

            var tempList = new List<(int pID, int uID, bool isUsed, double qty)>();

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qryOld, con))
            {
                cmd.Parameters.AddWithValue("@MainID", MainID);

                await con.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        int pID = dr["proID"] != DBNull.Value ? Convert.ToInt32(dr["proID"]) : 0;
                        int uID = dr["uniteID"] != DBNull.Value ? Convert.ToInt32(dr["uniteID"]) : 0;
                        bool isUsed = dr["isUsed"] != DBNull.Value && Convert.ToInt32(dr["isUsed"]) == 1;
                        double qty = dr["qty"] != DBNull.Value ? Convert.ToDouble(dr["qty"]) : 0;

                        tempList.Add((pID, uID, isUsed, qty));
                    }
                }
            }

            // نخزنها في المتغير العام بعد ما الاتصال يتقفل
            oldProductsList = tempList;
        }

        // تحديث المخزون في قاعدة البيانات
        private async void SaveInvoiceAndUpdateStock()
        {
            if (oldProductsList == null || oldProductsList.Count == 0)
                return;

            foreach (var item in oldProductsList)
            {
                await qtyStoreAddAsync(item.pID, item.uID, item.isUsed, item.qty);
            }
        }


        private double GetQuantityByUnit(int pID, int uID, bool isUsed)
        {
            double qty = 0;

            string qry = @"
            SELECT 
                idUnite1, idUnite2, idUnite3,
                qtyU1, qtyU2, qtyU3,
                qtyUsedU1, qtyUsedU2, qtyUsedU3
            FROM totalStor ts
            INNER JOIN products p ON ts.pID = p.pID
            WHERE ts.pID = @pID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@pID", pID);

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (dt.Rows.Count == 0) return 0;

                DataRow row = dt.Rows[0];

                int idU1 = row["idUnite1"] != DBNull.Value ? Convert.ToInt32(row["idUnite1"]) : 0;
                int idU2 = row["idUnite2"] != DBNull.Value ? Convert.ToInt32(row["idUnite2"]) : 0;
                int idU3 = row["idUnite3"] != DBNull.Value ? Convert.ToInt32(row["idUnite3"]) : 0;

                if (uID == idU1)
                    qty = isUsed ? SafeToDouble3(row["qtyUsedU1"]) : SafeToDouble3(row["qtyU1"]);
                else if (uID == idU2)
                    qty = isUsed ? SafeToDouble3(row["qtyUsedU2"]) : SafeToDouble3(row["qtyU2"]);
                else if (uID == idU3)
                    qty = isUsed ? SafeToDouble3(row["qtyUsedU3"]) : SafeToDouble3(row["qtyU3"]);
            }

            return qty;
        }

        /// <summary>
        /// دالة مساعدة لتحويل القيمة إلى double بأمان
        /// </summary>
        private double SafeToDouble3(object value)
        {
            return value != DBNull.Value && double.TryParse(value.ToString(), out double result)
                ? result
                : 0;
        }


        private void sellCheckState()
        {
            if (dgvMain.InvokeRequired)
            {
                dgvMain.Invoke(new System.Windows.Forms.MethodInvoker(sellCheckState));
                return;
            }


        }

        private void guna2DataGridView3_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (!dgvMain.Controls.OfType<VScrollBar>().Any(s => s.Visible))
            {
                dgvMain.Region = null;
            }

            bool hasValidRow = dgvMain.Rows
                             .Cast<DataGridViewRow>()
                             .Any(r => r.Cells["dgv2Name"].Value != null &&
                                       !string.IsNullOrWhiteSpace(r.Cells["dgv2Name"].Value.ToString()) &&
                                       !r.IsNewRow); // تجاهل الصف الفاضي الأخير الافتراضي

            if (hasValidRow)
            {
                btnCheckout.Enabled = true;
                btnEnd.Enabled = true;
                btnPrint.Enabled = true;
                txtDP.Enabled = true;
                txtDV.Enabled = true;
            }
            else
            {
                btnCheckout.Enabled = false;
                btnEnd.Enabled = false;
                btnPrint.Enabled = false;
                txtDP.Enabled = false;
                txtDV.Enabled = false;
            }
        }



        private async void timer1_Tick(object sender, EventArgs e)
        {

        }


        private Point defdgvMainLocation;
        private Size defdgvMainSize;

        private Point defgboxLocation;
        private Size defgboxSize;

        private Point defpanelLocation;
        private Size defpanelSize;
        private void frmPOS_Resize(object sender, EventArgs e)
        {

            defdgvMainLocation = dgvMain.Location;
            defdgvMainSize = dgvMain.Size;
            defgboxLocation = groupBox.Location;
            defgboxSize = groupBox.Size;
            defpanelLocation = classicPanel.Location;
            defpanelSize = classicPanel.Size;

            CenterLoadingImage();

        }
        private void CenterLoadingImage()
        {

        }

        private void LightMode()
        {
            backgroundPrmary = Color.FromArgb(243, 243, 243);
            backgroundseconder = Color.FromArgb(230, 230, 230);
            textColor = Color.FromArgb(51, 51, 51);
            textColor2 = Color.White;
            checkedFillColor = Color.FromArgb(136, 214, 218);
            checkedForColor = Color.FromArgb(250, 250, 20);
            borderColor = Color.FromArgb(1, 95, 95);

            safeImage.Image = Properties.Resources.safe_light;
            txtSearch.IconRight = Properties.Resources.search_ligh;

        }
        private void DarkMode()
        {
            //-> Dark Mode
            backgroundPrmary = Color.FromArgb(32, 32, 32);
            backgroundseconder = Color.FromArgb(38, 38, 38);
            textColor = Color.FromArgb(204, 204, 204);
            textColor2 = textColor;
            checkedFillColor = Color.FromArgb(1, 95, 95);
            checkedForColor = Color.FromArgb(2, 2, 2);
            borderColor = Color.FromArgb(136, 214, 218);

            safeImage.Image = Properties.Resources.safe_dark;
            txtSearch.IconRight = Properties.Resources.search_Dark;

        }
        private void ThemeMode()
        {
            this.BackColor = backgroundPrmary;

            //->Panels
            CategoryPanel.BackColor = backgroundPrmary;
            // currentFlowPanelProduct.BackColor = backgroundPrmary;
            topPanel.BackColor = backgroundseconder;
            bottomPanel.BackColor = backgroundseconder;
            classicPanel.BackColor = backgroundPrmary;

            //->Button
            btnCheckout.FillColor = borderColor;
            btnCheckout.ForeColor = textColor2;

            btnNew.FillColor = borderColor;
            btnNew.ForeColor = textColor2;
            btnNew.BorderColor = checkedFillColor;

            btnHold.FillColor = borderColor;
            btnHold.ForeColor = textColor2;
            btnHold.BorderColor = checkedFillColor;

            btnBill.FillColor = borderColor;
            btnBill.ForeColor = textColor2;
            btnBill.BorderColor = checkedFillColor;
            notificationP.FillColor = checkedFillColor;


            btnReverse.FillColor = borderColor;
            btnReverse.ForeColor = textColor2;
            btnReverse.BorderColor = checkedFillColor;

            //->Radio Button
            rbValue.CheckedState.FillColor = checkedFillColor;
            rbValue.CheckedState.BorderColor = checkedFillColor;
            rbValue.ForeColor = textColor;

            rbPersent.CheckedState.FillColor = checkedFillColor;
            rbPersent.CheckedState.BorderColor = checkedFillColor;
            rbPersent.ForeColor = textColor;



            lblClean.ForeColor = textColor;
            lblDesc.ForeColor = textColor;
            lblTotal.ForeColor = textColor;
            groupBox.ForeColor = textColor;

            //->TextBox
            txtSearch.ForeColor = textColor;
            txtSearch.BorderColor = checkedFillColor;
            txtSearch.FillColor = backgroundPrmary;

            txtPriceTotal.ForeColor = textColor;
            txtPriceTotal.BorderColor = checkedFillColor;
            txtPriceTotal.FillColor = backgroundseconder;

            txtDV.ForeColor = textColor;
            txtDV.BorderColor = checkedFillColor;
            txtDV.FillColor = backgroundseconder;

            txtDP.ForeColor = textColor;
            txtDP.BorderColor = checkedFillColor;
            txtDP.FillColor = backgroundPrmary;

            txtClean.ForeColor = textColor;
            txtClean.BorderColor = checkedFillColor;
            txtClean.FillColor = backgroundseconder;

            //-> datagride view 
            dgvMain.BackgroundColor = backgroundPrmary;
            dgvMain.GridColor = backgroundPrmary;

            dgvMain.DefaultCellStyle.BackColor = backgroundPrmary;
            dgvMain.DefaultCellStyle.ForeColor = textColor;
            dgvMain.DefaultCellStyle.SelectionBackColor = checkedFillColor;
            dgvMain.DefaultCellStyle.SelectionForeColor = textColor;

            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = backgroundseconder;
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            dgvMain.ColumnHeadersDefaultCellStyle.SelectionBackColor = checkedFillColor;

            dgvMain.RowsDefaultCellStyle.BackColor = backgroundPrmary;
            dgvMain.AlternatingRowsDefaultCellStyle.BackColor = backgroundPrmary;
            dgvMain.RowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
            dgvMain.RowsDefaultCellStyle.ForeColor = textColor;
            dgvMain.RowsDefaultCellStyle.SelectionForeColor = textColor;

            //dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                SearchName = txtSearch.Text;
                lastSelectedCategory = "search";
                foreach (Control ctrl in CategoryPanel.Controls)
                {
                    if (ctrl is Guna.UI2.WinForms.Guna2Button btn)
                        btn.Checked = false;
                }

                setFlowPanelPro();

                // إعادة المؤشرات
                currentPage = 0;
                firstNumber = 0;
                isFirstTime = true;
                allLoaded = false;

                // حذف أيّ عناصر قديمة
                currentFlowPanelProduct.Controls.Clear();
                currentFlowPanelProduct.Refresh();

                // تحميل أول دفعة
                await LoadNextPageAsync(18, string.Empty, SearchName);

            }
        }
        private int CalculateItemsPerRow()
        {
            // عرض العنصر نفسه
            int itemWidth = 230;

            // خذ Margin الأيسر والأيمن من أول عنصر في الـ pool
            int elementMargin = 0;
            if (currentFlowPanelProduct.Controls.OfType<ucProduct2>().FirstOrDefault() is ucProduct2 sample)
            {
                elementMargin = sample.Margin.Left + sample.Margin.Right;
            }

            // العرض الصافي للـ panel بعد طرح الـ Padding الداخلي
            int panelContentWidth = currentFlowPanelProduct.ClientSize.Width
                                  - currentFlowPanelProduct.Padding.Left
                                  - currentFlowPanelProduct.Padding.Right;

            // العرض الإجمالي لكل عنصر مع مسافته
            int totalItemWidth = itemWidth + elementMargin;

            // احسب عدد العناصر
            int itemsPerRow = panelContentWidth / totalItemWidth;
            return Math.Max(1, itemsPerRow);
        }

        private void dgvMain_Resize(object sender, EventArgs e)
        {
            if (dgvMain.Controls.OfType<VScrollBar>().Any(s => s.Visible))
                ClipControlRegion(dgvMain, "left", 17);
            else
                dgvMain.Region = null;


        }

        private void txtDP_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام والنقطة العشرية + Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // يمنع كتابة أكتر من نقطة
            TextBox txt = sender as TextBox;
            if (txt != null && e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                if (double.TryParse(txtPriceTotal.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double price) && price > 0)
                {
                    if (double.TryParse(txtDP.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double discount))
                    {
                        double calc = (discount / 100.0) * price;
                        double total = price - calc;

                        txtDV.Text = calc.ToString("F1", CultureInfo.InvariantCulture);
                        txtClean.Text = total.ToString("F1", CultureInfo.InvariantCulture);

                        UpdateColumnForAllRows(discount);
                    }
                }
            }
        }


        private void txtDV_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                double price = double.Parse(
                    txtPriceTotal.Text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture
                );
                if (price > 0)
                {
                    double discount = Convert.ToDouble(txtDV.Text);

                    double persent = (discount * 100.0) / price;

                    txtDP.Text = persent.ToString("F2", CultureInfo.InvariantCulture);

                    double calc = (persent / 100.0) * price;
                    double total = price - calc;

                    txtDV.Text = calc.ToString("F2");
                    txtClean.Text = total.ToString("F2");

                    UpdateColumnForAllRows(persent);
                }
            }

        }

        private void rbValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbValue.Checked)
            {
                txtDP.ReadOnly = true;
                txtDV.ReadOnly = false;

                rbPersent.Checked = false;


                txtDV.FillColor = backgroundPrmary;
                txtDP.FillColor = backgroundseconder;
            }
        }

        private void rbPersent_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPersent.Checked)
            {
                txtDP.ReadOnly = false;
                txtDV.ReadOnly = true;

                rbValue.Checked = false;

                txtDV.FillColor = backgroundseconder;
                txtDP.FillColor = backgroundPrmary;
            }
        }

        public void UpdateColumnForAllRows(double discount)

        {
            // التحقق من وجود جميع الأعمدة المطلوبة
            string[] requiredColumns = { "dgv2proID", "dgv2UnitPrice", "dgv2Qty", "dgv2lowestPriceRounded",
                                 "dgv2PurPrice", "dgv2TotalDes", "dgv2Dp", "dgv2Dv" };

            foreach (string col in requiredColumns)
            {
                if (!dgvMain.Columns.Contains(col))
                    return; // إذا أي عمود ناقص نخرج
            }

            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (row.IsNewRow || row.Cells["dgv2proID"].Value == null)
                    continue;

                // دالة مساعدة لتحويل القيم بأمان
                double SafeParse(object value)
                {
                    double result;
                    if (value == null || !double.TryParse(value.ToString(), out result))
                        return 0;
                    return result;
                }

                double price = SafeParse(row.Cells["dgv2UnitPrice"].Value);
                double qty = SafeParse(row.Cells["dgv2Qty"].Value);

                // أقل سعر مسموح
                double lowestPrice = SafeParse(row.Cells["dgv2lowestPriceRounded"].Value);
                if (lowestPrice == 0)
                {
                    lowestPrice = SafeParse(row.Cells["dgv2PurPrice"].Value);
                    row.Cells["dgv2lowestPriceRounded"].Value = lowestPrice.ToString("F1", CultureInfo.InvariantCulture);
                }

                // حساب سعر الوحدة بعد الخصم
                double discountedUnitPrice = price * (1 - discount / 100.0);

                // إذا سعر الوحدة بعد الخصم أقل من الحد الأدنى
                double rowDiscount = discount;
                if (discountedUnitPrice < lowestPrice && price > 0)
                {
                    rowDiscount = (1 - (lowestPrice / price)) * 100; // نسبة الخصم المناسبة
                    discountedUnitPrice = lowestPrice; // سعر الوحدة يساوي الحد الأدنى
                }

                // حساب السعر الإجمالي قبل وبعد الخصم
                double totalPrice = price * qty;
                double discountedTotal = discountedUnitPrice * qty;
                double discountValue = totalPrice - discountedTotal;

                // تقريب الأرقام لأعلى
                discountedUnitPrice = discountedUnitPrice;
                discountedTotal = discountedTotal;
                discountValue = discountValue;
                rowDiscount = rowDiscount;       // تحديث الأعمدة
                row.Cells["dgv2UnitPriceDis"].Value = discountedUnitPrice.ToString("F1", CultureInfo.InvariantCulture);
                row.Cells["dgv2TotalDes"].Value = discountedTotal.ToString("F1", CultureInfo.InvariantCulture);
                row.Cells["dgv2Dp"].Value = rowDiscount.ToString("F2", CultureInfo.InvariantCulture);
                row.Cells["dgv2Dv"].Value = discountValue.ToString("F1", CultureInfo.InvariantCulture);

            }
        }

        public async void showReturnedBill(int mainID)
        {
            dgvMain.Rows.Clear();


            try
            {
                string qry = @"
                SELECT 
                    d.DetailID,
                    p.pName,
					c.catName,
                    d.unite,
                    d.qty,
                    d.price,
                    d.amount,
                    d.pDescount,
                    d.vDescount,
                    ISNULL(d.vDescount, 0) AS vDescount,
                    SUM(d.amount - ISNULL(d.vDescount, 0)) AS TotalAfterDiscount
                FROM tblMain1 m
                INNER JOIN tblDetails d ON m.MainID = d.MainID
                INNER JOIN products p ON p.pID = d.proID
				INNER JOIN category c ON c.catID = p.categoryID
                WHERE m.MainID = @mainID AND (d.DeleteFlag IS NULL OR d.DeleteFlag = 0)
                GROUP BY 
                    d.DetailID,
                    p.pName,
					c.catName,
                    d.unite,
                    d.qty,
                    d.price,
                    d.amount,
                    d.pDescount,
                    d.vDescount";

                SqlParameter[] parameters = { new SqlParameter("@mainID", mainID) };

                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                int rowIndex = dgvMain.Rows.Count + 1;

                foreach (DataRow row in dt.Rows)
                {
                    dgvMain.Rows.Add(
                        rowIndex++,
                        row["DetailID"],
                        row["pName"],
                        row["catName"],
                        row["unite"],
                        row["qty"],
                        row["price"],
                        row["amount"],
                        row["pDescount"],
                        row["vDescount"],
                        row["TotalAfterDiscount"]
                    );
                }
                dgvMain.PerformLayout();
                dgvMain.Refresh();

            }
            catch
            {
                MessageBox.Show("حدث خطأ");
            }
        }
        public static DataTable LoadDataReturn(string qry, SqlParameter[] parameters)
        {
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                return dt;
            }
        }


        private Size panelSize1;
        private Point panelLocation1;

        private void tsMode_CheckedChanged(object sender, EventArgs e)
        {
            if (tsMode.Checked)
            {
                panelSize1 = classicPanel.Size;
                panelLocation1 = classicPanel.Location;
                classicPanel.Dock = DockStyle.Fill;
                viewPanel.Visible = false;
                txtCatSearch.Visible = false;
                showPanel.Visible = false;
                btnGeneralBill.Visible = false;
                btnTaskBill.Visible = false;
            }
            else
            {
                classicPanel.Size = panelSize1;
                classicPanel.Location = panelLocation1;
                classicPanel.Dock = DockStyle.None;
                viewPanel.Visible = true;
                txtCatSearch.Visible = true;
                showPanel.Visible = isTaskbill;
                btnGeneralBill.Visible = true;
                btnTaskBill.Visible = true;
            }
        }

        private void qtyStore()
        {
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (row.IsNewRow) continue; // تخطي الصف الجديد الفاضي

                int pID = Convert.ToInt32(row.Cells["dgv2proID"].Value);
                string status1 = row.Cells["dgv2Status"].Value?.ToString();
                bool isUsed;

                string qry;

                if (status1 == "مستعمل")
                {
                    qry = @"UPDATE totalStor 
                    SET qtyUsedU1 = @qtyU1,
                        qtyUsedU2 = @qtyU2,
                        qtyUsedU3 = @qtyU3
                    WHERE pID = @pID";
                    isUsed = true;
                }
                else
                {
                    qry = @"UPDATE totalStor 
                    SET qtyU1 = @qtyU1,
                        qtyU2 = @qtyU2,
                        qtyU3 = @qtyU3
                    WHERE pID = @pID";
                    isUsed = false;
                }

                // حساب الكميات حسب الوحدة
                SetProductUnitInfo(
                    pID,
                    isUsed,
                    Convert.ToInt32(row.Cells["dgv2UniteID"].Value),
                    Convert.ToDouble(row.Cells["dgv2Qty"].Value)
                );

                // استخدام اتصال جديد لكل عملية
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@pID", pID);
                    cmd.Parameters.AddWithValue("@qtyU1", qtyU1);
                    cmd.Parameters.AddWithValue("@qtyU2", qtyU2);
                    cmd.Parameters.AddWithValue("@qtyU3", qtyU3);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private async Task qtyStoreAddAsync(int pid, int uid, bool isuse, double qty)
        {
            string qry;

            if (isuse)
            {
                qry = @"UPDATE totalStor 
                   SET qtyUsedU1 = @qtyU1,
                       qtyUsedU2 = @qtyU2,
                       qtyUsedU3 = @qtyU3
                 WHERE pID = @pID";
            }
            else
            {
                qry = @"UPDATE totalStor 
                   SET qtyU1 = @qtyU1,
                       qtyU2 = @qtyU2,
                       qtyU3 = @qtyU3
                 WHERE pID = @pID";
            }

            // حساب الكميات على حسب الوحدة (تتنفذ في نفس Thread عادي)
            SetProductUnitInfoAdd(pid, isuse, uid, qty);

            await Task.Run(() =>
            {
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@pID", pid);
                    cmd.Parameters.AddWithValue("@qtyU1", qtyU1);
                    cmd.Parameters.AddWithValue("@qtyU2", qtyU2);
                    cmd.Parameters.AddWithValue("@qtyU3", qtyU3);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            });
        }


        private double qtyU1, qtyU2, qtyU3;

        private void SetProductUnitInfo(int pID, bool isUsed, int currentUinte, double extraQtyU = 0)
        {
            string query = @"
                 SELECT p.*, c.*, u.uName, ts.*
                 FROM products p
                 INNER JOIN category c ON c.catID = p.categoryID
                 INNER JOIN untits u ON p.idUniteDef = u.uID
                 INNER JOIN totalStor ts ON ts.pID = p.pID
                 WHERE p.pID = @value";

            using (SqlConnection con = MainClass.GetConnection()) // ✅ بدل new SqlConnection
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@value", pID);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count == 0)
                    return; // لو مفيش بيانات للمنتج ده

                DataRow row = dt.Rows[0];

                int idUnite1 = Convert.ToInt32(row["idUnite1"]);
                int idUnite2 = Convert.ToInt32(row["idUnite2"]);
                int idUnite3 = Convert.ToInt32(row["idUnite3"]);

                int numberU2 = Convert.ToInt32(row["numberU2"]); // كم وحدة U3 في U2
                int numberU3 = Convert.ToInt32(row["numberU3"]); // كم U2 في U1

                // 1️⃣ الحصول على الكمية حسب الوحدة الافتراضية
                if (currentUinte == idUnite3)
                {
                    qtyU3 = isUsed ? Convert.ToDouble(row["qtyUsedU3"]) : Convert.ToDouble(row["qtyU3"]);
                    qtyU3 -= extraQtyU;
                }
                else if (currentUinte == idUnite2)
                {
                    double baseQty = isUsed ? Convert.ToDouble(row["qtyUsedU2"]) : Convert.ToDouble(row["qtyU2"]);
                    baseQty -= extraQtyU;
                    qtyU3 = baseQty * numberU3; // نحولها للوحدة الأصغر U3
                }
                else
                {
                    double baseQty = isUsed ? Convert.ToDouble(row["qtyUsedU1"]) : Convert.ToDouble(row["qtyU1"]);
                    baseQty -= extraQtyU;
                    qtyU3 = baseQty * numberU2 * numberU3; // نحولها للوحدة الأصغر U3
                }

                // 2️⃣ حساب الكميات بوحدات مختلفة
                qtyU2 = qtyU3 / numberU3; // كام وحدة U2
                qtyU1 = qtyU2 / numberU2; // كام وحدة U1
            }

        }

        private void SetProductUnitInfoAdd(int pID, bool isUsed, int currentUinte, double extraQtyU = 0)
        {
            string query = @"
          SELECT p.*, c.*, u.uName, ts.*
          FROM products p
          INNER JOIN category c ON c.catID = p.categoryID
          INNER JOIN untits u ON p.idUniteDef = u.uID
          INNER JOIN totalStor ts ON ts.pID = p.pID
          WHERE p.pID = @value";

            using (SqlConnection con = MainClass.GetConnection()) // ✅ بدل new SqlConnection
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@value", pID);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count == 0)
                    return; // لو مفيش بيانات للمنتج ده

                DataRow row = dt.Rows[0];

                int idUnite1 = Convert.ToInt32(row["idUnite1"]);
                int idUnite2 = Convert.ToInt32(row["idUnite2"]);
                int idUnite3 = Convert.ToInt32(row["idUnite3"]);

                int numberU2 = Convert.ToInt32(row["numberU2"]); // كم وحدة U3 في U2
                int numberU3 = Convert.ToInt32(row["numberU3"]); // كم U2 في U1

                // 1️⃣ الحصول على الكمية حسب الوحدة الافتراضية
                if (currentUinte == idUnite3)
                {
                    qtyU3 = isUsed ? Convert.ToDouble(row["qtyUsedU3"]) : Convert.ToDouble(row["qtyU3"]);
                    qtyU3 += extraQtyU;
                }
                else if (currentUinte == idUnite2)
                {
                    double baseQty = isUsed ? Convert.ToDouble(row["qtyUsedU2"]) : Convert.ToDouble(row["qtyU2"]);
                    baseQty += extraQtyU;
                    qtyU3 = baseQty * numberU3; // نحولها للوحدة الأصغر U3
                }
                else
                {
                    double baseQty = isUsed ? Convert.ToDouble(row["qtyUsedU1"]) : Convert.ToDouble(row["qtyU1"]);
                    baseQty += extraQtyU;
                    qtyU3 = baseQty * numberU2 * numberU3; // نحولها للوحدة الأصغر U3
                }

                // 2️⃣ حساب الكميات بوحدات مختلفة
                qtyU2 = qtyU3 / numberU3; // كام وحدة U2
                qtyU1 = qtyU2 / numberU2; // كام وحدة U1
            }
        }


        private async void btnPrint_Click(object sender, EventArgs e)
        {
            await SyncInvoiceDetailsAsync();
            await UpdateInvoiceAsync(MainID, "underwork");

            MainClass.PrintInvoiceAsync(MainID, false, "بيان أسعار", Convert.ToDouble(txtClean.Text == string.Empty ? "0" : txtClean.Text));

        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            using (frmOrderCard frm = new frmOrderCard())
            {
                frm.ShowDialog();
            }
            this.Focus();
        }

        private void cbPayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!MainClass.WholeSale && (cbPayType.SelectedIndex == 2 || cbPayType.SelectedIndex == 3))
            {
                cbPayType.SelectedIndex = 0;
                GetCurrentSell(0);

                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            else if (!MainClass.HalfWholeSale && cbPayType.SelectedIndex == 1)
            {
                cbPayType.SelectedIndex = 0;
                GetCurrentSell(0);
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (cbPayType.SelectedIndex == 0) // تجزئة
            {
                GetCurrentSell(0);
            }
            else if (cbPayType.SelectedIndex == 1) // نصف جملة 
            {
                GetCurrentSell(1);
            }
            else if (cbPayType.SelectedIndex == 2) // جملة
            {
                GetCurrentSell(2);
            }
            else if (cbPayType.SelectedIndex == 3) // اقل سعر بيع
            {
                GetCurrentSell(3);
            }
        }

        private void GetCurrentSell(int type)
        {
            // التحقق من وجود جميع الأعمدة المطلوبة
            string[] requiredColumns = { "dgvWholesale", "dgvsemiWholesale", "dgv2lowestPriceRounded", "dgv2UnitPrice", "dgv2TotalDes" };

            foreach (string col in requiredColumns)
            {
                if (!dgvMain.Columns.Contains(col))
                    return; // إذا أي عمود ناقص نخرج
            }

            double totalSum = 0; // متغير لتجميع القيم

            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (row.IsNewRow || row.Cells["dgv2proID"].Value == null)
                    continue;

                // دالة مساعدة لتحويل القيم بأمان
                double SafeParse(object value)
                {
                    double result;
                    if (value == null || !double.TryParse(value.ToString(), out result))
                        return 0;
                    return result;
                }

                double qty = SafeParse(row.Cells["dgv2Qty"].Value);
                double price = 0;

                if (type == 0)
                    price = SafeParse(row.Cells["dgv2UnitPrice"].Value);
                else if (type == 1)
                    price = SafeParse(row.Cells["dgvsemiWholesale"].Value);
                else if (type == 2)
                    price = SafeParse(row.Cells["dgvWholesale"].Value);
                else if (type == 3)
                    price = SafeParse(row.Cells["dgv2lowestPriceRounded"].Value);

                double total = price * qty;
                row.Cells["dgv2UnitPriceDis"].Value = price;
                row.Cells["dgv2TotalDes"].Value = total;
                row.Cells["dgv2Total"].Value = total;

            }

            GetTotal2();
        }

        private void dgvMain_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvMain.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvMain.CurrentCell.ColumnIndex)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, true);

                    // ارسم النص بلون مختلف للهيدر المحدد
                    TextRenderer.DrawText(
                        e.Graphics,
                        e.FormattedValue?.ToString(),
                        new Font("Tahoma", 11, FontStyle.Bold),
                        e.CellBounds,
                        Color.FromArgb(204, 204, 204),              // ← لون خط الهيدر المحدد
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    // ارسم حدود الخلية
                    e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                }
            }
        }
        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {

            // الألوان العادية للصفوف
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);

            // الصفوف المتبادلة (صف غامق وصف فاتح)
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // الرمادي الفاتح
            dgv.RowsDefaultCellStyle.BackColor = Color.White;                              // الصف العادي

            // ألوان التحديد (Selection)
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // ✅ ألوان الهيدر (عادي + خط)
            dgv.EnableHeadersVisualStyles = false; // مهم عشان ألوانك تشتغل
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;                 // لون خط الهيدر

            // لون الهيدر وقت التحديد (لو حابب تخليه مختلف)
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void txtCatSearch_TextChanged(object sender, EventArgs e)
        {
            AddCategory(txtCatSearch.Text);

        }

        private void btnGeneralBill_Click(object sender, EventArgs e)
        {
            isTaskbill = false;
            btnGeneralBill.Checked = true;
            btnCheckout.Visible = true;
            btnEnd.Visible = false;
            txtPartyName.Visible = false;
            txtTaskNumber.Visible = false;
            showPanel.Visible = false;
            taskID = 0;

        }

        private void btnTaskBill_Click(object sender, EventArgs e)
        {
            using (frmBlackout frmBlackout = new frmBlackout(this))
            {
                frmBlackout.Show();

                using (frmChooseTask frm = new frmChooseTask(this))
                {
                    frm.Owner = this;

                    DialogResult result = frm.ShowDialog();

                    if (result == DialogResult.OK)
                    {

                        isTaskbill = true;
                        btnEnd.Visible = true;
                        btnCheckout.Visible = false;
                        txtPartyName.Visible = true;
                        txtTaskNumber.Visible = true;
                        showPanel.Visible = true;

                    }
                    else
                    {
                        isTaskbill = false;
                        btnGeneralBill.Checked = true;
                        btnCheckout.Visible = true;
                        btnEnd.Visible = false;
                        txtPartyName.Visible = false;
                        txtTaskNumber.Visible = false;
                        showPanel.Visible = false;

                    }
                }
            }
        }
        public void resultSearch(int taskid, int paryid, string partyName, string taskNumber)
        {
            taskID = taskid;
            partiesID = paryid;
            txtTaskNumber.Text = taskNumber;
            txtPartyName.Text = partyName;
        }
        private static void CenterButtonInPanel(Panel panel, Guna2Button btn)
        {
            if (panel == null || btn == null) return;

            // حساب موقع منتصف البانل
            int x = (panel.Width - btn.Width) / 2;
            int y = (panel.Height - btn.Height) / 2;

            btn.Location = new Point(x, y);
        }

        private void CenterPanelHorizontally(Panel panel)
        {
            int x = (this.ClientSize.Width - panel.Width) / 2;
            int y = panel.Location.Y; // يفضل نسيب نفس الموضع الرأسي بدون تغيير

            panel.Location = new Point(x, y);
        }

        private void frmPOS_SizeChanged(object sender, EventArgs e)
        {
            CenterButtonInPanel(bottomPanel, btnEnd);
            CenterPanelHorizontally(showPanel);
        }

        private async void btnEnd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    // 🔹 1. البحث عن فاتورة قديمة بنفس الـ taskID
                    string checkQuery = "SELECT TOP 1 MainID, total, TotalWithInterest FROM tblMain1 WHERE taskID = @taskID AND MainID <> @ID";
                    int oldMainID = 0;
                    double oldTotal = 0, oldTotalWithInterest = 0;

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@taskID", taskID);
                        checkCmd.Parameters.AddWithValue("@ID", MainID);

                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldMainID = Convert.ToInt32(reader["MainID"]);
                                oldTotal = Convert.ToDouble(reader["total"]);
                                oldTotalWithInterest = Convert.ToDouble(reader["TotalWithInterest"]);
                            }
                        }
                    }

                    // 🔹 2. لو لقينا فاتورة قديمة بنفس الـ taskID
                    if (oldMainID > 0)
                    {
                        double newTotal = Convert.ToDouble(string.IsNullOrWhiteSpace(txtPriceTotal.Text) ? "0" : txtPriceTotal.Text);
                        double newTotalWithInterest = Convert.ToDouble(string.IsNullOrWhiteSpace(txtClean.Text) ? "0" : txtClean.Text);

                        double combinedTotal = oldTotal + newTotal;
                        double combinedTotalWithInterest = oldTotalWithInterest + newTotalWithInterest;

                        // 🔹 3. نحدث الفاتورة القديمة بالمجموع الجديد
                        string updateOldQuery = @"
                    UPDATE tblMain1
                    SET total = @combinedTotal,
                        TotalWithInterest = @combinedTotalWithInterest,
                        priceClear = @combinedTotalWithInterest,
                        InvoiceIssuanceValue = @combinedTotalWithInterest
                        
                    WHERE MainID = @oldID";
                        using (SqlCommand updateCmd = new SqlCommand(updateOldQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@combinedTotal", combinedTotal);
                            updateCmd.Parameters.AddWithValue("@combinedTotalWithInterest", combinedTotalWithInterest);
                            updateCmd.Parameters.AddWithValue("@oldID", oldMainID);
                            updateCmd.ExecuteNonQuery();
                        }

                        // 🔹 4. تحديث MainID في جدول التفاصيل (tblDetails)
                        string updateDetailsQuery = "UPDATE tblDetails SET MainID = @oldID WHERE MainID = @newID";
                        using (SqlCommand updateDetailsCmd = new SqlCommand(updateDetailsQuery, con))
                        {
                            updateDetailsCmd.Parameters.AddWithValue("@oldID", oldMainID);
                            updateDetailsCmd.Parameters.AddWithValue("@newID", MainID);
                            updateDetailsCmd.ExecuteNonQuery();
                        }

                        // 🔹 5. حذف الفاتورة الجديدة لأنها تم دمجها
                        string deleteNewQuery = "DELETE FROM tblMain1 WHERE MainID = @newID";
                        using (SqlCommand delCmd = new SqlCommand(deleteNewQuery, con))
                        {
                            delCmd.Parameters.AddWithValue("@newID", MainID);
                            delCmd.ExecuteNonQuery();
                        }
                        MainID = oldMainID; // نستخدم الـ oldMainID للعمليات القادمة
                    }
                    else
                    {
                        // 🔹 لو مافيش فاتورة بنفس الـ taskID، نحدث الحالية عادي
                        string qry = @"
                UPDATE tblMain1
                SET partiesID = @partiesID,
                    taskID = @taskID,
                    TotalWithInterest = @TotalWithInterest,
                    shiftID = @shiftID,
                    priceClear = @TotalWithInterest,
                    InterestAmount = @InterestAmount,
                    PaidAmount = @PaidAmount,
                    [status] = @status,
                    PaymentMethod = @PaymentMethod,
                    total = @total,
                    descountValue = @descountValue
                WHERE MainID = @ID";

                        using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", MainID);
                            cmd.Parameters.AddWithValue("@shiftID", MainClass.shiftID);
                            cmd.Parameters.AddWithValue("@partiesID", partiesID);
                            cmd.Parameters.AddWithValue("@taskID", taskID);

                            cmd.Parameters.AddWithValue("@TotalWithInterest", Convert.ToDouble(string.IsNullOrWhiteSpace(txtClean.Text) ? "0" : txtClean.Text));
                            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(string.IsNullOrWhiteSpace(txtPriceTotal.Text) ? "0" : txtPriceTotal.Text));
                            cmd.Parameters.AddWithValue("@descountValue", Convert.ToDouble(string.IsNullOrWhiteSpace(txtDV.Text) ? "0" : txtDV.Text));

                            cmd.Parameters.AddWithValue("@InterestAmount", 0);
                            cmd.Parameters.AddWithValue("@PaidAmount", 0);
                            cmd.Parameters.AddWithValue("@PaymentMethod", "اجل");
                            cmd.Parameters.AddWithValue("@status", "finshed");

                            cmd.ExecuteNonQuery();
                        }

                    }
                    string updateTaskQuery = "UPDATE Task SET mainID = @MainID WHERE taskID = @taskID";
                    using (SqlCommand updateTaskCmd = new SqlCommand(updateTaskQuery, con))
                    {
                        updateTaskCmd.Parameters.AddWithValue("@MainID", MainID);
                        updateTaskCmd.Parameters.AddWithValue("@taskID", taskID);
                        updateTaskCmd.ExecuteNonQuery();
                    }
                }
                if (MainID <= 0)
                {
                    invoiceCode = GenerateUniqueInvoiceCode();
                    MainID = await CreateInvoiceAsync(invoiceCode);
                }

                qtyStore();
                if (fromReturnsBill) SaveInvoiceAndUpdateStock();

                Notifier.ShowNotification("تم الحفظ", "تم حفظ الفاتورة ,والمنتجات بنجاح ✅");
                dgvMain.Rows.Clear();
                txtPriceTotal.Text = "0";
                txtClean.Text = "0";
                txtDP.Text = "0";
                txtDV.Text = "0";
                isTaskbill = false;
                btnGeneralBill.Checked = true;
                btnCheckout.Visible = true;
                btnEnd.Visible = false;
                txtPartyName.Visible = false;
                txtTaskNumber.Visible = false;
                showPanel.Visible = false;

                invoiceCode = GenerateUniqueInvoiceCode();
                MainID = await CreateInvoiceAsync(invoiceCode);
                fromReturnsBill = false;

                frmShowBackup frmshowBackup = new frmShowBackup();
                frmshowBackup.backupType = "DIFFERENTIAL";
                frmshowBackup.showNotification = false;
                frmshowBackup.ShowDialog(this);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving bill: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
