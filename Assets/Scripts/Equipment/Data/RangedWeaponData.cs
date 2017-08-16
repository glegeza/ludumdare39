namespace DLS.LD39.Equipment.Data
{
    using Combat;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Weapons/Ranged")]
    public class RangedWeaponData : WeaponData
    {
        public int Range;

        public override WeaponStats GetStats()
        {
            return new RangedWeaponStats(this);
        }
    }
}
