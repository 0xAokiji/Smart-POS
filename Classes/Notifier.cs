using System;

namespace pos.Classes
{
    internal class Notifier
    {
        public static void ShowNotification(string title, string message)
        {
            NotifyIcon notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Information,
                Text = "Smart Cashier",
                BalloonTipTitle = title,
                BalloonTipText = message,
                BalloonTipIcon = ToolTipIcon.Info
            };

            notifyIcon.ShowBalloonTip(5000);

            // ✅ استخدم Timer لتDispose بعد انتهاء العرض
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
            {
                Interval = 6000 // بعد 9 ثواني مثلاً
            };
            timer.Tick += (sender, args) =>
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
