namespace DLS.LD39.Equipment.Data
{
    using Combat;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Equipment/Weapons/Ranged")]
    public class RangedWeaponData : WeaponData
    {
        [Header("Ranged Stats")]
        public int Range;

        public override WeaponStats GetStats()
        {
            return new RangedWeaponStats(this);
        }
    }
}
