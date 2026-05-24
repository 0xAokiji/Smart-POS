
using DevExpress.Pdf.Xmp;
using Guna.UI2.WinForms;
using pos.Classes;
using pos.Model;
using pos.Model.Stor;
using pos.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using System.Xml.Linq;
using static pos.GeneralForms.frmMian2;


namespace pos.View
{
    public partial class frmProductView : SampleView, IRefreshableForm
    {

        public bool cancels = true;
        private string position = "";
        private FlowLayoutPanel storPanel;

        private string display;

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;

        public frmProductView()
        {
            InitializeComponent();

            ThemeMode();


            this.ShowInTaskbar = false;


            textSuggester();
            guna2DataGridView2.ContextMenuStrip = contextMenuStrip1;
            CategoryPanel.ContextMenuStrip = contextMenuStrip1;
            this.ContextMenuStrip = contextMenuStrip1;


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

        private async void frmProductView_Load(object sender, EventArgs e)
        {

            int x = this.Size.Width;
            int x2 = txtSearch1.Size.Width;
            int z = (x - x2) / 2;
            txtSearch1.Location = new Point(z, 35);
            position = "show produst";
            txtSearch1.CustomizableEdges.BottomLeft = true;

            GetData();
            componentPanel.Controls.Clear();
            dgvProductes.Columns["dgvName2"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            componentPanel.Controls.Add(CategoryPanel);
            componentPanel.Controls.Add(proPanel);
            componentPanel.Controls.Add(txtCatSearch);

            proPanel.Controls.Add(dgvProductes);

            AddCategory(txtCatSearch.Text);
            ApplyGridStyle(dgvProductes);
            //if (MainClass.role == "مدير")
            //    guna2DataGridView2.Columns["dgvDel"].Visible = true;
            //else
            //    guna2DataGridView2.Columns["dgvDel"].Visible = false;
        }
        private void textSuggester()
        {
            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT pName FROM products", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                    while (reader.Read())
                    {
                        dataSource.Add(reader.GetString(0));
                    }

                    txtSearch1.AutoCompleteCustomSource = dataSource;
                    txtSearch1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtSearch1.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
        }
        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.Visible = true;
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;

            // أحجام الخلايا والهيدر
            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 45;

            // الخطوط
            dgv.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

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

        private async Task sumTotalInStoreAsync()
        {
            decimal totalNewSales = 0m;
            decimal totalUsedSales = 0m;
            decimal grandTotal = 0m;

            string qry = @"
            SELECT 
                SUM(CAST(ts.qtyU1 AS DECIMAL(18,2)) * CAST(p.sellPrice AS DECIMAL(18,2))) AS TotalNewSales,
                SUM(CAST(ts.qtyUsedU1 AS DECIMAL(18,2)) * CAST(p.sellPriceUsed AS DECIMAL(18,2))) AS TotalUsedSales,
                SUM(CAST(ts.qtyU1 AS DECIMAL(18,2)) * CAST(p.sellPrice AS DECIMAL(18,2))) +
                SUM(CAST(ts.qtyUsedU1 AS DECIMAL(18,2)) * CAST(p.sellPriceUsed AS DECIMAL(18,2))) AS GrandTotal
            FROM products p
            JOIN totalStor ts ON ts.pID = p.pID;";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        totalNewSales = reader["TotalNewSales"] != DBNull.Value ? Convert.ToDecimal(reader["TotalNewSales"]) : 0m;
                        totalUsedSales = reader["TotalUsedSales"] != DBNull.Value ? Convert.ToDecimal(reader["TotalUsedSales"]) : 0m;
                        grandTotal = reader["GrandTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrandTotal"]) : 0m;
                    }
                }
            }

            // ⚡ تحديث الـ UI لازم يحصل على الـ UI thread
            this.Invoke((MethodInvoker)(() =>
            {
                txtStoreTotal.Text = grandTotal.ToString("N0");
                txtNew.Text = totalNewSales.ToString("N0");
                txtUsed.Text = totalUsedSales.ToString("N0");
            }));
        }

        public void OnFormShownAgain()
        {

        }
        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();

        public void ReloadData()
        {

            ThemeMode();

            if (openedForms.ContainsKey("frmProductAdd2"))
            {
                var form = openedForms["frmProductAdd2"] as frmProductAdd2;
                //form?.ThemeMode();
            }
            //if (openedForms.ContainsKey("frmpurchasesBill"))
            //{
            //    var form = openedForms["frmpurchasesBill"] as frmpurchasesBill;
            //    form?.ThemeMode();
            //}

            if (storPanel != null)
            {
                foreach (System.Windows.Forms.Control c in storPanel.Controls)
                {
                    if (c is ucStore item)
                    {
                        item.themRefresh();
                    }
                }
            }
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
                        <add key=""DisplayMode"" value=""dgv"" />
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

                if (settings["DisplayMode"] != null &&
                    !string.IsNullOrWhiteSpace(settings["DisplayMode"].Value) &&
                    settings["DisplayMode"].Value.ToLower() != "null")
                {
                    displaymode = settings["DisplayMode"].Value;
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

        // تحديد عدد الصفحة الحالية
        private int currentPage = 0;
        private bool isLoading = false;
        private bool allLoaded = false;
        private static int firstNumber = 0;
        private static bool isFirstTime = true;
        private List<int> proID = new List<int>();
        private string SearchName = "";


        private async Task LoadStorePageAsync(int proNum, string selectedCategory, string searchName)
        {
            if (isLoading || allLoaded) return;

            isLoading = true;
            Debug.WriteLine($"Loading store page: {currentPage}, Items per page: {proNum}");

            var rows = await Task.Run(() => GetStorePage(currentPage, proNum, selectedCategory, searchName));

            if (rows.Count == 0)
            {
                allLoaded = true;
                isLoading = false;
                Debug.WriteLine("All store products loaded.");
                return;
            }
            storPanel.SuspendLayout();

            foreach (var row in rows)
            {
                string sellPrice = GetSellPrice(Convert.ToInt32(row["pID"]));

                byte[] imageData = (byte[])row["pImage"];
                MemoryStream ms = new MemoryStream(imageData);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                var s = new ucStore()
                {
                    id = Convert.ToInt32(row["pID"]),
                    PName = row["pName"].ToString(),
                    pprice = sellPrice,
                    PCategory = row["catName"].ToString(),
                    PImage = image,
                    pQty = row["TotalQty"].ToString(),
                    pWholPrice = row["purPrice"].ToString(),
                };

                s.Size = new Size(260, 170);
                storPanel.Controls.Add(s);

                // الأحداث
                AttachEvents(s, row);
            }

            storPanel.ResumeLayout();
            currentPage++;
            isLoading = false;
        }
        private List<DataRow> GetStorePage(int page, int proNum, string selectedCategory, string searchName)
        {
            if (page == -1)
                page = 0;

            int nextpro = page * proNum + firstNumber;

            string qry = @"
            SELECT 
                MIN(p.pID) AS pID, 
                p.pName, 
                MIN(p.categoryID) AS categoryID, 
                MIN(p.purPrice) AS purPrice, 
                MIN(c.catName) AS catName, 
                MAX(CONVERT(VARBINARY(MAX), p.pImage)) AS pImage,
                ts.qtyU1 AS TotalQty, 
                MIN(p.requestP) AS requestP
            FROM 
                products p 
            JOIN 
                category c ON p.categoryID = c.catID 
            JOIN 
                totalStor ts ON ts.pID = p.pID 
            WHERE 
                (@catName IS NULL OR c.catName LIKE '%' + @catName + '%') AND
                (@searchName IS NULL OR p.pName LIKE '%' + @searchName + '%')
            GROUP BY 
                p.pName, ts.qtyU1
            HAVING 
                ts.qtyU1 >= 0
            ORDER BY 
                MIN(p.pID)
            OFFSET @nextpro ROWS FETCH NEXT @proNum ROWS ONLY";

            DataTable dt = new DataTable();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ اتصال خاص لكل استدعاء
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@catName", string.IsNullOrWhiteSpace(selectedCategory) ? (object)DBNull.Value : selectedCategory);
                cmd.Parameters.AddWithValue("@searchName", string.IsNullOrWhiteSpace(searchName) ? (object)DBNull.Value : searchName);
                cmd.Parameters.AddWithValue("@nextpro", nextpro);
                cmd.Parameters.AddWithValue("@proNum", proNum);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
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




        private string GetSellPrice(int pID)
        {
            string price = "0";
            string qry = @"SELECT sellPrice 
                   FROM products 
                   WHERE pID = @pID";

            using (SqlConnection con = MainClass.GetConnection()) // اتصال جديد لكل استدعاء
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@pID", pID);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                        price = dt.Rows[0]["sellPrice"].ToString();
                }
            }

            return price;
        }


        private void AttachEvents(ucStore s, DataRow row)
        {
            s.onSelectEdit += (ss, ee) =>
            {
                if (MainClass.ProCardEdite)
                {
                    frmBlackout frmBlackout1 = new frmBlackout(this);
                    frmBlackout1.Show();
                    frmBlackout1.Owner = this;
                    frmCategoryCard frm = new frmCategoryCard();
                    frm.Owner = this;
                    frm.id = Convert.ToInt32(row["pID"]);
                    frm.ShowDialog();
                    this.Focus();
                    frmBlackout1.Close();
                }
                else
                {
                    guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                    guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                }
            };

            s.onSelectDel += (ss, ee) =>
            {
                if (guna2MessageDialog2.Show(" هل تريد حذفه؟ ") == DialogResult.Yes)
                {
                    int id = s.id;
                    string pName = MainClass.GetSingleValue<string>(
                        "SELECT pName FROM products WHERE pID = @pID",
                        new Dictionary<string, object> { ["@pID"] = id }
                    );
                    MainClass.SQL("INSERT INTO rconrdEditingPro VALUES(@posName, @editeIn, NULL, @tableName, @typeEdit, @date, @time)", new Hashtable
                    {
                        ["@posName"] = MainClass.USER,
                        ["@editeIn"] = pName,
                        ["@tableName"] = "المنتج",
                        ["@typeEdit"] = "حذف",
                        ["@date"] = DateTime.Today,
                        ["@time"] = DateTime.Now.ToShortTimeString()
                    });

                    MainClass.SQL("UPDATE totalStor SET qty = 0 WHERE pID = @pID", new Hashtable
                    {
                        ["@pID"] = id
                    });

                    storPanel.Controls.Remove(s);
                    s.Dispose();
                }
            };

            s.click1 += (ss, ee) =>
            {
                if (proID.Contains(s.id)) proID.Remove(s.id); else proID.Add(s.id);
            };
            s.click2 += (ss, ee) =>
            {
                if (proID.Contains(s.id)) proID.Remove(s.id); else proID.Add(s.id);
            };
        }

        // Update pWholPrice
        public void UpdateStorePurPricesFromDB()
        {

        }




        // 🔹 متغير لحفظ الفئة المختارة مسبقًا
        private static string selectedCategory = "الكل";

        private void AddCategory(string catName)
        {
            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT catID, catName FROM v_CategoryArabicSorted WHERE catName LIKE '%' + @catName + '%' ORDER BY SortOrder, catName;", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@catName", catName);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }


                CategoryPanel.Controls.Clear();

                // 🔹 زر "الكل"
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
                else
                {
                    allCategoriesButton.FillColor = Color.FromArgb(1, 95, 95);
                    allCategoriesButton.ForeColor = Color.White;
                    allCategoriesButton.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
                    allCategoriesButton.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
                    allCategoriesButton.CheckedState.ForeColor = Color.FromArgb(51, 51, 51);
                    allCategoriesButton.BorderColor = Color.Gray;
                }

                allCategoriesButton.Checked = (selectedCategory == "الكل");
                allCategoriesButton.AutoRoundedCorners = true;
                allCategoriesButton.Click += (s, e) =>
                {
                    selectedCategory = "الكل";
                    b_clik(s, e);
                };

                Guna2Separator separator = new Guna2Separator();
                separator.Size = new Size(160, 12);
                separator.FillThickness = 3;

                CategoryPanel.Controls.Add(allCategoriesButton);
                CategoryPanel.Controls.Add(separator);

                // 🔹 إضافة باقي الفئات
                foreach (DataRow row in dt.Rows)
                {
                    string catName1 = row["catName"].ToString();
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.Size = new Size(160, 35);
                    b.AutoRoundedCorners = true;
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = catName1;
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
                    else
                    {
                        b.FillColor = Color.FromArgb(1, 95, 95);
                        b.ForeColor = Color.White;
                        b.CheckedState.FillColor = Color.FromArgb(136, 214, 218);
                        b.CheckedState.ForeColor = Color.FromArgb(51, 51, 51);
                        b.CheckedState.BorderColor = Color.FromArgb(1, 95, 95);
                        b.BorderColor = Color.Gray;
                    }

                    b.Checked = (selectedCategory == catName1);
                    b.Click += (s, e) =>
                    {
                        selectedCategory = catName1;
                        b_clik(s, e);
                    };

                    CategoryPanel.Controls.Add(b);
                }
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
            }
        }



        private string lastSelectedCategory = string.Empty;
        private string selectedCategoryName;

        private async void b_clik(object? sender, EventArgs e)
        {
            var b = (Guna.UI2.WinForms.Guna2Button)sender;
            txtSearch1.Text = string.Empty;

            selectedCategoryName = (b.Text == "الكل") ? string.Empty : b.Text;

            // إذا ضغط على نفس الفئة، لا نفعل شيئاً
            if (selectedCategoryName == lastSelectedCategory)
                return;

            // حدّث الفئة الأخيرة
            lastSelectedCategory = selectedCategoryName;

            //currentPage = 0;
            //firstNumber = 0;
            //isFirstTime = true;
            //allLoaded = false;
            //storPanel.Controls.Clear();
            //storPanel.Refresh();
            //// تحميل أول دفعة
            //setFlowPanelStore();
            //await LoadStorePageAsync(36, selectedCategoryName, string.Empty);
            ResetData();
            dgvProductes.Rows.Clear();
            dgvProductes.DataSource = null;

            btnEdite.Visible = true;
            btnQty.Visible = true;
            btnDelete.Visible = true;

            await GetData(selectedCategoryName); // 🔥 استخدم await مباشر

            proPanel.Controls.Clear();
            proPanel.Controls.Add(dgvProductes);
            SetupGrid();

        }

        private System.Windows.Forms.Timer ScrollTimer = new System.Windows.Forms.Timer();



        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            int currentValue = storPanel.VerticalScroll.Value;

            int countPerRow = CalculateItemsPerRow();

            // لو كان قرب نهاية السكول وتحميل مش شغال ومفيش تحميل خلاص
            if (!isLoading && !allLoaded &&
                currentValue + storPanel.ClientSize.Height >= storPanel.VerticalScroll.Maximum - 10)
            {
                _ = LoadStorePageAsync(countPerRow, selectedCategoryName, txtSearch1.Text);
            }
        }

        private int CalculateItemsPerRow()
        {

            // عرض العنصر نفسه
            int itemWidth = 260;

            // خذ Margin الأيسر والأيمن من أول عنصر في الـ pool
            int elementMargin = 0;
            if (storPanel.Controls.OfType<ucStore>().FirstOrDefault() is ucStore sample)
            {
                elementMargin = sample.Margin.Left + sample.Margin.Right;
            }

            // العرض الصافي للـ panel بعد طرح الـ Padding الداخلي
            int panelContentWidth = storPanel.ClientSize.Width
                                  - storPanel.Padding.Left
                                  - storPanel.Padding.Right;

            // العرض الإجمالي لكل عنصر مع مسافته
            int totalItemWidth = itemWidth + elementMargin;

            // احسب عدد العناصر
            int itemsPerRow = panelContentWidth / totalItemWidth;
            return Math.Max(1, itemsPerRow);
        }

        private void setFlowPanelStore()
        {
            // حذف البانل السابق لو موجود
            if (storPanel != null)
            {
                proPanel.Controls.Remove(storPanel);
                storPanel.Dispose();
                storPanel = null;
            }

            // إنشاء بانل جديد
            storPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = Padding.Empty,
                RightToLeft = RightToLeft.Yes,
                BackColor = backgroundPrimary

            };

            // إضافته للحاوية
            proPanel.Controls.Add(storPanel);

            // عرض العنصر المتوقع
            const int itemWidth = 260;

            // عند إضافة أول عنصر، احسب الـ Margin المثالي
            storPanel.ControlAdded += (s, ev) =>
            {
                if (storPanel.Controls.Count == 1)
                {
                    // حساب عدد العناصر في الصف
                    int itemsPerRow = CalculateItemsPerRow();

                    // حساب المساحة المتبقية
                    int panelWidth = storPanel.ClientSize.Width;
                    int totalItemsWidth = itemsPerRow * itemWidth;
                    int leftover = (panelWidth - totalItemsWidth) - 20;

                    int marginPerSide = leftover > 0
                        ? leftover / (itemsPerRow * 2)
                        : 0;

                    // تعيين Margin لكل العناصر الموجودة
                    foreach (ucStore card in storPanel.Controls.OfType<ucStore>())
                        card.Margin = new Padding(marginPerSide, 5, marginPerSide, 5);

                    // تعيين Margin لأي عنصر جديد يُضاف لاحقًا
                    storPanel.ControlAdded += (s2, ev2) =>
                    {
                        if (ev2.Control is ucStore newCard)
                            newCard.Margin = new Padding(marginPerSide, 5, marginPerSide, 5);
                    };
                }
            };
        }


        private async void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            await LoadStorePageAsync(36, String.Empty, txtSearch1.Text);

            //GetData();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

        }

        public void AddControls(Form f)
        {
            // تحقق هل الفورم موجود بالفعل في mainpanel
            foreach (Control ctrl in mainpanel.Controls)
            {
                if (ctrl is Form existingForm && existingForm.Name == f.Name)
                {
                    existingForm.BringToFront(); // اعرضه في الواجهة فقط
                    return;
                }
            }

            // لو مش موجود، أضفه
            mainpanel.Controls.Clear(); // امسح الموجود (أو علّق هذا السطر لو حابب تحتفظ بالباقي)
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(f);
            f.Show();

            // خزنه في openedForms
            if (openedForms.ContainsKey(f.Name))
            {
                openedForms[f.Name] = f; // تحديث الفورم الموجود
            }
            else
            {
                openedForms.Add(f.Name, f); // إضافة جديد
            }
        }



        bool check = true;
        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            check = false;


        }

        private void btnPervious_Click(object sender, EventArgs e)
        {
            check = true;
            componentPanel.Controls.Clear();
            storPanel.Dock = DockStyle.Fill;
            componentPanel.Controls.Add(storPanel);
            componentPanel.Controls.Add(txtCatSearch);

            btnPervious.Visible = false;

        }

        private System.Windows.Forms.Timer searchTimer;

        private async void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            // ✅ تحقق إذا التكست فاضي
            string searchText = txtSearch1.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                txtSearch1.TextAlign = HorizontalAlignment.Left;
            }
            else
            {
                // ✅ تحقق من أول حرف بطريقة آمنة
                char firstChar = searchText[0];
                txtSearch1.TextAlign = IsArabic(firstChar) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }

            // ✅ أوقف المؤقت القديم لو شغال
            if (searchTimer != null)
            {
                searchTimer.Stop();
                searchTimer.Dispose();
            }

            // ✅ إعداد مؤقت جديد
            searchTimer = new System.Windows.Forms.Timer
            {
                Interval = 500 // نصف ثانية
            };

            searchTimer.Tick += async (s, args) =>
            {
                searchTimer.Stop();
                searchTimer.Dispose();

                // 🔄 إعادة ضبط البيانات
                ResetData();
                dgvProductes.Rows.Clear();
                dgvProductes.DataSource = null;

                btnEdite.Visible = true;
                btnQty.Visible = true;
                btnDelete.Visible = true;

                if (string.IsNullOrEmpty(searchText))
                    await GetData(lastSelectedCategory);
                else
                    await GetData(string.Empty);
                // ✅ تحديث واجهة العرض
                proPanel.Controls.Clear();
                proPanel.Controls.Add(dgvProductes);
                SetupGrid();
            };

            searchTimer.Start();
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private void اضافةمنتجToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            //frmBlackout frmBlackout = new frmBlackout(this);
            //frmBlackout.Owner = this;

            //frmBlackout.Show();

            //frm = new frmpurchasesBill();
            ////frm.FormClosing += new FormClosingEventHandler(frmpurchasesBill_FormClosing);
            //frm.ButtonClicked += Form_ButtonClicked;
            //frm.ShowDialog();
            //this.Focus();

            //frmBlackout.Close();

        }

        private async void عرضالمنتجاتToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            refrish();
        }
        private async void refrish()
        {
            position = "show produst";


            btnPriceIncrease1.Visible = true;
            txtSearch1.Visible = true;
            btnEdite.Visible = true;
            btnQty.Visible = true;
            btnDelete.Visible = true;
            txtStoreTotal.Visible = true;
            txtUsed.Visible = true;
            txtNew.Visible = true;
            btnNew.Visible = true;
            btnUsed.Visible = true;
            btnTotal.Visible = true;

            mainpanel.Controls.Clear();

            // 🔹 إضافة البانل
            mainpanel.Controls.Add(componentPanel);

            // 🔹 إضافة كل الأزرار والتكست بوكس
            mainpanel.Controls.Add(btnPriceIncrease1);
            mainpanel.Controls.Add(txtSearch1);
            mainpanel.Controls.Add(btnEdite);
            mainpanel.Controls.Add(btnQty);
            mainpanel.Controls.Add(btnDelete);
            mainpanel.Controls.Add(txtStoreTotal);
            mainpanel.Controls.Add(txtUsed);
            mainpanel.Controls.Add(txtNew);
            mainpanel.Controls.Add(btnNew);
            mainpanel.Controls.Add(btnUsed);
            mainpanel.Controls.Add(btnTotal);
            mainpanel.Controls.Add(txtProductInfo);


            componentPanel.Controls.Clear();
            //componentPanel.Controls.Add(dgvProductes);
            dgvProductes.Columns["dgvName2"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            componentPanel.Controls.Add(CategoryPanel);
            componentPanel.Controls.Add(proPanel);
            componentPanel.Controls.Add(txtCatSearch);

            proPanel.Controls.Add(dgvProductes);
            AddCategory(txtCatSearch.Text);

            ResetData();
            dgvProductes.Rows.Clear();
            dgvProductes.DataSource = null;

            await GetData(selectedCategoryName); // 🔥 استخدم await مباشر

            proPanel.Controls.Clear();
            proPanel.Controls.Add(dgvProductes);
            SetupGrid();

            dgvProductes.Visible = true;
            dgvProductes.Dock = DockStyle.Fill;
            dgvProductes.BringToFront();
            dgvProductes.AutoGenerateColumns = false;
            dgvProductes.AllowUserToAddRows = false;
            dgvProductes.ReadOnly = true;
            dgvProductes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductes.MultiSelect = false;
            dgvProductes.RowHeadersVisible = false;
            dgvProductes.AllowUserToResizeRows = false;


            // خلى الصفوف تاخد ارتفاع مناسب للخط
            dgvProductes.RowTemplate.Height = 35;
            dgvProductes.ColumnHeadersHeight = 45;

            dgvProductes.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgvProductes.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

        }

        private void عرضسجلاتالموردينToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //txtSearch1.Visible = false;
            //frmRegisterS frmRegisterS = new frmRegisterS();
            //AddControls(frmRegisterS);
        }

        private async void عرضالمنتجاتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            position = "show produst";
            btnPriceIncrease1.Visible = true;
            componentPanel.Controls.Clear();
            componentPanel.Controls.Add(CategoryPanel);
            storPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            storPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            storPanel.Location = new Point(this.ClientSize.Width - storPanel.Width - 165, storPanel.Top);
            componentPanel.Controls.Add(storPanel);
            componentPanel.Controls.Add(txtCatSearch);

            AddCategory(txtCatSearch.Text);

            await LoadStorePageAsync(36, String.Empty, txtSearch1.Text);

            //GetData();
        }

        private void اضافةمنتجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmBlackout frmBlackout = new frmBlackout(this);
            //frmBlackout.Owner = this;

            //frmBlackout.Show();

            //frm = new frmpurchasesBill();
            ////frm.FormClosing += new FormClosingEventHandler(frmpurchasesBill_FormClosing);
            //frm.ButtonClicked += Form_ButtonClicked;
            //frm.ShowDialog();
            //this.Focus();

            //frmBlackout.Close();
        }

        private void سجلالموردينToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void اضافةموردToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSupplier supplier = new frmSupplier();
            supplier.ShowDialog();
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

        private void عرضالاصنافToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowCategories)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            position = "view cat";

            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;
            btnDelete.Visible = false;

            mainpanel.Controls.Clear();

            frmCategoryViewMain frmCategoryView = new frmCategoryViewMain();
            AddControls(frmCategoryView);
        }
        private int billID = 0;
        private frmpurchasesBill frm;
        //private frmpurchaseAdd frmp;
        private frmProductAdd2 frmP2;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private bool cancel = false;
        private int name;


        private Panel billPanel = new Panel();
        private Panel addProductPanel = new Panel();

        private void addBil()
        {
            btnEdite.Visible = false;
            btnDelete.Visible = false;
            btnQty.Visible = false;
            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;


            GetData();
            mainpanel.Controls.Clear();
            addProductPanel.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(addProductPanel);

            frm = new frmpurchasesBill();
            frmProductAdd2 frm2 = new frmProductAdd2(this);

            AddControls(frm, billPanel);

            frm2.newBill += (s, e) => frm.newBill();

            AddControls(frm2, addProductPanel);

        }

        public void AddControls(Form f, Panel panel)
        {
            // هل الفورم موجود بالفعل في البانل؟
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is Form existingForm && existingForm.Name == f.Name)
                {
                    existingForm.BringToFront(); // اجلبه للواجهة فقط
                    return;
                }
            }

            // لو مش موجود، أضفه للبانل
            panel.Controls.Clear(); // احذف السطر دا لو مش عايز تحذف الفورمات السابقة
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            panel.Controls.Add(f);
            f.Show();

            // سجل الفورم في القاموس أو حدّثه
            if (openedForms.ContainsKey(f.Name))
            {
                openedForms[f.Name] = f;
            }
            else
            {
                openedForms.Add(f.Name, f);
            }
        }




        private void toolStripButton3_Click(object sender, EventArgs e)
        {


        }
        //private void Form_ButtonClicked(object sender, frmpurchasesBill e)
        //{
        //    cancel = false;
        //    name = 1;
        //    foreach (Form form in Application.OpenForms)
        //    {
        //        if (form.Name == "frmpurchasesBill")
        //        {
        //            form.Close();
        //            break;
        //        }
        //    }
        //    frmBlackout frm = new frmBlackout(this);
        //    frm.Owner = this;
        //    frm.Show();
        //    frmP2 = new frmProductAdd2(this);
        //    frmP2.bill = true;
        //    //frmP2.bID = billID;
        //    cancel = false;
        //    frmP2.ButtonHide += Form3_ButtonHide;
        //    frmP2.ShowDialog();
        //    this.Focus();
        //    frm.Close();


        //}
        private void SecondForm_ValueSubmitted(object sender, int value)
        {
            billID = value;
        }
        private void Form2_ButtonCancel(object sender, EventArgs e)
        {
            cancel = true;
            cancels = true;
        }
        private void Form2_ButtonHide(object sender, EventArgs e)
        {
            cancel = false;
            cancels = true;
            name = 1;
        }
        private void Form3_ButtonHide(object sender, EventArgs e)
        {
            cancel = false;
            cancels = true;
            name = 2;
        }

        private void btnPriceIncrease1_Click(object sender, EventArgs e)
        {

            if (MainClass.IncreasePrice)
            {
                if (proID.Count == 0)
                {
                    guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                    guna2MessageDialog2.Show("حدد المنتجات اولا");

                    return;
                }
                frmProductView mainForm = this.FindForm() as frmProductView;

                if (mainForm != null)
                {
                    //frmBlackout frmBlackout = new frmBlackout(this);
                    //frmBlackout.Show(mainForm);
                    //frmBlackout.Owner = mainForm;

                    frmPriceIncrease frm = new frmPriceIncrease(proID);
                    DialogResult result = frm.ShowDialog(mainForm);

                    if (result == DialogResult.OK)
                    {
                        refrish();



                    }


                    //frmBlackout.Close();
                    mainForm.Focus();
                    checkdgvstate();

                }
                else
                {
                    // معالجة الحالة لو ما لقاها (اختياري)
                }
            }
            else
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }




        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void اضاقةمخزنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.AddStore)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = this;
            frmAddStore frmAddStore = new frmAddStore();
            frmAddStore.Owner = this;
            frmAddStore.ShowDialog();
            frmBlackout.Close();
        }

        private void اضافةمنتجبدونفاتورهToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmP2 = new frmProductAdd2(this);
            frmP2.bill = false;
            frmP2.billID = billID;
            cancel = false;
            frmP2.btnExit.Visible = true;
            frmP2.ButtonHide += Form3_ButtonHide;
            frmP2.ShowDialog();
            this.Focus();
        }

        private void اضافةمنتجبدونفاتورةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlackout frm = new frmBlackout(this);
            frm.Owner = this;
            frm.Show();
            frmP2 = new frmProductAdd2(this);
            frmP2.bill = false;

            cancel = false;
            frmP2.ButtonHide += Form3_ButtonHide;
            frmP2.ShowDialog();
            this.Focus();
            frm.Close();
        }

        private void اضافةكرتمنتججديدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainClass.ProCardAdd)
            {
                frmBlackout frmBlackout1 = new frmBlackout(this);
                frmBlackout1.Show();
                frmBlackout1.Owner = this;
                frmCategoryCard frm = new frmCategoryCard();
                frm.Owner = this;
                frm.ShowDialog();
                this.Focus();
                frmBlackout1.Close();
            }
            else
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        private void اضافةكرتمنتججديدToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MainClass.ProCardAdd)
            {
                frmBlackout frmBlackout1 = new frmBlackout(this)
                {
                    Owner = this // ✅ خليها قبل Show
                };
                frmBlackout1.Show();

                using (frmCategoryCard frm = new frmCategoryCard())
                {
                    frm.Owner = this;
                    frm.ShowDialog();
                }

                // ✅ ركز الفورم الرئيسي بعد قفل الحوار
                this.Focus();

                // ✅ اقفل blackout بأمان
                if (!frmBlackout1.IsDisposed)
                    frmBlackout1.Close();

            }
            else
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        private async void تحديثToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refrish();

        }

        private async void عرضجمToolStripMenuItem_Click(object sender, EventArgs e)
        {
            position = "view card";
            btnPriceIncrease1.Visible = false;
            guna2DataGridView2.Visible = true;
            componentPanel.Controls.Clear();
            guna2DataGridView2.Dock = DockStyle.Fill;
            guna2DataGridView2.Location = new Point(this.ClientSize.Width - proPanel.Width - 165, proPanel.Top);
            componentPanel.Controls.Add(guna2DataGridView2);
            componentPanel.Controls.Add(txtCatSearch);

            //await LoadStorePageAsync(36, String.Empty, txtSearch1.Text);

            //GetData();
        }

        private async void guna2DataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (guna2DataGridView2.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                // تعيين الـ Parent للنموذج الرئيسي
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;

                if (guna2MessageDialog2.Show("هل أنت متأكد من رغبتك في حذف هذا المنتج؟\nسيؤدي ذلك إلى حذف جميع السجلات القديمة المتعلقة به، سواء من عمليات البيع أو الشراء.") == DialogResult.Yes)
                {
                    int pID = Convert.ToInt32(guna2DataGridView2.CurrentRow.Cells["dgvid"].Value);
                    string qry = @"DELETE FROM products WHERE pID = @id; 
                                   DELETE FROM totalStor WHERE pID = @id";
                    Hashtable ht = new Hashtable();
                    ht.Add("@id", pID);
                    MainClass.SQL(qry, ht);

                    Notifier.ShowNotification("Done ✅", "تم الحذف بنجاح");

                    await LoadStorePageAsync(36, String.Empty, txtSearch1.Text);

                    //GetData();

                }

            }
            if (e.ColumnIndex == guna2DataGridView2.Columns["dgvName"].Index && e.RowIndex >= 0)
            {
                if (MainClass.ProCardEdite)
                {
                    frmBlackout frmBlackout1 = new frmBlackout(this);
                    frmBlackout1.Show();
                    frmBlackout1.Owner = this;
                    frmCategoryCard frm = new frmCategoryCard();
                    frm.Owner = this;
                    frm.id = Convert.ToInt32(guna2DataGridView2.CurrentRow.Cells["dgvid"].Value);
                    frm.ShowDialog();
                    this.Focus();
                    frmBlackout1.Close();
                }
                else
                {
                    guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                    guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

                }
            }

        }

        private async void txtSearch1_IconRightClick(object sender, EventArgs e)
        {
            ResetData(); // 🔥 دالة جديدة لإعادة تهيئة كل شيء

            dgvProductes.Rows.Clear();
            dgvProductes.DataSource = null;

            btnEdite.Visible = true;
            btnQty.Visible = true;
            btnDelete.Visible = true;

            await GetData(null); // 🔥 استخدم await مباشر

            proPanel.Controls.Clear();
            proPanel.Controls.Add(dgvProductes);
            SetupGrid();
        }
        private void ResetData()
        {
            proPanel.Controls.Clear();
            currentPage = 0;
            firstNumber = 0;
            allLoaded = false;
            isLoading = false;
            hasMoreData = true;
        }

        private void SetupGrid()
        {
            dgvProductes.Visible = true;
            dgvProductes.Dock = DockStyle.Fill;
            dgvProductes.BringToFront();
            dgvProductes.AutoGenerateColumns = false;
            dgvProductes.AllowUserToAddRows = false;
            dgvProductes.ReadOnly = true;
            dgvProductes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductes.MultiSelect = false;
            dgvProductes.RowHeadersVisible = false;
            dgvProductes.AllowUserToResizeRows = false;

            // 🔹 خلي البوردر ستايل Single
            dgvProductes.CellBorderStyle = DataGridViewCellBorderStyle.Single;

        }

        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            textColor2 = MainClass.TextColor3;
            checkedFillColor = MainClass.CheckedFillColor;
            checkedForeColor = MainClass.CheckedForeColor;
        }
        private void ThemeMode()
        {

            if (MainClass.ThemeMode == "dark")
            {
                btnPriceIncrease1.Image = Properties.Resources.signboard_Dark;

                AddItem.Image = Properties.Resources.add_store_Dark;
                toolStripProduct.Image = Properties.Resources.productes_store_Dark;
                ToolStripSuplieser.Image = Properties.Resources.supplier_store_Dark;
                ToolStripCategores.Image = Properties.Resources.categories_store_Dark;
                ToolStripAddStore.Image = Properties.Resources.store_add_Dark;
                ToolStripEditeRecored.Image = Properties.Resources.report_store_Dark;

                txtSearch1.IconRight = Properties.Resources.search_Dark;

                btnEdite.Image = Properties.Resources.edit_dark2;


            }
            else if (MainClass.ThemeMode == "light")
            {
                //btnPriceIncrease1.Image = Properties.Resources.signboard_Light;

                //AddItem.Image = Properties.Resources.add_store_Light;
                //toolStripProduct.Image = Properties.Resources.productes_store_Light;
                //ToolStripSuplieser.Image = Properties.Resources.supplier_store_Light;
                //ToolStripCategores.Image = Properties.Resources.categories_store_Light;
                //ToolStripAddStore.Image = Properties.Resources.store_add_Light;
                //ToolStripEditeRecored.Image = Properties.Resources.report_store_Light;

                //txtSearch1.IconRight = Properties.Resources.search_ligh;

                btnPriceIncrease1.Image = Properties.Resources.signboard_Dark;

                AddItem.Image = Properties.Resources.add_store_Dark;
                toolStripProduct.Image = Properties.Resources.productes_store_Dark;
                ToolStripSuplieser.Image = Properties.Resources.supplier_store_Dark;
                ToolStripCategores.Image = Properties.Resources.categories_store_Dark;
                ToolStripAddStore.Image = Properties.Resources.store_add_Dark;
                ToolStripEditeRecored.Image = Properties.Resources.report_store_Dark;

                txtSearch1.IconRight = Properties.Resources.search_Dark;

                btnEdite.Image = Properties.Resources.edit_dark2;
            }

            ThemeColor();

            this.BackColor = backgroundPrimary;

            btnPriceIncrease1.BackColor = backgroundPrimary;
            btnPriceIncrease1.FillColor = backgroundPrimary;
            btnPriceIncrease1.FillColor2 = backgroundPrimary;
            btnPriceIncrease1.ForeColor = textColor2;

            //Panels
            componentPanel.BackColor = backgroundPrimary;
            proPanel.BackColor = backgroundPrimary;
            CategoryPanel.BackColor = backgroundPrimary;
            menuStrip.BackColor = checkedFillColor;
            addProductPanel.BackColor = backgroundPrimary;
            billPanel.BackColor = backgroundPrimary;

            //Text box
            txtSearch1.BackColor = backgroundPrimary;
            txtSearch1.ForeColor = textColor;
            txtSearch1.BorderColor = checkedFillColor;
            txtSearch1.FillColor = backgroundPrimary;

            //labels 
            AddItem.ForeColor = textColor2;
            toolStripProduct.ForeColor = textColor2;
            ToolStripSuplieser.ForeColor = textColor2;
            ToolStripCategores.ForeColor = textColor2;
            ToolStripAddStore.ForeColor = textColor2;
            ToolStripEditeRecored.ForeColor = textColor2;

            ////-> datagride view 
            //dgvProductes.BackgroundColor = backgroundPrimary;
            //dgvProductes.GridColor = backgroundPrimary;

            //dgvProductes.DefaultCellStyle.BackColor = backgroundPrimary;
            //dgvProductes.DefaultCellStyle.ForeColor = textColor;
            //dgvProductes.DefaultCellStyle.SelectionBackColor = checkedFillColor;
            //dgvProductes.DefaultCellStyle.SelectionForeColor = textColor2;

            //dgvProductes.ColumnHeadersDefaultCellStyle.BackColor = backgroundSecondary;
            //dgvProductes.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            //dgvProductes.ColumnHeadersDefaultCellStyle.SelectionBackColor = checkedFillColor;
            //dgvProductes.ColumnHeadersDefaultCellStyle.SelectionForeColor = textColor2;

            //dgvProductes.RowsDefaultCellStyle.BackColor = backgroundPrimary;
            //dgvProductes.AlternatingRowsDefaultCellStyle.BackColor = backgroundPrimary;
            //dgvProductes.RowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
            //dgvProductes.RowsDefaultCellStyle.ForeColor = textColor;
            //dgvProductes.RowsDefaultCellStyle.SelectionForeColor = textColor2;

            // Buttons
            btnEdite.BackColor = backgroundPrimary;
            btnEdite.FillColor = checkedFillColor;
            btnEdite.ForeColor = textColor2;

            btnDelete.BackColor = backgroundPrimary;
            btnDelete.FillColor = checkedFillColor;
            btnDelete.ForeColor = textColor2;

            //dgvProductes.EnableHeadersVisualStyles = false;
        }
        //private void ClipControlRegion(System.Windows.Forms.Control control, string direction, int cutSize)
        //{
        //    Rectangle rect = control.ClientRectangle;

        //    switch (direction.ToLower())
        //    {
        //        case "top":
        //            rect.Y += cutSize;
        //            rect.Height -= cutSize;
        //            break;
        //        case "bottom":
        //            rect.Height -= cutSize;
        //            break;
        //        case "left":
        //            rect.X += cutSize;
        //            rect.Width -= cutSize;
        //            break;
        //        case "right":
        //            rect.Width -= cutSize;
        //            break;
        //        default:
        //            throw new ArgumentException("Direction must be: top, bottom, left, or right.");
        //    }

        //    GraphicsPath path = new GraphicsPath();
        //    path.AddRectangle(rect);
        //    control.Region = new Region(path);
        //}

        private void frmProductView_Resize(object sender, EventArgs e)
        {
            //ClipControlRegion(CategoryPanel, "right", 10);
            //ClipControlRegion(proPanel, "left", 17);
        }

        int pageSize = 30;
        bool hasMoreData = true;

        // 🔥 متغيرات Global لتخزين الفلاتر
        private string currentCategoryValue = null;
        private string currentSearchValue = null;

        public async Task GetData(string categoryValue = null)
        {
            if (isLoading || !hasMoreData)
                return;

            isLoading = true;

            try
            {
                // ✅ لو فيه categoryValue جديدة، حدث المتغير
                if (categoryValue != null)
                    currentCategoryValue = categoryValue;

                // ✅ دايمًا خزن البحث الحالي
                currentSearchValue = string.IsNullOrWhiteSpace(txtSearch1.Text) ? null : txtSearch1.Text.Trim();

                string qry = @"
        SELECT q.*, 
               CASE WHEN u.uName IS NULL OR u.uName = '' 
                    THEN N'غير محدد' 
                    ELSE u.uName 
               END AS uniteName
        FROM (
            SELECT 
                MIN(p.pID) AS pID, 
                CASE WHEN MIN(p.pName) IS NULL OR MIN(p.pName) = '' THEN N'غير محدد' ELSE MIN(p.pName) END AS pName,
                CASE WHEN MIN(p.pCode) IS NULL OR MIN(p.pCode) = '' THEN N'غير محدد' ELSE MIN(p.pCode) END AS pCode,
                CASE WHEN MIN(p.shorcut) IS NULL OR MIN(p.shorcut) = '' THEN N'غير محدد' ELSE MIN(p.shorcut) END AS shorcut,
                CASE WHEN MIN(c.catName) IS NULL OR MIN(c.catName) = '' THEN N'غير محدد' ELSE MIN(c.catName) END AS catName,
                CASE WHEN MIN(p.compName) IS NULL OR MIN(p.compName) = '' THEN N'غير محدد' ELSE MIN(p.compName) END AS compName,
                CASE WHEN MIN(p.ProductInfo) IS NULL OR MIN(p.ProductInfo) = '' THEN N'لم يتم اضافة بيانات استخدام لهذا المنتج' ELSE MIN(p.ProductInfo) END AS ProductInfo,

                CASE WHEN ts.qtyU1 IS NULL OR ts.qtyU1 = 0 THEN N'غير متوفر' ELSE CAST(ts.qtyU1 AS NVARCHAR) END AS TotalQtyNew,
                CASE WHEN ts.qtyUsedU1 IS NULL OR ts.qtyUsedU1 = 0 THEN N'غير متوفر' ELSE CAST(ts.qtyUsedU1 AS NVARCHAR) END AS TotalQtyUsed,

                CASE WHEN MIN(p.sellPrice) IS NULL OR MIN(p.sellPrice) = 0 THEN N'غير متوفر' ELSE CAST(MIN(p.sellPrice) AS NVARCHAR) END AS sellPrice,
                CASE WHEN MIN(p.sellPriceUsed) IS NULL OR MIN(p.sellPriceUsed) = 0 THEN N'غير متوفر' ELSE CAST(MIN(p.sellPriceUsed) AS NVARCHAR) END AS sellPriceUsed,

                CASE WHEN MIN(p.purPrice) IS NULL OR MIN(p.purPrice) = 0 THEN N'غير متوفر' ELSE CAST(MIN(p.purPrice) AS NVARCHAR) END AS purPrice,
                CASE WHEN MIN(p.purUsedPrice) IS NULL OR MIN(p.purUsedPrice) = 0 THEN N'غير متوفر' ELSE CAST(MIN(p.purUsedPrice) AS NVARCHAR) END AS purUsedPrice,
                CAST(ISNULL(ts.qtyU1,0) * ISNULL(MIN(p.sellPrice),0) 
                    + ISNULL(ts.qtyUsedU1,0) * ISNULL(MIN(p.sellPriceUsed),0) AS DECIMAL(18,2)) AS TotalValue,

                p.idUnite1
            FROM 
                products p 
            JOIN 
                category c ON p.categoryID = c.catID 
            JOIN 
                totalStor ts ON ts.pID = p.pID 
            WHERE
                (@catName IS NULL OR @catName = '' OR c.catName LIKE '%' + @catName + '%')
                AND
                (@searchName IS NULL OR @searchName = '' 
                    OR p.pName LIKE '%' + @searchName + '%'
                    OR p.pNewBarode LIKE '%' + @searchName + '%'
                    OR p.pUsedBarode LIKE '%' + @searchName + '%')
            GROUP BY 
                p.pName, ts.qtyU1, ts.qtyUsedU1, p.idUnite1
        ) q
        LEFT JOIN untits u ON u.uID = q.idUnite1
        ORDER BY q.pName ASC
        OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

                int offset = currentPage * pageSize;

                SqlParameter[] parameters = {
                    new SqlParameter("@catName", (object)currentCategoryValue ?? DBNull.Value),
                    new SqlParameter("@searchName", (object)currentSearchValue ?? DBNull.Value),
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize)
                };

                DataTable dt = await Task.Run(() => LoadDataReturn(qry, parameters));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                int rowIndex = dgvProductes.Rows.Count + 1;

                foreach (DataRow row in dt.Rows)
                {
                    decimal.TryParse(row["TotalValue"]?.ToString(), out decimal totalValue);

                    dgvProductes.Rows.Add(
                        rowIndex++,
                        false,
                        row["pID"],
                        row["pName"],
                        row["shorcut"],
                        row["catName"],
                        row["compName"],
                        row["pCode"],
                        row["uniteName"],
                        row["TotalQtyNew"],
                        row["TotalQtyUsed"],
                        row["sellPrice"],
                        row["sellPriceUsed"],
                        row["purPrice"],
                        row["purUsedPrice"],
                        totalValue.ToString("N0"),
                        row["ProductInfo"]
                    );
                }

                currentPage++;
                SetupDataGridView();
                await sumTotalInStoreAsync();

            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
            }
            finally
            {
                isLoading = false;
            }
        }

        private void SetupDataGridView()
        {
            // 🔹 إعدادات الصفوف والعناوين
            dgvProductes.RowTemplate.Height = 35;
            dgvProductes.ColumnHeadersHeight = 45;

            dgvProductes.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgvProductes.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

            // 🔹 الأعمدة: حجم ثابت + منع الترتيب
            foreach (DataGridViewColumn col in dgvProductes.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                col.SortMode = DataGridViewColumnSortMode.NotSortable; // 🔥 منع الترتيب
            }

            // ✅ لف النص في العمود المطلوب
            dgvProductes.Columns["dgvName2"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            // ✅ تحسين مظهر النص
            dgvProductes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }



        public static DataTable LoadDataReturn(string qry, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدام GetConnection
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }



        private async void dgvProductes_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (dgvProductes.FirstDisplayedScrollingRowIndex + dgvProductes.DisplayedRowCount(false) >= dgvProductes.RowCount)
                {
                    await GetData(); // 🔥 جلب البيانات
                }
            }
        }

        private void dgvProductes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //checkdgvstate();
            if (e.RowIndex >= 0)
            {
                // جلب الحالة الحالية للعمود
                bool currentVisible = dgvProductes.Columns["dgvSelect"].Visible;

                // تغيير حالة الظهور (Toggle)
                dgvProductes.Columns["dgvSelect"].Visible = !currentVisible;

                if (currentVisible)
                {
                    // لو هيتم إخفاء العمود، نخلي كل القيم فيه false
                    foreach (DataGridViewRow row in dgvProductes.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            row.Cells["dgvSelect"].Value = false;
                            proID.Clear();

                            btnDelete.Enabled = false;
                            btnEdite.Enabled = false;
                            btnQty.Enabled = false;
                        }
                    }
                    proID.Clear();

                }

            }

        }

        private void dgvProductes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = dgvProductes.Columns[e.ColumnIndex].Name;

                if (dgvProductes.Columns["dgvSelect"].Visible)
                {
                    // عكس القيمة الحالية للتحديد
                    var cell = dgvProductes.Rows[e.RowIndex].Cells["dgvSelect"];
                    bool isChecked = cell.Value != null && (bool)cell.Value;
                    bool newValue = !isChecked;
                    cell.Value = newValue;


                    // قراءة قيمة pID من العمود dgvproID
                    object idValue = dgvProductes.Rows[e.RowIndex].Cells["dgvproID"].Value;

                    if (idValue != null && int.TryParse(idValue.ToString(), out int pID))
                    {
                        if (newValue)
                        {
                            // إضافة pID إذا غير موجود في القائمة
                            if (!proID.Contains(pID))
                                proID.Add(pID);
                        }
                        else
                        {
                            // حذف pID من القائمة
                            if (proID.Contains(pID))
                                proID.Remove(pID);
                        }
                    }
                    if (proID.Count > 1)
                    {
                        btnEdite.Enabled = false;
                        btnDelete.Enabled = true;
                        btnQty.Enabled = false;
                    }
                    else if (proID.Count == 0)
                    {
                        btnEdite.Enabled = false;
                        btnQty.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnEdite.Enabled = true;
                        btnQty.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                }
                string proInf = dgvProductes.Rows[e.RowIndex].Cells["dgvProInfo"].Value.ToString();
                txtProductInfo.Text = proInf;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MainClass.ProCardDetete)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                // تعيين الـ Parent للنموذج الرئيسي
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;

                if (guna2MessageDialog2.Show(" هل تريد حذف هذا الصنف ") == DialogResult.Yes)
                {
                    if (proID.Count > 0)
                    {
                        foreach (int pID in proID)
                        {
                            string deleteQuery = @"
                        DELETE FROM products WHERE pID = @id;
                        DELETE FROM totalStor WHERE pID = @id;";

                            Hashtable ht = new Hashtable();
                            ht.Add("@id", pID);
                            MainClass.SQL(deleteQuery, ht);
                        }

                        // حذف الصفوف من الـ DataGridView
                        for (int i = dgvProductes.Rows.Count - 1; i >= 0; i--)
                        {
                            DataGridViewRow row = dgvProductes.Rows[i];
                            if (!row.IsNewRow)
                            {
                                int rowId = Convert.ToInt32(row.Cells["dgvproID"].Value); // غيرت من "dgvid" إلى "dgvproID"

                                if (proID.Contains(rowId))
                                {
                                    dgvProductes.Rows.RemoveAt(i);
                                }
                            }
                        }

                        proID.Clear();

                        refrish();
                        checkdgvstate();

                    }
                }
            }
            else
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        private int numberStaffRow()
        {
            string query = "SELECT COUNT(*) FROM products";
            int rowCount = 0;

            using (SqlConnection conn = MainClass.GetConnection()) // استخدام GetConnection
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                rowCount = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
            }

            return rowCount;
        }

        private void ClipControlRegion2(System.Windows.Forms.Control control, string direction, int cutSize)
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

        private void dgvProductes_Paint(object sender, PaintEventArgs e)
        {
            int num = numberStaffRow();
            if (num >= 15)
                ClipControlRegion2(dgvProductes, "left", 17);
            else
                ClipControlRegion2(dgvProductes, "left", 10);
        }

        private void btnEdite_Click(object sender, EventArgs e)
        {
            if (MainClass.ProCardEdite)
            {
                int pID = 0;
                if (proID.Count > 0)
                {
                    pID = proID[0];
                    frmCategoryCard frm = new frmCategoryCard();
                    frm.Owner = this;
                    frm.id = pID;
                    frm.ShowDialog();
                    this.Focus();
                    proID.Clear();

                    refrish();

                    checkdgvstate();

                }
            }
            else
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        void checkdgvstate()
        {
            if (proID.Count == 0)
            {
                btnEdite.Enabled = false;
                btnDelete.Enabled = false;
                btnQty.Enabled = false;
                if (dgvProductes.Columns.Contains("dgvSelect"))
                    dgvProductes.Columns["dgvSelect"].Visible = false;

            }
            else if (proID.Count == 1)
            {
                btnEdite.Enabled = true;
                btnQty.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdite.Enabled = false;
                btnQty.Enabled = false;
                btnDelete.Enabled = true;
            }
        }

        private void اضافةفاتورةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.AddSupplierBill)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            addBil();

        }

        private void عميلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowSuppliers)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Owner = this;

            frmBlackout.Show();
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.partyType = "عميل";
                frm.ShowDialog();

            }
            frmBlackout.Close();
            this.Focus();
        }


        private void موردToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowSuppliers)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Owner = this;

            frmBlackout.Show();
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.partyType = "مورد";
                frm.ShowDialog();

            }
            frmBlackout.Close();
            this.Focus();
        }

        private void فواتيرالعميلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowCustomerBills)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;
            btnDelete.Visible = false;

            mainpanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");


            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "عميل";
            frmAllBills.lblTitle.Text = "فواتير العملاء";
            AddControls(frmAllBills);
        }

        private void فواتيرالموردToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowSupplierBills)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;

            btnDelete.Visible = false;
            mainpanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");

            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "مورد";
            frmAllBills.lblTitle.Text = "فواتير الموردين";
            AddControls(frmAllBills);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowStoreBalance)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (txtNew.UseSystemPasswordChar)
            {
                btnNew.Text = "إخفاء إجمالي الجديد";
                txtNew.UseSystemPasswordChar = false;
                txtNew.PasswordChar = '\0';
            }
            else
            {
                btnNew.Text = "إظهار إجمالي الجديد";
                txtNew.UseSystemPasswordChar = true;
                txtNew.PasswordChar = '●';
            }
        }

        private void btnUsed_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowStoreBalance)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            if (txtUsed.UseSystemPasswordChar)
            {
                btnUsed.Text = "إخفاء إجمالي المستعمل";
                txtUsed.UseSystemPasswordChar = false;
                txtUsed.PasswordChar = '\0';

            }
            else
            {
                btnUsed.Text = "إظهار إجمالي المستعمل";
                txtUsed.UseSystemPasswordChar = true;
                txtUsed.PasswordChar = '●';

            }
        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowStoreBalance)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            if (txtStoreTotal.UseSystemPasswordChar)
            {
                btnTotal.Text = "إخفاء إجمالي سعر البيع للمخزون";

                txtStoreTotal.UseSystemPasswordChar = false;
                txtStoreTotal.PasswordChar = '\0';

            }
            else
            {
                btnTotal.Text = "إظهار إجمالي سعر البيع للمخزون";
                txtStoreTotal.UseSystemPasswordChar = true;
                txtStoreTotal.PasswordChar = '●';

            }
        }

        private void فواتيرالعملاءالمحذوفةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowDeletedCusBills)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;
            btnDelete.Visible = false;
            mainpanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");


            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "عميل";
            frmAllBills.lblTitle.Text = "فواتير العملاء المحذوفة";
            frmAllBills.isDeleted = true;
            AddControls(frmAllBills);
        }

        private void فواتيرالموردينالمحذوفةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowDeletedSupBills)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;
            btnDelete.Visible = false;
            mainpanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");

            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "مورد";
            frmAllBills.lblTitle.Text = "فواتير الموردين المحذوفة";
            frmAllBills.isDeleted = true;
            AddControls(frmAllBills);
        }

        private void btnQty_Click(object sender, EventArgs e)
        {
            if (MainClass.ProCardEdite)
            {
                int pID = 0;
                if (proID.Count > 0)
                {
                    pID = proID[0];
                    frmQtyChange frm = new frmQtyChange();
                    frm.Owner = this;
                    frm.proID = pID;
                    frm.ShowDialog();
                    this.Focus();
                    proID.Clear();

                    refrish();
                    checkdgvstate();

                }
            }
            else
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        private void dgvProductes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // لو الهيدر (RowIndex = -1)
            if (e.RowIndex == -1 && dgvProductes.CurrentCell != null)
            {
                if (e.ColumnIndex == dgvProductes.CurrentCell.ColumnIndex)
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

        private void عرضالمخازنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!MainClass.ShowCategories)
            //{
            //    guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            //    guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            //    guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
            //    guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
            //    return;
            //}
            position = "view stores";

            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;
            btnDelete.Visible = false;

            mainpanel.Controls.Clear();

            frmShowStors frmShowStors = new frmShowStors();
            AddControls(frmShowStors);
        }

        private void txtCatSearch_TextChanged(object sender, EventArgs e)
        {
            AddCategory(txtCatSearch.Text);

        }

        private void عميلToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowSuppliers)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            string targetFormName = "frmPartiesView"; // ← اسم الفورم اللي عايز تقفله

            foreach (Control ctrl in mainpanel.Controls)
            {
                if (ctrl is Form existingForm && existingForm.Name == targetFormName)
                {
                    existingForm.Close();   // يغلق الفورم
                    existingForm.Dispose(); // تنظيف الذاكرة
                    mainpanel.Controls.Remove(existingForm); // إزالة من الـ Panel
                    break;
                }
            }

            position = "view customers";

            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;
            btnDelete.Visible = false;

            frmPartiesView frmPartiesView = new frmPartiesView();
            frmPartiesView.type = "عميل";
            AddControls(frmPartiesView);
        }

        private void موردToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowSuppliers)
            {
                guna2MessageDialog2.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog2.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog2.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog2.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            string targetFormName = "frmPartiesView"; // ← اسم الفورم اللي عايز تقفله

            foreach (Control ctrl in mainpanel.Controls)
            {
                if (ctrl is Form existingForm && existingForm.Name == targetFormName)
                {
                    existingForm.Close();   // يغلق الفورم
                    existingForm.Dispose(); // تنظيف الذاكرة
                    mainpanel.Controls.Remove(existingForm); // إزالة من الـ Panel
                    break;
                }
            }


            position = "view supplier";

            btnPriceIncrease1.Visible = false;
            txtSearch1.Visible = false;
            btnEdite.Visible = false;
            btnQty.Visible = false;
            btnDelete.Visible = false;

            frmPartiesView frmPartiesView = new frmPartiesView();
            frmPartiesView.type = "مورد";
            AddControls(frmPartiesView);
        }
    }
}
