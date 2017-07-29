namespace DLS.Utility.Unity.ProceduralMeshes
{
    using System;
    using UnityEngine;

    public static class QuadRingBuilder
    {
        /// <summary>
        /// Builds a procedural ring mesh composed of quad segments
        /// </summary>
        /// <param name="segments">The number of quads the ring is composed of
        /// </param>
        /// <param name="innerRadius">The radius of the inner edge of the ring
        /// </param>
        /// <param name="outerRadius">The radius of the outer edge of the ring
        /// </param>
        /// <returns>The completed quad ring mesh</returns>
        public static Mesh GetQuadRingMesh(int segments, float innerRadius, float outerRadius)
        {
            return GetQuadRingMesh(segments, innerRadius, outerRadius, Vector2.zero, Vector2.one);
        }

        /// <summary>
        /// Builds a procedural ring mesh composed of quad segments
        /// </summary>
        /// <param name="segments">The number of quads the ring is composed of
        /// </param>
        /// <param name="innerRadius">The radius of the inner edge of the ring
        /// </param>
        /// <param name="outerRadius">The radius of the outer edge of the ring
        /// </param>
        /// <param name="botLeftUV">Texture UV coordinates for the bottom left
        /// corner of each of the ring's quads</param>
        /// <param name="topRightUV">Texture UV coordinates for the top right
        /// corner of each of the ring's quads</param>
        /// <returns>The completed quad ring mesh</returns>
        public static Mesh GetQuadRingMesh(int segments, float innerRadius, float outerRadius, Vector2 botLeftUV, Vector2 topRightUV)
        {
            if (innerRadius >= outerRadius)
            {
                throw new ArgumentException(String.Format("Inner radius {0} must be smaller than outer radius {0}.", innerRadius, outerRadius));
            }
            if (segments < 3)
            {
                throw new ArgumentException(String.Format("Segments {0} must be >= 3", segments));
            }

            var botRightUV = new Vector2(topRightUV.x, botLeftUV.y);
            var topLeftUV = new Vector2(botLeftUV.x, topRightUV.y);

            var verts = new Vector3[segments * 4];
            var colors = new Color[segments * 4];
            var normals = new Vector3[segments * 4];
            var uvs = new Vector2[segments * 4];
            var tris = new int[segments * 6];

            var step = Mathf.PI * 2.0f / segments;

            var startVert = 0;
            var startTri = 0;
            for (var i = 0; i < segments; i++)
            {
                var angle = i * step;
                var nextAngle = (i < segments - 1 ? i + 1 : 0) * step;

                var x1 = Mathf.Cos(angle);
                var y1 = Mathf.Sin(angle);

                var x2 = Mathf.Cos(nextAngle);
                var y2 = Mathf.Sin(nextAngle);

                verts[startVert] = new Vector3(x1, y1, 0.0f) * innerRadius;
                colors[startVert] = Color.white;
                normals[startVert] = new Vector3(0.0f, 0.0f, -1.0f);
                uvs[startVert] = botLeftUV;

                verts[startVert + 1] = new Vector3(x2, y2, 0.0f) * innerRadius;
                colors[startVert + 1] = Color.white;
                normals[startVert + 1] = new Vector3(0.0f, 0.0f, -1.0f);
                uvs[startVert + 1] = topLeftUV;

                verts[startVert + 2] = new Vector3(x1, y1, 0.0f) * outerRadius;
                colors[startVert + 2] = Color.white;
                normals[startVert + 2] = new Vector3(0.0f, 0.0f, -1.0f);
                uvs[startVert + 2] = botRightUV;

                verts[startVert + 3] = new Vector3(x2, y2, 0.0f) * outerRadius;
                colors[startVert + 3] = Color.white;
                normals[startVert + 3] = new Vector3(0.0f, 0.0f, -1.0f);
                uvs[startVert + 3] = topRightUV;

                tris[startTri + 0] = startVert;
                tris[startTri + 1] = startVert + 1;
                tris[startTri + 2] = startVert + 2;
                tris[startTri + 3] = startVert + 2;
                tris[startTri + 4] = startVert + 1;
                tris[startTri + 5] = startVert + 3;

                startVert += 4;
                startTri += 6;
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
