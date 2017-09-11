namespace DLS.LD39.Generation.Subdivider
{
    using System.Collections.Generic;
    using System.Linq;
    using Utility;

    public abstract class RoomOld
    {
        private readonly HashSet<Room> _neighbors = new HashSet<Room>();
        private readonly HashSet<IntVector2> _tiles = new HashSet<IntVector2>();
        private readonly HashSet<IntVector2> _hallConnectors = new HashSet<IntVector2>();

        protected RoomOld(RectNode roomNode)
        {
            RoomNode = roomNode;

            if (RoomNode.Parent == null)
            {
                return;
            }

            SiblingNode = Equals(RoomNode.Parent.Left, RoomNode) 
                ? roomNode.Parent.Right 
                : roomNode.Parent.Left;
        }

        public IEnumerable<IntVector2> Tiles { get { return _tiles; } }

        public RectNode SiblingNode { get; private set; }

        public RectNode RoomNode { get; private set; }

        public abstract IntVector2 Center { get; }

        public abstract bool ContainsTile(IntVector2 position);

        public IEnumerable<Room> Neighbors
        {
            get { return _neighbors; }
        }

        public IEnumerable<IntVector2> Connectors
        {
            get { return _hallConnectors; }
        }

        public void AddNeighbor(Room neighbor)
        {
            _neighbors.Add(neighbor);
        }

        public static void GetClosestPair(Room a, Room b, out IntVector2 aConnect, out IntVector2 bConnect)
        {
            var minDistance = int.MaxValue;
            aConnect = a.Connectors.First();
            bConnect = b.Connectors.First();
            foreach (var connectorA in a.Connectors)
            {
                foreach (var connectorB in b.Connectors)
                {
                    var distance = IntVector2.ManhattanDistance(connectorA, connectorB);
                    if (distance >= minDistance) continue;
                    aConnect = connectorA;
                    bConnect = connectorB;
                    minDistance = distance;
                }
            }
        }

        public IntVector2 GetClosestConnector(IntVector2 target)
        {
            if (!_hallConnectors.Any())
            {
                throw new System.Exception("No connectors");
            }

            var min = int.MaxValue;
            IntVector2 closest = new IntVector2(0, 0);
            foreach (var connector in _hallConnectors)
            {
                var distance = IntVector2.ManhattanDistance(connector, target);
                if (distance >= min) continue;
                min = distance;
                closest = connector;
            }
            return closest;
        }

        protected bool AddTile(IntVector2 tile)
        {
            return _tiles.Add(tile);
        }

        protected bool AddConnector(IntVector2 newConnector)
        {
            return _hallConnectors.Add(newConnector);
        }
    }
}