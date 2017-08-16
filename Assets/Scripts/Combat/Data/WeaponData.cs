namespace DLS.LD39.Combat.Data
{
    using DLS.LD39.Equipment.Data;
    using UnityEngine;

    public abstract class WeaponData : EquipmentData
    {
        [Header("Weapon Stats")]
        public int MinDamage;
        public int MaxDamage;
        public int BaseToHitModifier;
        public WeaponSlot Slot;

        public abstract WeaponStats GetStats();
    }
}
