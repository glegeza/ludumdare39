namespace DLS.LD39.Generation
{
    using Map;
    using Units;
    using System;
    using System.Collections.Generic;
    using Data;
    using UnityEngine;
    using Utility;

    public abstract class Room : MapElement, IEquatable<Room>
    {
        private readonly HashSet<IntVector2> _tiles = new HashSet<IntVector2>();
        private readonly HashSet<Room> _neighbors = new HashSet<Room>();

        protected Room(IntVector2 position, string id, RoomType type=null) : base(id)
        {
            Template = type;
            Position = position;
        }

        public RoomType Template { get; private set; }

        public IntVector2 Position { get; private set; }

        public override IEnumerable<IntVector2> Tiles
        {
            get
            {
                return _tiles;
            }
        }

        public bool UnitInRoom(GameUnit unit)
        {
            var map = unit.Position.CurrentTile.Map;
            var unitPos = unit.Position.CurrentTile;
            foreach (var tile in _tiles)
            {
                var roomTile = map.GetTile(tile);
                if (unitPos.Equals(roomTile))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool Overlaps(Room other)
        {
            return other != null && other._tiles.Overlaps(_tiles);
        }

        public override bool TileInElement(int x, int y)
        {
            return _tiles.Contains(new IntVector2(x, y));
        }

        public IntVector2 TransformLocalTileCoords(int lX, int lY)
        {
            return TransformLocalTileCoords(new IntVector2(lX, lY));
        }

        public IntVector2 TransformLocalTileCoords(IntVector2 localCoords)
        {
            return Position + localCoords;
        }

        public void AddNeighbor(Room neighbor)
        {
            _neighbors.Add(neighbor);
        }

        public bool Equals(Room other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || string.Equals(ID, other.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Room) obj);
        }

        public override int GetHashCode()
        {
            return ID != null ? ID.GetHashCode() : 0;
        }

        public void SetTiles(TileMap map, string tileID = "default")
        {
            foreach (var tile in _tiles)
            {
                map.SetTileAt(tile.X, tile.Y, tileID);
            }
        }

        protected void AddTile(IntVector2 tile)
        {
            _tiles.Add(tile);
        }

        protected void AddTiles(IEnumerable<IntVector2> tiles)
        {
            foreach (var tile in tiles)
            {
                _tiles.Add(tile);
            }
        }

        protected void AddTiles(IntRect tiles)
        {
            for (var x = tiles.X; x <= tiles.BottomRight.X; x++)
            {
                for (var y = tiles.Y; y <= tiles.TopRight.Y; y++)
                {
                    _tiles.Add(new IntVector2(x, y));
                }
            }
        }
    }
}
