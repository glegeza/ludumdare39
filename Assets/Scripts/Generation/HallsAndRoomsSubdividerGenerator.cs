namespace DLS.LD39.Generation
{
    using System;
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
        public int Width = 40;
        public int Height = 40;
        public int MaxDepth = 8;
        public int MinRoomSize = 2;
        public int MaxRoomSideSize = 6;
        public TileSetData TileSet;

        private TileMap _map;
        private RectNode _rootNode;

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
            BuildRooms();
            BuildCorridors();
        }

        private void SplitRootNode()
        {
            _rootNode = new RectNode(0, 0, Width, Height, null);
            RecursiveRectSplitter.SplitAlternating(_rootNode, MinRoomSize + 2, MaxDepth, RectNode.CutDirection.Horizontal);
        }

        private void GenerateTileMap()
        {
            _map.Width = Width;
            _map.Height = Height;
            _map.RebuildMap();
        }

        private void BuildCorridors()
        {
            var nodesToCheck = new Queue<RectNode>();
            nodesToCheck.Enqueue(_rootNode);

            while (nodesToCheck.Any())
            {
                var nextNode = nodesToCheck.Dequeue();

                if (!nextNode.IsLeaf)
                {
                    var nodeA = GetRandomRoom(nextNode.Left);
                    var nodeB = GetRandomRoom(nextNode.Right);
                    BuildCorridor(nodeA, nodeB);
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
        }

        private void BuildCorridor(RectNode nodeA, RectNode nodeB)
        {
            var corridor = new SingleWidthCorridor();
            corridor.AddNode(nodeA.Rect.Center);
            corridor.AddNode(nodeB.Rect.Center);
            corridor.SetTiles(_map, "default");
        }

        private void BuildCorridor(RectNode node)
        {
            var corridor = new SingleWidthCorridor();
            corridor.AddNode(node.Left.Rect.Center);
            corridor.AddNode(node.Right.Rect.Center);
            corridor.SetTiles(_map, "default");
        }

        private void BuildRooms()
        {
            foreach (var leaf in _rootNode.LeafNodes())
            {
                CreateRoom(leaf);
            }
        }

        private RectNode GetRandomRoom(RectNode node)
        {
            return node.IsLeaf 
                ? node 
                : GetRandomRoom(Random.Range(0.0f, 1.0f) < 0.5f ? node.Left : node.Right);
        }

        private void CreateRoom(RectNode node)
        {
            var size = new IntVector2(
                Random.Range(MinRoomSize, node.Rect.Width - 2),
                Random.Range(MinRoomSize, node.Rect.Height - 2));

            var pos = new IntVector2(
                node.Rect.Center.X - size.X / 2,
                node.Rect.Center.Y - size.Y / 2);

            for (var x = 0; x < size.X; x++)
            {
                for (var y = 0; y < size.Y; y++)
                {
                    var posX = pos.X + x;
                    var posY = pos.Y + y;
                    _map.SetTileAt(posX, posY, "default");
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