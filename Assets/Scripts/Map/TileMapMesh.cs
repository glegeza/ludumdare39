namespace DLS.LD39.Map
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Generates an arbitrarily sized plane of quads that can be textured
    /// as individual tiles by U,V coordinate.
    /// </summary>
    public class TileMapMesh
    {
        private List<Vector3> _verts = new List<Vector3>();
        private List<Vector2> _uvs = new List<Vector2>();
        private List<int> _tris = new List<int>();

        private bool _isDirty = false;

        /// <summary>
        /// Generates a plane composed of quad tiles.
        /// </summary>
        /// <param name="tileSize">Size of each tile in world units</param>
        /// <param name="width">The number of tiles across</param>
        /// <param name="height">The number of tiles top to bottom</param>
        /// <param name="meshName">The name of the generated mesh</param>
        public TileMapMesh(Vector2 tileSize, int width, int height, string meshName="ProcTileQuad")
        {
            // Vertex layout
            // 0 -------- 1
            // |          |
            // |          |
            // |          |
            // |          |
            // 2 -------- 3

            Width = width;
            Height = height;

            var planeSize = 
                new Vector2(tileSize.x * width, tileSize.y * height);
            var planeUpperLeft =
                new Vector2(-planeSize.x / 2.0f, -planeSize.y / 2.0f);

            var topLeftUV = new Vector2(0.0f, 1.0f);
            var topRightUV = new Vector2(0.25f, 1.0f);
            var botRightUV = new Vector2(0.25f, 0.0f);
            var botLeftUV = new Vector2(0.0f, 0.0f);

            var curIdx = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    // Everything vertex is offset by one tile UP only
                    // because this was misaligning and I have no idea
                    // why

                    _verts.Add(new Vector3(
                        x * tileSize.x + planeUpperLeft.x,
                        y * tileSize.y + planeUpperLeft.y + tileSize.y));
                    _verts.Add(new Vector3(
                        x * tileSize.x + tileSize.x + planeUpperLeft.x,
                        y * tileSize.y + planeUpperLeft.y + tileSize.y));
                    _verts.Add(new Vector3(
                        x * tileSize.x + planeUpperLeft.x,
                        y * tileSize.y - tileSize.y + planeUpperLeft.y + tileSize.y));
                    _verts.Add(new Vector3(
                        x * tileSize.x + tileSize.x + planeUpperLeft.x,
                        y * tileSize.y - tileSize.y + planeUpperLeft.y + tileSize.y));

                    _uvs.Add(topLeftUV);
                    _uvs.Add(topRightUV);
                    _uvs.Add(botLeftUV);
                    _uvs.Add(botRightUV);

                    _tris.Add(curIdx + 0);
                    _tris.Add(curIdx + 1);
                    _tris.Add(curIdx + 2);
                    _tris.Add(curIdx + 2);
                    _tris.Add(curIdx + 1);
                    _tris.Add(curIdx + 3);

                    curIdx += 4;
                }
            }

            Mesh = new Mesh()
            {
                name = meshName
            };
            Mesh.SetVertices(_verts);
            Mesh.SetUVs(0, _uvs);
            Mesh.SetTriangles(_tris, 0);
        }

        public Mesh Mesh
        {
            get; private set;
        }

        public int Height
        {
            get; private set;
        }

        public int Width
        {
            get; private set;
        }

        public void UpdateMesh()
        {
            if (_isDirty)
            {
                Mesh.SetUVs(0, _uvs);
                _isDirty = false;
            }
        }

        public void SetTileUV(Vector2 uvBotLeft, float uvWidth, float uvHeight, int x, int y)
        {
            var uvTopLeft = new Vector2(uvBotLeft.x, uvBotLeft.y + uvHeight);
            var uvTopRight = new Vector2(uvTopLeft.x + uvWidth, uvTopLeft.y);
            var uvBotRight = new Vector2(uvBotLeft.x + uvWidth, uvBotLeft.y);

            var startIdx = (y * Height + x) * 4;

            _uvs[startIdx] = uvTopLeft;
            _uvs[startIdx + 1] = uvTopRight;
            _uvs[startIdx + 2] = uvBotLeft;
            _uvs[startIdx + 3] = uvBotRight;

            _isDirty = true;
        }
    }
}
