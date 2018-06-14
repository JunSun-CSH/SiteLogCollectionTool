using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csh.Xrs.Tools.SiteLogCollection.Interfaces
{
    public interface ILogCollector
    {
        string CopiedFiles { get; set; }
        string CopyTo { get; set; }
        //DateTime FromDateTime { get; set; }
        //DateTime ToDateTime { get; set; }
        Dictionary<string, string> LogFilter { get; set; }
        bool CopyRequiredFiles();
        bool InstallRequiredApps();
        bool BeginLogCollection();
        bool BeforeLogCollection();
        bool Collect();
        bool AfterLogCollection();
        bool EndLogCollection();
    }
}
