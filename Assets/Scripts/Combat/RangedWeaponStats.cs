namespace DLS.LD39.Combat
{
    using DLS.LD39.Map;
    using UnityEngine;

    public class RangedWeaponStats : WeaponStats
    {
        public RangedWeaponStats(int minDmg, int maxDmg, int baseToHit, int range, WeaponSlot slot) 
            : base(minDmg, maxDmg, baseToHit, WeaponType.Ranged, slot)
        {
            Range = range;
        }

        public int Range
        {
            get; private set;
        }

        public override bool TileIsLegalTarget(Tile attackerPos, Tile targetPos)
        {
            if (attackerPos == null || targetPos == null)
            {
                return false;
            }
            var distance = Vector2.Distance(attackerPos.TileCoords, targetPos.TileCoords);
            return distance <= Range;
        }
    }
}
