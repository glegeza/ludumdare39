using DLS.LD39.Map;

namespace DLS.LD39.Combat
{
    public abstract class WeaponStats
    {
        protected WeaponStats(int minDmg, int maxDmg, int baseToHit, WeaponType type, WeaponSlot slot)
        {
            MinDamage = minDmg;
            MaxDamage = maxDmg;
            BaseToHit = baseToHit;
            Type = type;
        }

        public int MinDamage
        {
            get; private set;
        }

        public int MaxDamage
        {
            get; private set;
        }

        public int BaseToHit
        {
            get; private set;
        }

        public WeaponType Type
        {
            get; private set;
        }

        public WeaponSlot Slot
        {
            get; private set;
        }

        public abstract bool TileIsLegalTarget(Tile attackerPos, Tile targetPos);
    }
}
