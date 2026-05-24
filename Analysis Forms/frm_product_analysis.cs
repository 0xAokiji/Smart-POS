using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace pos.Analysis_Forms
{
    public partial class frm_product_analysis : Form
    {


        public frm_product_analysis()
        {
            InitializeComponent();
            InitializeChart(); // تهيئة الرسم البياني عند تحميل النموذج
        }

        public class SalesData
        {
            public string ProductName { get; set; }
            public string productprice { get; set; }
            public decimal SalesAmount { get; set; }
            public decimal CurrentQty { get; set; }
        }

        private List<SalesData> GetSalesDataFromDB(string period)
        {
            List<SalesData> data = new List<SalesData>();

            using (SqlConnection conn = MainClass.GetConnection()) // ✅ بدل new SqlConnection
            {
                conn.Open();

                string query = "";

                // تحديد الاستعلام بناءً على الفترة المطلوبة
                if (period == "today")
                {
                    query = @"SELECT 
                        p.pCode AS ProductCode,
                        p.pName AS ProductName,
                        p.sellPrice AS productprice,
                        SUM(d.qty) AS TotalSalesAmount,
                        ts.qtyU1 AS CurrentQty
                    FROM dbo.tblDetails d
                    JOIN dbo.products p ON d.proID = p.pID
                    JOIN dbo.tblMain1 m ON d.MainID = m.MainID
                    LEFT JOIN dbo.totalStor ts ON p.pID = ts.pID
                    WHERE CAST(m.aDate AS DATE) = CAST(GETDATE() AS DATE)
                    GROUP BY p.pCode, p.pName, ts.qtyU1 ,p.sellPrice
                    ORDER BY p.pCode;";
                }
                else if (period == "week")
                {
                    query = @"SELECT 
                        p.pCode AS ProductCode,
                        p.pName AS ProductName,
                        p.sellPrice AS productprice,
                        SUM(d.qty) AS TotalSalesAmount,
                        ts.qtyU1 AS CurrentQty
                    FROM dbo.tblDetails d
                    JOIN dbo.products p ON d.proID = p.pID
                    JOIN dbo.tblMain1 m ON d.MainID = m.MainID
                    LEFT JOIN dbo.totalStor ts ON p.pID = ts.pID
                    WHERE m.aDate >= DATEADD(DAY, -7, GETDATE())
                    GROUP BY p.pCode, p.pName, ts.qtyU1 ,p.sellPrice
                    ORDER BY p.pCode;";
                }
                else if (period == "month")
                {
                    query = @"SELECT 
                        p.pCode AS ProductCode,
                        p.pName AS ProductName,
                        p.sellPrice AS productprice,
                        SUM(d.qty) AS TotalSalesAmount,
                        ts.qtyU1 AS CurrentQty
                    FROM dbo.tblDetails d
                    JOIN dbo.products p ON d.proID = p.pID
                    JOIN dbo.tblMain1 m ON d.MainID = m.MainID
                    LEFT JOIN dbo.totalStor ts ON p.pID = ts.pID
                    WHERE MONTH(m.aDate) = MONTH(GETDATE()) 
                      AND YEAR(m.aDate) = YEAR(GETDATE())
                    GROUP BY p.pCode, p.pName, ts.qtyU1 ,p.sellPrice
                    ORDER BY p.pCode;";
                }
                else if (period == "year")
                {
                    query = @"SELECT 
                        p.pCode AS ProductCode,
                        p.pName AS ProductName,
                        p.sellPrice AS productprice,
                        SUM(d.qty) AS TotalSalesAmount, -- نجمع الكميات المباعة
                        ts.qtyU1 AS CurrentQty -- الكمية الموجودة في المخزن
                    FROM dbo.tblDetails d
                    JOIN dbo.products p ON d.proID = p.pID
                    JOIN dbo.tblMain1 m ON d.MainID = m.MainID
                    LEFT JOIN dbo.totalStor ts ON p.pID = ts.pID
                    GROUP BY p.pCode, p.pName, ts.qtyU1, p.sellPrice 
                    ORDER BY p.pCode;";
                }

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // تحويل DataTable إلى List<SalesData>
                foreach (DataRow row in dt.Rows)
                {
                    data.Add(new SalesData
                    {
                        ProductName = row["ProductName"].ToString(),
                        productprice = row["productprice"].ToString(),
                        SalesAmount = Convert.ToDecimal(row["TotalSalesAmount"].ToString()),
                        CurrentQty = Convert.ToDecimal(row["CurrentQty"].ToString())
                    });
                }
            }

            return data;
        }

        // دالة لتحميل بيانات المبيعات بناءً على الفترة المحددة
        private void LoadSalesData(string period)
        {
            var salesData = GetSalesDataFromDB(period);

            if (salesData.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات لعرضها في هذه الفترة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // إنشاء DataTable لعرضه في GridControl باستخدام الـ index للأعمدة
            DataTable table = new DataTable();

            // إضافة الأعمدة باستخدام الـ index
            table.Columns.Add("ProductName");
            table.Columns.Add("productprice");
            table.Columns.Add("SalesAmount", typeof(decimal));
            table.Columns.Add("CurrentQty", typeof(decimal));

            foreach (var item in salesData)
            {
                // إضافة البيانات باستخدام الـ index
                table.Rows.Add(item.ProductName, item.productprice, item.SalesAmount, item.CurrentQty);
            }

            gridControlSales.DataSource = table; // ربط الجدول بالـ Grid

            UpdateChart(salesData); // تحديث الرسم البياني
        }


        // تهيئة الرسم البياني (Chart) لعرض المبيعات
        private void InitializeChart()
        {
            chartControlSeles.Series.Clear(); // مسح أي سلسلة موجودة

            var series = new Series("مبيعات", ViewType.Line);
            chartControlSeles.Series.Add(series); // إضافة السلسلة إلى الرسم البياني

            series.ArgumentDataMember = "Date"; // ربط تاريخ المبيعات
            series.ValueDataMembers.AddRange(new string[] { "SalesAmount" }); // ربط قيمة المبيعات

            chartControlSeles.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False; // إخفاء الأسطورة
            chartControlSeles.Titles.Add(new ChartTitle() { Text = "مبيعات المنتجات" }); // إضافة عنوان للرسم البياني

            // إضافة تنسيقات إضافية
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True; // إظهار القيم على الرسم البياني
            series.View.Color = Color.Green; // تغيير لون السلسلة
        }

        // تحديث الرسم البياني بناءً على البيانات
        private void UpdateChart(List<SalesData> salesData)
        {
            chartControlSeles.Series.Clear(); // نحذف أي بيانات سابقة

            // تجميع البيانات حسب المنتج
            var groupedData = salesData
                .GroupBy(x => x.ProductName)
                .Select(g => new SalesData
                {
                    ProductName = g.Key,
                    SalesAmount = g.Sum(x => x.SalesAmount)
                })
                .OrderByDescending(x => x.SalesAmount)
                .Take(10)  // تحديد أعلى 6 منتجات
                .ToList();

            Series series = new Series("مبيعات", ViewType.Bar); // استخدام الرسم البياني الشريطي

            foreach (var item in groupedData)
            {
                // نضيف كل نقطة بالرسم البياني (اسم المنتج وكمية المبيعات)
                series.Points.Add(new SeriesPoint(item.ProductName, item.SalesAmount));
            }

            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series.View.Color = Color.Green;
            series.ArgumentScaleType = ScaleType.Qualitative; // تغيير من ScaleType.DateTime إلى ScaleType.Qualitative لأننا نعرض أسماء المنتجات

            chartControlSeles.Series.Add(series);

            chartControlSeles.Titles.Clear();
            chartControlSeles.Titles.Add(new ChartTitle() { Text = "أفضل 10 منتجات مبيعًا" });
        }



        // التعامل مع زر مبيعات اليوم
        private void BtnTodaySales_Click(object sender, EventArgs e)
        {
            LoadSalesData("today"); // تحميل بيانات مبيعات اليوم
            BtnTodaySales.Checked = true;
            BtnMonthlySales.Checked = false;
            BtnWeeklySales.Checked = false;
            BtnYearlySales.Checked = false;
        }

        // التعامل مع زر مبيعات الأسبوع
        private void BtnWeeklySales_Click(object sender, EventArgs e)
        {
            LoadSalesData("week"); // تحميل بيانات مبيعات الأسبوع
            BtnTodaySales.Checked = false;
            BtnMonthlySales.Checked = false;
            BtnWeeklySales.Checked = true;
            BtnYearlySales.Checked = false;
        }

        // التعامل مع زر مبيعات الشهر
        private void BtnMonthlySales_Click(object sender, EventArgs e)
        {
            LoadSalesData("month"); // تحميل بيانات مبيعات الشهر
            BtnTodaySales.Checked = false;
            BtnMonthlySales.Checked = true;
            BtnWeeklySales.Checked = false;
            BtnYearlySales.Checked = false;
        }

        private void frm_product_analysis_Load(object sender, EventArgs e)
        {
            LoadSalesData("today"); // تحميل بيانات مبيعات اليوم


        }

        private void BtnYearlySales_Click(object sender, EventArgs e)
        {
            LoadSalesData("year");
            BtnTodaySales.Checked = false;
            BtnMonthlySales.Checked = false;
            BtnWeeklySales.Checked = false;
            BtnYearlySales.Checked = true;
        }
    }
}
