namespace DLS.LD39.Equipment
{
    using DLS.LD39.Actions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Shield : Loot
    {
        public Shield(string name, string desc) : base(name, desc, LootType.Shield, new List<Action>())
        {
        }
    }
}
