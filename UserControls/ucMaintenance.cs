using DevExpress.CodeParser;
using Guna.UI2.WinForms;
using pos.Classes;
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

namespace pos.Model
{
    public partial class ucMaintenance : UserControl
    {
        public ucMaintenance()
        {
            InitializeComponent();
        }

        public event EventHandler onSelectEdit = null;
        public event EventHandler onSelectFinsh = null;
        public event EventHandler onRejected = null;
        public event EventHandler onBill = null;
        public event EventHandler onAbout = null;

        private bool isRun = false;



        public int id { get; set; }
        public int billID { get; set; }


        public string pName
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public string pPhone
        {
            get { return lblPhone.Text; }
            set { lblPhone.Text = value; }
        }
        public string maint_Techn
        {
            get { return lblMainten.Text; }
            set { lblMainten.Text = value; }
        }
        public string state
        {
            get { return lblState.Text; }
            set { lblState.Text = value; }
        }
        public string priority
        {
            get { return lblPriority.Text; }
            set { lblPriority.Text = value; }
        }
        public string taskNumber
        {
            get { return txtTaskNumber.Text; }
            set { txtTaskNumber.Text = value; }
        }
        public string paymentStatus
        {
            get { return txtPaymentStatus.Text; }
            set { txtPaymentStatus.Text = value; }
        }



        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            onSelectEdit?.Invoke(this, e);

        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            onSelectFinsh?.Invoke(this, e);
        }


        private void ucMaintenance_Load(object sender, EventArgs e)
        {
            // إذا كانت الحالة "قيد التنفيذ" => نعرض زر الإيقاف (pause)
            if (lblState.Text == "قيد التنفيذ")
            {
                isRun = true;
                btnRun.Image = Properties.Resources.pause;
            }
            else // غير ذلك => نعرض زر التشغيل (play)
            {
                isRun = false;
                btnRun.Image = Properties.Resources.play;
            }
            if (paymentStatus == "مدفوع")
                btnPay.Enabled = false;
            else
                btnPay.Enabled = true;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qry = @"
                UPDATE Task 
                SET status = @status
                WHERE taskID = @taskID;";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@taskID", id);

                        // إذا المهمة قيد التنفيذ -> نوقفها
                        if (isRun)
                        {
                            isRun = false;
                            btnRun.Image = Properties.Resources.play;
                            cmd.Parameters.AddWithValue("@status", "قيد الانتظار");
                            lblState.Text = "قيد الانتظار";
                            Notifier.ShowNotification("تم التحديث ✔️", "تم تحديث حالة المهمة إلى 'قيد الانتظار'.");
                        }
                        else // إذا كانت متوقفة -> نبدأ تشغيلها
                        {
                            isRun = true;
                            btnRun.Image = Properties.Resources.pause;
                            cmd.Parameters.AddWithValue("@status", "قيد التنفيذ");
                            lblState.Text = "قيد التنفيذ";
                            Notifier.ShowNotification("تم التحديث ✔️", "تم تحديث حالة المهمة إلى 'قيد التنفيذ'.");
                        }

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        int rows = cmd.ExecuteNonQuery();

                        if (rows <= 0)
                            Notifier.ShowNotification("تنبيه ⚠️", "لم يتم العثور على المهمة المحددة.");

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ أثناء تحديث الحالة: " + ex.Message);
            }
        }


        private void btnRejected_Click(object sender, EventArgs e)
        {
            onRejected?.Invoke(this, e);

        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            onBill?.Invoke(this, e);

        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            onAbout?.Invoke(this, e);

        }
    }
}
