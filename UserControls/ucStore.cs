using DevExpress.Drawing;
using DevExpress.XtraReports.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Model
{
    public partial class ucStore : UserControl
    {
       

        public event EventHandler onSelectDel = null;
        public event EventHandler onSelectEdit = null;
        public event EventHandler click1 = null;
        public event EventHandler click2 = null;

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color textColor2;
        private Color checkedFillColor;
        private Color checkedForeColor;
        public ucStore()
        {
            InitializeComponent();

            ThemeMode();

        }
        public void themRefresh()
        {
           
            ThemeMode();
        }
        public int id { get; set; }

        public string PName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }
        public string pprice
        {
            get { return lblPrice.Text; }
            set { lblPrice.Text = value; }
        }
        public string PCategory
        {
            get { return lblCat.Text; }
            set { lblCat.Text = value; }
        }
        public string pWholPrice
        {
            get { return lblWhol.Text; }
            set { lblWhol.Text = value; }
        }
        public string pQty
        {
            get { return lblQty.Text; }
            set { lblQty.Text = value; }
        }

        public Image PImage
        {
            get { return imgProduct.Image; }
            set { imgProduct.Image = value; }
        }

        private void txtImage_Click(object sender, EventArgs e)
        {
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            onSelectDel?.Invoke(this, e);

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            onSelectEdit?.Invoke(this, e);

        }
        public void SetWholePrice(string price)
        {
            this.pprice = price;
            lblPrice.Text = price;

            checkBox.Visible = false;
            btnEdite.Visible = true;
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

        private void ucStore_Load(object sender, EventArgs e)
        {
            
            int x = bottomPanel.Size.Width;
            int x2 = lblName.Size.Width;
            int z = (x - x2) / 2;
            lblName.Location = new Point(z, 2);
        }
        private bool checkedB = true;
        private void panel1_Click(object sender, EventArgs e)
        {

            if (checkedB)
            {
                checkedB = false;
                checkBox.Visible = true;
                checkBox.Checked = true;
                btnEdite.Visible = false;
            }
            else
            {
                checkedB = true;
                checkBox.Visible = false;
                btnEdite.Visible = true;

            }

            click1?.Invoke(this, e);

        }

        private void imgProduct_Click(object sender, EventArgs e)
        {
            if (checkedB)
            {
                checkedB = false;
                checkBox.Visible = true;
                checkBox.Checked = true;
                btnEdite.Visible = false;
            }
            else
            {
                checkedB = true;
                checkBox.Visible = false;
                btnEdite.Visible = true;

            }
            click2?.Invoke(this, e);

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

            ThemeColor();

            this.BackColor = backgroundPrimary;

            //Panels
            bottomPanel.BackColor = backgroundSecondary;
            topPanel.BackColor = checkedFillColor;

            btnDel.BackColor = checkedFillColor;
            btnEdite.BackColor = checkedFillColor;
            imgProduct.BackColor = backgroundPrimary;



            //Lables
            label2.ForeColor = textColor;
            label3.ForeColor = textColor;
            label4.ForeColor = textColor;
            label5.ForeColor = textColor;
            lblCat.ForeColor = textColor;
            lblQty.ForeColor = textColor;
            lblWhol.ForeColor = textColor;
            lblPrice.ForeColor = textColor;
            lblName.ForeColor = textColor;


           
        }
    }
}
