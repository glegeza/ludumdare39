namespace DLS.LD39
{
    using UnityEngine;
    using DLS.Utility.Unity.Cameras;

    /// <summary>
    /// Responsible for setting up all of the other persistent game
    /// managers, controllers, etc.
    /// </summary>
    class GameInitializer : SingletonComponent<GameInitializer>
    {
        private SimplePixelPerfectOrthoCamera _orthoCamera;
        private int _prevHeight = 0;

        private void Start()
        {
            _orthoCamera = FindObjectOfType<SimplePixelPerfectOrthoCamera>();
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
