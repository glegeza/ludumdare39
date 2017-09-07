namespace DLS.LD39.Generation.Subdivider
{
    using System.Collections.Generic;
    using Utility;

    public class Room
    {
        private HashSet<Room> _neighbors = new HashSet<Room>();

        public Room(IntRect roomRect, RectNode roomNode)
        {
            Rect = roomRect;
            RoomNode = roomNode;

            if (RoomNode.Parent == null)
            {
                return;
            }

            SiblingNode = RoomNode.Parent.Left == RoomNode 
                ? roomNode.Parent.Right 
                : roomNode.Parent.Left;
        }

        public RectNode SiblingNode { get; private set; }

        public RectNode RoomNode { get; private set; }

        public IntRect Rect { get; private set; }

        public IEnumerable<Room> Neighbors
        {
            get { return _neighbors; }
        }

        public void AddNeighbor(Room neighbor)
        {
            _neighbors.Add(neighbor);
        }
    }
}