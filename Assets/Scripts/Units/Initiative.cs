namespace DLS.LD39.Units
{
    using UnityEngine;

    /// <summary>
    /// Component used to indicate that an object is part of the regular turn
    /// order.
    /// </summary>
    public class Initiative : MonoBehaviour
    {
        public float InitiativeValue
        {
            get;
            private set;
        }

        public bool IsActiveUnit
        {
            get;
            private set;
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
