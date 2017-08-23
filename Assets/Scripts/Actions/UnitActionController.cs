namespace DLS.LD39.Actions
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UnitActionController : GameUnitComponent
    {
        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

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

        public void AddAction(Action action)
        {
            _actions.Add(action.ID, action);
        }

        public bool TryAction(string id, GameObject target, Tile tile)
        {
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

        private void DoAction(Action action, GameObject target, Tile tile)
        {
            var apCost = action.GetAPCost(AttachedUnit);
            var energyCost = action.GetEnergyCost(AttachedUnit);

            Ready = false;

            action.AttemptAction(AttachedUnit, target, tile, () => { Ready = true; } );
        }

        private bool ActionIsValid(Action action, GameObject target, Tile tile)
        {
            var apCost = action.GetAPCost(AttachedUnit);
            var energyCost = action.GetEnergyCost(AttachedUnit);

            return action.ActionIsValid(AttachedUnit, target, tile) &&
                UnitHasEnoughEnergy(action.GetEnergyCost(AttachedUnit)) &&
                UnitHasEnoughAP(action.GetAPCost(AttachedUnit));
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
