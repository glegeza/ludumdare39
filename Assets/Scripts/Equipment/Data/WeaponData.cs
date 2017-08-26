namespace DLS.LD39.Equipment.Data
{
    using UnityEngine;
    using Combat;

    public abstract class WeaponData : EquipmentData<Weapon>
    {
        [Header("Weapon Stats")]
        public int MinDamage = 0;
        public int MaxDamage = 0;
        public int BaseToHitModifier = 0;
        public WeaponSlot Slot = WeaponSlot.Primary;

        [Header("Costs")]
        public int APCost = 0;
        public int EnergyCost = 0;

        [Header("Display")]
        public Sprite Icon = null;

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
