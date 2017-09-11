namespace DLS.LD39.Generation.Subdivider
{
    using System;
    using Utility;

    public class Connection : IEquatable<Connection>
    {
        public Connection(Room a, IntVector2 aConnect, Room b, IntVector2 bConnect)
        {
            RoomA = a;
            AConnect = aConnect;
            RoomB = b;
            BConnect = bConnect;
        }

        public Room RoomA { get; private set; }

        public IntVector2 AConnect { get; private set; }

        public Room RoomB { get; private set; }

        public IntVector2 BConnect { get; private set; }

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
            return obj.GetType() == GetType() && Equals((Connection) obj);
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
