namespace DLS.LD39.Map
{
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class TileMap : MonoBehaviour
    {
        public int Width;
        public int Height;
        public Vector2 TileSize;

        private MeshFilter _filter;
        private BoxCollider2D _collider;
        private TileMapMesh _mesh;

        public Vector2 WorldSize
        {
            get; private set;
        }

        /// <summary>
        /// Gets the local coordinates of the center of the given
        /// tile
        /// </summary>
        /// <param name="x">x position of the tile</param>
        /// <param name="y">y position of the tile</param>
        /// <returns>The tile's center in local space</returns>
        public Vector2 GetLocalCoords(int x, int y)
        {
            var tileLocal = new Vector2(
                x * TileSize.x,
                y * TileSize.y);
            tileLocal += new Vector2(
                -WorldSize.x / 2.0f, -WorldSize.y / 2.0f);
            return tileLocal + new Vector2(TileSize.x / 2.0f, TileSize.y / 2.0f);
        }

        /// <summary>
        /// Gets the world coordinates of the center of the given
        /// tile
        /// </summary>
        /// <param name="x">x position of the tile</param>
        /// <param name="y">y position of the tile</param>
        /// <returns>The tile's center in world space</returns>
        public Vector2 GetWorldCoords(int x, int y)
        {
            return transform.TransformPoint(GetLocalCoords(x, y));
        }

        /// <summary>
        /// Returns the x, y coordinates of a tile at a given point in
        /// world space. Tiles are numbered from 0,0 at the bottom left
        /// to width-1, height-1 at the top right.
        /// </summary>
        /// <param name="worldCoords">The world space point to check</param>
        /// <param name="x">X coordinate of the tile</param>
        /// <param name="y">Y coordinate of the tile</param>
        /// <returns>True if the given world space coordinate lies within the
        /// tile map.</returns>
        public bool GetTileCoords(Vector2 worldCoords, out int x, out int y)
        {
            x = y = -1;
            var localCoords = transform.InverseTransformPoint(worldCoords);

            // Translate so we're starting from the upper left corner
            var halfWidth = WorldSize.x / 2.0f;
            var halfHeight = WorldSize.y / 2.0f;
            localCoords += new Vector3(halfWidth, halfHeight, 0.0f);

            x = (int)localCoords.x;
            y = (int)localCoords.y;

            return !(localCoords.x < 0.0f || localCoords.x > WorldSize.x ||
                localCoords.y < 0.0f || localCoords.y > WorldSize.y);
        }

        public void SetTileAt(int x, int y)
        {
            _mesh.SetTileUV(new Vector2(0.25f, 0.0f), 0.25f, 1.0f, x, y);
        }

        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _collider = GetComponent<BoxCollider2D>();
            GetMesh();
            WorldSize = new Vector2(Width * TileSize.x,
                Width * TileSize.y);
            _collider.size = WorldSize;
        }

        private void Update()
        {
            _mesh.UpdateMesh();
        }

        private void GetMesh()
        {
            _mesh = new TileMapMesh(TileSize, Width, Height);
            _filter.sharedMesh = _mesh.Mesh;
        }
    }
}