// ReSharper disable CheckNamespace
namespace DLS.Utility.Unity.Cameras
{
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class SimplePixelPerfectOrthoCamera : MonoBehaviour
    {
        private Camera _camera;

        public int VerticalResolution;
        public int TargetPPU;
        public float TargetScale;

        public float UnitsPerPixel { get; private set; }

        public void UpdateParams()
        {
            UpdateCamera();
        }

        [UsedImplicitly]
        private void Start()
        {
            _camera = GetComponent<Camera>();
            UpdateCamera();
        }

        [UsedImplicitly]
        private void OnValidate()
        {
            if (VerticalResolution <= 0 || TargetPPU <= 0 || TargetScale <= 0.0f)
            {
                return;
            }
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            if (_camera == null)
            {
                _camera = GetComponent<Camera>();
            }
            _camera.orthographicSize = OrthographicCameraHelper.GetOrthoSize(VerticalResolution, TargetPPU, TargetScale);
            UnitsPerPixel = 1.0f / TargetPPU;
        }
    }
}
