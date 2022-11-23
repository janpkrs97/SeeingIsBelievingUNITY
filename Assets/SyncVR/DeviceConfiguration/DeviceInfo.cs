using System.Collections;
using System.Collections.Generic;
using SyncVR.Util;
using UnityEngine;

namespace SyncVR.DeviceConfiguration
{
    public class DeviceInfo
    {
        public string deviceName;
        public string deviceType;
        public string customerName;
        public string departmentName;
        public string appPackageName;
        public string appProductName;

        public DeviceInfo()
        {
            appPackageName = SystemInfoUtil.PackageName();
            appProductName = Application.productName;
        }
    }
}