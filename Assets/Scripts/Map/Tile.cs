namespace DLS.LD39.Map
{
    using DLS.LD39.Props;
    using DLS.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class Tile : IEquatable<Tile>
    {
        private Dictionary<TileEdge, Tile> _adjacentTiles = new Dictionary<TileEdge, Tile>();
        private Dictionary<PropLayer, Prop> _props = new Dictionary<PropLayer, Prop>();
        private bool _isPassable;

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
            get
            {
                return _isPassable && _props.Values.All(p => p.Data.Passable);
            }
            set
            {
                _isPassable = value;
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
            get
            {
                return _adjacentTiles.Values;
            }
        }

        public Prop PropOnLayer(PropLayer layer)
        {
            if (_props.ContainsKey(layer))
            {
                return _props[layer];
            }
            else
            {
                return null;
            }
        }

        public void RemoveProp(PropLayer layer)
        {
            _props.Remove(layer);
        }

        public void AddProp(Prop prop)
        {
            _props[prop.Data.Layer] = prop;
        }

        public int GetMoveCost(Tile target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (!IsAdjacent(target))
            {
                throw new ArgumentException(String.Format("GetTileCost: {0} is not adjacent to {1}", this, target));
            }

            return (target.X == X || target.Y == Y)
                ? 10
                : 14;
        }

        public bool IsEnterable()
        {
            return Passable && ActiveUnits.Instance.GetUnitAtTile(this) == null;
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
