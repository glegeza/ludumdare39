namespace DLS.LD39
{
    using UnityEngine;
    using Controllers;

    /// <summary>
    /// Responsible for setting up all of the other persistent game
    /// managers, controllers, etc.
    /// </summary>
    class GameInitializer : MonoBehaviour
    {
        private static GameInitializer _instance;

        public static GameInitializer Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            MapClickController.Instance.Initialize();
        }
    }
}
