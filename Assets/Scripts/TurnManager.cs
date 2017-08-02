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
        private static TurnManager _instance;

        public float DelayTime = 1.0f;

        private SimplePriorityQueue<GameUnit> _unitsWaiting = new SimplePriorityQueue<GameUnit>();
        private List<GameUnit> _unitsDone = new List<GameUnit>();

        public static TurnManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public IEnumerable<GameUnit> UnitsDone
        {
            get
            {
                return _unitsDone;
            }
        }

        public GameUnit ActiveUnit
        {
            get; private set;
        }

        public IEnumerable<GameUnit> UnitsWaiting
        {
            get
            {
                return _unitsWaiting;
            }
        }

        public void AdvanceTurn()
        {
            if (ActiveUnit == null)
            {
                return;
            }
            ActiveUnit.EndTurn();
        }

        public void BeginNewRound()
        {
            if (ActiveUnit != null)
            {

            }
            _unitsWaiting.Clear();

            foreach (var unit in _unitsDone)
            {
                _unitsWaiting.Enqueue(unit, unit.Initiative.InitiativeValue);
            }
        }

        public void RegisterUnit(GameUnit unit)
        {
            if (ActiveUnit == null)
            {
                ActiveUnit = unit;
                ActiveUnit.BeginTurn();
            }
            else if (unit.Initiative.InitiativeValue < ActiveUnit.Initiative.InitiativeValue)
            {
                _unitsDone.Add(unit);
            }
            else
            {
                _unitsWaiting.Enqueue(unit, unit.Initiative.InitiativeValue);
            }
            unit.TurnEnded += OnUnitTurnEnded;
        }

        public void UnregisterUnit(GameUnit unit)
        {
            _unitsWaiting.Remove(unit);
            _unitsDone.Remove(unit);
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void OnUnitTurnEnded(object sender, EventArgs e)
        {
            var unit = sender as GameUnit;
            if (unit == null)
            {
                throw new InvalidOperationException("OnUnitTurnEnded subscribed to non-GameUnit event");
            }
            Debug.LogFormat("Turn ended for unit {0}", unit.name);
            SetNextUnit();
        }

        private void SetNextUnit()
        {
            if (ActiveUnit != null)
            {
                _unitsDone.Add(ActiveUnit);
                ActiveUnit = null;
            }
            if (!_unitsWaiting.Any())
            {
                BeginNewRound();
            }
            ActiveUnit = _unitsWaiting.Dequeue();
            ActiveUnit.BeginTurn();
        }
    }
}
