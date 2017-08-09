namespace DLS.LD39.Units
{
    using UnityEngine;

    public class ActionPoints : GameUnitComponent
    {
        private GameUnit _unit;

        public int PointsPerTurn
        {
            get
            {
                return _unit.Stats.Speed * 3;
            }
        }

        public int PointsRemaining
        {
            get; set;
        }

        public int MaximumPoints
        {
            get
            {
                return _unit.Stats.Speed * 6;
            }
        }

        public bool CanSpendPoints(int amount)
        {
            return amount <= PointsRemaining;
        }

        public void SpendPoints(int amount)
        {
            if (!CanSpendPoints(amount))
            {
                Debug.LogError("Unit attempting to spend too many points");
                return;
            }

            PointsRemaining -= amount;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            _unit = unit;
        }

        protected override void OnTurnStarted()
        {
            PointsRemaining += PointsPerTurn;
            PointsRemaining = Mathf.Min(PointsRemaining, MaximumPoints);
        }
    }
}
