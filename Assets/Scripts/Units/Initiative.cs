namespace DLS.LD39.Units
{
    using UnityEngine;

    /// <summary>
    /// Component used to indicate that an object is part of the regular turn
    /// order.
    /// </summary>
    public class Initiative : MonoBehaviour, IGameUnitComponent
    {
        private GameUnit _unit;

        public float InitiativeValue
        {
            get
            {
                return _unit.Stats.Speed;
            }
        }

        public bool IsActiveUnit
        {
            get;
            set;
        }

        public void Initialize(GameUnit unit)
        {
            _unit = unit;
        }

        public void BeginTurn()
        {
            IsActiveUnit = true;
        }

        public void EndTurn()
        {
            IsActiveUnit = false;
        }
    }
}
