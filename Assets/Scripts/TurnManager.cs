namespace DLS.LD39
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Priority_Queue;
    using UnityEngine;

    class TurnManager : MonoBehaviour
    {
        public float DelayTime = 1.0f;

        private List<GameUnit> _units = new List<GameUnit>();

        private SimplePriorityQueue<GameUnit> _unitsRemaining = new SimplePriorityQueue<GameUnit>();

        public GameUnit ActiveUnit
        {
            get; private set;
        }

        public void EndCurrentUnitTurn()
        {
            if (ActiveUnit == null)
            {
                return;
            }
            ActiveUnit.EndTurn();
        }

        public void RegisterUnit(GameUnit unit)
        {
            _units.Add(unit);
            unit.TurnEnded += OnUnitTurnEnded;
            UpdateTurnOrder();
        }

        public void UpdateTurnOrder()
        {
            _unitsRemaining.Clear();

            foreach (var unit in _units)
            {
                _unitsRemaining.Enqueue(unit, unit.Init.InitiativeValue);
            }
        }

        private void OnUnitTurnEnded(object sender, EventArgs e)
        {
            var unit = sender as GameUnit;
            if (unit == null)
            {
                throw new InvalidOperationException("OnUnitTurnEnded subscribed to non-GameUnit event");
            }

            SetNextUnit();
        }

        private void SetNextUnit()
        {
            if (!_unitsRemaining.Any())
            {
                UpdateTurnOrder();
            }
            ActiveUnit = _unitsRemaining.Dequeue();
        }
    }
}
