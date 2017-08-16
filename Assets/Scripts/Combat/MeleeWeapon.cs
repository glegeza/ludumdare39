namespace DLS.LD39.Combat
{
    using DLS.LD39.Map;

    public class MeleeWeapon : WeaponStats
    {
        public MeleeWeapon(int minDmg, int maxDmg, int baseToHit, WeaponSlot slot) 
            : base(minDmg, maxDmg, baseToHit, WeaponType.Melee, slot)
        { }

        public override bool TileIsLegalTarget(Tile attackerPos, Tile targetPos)
        {
            return (attackerPos != null && targetPos != null) &&
                attackerPos.IsAdjacent(targetPos);
        }
    }
}
