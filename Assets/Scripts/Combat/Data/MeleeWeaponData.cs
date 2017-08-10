namespace DLS.LD39.Combat.Data
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Weapons/Melee")]
    public class MeleeWeaponData : WeaponData
    {
        public override WeaponStats GetStats()
        {
            return new MeleeWeapon(MinDamage, MaxDamage, BaseToHitModifier);
        }
    }
}
