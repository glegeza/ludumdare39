namespace DLS.LD39.Equipment
{
    using DLS.LD39.Equipment.Data;

    public class BatteryPack : Loot
    {
        public BatteryPack(BatteryPackData data) 
            : base(data.Name, data.Description, LootType.BatteryPack)
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
