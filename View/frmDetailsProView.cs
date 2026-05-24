using DevExpress.XtraEditors;
using pos.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.View
{
    public partial class frmDetailsProView : DevExpress.XtraEditors.XtraForm
    {
        public frmDetailsProView()
        {
            InitializeComponent();
            this.TopMost = true;

        }

        public int mainID;
        public bool Bill = false;
        
        private void UpdateHeight()
        {
            int rowHeight = 23;
            int rowCount = dataGridView1.Rows.Count;

            int newHeight = (rowCount ) * rowHeight; // الحد الأدنى للارتفاع

            this.Height = newHeight; // تعديل ارتفاع النموذج

            panel1.Height = newHeight;

            dataGridView1.Height = newHeight;
        }


        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateHeight();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateHeight();
        }

        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }
        private void getData()
        {
            string qry2 = @"SELECT p.pName, d.qty ,d.amount
                    FROM tblMain1 m 
                    INNER JOIN tblDetails d ON m.MainID = d.MainID 
                    INNER JOIN products p ON p.pID = d.proID
                    WHERE m.MainID = @ID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd2 = new SqlCommand(qry2, con))
            {
                cmd2.Parameters.AddWithValue("@ID", mainID);
                cmd2.CommandType = CommandType.Text;

                DataTable dt2 = new DataTable();
                using (SqlDataAdapter da2 = new SqlDataAdapter(cmd2))
                {
                    da2.Fill(dt2);
                }

                ListBox lb = new ListBox();
                lb.Items.Add(dgvName);
                lb.Items.Add(dgvQty);
                lb.Items.Add(dgvPrice);
                lb.Items.Add(dgvTotal);

                dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);

                try
                {
                    for (int i = 0; i < lb.Items.Count; i++)
                    {
                        string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                        dataGridView1.Columns[colNam1].DataPropertyName = dt2.Columns[i].ToString();
                    }

                    dataGridView1.DataSource = dt2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }


        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDetailsProView_Load(object sender, EventArgs e)
        {
            this.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int radius = 20; // قطر الدائرة التي تحدد منحنى الحواف

                // أركان النافذة
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
            getData();

            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.AllowUserToResizeRows = true;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // تفعيل السطور الملتفة
            dataGridView1.ClearSelection();

            UpdateHeight();
           

        }

   

    }
}