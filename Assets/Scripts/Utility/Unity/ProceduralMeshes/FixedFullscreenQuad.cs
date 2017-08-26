// ReSharper disable once CheckNamespace
namespace DLS.Utility.Unity.ProceduralMeshes
{
    using UnityEngine;
    using Cameras;
    using JetBrains.Annotations;

    /// <summary>
    /// Generates a quad and ensures that it always fills the viewport of
    /// TargetCamera.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [UsedImplicitly]
    public class FixedFullscreenQuad : MonoBehaviour
    {
        public Camera TargetCamera;
        public bool MaintainProportions;
        public float WidthHeightRatio;

        [UsedImplicitly]
        private void Start()
        {
            if (TargetCamera == null)
            {
                Debug.LogError("TargetCamera not set on ConstantSizeQuad. Defaulting to main camera.");
                TargetCamera = Camera.main;
            }
            var filter = GetComponent<MeshFilter>();
            filter.mesh = QuadBuilder.GetQuad("Fullscreen Procedural Quad");
        }

        [UsedImplicitly]
        private void Update()
        {
            if (MaintainProportions)
            {
                CameraBoundsHelper.ScaleAxisAlignedTransformToFillCamera(TargetCamera, transform, WidthHeightRatio);
            }
            else
            {
                CameraBoundsHelper.ScaleAxisAlignedTransformToFillCamera(TargetCamera, transform);
            }
        }
    }
}