using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

namespace SyncVR.Analytics
{
    public class AnalyticsService : MonoBehaviour
    {
        public class LoggingInterfaceCallbackHandler : AndroidJavaProxy
        {
            public LoggingInterfaceCallbackHandler() : base("tech.syncvr.logging_connector.LoggingInterfaceCallbackHandler") { }

            public void onServiceBound()
            {
                Debug.Log("LoggingConnector received onServiceBound callback!");
            }

            public void onServiceUnbound()
            {
                Debug.Log("LoggingConnector received onServiceUnbound callback!");
            }
        }

        public static AnalyticsService Instance { get; private set; }
        public bool analyticsEnabled { get; private set; }

        private const string analyticsEnabledKey = "analytics_enabled";
        private LoggingInterfaceCallbackHandler loggingInterfaceCallbackHandler;
        public bool isBound { get; private set; } = false;

        public enum EventType
        {
            AppStart,
            AppGetFocus,
            AppLoseFocus,
            AppPause,
            AppUnpause,
            SceneLoad,
            Error
        }

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
            StartCoroutine(BindToLoggingInterface());
        }

        public void OnEnable()
        {
            analyticsEnabled = Boolean.Parse(PlayerPrefs.GetString(analyticsEnabledKey, Boolean.FalseString));
        }

        public void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            LogEvent(EventType.AppStart, new Dictionary<string, object>() { { "version", Application.version } });
        }

        private IEnumerator BindToLoggingInterface ()
        {
            loggingInterfaceCallbackHandler = new LoggingInterfaceCallbackHandler();

            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.logging_connector.LoggingInterfaceBinder"))
                {
                    using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                    {
                        isBound = false;
                        while (!isBound)
                        {
                            isBound = serviceBinder.Call<bool>("bind", loggingInterfaceCallbackHandler, activity);
                            if (!isBound)
                            {
                                Debug.Log("Couldn't bind to LoggingInterface yet!");
                            }
                            yield return new WaitForSeconds(2f);
                        }
                    }
                }
            }

        }

        public void SetAnalyticsEnabled(bool enabled)
        {
            if (!analyticsEnabled && enabled)
            {
                PlayerPrefs.SetString(analyticsEnabledKey, Boolean.TrueString);
                analyticsEnabled = true;
                LogEvent("Analytics", new Dictionary<string, object>() { { "version", Application.version }, { "analytics_enabled", true } });
            }
            else if (analyticsEnabled && !enabled)
            {
                PlayerPrefs.SetString(analyticsEnabledKey, Boolean.FalseString);
                analyticsEnabled = false;
            }
        }

        public void LogEvent(EventType eventType, Dictionary<string, object> data)
        {
            LogEvent(eventType.ToString(), data);
        }

        public void LogEvent(string eventType, Dictionary<string, object> data)
        {
            data.Add("time", DateTime.Now.ToString("s"));
            data.Add("battery", SystemInfo.batteryLevel);

            if (Debug.isDebugBuild || !analyticsEnabled || !isBound)
            {
                Debug.Log(eventType + " - " + JsonConvert.SerializeObject(data));
            }
            if (analyticsEnabled && isBound)
            {
                using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.logging_connector.LoggingInterfaceBinder"))
                    {
                        using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                        {
                            serviceBinder.Call("sendAnalytics", eventType, JsonConvert.SerializeObject(data));
                        }
                    }
                }
            }
        }

        public void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                LogEvent(EventType.AppLoseFocus, new Dictionary<string, object>());
            }
            else
            {
                LogEvent(EventType.AppGetFocus, new Dictionary<string, object>());
            }
        }

        public void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                LogEvent(EventType.AppUnpause, new Dictionary<string, object>());
            }
            else
            {
                LogEvent(EventType.AppPause, new Dictionary<string, object>());
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LogEvent(EventType.SceneLoad, new Dictionary<string, object>() { { "scene_name", scene.name } });
        }
    }
}