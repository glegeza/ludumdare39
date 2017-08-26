// ReSharper disable once CheckNamespace
namespace DLS.Utility.Unity.ProceduralMeshes
{
    using Cameras;
    using UnityEngine;

    public static class QuadBuilder
    {
        /// <summary>
        /// Builds a quad of width and height centered at x, y
        /// </summary>
        public static Mesh GetQuad(float x, float y, float width, float height, string meshName = "ProcQuad")
        {
            return GetQuad(x, y, width, height, Vector2.zero, Vector2.one, meshName);
        }

        /// <summary>
        /// Builds a quad of width and height centered at x, y
        /// </summary>
        public static Mesh GetQuad(float x, float y, float width, float height, Vector2 botLeftUV, Vector2 topRightUV, string meshName = "ProcQuad")
        {
            // Vertex layout
            // 0 -------- 1
            // |          |
            // |          |
            // |          |
            // |          |
            // 2 -------- 3

            var topLeftUV = new Vector2(botLeftUV.x, topRightUV.y);
            var botRightUV = new Vector2(topRightUV.x, botLeftUV.y);

            var halfWidth = width / 2.0f;
            var halfHeight = height / 2.0f;

            Vector3[] verts = new Vector3[]
            {
                new Vector3(x - halfWidth, y + halfHeight),
                new Vector3(x + halfWidth, y + halfHeight),
                new Vector3(x - halfWidth, y - halfHeight),
                new Vector3(x + halfWidth, y - halfHeight)
            };
            Vector2[] uv = new Vector2[]
            {
                topLeftUV, topRightUV, botLeftUV, botRightUV
            };
            //Vector2[] uv = new Vector2[]
            //{
            //    new Vector2(0.0f, 1.0f),
            //    new Vector2(1.0f, 1.0f),
            //    new Vector2(0.0f, 0.0f),
            //    new Vector2(1.0f, 0.0f)
            //};
            int[] tris = new int[]
            {
                0, 1, 2,
                2, 1, 3
            };

            var mesh = new Mesh();
            mesh.name = "GenQuad";
            mesh.vertices = verts;
            mesh.uv = uv;
            mesh.triangles = tris;

            return mesh;
        }

        /// <summary>
        /// Builds a 1x1 quad Mesh centered at 0,0
        /// </summary>
        public static Mesh GetQuad(Vector2 botLeftUV, Vector2 topRightUV, string meshName = "ProcQuad")
        {
            return GetQuad(0.0f, 0.0f, 1.0f, 1.0f, botLeftUV, topRightUV, meshName);
        }

        /// <summary>
        /// Builds a 1x1 quad Mesh centered at 0,0
        /// </summary>
        public static Mesh GetQuad(string meshName = "ProcQuad")
        {
            return GetQuad(0.0f, 0.0f, 1.0f, 1.0f, meshName);
        }

        /// <summary>
        /// Creates a quad scaled to fill a camera's viewport.
        /// </summary>
        /// <param name="camera">The target camera</param>
        /// <param name="depth">The depth the quad will be placed at </param>
        /// <param name="objName">The name of the new object</param>
        /// <param name="quadMaterial">The material to use for quad's mesh
        /// renderer.</param>
        /// <param name="parent">A parent object to attach the quad to.</param>
        /// <returns>A new GameObject with attached MeshFilter and MeshRenderer
        /// components to display a scaled quad.</returns>
        public static GameObject GetFullscreenScaledQuad(Camera camera, float depth, string objName = "FullscreenQuad", Material quadMaterial = null, Transform parent = null)
        {
            var quad = GetQuadObject(objName, quadMaterial, parent);
            quad.transform.localPosition = new Vector3(0f, 0f, depth);
            CameraBoundsHelper.ScaleAxisAlignedTransformToFillCamera(camera, quad.transform);
            return quad;
        }

        /// <summary>
        /// Builds a GameObject with a MeshRenderer and MeshFilter to display
        /// a quad of width x height centered at x, y
        /// </summary>
        /// <param name="x">Center x position of the quad</param>
        /// <param name="y">Center y position of the quad</param>
        /// <param name="height">Height of the quad</param>
        /// <param name="width">Width of the quad</param>
        /// <param name="objName">Name of the new GameObject</param>
        /// <param name="quadMaterial">Material to use to render the quad</param>
        /// <param name="parent">Object to parent the new quad to</param>
        public static GameObject GetQuadObject(float x, float y, float width, float height, string objName = "ProcQuad", Material quadMaterial = null, Transform parent = null)
        {
            var gameObject = new GameObject(objName);
            var mf = gameObject.AddComponent<MeshFilter>();
            mf.mesh = GetQuad(x, y, width, height, objName);

            var mr = gameObject.AddComponent<MeshRenderer>();
            mr.material = quadMaterial;

            if (parent)
            {
                gameObject.transform.SetParent(parent, false);
            }

            return gameObject;
        }

        /// <summary>
        /// Builds a GameObject with a MeshRenderer and MeshFilter to 
        /// display a 1x1 quad centered at 0,0
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="quadMaterial"></param>
        /// <param name="parent">Object to parent the new quad to</param>
        /// <returns></returns>
        public static GameObject GetQuadObject(string objName = "ProcQuad", Material quadMaterial = null, Transform parent = null)
        {
            return GetQuadObject(0.0f, 0.0f, 1.0f, 1.0f, objName, quadMaterial, parent);
        }
    }
}