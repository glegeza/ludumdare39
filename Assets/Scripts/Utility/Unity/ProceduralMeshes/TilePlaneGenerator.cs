namespace DLS.Utility.Unity.ProceduralMeshes
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Generates an arbitrarily sized plane of quads that can be textured
    /// as individual tiles by U,V coordinate.
    /// </summary>
    public static class TilePlaneGenerator
    {
        /// <summary>
        /// Generates a plane composed of quad tiles.
        /// </summary>
        /// <param name="tileSize">Size of each tile in world units</param>
        /// <param name="width">The number of tiles across</param>
        /// <param name="height">The number of tiles top to bottom</param>
        /// <param name="meshName">The name of the generated mesh</param>
        /// <returns>The generated mesh</returns>
        public static Mesh GetPlane(Vector2 tileSize, int width, int height, string meshName="ProcTileQuad")
        {
            // Vertex layout
            // 0 -------- 1
            // |          |
            // |          |
            // |          |
            // |          |
            // 2 -------- 3

            var planeSize = 
                new Vector2(tileSize.x * width, tileSize.y * height);
            var planeUpperLeft =
                new Vector2(-planeSize.x / 2.0f, -planeSize.y / 2.0f);

            var topLeftUV = new Vector2(0.0f, 1.0f);
            var topRightUV = new Vector2(1.0f, 1.0f);
            var botRightUV = new Vector2(1.0f, 0.0f);
            var botLeftUV = new Vector2(0.0f, 0.0f);

            var verts = new List<Vector3>();
            var uv = new List<Vector2>();
            var tris = new List<int>();

            var curIdx = 0;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    // Everything vertex is offset by one tile UP only
                    // because this was misaligning and I have no idea
                    // why

                    verts.Add(new Vector3(
                        x * tileSize.x + planeUpperLeft.x,
                        y * tileSize.y + planeUpperLeft.y + tileSize.y));
                    verts.Add(new Vector3(
                        x * tileSize.x + tileSize.x + planeUpperLeft.x,
                        y * tileSize.y + planeUpperLeft.y + tileSize.y));
                    verts.Add(new Vector3(
                        x * tileSize.x + planeUpperLeft.x,
                        y * tileSize.y - tileSize.y + planeUpperLeft.y + tileSize.y));
                    verts.Add(new Vector3(
                        x * tileSize.x + tileSize.x + planeUpperLeft.x,
                        y * tileSize.y - tileSize.y + planeUpperLeft.y + tileSize.y));

                    uv.Add(topLeftUV);
                    uv.Add(topRightUV);
                    uv.Add(botLeftUV);
                    uv.Add(botRightUV);

                    tris.Add(curIdx + 0);
                    tris.Add(curIdx + 1);
                    tris.Add(curIdx + 2);
                    tris.Add(curIdx + 2);
                    tris.Add(curIdx + 1);
                    tris.Add(curIdx + 3);

                    curIdx += 4;
                }
            }

            var mesh = new Mesh()
            {
                name = meshName
            };
            mesh.SetVertices(verts);
            mesh.SetUVs(0, uv);
            mesh.SetTriangles(tris, 0);

            return mesh;
        }
    }
}
