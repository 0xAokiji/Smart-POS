using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using Syncfusion.WinForms.DataGrid.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Test
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
            sfDataGrid1.CurrentCellKeyDown += SfDataGrid1_CurrentCellKeyDown;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Icon = SystemIcons.Information;  // يمكنك استخدام أيقونة أخرى حسب الحاجة
            notifyIcon1.ShowBalloonTip(3000, "Free Cash", "تم الحذف بنجاح", ToolTipIcon.Info);
        }

        // columns Names
        private void column()
        {
            sfDataGrid1.AutoGenerateColumns = false;

            // عمود اسم المنتج - تلقائي حسب المحتوى
            // عمود نصي

            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None; // تعطيل التوسيط التلقائي لكل الأعمدة
            var productNameColumn = new GridTextColumn()
            {
                MappingName = "ProductName",
                HeaderText = "اسم المنتج",
                AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells
            };
            sfDataGrid1.Columns.Add(productNameColumn);

            // عمود رقمي
            sfDataGrid1.Columns.Add(new GridNumericColumn()
            {
                MappingName = "Price",
                HeaderText = "السعر",
                Width = 100 // العرض بوحدة البكسل
            });

            // عمود تاريخ
            sfDataGrid1.Columns.Add(new GridDateTimeColumn()
            {
                MappingName = "PurchaseDate",
                HeaderText = "تاريخ الشراء",
                Width = 300 // العرض بوحدة البكسل
            });

            // تعيين مصدر البيانات
            productList = new BindingList<Product>(GetProductList("IPhone 6", 5000, DateTime.Today));
            sfDataGrid1.DataSource = productList;
        }

        // Add Data
        private List<Product> GetProductList(string proName, double price, DateTime date)
        {
            return new List<Product>
            {
                new Product { ProductName = proName, Price = price, PurchaseDate = date },
            };
        }

        private void test_Load(object sender, EventArgs e)
        {
            column();
        }

        // BindingList to store the product data
        private BindingList<Product> productList;

        private void SfDataGrid1_CurrentCellKeyDown(object sender, Syncfusion.WinForms.DataGrid.Events.CurrentCellKeyEventArgs e)
        {
            if (e.KeyEventArgs.KeyCode == Keys.Enter)
            {
                var currentCell = sfDataGrid1.CurrentCell;
                if (currentCell != null)
                {
                    var rowIndex = currentCell.RowIndex;

                    // التحقق إذا كنت في آخر صف
                    if (rowIndex == sfDataGrid1.RowCount - 1)
                    {
                        // التأكد من أن العمود هو الأول
                        if (currentCell.ColumnIndex == 0)
                        {
                            var column = sfDataGrid1.Columns[currentCell.ColumnIndex];
                            var columnName = column.MappingName;

                            if (columnName == "ProductName") // العمود المطلوب
                            {
                                var record = sfDataGrid1.GetRecordAtRowIndex(rowIndex);
                                var value = record.GetType().GetProperty(columnName)?.GetValue(record);

                                MessageBox.Show($"أدخلت الكمية: {value}");
                                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                                {
                                    // إضافة صف فارغ إلى BindingList
                                    productList.Add(new Product { ProductName = string.Empty, Price = 0, PurchaseDate = DateTime.MinValue });
                                    sfDataGrid1.Refresh();  // تحديث DataGrid لعرض الصف الجديد
                                }
                            }
                        }

                    }
                    else
                    {

                    }
                }
            }
        }

        private void ucProduct1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InsertDummyProducts();
        }

        private void InsertDummyProducts()
        {
            // اختر أي صورة مخزنة محليًا على جهازك
            string imagePath = @"E:\Defult_image.png"; // غيّر المسار حسب مكان الصورة
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            //using (SqlConnection con = new SqlConnection(MainClass.con_string))
            //{
            //    con.Open();

            //    for (int i = 1; i <= 500; i++)
            //    {
            //        SqlCommand cmd = new SqlCommand(@"
            //    INSERT INTO products (
            //        pName, pCode, shorcut, compName, categoryID, expitation,
            //        printBarcode, Wobalance, purPrice, sellPrice, discountPro,
            //        hDiscountPro, tax, discAllaw, maxAllaw, pImage, pqty,
            //        wholesale, minimump, requestP, idUnite1, idUnite2,
            //        idUniteDef, idUnite3, numberU2, numberU3, priceU2, priceU3
            //    )
            //    VALUES (
            //        @pName, @pCode, @shorcut, @compName, @categoryID, @expitation,
            //        @printBarcode, @Wobalance, @purPrice, @sellPrice, @discountPro,
            //        @hDiscountPro, @tax, @discAllaw, @maxAllaw, @pImage, @pqty,
            //        @wholesale, @minimump, @requestP, @idUnite1, @idUnite2,
            //        @idUniteDef, @idUnite3, @numberU2, @numberU3, @priceU2, @priceU3
            //    )", con);

            //        cmd.Parameters.AddWithValue("@pName", $"منتج {i}");
            //        cmd.Parameters.AddWithValue("@pCode", $"PRD{i:D4}");
            //        cmd.Parameters.AddWithValue("@shorcut", $"M{i}");
            //        cmd.Parameters.AddWithValue("@compName", "شركة تجريبية");
            //        cmd.Parameters.AddWithValue("@categoryID", 1);
            //        cmd.Parameters.AddWithValue("@expitation", false);
            //        cmd.Parameters.AddWithValue("@printBarcode", true);
            //        cmd.Parameters.AddWithValue("@Wobalance", false);
            //        cmd.Parameters.AddWithValue("@purPrice", 10 + i);
            //        cmd.Parameters.AddWithValue("@sellPrice", 15 + i);
            //        cmd.Parameters.AddWithValue("@discountPro", 0);
            //        cmd.Parameters.AddWithValue("@hDiscountPro", 0);
            //        cmd.Parameters.AddWithValue("@tax", 0);
            //        cmd.Parameters.AddWithValue("@discAllaw", 0);
            //        cmd.Parameters.AddWithValue("@maxAllaw", 0);
            //        cmd.Parameters.AddWithValue("@pImage", imageBytes);
            //        cmd.Parameters.AddWithValue("@pqty", 100);
            //        cmd.Parameters.AddWithValue("@wholesale", 12 + i);
            //        cmd.Parameters.AddWithValue("@minimump", 5);
            //        cmd.Parameters.AddWithValue("@requestP", 10);
            //        cmd.Parameters.AddWithValue("@idUnite1", 1);
            //        cmd.Parameters.AddWithValue("@idUnite2", 2);
            //        cmd.Parameters.AddWithValue("@idUniteDef", 1);
            //        cmd.Parameters.AddWithValue("@idUnite3", 3);
            //        cmd.Parameters.AddWithValue("@numberU2", 2);
            //        cmd.Parameters.AddWithValue("@numberU3", 3);
            //        cmd.Parameters.AddWithValue("@priceU2", 7 + i);
            //        cmd.Parameters.AddWithValue("@priceU3", 6 + i);

            //        cmd.ExecuteNonQuery();
            //    }

            //    con.Close();
            //    MessageBox.Show("تمت إضافة 500 منتج بنجاح.");
            //}
        }

    }
}
