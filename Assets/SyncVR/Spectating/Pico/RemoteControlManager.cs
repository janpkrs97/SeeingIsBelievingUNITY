using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using SyncVR.Util;

public class RemoteControlManager : MonoBehaviour
{
    public class RemoteControlSelectedOption
    {
        public int version;
        public int option_id;
    }

    public class RemoteControlOptionCollection
    {
        public int version;
        public Dictionary<string, List<RemoteControlOption>> category_options;
    }

    public class RemoteControlOption
    {
        // we use the convention that objects being serialized to json have snake_case property names
        public int option_id;
        public string option_name;
        public string option_category;

        public RemoteControlOption()
        {
            option_id = 0;
            option_name = "";
            option_category = "";
        }

        public RemoteControlOption(int _option_id, string _option_name)
        {
            option_id = _option_id;
            option_name = _option_name;
        }
    }

    public class ServiceBinderCallbackHandler : AndroidJavaProxy
    {
        public ServiceBinderCallbackHandler() : base("tech.syncvr.syncvr_agent_connector.ServiceBinderCallbackHandler") { }

        public void onServiceBound()
        {
            Debug.Log("Received the onServiceBound callback! Hurray!");
        }

        public void onServiceUnbound()
        {

        }

        public void onTakeRemoteControl()
        {
            Debug.Log("Received the onTakeRemoteControl callback! Hurray!");

            Instance.isRemoteControl = true;
            if (Instance.remoteControlCallbackHandler != null)
            {
                Loom.Instance.QueueOnMainThread(() =>
                {
                    Instance.remoteControlCallbackHandler.TakeRemoteControl();
                });
            }
        }

        public void onReleaseRemoteControl()
        {
            Debug.Log("Received te onReleaseRemoteControl callback! Hurray!");

            Instance.isRemoteControl = false;
            if (Instance.remoteControlCallbackHandler != null)
            {
                Loom.Instance.QueueOnMainThread(() =>
                {
                    Instance.remoteControlCallbackHandler.ReleaseRemoteControl();
                });
            }
        }

        public void onOptionSelected(string selectedOptionJson)
        {
            Debug.Log("Received Selected Option: " + selectedOptionJson);

            if (Instance.remoteControlCallbackHandler != null)
            {
                RemoteControlSelectedOption selectedOption = JsonConvert.DeserializeObject<RemoteControlSelectedOption>(selectedOptionJson);
                if (selectedOption.version == Instance.optionsVersion)
                {
                    Loom.Instance.QueueOnMainThread(() =>
                    {
                        Instance.remoteControlCallbackHandler.OptionSelected(selectedOption.option_id);
                    });
                }
            }
        }
    }

    private ServiceBinderCallbackHandler serviceBinderCallbackHandler;
    public IRemoteControlCallbackHandler remoteControlCallbackHandler;

    public bool isRemoteControl { get; private set; }

    private int optionsVersion = 0;
    private bool isBound = false;

    public static RemoteControlManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Start()
    {
        serviceBinderCallbackHandler = new ServiceBinderCallbackHandler();

        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.syncvr_agent_connector.ServiceBinder"))
            {
                using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    isBound = false;
                    while (!isBound)
                    {
                        yield return new WaitForSeconds(2f);
                        isBound = serviceBinder.Call<bool>("bind", serviceBinderCallbackHandler, activity, Application.productName);
                        if (!isBound)
                        {
                            Debug.Log("Couldn't bind to service yet!");
                        }
                    }
                }
            }
        }
    }

    public void OnApplicationFocus(bool focus)
    {
        if (!isBound)
        {
            return;
        }

        if (focus)
        {
            Debug.Log("Application got focus!");
            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.syncvr_agent_connector.ServiceBinder"))
                {
                    using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                    {
                        serviceBinder.Call("onGetFocus");
                    }
                }
            }
        }
        else
        {
            Debug.Log("Application lost focus!");
            using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.syncvr_agent_connector.ServiceBinder"))
                {
                    using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                    {
                        serviceBinder.Call("onLoseFocus");
                    }
                }
            }
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (!isBound)
        {
            return;
        }

        if (pause)
        {
            Debug.Log("Application paused!");
        }
        else
        {
            Debug.Log("Application unpaused!");
        }
    }

    public void OnApplicationQuit()
    {
        if (!isBound)
        {
            return;
        }

        Debug.Log("Application Quit!");
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.syncvr_agent_connector.ServiceBinder"))
            {
                using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    serviceBinder.Call("onApplicationQuit");
                }
            }
        }
    }

    public void SendRemoteControlOptions(List<RemoteControlOption> options)
    {
        optionsVersion++;
        RemoteControlOptionCollection collection = new RemoteControlOptionCollection();
        collection.version = optionsVersion;
        collection.category_options = new Dictionary<string, List<RemoteControlOption>>();

        List<string> categories = options.Select(x => x.option_category).Distinct().ToList();
        foreach (string category in categories)
        {
            collection.category_options.Add(category, options.Where(x => x.option_category == category).ToList());
        }

        string collectionJson = JsonConvert.SerializeObject(collection);
        Debug.Log("About to send Available Options: " + collectionJson);

        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (AndroidJavaClass serviceBinderClass = new AndroidJavaClass("tech.syncvr.syncvr_agent_connector.ServiceBinder"))
            {
                using (AndroidJavaObject serviceBinder = serviceBinderClass.CallStatic<AndroidJavaObject>("getInstance"))
                {
                    serviceBinder.Call("sendAvailableOptions", collectionJson);
                }
            }
        }
    }

}
