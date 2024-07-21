using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Framework
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static bool canDestroyOnLoad;
        
        private static volatile T _instance;
        private static readonly ConcurrentDictionary<Type, object> locks = new ();

        public static T Instance
        {
            get
            {
                if (_instance!= null)
                    return _instance;

                if (!locks.TryGetValue(typeof(T), out object lockObject))
                {
                    lockObject = new object();
                    locks.TryAdd(typeof(T), lockObject);
                }

                lock (lockObject)
                {
                    if (_instance!= null)
                        return _instance;

                    T instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject singletonObject = new (typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                    }

                    if (canDestroyOnLoad)
                        DontDestroyOnLoad(instance.gameObject);

                    _instance = instance;
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null
                && _instance != this as T)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            
            if (canDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}