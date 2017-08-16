namespace DLS.LD39.Equipment
{
    using DLS.LD39.Combat;
    using DLS.LD39.Combat.Data;

    public class PrimaryWeapon : Weapon
    {
        public PrimaryWeapon(string name, string desc, WeaponData data) : base(name, desc, LootType.PrimaryWeapon, data.GetStats())
        {
            if (data.Slot != WeaponSlot.Primary)
            {
                throw new System.Exception("Initializing PrimaryWeapon with SecondaryWeapon stats");
            }
        }
    }
}
