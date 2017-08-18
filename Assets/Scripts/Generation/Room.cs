namespace DLS.LD39.Generation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
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
