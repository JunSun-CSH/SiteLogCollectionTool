using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Csh.Xrs.Tools.SiteLogCollection.Interfaces
{
    public interface ITip
    {
        Point Location { get; set; }
        int FontSize { get; set; }
        FontFamily FontFamily { get; set; }
        void Show(string text, int displayTime);
    }
}
