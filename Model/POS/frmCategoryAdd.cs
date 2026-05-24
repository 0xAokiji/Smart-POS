using DevExpress.Office.Services;
using DevExpress.Pdf.Xmp;
using DevExpress.XtraMap.ItemEditor;
using DevExpress.XtraReports.Design;
using DevExpress.XtraRichEdit.Export.Doc;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.XtraRichEdit.Utils;
using pos.Classes;
using pos.View;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;

namespace pos.Model
{
    public partial class frmCategoryAdd : SampleAdd
    {
        public int id = 0;
        public string cat = string.Empty;
        private string typeEdit = string.Empty;


        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;


        public frmCategoryAdd()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            string qry = @"SELECT catName FROM category ";

            using (SqlConnection con = MainClass.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;
                    DataTable dt2 = new DataTable();
                    using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                    {
                        da2.Fill(dt2);
                    }

                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        dataSource.Add(dt2.Rows[i][0].ToString());
                    }

                    this.txtName.AutoCompleteCustomSource = dataSource;
                    this.txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
            }
            iconImage.Image = Properties.Resources.categories_Dark;

            //ThemeMode();
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

        private void frmCategoryAdd_Load(object sender, EventArgs e)
        {
           
        }


        public override void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                saveData();
            }
        }

        private void saveData()
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qryx = @"SELECT COUNT(*) FROM category WHERE catName = @catName";

                    using (SqlCommand cmd1 = new SqlCommand(qryx, con))
                    {
                        cmd1.Parameters.AddWithValue("@catName", txtName.Text);
                        cmd1.CommandType = CommandType.Text;

                        con.Open();
                        int count = (int)cmd1.ExecuteScalar();
                        con.Close();

                        if (count > 0 && id == 0)
                        {
                            Notifier.ShowNotification("خطأ", "هذا الصنف موجد بالفعل");

                            txtName.Focus();
                            txtName.SelectAll();
                            this.ActiveControl = txtName;
                            return;
                        }
                    }

                    string qry1 = @"SELECT catName FROM category ";
                    DataTable dt2 = new DataTable();

                    using (SqlCommand cmd = new SqlCommand(qry1, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                        {
                            da2.Fill(dt2);
                        }
                    }

                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        if (dt2.Rows[i][0].ToString() == txtName.Text)
                        {
                            Notifier.ShowNotification("خطأ", "هذا الصنف موجود بالفعل");
                            txtName.Focus();
                            txtName.SelectAll();
                            return;
                        }
                    }

                    string qry = string.Empty;
                    Hashtable ht = new Hashtable();

                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Please enter a name.");
                        return;
                    }

                    if (id == 0) // Insert
                    {
                        qry = "Insert into category Values (@Name)";
                        ht.Add("@Name", txtName.Text);
                        typeEdit = "اضافه";
                        cat = txtName.Text;
                    }
                    else
                    {
                        qry = "Update category Set catName = @Name where catID = @id";
                        ht.Add("@Name", txtName.Text);
                        ht.Add("@id", id);
                        typeEdit = "تعديل";
                    }

                    Hashtable ht3 = new Hashtable();
                    string qry3 = @"Insert into rconrdEditingPro Values(@posName, @editeIn,@editeTo, @tableName , @typeEdit,@date ,@time); Select SCOPE_IDENTITY()";

                    ht3.Add("@posName", MainClass.USER);
                    ht3.Add("@editeIn", cat);
                    ht3.Add("@editeTo", txtName.Text);
                    ht3.Add("@tableName", "صنف");
                    ht3.Add("@typeEdit", typeEdit);
                    ht3.Add("@date", Convert.ToDateTime(DateTime.Now.Date));
                    ht3.Add("@time", Convert.ToString(DateTime.Now.ToShortTimeString()));
                    MainClass.SQL(qry3, ht3);

                    if (MainClass.SQL(qry, ht) > 0)
                    {
                        id = 0;
                        txtName.Text = string.Empty;
                        txtName.Focus();
                    }

                    Notifier.ShowNotification("تم", "تم الحفظ بنحاج");
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ");
                return;
            }
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
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
                iconImage.Image = Properties.Resources.categories_Dark;
               
            }
            else if (MainClass.ThemeMode == "light")
            {
                iconImage.Image = Properties.Resources.categories_Light;
               
            }

            ThemeColor();

            this.BackColor = backgroundPrimary;

            //Panels
            bottomPanel.BackColor = backgroundSecondary;
            topPanel.BackColor = checkedFillColor;

            iconImage.BackColor = checkedFillColor;

            //Lables
            lblTitle.ForeColor = textColor2;

            //Text box
            txtName.BackColor = backgroundPrimary;
            txtName.ForeColor = textColor2;
            txtName.BorderColor = checkedFillColor;
            txtName.FillColor = backgroundPrimary;



            //->Button  
            btn_Close.FillColor = Color.Red;
            btn_Close.ForeColor = textColor2;

            btn_Save.FillColor = checkedFillColor;
            btn_Save.ForeColor = textColor2;

        }
    }
}
