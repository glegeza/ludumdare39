namespace DLS.LD39.Generation
{
    using DLS.Utility;
    using System.Collections.Generic;

    public abstract class MapElement
    {
        private HashSet<MapConnection> _mapConnections = new HashSet<MapConnection>();

        public IEnumerable<MapConnection> Connections
        {
            get
            {
                return _mapConnections;
            }
        }

        /// <summary>
        /// Adds a new connection to this element's connection list.
        /// </summary>
        /// <param name="other">The map element being connected to.</param>
        /// <param name="thisConnection">The tile on this element that connects to the other element.</param>
        /// <param name="otherConnection">The tile on the other element that connects to this element.</param>
        public void AddConnection(MapElement other, IntVector2 thisConnection, IntVector2 otherConnection)
        {
            var connection = new MapConnection(this, other, thisConnection, otherConnection);
            if (_mapConnections.Contains(connection))
            {
                return;
            }

            _mapConnections.Add(connection);
            other._mapConnections.Add(connection);
        }
    }
}
