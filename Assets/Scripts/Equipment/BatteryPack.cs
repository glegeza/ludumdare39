namespace DLS.LD39.Equipment
{
    using Data;

    public class BatteryPack : Loot
    {
        public BatteryPack(BatteryPackData data) 
            : base(data.Name, data.Description, LootType.BatteryPack, data.Actions)
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
