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

        public bool IsActive
        {
            get;
            private set;
        }

        public void BeginTurn()
        {

        }

        public void EndTurn()
        {

        }
    }
}
