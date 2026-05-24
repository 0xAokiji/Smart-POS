using DevExpress.XtraRichEdit.Utils;
using pos.GeneralForms;
using pos.View;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace pos.Model
{

    public partial class frmBillList : Form
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        public string enter;
        public bool typeX = false;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        frmPOS parentForm;
        bool fromPOS = false;

        //Fields
        private int bordarRadius = 10;
        private int borderSize = 2;
        private Color borderColor = MainClass.CheckedFillColor;

        private Color backgroundPrimary;
        private Color backgroundSecondary;
        private Color textColor;
        private Color checkedFillColor;
        private Color checkedForeColor;

        private Guna.UI2.WinForms.MessageDialog MessageBox1 = new Guna.UI2.WinForms.MessageDialog();

        // Paging / loading fields (adapted from frmpurchaseView style)
        private int pageSize = 14;
        private bool hasMoreData = true;
        private int currentPage = 0;
        private bool isLoading = false;

        public frmBillList()
        {
            InitializeComponent();
            MainClass.themeMode();
            ThemeMode();

            this.ShowInTaskbar = false;

            // تغيير خصائص النافذة لمنع ظهورها في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

            fromPOS = false;
        }

        public frmBillList(frmPOS parent)
        {
            InitializeComponent();
            ThemeMode();

            parentForm = parent;
            this.ShowInTaskbar = false;

            // تغيير خصائص النافذة لمنع ظهورها في Alt+Tab
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

            fromPOS = true;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public int MainID = 0;
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private GraphicsPath GetRoundedPath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        private void FormRegionAndBorder(Form form, float radius, Graphics graph, Color borderColor, float borderSize)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                using (GraphicsPath roundPath = GetRoundedPath(form.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (Matrix transform = new Matrix())
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    form.Region = new Region(roundPath);
                    if (borderSize >= 1)
                    {
                        Rectangle rect = form.ClientRectangle;
                        float scaleX = 1.0F - ((borderSize + 1) / rect.Width);
                        float scaleY = 1.0F - ((borderSize + 1) / rect.Height);
                        transform.Scale(scaleX, scaleY);
                        transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                        graph.Transform = transform;
                        graph.DrawPath(penBorder, roundPath);
                    }
                }
            }
        }
        private void ControlRegionAndBorder(Control control, float radius, Graphics graph, Color borderColor)
        {
            using (GraphicsPath roundPath = GetRoundedPath(control.ClientRectangle, radius))
            using (Pen penBorder = new Pen(borderColor, 1))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                control.Region = new Region(roundPath);
                graph.DrawPath(penBorder, roundPath);
            }
        }
        private void DrawPath(Rectangle rect, Graphics graph, Color color)
        {
            using (GraphicsPath roundPath = GetRoundedPath(rect, bordarRadius))
            using (Pen penBorder = new Pen(color, 3))
            {
                graph.DrawPath(penBorder, roundPath);
            }
        }
        private struct FormBoundsColors
        {
            public Color TopLeftColor;
            public Color TopRightColor;
            public Color BottomLeftColor;
            public Color BottomRightColor;
        }
        private FormBoundsColors GetFormBoundsColors()
        {
            var fbColor = new FormBoundsColors();
            using (var bmp = new Bitmap(1, 1))
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle rectBmp = new Rectangle(0, 0, 1, 1);
                //Top Left
                rectBmp.X = this.Bounds.X - 1;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopLeftColor = bmp.GetPixel(0, 0);
                //Top Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Y;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.TopRightColor = bmp.GetPixel(0, 0);
                //Bottom Left
                rectBmp.X = this.Bounds.X;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomLeftColor = bmp.GetPixel(0, 0);
                //Bottom Right
                rectBmp.X = this.Bounds.Right;
                rectBmp.Y = this.Bounds.Bottom;
                graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
                fbColor.BottomRightColor = bmp.GetPixel(0, 0);
            }
            return fbColor;
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //-> SMOOTH OUTER DORDER
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle recForm = this.ClientRectangle;
            int mWight = recForm.Width / 2;
            int mHight = recForm.Height / 2;
            var fbColors = GetFormBoundsColors();

            //Top Left
            DrawPath(recForm, e.Graphics, fbColors.TopLeftColor);

            //Top Right
            Rectangle recTopRight = new Rectangle(mWight, recForm.Y, mWight, mHight);
            DrawPath(recTopRight, e.Graphics, fbColors.TopRightColor);

            //Bottom Left
            Rectangle recBottomLeft = new Rectangle(recForm.X, recForm.X + mHight, mWight, mHight);
            DrawPath(recBottomLeft, e.Graphics, fbColors.BottomLeftColor);

            //Bottom Right
            Rectangle recBottomRight = new Rectangle(mWight, recForm.Y + mHight, mWight, mHight);
            DrawPath(recBottomRight, e.Graphics, fbColors.BottomRightColor);
        }
        private async void frmBillList_Load(object sender, EventArgs e)
        {
            if (typeX)
            {
                btnEnd.Visible = false;
                btnHold.Visible = false;
                btnUnCom.Visible = false;
                btn_Delete.FillColor = Color.FromArgb(64, 64, 64);
                btn_Delete.Enabled = false;

            }
            if (enter == "un")
            {

                notEndBill();
            }
            else if (enter == "hold")
            {

                holdBill();
            }
            else if (enter == "payed")
            {
                payedBill();
            }
            else
            {
                await LoadDataAsync();
            }
        }

        // Reworked LoadData to use DataTable-loading and manual population (frmpurchaseView style)
        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            // Initialize grid style and paging
            ApplyGridStyle(maindgv);
            currentPage = 0;
            hasMoreData = true;
            await System.Threading.Tasks.Task.Run(() =>
            {
                // run first page synchronously on background thread then marshal to UI
            });

            // Determine which dataset to load based on current mode
            if (typeX)
            {
                await LoadMaintenanceFinishedAsync();
            }
            else
            {
                if (enter == "un")
                    notEndBill();
                else if (enter == "hold")
                    holdBill();
                else if (enter == "payed")
                    payedBill();
                else
                    notEndBill();
            }
        }

        private void LoadMoreIfNeeded()
        {
            if (!isLoading && hasMoreData)
            {
                // For simplicity keep synchronous single-page load in this refactor.
            }
        }

        private void guna2DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            int count = 0;
            foreach (DataGridViewRow row in maindgv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private async void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (maindgv.CurrentRow == null) return;

            int mainIDToDelete = Convert.ToInt32(maindgv.CurrentRow.Cells["dgvid"].Value);

            if (maindgv.CurrentCell.OwningColumn.Name == "dgvDel")
            {
                DialogResult result = MessageBox.Show("هل تريد حذف هذه الفاتورة؟", "تأكيد",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection con = MainClass.GetConnection())
                        {
                            con.Open();

                            using (SqlTransaction transaction = con.BeginTransaction())
                            {
                                string deleteQuery = typeX
                                    ? @"DELETE FROM maintenance WHERE mID = @MainID;
                                DELETE FROM tblMain1 WHERE MainID = @MainID;"
                                    : @"DELETE FROM tblDetails WHERE MainID = @MainID;
                                DELETE FROM tblMain1 WHERE MainID = @MainID;";

                                using (SqlCommand cmd = new SqlCommand(deleteQuery, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@MainID", mainIDToDelete);
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("حدث خطأ أثناء الحذف: " + ex.Message);
                    }

                    // إعادة تحميل البيانات
                    if (fromPOS)
                        parentForm.clearData();

                    if (enter == "un")
                        notEndBill();
                    else if (enter == "hold")
                        holdBill();
                    else
                        await LoadDataAsync();
                }
            }
            else if (maindgv.CurrentCell.OwningColumn.Name == "dgvDetail")
            {
                frmBlackout frmBlackout = new frmBlackout(this);
                frmBlackout.Show();
                frmBlackout.Owner = this;

                frmDetailsProView frmDetailsProView = new frmDetailsProView
                {
                    Owner = this,
                    mainID = mainIDToDelete,
                    Bill = true
                };

                frmDetailsProView.ShowDialog();
                frmBlackout.Close();
            }
        }



        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // نتأكد إن الضغط مش على الهيدر
            {
                MainID = Convert.ToInt32(maindgv.Rows[e.RowIndex].Cells["dgvid"].Value);
                parentForm.ReloadInvoiceToPOS(MainID);
                this.Close();
            }
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            btnEnd.Checked = true;
            btnHold.Checked = false;
            btnUnCom.Checked = false;

            btn_Delete.FillColor = Color.FromArgb(64, 64, 64);
            btn_Delete.Enabled = false;

            if (MainClass.USER == "مدير")
                maindgv.Columns["dgvDel"].Visible = true;
            else
                maindgv.Columns["dgvDel"].Visible = false;

            enter = "payed";
            maindgv.Controls.Clear();
            if (typeX == true)
            {
                maindgv.Columns[5].HeaderText = "اسم العميل";

                string qry = @"SELECT mID, name, pPrice FROM maintenance WHERE status LIKE N'%finsh%'";
                ListBox lb = new ListBox();
                lb.Items.Add(dgvid);
                lb.Items.Add(dgvStatus);
                lb.Items.Add(dgvTotal);

                MainClass.LoadData(qry, maindgv, lb);
            }
            else
            {
                payedBill();
            }


        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            btnEnd.Checked = false;
            btnHold.Checked = true;
            btnUnCom.Checked = false;
            btn_Delete.FillColor = Color.Red;
            btn_Delete.Enabled = true;

            enter = "hold";
            maindgv.Columns["dgvDel"].Visible = true;
            holdBill();
        }

        private void btnUnCom_Click(object sender, EventArgs e)
        {
            btnEnd.Checked = false;
            btnHold.Checked = false;
            btnUnCom.Checked = true;
            btn_Delete.FillColor = Color.Red;
            btn_Delete.Enabled = true;

            enter = "un";
            maindgv.Columns["dgvDel"].Visible = true;

            notEndBill();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string qry = string.Empty;

            DialogResult result = MessageBox.Show("هل تريد حذف كل الفوراتير؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (enter == "un" || enter == "hold")
                {
                    detetProductsPos();
                }
                else
                {
                    return;
                }

                this.Close();
            }
        }
        private void detetProductsPos()
        {
            if (maindgv.Rows.Count == 0) return;

            try
            {
                using (SqlConnection con = MainClass.GetConnection())
                {
                    con.Open();

                    using (SqlTransaction transaction = con.BeginTransaction())
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM tblMain1 WHERE MainID = @MainID", con, transaction))
                    {
                        cmd.Parameters.Add("@MainID", SqlDbType.Int);

                        foreach (DataGridViewRow row in maindgv.Rows)
                        {
                            if (row.IsNewRow) continue;

                            if (row.Cells["dgvid"].Value == null) continue;

                            if (!int.TryParse(row.Cells["dgvid"].Value.ToString(), out int mainID)) continue;

                            cmd.Parameters["@MainID"].Value = mainID;
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // New helper to load DataTable (copied/adapted from frmpurchaseView)
        public static DataTable LoadDataReturn(string qry, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = MainClass.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // Apply visual style similar to frmpurchaseView
        private void ApplyGridStyle(Guna.UI2.WinForms.Guna2DataGridView dgv)
        {
            // إعدادات عامة
            dgv.Visible = true;
            //dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;

            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 45;

            dgv.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold);

            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.RowsDefaultCellStyle.BackColor = Color.White;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 153, 153);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        }

        // Rewritten data population methods to fill grid manually (frmpurchaseView approach)
        private async void notEndBill()
        {
            // paging state (same pattern as GetPruchaseData / holdBill)
            try
            {
                if (isLoading || !hasMoreData)
                    return;

                isLoading = true;

                // clear on first page
                if (currentPage == 0)
                {
                    maindgv.Controls.Clear();
                    maindgv.Rows.Clear();
                }

                string qry = $@"
                WITH RankedDetails AS (
                    SELECT 
                        m.MainID, 
                        d.proID, 
                        m.InvoiceCode, 
                        d.qty, 
                        m.status, 
                        m.total, 
                        m.shiftID,
                        ISNULL(pr.pName, N'غير محدد') AS CustomerName, 
                        ROW_NUMBER() OVER (PARTITION BY m.MainID ORDER BY d.proID) AS rn
                    FROM tblMain1 m
                    LEFT JOIN tblDetails d ON m.MainID = d.MainID
                    LEFT JOIN products p ON p.pID = d.proID
                    LEFT JOIN Parties pr ON pr.pID = m.partiesID
                    WHERE m.status LIKE N'%underwork%' 
                      AND m.shiftID = " + MainClass.shiftID + @"
                )
                SELECT MainID, proID, CustomerName, InvoiceCode, qty, status, total
                FROM RankedDetails
                WHERE rn = 1 OR rn IS NULL
                ORDER BY MainID ASC
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                int offset = currentPage * pageSize;
                SqlParameter[] parameters =
                {
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize)
                };

                DataTable dt = await System.Threading.Tasks.Task.Run(() => LoadDataReturn(qry, parameters));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                foreach (DataRow dr in dt.Rows)
                {
                    int idx = maindgv.Rows.Add();
                    DataGridViewRow row = maindgv.Rows[idx];

                    if (maindgv.Columns.Contains("dgvid") && dt.Columns.Contains("MainID"))
                        row.Cells["dgvid"].Value = dr["MainID"];

                    if (maindgv.Columns.Contains("dgvPid") && dt.Columns.Contains("proID"))
                        row.Cells["dgvPid"].Value = dr["proID"];

                    if (maindgv.Columns.Contains("dgvName") && dt.Columns.Contains("CustomerName"))
                        row.Cells["dgvName"].Value = dr["CustomerName"];

                    if (maindgv.Columns.Contains("dgvCode") && dt.Columns.Contains("InvoiceCode"))
                        row.Cells["dgvCode"].Value = dr["InvoiceCode"];

                    if (maindgv.Columns.Contains("dgvQty") && dt.Columns.Contains("qty"))
                        row.Cells["dgvQty"].Value = dr["qty"];

                    if (maindgv.Columns.Contains("dgvStatus") && dt.Columns.Contains("status"))
                        row.Cells["dgvStatus"].Value = dr["status"];

                    if (maindgv.Columns.Contains("dgvTotal") && (dt.Columns.Contains("total") || dt.Columns.Contains("totalPrice")))
                    {
                        row.Cells["dgvTotal"].Value = dt.Columns.Contains("total") ? dr["total"] : dr["totalPrice"];
                    }
                }

                currentPage++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ عند تحميل البيانات: " + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async void holdBill()
        {
            // paging state (keeps same pattern as GetPruchaseData)
            try
            {
                if (isLoading || !hasMoreData)
                    return;

                isLoading = true;

                // clear on first page
                if (currentPage == 0)
                {
                    maindgv.Controls.Clear();
                    maindgv.Rows.Clear();
                }

                string qry = $@"
                WITH RankedDetails AS (
                    SELECT 
                        m.MainID, 
                        d.proID, 
                        ISNULL(pr.pName, N'غير محدد') AS CustomerName,
                        m.InvoiceCode, 
                        d.qty, 
                        m.status, 
                        m.total, 
                        m.shiftID,
                        ROW_NUMBER() OVER (PARTITION BY m.MainID ORDER BY d.proID) AS rn
                    FROM tblMain1 m
                    LEFT JOIN tblDetails d ON m.MainID = d.MainID
                    LEFT JOIN products p ON p.pID = d.proID
                    LEFT JOIN Parties pr ON pr.pID = m.partiesID
                    WHERE m.status LIKE N'%pending%' 
                      AND m.shiftID = " + MainClass.shiftID + @"
                )
                SELECT MainID, proID, CustomerName, InvoiceCode, qty, status, total
                FROM RankedDetails
                WHERE rn = 1 OR rn IS NULL
                ORDER BY MainID ASC
                OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;";

                int offset = currentPage * pageSize;
                SqlParameter[] parameters =
                {
                    new SqlParameter("@offset", offset),
                    new SqlParameter("@limit", pageSize)
                };

                DataTable dt = await System.Threading.Tasks.Task.Run(() => LoadDataReturn(qry, parameters));

                if (dt.Rows.Count < pageSize)
                    hasMoreData = false;

                foreach (DataRow dr in dt.Rows)
                {
                    int idx = maindgv.Rows.Add();
                    DataGridViewRow row = maindgv.Rows[idx];

                    if (maindgv.Columns.Contains("dgvid") && dt.Columns.Contains("MainID"))
                        row.Cells["dgvid"].Value = dr["MainID"];

                    if (maindgv.Columns.Contains("dgvPid") && dt.Columns.Contains("proID"))
                        row.Cells["dgvPid"].Value = dr["proID"];

                    if (maindgv.Columns.Contains("dgvName") && dt.Columns.Contains("CustomerName"))
                        row.Cells["dgvName"].Value = dr["CustomerName"];

                    if (maindgv.Columns.Contains("dgvCode") && dt.Columns.Contains("InvoiceCode"))
                        row.Cells["dgvCode"].Value = dr["InvoiceCode"];

                    if (maindgv.Columns.Contains("dgvQty") && dt.Columns.Contains("qty"))
                        row.Cells["dgvQty"].Value = dr["qty"];

                    if (maindgv.Columns.Contains("dgvStatus") && dt.Columns.Contains("status"))
                        row.Cells["dgvStatus"].Value = dr["status"];

                    if (maindgv.Columns.Contains("dgvTotal") && (dt.Columns.Contains("total") || dt.Columns.Contains("totalPrice")))
                    {
                        row.Cells["dgvTotal"].Value = dt.Columns.Contains("total") ? dr["total"] : dr["totalPrice"];
                    }
                }

                currentPage++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ عند تحميل البيانات: " + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void payedBill()
        {
            maindgv.Controls.Clear();
            maindgv.Rows.Clear();

            string qry = $@"WITH RankedDetails AS (
                                SELECT 
                                    m.MainID, d.proID, d.qty, m.status, m.total, m.shiftID,
                                    ROW_NUMBER() OVER (PARTITION BY m.MainID ORDER BY d.proID) AS rn
                                FROM tblMain1 m
                                INNER JOIN tblDetails d ON m.MainID = d.MainID
                                INNER JOIN products p ON p.pID = d.proID
                                WHERE m.status LIKE N'%Finish%' AND m.shiftID = {MainClass.shiftID}
                            )
                            SELECT MainID, proID, qty, status, total
                            FROM RankedDetails
                            WHERE rn = 1";

            DataTable dt = LoadDataReturn(qry);

            foreach (DataRow dr in dt.Rows)
            {
                int idx = maindgv.Rows.Add();
                DataGridViewRow row = maindgv.Rows[idx];

                if (maindgv.Columns.Contains("dgvid") && dt.Columns.Contains("MainID"))
                    row.Cells["dgvid"].Value = dr["MainID"];

                if (maindgv.Columns.Contains("dgvPid") && dt.Columns.Contains("proID"))
                    row.Cells["dgvPid"].Value = dr["proID"];

                if (maindgv.Columns.Contains("dgvQty") && dt.Columns.Contains("qty"))
                    row.Cells["dgvQty"].Value = dr["qty"];

                if (maindgv.Columns.Contains("dgvStatus") && dt.Columns.Contains("status"))
                    row.Cells["dgvStatus"].Value = dr["status"];

                if (maindgv.Columns.Contains("dgvTotal") && dt.Columns.Contains("total"))
                    row.Cells["dgvTotal"].Value = dr["total"];
            }
        }

        // Optional: maintenance finished listing when typeX==true
        private async System.Threading.Tasks.Task LoadMaintenanceFinishedAsync()
        {
            maindgv.Controls.Clear();
            maindgv.Rows.Clear();

            string qry = @"SELECT m.mID AS MainID, m.customarName AS CustomerName, (m.pPrice + ISNULL(SUM(p.sellPrice), 0)) AS totalPrice
                           FROM maintenance m
                           LEFT JOIN maintenanceDetails md ON m.mID = md.mID
                           LEFT JOIN products p ON md.ProductID = p.pID
                           WHERE m.status LIKE N'%finsh%'
                           GROUP BY m.mID, m.customarName, m.pcName, m.pPrice;";

            DataTable dt = LoadDataReturn(qry);

            foreach (DataRow dr in dt.Rows)
            {
                int idx = maindgv.Rows.Add();
                DataGridViewRow row = maindgv.Rows[idx];

                if (maindgv.Columns.Contains("dgvid") && dt.Columns.Contains("MainID"))
                    row.Cells["dgvid"].Value = dr["MainID"];

                if (maindgv.Columns.Contains("dgvName") && dt.Columns.Contains("CustomerName"))
                    row.Cells["dgvName"].Value = dr["CustomerName"];

                if (maindgv.Columns.Contains("dgvTotal") && dt.Columns.Contains("totalPrice"))
                    row.Cells["dgvTotal"].Value = dr["totalPrice"];
            }
        }

        private void frmBillList_Paint(object sender, PaintEventArgs e)
        {
            FormRegionAndBorder(this, bordarRadius, e.Graphics, borderColor, borderSize);

        }

        private void ThemeColor()
        {
            backgroundPrimary = MainClass.BackgroundPrimary;
            backgroundSecondary = MainClass.BackgroundSecondary;
            textColor = MainClass.TextColor;
            checkedFillColor = MainClass.CheckedFillColor;
            checkedForeColor = MainClass.CheckedForeColor;
        }
        private void ThemeMode()
        {

            if (MainClass.ThemeMode == "dark")
                iconImage.Image = Properties.Resources.bill_dark;
            else if (MainClass.ThemeMode == "light")
                iconImage.Image = Properties.Resources.bill_light;

            ThemeColor();

            this.BackColor = backgroundPrimary;
            mainPanel.BackColor = backgroundPrimary;
            bottomPanel.BackColor = backgroundSecondary;
            topPanel.BackColor = checkedFillColor;

            lblTitle.ForeColor = textColor;
            iconImage.BackColor = checkedFillColor;

            //->Button
            btnHold.FillColor = backgroundPrimary;
            btnHold.ForeColor = textColor;
            btnHold.CheckedState.FillColor = checkedFillColor;
            btnHold.CheckedState.ForeColor = checkedForeColor;


            btnEnd.FillColor = backgroundPrimary;
            btnEnd.ForeColor = textColor;
            btnEnd.CheckedState.FillColor = checkedFillColor;
            btnEnd.CheckedState.ForeColor = checkedForeColor;

            btnUnCom.FillColor = backgroundPrimary;
            btnUnCom.ForeColor = textColor;
            btnUnCom.CheckedState.FillColor = checkedFillColor;
            btnUnCom.CheckedState.ForeColor = checkedForeColor;

            btn_Delete.FillColor = Color.Red;
            btn_Delete.ForeColor = textColor;

            btnCansel.FillColor = checkedFillColor;
            btnCansel.ForeColor = textColor;
            //-> datagride view 
            maindgv.BackgroundColor = backgroundPrimary;
            maindgv.GridColor = backgroundPrimary;
            maindgv.AlternatingRowsDefaultCellStyle.BackColor = backgroundPrimary;
            maindgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
            maindgv.AlternatingRowsDefaultCellStyle.ForeColor = textColor;
            // keep selected-row text RGB(51,51,51)
            maindgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);

            maindgv.ColumnHeadersDefaultCellStyle.BackColor = backgroundSecondary;
            maindgv.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            maindgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = backgroundSecondary;

            maindgv.RowsDefaultCellStyle.BackColor = backgroundPrimary;
            maindgv.RowsDefaultCellStyle.SelectionBackColor = checkedFillColor;
            maindgv.RowsDefaultCellStyle.ForeColor = textColor;
            // keep selected-row text RGB(51,51,51)
            maindgv.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(51, 51, 51);

            // ensure font size is slightly smaller everywhere for maindgv rows
            maindgv.DefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);
            maindgv.RowsDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Regular);

        }

        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Scroll handler: load next page when user scrolls to bottom
        private async void maindgv_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll) return;

            if (maindgv.RowCount == 0 || isLoading || !hasMoreData) return;

            // use RowCount-1 guard to be robust
            if (maindgv.FirstDisplayedScrollingRowIndex + maindgv.DisplayedRowCount(false) >= maindgv.RowCount - 1)
            {
                try
                {
                    if (typeX)
                    {
                        await LoadMaintenanceFinishedAsync();
                    }
                    else if (enter == "un")
                    {
                        notEndBill();
                    }
                    else if (enter == "hold")
                    {
                        holdBill();
                    }
                    else if (enter == "payed")
                    {
                        payedBill();
                    }
                    else
                    {
                        notEndBill();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading more data: " + ex.Message);
                }
            }
        }
    }
}