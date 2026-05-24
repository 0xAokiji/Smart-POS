using DevExpress.Pdf.Xmp;
using DevExpress.Utils.Html.Internal;
using pos.Classes;
using pos.GeneralForms;
using pos.Model.POS;
using pos.Model.Stor;
using pos.View;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pos.Model
{
    public partial class frmMaintenanceAdd : Form
    {
        public int ReturnedTaskID { get; set; }
        public int paryIDReturn { get; set; }
        public int id = 0;
        private int taskID = 0;
        int tec_ID = 0;
        private int pary_ID;
        private string barcode = "";
        private Image barcodeImag;
        PrintDocument printDoc = new PrintDocument();
        private Dictionary<string, int> nameToID = new Dictionary<string, int>();



        public frmMaintenanceAdd()
        {
            InitializeComponent();
            printDoc.PrintPage += new PrintPageEventHandler(PrintDoc_PrintPage);
            textSuggester();
        }

        private void frmMaintenanceAdd_Load(object sender, EventArgs e)
        {

            string qry_Tec = "SELECT staffID as id, sName as name FROM staff WHERE sRole = N'فني'";

            DataTable dt = GetDataTable(qry_Tec);
            cmb_Technician.DataSource = dt;
            cmb_Technician.DisplayMember = "name";
            cmb_Technician.ValueMember = "id";

            if (cmb_Technician.Items.Count > 0)
            {
                cmb_Technician.SelectedIndex = 0;
            }



            if (id != 0)
            {
                LoadData(id);
            }
            else 
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); // التاريخ بصيغة 2025-10-19
                txtTime.Text = DateTime.Now.ToString("hh:mm tt"); // مثال: 02:35 PM
                barcode = GenerateUniqueInvoiceCode();

            }
            var barGenerator = new generatBarCode();
            barcodeImag = barGenerator.CreateBarCode(barcode);
            imgBarcode.Image = barcodeImag;
            txtTaskNumber.Text = barcode;

        }
        private void LoadData(int id)
        {
            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    string qry = @"
                SELECT 
                    t.taskID,
                    t.paryID,
                    t.taskNumber,
                    t.partyNotes,
                    t.tecnicalID,
                    t.descriptionProblem,
                    t.Priority,
                    t.taskPrice,
                    t.status,
                    t.startDate,
                    t.startTime,
                    t.endDate,
                    t.endTime,
                    p.pName AS PartyName
                FROM Task t
                INNER JOIN Parties p ON t.paryID = p.pID
                WHERE t.taskID = @id";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // تعبئة القيم في عناصر الفورم
                                txtTaskNumber.Text = reader["taskNumber"].ToString();
                                txtCustomerNote.Text = reader["partyNotes"].ToString();
                                txtDescriptionProblem.Text = reader["descriptionProblem"].ToString();
                                txtSalary.Text = reader["taskPrice"].ToString();
                                txtParyName.Text = reader["PartyName"].ToString();

                                pary_ID = Convert.ToInt32(reader["paryID"]);
                                barcode = reader["taskNumber"].ToString();
                                cmb_Technician.SelectedValue = Convert.ToInt32(reader["tecnicalID"]);
                                cbPriority.SelectedIndex = Convert.ToInt32(reader["Priority"]) - 1;

                                // لو عايز تعرض الحالة أو التواريخ
                                // lblStatus.Text = reader["status"].ToString();
                                txtDate.Text = Convert.ToDateTime(reader["startDate"]).ToString("yyyy-MM-dd");
                                txtTime.Text = reader["startTime"].ToString();
                            }
                            else
                            {
                                Notifier.ShowNotification("تحذير ⚠️", "لم يتم العثور على البيانات المطلوبة");
                            }
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ أثناء تحميل البيانات");
                Console.WriteLine(ex.Message);
            }
        }

        private static DataTable GetDataTable(string query)
        {
            using (SqlConnection con = MainClass.GetConnection()) // تأكد إن عندك دالة GetConnection
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            save();

        }
        private void save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtParyName.Text) || string.IsNullOrWhiteSpace(txtCustomerNote.Text)
                    || cmb_Technician.SelectedItem == null || string.IsNullOrWhiteSpace(txtPhone.Text))
                {
                    Notifier.ShowNotification("Error ❌", "احدي البيانات فارغة");
                    return;
                }

                int maintID = 0;

                using (SqlConnection con = MainClass.GetConnection()) // استخدام اتصال جديد
                {
                    string qry;
                    string qry1 = string.Empty;

                    if (id == 0) // إضافة
                    {
                        qry = @"
                        INSERT INTO Task (paryID, taskNumber, partyNotes, tecnicalID, descriptionProblem, Priority, PriorityName, taskPrice, status, paymentStatus, startDate, startTime, endDate, endTime)
                        VALUES (@paryID, @taskNumber, @partyNotes, @tecnicalID, @descriptionProblem, @Priority, @PriorityName, @taskPrice, @status, @paymentStatus, @startDate, @startTime, @endDate, @endTime);
                        SELECT SCOPE_IDENTITY();";
                    }
                    else // تحديث
                    {
                        qry = @"
                        UPDATE Task SET 
                            paryID = @paryID, 
                            taskNumber = @taskNumber, 
                            partyNotes = @partyNotes, 
                            tecnicalID = @tecnicalID, 
                            descriptionProblem = @descriptionProblem,
                            Priority = @Priority,
                            PriorityName = @PriorityName,
                            taskPrice = @taskPrice,
                            startDate = @startDate,
                            startTime = @startTime,
                            endDate = @endDate,
                            endTime = @endTime
                        WHERE taskID = @id";
                    }

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@paryID", pary_ID); // رقم العميل أو الجهة المرتبطة بالمهمة
                        cmd.Parameters.AddWithValue("@taskNumber", txtTaskNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@partyNotes", txtCustomerNote.Text.Trim());
                        cmd.Parameters.AddWithValue("@tecnicalID", Convert.ToInt32(cmb_Technician.SelectedValue)); // رقم الفني
                        cmd.Parameters.AddWithValue("@descriptionProblem", txtDescriptionProblem.Text.Trim());
                        cmd.Parameters.AddWithValue("@Priority", cbPriority.SelectedIndex + 1);
                        cmd.Parameters.AddWithValue("@PriorityName", cbPriority.Text);
                        cmd.Parameters.AddWithValue("@taskPrice", Convert.ToDouble(txtSalary.Text));
                        cmd.Parameters.AddWithValue("@status", "تم الاستلام");
                        cmd.Parameters.AddWithValue("@paymentStatus", "غير مدفوع");
                        cmd.Parameters.AddWithValue("@startDate", DateTime.Now.Date);
                        cmd.Parameters.AddWithValue("@startTime", DateTime.Now.ToShortTimeString());
                        cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@endTime", DBNull.Value);

                        if (con.State == ConnectionState.Closed) con.Open();

                        if (id == 0)
                        {
                            taskID = Convert.ToInt32(cmd.ExecuteScalar());
                            Notifier.ShowNotification("تم ✅", $" تم حفظ المهمه بنجاح");
                        }
                        else
                        {
                            cmd.ExecuteNonQuery();
                            Notifier.ShowNotification("تم ✅", $" تم تحديث المهمه بنجاح");

                        }

                        con.Close();
                    }


                }
                this.DialogResult = DialogResult.OK;
                ReturnedTaskID = taskID;
                paryIDReturn = pary_ID;
                this.Close();
            }
            catch
            {
                Notifier.ShowNotification("Error ❌", "حدث خطأ");
                return;
            }
        }


        private string GenerateUniqueInvoiceCode()
        {
            const string digits = "0123456789";
            Random random = new Random();
            string code;
            bool exists;

            do
            {
                // توليد كود عشوائي 8 رقم فقط
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 8; i++)
                {
                    sb.Append(digits[random.Next(digits.Length)]);
                }
                code = sb.ToString();

                // التأكد من أنه غير موجود مسبقاً في قاعدة البيانات
                using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدم GetConnection
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Task WHERE taskNumber = @code", con))
                    {
                        cmd.Parameters.AddWithValue("@code", code);
                        exists = (int)cmd.ExecuteScalar() > 0;
                    }
                }


            } while (exists);

            return code;

        }
        public void PrintBarcodes()
        {

            try
            {
                printDoc.PrinterSettings.PrinterName = MainClass.BarcodePrinter;
                PaperSize paperSize = new PaperSize("Custom", 260, 98);
                printDoc.DefaultPageSettings.PaperSize = paperSize;
                printDoc.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);

                printDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الطباعة: " + ex.Message);
            }

        }
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var barGenerator = new generatBarCode();
            int barcodeWidth = 230;
            int barcodeHeight = 40; // ⬅️ قللت الارتفاع من 60 لـ 40

            int x = (e.PageBounds.Width - barcodeWidth) / 2;
            int x2 = (e.PageBounds.Width - 260) / 2;

            int y = 10;

            // اسم المنتج (فوق الباركود في النص)
            RectangleF nameRect = new RectangleF(x - 26, y, barcodeWidth, 20);
            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            centerFormat.LineAlignment = StringAlignment.Center;
            centerFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            e.Graphics.DrawString(MainClass.CompanyName,
                new Font("Arial", 10, FontStyle.Bold), Brushes.Black, nameRect, centerFormat);

            y += 15;

            // ⬅️ رسم الباركود بالارتفاع الجديد
            barcodeImag = barGenerator.CreateBarCode(barcode);
            e.Graphics.DrawImage(barcodeImag, new Rectangle(x - 21, y, barcodeWidth, barcodeHeight));

            y += barcodeHeight - 15;

            int productWidth = 170;

            // احسب X جديد علشان يبقى في النص
            int productX = (e.PageBounds.Width - productWidth) / 2;

            RectangleF nameRect2 = new RectangleF(productX - 23, y, productWidth, 40);
            StringFormat centerFormat2 = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.DirectionRightToLeft,
                Trimming = StringTrimming.Word
            };

            e.Graphics.DrawString(
                txtParyName.Text + " : " + txtPhone.Text,
                new Font("Arial", 9, FontStyle.Regular),
                Brushes.Black,
                nameRect2,
                centerFormat2
            );

            e.HasMorePages = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            PrintBarcodes();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmPartesSearch frm = new frmPartesSearch(this);
            frm.type = "عميل";
            frm.ShowDialog(this);
            this.Focus();
        }
        public void resultSearch(string pName)
        {
            txtParyName.Text = pName;
        }
        private void txtParyName_TextChanged(object sender, EventArgs e)
        {
            if (nameToID.ContainsKey(txtParyName.Text))
            {
                pary_ID = nameToID[txtParyName.Text];
                ValidateInputs();

            }
            else
            {
                pary_ID = 0;
            }

            if (pary_ID > 0)
            {
                string qry = @"SELECT pPhone, pAdderss FROM Parties WHERE pID = @pID";

                using (SqlConnection con = MainClass.GetConnection()) // ✅ استخدم اتصال جاهز
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@pID", pary_ID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtPhone.TextAlign = HorizontalAlignment.Center;

                                txtPhone.Text = reader["pPhone"].ToString();

                                lblValedatName.Visible = false;
                                btnEditParties.Enabled = true;

                                txtParyName.HoverState.BorderColor = Color.FromArgb(136, 214, 218);
                                txtParyName.FocusedState.BorderColor = Color.FromArgb(136, 214, 218);
                                txtParyName.BorderColor = Color.FromArgb(136, 214, 218);
                            }
                        }
                    }
                }
            }
            else if (string.IsNullOrWhiteSpace(txtParyName.Text))
            {
                txtParyName.TextAlign = HorizontalAlignment.Right;

                txtPhone.Text = string.Empty;
                lblValedatName.Visible = false;
                btnEditParties.Enabled = false;

                txtParyName.HoverState.BorderColor = Color.FromArgb(136, 214, 218);
                txtParyName.FocusedState.BorderColor = Color.FromArgb(136, 214, 218);
                txtParyName.BorderColor = Color.FromArgb(136, 214, 218);

                return;
            }
            else
            {
                txtPhone.Text = string.Empty;
                txtPhone.TextAlign = HorizontalAlignment.Right;
                txtPhone.PlaceholderText = "بيانات الاتصال فارغة";

                txtParyName.HoverState.BorderColor = Color.Red;
                txtParyName.FocusedState.BorderColor = Color.Red;
                txtParyName.BorderColor = Color.Red;

                lblValedatName.Visible = true;
                lblValedatName.Text = "هذا الاسم غير موجود";
                lblValedatName.ForeColor = Color.Red;

                btnEditParties.Enabled = false;
            }

            if (!string.IsNullOrEmpty(txtParyName.Text))
            {
                char firstChar = txtParyName.Text[0];
                txtParyName.TextAlign = IsArabic(firstChar)
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left;
            }
        }

        private bool IsArabic(char c)
        {
            return (c >= 0x0600 && c <= 0x06FF) || // Arabic
                   (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                   (c >= 0x08A0 && c <= 0x08FF);   // Arabic Extended
        }
        private void textSuggester()
        {
            string qry = @"SELECT pID, pName FROM Parties WHERE PartyType LIKE @PartyType";
            AutoCompleteStringCollection dataSource = new AutoCompleteStringCollection();

            using (SqlConnection con = MainClass.GetConnection()) // ✅ الاتصال الصحيح
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@PartyType", "%" + "عميل" + "%");

                    con.Open(); // ✅ افتح الاتصال

                    DataTable dt2 = new DataTable();
                    using (SqlDataAdapter da2 = new SqlDataAdapter(cmd))
                    {
                        da2.Fill(dt2);
                        foreach (DataRow row in dt2.Rows)
                        {
                            string name = row["pName"].ToString();
                            int id = Convert.ToInt32(row["pID"]);
                            dataSource.Add(name);
                            nameToID[name] = id;
                        }
                    }
                }
            }

            txtParyName.AutoCompleteCustomSource = dataSource;
            txtParyName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtParyName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void btnAddParties_Click(object sender, EventArgs e)
        {

            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.partyType = "عميل";
                frm.ShowDialog(this);

            }
            this.Focus();
            textSuggester(); // Initialize text suggester for party names
        }

        private void btnEditParties_Click(object sender, EventArgs e)
        {
            using (frmAddParties frm = new frmAddParties())
            {

                frm.Owner = this;
                frm.pID = pary_ID; // Pass the selected party ID to the form
                frm.partyType = "عميل";
                frm.ShowDialog(this);

            }
            this.Focus();
            textSuggester(); // Initialize text suggester for party names
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            // يسمح بالأرقام فقط وحذف (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يمنع الكتابة
            }
        }

        private void ValidateInputs()
        {
            // 🔹 قائمة بكل عناصر الإدخال اللي عايز تتحقق منها
            Guna.UI2.WinForms.Guna2TextBox[] textboxes =
                { txtParyName, txtCustomerNote, txtPhone, txtSalary, txtDescriptionProblem };

            bool allValid = true;

            // 🔸 التحقق من كل TextBox
            foreach (var txt in textboxes)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.BorderColor = Color.Red; // إطار أحمر عند الخطأ
                    allValid = false;
                }
                else
                {
                    txt.BorderColor = Color.FromArgb(1, 95, 95); // اللون الافتراضي
                }
            }

            // 🔹 التحقق من الكومبو بوكس
            if (cmb_Technician.SelectedItem == null)
            {
                cmb_Technician.BorderColor = Color.Red;
                allValid = false;
            }
            else
            {
                cmb_Technician.BorderColor = Color.FromArgb(1, 95, 95);
            }

            // 🔸 تفعيل أو تعطيل الزر بناءً على النتيجة
            btnSave.Enabled = allValid;
        }


        private void txtCustomerNote_TextChanged(object sender, EventArgs e)
        {
            ValidateInputs();
        }

        private void cmb_Technician_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateInputs();

        }
    }
}



