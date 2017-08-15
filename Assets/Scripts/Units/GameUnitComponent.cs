namespace DLS.LD39.Units
{
    using UnityEngine;

    public abstract class GameUnitComponent : MonoBehaviour
    {
        private bool _inTurn = false;

        public GameUnit AttachedUnit
        {
            get; private set;
        }

        public bool ComponentInitialized
        {
            get; private set;
        }

        public void Initialize(GameUnit unit)
        {
            if (unit == null)
            {
                throw new System.ArgumentNullException("unit");
            }
            AttachedUnit = unit;
            ComponentInitialized = true;
            OnInitialized(unit);
        }

        public void BeginTurn()
        {
            if (_inTurn)
            {
                throw new System.Exception("BeginTurn called twice in a row.");
            }
            _inTurn = true;
            OnTurnStarted();
        }

        public void EndTurn()
        {
            if (!_inTurn)
            {
                throw new System.Exception("EndTurn called before BeginTurn.");
            }
            _inTurn = false;
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
