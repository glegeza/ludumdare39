namespace DLS.LD39.Generation
{
    using JetBrains.Annotations;
    using Map;
    using Subdivider;
    using UnityEngine;
    using System.Linq;

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

        public void ResetMap()
        {
            GenerateTileMap();
            SplitRootNode();
            _roomMap = ConnectionBuilder.GetMap(_rootNode, MinRoomSize, MaxRoomSideSize);
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

        private void BuildConnections()
        {
            foreach (var connection in _roomMap.Connections.Distinct())
            {
                BuildCorridor(connection);
            }
            foreach (var room in _roomMap.Rooms)
            {
                room.SetTiles(_map, "default");
            }
        }

        private void BuildCorridor(MapConnection connect)
        {
            Debug.LogFormat("Building corridor between {0} and {1}", connect.LocationA, connect.LocationB);
            var corridor = new SingleWidthCorridor();
            corridor.AddNode(connect.AConnect);
            corridor.AddNode(connect.BConnect);
            corridor.SetTiles(_map, "default");
        }
    }
}