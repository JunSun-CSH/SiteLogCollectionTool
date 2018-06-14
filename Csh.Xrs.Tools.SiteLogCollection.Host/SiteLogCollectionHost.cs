using System.Collections.Generic;
using System.Threading.Tasks;

using Csh.Xrs.Tools.SiteLogCollection.Entities;
using Csh.Xrs.Tools.SiteLogCollection.Interfaces;

using Topshelf.Hosts;

namespace Csh.Xrs.Tools.SiteLogCollection.Host
{
    public class SiteLogCollectionService : IService
    {
        private LogCollectionEngine LogCollectionEngine;
        private bool Initialize()
        {
            bool result = false;

            try
            {
                LogCollectionEngine = new LogCollectionEngine();
                result = true;
            }
            catch
            {

            }

            return result;
        }

        public bool Start()
        {
            bool result = false;

            try
            {
                result = Initialize();

                if (result)
                {
                    LogCollectionEngine.Start();
                    Task.Factory.StartNew(LogCollectionEngine.Run);
                }
            }
            catch
            {

            }

            return result;
        }

        public bool Stop()
        {

            bool result = false;

            try
            {
                result = LogCollectionEngine.Stop();
            }
            catch
            {

            }

            return result;
        }
    }
}