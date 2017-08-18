namespace DLS.LD39.Units
{
    using UnityEngine;

    public class EnergyPoints : GameUnitComponent
    {
        public int PointsRemaining
        {
            get; private set;
        }

        public int EnergyCapacity
        {
            get
            {
                return AttachedUnit.Equipment.Battery.SlotItem == null
                    ? 0
                    : AttachedUnit.Equipment.Battery.SlotItem.MaximumCapacity;
            }
        }

        public bool PointsAvailable(int amount)
        {
            return amount <= PointsRemaining;
        }

        public void SpendPoints(int amount)
        {
            if (!PointsAvailable(amount))
            {
                Debug.LogError("Unit attempting to spend too much energy.");
            }
            PointsRemaining -= amount;
        }

        protected override void OnTurnStarted()
        {
            var battery = AttachedUnit.Equipment.Battery.SlotItem;
            if (battery == null)
            {
                PointsRemaining = 0;
                return;
            }
            PointsRemaining += battery.PassiveRegen;
            PointsRemaining = Mathf.Min(PointsRemaining, EnergyCapacity);
        }
    }
}
