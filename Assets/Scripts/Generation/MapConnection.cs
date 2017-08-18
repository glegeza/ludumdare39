namespace DLS.LD39.Generation
{
    using DLS.Utility;
    using System;

    public class MapConnection : IEquatable<MapConnection>
    {
        /// <summary>
        /// Creates a new link between two MapElements
        /// </summary>
        /// <param name="a">The first map element</param>
        /// <param name="b">The second map element</param>
        /// <param name="cA">The location on the first map element connected to cB</param>
        /// <param name="cB">The location on the second map element connected to cA</param>
        public MapConnection(MapElement a, MapElement b, IntVector2 cA, IntVector2 cB)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            A = a;
            B = b;
            ConnectionA = cA;
            ConnectionB = cB;
        }

        public MapElement A
        {
            get; private set;
        }

        public MapElement B
        {
            get; private set;
        }

        public IntVector2 ConnectionA
        {
            get; private set;
        }

        public IntVector2 ConnectionB
        {
            get; private set;
        }

        public override int GetHashCode()
        {
            return SimpleHashBuilder.GetHash(A, B, ConnectionA, ConnectionB);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals(obj as Room);
        }

        public bool Equals(MapConnection other)
        {
            if (other == null)
            {
                return false;
            }

            var sameElements = (A == other.A && B == other.B) ||
                (A == other.B && B == other.A);
            var samePositions = (ConnectionA == other.ConnectionA && ConnectionB == other.ConnectionB) ||
                (ConnectionA == other.ConnectionB && ConnectionB == other.ConnectionA);
            return sameElements && samePositions;
        }
    }
}
