namespace DLS.LD39
{
    using UnityEngine;
    using Controllers;
    using DLS.Utility.Unity.Cameras;

    /// <summary>
    /// Responsible for setting up all of the other persistent game
    /// managers, controllers, etc.
    /// </summary>
    class GameInitializer : MonoBehaviour
    {
        private static GameInitializer _instance;
        private SimplePixelPerfectOrthoCamera _orthoCamera;
        private int _prevHeight = 0;

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
            _orthoCamera = FindObjectOfType<SimplePixelPerfectOrthoCamera>();
        }

        private void Start()
        {
            MapClickRouter.Instance.Initialize();
            _prevHeight = Camera.main.pixelHeight;
            _orthoCamera.VerticalResolution = Camera.main.pixelHeight;
            _orthoCamera.UpdateParams();
        }

        private void Update()
        {
            if (Camera.main.pixelHeight != _prevHeight)
            {
                Debug.LogFormat("Updating resolution from {0} to {1}", _prevHeight, Camera.main.pixelHeight);
                _prevHeight = Camera.main.pixelHeight;
                _orthoCamera.VerticalResolution = _prevHeight;
                _orthoCamera.UpdateParams();
            }
        }
    }
}
