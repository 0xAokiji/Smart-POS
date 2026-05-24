using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos.Model.Stor
{
    public partial class frmShowBills : Form
    {
        public frmShowBills()
        {
            InitializeComponent();
        }

        private void frmShowBills_Load(object sender, EventArgs e)
        {
            if (!MainClass.ShowCustomerBills)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            mainPanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");


            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "عميل";
            frmAllBills.lblTitle.Text = "فواتير العملاء";
            AddControls(frmAllBills);
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
            if (!MainClass.ShowCustomerBills)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            mainPanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");


            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "عميل";
            frmAllBills.lblTitle.Text = "فواتير العملاء";
            AddControls(frmAllBills);
        }

        private void فواتيرالعملاءالمحذوفةToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowDeletedCusBills)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }


            openedForms.Remove("frmAll_Bills");

            mainPanel.Controls.Clear();

            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "عميل";
            frmAllBills.lblTitle.Text = "فواتير العملاء المحذوفة";
            frmAllBills.isDeleted = true;
            AddControls(frmAllBills);
        }

        private void عرضالمنتجاتToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowSupplierBills)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            mainPanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");

            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "مورد";
            frmAllBills.lblTitle.Text = "فواتير الموردين";
            AddControls(frmAllBills);
        }

        private void اضافةمنتجبدونفاتورهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MainClass.ShowDeletedSupBills)
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Parent = (Form)this.TopLevelControl;
                guna2MessageDialog1.Show("عذرًا، ليس لديك الصلاحية لأداء هذا الإجراء");
                return;
            }

            mainPanel.Controls.Clear();

            openedForms.Remove("frmAll_Bills");

            frmAll_Bills frmAllBills = new frmAll_Bills();
            frmAllBills.partyType = "مورد";
            frmAllBills.lblTitle.Text = "فواتير الموردين المحذوفة";
            frmAllBills.isDeleted = true;
            AddControls(frmAllBills);
        }
    }
}
