using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace pos.Reports
{
    public partial class XtraReportCutomerA5 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReportCutomerA5()
        {
            InitializeComponent();
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
