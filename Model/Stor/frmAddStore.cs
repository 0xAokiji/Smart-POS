using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using pos.Classes;
using pos.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;


namespace pos.Model
{
    public partial class frmAddStore : Form
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        public int id = 0;


        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;



        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public frmAddStore()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            iconImage.Image = Properties.Resources.store_Dark;

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

        private void frmAddStore_Load(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string qry;

            if (id == 0)
            {
                qry = "INSERT INTO addStore (storeName, storeNumber) VALUES (@Name, @Number)";
            }
            else
            {
                qry = "UPDATE addStore SET storeName = @Name, storeNumber = @Number WHERE storeID = @id";
            }

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Number", int.Parse(txtCode.Text.Trim()));

                if (id != 0)
                    cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    Notifier.ShowNotification("Done ✅", id == 0 ? "تم إضافة المخزن بنجاح" : "تم تعديل المخزن بنجاح");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("حدث خطأ أثناء الحفظ ❌", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

            if (MainClass.ThemeMode == "dark")
            {
                iconImage.Image = Properties.Resources.store_Dark;

            }
            else if (MainClass.ThemeMode == "light")
            {
                iconImage.Image = Properties.Resources.store_Light;

            }

            ThemeColor();

            this.BackColor = backgroundPrimary;

            //Panels
            bottomPanel.BackColor = backgroundSecondary;
            topPanel.BackColor = checkedFillColor;

            iconImage.BackColor = checkedFillColor;

            //Lables
            lblTitle.ForeColor = textColor;
            lblName.ForeColor = textColor;
            lblCode.ForeColor = textColor;

            //Text box
            txtName.BackColor = backgroundPrimary;
            txtName.ForeColor = textColor2;
            txtName.BorderColor = checkedFillColor;
            txtName.FillColor = backgroundPrimary;

            txtCode.BackColor = backgroundPrimary;
            txtCode.ForeColor = textColor2;
            txtCode.BorderColor = checkedFillColor;
            txtCode.FillColor = backgroundPrimary;


            //->Button  
            btn_Close.FillColor = Color.Red;
            btn_Close.ForeColor = textColor;

            btn_Save.FillColor = checkedFillColor;
            btn_Save.ForeColor = textColor;

        }
    }
}
