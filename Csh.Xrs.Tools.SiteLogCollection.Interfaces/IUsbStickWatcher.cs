using System;
using System.Collections.Generic;
using System.Management;

namespace Csh.Xrs.Tools.SiteLogCollection.Interfaces
{
    public delegate bool OnUsbStickPlugIn(string diskName);
    public delegate bool OnUsbStickPlugOut(string diskName);

    public interface IUsbStickWatcher: IWatcher
    {
        void AddOnUsbStickPlugIn(OnUsbStickPlugIn onUsbStickPlugIn);
        void AddOnUsbStickPlugOut(OnUsbStickPlugOut onUsbStickPlugOut);
    }
}