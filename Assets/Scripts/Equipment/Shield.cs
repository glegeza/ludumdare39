namespace DLS.LD39.Equipment
{
    using Actions;
    using System.Collections.Generic;

    public class Shield : Loot
    {
        public Shield(string name, string desc) : base(name, desc, LootType.Shield, new List<Action>())
        {
        }
    }
}
