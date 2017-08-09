namespace DLS.LD39.Units
{
    using UnityEngine;

    /// <summary>
    /// Component used to indicate that an object is part of the regular turn
    /// order.
    /// </summary>
    public class Initiative : GameUnitComponent
    {
        private GameUnit _unit;

        public float InitiativeValue
        {
            get
            {
                return _unit.Stats.Speed;
            }
        }
    }
}
