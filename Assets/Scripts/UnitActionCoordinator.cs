namespace DLS.LD39
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utility;
    using UnityEngine;

    public class UnitActionCoordinator : SingletonComponent<UnitActionCoordinator>
    {
        public event EventHandler<EventArgs> UnitStartedAction;

        public event EventHandler<EventArgs> UnitCompletedAction;

        public bool ActionInProgress
        {
            get; private set;
        }

        public bool TryMove()
        {
            throw new NotImplementedException();
        }

        public bool TryRangedAttack()
        {
            throw new NotImplementedException();
        }

        public bool TryMeleeAttack()
        {
            throw new NotImplementedException();
        }

        private void BeginAction()
        {
            if (TurnOrderTracker.Instance.ActiveUnit == null)
            {
                Debug.LogError("Attempting to begin action without an active unit.");
                return;
            }
            if (ActionInProgress)
            {
                Debug.LogError("Attempting to begin action while action is already in progress.");
                return;
            }

            ActionInProgress = true;
            UnitStartedAction.SafeRaiseEvent(this);
        }

        private void CompleteAction()
        {
            if (TurnOrderTracker.Instance.ActiveUnit == null)
            {
                Debug.LogError("Attempting to end action without an active unit.");
                return;
            }
            if (!ActionInProgress)
            {
                Debug.LogError("Attempting to call CompleteAction before BeginAction.");
            }

            ActionInProgress = true;
            UnitCompletedAction.SafeRaiseEvent(this);
        }
    }
}
