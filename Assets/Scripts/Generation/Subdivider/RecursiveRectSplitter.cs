namespace DLS.LD39.Generation.Subdivider
{
    public static class RecursiveRectSplitter
    {
        public static void SplitRandom(RectNode root, int minSize, int maxDepth)
        {
            var firstDirection = GetNextDirection(RectNode.CutDirection.Horizontal, false);
            RecursivelySplitNode(root, minSize, maxDepth, firstDirection, false);
        }

        public static void SplitAlternating(RectNode root, int minSize, int maxDepth, 
            RectNode.CutDirection startDir)
        {
            RecursivelySplitNode(root, minSize, maxDepth, startDir, true);
        }

        private static void RecursivelySplitNode(RectNode node, int minSize, int maxDepth,
            RectNode.CutDirection direction, bool alternate)
        {
            if (!node.Split(direction, minSize, maxDepth))
            {
                return;
            }

            var nextDirection = GetNextDirection(direction, alternate);

            RecursivelySplitNode(node.Left, minSize, maxDepth, nextDirection, alternate);
            RecursivelySplitNode(node.Right, minSize, maxDepth, nextDirection, alternate);
        }

        private static RectNode.CutDirection GetNextDirection(RectNode.CutDirection lastCut, bool alternate)
        {
            if (!alternate)
            {
                return UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f
                    ? RectNode.CutDirection.Horizontal
                    : RectNode.CutDirection.Vertical;
            }

            return lastCut == RectNode.CutDirection.Horizontal
                ? RectNode.CutDirection.Vertical
                : RectNode.CutDirection.Horizontal;
        }
    }
}
