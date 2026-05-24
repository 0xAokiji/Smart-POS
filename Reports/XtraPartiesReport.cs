using DevExpress.XtraReports.UI;
using System;
using System.Drawing;

namespace pos.Reports
{
    public partial class XtraPartiesReport : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraPartiesReport()
        {
            InitializeComponent();

            // إنشاء قاعدة التنسيق
            FormattingRule rule = new FormattingRule();

            // تحديد الشرط (Expression)
            rule.Condition = "[transfareType] == 'سداد من الأجل'";

            // تعيين التنسيق (لون الخلفية)
            rule.Formatting.BackColor = Color.Gainsboro;

            // ربط القاعدة بالصف المطلوب
            rule.DataSource = this.DataSource;
            rule.DataMember = this.DataMember;

            // إضافة القاعدة إلى التقرير
            this.FormattingRuleSheet.Add(rule);

            // تطبيق القاعدة على الصف tableRow2
            tableRow2.FormattingRules.Add(rule);
        }
        // صور
        private Image _CompanyLogo;
        public Image CompanyLogo
        {
            get => _CompanyLogo;
            set
            {
                _CompanyLogo = value;
                picLogo.Image = _CompanyLogo;
            }
        }

        private Image _CompanyQRCode;
        public Image CompanyQRCode
        {
            get => _CompanyQRCode;
            set
            {
                _CompanyQRCode = value;
                picQRCode.Image = _CompanyQRCode;
            }
        }

        // ✅ نصوص الشركة
        private string _CompanyName;
        public string CompanyName
        {
            get => _CompanyName;
            set
            {
                _CompanyName = value;
                lblComName.Text = _CompanyName;
            }
        }

        private string _CompanyAddress;
        public string CompanyAddress
        {
            get => _CompanyAddress;
            set
            {
                _CompanyAddress = value;
                lblComAddress.Text = _CompanyAddress;
            }
        }

        private string _Phone1;
        public string Phone1
        {
            get => _Phone1;
            set
            {
                _Phone1 = value;
                lblComPhone1.Text = _Phone1;
            }
        }

        private string _Phone2;
        public string Phone2
        {
            get => _Phone2;
            set
            {
                _Phone2 = value;
                lblComPhone2.Text = _Phone2;
            }
        }
    }
}
