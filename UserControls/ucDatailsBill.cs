using DevExpress.CodeParser;
using Guna.UI2.WinForms;
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

namespace pos.UserControls
{
    public partial class ucDatailsBill : UserControl
    {
        public ucDatailsBill()
        {
            InitializeComponent();
            dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);


        }
        public event EventHandler onSelectDel = null;
        public int mainID { get; set; }


        private void UpdateHeight()
        {
            int rowHeight = 23;
            int rowCount = dataGridView1.Rows.Count;

            int newHeight = (rowCount) * rowHeight; // الحد الأدنى للارتفاع

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
            string qry2 = @"SELECT p.pName, d.qty ,d.price,d.amount ,d.unite ,d.pDescount
                            FROM tblMain1 m INNER JOIN tblDetails d ON m.MainID = d.MainID INNER JOIN products p ON p.pID = d.proID
                               WHERE m.MainID = " + mainID ;

            //SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
            //cmd2.Parameters.AddWithValue("@ID", mainID);
            //cmd2.Connection = MainClass.con;
            //cmd2.CommandType = CommandType.Text;

            //DataTable dt2 = new DataTable();
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //da2.Fill(dt2);

            ListBox lb = new ListBox();
            lb.Items.Add(dgv2Name);
            lb.Items.Add(dgv2Qty);
            lb.Items.Add(dgv2Unite);
            lb.Items.Add(dgv2UnitPrice);
            lb.Items.Add(dgv2Dv);
            lb.Items.Add(dgv2Total);
            MainClass.LoadData(qry2, guna2DataGridView3, lb);

            //for (int i = 0; i < lb.Items.Count; i++)
            //{
            //    String colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
            //    dataGridView1.Columns[colNam1].DataPropertyName = dt2.Columns[i].ToString();
            //}

            //dataGridView1.DataSource = dt2;




        }





        private void ucDatailsBill_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();

           
            getData();

            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.AllowUserToResizeRows = true;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True; 

            UpdateHeight();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;


            if (guna2MessageDialog1.Show(" هل تريد حذه هذه الفاتورة ") == DialogResult.Yes)
                onSelectDel?.Invoke(this, e);
        }
    }
}
