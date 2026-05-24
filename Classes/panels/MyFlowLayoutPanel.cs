using System.Windows.Forms;

namespace pos.Classes.panels
{
    public class MyFlowLayoutPanel : FlowLayoutPanel
    {
        public MyFlowLayoutPanel()
        {
            DoubleBuffered = true; // مهم جدًا
            SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
        
    }
}
