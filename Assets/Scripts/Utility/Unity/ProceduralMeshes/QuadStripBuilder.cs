namespace DLS.Utility.Unity.ProceduralMeshes
{
    using System;
    using UnityEngine;

    public static class QuadStripBuilder
    {
        /// <summary>
        /// Builds a procedural strip of quad segments
        /// </summary>
        /// <param name="segments">The number of quads the strip is composed of
        /// </param>
        /// <param name="length">The length of the strip in world units</param>
        /// <param name="height">The height of the strip in world units</param>
        /// <returns>The completed quad strip mesh</returns>
        public static Mesh GetQuadStripMesh(int segments, float length, float height)
        {
            return GetQuadStripMesh(segments, length, height, Vector2.zero, Vector2.one, 0.0f);
        }

        /// <summary>
        /// Builds a procedural strip of quad segments
        /// </summary>
        /// <param name="segments">The number of quads the strip is composed of
        /// </param>
        /// <param name="length">The length of the strip in world units</param>
        /// <param name="height">The height of the strip in world units</param>
        /// <param name="botLeftUV">Bottom left texture UV coord</param>
        /// <param name="topRightUV">Top right texture UV coord</param>
        /// <returns>The completed quad strip mesh</returns>
        public static Mesh GetQuadStripMesh(int segments, float length, float height, Vector2 botLeftUV, Vector2 topRightUV, float uvRotation)
        {
            if (segments < 1 || length <= 0.0f)
            {
                throw new ArgumentException("QuadStrip must have 1 or more segments and a positive length");
            }
            var botRightUV = new Vector2(topRightUV.x, botLeftUV.y);
            var topLeftUV = new Vector2(botLeftUV.x, topRightUV.y);

            var texWidth = Mathf.Abs(topLeftUV.x - botRightUV.x);
            var texHeight = Mathf.Abs(topLeftUV.y - botRightUV.y);

            var offset = new Vector2(texWidth, 0.0f);
            var tiling = new Vector3(1.0f, 1.0f, 1.0f);
            Quaternion quat = Quaternion.Euler(0, 0, uvRotation);
            Matrix4x4 matrix2 = Matrix4x4.TRS(Vector3.zero, quat, tiling);

            botLeftUV = matrix2 * botLeftUV;
            topRightUV = matrix2 * topRightUV;
            botRightUV = matrix2 * botRightUV;
            topLeftUV = matrix2 * topLeftUV;

            botLeftUV += offset;
            topRightUV += offset;
            botRightUV += offset;
            topLeftUV += offset;

            var segmentLength = length / segments;
            var xStart = -length / 2.0f;
            var x = xStart;
            var halfHeight = height / 2.0f;
            var normal = new Vector3(0.0f, 0.0f, -1.0f);

            var verts = new Vector3[segments * 4];
            var colors = new Color[segments * 4];
            var normals = new Vector3[segments * 4];
            var uvs = new Vector2[segments * 4];
            var tris = new int[segments * 6];

            for (int i = 0; i < segments; i++)
            {
                var startVert = i * 4;
                uvs[startVert]         = topLeftUV;
                verts[startVert]       = new Vector3(x, halfHeight, 0.0f);
                colors[startVert]      = Color.white;
                normals[startVert]     = normal;

                uvs[startVert + 1]     = botLeftUV;
                verts[startVert + 1]   = new Vector3(x, -halfHeight, 0.0f);
                colors[startVert + 1]  = Color.white;
                normals[startVert + 1] = normal;

                uvs[startVert + 2]     = topRightUV;
                verts[startVert + 2]   = new Vector3(x + segmentLength, halfHeight, 0.0f);
                colors[startVert + 2]  = Color.white;
                normals[startVert + 2] = normal;

                uvs[startVert + 3]     = botRightUV;
                verts[startVert + 3]   = new Vector3(x + segmentLength, -halfHeight, 0.0f);
                colors[startVert + 3]  = Color.white;
                normals[startVert + 3] = normal;

                var startTri = i * 6;

                tris[startTri + 0] = startVert + 0;
                tris[startTri + 1] = startVert + 2;
                tris[startTri + 2] = startVert + 1;
                tris[startTri + 3] = startVert + 2;
                tris[startTri + 4] = startVert + 3;
                tris[startTri + 5] = startVert + 1;

                x += segmentLength;
            }

            return new Mesh()
            {
                vertices = verts,
                colors = colors,
                normals = normals,
                triangles = tris,
                uv = uvs
            };
        }
    }
}
