﻿namespace DLS.LD39.Map
{
    using Utility;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(MeshRenderer))]
    public class TileMap : MonoBehaviour
    {
        private static readonly Dictionary<TileEdge, Vector2> EdgeMap = new Dictionary<TileEdge, Vector2>()
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
        public string DefaultTile = "floor_tile";
        public int Width;
        public int Height;
        public Vector2 TileSize;

        private TileSet _tileSet;
        private MeshFilter _filter;
        private BoxCollider2D _collider;
        private TileMapMesh _mesh;
        private readonly List<Tile> _tiles = new List<Tile>();

        public Vector2 WorldSpaceSize
        {
            get; private set;
        }

        public IEnumerable<Tile> Tiles
        {
            get
            {
                return _tiles;
            }
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

        public bool TileCoordsValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public Tile GetTile(IntVector2 pos)
        {
            return GetTile(pos.X, pos.Y);
        }

        public Tile GetTile(int x, int y)
        {
            if (!TileCoordsValid(x, y))
            {
                return null;
            }
            return _tiles[GetTileIdx(x, y)];
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
            SetTileAt(x, y, _tileSet.GetIndexedTile(tileType));
        }

        public void SetTileAt(int x, int y, string tileType)
        {
            SetTileAt(x, y, _tileSet.GetTileByID(tileType));
        }

        public void RebuildMap()
        {
            CleanupOldMesh();
            GetMesh();
            _tiles.Clear();
            WorldSpaceSize = new Vector2(Width * TileSize.x,
                Height * TileSize.y);
            _collider.size = WorldSpaceSize;

            BuildTiles();
            BuildAdjacenyData();
        }

        [UsedImplicitly]
        private void Awake()
        {
            _tileSet = new TileSet(TileData);
            _filter = GetComponent<MeshFilter>();
            _collider = GetComponent<BoxCollider2D>();

            RebuildMap();
        }

        [UsedImplicitly]
        private void Update()
        {
            _mesh.UpdateMesh();
        }

        private void SetTileAt(int x, int y, IndexedTile tile)
        {
            var tileObj = GetTile(x, y);
            if (tileObj == null) return;
            tileObj.SetType(tile.Type);
            _mesh.SetTileUV(tile.BottomLeft, tile.Width, tile.Height, x, y);
        }

        private void BuildTiles()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    _tiles.Add(new Tile(x, y, this));
                    SetTileAt(x, y, DefaultTile);
                }
            }
        }

        private void BuildAdjacenyData()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var tile = GetTile(x, y);
                    foreach (var edge in EdgeMap)
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

        private void GetMesh()
        {
            _mesh = new TileMapMesh(TileSize, Width, Height);
            _filter.sharedMesh = _mesh.Mesh;
        }

        private void CleanupOldMesh()
        {
            if (_mesh != null)
            {
                Destroy(_mesh.Mesh);
            }
            _mesh = null;
        }

        private int GetTileIdx(int x, int y)
        {
            return y * Width + x;
        }
    }
}