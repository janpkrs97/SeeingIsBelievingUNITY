using System.Collections;
using System.Collections.Generic;
using SyncVR.Util;
using UnityEngine;


namespace SyncVR.DeviceIdentity
{
    public class DeviceIdentityService : MonoBehaviour
    {
        public class DeviceIdentityCallbackHandler : AndroidJavaProxy
        {
            public DeviceIdentityCallbackHandler() : base("tech.syncvr.device_identity_connector.DeviceIdentityInterfaceCallbackHandler") { }

            public void onServiceBound()
            {
                Debug.Log("DeviceIdentityService received onServiceBound callback!");
            }

            public void onServiceUnbound()
            {
                Debug.Log("DeviceIdentityService received onServiceUnbound callback!");
            }

            public void onDeviceIdReceived(string deviceId)
            {
                Debug.Log("Received DeviceID from DeviceIdentityService: " + deviceId);
                Instance.DeviceId = deviceId;
            }
        }

        public static DeviceIdentityService Instance { get; private set; }
        private string _DeviceId = "";
        public string DeviceId
        {
            get
            {
                if (_DeviceId == "")
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (SystemInfoUtil.GetSDKInt() < 29)
                        {
                            using (AndroidJavaObject jo = new AndroidJavaObject("android.os.Build"))
                            {
                                Debug.Log("Directly getting serial nr cause not on Android 10 and no MDM agent!");
                                return jo.CallStatic<string>("getSerial"); ;
                            }
                        }
                    }
                }

                return _DeviceId;
            }
            private set
            {
                _DeviceId = value;
            }
        }
        public bool isBound { get; private set; } = false;
        private DeviceIdentityCallbackHandler deviceIdentityCallbackHandler;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(transform.root.gameObject);
            StartCoroutine(BindToDeviceIdentityInterface());
        }

        private IEnumerator BindToDeviceIdentityInterface()
        {
            deviceIdentityCallbackHandler = new DeviceIdentityCallbackHandler();
            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.device_identity_connector.DeviceIdentityInterfaceBinder"))
                {
                    using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                    {
                        isBound = false;
                        while (!isBound)
                        {
                            isBound = serviceBinder.Call<bool>("bind", deviceIdentityCallbackHandler, activity);
                            if (!isBound)
                            {
                                Debug.Log("Couldn't bind to DeviceIdentityInterface yet!");
                            }
                            yield return new WaitForSeconds(2f);
                        }
                    }
                }
            }

        }
    }
}