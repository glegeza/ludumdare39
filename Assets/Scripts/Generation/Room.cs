namespace DLS.LD39.Generation
{
    using DLS.LD39.Map;
    using System;
    using Utility;

    public class Room : MapElement, IEquatable<Room>
    {
        public Room(int x, int y, int width, int height)
        {
            MapRect = new IntRect(x, y, width, height);
        }

        public IntRect MapRect
        {
            get; private set;
        }

        public void SetTiles(TileMap map)
        {
            for (var x = 0; x < MapRect.Width; x++)
            {
                for (var y = 0; y < MapRect.Height; y++)
                {
                    map.SetTileAt(x + MapRect.X, y + MapRect.Y, "default");
                }
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
    }
}
