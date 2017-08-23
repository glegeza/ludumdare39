namespace DLS.LD39.Equipment
{
    using System.Collections.Generic;
    using DLS.LD39.Equipment.Data;
    using DLS.LD39.Actions;

    public class BatteryPack : Loot
    {
        public BatteryPack(BatteryPackData data) 
            : base(data.Name, data.Description, LootType.BatteryPack, new List<Action>())
        {
            MaximumCapacity = data.Capacity;
            PassiveRegen = data.PassiveRegenRate;
        }

        public int MaximumCapacity
        {
            get; private set;
        }

        public int PassiveRegen
        {
            get; private set;
        }
    }
}
