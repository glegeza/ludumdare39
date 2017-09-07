namespace DLS.LD39.Generation.Subdivider
{
    using System.Collections.Generic;
    using System.Linq;
    using Utility;

    public class Corridor
    {
        private readonly HashSet<IntVector2> _tiles = new HashSet<IntVector2>();

        public Corridor(RectNode parentNode)
        {
            NodeA = parentNode.Left;
            NodeB = parentNode.Right;
            var nodeQueue = new Queue<IntVector2>();
            nodeQueue.Enqueue(NodeA.Rect.Center);
            nodeQueue.Enqueue(NodeB.Rect.Center);
            GenerateTiles(nodeQueue);
        }

        public RectNode NodeA { get; private set; }

        public RectNode NodeB { get; private set; }

        public IEnumerable<IntVector2> Tiles
        {
            get { return _tiles; }
        }

        private void GenerateTiles(Queue<IntVector2> nodes)
        {
            var currentTile = nodes.Dequeue();
            _tiles.Add(currentTile);

            while (nodes.Any())
            {
                var target = nodes.Dequeue();

                while (!currentTile.Equals(target))
                {
                    currentTile = GetNextTile(currentTile, target);
                    _tiles.Add(currentTile);
                }
            }
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
    }
}
