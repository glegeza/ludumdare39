namespace DLS.LD39.Map
{
    using DLS.Utility;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tile : IEquatable<Tile>
    {
        private Dictionary<TileEdge, Tile> _adjacentTiles = new Dictionary<TileEdge, Tile>();

        public Tile(int x, int y, int layer, TileMap map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            if (x < 0 || y < 0 || x >= map.Width || y >= map.Height)
            {
                throw new Exception("tile x, y invalid");
            }
            if (layer < 0)
            {
                throw new Exception("tile layer invalid");
            }

            X = x;
            Y = y;
            Layer = layer;
            Map = map;
            LocalCoords = map.GetLocalCoords(x, y);
            Passable = true;
        }

        public TileMap Map
        {
            get;
            private set;
        }

        public Vector2 LocalCoords
        {
            get;
            private set;
        }

        public Vector2 WorldCoords
        {
            get
            {
                return Map.transform.TransformPoint(LocalCoords);
            }
        }

        public bool Passable
        {
            get; set;
        }

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public int Layer
        {
            get;
            private set;
        }

        public IEnumerable<Tile> AdjacentTiles
        {
            get
            {
                return _adjacentTiles.Values;
            }
        }

        public bool IsEnterable()
        {
            return Passable && UnitSpawner.Instance.UnitAtTile(this) == null;
        }

        public bool IsAdjacent(Tile tile)
        {
            if (tile == null)
            {
                throw new ArgumentNullException("tile");
            }

            foreach (var t in _adjacentTiles.Values)
            {
                if (t.Equals(tile))
                {
                    return true;
                }
            }
            return false;
        }

        public Tile GetAdjacent(TileEdge edge)
        {
            if (_adjacentTiles.ContainsKey(edge))
            {
                return _adjacentTiles[edge];
            }
            return null;
        }

        public void AddAdjacentTile(Tile tile, TileEdge edge)
        {
            if (_adjacentTiles.ContainsKey(edge))
            {
                var error = String.Format("Adding duplicate edge {0} to {1}", 
                    edge, this);
                Debug.LogErrorFormat(error);
                throw new ArgumentException(error);
            }

            _adjacentTiles.Add(edge, tile);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var tile = obj as Tile;
            if (tile == null)
            {
                return false;
            }

            return Equals(tile);
        }

        public override int GetHashCode()
        {
            return SimpleHashBuilder.GetHash(X, Y, Layer, Map);
        }

        public override string ToString()
        {
            return String.Format("{0} [{1}, {2}]", Map.name, X, Y);
        }

        public bool Equals(Tile other)
        {
            if (other == null)
            {
                return false;
            }
            return X == other.X && Y == other.Y &&
                Map.Equals(other.Map) && Layer == other.Layer;
        }
    }
}
