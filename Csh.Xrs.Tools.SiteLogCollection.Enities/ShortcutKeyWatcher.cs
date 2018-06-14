using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Csh.Xrs.Tools.SiteLogCollection.Interfaces;

namespace Csh.Xrs.Tools.SiteLogCollection.Entities
{
    public class ShortcutKeyWatcher : IShortcutWatcher
    {
        public bool RegisterGlobalShortcut(string shortcutKeys, ILogCollector logCollector)
        {
            throw new NotImplementedException();
        }

        public bool RemoveGlobalShortcut()
        {
            throw new NotImplementedException();
        }

        public bool StartWatching()
        {
            return false;
        }

        public bool StopWatching()
        {
            throw new NotImplementedException();
        }
    }
}
