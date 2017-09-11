namespace DLS.LD39.Generation
{
    using System;
    using Map;
    using Utility;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public abstract class MapElement : IEquatable<MapElement>
    {
        private readonly HashSet<IntVector2> _connectors = new HashSet<IntVector2>();
        private readonly HashSet<MapConnection> _connections = new HashSet<MapConnection>();
        private readonly HashSet<string> _tags = new HashSet<string>();

        protected MapElement(string id)
        {
            ID = id;
        }

        public string ID { get; private set; }

        public IEnumerable<MapConnection> Connections
        {
            get { return _connections; }
        }

        public IEnumerable<IntVector2> Connectors
        {
            get { return _connectors; }
        }

        public IEnumerable<string> Tags
        {
            get { return _tags; }
        }

        public abstract IEnumerable<IntVector2> Tiles
        {
            get;
        }

        public static void GetClosestConnectionPair(MapElement a, MapElement b, out IntVector2 aConnect,
            out IntVector2 bConnect)
        {
            if (!a._connectors.Any() || !b._connectors.Any())
            {
                Debug.LogError("Whu?");
                throw new Exception();
            }
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

        public abstract bool TileInElement(int x, int y);

        public bool TileInElement(IntVector2 tilePos)
        {
            return TileInElement(tilePos.X, tilePos.Y);
        }

        public bool TileInElement(Tile tile)
        {
            return TileInElement(tile.X, tile.Y);
        }

        public void AddTag(string tag)
        {
            _tags.Add(tag.ToLower());
        }

        public bool HasTag(string tag)
        {
            var lower = tag.ToLower();
            return _tags.Contains(lower);
        }

        public void AddConnector(IntVector2 connector)
        {
            _connectors.Add(connector);
        }

        public bool AddConnection(IntVector2 connector, MapElement other, IntVector2 otherConnector)
        {
            if (!_connectors.Contains(connector) || !other._connectors.Contains(otherConnector))
            {
                return false;
            }

            var connection = new MapConnection(this, connector, other, otherConnector);
            _connections.Add(connection);
            other._connections.Add(connection);
            return true;
        }

        public bool Equals(MapElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ID, other.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MapElement) obj);
        }

        public override int GetHashCode()
        {
            return (ID != null ? ID.GetHashCode() : 0);
        }
    }
}
