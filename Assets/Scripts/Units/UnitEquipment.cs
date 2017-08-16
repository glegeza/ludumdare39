namespace DLS.LD39.Units
{
    using DLS.LD39.Equipment;

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

        private void Awake()
        {
            PrimaryWeapon = new EquipmentSlot<PrimaryWeapon>();
            SecondaryWeapon = new EquipmentSlot<SecondaryWeapon>();
            Shield = new EquipmentSlot<Shield>();
            Battery = new EquipmentSlot<BatteryPack>();
            Accessory = new EquipmentSlot<SuitAccessory>();
        }
    }
}
