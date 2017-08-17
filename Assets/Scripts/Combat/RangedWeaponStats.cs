namespace DLS.LD39.Combat
{
    using DLS.LD39.Equipment.Data;
    using DLS.LD39.Map;
    using UnityEngine;

    public class RangedWeaponStats : WeaponStats
    {
        public RangedWeaponStats(RangedWeaponData data) 
            : base(data, WeaponType.Ranged)
        {
            Range = data.Range;
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
