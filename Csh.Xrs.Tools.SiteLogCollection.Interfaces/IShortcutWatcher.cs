using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csh.Xrs.Tools.SiteLogCollection.Interfaces
{
    public interface IShortcutWatcher:IWatcher
    {
        bool RegisterGlobalShortcut(string shortcutKeys, ILogCollector logCollector);
        bool RemoveGlobalShortcut();
    }
}
