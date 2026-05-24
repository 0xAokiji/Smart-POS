using System;
using System.Windows.Forms;

public class MyCustomPanel : Panel
{
    public MyCustomPanel()
    {
        this.AutoScroll = false;
    }

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.Style &= ~0x00200000; // WS_VSCROLL
            cp.Style &= ~0x00100000; // WS_HSCROLL
            return cp;
        }
    }
}
