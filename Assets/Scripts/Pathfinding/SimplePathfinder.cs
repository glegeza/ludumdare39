namespace DLS.LD39.Pathfinding
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Priority_Queue;

    public class SimplePathfinder
    {
        public Queue<Tile> GetPath(Tile start, Tile target)
        {
            if (start == null || target == null)
            {
                throw new ArgumentException("start or target is null");
            }
            if (start.Equals(target) || !target.IsEnterable())
            {
                return new Queue<Tile>();
            }

            var frontier = new FastPriorityQueue<Tile>(100);
            var cameFrom = new Dictionary<Tile, Tile>();
            var cost = new Dictionary<Tile, int>();
            frontier.Enqueue(start, 0);
            cameFrom.Add(start, null);
            cost.Add(start, 0);

            while (frontier.Any())
            {
                var current = frontier.Dequeue();

                if (current.Equals(target))
                {
                    break;
                }

                foreach (var next in current.AdjacentTiles)
                {
                    if (!next.Passable || ActiveUnits.Instance.GetUnitAtTile(next))
                    {
                        continue;
                    }
                    var newCost = cost[current] + GetTileCost(current, next);
                    if (!cost.ContainsKey(next) || newCost < cost[next])
                    {
                        cost[next] = newCost;
                        var priority = newCost + Heuristic(next, target);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            // target is unreachable
            if (!cameFrom.ContainsKey(target))
            {
                return new Queue<Tile>();
            }

            var path = new List<Tile>();
            var nextInPath = target;
            while (nextInPath != start)
            {
                path.Add(nextInPath);
                nextInPath = cameFrom[nextInPath];
            }
            var pathQueue = new Queue<Tile>();
            path.Reverse();

            foreach (var step in path)
            {
                pathQueue.Enqueue(step);
            }

            return pathQueue;
        }

        private int Heuristic(Tile current, Tile target)
        {
            return Mathf.Abs(target.X - current.X) + Mathf.Abs(target.Y - current.Y);
        }

        private int GetTileCost(Tile parent, Tile candidate)
        {
            return parent.GetMoveCost(candidate);
        }
    }
}
