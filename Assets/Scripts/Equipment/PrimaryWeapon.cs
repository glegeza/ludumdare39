namespace DLS.LD39.Equipment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PrimaryWeapon : Loot
    {
        public PrimaryWeapon(string name, string desc) : base(name, desc, LootType.PrimaryWeapon)
        {
        }
    }
}
