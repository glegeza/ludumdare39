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
            Debug.LogFormat("Attempting to split node with depth {0}", node.Depth);
            var roll = Random.Range(0.0f, 1.0f);
            Debug.LogFormat("Split check roll: {0}", roll);
            if (node.Depth > requiredLevels && roll >= splitChance)
            {
                Debug.LogFormat("Failed split roll");
                return;
            }
            if (!node.Split(direction, minSize, maxDepth))
            {
                Debug.LogFormat("Too small to split");
                return;
            }

            var nextDirection = GetNextDirection(direction, alternate);
            Debug.LogFormat("Splitting {0}", nextDirection);

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
