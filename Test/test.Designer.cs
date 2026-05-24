namespace pos.Test
{
    partial class test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(test));
            notifyIcon1 = new NotifyIcon(components);
            toastNotificationsManager1 = new DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager(components);
            sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            userControl11 = new UserControl1();
            ucProduct21 = new pos.UserControls.ucProduct2();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)toastNotificationsManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sfDataGrid1).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // toastNotificationsManager1
            // 
            toastNotificationsManager1.ApplicationId = "b483a71e-d9a9-44de-ba4d-1c637a9a4ae5";
            toastNotificationsManager1.Notifications.AddRange(new DevExpress.XtraBars.ToastNotifications.IToastNotificationProperties[] { new DevExpress.XtraBars.ToastNotifications.ToastNotification("0bc44328-cc03-468d-b2dd-4078fd500b77", null, "Pellentesque lacinia tellus eget volutpat", "Body toast 1", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.Text01), new DevExpress.XtraBars.ToastNotifications.ToastNotification("a5aa9f8d-a848-4477-b220-a93457fba75c", null, "Pellentesque lacinia tellus eget volutpat", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.Text01), new DevExpress.XtraBars.ToastNotifications.ToastNotification("a4960e6d-46bf-479f-a4c5-c59727bbd84e", null, "Pellentesque lacinia tellus eget volutpat", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.Text01), new DevExpress.XtraBars.ToastNotifications.ToastNotification("c9b0d988-bd0e-413a-80be-9eeb3df085b9", null, "Pellentesque lacinia tellus eget volutpat", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.Text01) });
            // 
            // sfDataGrid1
            // 
            sfDataGrid1.AccessibleName = "Table";
            sfDataGrid1.Location = new Point(303, 88);
            sfDataGrid1.Name = "sfDataGrid1";
            sfDataGrid1.Size = new Size(436, 292);
            sfDataGrid1.Style.BorderColor = Color.FromArgb(100, 100, 100);
            sfDataGrid1.Style.CheckBoxStyle.CheckedBackColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.CheckBoxStyle.CheckedBorderColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.CheckBoxStyle.IndeterminateBorderColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.HyperlinkStyle.DefaultLinkColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.TabIndex = 2;
            sfDataGrid1.Text = "sfDataGrid1";
            // 
            // userControl11
            // 
            userControl11.BackColor = Color.FromArgb(243, 243, 243);
            userControl11.Location = new Point(352, 180);
            userControl11.Name = "userControl11";
            userControl11.Size = new Size(150, 150);
            userControl11.TabIndex = 3;
            // 
            // ucProduct21
            // 
            ucProduct21.BackColor = Color.FromArgb(243, 243, 243);
            ucProduct21.barCode = null;
            ucProduct21.id = 0;
            ucProduct21.Location = new Point(535, 125);
            ucProduct21.Name = "ucProduct21";
            ucProduct21.PCategory = null;
            ucProduct21.PImage = (Image)resources.GetObject("ucProduct21.PImage");
            ucProduct21.PName = "اسم المنتج";
            ucProduct21.pprice = "label5";
            ucProduct21.pQty = "label4";
            ucProduct21.pshortFall = "label6";
            ucProduct21.Size = new Size(160, 180);
            ucProduct21.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(156, 138);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 5;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // test
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(ucProduct21);
            Controls.Add(userControl11);
            Controls.Add(sfDataGrid1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "test";
            Text = "test";
            WindowState = FormWindowState.Maximized;
            Load += test_Load;
            ((System.ComponentModel.ISupportInitialize)toastNotificationsManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)sfDataGrid1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private NotifyIcon notifyIcon1;
        private DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager toastNotificationsManager1;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private UserControl1 userControl11;
        private UserControls.ucProduct2 ucProduct21;
        private Button button1;
    }
}