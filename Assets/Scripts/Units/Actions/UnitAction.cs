namespace DLS.LD39.Units.Actions
{
    using System;
    using Utility;

    public abstract class UnitAction : GameUnitComponent
    {
        private EnergyPoints _energy;

        public event EventHandler<EventArgs> StartedAction;

        public event EventHandler<EventArgs> CompletedAction;

        public bool ActionInProgress
        {
            get; private set;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            _energy = unit.GetComponent<EnergyPoints>();
        }

        protected bool ResourcesAvailable(int ap, int energy)
        {
            return APAvailable(ap) && EnergyAvailable(energy);
        }

        protected bool APAvailable(int cost)
        {
            if (cost == 0)
            {
                return true;
            }

            return AttachedUnit.AP.PointsAvailable(cost);
        }

        protected bool EnergyAvailable(int cost)
        {
            if (_energy == null)
            {
                UnityEngine.Debug.Log("Energy is null");
            }
            if (cost == 0)
            {
                return true;
            }

            return _energy != null
                ? _energy.PointsAvailable(cost)
                : false;
        }

        protected void StartAction(EventArgs args, int ap, int energy=0)
        {
            if (ActionInProgress)
            {
                UnityEngine.Debug.LogError("Attempting to start action without completing in progress action.");
                return;
            }
            SpendAP(ap);
            SpendEnergy(energy);
            ActionInProgress = true;
            StartedAction.SafeRaiseEvent(this, args);
        }

        protected void CompleteAction(EventArgs args)
        {
            if (!ActionInProgress)
            {
                UnityEngine.Debug.LogError("Attempting to complete action without first starting it.");
                return;
            }
            ActionInProgress = false;
            CompletedAction.SafeRaiseEvent(this, args);
        }

        private void SpendAP(int cost)
        {
            AttachedUnit.AP.SpendPoints(cost);
        }

        private void SpendEnergy(int cost)
        {
            if (_energy != null)
            {
                _energy.SpendPoints(cost);
            }
        }
    }
}
