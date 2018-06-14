using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Csh.Xrs.Tools.SiteLogCollection.Interfaces;

namespace Csh.Xrs.Tools.SiteLogCollection.Entities
{
    public class FileLogCollector : ILogCollector
    {
        private ITip tip =  Tip.Instance;

        public string CopiedFiles { get; set; }
        public string CopyTo { get; set; }
        public Dictionary<string, string> LogFilter { get; set; }

        public bool AfterLogCollection()
        {
            tip.Show("After Log Collection", 2);
            return true;
        }

        public bool BeforeLogCollection()
        {
            tip.Show("Before Log Collection", 2);
            return true;
        }

        public bool BeginLogCollection()
        {
            tip.Show("Begin Log Collection", 2);
            return true;
        }

        public bool Collect()
        {
            tip.Show("Collect", 2);
            return true;
        }

        public bool CopyRequiredFiles()
        {
            tip.Show("Copy Required Files", 2);
            return true;
        }

        public bool EndLogCollection()
        {
            tip.Show("End Log Collection", 2);
            return true;
        }

        public bool InstallRequiredApps()
        {
            tip.Show("Install Required Apps", 2);
            return true;
        }
    }
}
