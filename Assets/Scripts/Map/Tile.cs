namespace DLS.LD39.Map
{
    using Props;
    using Units;
    using Utility;
    using Priority_Queue;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class Tile : FastPriorityQueueNode, IEquatable<Tile>
    {
        private Dictionary<TileEdge, Tile> _adjacentTiles = new Dictionary<TileEdge, Tile>();
        private Dictionary<PropLayer, Prop> _props = new Dictionary<PropLayer, Prop>();
        private bool _isPassable;
        private GameUnit _unit;

        public Tile(int x, int y, TileMap map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            if (!map.TileCoordsValid(x, y))
            {
                throw new Exception("tile x, y invalid");
            }

            X = x;
            Y = y;
            TileCoords = new Vector2(x, y);
            Map = map;
            LocalCoords = map.GetLocalCoords(x, y);
            Passable = true;
        }

        public TileType Type
        {
            get; private set;
        }

        public TileMap Map
        {
            get;
            private set;
        }

        public Vector2 TileCoords
        {
            get; private set;
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
                return _isPassable 
                    && _props.Values.All(p => p.Data.Passable) 
                    && _unit == null;
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

        public IEnumerable<Tile> AdjacentTiles
        {
            get
            {
                return _adjacentTiles.Values;
            }
        }

        public GameUnit Unit
        {
            get
            {
                return _unit;
            }
        }

        public void SetUnit(GameUnit unit)
        {
            if (!Passable && unit != null)
            {
                Debug.LogErrorFormat("Trying to add unit to impassable tile {0}", this);
                return;
            }
            _unit = unit;
        }

        public static int GetDistance(Tile a, Tile b)
        {
            return Mathf.CeilToInt(Vector2.Distance(a.TileCoords, b.TileCoords));
        }

        public Prop PropOnLayer(PropLayer layer)
        {
            Prop propOnLayer;
            _props.TryGetValue(layer, out propOnLayer);
            return propOnLayer;
        }

        public void RemoveProp(PropLayer layer)
        {
            _props.Remove(layer);
        }

        public void AddProp(Prop prop)
        {
            _props[prop.Data.Layer] = prop;
        }

        public void SetType(TileType tileType)
        {
            Type = tileType;
            Passable = tileType.Passable;
        }

        public int GetMoveCost(Tile target)
        {
            if (IsOrthogonallyAdjacent(target))
            {
                return 10;
            }
            if (IsDiagonallyAdjacent(target))
            {
                return 14;
            }

            throw new ArgumentException(String.Format("GetTileCost: {0} is not adjacent to {1}", this, target));
        }

        public bool IsOrthogonallyAdjacent(Tile target)
        {
            return IsAdjacent(target) && 
                (target.X == X || target.Y == Y);
        }

        public bool IsDiagonallyAdjacent(Tile target)
        {
            return IsAdjacent(target) && 
                (target.X != X && target.Y != Y);
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
            return SimpleHashBuilder.GetHash(X, Y, Map);
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
                Map.Equals(other.Map);
        }
    }
}
