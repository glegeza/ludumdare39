namespace DLS.LD39.Equipment
{
    using DLS.LD39.Combat;
    using DLS.LD39.Combat.Data;

    public class SecondaryWeapon : Weapon
    {
        public SecondaryWeapon(WeaponData data) : base(data.Name, data.Description, LootType.SecondaryWeapon, data.GetStats())
        {
            if (data.Slot != WeaponSlot.Secondary)
            {
                throw new System.Exception("Initializing SecondaryWeapon with PrimaryWeapon stats");
            }
        }
    }
}
