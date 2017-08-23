namespace DLS.LD39.Equipment
{
    using DLS.LD39.Actions;
    using System.Collections.Generic;

    public class SuitAccessory : Loot
    {
        public SuitAccessory(string name, string desc) : base(name, desc, LootType.SuitAccessory, new List<Action>())
        {
        }
    }
}
