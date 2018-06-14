using System;
using Topshelf;

namespace Csh.Xrs.Tools.SiteLogCollection.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(serviceHost =>
            {
                serviceHost.SetDescription("Collect the site logs.");
                serviceHost.SetDisplayName("SiteLogCollectionService");
                serviceHost.SetServiceName("SiteLogCollectionService");
                serviceHost.EnableShutdown();
                serviceHost.StartManually();
                serviceHost.RunAsPrompt();
                serviceHost.RunAsLocalSystem();
                serviceHost.Service<SiteLogCollectionService>(service =>
                {
                    service.ConstructUsing(construct => new SiteLogCollectionService());
                    service.WhenStarted(svc => svc.Start());
                    service.WhenStopped(svc => svc.Stop());
                });
            });
        }
    }
}
