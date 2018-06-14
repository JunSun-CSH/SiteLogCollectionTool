using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csh.Xrs.Tools.SiteLogCollection.Interfaces
{
    public interface IWatcher
    {
        bool StartWatching();
        bool StopWatching();
    }
}
