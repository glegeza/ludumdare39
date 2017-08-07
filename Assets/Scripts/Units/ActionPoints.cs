namespace DLS.LD39.Units
{
    using UnityEngine;

    class ActionPoints : MonoBehaviour, IGameUnitComponent
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

        public void Initialize(GameUnit unit)
        {
            _unit = unit;
        }

        public void BeginTurn()
        {
            PointsRemaining += PointsPerTurn;
            PointsRemaining = Mathf.Min(PointsRemaining, MaximumPoints);
        }

        public void EndTurn() { }

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
    }
}
