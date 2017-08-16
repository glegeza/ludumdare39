namespace DLS.LD39.Combat.Data
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Weapons/Ranged")]
    public class RangedWeaponData : WeaponData
    {
        public int Range;

        public override WeaponStats GetStats()
        {
            return new RangedWeapon(MinDamage, MaxDamage, BaseToHitModifier, Range, Slot);
        }
    }
}
