using DevExpress.CodeParser;
using pos.Classes;
using pos.View;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml.Linq;

namespace pos.Model
{
    public partial class frmSupplier : SampleAdd
    {

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;

        public frmSupplier()
        {
            InitializeComponent();

            DataTable dt3 = new DataTable();

            string qry1 = @"SELECT sName AS 'name' FROM supplier"; // تأكد من استخدام الاستعلام الصحيح

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd2 = new SqlCommand(qry1, con))
            using (SqlDataAdapter da3 = new SqlDataAdapter(cmd2))
            {
                da3.Fill(dt3);
            }

            AutoCompleteStringCollection dataSource2 = new AutoCompleteStringCollection();
            foreach (DataRow row in dt3.Rows)
            {
                dataSource2.Add(row["name"].ToString());
            }

            this.comboBox1.AutoCompleteCustomSource = dataSource2;
            this.comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            ThemeMode();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // لتقليل الوميض أثناء الرسم
                return cp;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // اجعل الحواف ناعمة (AntiAlias)
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // لون الإطار من MainClass
            Color borderColor = MainClass.CheckedFillColor;
            float borderSize = 1f;

            using (Pen pen = new Pen(borderColor, borderSize))
            {
                int offset = (int)(borderSize / 2);

                // رسم مستطيل حول الفورم
                e.Graphics.DrawRectangle(pen, new Rectangle(offset, offset, this.Width - (int)borderSize, this.Height - (int)borderSize));
            }
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                int storeNumber;
                if (!int.TryParse(txtSupCode.Text, out storeNumber))
                {
                    MessageBox.Show("ادخل الكود رقم صحيح");
                    return;
                }

                string qry;
                int selectedID = comboBox1.SelectedValue != null ? Convert.ToInt32(comboBox1.SelectedValue) : 0;

                if (selectedID == 0)
                {
                    qry = @"INSERT INTO supplier (sName, sPhone, supCode) VALUES (@name, @phone, @supCode)";

                    Hashtable ht3 = new Hashtable();
                    string qry3 = @"Insert into rconrdEditingPro Values(@posName, @editeIn,@editeTo, @tableName , @typeEdit,@date ,@time); Select SCOPE_IDENTITY()";

                    ht3.Add("@posName", MainClass.USER);
                    ht3.Add("@editeIn", comboBox1.Text);
                    ht3.Add("@editeTo", DBNull.Value);
                    ht3.Add("@tableName", "مورد");
                    ht3.Add("@typeEdit", "اضافه");
                    ht3.Add("@date", DateTime.Now.Date);
                    ht3.Add("@time", DateTime.Now.ToShortTimeString());

                    MainClass.SQL(qry3, ht3);
                }
                else
                {
                    qry = @"UPDATE supplier 
            SET sName = @name, sPhone = @phone, supCode = @supCode 
            WHERE sID = @sID";
                }

                // ✅ استخدام الاتصال الآمن
                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@name", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@supCode", storeNumber);

                    if (selectedID != 0)
                        cmd.Parameters.AddWithValue("@sID", selectedID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                Notifier.ShowNotification("Done ✅", "تم الحفظ بنجاح");

                string qry1 = "SELECT sID 'id', sName 'name' FROM supplier";
                MainClass.CBFill(qry1, comboBox1);
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
            }
        }



        private void frmSupplier_Load(object sender, EventArgs e)
        {

            string qry = "select sID 'id' , sName 'name' from supplier ";
            MainClass.CBFill(qry, comboBox1);
            txtSupCode.Focus();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    Notifier.ShowNotification("Error ❌", "لم يتم تحديد المورد");
                    return;
                }
                else
                {
                    Hashtable ht3 = new Hashtable();
                    string qry3 = @"Insert into rconrdEditingPro Values(@posName, @editeIn,@editeTo, @tableName , @typeEdit,@date ,@time); Select SCOPE_IDENTITY()";

                    ht3.Add("@posName", MainClass.USER);
                    ht3.Add("@editeIn", comboBox1.Text);
                    ht3.Add("@editeTo", DBNull.Value);
                    ht3.Add("@tableName", "مورد");
                    ht3.Add("@typeEdit", "حذف");
                    ht3.Add("@date", Convert.ToDateTime(DateTime.Now.Date));
                    ht3.Add("@time", Convert.ToString(DateTime.Now.ToShortTimeString()));

                    MainClass.SQL(qry3, ht3);

                    string qry = "Delete from supplier where sID = " + Convert.ToInt32(comboBox1.SelectedValue) + string.Empty;
                    Hashtable ht = new Hashtable();
                    MainClass.SQL(qry, ht);
                    string qry1 = "select sID 'id' , sName 'name' from supplier ";
                    MainClass.CBFill(qry1, comboBox1);
                    Notifier.ShowNotification("Done ✅", "تم حذف مورد");

                }
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
                return;

            }

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            imgDellete.Visible = true;

            string qry1 = "select sPhone from supplier where sID = " + Convert.ToInt32(comboBox1.SelectedValue) + string.Empty;

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd2 = new SqlCommand(qry1, con)) // استخدام الاتصال الآمن
            {
                cmd2.CommandType = CommandType.Text;
                DataTable dt3 = new DataTable();
                using (SqlDataAdapter da3 = new SqlDataAdapter(cmd2))
                {
                    da3.Fill(dt3);
                }

                if (dt3.Rows.Count > 0) // تأكد من وجود صفوف في الجدول
                {
                    txtPhone.Text = dt3.Rows[0]["sPhone"].ToString(); // الوصول إلى البيانات إذا كان هناك صف
                }
                else
                {
                    txtPhone.Text = "";
                    txtSupCode.Text = "";
                    txtPhone.PlaceholderText = "بيانات الاتصال فارغة";
                }
            }
        }


        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
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

            if (MainClass.ThemeMode == "dark")
            {
                iconImage.Image = Properties.Resources.supplier_Dark;

            }
            else if (MainClass.ThemeMode == "light")
            {
                iconImage.Image = Properties.Resources.supplier_Light;

            }

            ThemeColor();

            this.BackColor = backgroundPrimary;

            //Panels
            bottomPanel.BackColor = backgroundSecondary;
            topPanel.BackColor = checkedFillColor;

            iconImage.BackColor = checkedFillColor;
            imgDellete.BackColor = backgroundSecondary;


            //Lables
            lblTitle.ForeColor = textColor;
            lblName.ForeColor = textColor;
            lblCode.ForeColor = textColor;
            lblPhone.ForeColor = textColor;

            //Text box
            txtPhone.BackColor = backgroundPrimary;
            txtPhone.ForeColor = textColor2;
            txtPhone.BorderColor = checkedFillColor;
            txtPhone.FillColor = backgroundPrimary;

            txtSupCode.BackColor = backgroundPrimary;
            txtSupCode.ForeColor = textColor2;
            txtSupCode.BorderColor = checkedFillColor;
            txtSupCode.FillColor = backgroundPrimary;


            //->Button  
            btn_Close.FillColor = Color.Red;
            btn_Close.ForeColor = textColor;

            btn_Save.FillColor = checkedFillColor;
            btn_Save.ForeColor = textColor;

            //comboBox
            comboBox1.ForeColor = textColor;
            comboBox1.BackColor = backgroundSecondary;
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
