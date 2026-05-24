using System.Windows.Forms;


namespace pos.Classes
{
    class MyPanel : Panel
    {
        public MyPanel()
        {
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
    }
}
