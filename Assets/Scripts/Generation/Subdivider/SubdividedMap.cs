namespace DLS.LD39.Generation.Subdivider
{
    using System.Collections.Generic;

    public class SubdividedMap
    {
        public SubdividedMap(RectNode root,
            IEnumerable<Connection> connections,
            IEnumerable<Room> rooms)
        {
            Connections = new List<Connection>(connections);
            Rooms = new List<Room>(rooms);
            Root = root;
        }

        public RectNode Root
        {
            get; private set;
        }

        public IEnumerable<Connection> Connections
        {
            get; private set;
        }

        public IEnumerable<Room> Rooms
        {
            get; private set;
        }
    }
}
