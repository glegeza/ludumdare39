namespace DLS.LD39.Actions
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using DLS.Utility;
    using System.Collections.Generic;
    using UnityEngine;

    public class UnitActionController : GameUnitComponent
    {
        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        public event EventHandler<EventArgs> ActionsUpdated;

        public IEnumerable<Action> Actions
        {
            get
            {
                return _actions.Values;
            }
        }

        public bool Ready
        {
            get; private set;
        }

        public void UpdateActions()
        {
            Ready = true;
            _actions.Clear();
            var equipment = AttachedUnit.Equipment.EquippedItems;
            foreach (var item in equipment)
            {
                foreach (var action in item.Actions)
                {
                    AddAction(action);
                }
            }

            ActionsUpdated.SafeRaiseEvent(this);
        }

        public void AddAction(Action action)
        {
            _actions.Add(action.ID, action);
        }

        public bool TryAction(string id, GameObject target, Tile tile)
        {
            if (!Ready)
            {
                return false;
            }

            if (!_actions.ContainsKey(id))
            {
                throw new ArgumentException(String.Format("Invalid action type {0}", id));
            }

            var action = _actions[id];

            if (ActionIsValid(action, target, tile))
            {
                DoAction(action, target, tile);
                return true;
            }

            return false;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            UpdateActions();
        }

        protected override void OnTurnStarted()
        {
            Ready = true;
        }

        protected override void OnTurnEnded()
        {
            Ready = false;
        }

        private void DoAction(Action action, GameObject target, Tile tile)
        {
            var apCost = action.GetAPCost(AttachedUnit);
            var energyCost = action.GetEnergyCost(AttachedUnit);

            Ready = false;

            AttachedUnit.AP.SpendPoints(apCost);
            var ep = AttachedUnit.GetComponent<EnergyPoints>();
            if (ep != null)
            {
                ep.SpendPoints(energyCost);
            }

            action.AttemptAction(AttachedUnit, target, tile, () => { Ready = true; } );
        }

        private bool ActionIsValid(Action action, GameObject target, Tile tile)
        {
            var apCost = action.GetAPCost(AttachedUnit);
            var energyCost = action.GetEnergyCost(AttachedUnit);

            var valid = action.ActionIsValid(AttachedUnit, target, tile);
            var enoughEnergy = UnitHasEnoughEnergy(action.GetEnergyCost(AttachedUnit));
            var enoughAP = UnitHasEnoughAP(action.GetAPCost(AttachedUnit));

            return valid && enoughEnergy && enoughAP;
        }

        private bool UnitHasEnoughEnergy(int energy)
        {
            if (energy == 0)
            {
                return true;
            }

            var energyComp = AttachedUnit.GetComponent<EnergyPoints>();
            return energyComp != null && energyComp.PointsAvailable(energy);
        }

        private bool UnitHasEnoughAP(int ap)
        {
            return AttachedUnit.AP.PointsAvailable(ap);
        }
    }
}
