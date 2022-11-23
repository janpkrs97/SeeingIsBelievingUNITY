using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SyncVR.Authentication;

namespace SyncVR.DeviceConfiguration
{
    public static class DeviceInfoService
    {
        private const string CONTENT_PACKAGE_NAME = "tech.syncvr.painreduction";
        private const string getDeviceInfoURLProd = "https://europe-west1-optimum-time-233909.cloudfunctions.net/api_public/v1/device_info";
        private const string getDeviceInfoURLDev = "https://europe-west1-syncvr-dev.cloudfunctions.net/api_public/v1/device_info";
        private static string getDeviceInfoURL = getDeviceInfoURLProd;

        public static DeviceInfo deviceInfo { get; private set; }
        public static JObject deviceInfoRaw { get; private set; }

        public static bool isDeviceInfoRetrieved { get; private set; }
        public static bool isRetrieving { get; private set; }

        public static void SetUseDevelopmentEnvironment(bool useDev)
        {
            if (useDev)
            {
                getDeviceInfoURL = getDeviceInfoURLDev;
            }
            else
            {
                getDeviceInfoURL = getDeviceInfoURLProd;
            }
        }

        public static IEnumerator RetrieveDeviceInfoAPI()
        {
            if (isRetrieving)
            {
                Debug.Log("Allready retrieving device info! Aborting!");
                yield break;
            }
            if (!FirebaseAuthService.isLoggedIn)
            {
                Debug.Log("Not logged in! Aborting!");
                SetRetrievingFailed();
                yield break;
            }

            SetRetrievingStart();
            deviceInfo = new DeviceInfo();
            deviceInfoRaw = null;

            using (UnityWebRequest www = new UnityWebRequest(getDeviceInfoURL))
            {
                www.method = "GET";
                www.SetRequestHeader("Content-Type", "application/json");
                FirebaseAuthService.AuthenticateRequest(www);

                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("HTTP error in GET request: " + www.responseCode + " " + www.downloadHandler.text);
                    SetRetrievingFailed();
                    yield break;
                }
                else if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Network error in GET request!");
                    SetRetrievingFailed();
                    yield break;
                }
                else
                {
                    try
                    {
                        string res = www.downloadHandler.text;
                        ParseDeviceInfo(res);
                    }
                    catch (Exception e)
                    {
                        deviceInfo = null;
                        Debug.Log("Error parsing device info: " + e.StackTrace);
                    }
                    SetRetrievingSuccess();
                }

            }
        }

        private static void SetRetrievingStart()
        {
            isDeviceInfoRetrieved = false;
            isRetrieving = true;
        }
        private static void SetRetrievingFailed()
        {
            isDeviceInfoRetrieved = false;
            isRetrieving = false;
        }

        private static void SetRetrievingSuccess()
        {
            isDeviceInfoRetrieved = true;
            isRetrieving = false;

        }

        private static void ParseDeviceInfo(string json)
        {
            deviceInfoRaw = JsonConvert.DeserializeObject<JObject>(json);
            deviceInfo.customerName = deviceInfoRaw.Value<string>("customerName");
            deviceInfo.departmentName = deviceInfoRaw.Value<string>("departmentName");
            deviceInfo.deviceName = deviceInfoRaw.Value<string>("humanReadableName");
            deviceInfo.deviceType = deviceInfoRaw.Value<string>("deviceType");
        }

        public static JArray GetAppContent()
        {
            if (deviceInfoRaw == null)
            {
                return null;
            }

            // try to get the content from the apps list
            JArray apps = deviceInfoRaw.Value<JArray>("apps");
            JArray content = null;
            if (apps != null)
            {
                foreach (JObject app in apps)
                {
                    if (app.Value<string>("appId") == CONTENT_PACKAGE_NAME)
                    {
                        content = app.Value<JArray>("content");
                        if (content != null)
                        {
                            return content;
                        }
                    }
                }
            }

            // if that didn't succeed, and this is the painreduction app, retrieve content from toplevel
            content = deviceInfoRaw.Value<JArray>("content");
            if (content != null)
            {
                return content;
            }

            // if that also didnt succeed, return null;
            return null;

        }
    }
}
