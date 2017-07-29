namespace DLS.Utility.Unity.Cameras
{
    using UnityEngine;

    public static class CameraBoundsHelper
    {
        /// <summary>
        /// Calculates the upper left and bottom right bounds of a camera
        /// in world space
        /// </summary>
        public static void GetWorldViewportBounds(Camera camera, float depth, out Vector3 topLeft, out Vector3 botRight)
        {
            botRight = camera.ViewportToWorldPoint(new Vector3(1, 0, depth));
            topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, depth));
        }

        /// <summary>
        /// Scales an object along the x, y axis to fill the camera's viewport.
        /// Uses the object's current depth, but sets its x, y position to 0, 0
        /// </summary>
        public static void ScaleAxisAlignedTransformToFillCamera(Camera camera, Transform transform)
        {
            Vector3 topLeft, botRight;
            GetWorldViewportBounds(camera, transform.localPosition.z, out topLeft, out botRight);
            var width = Mathf.Abs(botRight.x - topLeft.x);
            var height = Mathf.Abs(topLeft.y - botRight.y);
            transform.localPosition = new Vector3(0.0f, 0.0f, transform.localPosition.z);
            transform.localScale = new Vector3(width, height, 1.0f);
        }

        /// <summary>
        /// Scales an object along the x, y axis to fill the camera's viewport
        /// while maintaining a set ratio of width : height. Uses the object's
        /// current depth, but sets its x, y position to 0,0
        /// </summary>
        public static void ScaleAxisAlignedTransformToFillCamera(Camera camera, Transform transform, float ratio)
        {
            Vector3 topLeft, botRight;
            GetWorldViewportBounds(camera, transform.localPosition.z, out topLeft, out botRight);
            var screenWidth = Mathf.Abs(botRight.x - topLeft.x);
            var screenHeight = Mathf.Abs(topLeft.y - botRight.y);
            var screenRatio = screenWidth / screenHeight;
            float width, height;
            if (screenRatio > ratio)
            {
                width = screenWidth;
                height = width * (1 / ratio);
            }
            else
            {
                height = screenHeight;
                width = height * ratio;
            }

            transform.localPosition = new Vector3(0.0f, 0.0f, transform.localPosition.z);
            transform.localScale = new Vector3(width, height, 1.0f);
        }
    }
}