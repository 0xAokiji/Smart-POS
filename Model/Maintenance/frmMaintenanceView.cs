using DevExpress.XtraEditors;
using pos.Analysis_Forms;
using pos.Classes;
using pos.GeneralForms.MainForm;
using pos.Model;
using pos.Model.Maintenance;
using pos.Model.POS;
using pos.UserControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pos.View
{
    public partial class frmMaintenanceView : SampleView
    {
        private int position = 0;
        public frmMaintenanceView()
        {
            InitializeComponent();
        }
        private int id = 0;
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            using (frmMaintenanceAdd frm = new frmMaintenanceAdd())
            {
                frm.FormClosed += Frm_FormClosed;
                frm.ShowDialog();
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

        }
        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {

            GetData(position);
        }
        public void GetData(int position)
        {
            try
            {
                storPanel.Controls.Clear();
                string qry1 = @"
                SELECT 
                    T.taskID,
                    T.mainID,
                    T.taskNumber,
                    T.descriptionProblem,
                    T.Priority,
                    T.PriorityName,
                    T.taskPrice,
                    T.status,
                    T.paymentStatus,
                    T.startDate,
                    T.startTime,
                    T.endDate,
                    T.endTime,
                    P.pName       AS CustomerName,
                    P.pPhone      AS CustomerPhone,
                    P.pID         AS CustomerID,
                    S.sName       AS TechnicianName,
                    S.sPhone      AS TechnicianPhone
                FROM [dbo].[Task] AS T
                INNER JOIN [dbo].[Parties] AS P 
                    ON T.paryID = P.pID
                INNER JOIN [dbo].[staff] AS S
                    ON T.tecnicalID = S.staffID
                ";

                // ✅ هنا نضيف الشرط بدل ما نبدّل الاستعلام كله
                if (position == 0)
                {
                    qry1 += @"
                    WHERE 
                        T.status NOT IN (N'تم التسليم', N'مرفوض', N'انهاء')";
                }
                else
                {
                    qry1 += @"
                    WHERE 
                        T.status IN (N'انهاء')";
                }

                qry1 += @"
                ORDER BY 
                T.Priority DESC,
                T.startDate DESC,
                T.startTime DESC;
                    ";

                using (SqlConnection con = MainClass.GetConnection())
                using (SqlCommand cmd1 = new SqlCommand(qry1, con))
                {
                    DataTable dt1 = new DataTable();
                    using (SqlDataAdapter da1 = new SqlDataAdapter(cmd1))
                    {
                        da1.Fill(dt1);
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt1.Rows)
                        {
                            int taskID = Convert.ToInt32(row["taskID"]);

                            bool exists = storPanel.Controls
                                .OfType<ucMaintenance>()
                                .Any(p => p.id == taskID);

                            if (!exists)
                            {


                                var s = new ucMaintenance()
                                {
                                    pName = row["CustomerName"].ToString(),
                                    pPhone = row["CustomerPhone"].ToString(),
                                    id = Convert.ToInt32(row["taskID"]),
                                    billID = row["mainID"] != DBNull.Value ? Convert.ToInt32(row["mainID"]) : 0,
                                    maint_Techn = row["TechnicianName"].ToString(),
                                    state = row["status"].ToString(),
                                    priority = row["PriorityName"].ToString(),
                                    taskNumber = row["taskNumber"].ToString(),
                                    paymentStatus = row["paymentStatus"].ToString()

                                };

                                s.Size = new Size(275, 212);
                                storPanel.Controls.Add(s);

                                if (position != 0)
                                {
                                    if(row["paymentStatus"].ToString() == "غير مدفوع")
                                        s.btnEnd.Enabled = false;
                                    else
                                        s.btnEnd.Enabled = true;

                                    s.btnRejected.Enabled = false;
                                    s.btnEnd.Text = "تسليم";
                                    s.btnRun.Visible = false;
                                }
                                // هنا باقي الأحداث بدون تغيير
                                s.onSelectEdit += (ss, ee) =>
                                {
                                    if (MainClass.EditeDevice)
                                    {
                                        frmBlackout frmBlackout = new frmBlackout(this);
                                        frmBlackout.Show();
                                        frmBlackout.Owner = this;
                                        using (frmMaintenanceAdd frm = new frmMaintenanceAdd())
                                        {
                                            frm.id = int.Parse(row["taskID"].ToString());
                                            frm.FormClosed += Frm_FormClosed;
                                            frm.ShowDialog();
                                        }
                                        this.Focus();
                                        frmBlackout.Close();
                                    }
                                    else
                                    {
                                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                        guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                                        guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                                    }
                                };

                                s.onBill += (ss, ee) =>
                                {
                                    int mainID = row["mainID"] == DBNull.Value || string.IsNullOrWhiteSpace(row["mainID"].ToString())
                                         ? 0
                                         : Convert.ToInt32(row["mainID"]);

                                    int customerID = row["CustomerID"] == DBNull.Value || string.IsNullOrWhiteSpace(row["CustomerID"].ToString())
                                        ? 0
                                        : Convert.ToInt32(row["CustomerID"]);

                                    int taskID = row["taskID"] == DBNull.Value || string.IsNullOrWhiteSpace(row["taskID"].ToString())
                                        ? 0
                                        : Convert.ToInt32(row["taskID"]);

                                    decimal taskPrice = row["taskPrice"] == DBNull.Value || string.IsNullOrWhiteSpace(row["taskPrice"].ToString())
                                        ? 0
                                        : Convert.ToDecimal(row["taskPrice"]);

                                    string customerName = row["CustomerName"]?.ToString() ?? "";

                                    bool isPaid = LoadInvoiceData(mainID, customerName, customerID, taskID, taskPrice);
                                    if(isPaid)
                                        s.btnEnd.Enabled = true;


                                };

                                s.onSelectFinsh += (ss, ee) =>
                                {
                                    if (position == 0)
                                    {
                                        taskEnd(taskID, row["taskNumber"]?.ToString() ?? "غير معروف");
                                    }
                                    else
                                    {
                                        deliverd(taskID, row["taskNumber"]?.ToString() ?? "غير معروف");
                                    }
                                };

                                s.onRejected += (ss, ee) =>
                                {

                                    try
                                    {
                                        using (SqlConnection con = MainClass.GetConnection())
                                        {
                                            string qry = @"
                                            UPDATE Task 
                                            SET 
                                                status = @status,
                                                endDate = @endDate,
                                                endTime = @endTime
                                            WHERE taskID = @taskID;
                                            ";

                                            using (SqlCommand cmd = new SqlCommand(qry, con))
                                            {
                                                // تمرير المعاملات (Parameters)
                                                cmd.Parameters.AddWithValue("@taskID", taskID); // رقم المهمة اللي عايز تعدلها
                                                cmd.Parameters.AddWithValue("@status", "مرفوض"); // الحالة الجديدة
                                                cmd.Parameters.AddWithValue("@endDate", DateTime.Now.Date); // التاريخ الحالي
                                                cmd.Parameters.AddWithValue("@endTime", DateTime.Now.ToShortTimeString()); // الوقت الحالي

                                                if (con.State == ConnectionState.Closed)
                                                    con.Open();

                                                int rows = cmd.ExecuteNonQuery();

                                                if (rows > 0)
                                                {
                                                    DeleteTheTask(taskID);
                                                    var taskNumber = row["taskNumber"]?.ToString() ?? "غير معروف";
                                                    Notifier.ShowNotification("تم التحديث ✅", $"تم رفض المهمة رقم {taskNumber} بنجاح.");

                                                }
                                                else
                                                    Notifier.ShowNotification("تنبيه ⚠️", "لم يتم العثور على المهمة المحددة.");

                                                con.Close();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Notifier.ShowNotification("Error ❌", "حدث خطأ أثناء تحديث الحالة: " + ex.Message);
                                    }

                                };
                                s.onAbout += (ss, ee) =>
                                {

                                    frmMaintenaceAbout frm = new frmMaintenaceAbout();
                                    frm.taskID = taskID;

                                    frm.ShowDialog();
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
                return;
            }
        }
        private void taskEnd(int taskID, string tasknumber)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qry = @"
                                            UPDATE Task 
                                            SET 
                                                status = @status,
                                                endDate = @endDate,
                                                endTime = @endTime
                                            WHERE taskID = @taskID;
                                            ";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        // تمرير المعاملات (Parameters)
                        cmd.Parameters.AddWithValue("@taskID", taskID); // رقم المهمة اللي عايز تعدلها
                        cmd.Parameters.AddWithValue("@status", "انهاء"); // الحالة الجديدة
                        cmd.Parameters.AddWithValue("@endDate", DateTime.Now.Date); // التاريخ الحالي
                        cmd.Parameters.AddWithValue("@endTime", DateTime.Now.ToShortTimeString()); // الوقت الحالي

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            DeleteTheTask(taskID);
                            var taskNumber = tasknumber;
                            Notifier.ShowNotification("تم تسليم ✅", $"تم الانتهاء من المهمة رقم {taskNumber} بنجاح.");

                        }
                        else
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
        private void deliverd(int taskID, string tasknumber)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qry = @"
                                            UPDATE Task 
                                            SET 
                                                status = @status,
                                                endDate = @endDate,
                                                endTime = @endTime
                                            WHERE taskID = @taskID;
                                            ";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        // تمرير المعاملات (Parameters)
                        cmd.Parameters.AddWithValue("@taskID", taskID); // رقم المهمة اللي عايز تعدلها
                        cmd.Parameters.AddWithValue("@status", "تم التسليم"); // الحالة الجديدة
                        cmd.Parameters.AddWithValue("@endDate", DateTime.Now.Date); // التاريخ الحالي
                        cmd.Parameters.AddWithValue("@endTime", DateTime.Now.ToShortTimeString()); // الوقت الحالي

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            DeleteTheTask(taskID);
                            var taskNumber = tasknumber;

                        }
                        else
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
        private void DeleteTheTask(int taskID)
        {
            // أولا، ابحث عن الـ UserControl الذي يحتوي على هذا المنتج بناءً على الـ productId
            foreach (var control in storPanel.Controls)
            {
                if (control is ucMaintenance taskControles)
                {
                    if (taskControles.id == taskID)
                    {
                        // إذا كانت الكمية صفر، احذف الـ UserControl من الـ FlowLayoutPanel
                        storPanel.Controls.Remove(taskControles);

                        break;  // لا حاجة للبحث أكثر
                    }
                }
            }
        }
        private bool LoadInvoiceData(int mainID, string partyName, int partyID, int taskID, decimal Benefits)
        {
            int partiesID;
            decimal totalWithInterest;
            decimal total;
            decimal discountValue;
            string invoiceCode = "";
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    string query = @"
                    SELECT 
                        partiesID,
                        InvoiceCode,
                        TotalWithInterest,
                        total,
                        descountValue
                    FROM tblMain1
                    WHERE MainID = @MainID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@MainID", mainID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 🔹 وضع القيم في متغيرات (حسب ما تحتاج)
                                partiesID = Convert.ToInt32(reader["partiesID"]);
                                totalWithInterest = Convert.ToDecimal(reader["TotalWithInterest"]);
                                total = Convert.ToDecimal(reader["total"]);
                                discountValue = Convert.ToDecimal(reader["descountValue"]);
                                invoiceCode = reader["InvoiceCode"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("⚠️ لم يتم العثور على فاتورة بهذا الرقم.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }
                }

                using (frmBlackout frmBlackout = new frmBlackout(this))
                {
                    frmBlackout.Show();

                    using (frmPayWays frm = new frmPayWays())
                    {
                        frm.mainID = mainID;
                        frm.partyType = "عميل";


                        frm.totalClean = totalWithInterest + Benefits;
                        frm.total = total;
                        frm.isTaskBill = true;
                        frm.lblBenefite.Text = "اجر العامل";
                        frm.lblClean.Text = "الاجمالي + اجر العامل";
                        frm.txtBenefits.Text = Benefits.ToString();
                        frm.discountValue = discountValue;
                        frm.invoiceCode = invoiceCode;
                        frm.partyName = partyName;
                        frm.selectedPartyID = partyID;
                        frm.btnNext1.Enabled = true;
                        frm.txtName.Enabled = false;
                        frm.btnUnknow.Enabled = false;
                        frm.btnSearch.Enabled = false;
                        frm.btnAddParties.Visible = false;
                        frm.btnEditParties.Visible = false;

                        frm.status = "new";

                        frm.Owner = this;
                        DialogResult result = frm.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            try
                            {
                                using (SqlConnection con = MainClass.GetConnection())
                                {
                                    string qry = @"
                                            UPDATE Task 
                                            SET 
                                                paymentStatus = @paymentStatus
                                            WHERE taskID = @taskID;
                                            ";

                                    using (SqlCommand cmd = new SqlCommand(qry, con))
                                    {
                                        // تمرير المعاملات (Parameters)
                                        cmd.Parameters.AddWithValue("@taskID", taskID); // رقم المهمة اللي عايز تعدلها
                                        cmd.Parameters.AddWithValue("@paymentStatus", "مدفوع"); // التاريخ الحالي

                                        if (con.State == ConnectionState.Closed)
                                            con.Open();

                                        cmd.ExecuteNonQuery();


                                        con.Close();
                                    }
                                }
                                GetData(position);

                            }
                            catch (Exception ex)
                            {
                                Notifier.ShowNotification("Error ❌", "حدث خطأ أثناء تحديث الحالة: " + ex.Message);
                                return false;
                            }

                            Notifier.ShowNotification("تم الدفع", "تم انهاء حساب المهمة بنجاح ✅");


                            frmShowBackup frmshowBackup = new frmShowBackup();
                            frmshowBackup.backupType = "DIFFERENTIAL";
                            frmshowBackup.showNotification = false;
                            frmshowBackup.ShowDialog(this);
                            return true;
                        }
                        else
                            return false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل بيانات الفاتورة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GetData(position);
        }

        private void frmMaintenanceView_Load(object sender, EventArgs e)
        {
            btnMaint.Checked = true;
            txtSearch1.Visible = true;
            GetData(position);
        }

        private void AddPruch_Click(object sender, EventArgs e)
        {
            frmBlackout frmBlackout = new frmBlackout(this);
            frmBlackout.Show();
            frmBlackout.Owner = this;
            using (frmMaintenanceAdd frm = new frmMaintenanceAdd())
            {
                frm.FormClosed += Frm_FormClosed;
                frm.ShowDialog();
            }
            this.Focus();
            frmBlackout.Close();
        }

        private void showpurRep_Click(object sender, EventArgs e)
        {

        }



        private void اضافهجهازاليالصيانهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMaintenanceAdd frm = new frmMaintenanceAdd())
            {
                frm.FormClosed += Frm_FormClosed;
                frm.ShowDialog();
            }
        }

        private void انوعالصيانهToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void اغلاقالبرنامجToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void dTec_Click(object sender, EventArgs e)
        {
            if (MainClass.RebortMaint)
            {
                btnMaint.Checked = false;
                btnEndTasks.Checked = true;
                txtSearch1.Visible = false;
                storPanel.Visible = false;
                //AddControls(new frmStaffRecord()); 
                btnMaint.Checked = false;
                btnEndTasks.Checked = true;
                txtSearch1.Visible = false;
                storPanel.Visible = false;
                //AddControls(new frmStaffRecord());
            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");

            }
        }

        

        private void btnMaint_Click(object sender, EventArgs e)
        {

            //txtSearch1.Visible = true;
            storPanel.Visible = true;
            position = 0;
            panelReport.Controls.Clear(); // امسح الموجود (أو علّق هذا السطر لو حابب تحتفظ بالباقي)
            storPanel.Dock = DockStyle.Fill;
            panelReport.Controls.Add(storPanel);
            txtSearch1.Visible = true;

            GetData(position);
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            frmMaintenanceAdd frm = new frmMaintenanceAdd();
            frm.ShowDialog();
        }

        private void btnEndTasks_Click(object sender, EventArgs e)
        {

            //txtSearch1.Visible = true;
            storPanel.Visible = true;
            position = 1;

            panelReport.Controls.Clear(); // امسح الموجود (أو علّق هذا السطر لو حابب تحتفظ بالباقي)
            storPanel.Dock = DockStyle.Fill;
            panelReport.Controls.Add(storPanel);
            txtSearch1.Visible = true;
            GetData(position);
        }
        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();

        public void AddControls(Form f)
        {
            // تحقق هل الفورم موجود بالفعل في mainpanel
            foreach (Control ctrl in panelReport.Controls)
            {
                if (ctrl is Form existingForm && existingForm.Name == f.Name)
                {
                    existingForm.BringToFront(); // اعرضه في الواجهة فقط
                    return;
                }
            }

            // لو مش موجود، أضفه
            panelReport.Controls.Clear(); // امسح الموجود (أو علّق هذا السطر لو حابب تحتفظ بالباقي)
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            panelReport.Controls.Add(f);
            f.Show();

            // خزنه في openedForms
            if (openedForms.ContainsKey(f.Name))
            {
                openedForms[f.Name] = f; // تحديث الفورم الموجود
            }
            else
            {
                openedForms.Add(f.Name, f); // إضافة جديد
            }
        }
        private void btnRecordes_Click(object sender, EventArgs e)
        {
            txtSearch1.Visible = false;

            frmMaintananceRecordes frm = new frmMaintananceRecordes();
            AddControls(frm);
        }
    }
}

