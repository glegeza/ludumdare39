namespace DLS.LD39.Equipment
{
    using DLS.LD39.Combat;

    public abstract class Weapon : Loot
    {
        protected Weapon(string name, string desc, LootType type, WeaponStats weapon) 
            : base(name, desc, type)
        {
            if (! (type == LootType.PrimaryWeapon || type == LootType.SecondaryWeapon))
            {
                throw new System.Exception("Weapon type must be a primary or secondary weapon");
            }
            if (weapon == null)
            {
                throw new System.ArgumentNullException("weapon");
            }
            Stats = weapon;
        }

        public WeaponStats Stats
        {
            get; private set;
        }
    }
}
