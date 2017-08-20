namespace DLS.LD39.Generation
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Utility;

    public class Room : MapElement, IEquatable<Room>
    {
        private HashSet<IntVector2> _tiles = new HashSet<IntVector2>();

        public Room(int x, int y, int width, int height)
        {
            MapRect = new IntRect(x, y, width, height);
            UpdateTiles();
        }

        public int Width
        {
            get
            {
                return MapRect.Width;
            }
        }

        public int Height
        {
            get
            {
                return MapRect.Height;
            }
        }

        public IntRect MapRect
        {
            get; private set;
        }

        public override IEnumerable<IntVector2> Tiles
        {
            get
            {
                return _tiles;
            }
        }

        public IntVector2 TranslateLocalTileCoords(int x, int y)
        {
            return new IntVector2(x + MapRect.X, y + MapRect.Y);
        }

        public void SetTiles(TileMap map, string tileID="default")
        {
            foreach (var tile in _tiles)
            {
                map.SetTileAt(tile.X, tile.Y, tileID);
            }
        }

        public bool Equals(Room other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(Room other)
        {
            return other != null && MapRect.Intersects(other.MapRect);
        }
        
        public bool Overlaps(Room other, int padding)
        {
            var paddedRect = new IntRect(MapRect.X - padding, MapRect.Y - padding,
                MapRect.Width + padding, MapRect.Height + padding);
            return other != null && paddedRect.Intersects(other.MapRect);
        }

        private void UpdateTiles()
        {
            _tiles.Clear();
            for (var xPos = 0; xPos < MapRect.Width; xPos++)
            {
                for (var yPos = 0; yPos < MapRect.Height; yPos++)
                {
                    _tiles.Add(new IntVector2(xPos + MapRect.X, yPos + MapRect.Y));
                }
            }
        }

        public override bool TileInElement(int x, int y)
        {
            return _tiles.Contains(new IntVector2(x, y));
        }
    }
}
