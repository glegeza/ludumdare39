namespace DLS.LD39.Equipment
{
    using DLS.LD39.Combat;
    using Data;

    public class PrimaryWeapon : Weapon
    {
        public PrimaryWeapon(WeaponData data) : base(data.Name, data.Description, LootType.PrimaryWeapon, data.GetStats())
        {
            if (data.Slot != WeaponSlot.Primary)
            {
                throw new System.Exception("Initializing PrimaryWeapon with SecondaryWeapon stats");
            }
        }
    }
}
