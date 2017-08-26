namespace DLS.LD39.Units
{
    using Equipment;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class UnitEquipment : GameUnitComponent
    {
        public EquipmentSlot<PrimaryWeapon> PrimaryWeapon
        {
            get; private set;
        }

        public EquipmentSlot<SecondaryWeapon> SecondaryWeapon
        {
            get; private set;
        }

        public EquipmentSlot<Shield> Shield
        {
            get; private set;
        }

        public EquipmentSlot<BatteryPack> Battery
        {
            get; private set;
        }

        public EquipmentSlot<SuitAccessory> Accessory
        {
            get; private set;
        }

        public IEnumerable<Loot> EquippedItems
        {
            get
            {
                var equipment = new List<Loot>();
                AddIfNotNull(PrimaryWeapon.SlotItem, equipment);
                AddIfNotNull(SecondaryWeapon.SlotItem, equipment);
                AddIfNotNull(Shield.SlotItem, equipment);
                AddIfNotNull(Battery.SlotItem, equipment);
                AddIfNotNull(Accessory.SlotItem, equipment);
                return equipment;
            }
        }

        [UsedImplicitly]
        private void Awake()
        {
            PrimaryWeapon = new EquipmentSlot<PrimaryWeapon>();
            SecondaryWeapon = new EquipmentSlot<SecondaryWeapon>();
            Shield = new EquipmentSlot<Shield>();
            Battery = new EquipmentSlot<BatteryPack>();
            Accessory = new EquipmentSlot<SuitAccessory>();
        }

        private void AddIfNotNull(Loot loot, List<Loot> list)
        {
            if (loot != null)
            {
                list.Add(loot);
            }
        }
    }
}
