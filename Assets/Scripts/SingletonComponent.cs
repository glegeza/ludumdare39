namespace DLS.LD39
{
    using UnityEngine;

    public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>
    {

        // Singleton reference
        private static T _instance;

        public static T Instance
        {
            get
            {
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
