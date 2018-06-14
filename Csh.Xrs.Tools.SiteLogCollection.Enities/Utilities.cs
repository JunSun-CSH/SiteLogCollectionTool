using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Csh.Xrs.Tools.SiteLogCollection.Entities;

namespace Csh.Xrs.Tools.SiteLogCollection.Common
{
    public static class Utilities
    {
        public static List<LogCollectionTask> LogCollectionTaskConfigFileToLogCollectionTask(string logCollectionTaskConfigFilePath)
        {
            List<LogCollectionTask> logCollectionTasks = new List<LogCollectionTask>();

            var logCollectionTask = new LogCollectionTask()
            {
                Id = Guid.NewGuid(),
                Name = "Collect Windows Log",
                TaskType = LogCollectionTaskType.File,
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                Enabled = true,
                LogCollector = new FileLogCollector()
            };

            logCollectionTasks.Add(logCollectionTask);

            return logCollectionTasks;
        }

        //public static bool ResultReport(Action<>)
    }
}
