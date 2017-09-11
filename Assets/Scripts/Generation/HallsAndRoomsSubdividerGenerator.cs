namespace DLS.LD39.Generation
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Map;
    using Subdivider;
    using UnityEngine;
    using System.Linq;
    using Utility;

    [RequireComponent(typeof(TileMap))]
    public class HallsAndRoomsSubdividerGenerator : MonoBehaviour
    {
        public TileSetData TileSet;

        [Header("Recursion Parameters")]
        [Range(0, 100)]
        public int SplitChance = 100;
        public int MinRecursionDepth = 2;
        public int MaxRecursionDepth = 8;

        [Header("Map Size Parameters")]
        public int MapWidth = 40;
        public int MapHeight = 40;

        [Header("Room Dimensions")]
        public int MinRoomSize = 2;
        public int MaxRoomSideSize = 6;

        private TileMap _map;
        private RectNode _rootNode;
        private SubdividedMap _roomMap;
        private Room _entryRoom;
        private Room _exitRoom;
        private List<Room> _roomsInPath = new List<Room>();

        public void ResetMap()
        {
            GenerateTileMap();
            SplitRootNode();
            _roomMap = ConnectionBuilder.GetMap(_rootNode, MinRoomSize, MaxRoomSideSize);
            IdentityAndTagRooms();
            BuildConnections();
        }

        [UsedImplicitly]
        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        [UsedImplicitly]
        private void Start()
        {
            ResetMap();
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetMap();
            }
        }

        private void SplitRootNode()
        {
            var chance = SplitChance / 100.0f;
            _rootNode = new RectNode(0, 0, MapWidth, MapHeight, null);
            RecursiveRectSplitter.SplitAlternating(_rootNode, MinRoomSize + 2, MaxRecursionDepth, RectNode.CutDirection.Horizontal, chance, MinRecursionDepth);
        }

        private void GenerateTileMap()
        {
            _map.Width = MapWidth;
            _map.Height = MapHeight;
            _map.RebuildMap();
        }

        private void IdentityAndTagRooms()
        {
            // Find the room that's closest to the lower left and upper right
            var entryRoom = _roomMap.Rooms.First();
            var exitRoom = _roomMap.Rooms.First();
            var lowerLeft = new IntVector2(0, 0);
            var upperRight = new IntVector2(MapWidth, MapHeight);
            var closestDistToLowerLeft = IntVector2.Distance(lowerLeft, upperRight);
            var closestDistToUpperRight = IntVector2.Distance(lowerLeft, upperRight);
            foreach (var room in _roomMap.Rooms)
            {
                var roomDistToLowerLeft = IntVector2.Distance(room.Position, lowerLeft);
                var roomDistToUpperRight = IntVector2.Distance(room.Position, upperRight);
                if (roomDistToLowerLeft < closestDistToLowerLeft)
                {
                    closestDistToLowerLeft = roomDistToLowerLeft;
                    entryRoom = room;
                }
                if (roomDistToUpperRight < closestDistToUpperRight)
                {
                    closestDistToUpperRight = roomDistToUpperRight;
                    exitRoom = room;
                }
            }

            _entryRoom = entryRoom;
            _exitRoom = exitRoom;

            entryRoom.AddTag("entry");
            exitRoom.AddTag("exit");
        }

        private void BuildConnections()
        {
            foreach (var room in _roomMap.Rooms)
            {
                var tileType = "default";
                if (room.HasTag("entry"))
                {
                    tileType = "green";
                }
                else if (room.HasTag("exit"))
                {
                    tileType = "red";
                }
                room.SetTiles(_map, tileType);
            }
            foreach (var connection in _roomMap.Connections.Distinct())
            {
                BuildCorridor(connection);
            }
        }

        private void BuildCorridor(MapConnection connect)
        {
            Debug.LogFormat("Building corridor between {0} and {1}", connect.LocationA, connect.LocationB);
            var corridor = new SingleWidthCorridor();
            corridor.AddNode(connect.AConnect);
            corridor.AddNode(connect.BConnect);
            corridor.SetTiles(_map, "hazard");
        }
    }
}