using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

namespace SyncVR.Util
{
    public class Loom : MonoBehaviour
    {
        public static Loom Instance { get; private set; }

        private List<Action> _actions = new List<Action>();
        private List<Action> _currentActions = new List<Action>();

        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }
        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
        private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

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
        }

        public void Update()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            foreach (Action a in _currentActions)
            {
                a();
            }
            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));

                foreach (DelayedQueueItem item in _currentDelayed)
                {
                    _delayed.Remove(item);
                }
            }
            foreach (DelayedQueueItem delayed in _currentDelayed)
            {
                delayed.action();
            }

        }

        public void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }

        public void QueueOnMainThread(Action action, float time)
        {
            if (time != 0)
            {
                lock (_delayed)
                {
                    _delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
            else
            {
                lock (_actions)
                {
                    _actions.Add(action);
                }
            }
        }

        public void RunAsync(Action a)
        {
            ThreadPool.QueueUserWorkItem(RunAction, a);
        }

        private void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch (Exception e)
            {
                Debug.Log("Exception in Loom runaction: " + e.Message + e.StackTrace);
            }
        }
    }
}