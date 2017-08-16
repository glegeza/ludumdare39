namespace DLS.LD39.Equipment
{
    using DLS.LD39.Combat;
    using DLS.LD39.Combat.Data;

    public class SecondaryWeapon : Weapon
    {
        public SecondaryWeapon(string name, string desc, WeaponData data) : base(name, desc, LootType.SecondaryWeapon, data.GetStats())
        {
            if (data.Slot != WeaponSlot.Secondary)
            {
                throw new System.Exception("Initializing SecondaryWeapon with PrimaryWeapon stats");
            }
        }
    }
}
