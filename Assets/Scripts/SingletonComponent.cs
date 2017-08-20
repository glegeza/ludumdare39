namespace DLS.LD39
{
    using System;
    using UnityEngine;

    public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        var name = typeof(T).Name;
                        Debug.LogFormat("No instance of singleton {0} found, creating one.",
                            name);
                        var container = new GameObject(String.Format("{0} Container", name));
                        instance = container.AddComponent<T>();
                    }
                    _instance = instance;
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogErrorFormat("Duplicate {0} singleton. Destroying instance attached to {1}.", 
                    typeof(T).Name, gameObject.name);
                Destroy(this);
            }
            else
            {
                _instance = (T)this;
            }
        }
    }
}
