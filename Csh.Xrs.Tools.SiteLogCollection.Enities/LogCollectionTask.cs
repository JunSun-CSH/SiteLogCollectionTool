using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Csh.Xrs.Tools.SiteLogCollection.Interfaces;

namespace Csh.Xrs.Tools.SiteLogCollection.Entities
{
    public enum LogCollectionTaskType
    {
        File = 1,
        ScreenShot = 2,
        Video = 3
    }

    public class LogCollectionTask
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public LogCollectionTaskType TaskType { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public bool Enabled { get; set; }
        public ILogCollector LogCollector { get; set; }
    }
}
