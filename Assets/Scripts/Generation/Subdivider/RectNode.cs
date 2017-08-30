namespace DLS.LD39.Generation.Subdivider
{
    using JetBrains.Annotations;
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

        public bool Split(CutDirection direction, int minSize)
        {
            var maxSize = GetMaximum(direction, minSize);
            if (!CanSplit(minSize, maxSize))
            {
                return false;
            }
            
            var splitPosition = GetSplitPosition(GetMaximum(direction, minSize), minSize);
            CreateLeaves(direction, splitPosition);
            return true;
        }

        private bool CanSplit(int minSize, int maxSplitSize)
        {
            return Left != null && maxSplitSize >= minSize;
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
