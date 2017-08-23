namespace DLS.LD39.Equipment.Data
{
    using UnityEngine;
    using Combat;

    public abstract class WeaponData : EquipmentData<Weapon>
    {
        [Header("Weapon Stats")]
        public int MinDamage;
        public int MaxDamage;
        public int BaseToHitModifier;
        public WeaponSlot Slot;

        [Header("Costs")]
        public int APCost;
        public int EnergyCost;

        [Header("Display")]
        public Sprite Icon;

        public abstract WeaponStats GetStats();

        public override Weapon GetLoot()
        {
            if (Slot == WeaponSlot.Primary)
            {
                return new PrimaryWeapon(this);
            }

            return new SecondaryWeapon(this);
        }
    }
}
