namespace DLS.LD39.Units
{
    using DLS.LD39.Equipment;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UnitEquipment : GameUnitComponent
    {
        public EquipmentSlot PrimaryWeapon
        {
            get; private set;
        }

        public EquipmentSlot SecondaryWeapon
        {
            get; private set;
        }

        public EquipmentSlot Shield
        {
            get; private set;
        }

        public EquipmentSlot Battery
        {
            get; private set;
        }

        public EquipmentSlot Accessory
        {
            get; private set;
        }

        private void Awake()
        {
            PrimaryWeapon = new EquipmentSlot(LootType.PrimaryWeapon);
            SecondaryWeapon = new EquipmentSlot(LootType.SecondaryWeapon);
            Shield = new EquipmentSlot(LootType.Shield);
            Battery = new EquipmentSlot(LootType.BatteryPack);
            Accessory = new EquipmentSlot(LootType.SuitAccessory);
        }
    }
}
