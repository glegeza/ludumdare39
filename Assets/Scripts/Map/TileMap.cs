namespace DLS.LD39.Map
{
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(MeshRenderer))]
    class TileMap : MonoBehaviour
    {
        static private Dictionary<TileEdge, Vector2> _edgeMap = new Dictionary<TileEdge, Vector2>()
        {
            { TileEdge.Down, new Vector2(0, -1) },
            { TileEdge.Left, new Vector2(-1, 0) },
            { TileEdge.Right, new Vector2(1, 0) },
            { TileEdge.Up, new Vector2(0, 1) },
            { TileEdge.DownLeft, new Vector2(-1, -1) },
            { TileEdge.DownRight, new Vector2(1, -1) },
            { TileEdge.UpLeft, new Vector2(-1, 1) },
            { TileEdge.UpRight, new Vector2(1, 1) },
        };

        public TileSetData TileData;
        public int DefaultTile = 7;
        public int Width;
        public int Height;
        public Vector2 TileSize;

        private TileSet _tileSet;
        private MeshFilter _filter;
        private BoxCollider2D _collider;
        private MeshRenderer _renderer;
        private TileMapMesh _mesh;
        private List<Tile> _tiles = new List<Tile>();
        private List<TileMap> _adjacentMaps = new List<TileMap>();

        public Vector2 WorldSpaceSize
        {
            get; private set;
        }

        /// <summary>
        /// Joins two tile maps.
        /// </summary>
        /// <param name="map1">The first map to join</param>
        /// <param name="map1Tile"></param>
        /// <param name="tile1Edge"></param>
        /// <param name="map2"></param>
        /// <param name="map2Tile"></param>
        /// <param name="tile2Edge"></param>
        public static void JoinMaps(TileMap map1, Tile map1Tile, TileEdge tile1Edge, TileMap map2, Tile map2Tile, TileEdge tile2Edge)
        {
            map1._adjacentMaps.Add(map2);
            map2._adjacentMaps.Add(map1);
            map1Tile.AddAdjacentTile(map2Tile, tile1Edge);
            map2Tile.AddAdjacentTile(map1Tile, tile2Edge);
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
                -WorldSpaceSize.x / 2.0f, -WorldSpaceSize.y / 2.0f);
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
            var halfWidth = WorldSpaceSize.x / 2.0f;
            var halfHeight = WorldSpaceSize.y / 2.0f;
            localCoords += new Vector3(halfWidth, halfHeight, 0.0f);

            x = (int)(localCoords.x / TileSize.x);
            y = (int)(localCoords.y / TileSize.y);

            return !(localCoords.x < 0.0f || localCoords.x > WorldSpaceSize.x ||
                localCoords.y < 0.0f || localCoords.y > WorldSpaceSize.y);
        }

        public Tile GetTile(int x, int y)
        {
            var idx = y * Width + x;
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return null;
            }
            return _tiles[y * Width + x];
        }

        public Tile GetTileAtWorldCoords(Vector2 worldCoords)
        {
            int x, y;
            if (!GetTileCoords(worldCoords, out x, out y))
            {
                return null;
            }

            return GetTile(x, y);
        }

        public void SetTileAt(int x, int y, int tileType)
        {
            var tile = _tileSet.GetIndexedTile(tileType);
            _mesh.SetTileUV(tile.BottomLeft, tile.Width, tile.Height, x, y);
        }

        private void Start()
        {
            _tileSet = new TileSet(TileData);
            _filter = GetComponent<MeshFilter>();
            _collider = GetComponent<BoxCollider2D>();
            _renderer = GetComponent<MeshRenderer>();

            GetMesh();
            WorldSpaceSize = new Vector2(Width * TileSize.x,
                Height * TileSize.y);
            _collider.size = WorldSpaceSize;

            // Create tiles
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _tiles.Add(new Tile(x, y, 0, this));
                    SetTileAt(x, y, DefaultTile);
                }
            }

            // Build adjacency data
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var tile = GetTile(x, y);
                    foreach (var edge in _edgeMap)
                    {
                        var adjX = x + (int)edge.Value.x;
                        var adjY = y + (int)edge.Value.y;
                        var adjTile = GetTile(adjX, adjY);
                        if (adjTile != null)
                        {
                            tile.AddAdjacentTile(adjTile, edge.Key);
                        }
                    }

                }
            }
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