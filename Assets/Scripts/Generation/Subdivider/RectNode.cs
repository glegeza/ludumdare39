namespace DLS.LD39.Generation.Subdivider
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Map;
    using UnityEngine;
    using Utility;
    using Random = UnityEngine.Random;

    public class RectNode
    {
        public enum CutDirection
        {
            Vertical,
            Horizontal
        }

        public RectNode(int x, int y, int width, int height, RectNode parent)
        {
            Rect = new IntRect(x, y, width, height);
            Parent = parent;
            SetDepth();
        }

        public RectNode([NotNull] IntRect rect, RectNode parent)
        {
            Parent = parent;
            Rect = rect;
            SetDepth();
        }

        public int Depth { get; private set; }

        public IntRect Rect { get; private set; }

        public RectNode Parent { get; private set; }

        public RectNode Left { get; set; }

        public RectNode Right { get; set; }

        public bool IsLeaf
        {
            get
            {
                return Left == null && Right == null;
            }
        }

        public IEnumerable<RectNode> LeafNodes()
        {
            var nodesToCheck = new Queue<RectNode>();
            var leafNodes = new List<RectNode>();
            nodesToCheck.Enqueue(this);
            while (nodesToCheck.Any())
            {
                var node = nodesToCheck.Dequeue();
                if (node.IsLeaf)
                {
                    leafNodes.Add(node);
                    continue;
                }

                if (node.Left != null)
                {
                    nodesToCheck.Enqueue(node.Left);
                }
                if (node.Right != null)
                {
                    nodesToCheck.Enqueue(node.Right);
                }
            }

            return leafNodes;
        }

        public bool Split(CutDirection direction, int minSize, int maxDepth)
        {
            var maxSize = GetMaximum(direction, minSize);
            if (!CanSplit(minSize, maxSize, maxDepth))
            {
                return false;
            }
            
            var splitPosition = GetSplitPosition(GetMaximum(direction, minSize), minSize);
            CreateLeaves(direction, splitPosition);
            return true;
        }

        public IEnumerable<IntVector2> Positions()
        {
            for (var x = Rect.X; x < Rect.BottomRight.X; x++)
            {
                for (var y = Rect.Y; y < Rect.TopLeft.Y; y++)
                {
                    yield return new IntVector2(x, y);
                }
            }
        }

        public IEnumerable<Tile> ValidTiles(TileMap map)
        {
            return Positions().Select(map.GetTile).Where(tile => tile != null);
        }

        public void SetTiles(TileMap map, string tileID)
        {
            foreach (var position in Positions())
            {
                map.SetTileAt(position.X, position.Y, tileID);
            }
        }

        private bool CanSplit(int minSize, int maxSplitSize, int maxDepth)
        {
            return Left == null && maxSplitSize >= minSize &&
                (maxDepth < 0 || maxDepth > Depth);
        }

        private int GetMaximum(CutDirection direction, int minSize)
        {
            return (direction == CutDirection.Horizontal
                ? Rect.Height
                : Rect.Width) - minSize;
        }

        private static int GetSplitPosition(int size, int minSize)
        {
            return Mathf.Max(Random.Range(0, size), minSize);
        }

        private void CreateLeaves(CutDirection direction, int splitPosition)
        {
            if (direction == CutDirection.Horizontal)
            {
                Left = new RectNode(Rect.X, Rect.Y, Rect.Width,
                    splitPosition, this);
                Right = new RectNode(Rect.X, Rect.Y + splitPosition,
                    Rect.Width, Rect.Height - splitPosition, this);
            }
            else
            {
                Left = new RectNode(Rect.X, Rect.Y, splitPosition,
                    Rect.Height, this);
                Right = new RectNode(Rect.X + splitPosition, Rect.Y,
                    Rect.Width - splitPosition, Rect.Height, this);
            }
        }

        private void SetDepth()
        {
            Depth = 0;
            var nextLevel = Parent;
            while (nextLevel != null)
            {
                Depth += 1;
                nextLevel = nextLevel.Parent;
            }
        }
    }
}
