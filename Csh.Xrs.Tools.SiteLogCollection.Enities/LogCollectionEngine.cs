using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using Csh.Xrs.Tools.SiteLogCollection.Interfaces;

namespace Csh.Xrs.Tools.SiteLogCollection.Entities
{
    public class LogCollectionEngine : ILogCollectionEngine
    {
        private List<IWatcher> watchers = new List<IWatcher>();
        private BlockingCollection<LogCollectionTask> logCollectionTasks = new BlockingCollection<LogCollectionTask>();
        private ITip tip;
        private static AutoResetEvent logCollectionTrigger = new AutoResetEvent(false);

        public LogCollectionEngine()
        {
            tip = Tip.Instance;
            watchers.Add(new ShortcutKeyWatcher());
            watchers.Add(new WmiUsbStickWatcher());
        }

        public bool AfterLogCollection(LogCollectionTask logCollectionTask)
        {
            return false;
        }

        public bool BeforeLogCollection(LogCollectionTask logCollectionTask)
        {
            return false;
        }

        public bool BeginLogCollection(LogCollectionTask logCollectionTask)
        {
            return false;
        }

        public bool CopyRequiredFiles(LogCollectionTask logCollectionTask)
        {
            return false;
        }

        public bool EndLogCollection(LogCollectionTask logCollectionTask)
        {
            return false;
        }

        public bool InstallRequiredApps(LogCollectionTask logCollectionTask)
        {
            return false;
        }

        public bool Start()
        {
            bool result = false;

            try
            {
                foreach (var watcher in watchers)
                {
                    watcher.StartWatching();

                    if (watcher is IUsbStickWatcher)
                    {
                        var usbStickWatcher = watcher as IUsbStickWatcher;
                        usbStickWatcher.AddOnUsbStickPlugIn(new OnUsbStickPlugIn((diskName) =>
                        {
                            var logCollectionTasksFromUsbStick = Common.Utilities.LogCollectionTaskConfigFileToLogCollectionTask(diskName);

                            foreach (var taskFromUsbStick in logCollectionTasksFromUsbStick)
                            {
                                logCollectionTasks.Add(taskFromUsbStick);
                            }

                            logCollectionTasks.CompleteAdding();
                            logCollectionTrigger.Set();
                            return true;
                        }));
                    }
                    else if (watcher is IShortcutWatcher)
                    {

                    }
                }

                result = true;
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public bool Stop()
        {
            bool result = false;

            try
            {
                foreach (var watcher in watchers)
                {
                    watcher.StopWatching();
                }

                watchers.Clear();
                watchers = null;
                result = true;
            }
            catch (Exception)
            {

            }

            return result;
        }

        public bool Run()
        {
            bool result = false;

            try
            {
                while (true)
                {
                    logCollectionTrigger.WaitOne();

                    while (logCollectionTasks.TryTake(out LogCollectionTask logCollectionTask))
                    {
                        var logCollector = logCollectionTask.LogCollector;

                        logCollector.InstallRequiredApps();
                        logCollector.CopyRequiredFiles();
                        logCollector.BeforeLogCollection();
                        logCollector.BeginLogCollection();
                        logCollector.Collect();
                        logCollector.AfterLogCollection();
                        logCollector.EndLogCollection();
                    }
                    
                    logCollectionTrigger.Reset();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;

        }
    }
}
