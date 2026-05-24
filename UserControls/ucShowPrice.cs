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

namespace pos.UserControls
{
    public partial class ucShowPrice : UserControl
    {
        public event EventHandler onClick= null;
        public ucShowPrice()
        {
            InitializeComponent();
        }

        public int id { get; set; }

        public string PName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }
        public string pPrice
        {
            get { return lblPrice.Text; }
            set { lblPrice.Text = value; }
        }
        public string pNumber
        {
            get { return lblNumber.Text; }
            set { lblNumber.Text = value; }
        }
        public string pQty
        {
            get { return lblQty.Text; }
            set { lblQty.Text = value; }
        }


        private void ucShowPrice_Load(object sender, EventArgs e)
        {
            this.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int radius = 10;

                Rectangle corner1 = new Rectangle(0, 0, radius * 2, radius * 2);
                Rectangle corner2 = new Rectangle(this.Width - radius * 2, 0, radius * 2, radius * 2);
                Rectangle corner3 = new Rectangle(0, this.Height - radius * 2, radius * 2, radius * 2);
                Rectangle corner4 = new Rectangle(this.Width - radius * 2, this.Height - radius * 2, radius * 2, radius * 2);

                path.AddArc(corner1, 180, 90);
                path.AddArc(corner2, 270, 90);
                path.AddArc(corner4, 0, 90);
                path.AddArc(corner3, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            };
            int x = guna2Panel1.Size.Width;
            int x2 = lblName.Size.Width;
            int z = (x - x2) / 2;
            lblName.Location = new Point(z, 5);
        }

        private void guna2Panel1_Click(object sender, EventArgs e)
        {
            onClick?.Invoke(this, e);
        }
    }
}
