namespace DLS.Utility.Unity.ProceduralMeshes
{
    using UnityEngine;

    public static class CircleMeshGenerator
    {
        /// <summary>
        /// Generates a 2d circle or ellipse mesh.
        /// </summary>
        /// <param name="segments">The number of triangles segments the mesh
        /// will be composed of</param>
        /// <param name="color">Default vertex color</param>
        /// <param name="a">Semi-major axis</param>
        /// <param name="b">Semi-minor axiis</param>
        /// <returns>Constructed circle Mesh object</returns>
        public static Mesh GetMesh(int segments, Color color, float a=1, float b=1)
        {
            var mesh = new Mesh();
            var vertCount = segments + 1;
            var vertices = new Vector3[vertCount];
            var tris = new int[segments * 3];
            var colors = new Color[vertCount];
            var normals = new Vector3[vertCount];

            var step = (Mathf.PI * 2) / segments;
            for (var i = 0; i < segments; i++)
            {
                var angle = i * step;
                var x = a*Mathf.Cos(angle);
                var y = b*Mathf.Sin(angle);
                var z = 0.0f;
                vertices[i] = new Vector3(x, y, z);
                colors[i] = color;
                normals[i] = Vector3.forward;
            }
            vertices[vertices.Length - 1] = new Vector3(0, 0, 0);
            colors[vertices.Length - 1] = color;
            normals[vertices.Length - 1] = Vector3.forward;

            var centerIdx = vertices.Length - 1;

            for (var i = 0; i < segments; i++)
            {
                var currentTri = i * 3;
                tris[currentTri] = i < segments - 1 ? i + 1 : 0;
                tris[currentTri + 1] = i;
                tris[currentTri + 2] = centerIdx;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors = colors;
            mesh.triangles = tris;
            return mesh;
        }
    }
}
