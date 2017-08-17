namespace DLS.LD39.Equipment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Shield : Loot
    {
        public Shield(string name, string desc) : base(name, desc, LootType.Shield)
        {
        }
    }
}
