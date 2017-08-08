namespace DLS.LD39.Units
{
    using UnityEngine;

    public abstract class UnitController : MonoBehaviour
    {
        public abstract void Initialize(GameUnit unit);

        public abstract void BeginTurn();

        public abstract void EndTurn();
    }
}
