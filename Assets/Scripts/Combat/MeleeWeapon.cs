namespace DLS.LD39.Combat
{
    public class MeleeWeapon : WeaponStats
    {
        public MeleeWeapon(int minDmg, int maxDmg, int baseToHit) 
            : base(minDmg, maxDmg, baseToHit, WeaponType.Melee)
        { }
    }
}
