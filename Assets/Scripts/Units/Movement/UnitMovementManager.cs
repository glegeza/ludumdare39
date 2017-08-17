namespace DLS.LD39.Units.Movement
{
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class UnitMovementHelper
    {
        private static SimplePathfinder _pathFinder = new SimplePathfinder();

        public static int GetMaxMovementAlongPath(GameUnit unit, IEnumerable<Tile> path)
        {
            var curMove = 0;
            var cost = 0;

            var lastPos = unit.Position.CurrentTile;
            foreach (var tile in path)
            {
                cost += lastPos.GetMoveCost(tile);
                if (cost > unit.AP.PointsRemaining)
                {
                    break;
                }
                lastPos = tile;
                curMove++;
            }

            return curMove;
        }

        public static int CostOfPath(Tile start, IEnumerable<Tile> path)
        {
            var curTile = start;
            var totalCost = 0;
            foreach (var step in path)
            {
                totalCost += curTile.GetMoveCost(step);
                curTile = step;
            }

            return totalCost;
        }
    }
}
