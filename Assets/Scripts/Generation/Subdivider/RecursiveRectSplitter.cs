namespace DLS.LD39.Generation.Subdivider
{
    using UnityEngine;

    public static class RecursiveRectSplitter
    {
        public static void SplitRandom(RectNode root, int minSize, int maxDepth, float splitChance=1.0f,
            int requiredLevels=2)
        {
            var firstDirection = GetNextDirection(RectNode.CutDirection.Horizontal, false);
            RecursivelySplitNode(root, minSize, maxDepth, firstDirection, false, splitChance,
                requiredLevels);
        }

        public static void SplitAlternating(RectNode root, int minSize, int maxDepth, 
            RectNode.CutDirection startDir, float splitChance=1.0f, int requiredLevels=2)
        {
            RecursivelySplitNode(root, minSize, maxDepth, startDir, true, splitChance,
                requiredLevels);
        }

        private static void RecursivelySplitNode(RectNode node, int minSize, int maxDepth,
            RectNode.CutDirection direction, bool alternate, float splitChance=1.0f,
            int requiredLevels=2)
        {
            if (node.Depth > requiredLevels && Random.Range(0.0f, 1.0f) <= splitChance)
            {
                return;
            }
            if (!node.Split(direction, minSize, maxDepth))
            {
                return;
            }

            var nextDirection = GetNextDirection(direction, alternate);

            RecursivelySplitNode(node.Left, minSize, maxDepth, nextDirection, alternate, splitChance, requiredLevels);
            RecursivelySplitNode(node.Right, minSize, maxDepth, nextDirection, alternate, splitChance, requiredLevels);
        }

        private static RectNode.CutDirection GetNextDirection(RectNode.CutDirection lastCut, bool alternate)
        {
            if (!alternate)
            {
                return Random.Range(0.0f, 1.0f) < 0.5f
                    ? RectNode.CutDirection.Horizontal
                    : RectNode.CutDirection.Vertical;
            }

            return lastCut == RectNode.CutDirection.Horizontal
                ? RectNode.CutDirection.Vertical
                : RectNode.CutDirection.Horizontal;
        }
    }
}
