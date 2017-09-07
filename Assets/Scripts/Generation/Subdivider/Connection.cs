namespace DLS.LD39.Generation.Subdivider
{
    using System;

    public class Connection : IEquatable<Connection>
    {
        public Connection(Room a, Room b)
        {
            RoomA = a;
            RoomB = b;
        }

        public Room RoomA
        {
            get; private set;
        }

        public Room RoomB
        {
            get; private set;
        }

        public bool Equals(Connection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Equals(RoomA, other.RoomA) && Equals(RoomB, other.RoomB)) ||
                (Equals(RoomA, other.RoomB) && Equals(RoomB, other.RoomA));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Connection) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((RoomA != null ? RoomA.GetHashCode() : 0) * 397) ^ (RoomB != null ? RoomB.GetHashCode() : 0);
            }
        }
    }
}
