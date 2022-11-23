using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace SyncVR.Util
{
    public static class SystemInfoUtil
    {
        private static string packageName = "";
        private static string storagePath = "";
        private static IPAddress ipAddress = null;
        private static string wifiSSID = "";
        private static string wifiBSSID = "";

        public static string PackageName()
        {
            if (packageName == "")
            {
                packageName = Application.identifier;
            }
            return packageName;
        }

        public static string StoragePath()
        {
            if (storagePath == "")
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    using (AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment"))
                    {
                        storagePath = jc.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
                    }
                }
                else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    storagePath = Application.persistentDataPath;
                }
            }

            return storagePath;
        }

        /**
         * Returns available storage space in MB
         */
        public static float GetAvailableStorage()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                string path = "";
                using (AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment"))
                {
                    path = jc.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
                }

                long blocks;
                long blockSize;
                using (AndroidJavaObject stat = new AndroidJavaObject("android.os.StatFs", path))
                {
                    blocks = stat.Call<long>("getAvailableBlocksLong");
                    blockSize = stat.Call<long>("getBlockSizeLong");
                }

                return (blocks * blockSize) / 1000000f;
            }
            else
            {
                return 1000000;
            }
        }

        public static IPAddress GetIPAddress()
        {
            if (ipAddress == null)
            {
                GetNetworkInformation();
            }

            return ipAddress;
        }

        public static string GetWifiSSID()
        {
            if (wifiSSID == "")
            {
                GetNetworkInformation();
            }

            return wifiSSID;
        }

        public static string GetWifiBSSID()
        {
            if (wifiBSSID == "")
            {
                GetNetworkInformation();
            }

            return wifiBSSID;
        }

        private static void GetNetworkInformation()
        {

            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    try
                    {
                        using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                        {
                            // wifiInfo will be of type android.net.wifi.WifiInfo
                            AndroidJavaObject wifiInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo");

                            // get the ipaddress
                            ipAddress = new IPAddress(BitConverter.GetBytes(wifiInfo.Call<int>("getIpAddress")));
                            // get the ssid
                            wifiSSID = wifiInfo.Call<string>("getSSID");
                            // get the bssid
                            wifiBSSID = wifiInfo.Call<string>("getBSSID");

                            Debug.Log("Wifi SSID: " + wifiSSID + ", BSSID: " + wifiBSSID + " with IP Address: " + ipAddress);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Exception trying to obtain wifi info: " + e.Message + "\n" + e.StackTrace);
                    }
                }
            }
            else
            {
                ipAddress = new IPAddress(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                wifiSSID = "unknown network";
                Debug.Log("Connected to: " + wifiSSID + " with IP Address: " + ipAddress);
            }
        }

        public static int GetSDKInt()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    return version.GetStatic<int>("SDK_INT");
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
