namespace DLS.LD39.Generation.Subdivider
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Utility;

    public static class ConnectionBuilder
    {
        private class DivisionConnection
        {
            public DivisionConnection(RectNode a, RectNode b)
            {
                A = a;
                B = b;
            }

            public RectNode A;
            public RectNode B;
        }

        public static SubdividedMap GetMap(RectNode root, int minRoomSize, int maxRoomSide)
        {
            var connections = new List<Connection>();

            var roomMap = new Dictionary<RectNode, Room>();
            var divConnections = new List<DivisionConnection>();
            var nodesToCheck = new Queue<RectNode>();
            nodesToCheck.Enqueue(root);

            while (nodesToCheck.Any())
            {
                var nextNode = nodesToCheck.Dequeue();

                if (!nextNode.IsLeaf)
                {
                    var nodeA = GetRandomRoom(nextNode.Left);
                    var nodeB = GetRandomRoom(nextNode.Right);
                    divConnections.Add(new DivisionConnection(nodeA, nodeB));
                }
                else
                {
                    roomMap.Add(nextNode, BuildRoom(nextNode, minRoomSize, maxRoomSide));
                }

                if (nextNode.Left != null)
                {
                    nodesToCheck.Enqueue(nextNode.Left);
                }
                if (nextNode.Right != null)
                {
                    nodesToCheck.Enqueue(nextNode.Right);
                }
            }

            foreach (var divConnection in divConnections)
            {
                connections.AddRange(GetConnections(divConnection, roomMap));
            }

            return new SubdividedMap(root, connections, roomMap.Values);
        }

        private static IEnumerable<Connection> GetConnections(DivisionConnection divConnection,
            Dictionary<RectNode, Room> roomMap)
        {
            var roomA = roomMap[divConnection.A];
            var roomB = roomMap[divConnection.B];

            var connections = new List<Connection>();
            var startRoom = roomA;
            var currentRoom = roomA;
            var targetRoom = roomB;
            var currentTile = startRoom.Rect.Center;
            var targetTile = targetRoom.Rect.Center;

            while (true)
            {
                var tileRoom = PointInRoom(currentTile, roomMap.Values);
                if (targetRoom.Equals(tileRoom))
                {
                    connections.Add(new Connection(currentRoom, targetRoom));
                    currentRoom.AddNeighbor(targetRoom);
                    targetRoom.AddNeighbor(currentRoom);
                    break;
                }
                if (tileRoom != null && !currentRoom.Equals(tileRoom))
                {
                    connections.Add(new Connection(currentRoom, tileRoom));
                    currentRoom.AddNeighbor(tileRoom);
                    tileRoom.AddNeighbor(currentRoom);
                    currentRoom = tileRoom;
                }

                currentTile = GetNextTile(currentTile, targetTile);
            }

            return connections;
        }

        private static Room PointInRoom(IntVector2 tile, IEnumerable<Room> rooms)
        {
            return rooms.FirstOrDefault(room => room.Rect.Contains(tile));
        }

        private static IntVector2 GetNextTile(IntVector2 currentTile, IntVector2 target)
        {
            IntVector2 nextTile;
            if (target.X > currentTile.X)
            {
                nextTile = new IntVector2(currentTile.X + 1, currentTile.Y);
            }
            else if (target.X < currentTile.X)
            {
                nextTile = new IntVector2(currentTile.X - 1, currentTile.Y);
            }
            else if (target.Y > currentTile.Y)
            {
                nextTile = new IntVector2(currentTile.X, currentTile.Y + 1);
            }
            else
            {
                nextTile = new IntVector2(currentTile.X, currentTile.Y - 1);
            }

            return nextTile;
        }

        private static Room BuildRoom(RectNode node, int minRoomSize, int maxRoomSide)
        {
            var maxX = Mathf.Min(maxRoomSide, node.Rect.Width - 2);
            var maxY = Mathf.Min(maxRoomSide, node.Rect.Height - 2);

            var size = new IntVector2(
                Random.Range(minRoomSize, maxX),
                Random.Range(minRoomSize, maxY));

            var pos = new IntVector2(
                node.Rect.Center.X - size.X / 2,
                node.Rect.Center.Y - size.Y / 2);

            var roomRect = new IntRect(pos, size.X, size.Y);
            return new Room(roomRect, node);
        }

        private static RectNode GetRandomRoom(RectNode node)
        {
            while (true)
            {
                if (node.IsLeaf) return node;
                node = Random.Range(0.0f, 1.0f) < 0.5f ? node.Left : node.Right;
            }
        }
    }
}
