using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SyncVR.Analytics;
using SyncVR.Authentication;
using SyncVR.DeviceConfiguration;
using SyncVR.DeviceIdentity;

public class SyncVRManager : MonoBehaviour
{
    public static SyncVRManager Instance { get; private set; }

    [SerializeField]
    private SyncVRAppKeys appKeys;
    [SerializeField]
    private bool useAnalytics;
    [SerializeField]
    private bool useDevEnvironment;

    public bool isSyncing { get; private set; } = true;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    public void Start()
    {
        StartCoroutine(StartBackendSync());
        AnalyticsService.Instance.SetAnalyticsEnabled(useAnalytics);
    }

    public void RefreshDeviceInfo()
    {
        if (!isSyncing)
        {
            StartCoroutine(RefreshDeviceInfoRoutine());
        }
    }

    private IEnumerator StartBackendSync()
    {
        if (appKeys != null)
        {
            if (appKeys.SyncVRAppKey != "")
            {
                FirebaseAuthService.SetUseDevelopmentEnvironment(useDevEnvironment);
                DeviceInfoService.SetUseDevelopmentEnvironment(useDevEnvironment);

                while (DeviceIdentityService.Instance.DeviceId == "")
                {
                    yield return null;
                }
                
                StartCoroutine(RefreshDeviceInfoRoutine());
            }
        }
    }

    private IEnumerator RefreshDeviceInfoRoutine()
    {
        isSyncing = true;

        StartCoroutine(FirebaseAuthService.Login(appKeys.SyncVRAppKey, DeviceIdentityService.Instance.DeviceId));
        while (!FirebaseAuthService.isLoggedIn)
        {
            yield return null;
        }
        Debug.Log("Logged in! Start obtaining Device Info!");

        StartCoroutine(DeviceInfoService.RetrieveDeviceInfoAPI());
        while (DeviceInfoService.isRetrieving) 
        {
            yield return null;
        }

        Debug.Log("Device Info was refreshed!");

        isSyncing = false;
    }

    public bool GetUseDevEnvironment()
    {
        return useDevEnvironment;
    }

}
