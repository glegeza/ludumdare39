namespace DLS.Utility.Unity.Cameras
{
    using System;
    using UnityEngine;

    public static class OrthographicCameraHelper
    {
        /// <summary>
        /// Returns the proper orthographic size for pixel art given a target
        /// vertical resolution, pixels per unit, and desired scaling factor
        /// </summary>
        /// <param name="verticalSize">Vertical screen size in pixels</param>
        /// <param name="PPU">Pixels per unit setting for art</param>
        /// <param name="scale">Desired art scaling</param>
        /// <returns>Orthographic camera size</returns>
        public static float GetOrthoSize(int verticalSize, int PPU, float scale)
        {
            return (verticalSize / (scale * PPU)) * 0.5f;
        }

        /// <summary>
        /// Scales a transform to fill the viewport of an orthographic camera
        /// </summary>
        public static void SizeToFitOrthographicCamera(this Transform transform, Camera camera)
        {
            if (!camera.orthographic)
            {
                throw new Exception("Attempting to fit transform to non-orthographic camera.");
            }

            var height = camera.orthographicSize * 2;
            var width = height * camera.aspect;
            transform.localScale = new Vector3(width, height, transform.localScale.z);
        }

        /// <summary>
        /// Calculates the field of view necessary for a perspective camera's
        /// frustum to intersect with the frustum of an orthographic camera
        /// at a particular depth.
        /// </summary>
        /// <param name="orthoSize">Size setting of the orthographic camera
        /// </param>
        /// <param name="distanceFromOrigin">Desired distance of
        /// intersection</param>
        /// <returns>Field of view value for the perspective camera</returns>
        /// <remarks>http://www.gamasutra.com/blogs/MichalBerlinger/20160323/268657/Combining_Perspective_and_Orthographic_Camera_for_Parallax_Effect_in_2D_Game.php</remarks>
        public static float GetFieldOfView(float orthoSize, float distanceFromOrigin)
        {
            float a = orthoSize;
            float b = Mathf.Abs(distanceFromOrigin);

            float fieldOfView = Mathf.Atan(a / b) * Mathf.Rad2Deg * 2f;
            return fieldOfView;
        }

        /// <summary>
        /// Calculates the field of view necessary for a perspective camera's
        /// frustum to intersect with this orthographic camera's frustum at
        /// a particular depth.
        /// </summary>
        /// <param name="camera">The orthographic camera</param>
        /// <param name="distanceFromOrigin">Desired intersection distance
        /// </param>
        /// <returns>Field of view for the perspective camera</returns>
        public static float GetPerspectiveFieldOfView(this Camera camera, float distanceFromOrigin)
        {
            return GetFieldOfView(camera.orthographicSize, distanceFromOrigin);
        }
    }
}
