namespace DLS.LD39
{
    using JetBrains.Annotations;
    using UnityEngine;
    using Utility.Unity.Cameras;

    /// <summary>
    /// Responsible for setting up all of the other persistent game
    /// managers, controllers, etc.
    /// </summary>
    [UsedImplicitly]
    class GameInitializer : SingletonComponent<GameInitializer>
    {
        private SimplePixelPerfectOrthoCamera _orthoCamera;
        private int _prevHeight;

        [UsedImplicitly]
        private void Start()
        {
            _orthoCamera = FindObjectOfType<SimplePixelPerfectOrthoCamera>();
            _prevHeight = Camera.main.pixelHeight;
            _orthoCamera.VerticalResolution = Camera.main.pixelHeight;
            _orthoCamera.UpdateParams();
        }

        [UsedImplicitly]
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
