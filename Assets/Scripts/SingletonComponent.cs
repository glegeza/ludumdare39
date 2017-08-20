namespace DLS.LD39
{
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
                        Debug.Log("No instance found, creating one.");
                        var container = new GameObject("Singleton Container");
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
                Destroy(gameObject);
            }
            else
            {
                _instance = (T)this;
            }
        }
    }
}
