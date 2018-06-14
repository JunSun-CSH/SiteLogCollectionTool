using System;
using System.Collections.Generic;
using System.Management;

using Csh.Xrs.Tools.SiteLogCollection.Interfaces;

namespace Csh.Xrs.Tools.SiteLogCollection.Entities
{
    public class WmiUsbStickWatcher : IUsbStickWatcher
    {
        private ManagementEventWatcher insertWatcher = null;
        private ManagementEventWatcher removeWatcher = null;
        private TimeSpan queryInterval = new TimeSpan(0, 0, 5);
        private ManagementScope scope = new ManagementScope("root\\CIMV2");
        private bool usbStickPluggedIn = false;
        private event OnUsbStickPlugIn onUsbStickPlugInEventHandler;
        private event OnUsbStickPlugOut onUsbStickPlugOutEventHandler;

        public WmiUsbStickWatcher()
        {
            scope.Options.EnablePrivileges = true;
        }

        public bool StartWatching()
        {
            bool startResult = false;

            try
            {
                AddUsbPlugInListener();
                AddUsbPlugOutListener();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            return startResult;
        }

        public bool StopWatching()
        {
            bool stopResult = false;

            try
            {
                if (insertWatcher != null)
                {
                    insertWatcher.Stop();
                    insertWatcher = null;
                }

                if (removeWatcher != null)
                {
                    removeWatcher.Stop();
                    removeWatcher = null;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                usbStickPluggedIn = false;
            }

            return stopResult;
        }

        public void AddOnUsbStickPlugIn(OnUsbStickPlugIn onUsbStickPlugIn)
        {
            onUsbStickPlugInEventHandler += onUsbStickPlugIn;
        }

        public void AddOnUsbStickPlugOut(OnUsbStickPlugOut onUsbStickPlugOut)
        {
            onUsbStickPlugOutEventHandler += onUsbStickPlugOut;
        }

        public string DiskName
        {
            get;
            set;
        }


        private Boolean AddUsbPlugInListener()
        {
            try
            {
                WqlEventQuery InsertQuery = new WqlEventQuery(
                    "__InstanceCreationEvent",
                    queryInterval,
                    "TargetInstance isa 'Win32_USBControllerDevice'");

                insertWatcher = new ManagementEventWatcher(scope, InsertQuery);
                insertWatcher.EventArrived += (sender, e) =>
                {
                    if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent" && !usbStickPluggedIn)
                    {
                        Console.Write("USB plug in time:" + DateTime.Now + "\r\n");

                        SelectQuery usbStickQuery = new SelectQuery("select * from win32_logicaldisk where drivetype='2'");
                        ManagementObjectSearcher mos = new ManagementObjectSearcher(usbStickQuery);

                        foreach (System.Management.ManagementObject disk in mos.Get())
                        {
                            try
                            {
                                usbStickPluggedIn = true;
                                //disk["Caption"] "G:"    object { string}
                                //disk["Name"]    "G:"    object { string}
                                //disk["DeviceID"]    "G:"    object { string}
                                DiskName = disk["Name"] as string;
                                onUsbStickPlugInEventHandler(DiskName);
                                //CollectWindowsLog();

                            }
                            finally
                            {

                            }
                        }

                    }
                };

                insertWatcher.Start();

                return true;
            }
            catch (Exception)
            {
                StopWatching();
                return false;
            }
        }

        private void AddUsbPlugOutListener()
        {
            try
            {
                WqlEventQuery RemoveQuery = new WqlEventQuery(
                    "__InstanceDeletionEvent",
                    queryInterval,
                    "TargetInstance isa 'Win32_USBControllerDevice'");

                removeWatcher = new ManagementEventWatcher(scope, RemoveQuery);
                removeWatcher.EventArrived += (sender, e) =>
                {
                    if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
                    {
                        usbStickPluggedIn = false;
                        //onUsbStickPlugOutEventHandler(DiskName);
                        Console.Write("USB plug out time:" + DateTime.Now + "\r\n");
                    }
                };

                removeWatcher.Start();
            }
            catch (Exception)
            {
                StopWatching();
            }
        }
    }
}
