namespace DLS.LD39.Units
{
    using UnityEngine;

    class ActionPoints : MonoBehaviour, IGameUnitComponent
    {
        public int PointsPerTurn
        {
            get; set;
        }

        public int PointsRemaining
        {
            get; set;
        }

        public int MaximumPoints
        {
            get; set;
        }

        public void BeginTurn()
        {
            PointsRemaining += PointsPerTurn;
            PointsRemaining = Mathf.Min(PointsRemaining, MaximumPoints);
        }

        public void EndTurn()
        {

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
    }
}
