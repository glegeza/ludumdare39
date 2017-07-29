namespace DLS.USG.Prototypes.Scrolling.Scripts
{
    using UnityEngine;
    using Utility.Unity.Cameras;
    using Utility.Unity.ProceduralMeshes;

    /// <summary>
    /// Generates a quad and ensures that it always fills the viewport of
    /// TargetCamera.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class FixedFullscreenQuad : MonoBehaviour
    {
        public Camera TargetCamera;
        public bool MaintainProportions;
        public float WidthHeightRatio;

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