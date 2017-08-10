namespace DLS.LD39.Combat
{
    public class RangedWeapon : WeaponStats
    {
        public RangedWeapon(int minDmg, int maxDmg, int baseToHit, int range) 
            : base(minDmg, maxDmg, baseToHit, WeaponType.Ranged)
        {
            Range = range;
        }

        public int Range
        {
            get; private set;
        }
    }
}
