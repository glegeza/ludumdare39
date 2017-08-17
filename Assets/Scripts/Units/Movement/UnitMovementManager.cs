namespace DLS.LD39.Units.Movement
{
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Priority_Queue;

    public class UnitMovementHelper
    {
        private SimplePathfinder _pathFinder = new SimplePathfinder();

        public delegate void ReachableTileCallback(HashSet<Tile> t);

        public int GetMaxMovementAlongPath(GameUnit unit, IEnumerable<Tile> path)
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

        public int CostOfPath(Tile start, IEnumerable<Tile> path)
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

        public IEnumerator GetReachableTilesFast(Tile start, int maxAP, ReachableTileCallback cb)
        {
            var frontier = new FastPriorityQueue<Tile>(250);
            var cost = new Dictionary<Tile, int>();
            var valid = new HashSet<Tile>();

            frontier.Enqueue(start, 0);
            valid.Add(start);
            cost.Add(start, 0);
            var passes = 0;

            while (frontier.Any())
            {
                var current = frontier.Dequeue();

                foreach (var next in current.AdjacentTiles)
                {
                    if (!next.IsEnterable())
                    {
                        continue;
                    }
                    var newCost = cost[current] + current.GetMoveCost(next);
                    if (!cost.ContainsKey(next) && newCost <= maxAP)
                    {
                        cost[next] = newCost;
                        frontier.Enqueue(next, newCost);
                        valid.Add(next);
                    }
                }

                passes += 1;
                if (passes > 50)
                {
                    passes = 0;
                    yield return null;
                }
            }
            
            cb(valid);
        }

        public IEnumerator GetReachableTiles(Tile start, int maxAP, ReachableTileCallback cb)
        {
            var seen = new HashSet<Tile>();
            var reachable = new HashSet<Tile>();
            var frontier = new Queue<Tile>();

            seen.Add(start);
            reachable.Add(start);
            foreach (var adj in start.AdjacentTiles)
            {
                seen.Add(adj);
                frontier.Enqueue(adj);
            }

            while (frontier.Any())
            {
                var next = frontier.Dequeue();
                int cost;
                var path = _pathFinder.GetPath(start, next, out cost);
                if (!path.Any())
                {
                    continue; // no path, so don't check adjacent tiles either
                }
                if (cost <= maxAP)
                {
                    reachable.Add(next);
                    foreach (var adj in next.AdjacentTiles.Where(a => !seen.Contains(a)))
                    {
                        seen.Add(adj);
                        if (cost + next.GetMoveCost(adj) <= maxAP)
                        {
                            frontier.Enqueue(adj);
                        }
                    }
                }
                yield return null;
            }

            cb(reachable);
        }
    }
}
