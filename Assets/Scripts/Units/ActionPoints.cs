namespace DLS.LD39.Units
{
    using UnityEngine;

    public class ActionPoints : GameUnitComponent
    {
        public int PointsRemaining
        {
            get; private set;
        }

        public bool PointsAvailable(int amount)
        {
            return amount <= PointsRemaining;
        }

        public void SpendPoints(int amount)
        {
            if (!PointsAvailable(amount))
            {
                Debug.LogError("Unit attempting to spend too many points");
                return;
            }

            PointsRemaining -= amount;
        }

        protected override void OnTurnStarted()
        {
            PointsRemaining += AttachedUnit.SecondaryStats.ActionPointRegen;
            PointsRemaining = Mathf.Min(
                PointsRemaining, AttachedUnit.SecondaryStats.ActionPointCap);
        }
    }
}