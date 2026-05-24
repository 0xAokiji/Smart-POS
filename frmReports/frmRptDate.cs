using DevExpress.DocumentServices.ServiceModel.DataContracts;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace pos.Model
{
    public partial class frmRptDate : SampleReport
    {

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public frmRptDate()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            // تغيير خصائص النافذة لمنع ظهورها في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            btnClose.CustomizableEdges.TopRight = true;
            btnSave.CustomizableEdges.TopRight = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK; // يمكنك استخدام أي قيمة تراها مناسبة

                int rowCount = 0;

                using (SqlConnection con1 = MainClass.GetConnection())
                {
                    con1.Open();
                    string qry = @"SELECT COUNT(*) AS 'RowCount' FROM [dateTime]";
                    using (SqlCommand cmd = new SqlCommand(qry, con1))
                    {
                        rowCount = (int)cmd.ExecuteScalar();
                    }
                }

                if (rowCount <= 0)
                {
                    Hashtable ht = new Hashtable();
                    string qry2 = @"INSERT INTO [dateTime] (startDate, endDate) VALUES (@startDate, @endDate)";
                    ht.Add("@startDate", dateTimePicker1.Value);
                    ht.Add("@endDate", dateTimePicker2.Value);
                    MainClass.SQL(qry2, ht);
                }
                else
                {
                    Hashtable ht2 = new Hashtable();
                    string qry3 = @"UPDATE [dateTime] 
                            SET startDate = @startDate, endDate = @endDate 
                            WHERE ID = (SELECT MAX(ID) FROM [dateTime])";
                    ht2.Add("@startDate", Convert.ToDateTime(dateTimePicker1.Value.ToString("yyyy-MM-dd")).Date);
                    ht2.Add("@endDate", Convert.ToDateTime(dateTimePicker2.Value.ToString("yyyy-MM-dd")).Date);
                    MainClass.SQL(qry3, ht2);
                }

                this.Close();
                frmHome frmHome = new frmHome();
            }
            catch (Exception)
            {
                MessageBox.Show("حدث خطأ أثناء الحفظ");
                return;
            }
        }



        private void frmRptPurches_Load(object sender, EventArgs e)
        {
            this.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int radius = 12; // قطر الدائرة التي تحدد منحنى الحواف

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

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
