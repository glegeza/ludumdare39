namespace DLS.LD39.Combat
{
    using DLS.LD39.Equipment.Data;
    using DLS.LD39.Map;

    public class MeleeWeaponStats : WeaponStats
    {
        public MeleeWeaponStats(MeleeWeaponData data) 
            : base(data, WeaponType.Melee)
        { }

        public override bool TileIsLegalTarget(Tile attackerPos, Tile targetPos)
        {
            return (attackerPos != null && targetPos != null) &&
                attackerPos.IsAdjacent(targetPos);
        }
    }
}
