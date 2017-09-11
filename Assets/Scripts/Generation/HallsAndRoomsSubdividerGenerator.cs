namespace DLS.LD39.Generation
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Map;
    using Subdivider;
    using UnityEngine;
    using System.Linq;
    using Utility;
    using Random = UnityEngine.Random;

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

        [UsedImplicitly]
        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        [UsedImplicitly]
        private void Start()
        {
            GenerateTileMap();
            SplitRootNode();
            _roomMap = ConnectionBuilder.GetMap(_rootNode, MinRoomSize, MaxRoomSideSize);
            BuildConnections();
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

        private void BuildCorridor(Connection connect)
        {
            Debug.LogFormat("Building corridor between {0} and {1}", connect.RoomA, connect.RoomB);
            var corridor = new SingleWidthCorridor();
            corridor.AddNode(connect.AConnect);
            corridor.AddNode(connect.BConnect);
            corridor.SetTiles(_map, "default");
        }

        private void SetTiles(IEnumerable<IntVector2> tiles)
        {
            foreach (var tile in tiles)
            {
                _map.SetTileAt(tile.X, tile.Y, "default");
            }
        }

        private void SetTiles(IntRect rect)
        {
            for (var x = rect.TopLeft.X; x < rect.TopRight.X; x++)
            {
                for (var y = rect.BottomLeft.Y; y < rect.TopRight.Y; y++)
                {
                    _map.SetTileAt(x, y, "default");
                }
            }
        }

        private void ColorNodes()
        {
            var nodesToColor = new Queue<RectNode>();
            nodesToColor.Enqueue(_rootNode);
            while (nodesToColor.Any())
            {
                var next = nodesToColor.Dequeue();
                var id = GetRandomTile();
                next.SetTiles(_map, id);

                if (next.Left == null) continue;
                nodesToColor.Enqueue(next.Left);
                nodesToColor.Enqueue(next.Right);
            }
        }

        private string GetRandomTile()
        {
            var tileIDs = TileSet.TileTypes.Select(t => t.ID).ToList();
            return tileIDs[Random.Range(0, tileIDs.Count)];
        }
    }
}