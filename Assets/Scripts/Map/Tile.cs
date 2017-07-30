namespace DLS.LD39.Map
{
    using DLS.Utility;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tile : IEquatable<Tile>
    {
        private HashSet<Tile> _adjacentTiles = new HashSet<Tile>();

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
            get;
            private set;
        }

        public void AddAdjacentTile(Tile tile)
        {
            if (_adjacentTiles.Count == 8)
            {
                throw new Exception("Trying to add 9th adjacent tile!");
            }
            _adjacentTiles.Add(tile);
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

        public bool Equals(Tile other)
        {
            return X == other.X && Y == other.Y &&
                Map.Equals(other.Map) && Layer == other.Layer;
        }
    }
}
