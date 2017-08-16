namespace DLS.LD39.Equipment.Data
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Equipment/Battery Pack")]
    public class BatteryPackData : EquipmentData<BatteryPack>
    {
        [Header("Battery Stats")]
        public int Capacity;
        public int PassiveRegenRate;

        public override BatteryPack GetLoot()
        {
            return new BatteryPack(this);
        }
    }
}
