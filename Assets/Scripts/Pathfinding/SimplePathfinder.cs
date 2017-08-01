namespace DLS.LD39.Pathfinding
{
    using DLS.LD39.Map;
    using System.Collections.Generic;
    using UnityEngine;

    class SimplePathfinder
    {
        public List<Node> GetPath(Tile start, Tile target)
        {
            var path = new List<Node>();



            return path;
        }

        private int GetGValue(Node parent, Tile candidate)
        {
            var parentTile = parent.NodeTile;
            return parent.G + (parentTile.X == candidate.X || parentTile.Y == candidate.Y
                ? 10
                : 14);
        }

        private int GetHValue(Tile candidate, Tile target)
        {
            var xDis = Mathf.Abs(target.X - candidate.X);
            var yDis = Mathf.Abs(target.Y - candidate.Y);
            return (xDis + yDis) * 10;
        }
    }
}
