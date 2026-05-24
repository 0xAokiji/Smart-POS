using DevExpress.CodeParser;
using DevExpress.XtraMap.ItemEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos.Model.Stor
{
    public partial class frmShowReturns : Form
    {
        public frmShowReturns()
        {
            InitializeComponent();
        }

        private void frmShowReturns_Load(object sender, EventArgs e)
        {
            if (!MainClass.ShowReturns)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            frmAll_Bills frm = new frmAll_Bills();
            frm.pos = true;
            frm.partyType = "عميل";
            frm.lblTitle.Text = "اضافة مرتجعات عميل";
            openedForms.Remove("frmAll_Bills");
            mainPanel.Controls.Clear();

            AddControls(frm);
        }
        private Dictionary<string, Form> openedForms = new Dictionary<string, Form>();
        public void AddControls(Form f)
        {
            // تحقق هل الفورم موجود بالفعل في mainPanel
            foreach (Control ctrl in mainPanel.Controls)
            {
                if (ctrl is Form existingForm && existingForm.Name == f.Name)
                {
                    existingForm.BringToFront(); // اعرضه في الواجهة فقط
                    return;
                }
            }

            // لو مش موجود، أضفه
            mainPanel.Controls.Clear(); // امسح الموجود (أو علّق هذا السطر لو حابب تحتفظ بالباقي)
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(f);
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

        private void فواتيرالعميلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowReturns)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            frmAll_Bills frm = new frmAll_Bills();
            frm.pos = true;
            frm.partyType = "عميل";
            frm.lblTitle.Text = "اضافة مرتجعات عميل";
            openedForms.Remove("frmAll_Bills");
            mainPanel.Controls.Clear();

            AddControls(frm);
        }

        private void عرضالمنتجاتToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowReturns)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            frmAll_Bills frm = new frmAll_Bills();
            frm.pos = true;
            frm.partyType = "مورد";
            frm.lblTitle.Text = "اضافة مرتجعات مورد";
            openedForms.Remove("frmAll_Bills");
            mainPanel.Controls.Clear();

            AddControls(frm);
        }

        private void فواتيرالعملاءالمحذوفةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowReturns)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            frmShowAllReturns frm = new frmShowAllReturns();
            frm.partyType = "عميل";
            frm.lblTitel.Text = "مرتجعات العملاء";
            openedForms.Remove("frmShowAllReturns");
            mainPanel.Controls.Clear();

            AddControls(frm);
        }

        private void اضافةمنتجبدونفاتورهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowReturns)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }
            frmShowAllReturns frm = new frmShowAllReturns();
            frm.partyType = "مورد";
            frm.lblTitel.Text = "مرتجعات الموردين";
            openedForms.Remove("frmShowAllReturns");
            mainPanel.Controls.Clear();

            AddControls(frm);
        }
    }
}
