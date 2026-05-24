using DevExpress.CodeParser;
using System;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace pos.Model
{
    public partial class frmPriceIncrease : Form
    {
        List<int> listId;
        private bool check2 = false;

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public frmPriceIncrease(List<int> lisID)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            ThemeMode();

            listId = lisID;
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

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {


                if (!check2)
                {
                    btn_Save.PerformClick();
                    check2 = true;
                    e.Handled = true;
                }

            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                btn_Close.PerformClick();

                e.Handled = true;
            }

            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void update()
        {
            string qry = string.Empty;
            if (rbValue.Checked == true) 
                qry = @"Update stor Set sellPrice = sellPrice + @price  where pID = @id;
                        Update products Set sellPrice = sellPrice + @price  where pID = @id";
            else if(rbPercent.Checked == true)
                qry = @"
                    UPDATE stor 
                    SET sellPrice = sellPrice + (sellPrice * @price / 100) 
                    WHERE pID = @id;

                    UPDATE products 
                    SET sellPrice = sellPrice + (sellPrice * @price / 100) 
                    WHERE pID = @id;";

            foreach (var id in listId)
            {
                Hashtable ht = new Hashtable();
                ht.Add("@id", id);
                ht.Add("@price", Convert.ToInt32(txtPrice.Text));

                MainClass.SQL(qry, ht);
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            update();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmPriceIncrease_Load(object sender, EventArgs e)
        {
           
        }

        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            textColor2 = MainClass.TextColor2;
            checkedFillColor = MainClass.CheckedFillColor;
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


            //Text box
            txtPrice.BackColor = backgroundPrimary;
            txtPrice.ForeColor = textColor2;
            txtPrice.BorderColor = checkedFillColor;
            txtPrice.FillColor = backgroundPrimary;


            //->Button  
            btn_Close.FillColor = Color.Red;
            btn_Close.ForeColor = textColor;

            btn_Save.FillColor = checkedFillColor;
            btn_Save.ForeColor = textColor;

            rbPercent.CheckedState.FillColor = checkedFillColor;
            rbPercent.ForeColor = textColor;
            rbPercent.CheckedState.BorderColor = checkedFillColor;

            rbValue.CheckedState.FillColor = checkedFillColor;
            rbValue.ForeColor = textColor;
            rbValue.CheckedState.BorderColor = checkedFillColor;

        }
    }
}
