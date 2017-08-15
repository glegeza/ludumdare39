namespace DLS.LD39.Units.Actions
{
    using System;

    public abstract class UnitAction : GameUnitComponent
    {
        public event EventHandler<EventArgs> StartedAction;

        public event EventHandler<EventArgs> CompletedAction;

        public bool ActionInProgress
        {
            get; private set;
        }        

        protected bool APAvailable(int cost)
        {
            return AttachedUnit.AP.PointsAvailable(cost);
        }

        protected void StartAction(EventArgs args, int ap)
        {
            if (ActionInProgress)
            {
                UnityEngine.Debug.LogError("Attempting to start action without completing in progress action.");
                return;
            }
            SpendAP(ap);
            ActionInProgress = true;
            StartedAction?.Invoke(this, args);
        }

        protected void CompleteAction(EventArgs args)
        {
            if (!ActionInProgress)
            {
                UnityEngine.Debug.LogError("Attempting to complete action without first starting it.");
                return;
            }
            ActionInProgress = false;
            CompletedAction?.Invoke(this, args);
        }

        private void SpendAP(int cost)
        {
            AttachedUnit.AP.SpendPoints(cost);
        }
    }
}
