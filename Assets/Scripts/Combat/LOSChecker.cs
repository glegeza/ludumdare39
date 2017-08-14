namespace DLS.LD39.Combat
{
    using DLS.LD39.Map;
    using System;
    using UnityEngine;

    public class LOSChecker : SingletonComponent<LOSChecker>
    {
        public bool LOSClear(Tile a, Tile b)
        {
            if (a == b)
            {
                throw new Exception("Tiles are the same");
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
    }
}
