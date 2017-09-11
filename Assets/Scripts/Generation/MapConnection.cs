namespace DLS.LD39.Generation
{
    using Utility;

    public class MapConnection
    {
        public MapConnection(MapElement a, IntVector2 aConnect, MapElement b, IntVector2 bConnect)
        {
            LocationA = a;
            AConnect = aConnect;
            LocationB = b;
            BConnect = bConnect;
        }

        public MapElement LocationA { get; private set; }

        public IntVector2 AConnect { get; private set; }

        public MapElement LocationB { get; private set; }

        public IntVector2 BConnect { get; private set; }

        public bool Equals(MapConnection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Equals(LocationA, other.LocationA) && Equals(LocationB, other.LocationB)) ||
                   (Equals(LocationA, other.LocationB) && Equals(LocationB, other.LocationA));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MapConnection)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((LocationA != null ? LocationA.GetHashCode() : 0) * 397) ^ (LocationB != null ? LocationB.GetHashCode() : 0);
            }
        }
}
}
