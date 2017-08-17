namespace DLS.LD39.Combat
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class LOSChecker : SingletonComponent<LOSChecker>
    {
        public bool LOSClear(Tile a, Tile b)
        {
            if (a == b)
            {
                Debug.LogError("Tiles are the same");
                return true;
                //throw new Exception("Tiles are the same");
            }

            var posA = a.WorldCoords;
            var posB = b.WorldCoords;

            var hits = Physics2D.LinecastAll(posA, posB);

            foreach (var hit in hits)
            {
                if (hit.collider.GetComponent<LOSBlocker>() != null)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<Tile> GetVisibleTiles(Tile origin, float distance)
        {
            var frontier = new Queue<Tile>();
            var visited = new HashSet<Tile>();
            var visible = new HashSet<Tile>();
            foreach (var tile in origin.AdjacentTiles)
            {
                frontier.Enqueue(tile);
                visited.Add(tile);
            }
            visible.Add(origin);
            visited.Add(origin);

            while (frontier.Any())
            {
                var thisTile = frontier.Dequeue();
                if (Vector2.Distance(thisTile.TileCoords, origin.TileCoords) > distance ||
                    !LOSClear(thisTile, origin))
                {
                    continue;
                }

                visible.Add(thisTile);

                foreach (var tile in thisTile.AdjacentTiles)
                {
                    if (visited.Contains(tile))
                    {
                        continue;
                    }
                    visited.Add(tile);
                    frontier.Enqueue(tile);
                }
            }

            return visible;
        }
    }
}
