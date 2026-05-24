using DevExpress.XtraEditors;
using pos.Classes;
using pos.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace pos.View
{
    public partial class frmCategoryView : SampleView
    {
        public frmCategoryView()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.InputLanguageChanged += new InputLanguageChangedEventHandler(MyForm_InputLanguageChanged);

            string qry = @"SELECT catName FROM category";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.CommandType = CommandType.Text;
                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                {
                    da2.Fill(dt2);
                    AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        dataSource.Add(dt2.Rows[i][0].ToString());
                    }
                    this.txtSearch1.AutoCompleteCustomSource = dataSource;
                }
            }

            this.txtSearch1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtSearch1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtSearch1.RightToLeft = System.Windows.Forms.RightToLeft.No;
        }




        public void GetData()
        {
            try
            {
                // تحقق من النص قبل البحث
                string searchText = txtSearch1.Text.Trim();

                // الاستعلام باستخدام Parameter لتجنب SQL Injection
                string qry = "SELECT * FROM category WHERE catName LIKE @SearchText";

                // إعداد الأعمدة المراد تحميلها
                ListBox lb = new ListBox();
                lb.Items.Add(dgvid);
                lb.Items.Add(dgvName);

                using (SqlConnection con = MainClass.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                        MainClass.LoadData(cmd, guna2DataGridView1, lb);
                    }
                }
            }
            catch (Exception ex)
            {
                // عرض رسالة خطأ
                MessageBox.Show("حدث خطأ: " + ex.Message);
                return;
            }
        }

        private void frmCategoryView_Load(object sender, EventArgs e)
        {
            int panelSize = topPanel.Size.Width;
            int txtSearchSize = txtSearch.Size.Width;
            int z = (panelSize - txtSearchSize) / 2;
            txtSearch.Location = new Point(z, 35);
            txtSearch.CustomizableEdges.BottomLeft = true;

            GetData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmCategoryAdd frm = new frmCategoryAdd();
            frm.ShowDialog();
            //MainClass.BlureBackground(new frmCategoryAdd());

            GetData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                    string qry = @"SELECT categoryID FROM products WHERE categoryID = @catID";
                    DataTable dt2 = new DataTable();

                    using (SqlConnection con = MainClass.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@catID", id);
                        cmd.CommandType = CommandType.Text;

                        using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                        {
                            da2.Fill(dt2);
                        }
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        Notifier.ShowNotification("Error ❌", "هذا الصنف يحتوي علي منتجات لا يمكن حذفه");
                        return;
                    }

                    if (guna2MessageDialog1.Show("هل تريد حذه هذا الصنف") == DialogResult.Yes)
                    {
                        Hashtable ht3 = new Hashtable();
                        string qry3 = @"Insert into rconrdEditingPro Values(@posName, @editeIn,@editeTo, @tableName , @typeEdit,@date ,@time); Select SCOPE_IDENTITY()";

                        ht3.Add("@posName", MainClass.USER);
                        ht3.Add("@editeIn", Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value));
                        ht3.Add("@editeTo", DBNull.Value);
                        ht3.Add("@tableName", "صنف");
                        ht3.Add("@typeEdit", "حذف");
                        ht3.Add("@date", Convert.ToDateTime(DateTime.Now.Date));
                        ht3.Add("@time", Convert.ToString(DateTime.Now.ToShortTimeString()));
                        MainClass.SQL(qry3, ht3);

                        string qry2 = "Delete from category where catID = " + id + "";
                        Hashtable ht = new Hashtable();
                        MainClass.SQL(qry2, ht);

                        Notifier.ShowNotification("Done ✅", "تم الحذف بنجاح");
                        GetData();
                    }
                }
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
                return;
            }
        }


        private void MyForm_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {

            if (InputLanguage.CurrentInputLanguage.Culture.TwoLetterISOLanguageName == "ar")
            {
                txtSearch1.RightToLeft = RightToLeft.No;
            }
            else
            {
                txtSearch1.RightToLeft = RightToLeft.Yes;
            }
        }

        
        private void guna2DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvName")
            {
                frmBlackout frmBlackout1 = new frmBlackout(this);
                frmBlackout1.StartPosition = FormStartPosition.Manual;
                frmBlackout1.Location = this.Location;
                frmBlackout1.Size = this.Size;
                frmBlackout1.Show(this);

                frmCategoryAdd frm = new frmCategoryAdd();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                frm.cat = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);

                frm.ShowDialog(this);

                frmBlackout1.Close();

                GetData();


            }


        }
    }
}
