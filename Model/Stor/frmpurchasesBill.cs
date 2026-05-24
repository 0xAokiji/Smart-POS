using DevExpress.Charts.Native;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using DevExpress.XtraWaitForm;
using pos.Classes;
using pos.GeneralForms;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace pos.Model
{
    public partial class frmpurchasesBill : SampleAdd
    {
        //Fields
        private float BorderRadius = 8f;
        private float BorderSize = 2f;
        private Color borderColor = Color.FromArgb(136, 214, 218);



        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedFillColor2;
        private Color checkedForeColor;

        public int id = 0;

        public frmpurchasesBill()
        {
            InitializeComponent();

            ThemeMode();
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

        private void frmpurchasesBill_Load(object sender, EventArgs e)
        {
            CheckSpecificTextBoxes();

            txtNumberS.Focus();
            dtPicker.Format = DateTimePickerFormat.Custom;
            dtPicker.Value = DateTime.Now;
            dtPicker.CustomFormat = "dd/MM/yyyy";

        }
        private void CenterMyPanel()
        {
            secondPanel.Width = 1018;
            secondPanel.Height = 231;

            secondPanel.Left = (mainPanel.Width - secondPanel.Width) / 2;
            secondPanel.Top = (mainPanel.Height - secondPanel.Height) / 2;
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {

            this.Close();
        }

        private int storeID = 0;
        private int supplierID = 0;
        public int qty = 0;
        public int Value { get; private set; } // قيمة int التي تريد إرسالها

        public frmpurchasesBill(int value)
        {
            Value = value;
        }
        public delegate void ButtonClickedEventHandler(object sender, frmpurchasesBill e);

        public event ButtonClickedEventHandler ButtonClicked;
        protected virtual void OnButtonClicked(int value)
        {
            // إنشاء instance من ButtonClickedEventArgs وإرساله مع الحدث
            ButtonClicked?.Invoke(this, new frmpurchasesBill(value));
        }


        public event EventHandler<bool> OnResultSelected; // تقدر تغيّر الـ string لأي نوع تريده


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                OnResultSelected?.Invoke(this, true);

                string qry;
                if (id == 0)
                {
                    qry = @"INSERT INTO billPrcheses 
                    (storeID, supplierID, pqty, serialNumber, notes, payWay, billNumber, total, clear, date, Time) 
                    VALUES 
                    (@storeID, @supplierID, @pqty, @serialNumber, @notes, @payWay, @billNumber, @total, @clear, @date, @Time);
                    SELECT SCOPE_IDENTITY();";
                }
                else
                {
                    qry = @"UPDATE billPrcheses 
                    SET storeID = @storeID, 
                        supplierID = @supplierID, 
                        pqty = @pqty, 
                        serialNumber = @serialNumber, 
                        notes = @notes, 
                        payWay = @payWay, 
                        billNumber = @billNumber, 
                        total = @total, 
                        clear = @clear, 
                        date = @date, 
                        Time = @Time 
                    WHERE bID = @id";
                }

                using (SqlConnection con = MainClass.GetConnection()) // ✅ الطريقة الموحدة
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        // 🔹 المعاملات
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@storeID", storeID);
                        cmd.Parameters.AddWithValue("@supplierID", supplierID);
                        cmd.Parameters.AddWithValue("@pqty", qty);
                        cmd.Parameters.AddWithValue("@serialNumber", DBNull.Value);
                        cmd.Parameters.AddWithValue("@notes", txtNote.Text);
                        cmd.Parameters.AddWithValue("@payWay", DBNull.Value);
                        cmd.Parameters.AddWithValue("@billNumber", txtBillNumber.Text);
                        cmd.Parameters.AddWithValue("@total", DBNull.Value);
                        cmd.Parameters.AddWithValue("@clear", DBNull.Value);
                        cmd.Parameters.AddWithValue("@date", dtPicker.Value);
                        cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToShortTimeString());

                        // 🔹 تنفيذ الاستعلام
                        if (id == 0)
                        {
                            id = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        else
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // 🔹 إرسال النتيجة بعد الحفظ
                OnButtonClicked(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ البيانات:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ✅ السماح فقط بالأرقام والتحكم (Backspace وغيرها)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // ✅ عند الضغط على Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // منع الصوت الافتراضي للـ Enter

                if (!int.TryParse(txtNumberS.Text, out int storeNumber))
                {
                    txtNumberS.Text = "0";
                    MessageBox.Show("لم يتم العثور على البيانات.");
                    txtNameS.Clear();
                    return;
                }

                string qry = @"SELECT storeName, storeID FROM addStore WHERE storeNumber = @storeNumber";

                try
                {
                    using (SqlConnection con = MainClass.GetConnection()) // ✅ نفس النمط في كل مرة
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddWithValue("@storeNumber", storeNumber);

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    txtNameS.Text = dr["storeName"].ToString();
                                    storeID = Convert.ToInt32(dr["storeID"]);
                                }
                                else
                                {
                                    MessageBox.Show("لم يتم العثور على البيانات.");
                                    txtNameS.Clear();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء الاتصال بقاعدة البيانات:\n" + ex.Message);
                }

                txtSupNumber.Focus();
            }
        }


        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }


        }

        private void txtSupNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ✅ السماح فقط بالأرقام وأزرار التحكم
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                if (!int.TryParse(txtSupNumber.Text, out int supCode))
                {
                    txtSupNumber.Text = "0";
                    MessageBox.Show("لم يتم العثور على البيانات.");
                    txtSupName.Clear();
                    return;
                }

                string qry = @"SELECT sName, sID FROM supplier WHERE supCode = @supCode";

                try
                {
                    using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدم الطريقة الموحدة للاتصال
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(qry, con))
                        {
                            cmd.Parameters.AddWithValue("@supCode", supCode);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtSupName.Text = reader["sName"].ToString();
                                    supplierID = Convert.ToInt32(reader["sID"]);
                                }
                                else
                                {
                                    MessageBox.Show("لم يتم العثور على البيانات.");
                                    txtSupName.Clear();
                                    return;
                                }
                            }

                            // ✅ جلب المجموع بعد التأكد من وجود المورد
                            string qry2 = @"SELECT ISNULL(SUM(clear), 0) AS TotalClear 
                                    FROM billPrcheses 
                                    WHERE supplierID = @supID";

                            using (SqlCommand cmd2 = new SqlCommand(qry2, con))
                            {
                                cmd2.Parameters.AddWithValue("@supID", supplierID);
                                object result = cmd2.ExecuteScalar();

                                txtSumSupp.Text = result != DBNull.Value ? result.ToString() : "0";
                            }
                        }
                    }

                    txtBillNumber.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء الاتصال بقاعدة البيانات:\n" + ex.Message);
                }
            }
        }

        bool enter = false;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmpurchasesBill_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // منع النافذة من الإغلاق الفعلي
            this.Hide();
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmBlackout")
                {
                    form.Close(); // إغلاق النافذة
                    break;
                }
            }

            //frmProductView frmMian = new frmProductView();
            //frmMian.add("Amer");
        }

        private void CheckSpecificTextBoxes()
        {
            // افحص تيكست بوكسات معينة
            //if (!string.IsNullOrWhiteSpace(txtNameS.Text) &&
            //    !string.IsNullOrWhiteSpace(txtSupName.Text) &&
            //    !string.IsNullOrWhiteSpace(txtBillNumber.Text))
            //{
            //    btnAdd.Enabled = true;
            //    btnAdd.FillColor = Color.FromArgb(241, 85, 126);
            //    btnAdd.BackColor = Color.Gainsboro;

            //}
            //else
            //{
            //    btnAdd.Enabled = false;
            //    btnAdd.BackColor = Color.Gainsboro;
            //    btnAdd.FillColor = Color.DimGray;
            //}
        }

        private void txtSupNumber_TextChanged(object sender, EventArgs e)
        {
            CheckSpecificTextBoxes();

            if (!string.IsNullOrWhiteSpace(txtNameS.Text) && !string.IsNullOrWhiteSpace(txtSupName.Text) && !string.IsNullOrWhiteSpace(txtBillNumber.Text))
                btnPSave.Enabled = true;
            else
                btnPSave.Enabled = false;

            if (string.IsNullOrWhiteSpace(txtNameS.Text))
            {
                txtNameS.TextAlign = HorizontalAlignment.Right;
                return;

            }
            if (!string.IsNullOrWhiteSpace(txtNameS.Text) && !string.IsNullOrWhiteSpace(txtSupName.Text) && !string.IsNullOrWhiteSpace(txtBillNumber.Text))
                btnPSave.Enabled = true;
            else
                btnPSave.Enabled = false;

            char firstChar = txtNameS.Text[0];

            if (IsArabic(firstChar))
                txtNameS.TextAlign = HorizontalAlignment.Right;
            else
                txtNameS.TextAlign = HorizontalAlignment.Left;
        }
        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
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
        public void ThemeMode()
        {
            ThemeColor();
            this.BackColor = backgroundPrimary;
            //Panels

            bottomPanel.BackColor = backgroundSecondary;
            mainPanel.BackColor = backgroundPrimary;
            topPanel.BackColor = checkedFillColor;

            //Lables
            lblTitle.ForeColor = textColor;
            lblSuplieser.ForeColor = textColor;
            lblStore.ForeColor = textColor;
            lblNote.ForeColor = textColor;
            lblDate.ForeColor = textColor;
            lblBillNumber.ForeColor = textColor;

            //Text box
            txtNumberS.BackColor = backgroundPrimary;
            txtNumberS.ForeColor = textColor;
            txtNumberS.BorderColor = checkedFillColor;
            txtNumberS.FillColor = backgroundPrimary;

            txtNameS.BackColor = backgroundPrimary;
            txtNameS.ForeColor = textColor;
            txtNameS.BorderColor = checkedFillColor;
            txtNameS.FillColor = backgroundPrimary;

            txtSupNumber.BackColor = backgroundPrimary;
            txtSupNumber.ForeColor = textColor;
            txtSupNumber.BorderColor = checkedFillColor;
            txtSupNumber.FillColor = backgroundPrimary;

            txtSupName.BackColor = backgroundPrimary;
            txtSupName.ForeColor = textColor;
            txtSupName.BorderColor = checkedFillColor;
            txtSupName.FillColor = backgroundPrimary;

            txtSumSupp.BackColor = backgroundPrimary;
            txtSumSupp.ForeColor = Color.White;
            txtSumSupp.BorderColor = checkedFillColor;
            txtSumSupp.FillColor = Color.FromArgb(0, 179, 60); ;

            txtBillNumber.BackColor = backgroundPrimary;
            txtBillNumber.ForeColor = textColor;
            txtBillNumber.BorderColor = checkedFillColor;
            txtBillNumber.FillColor = backgroundPrimary;

            txtNote.BackColor = backgroundPrimary;
            txtNote.ForeColor = textColor;
            txtNote.BorderColor = checkedFillColor;
            txtNote.FillColor = backgroundPrimary;

            dtPicker.BackColor = backgroundPrimary;
            dtPicker.ForeColor = textColor;
            dtPicker.BorderColor = checkedFillColor;
            dtPicker.FillColor = checkedFillColor;

            //Buttons
            btnPSave.FillColor = checkedFillColor;
            btnPSave.ForeColor = textColor2;


            btnNew.FillColor = Color.FromArgb(0, 179, 60);
            btnNew.ForeColor = Color.White;


            //GroupBox
            groupBox1.ForeColor = textColor;
            groupBox2.ForeColor = textColor;
            groupBox3.ForeColor = textColor;
            groupBox4.ForeColor = textColor;

        }

        private void txtNote_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNote.Text))
            {
                txtNote.TextAlign = HorizontalAlignment.Right;
                return;

            }
            char firstChar = txtNote.Text[0];

            if (IsArabic(firstChar))
                txtNote.TextAlign = HorizontalAlignment.Right;
            else
                txtNote.TextAlign = HorizontalAlignment.Left;
        }

        private void txtBillNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frmpurchasesBill_SizeChanged(object sender, EventArgs e)
        {
            CenterMyPanel();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            newBill();

        }
        public void newBill()
        {
            id = 0;
            txtNumberS.Clear();
            txtNameS.Clear();
            txtSupNumber.Clear();
            txtSupName.Clear();
            txtSumSupp.Clear();
            txtBillNumber.Clear();
            txtNote.Clear();
            dtPicker.Value = DateTime.Now;
            storeID = 0;
            supplierID = 0;
            qty = 0;
            btnPSave.Enabled = false;

            OnResultSelected?.Invoke(this, false);
        }
    }
}




