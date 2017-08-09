namespace DLS.LD39.Units
{
    using UnityEngine;

    public abstract class GameUnitComponent : MonoBehaviour
    {
        private bool _inTurn = false;

        public void Initialize(GameUnit unit)
        {
            if (unit == null)
            {
                throw new System.ArgumentNullException("unit");
            }
            OnInitialized(unit);
        }

        public void BeginTurn()
        {
            if (_inTurn)
            {
                throw new System.Exception("BeginTurn called twice.");
            }
            OnTurnStarted();
        }

        public void EndTurn()
        {
            if (!_inTurn)
            {
                throw new System.Exception("EndTurn called before BeginTurn.");
            }
            OnTurnEnded();
        }

        protected virtual void OnInitialized(GameUnit unit)
        { }

        protected virtual void OnTurnStarted()
        { }

        protected virtual void OnTurnEnded()
        { }
    }
}
