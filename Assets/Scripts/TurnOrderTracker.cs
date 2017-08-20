namespace DLS.LD39
{
    using DLS.LD39.Units;
    using Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Priority_Queue;
    using UnityEngine;

    class TurnOrderTracker : MonoBehaviour
    {
        private static TurnOrderTracker _instance;

        private SimplePriorityQueue<GameUnit> _unitsWaiting = new SimplePriorityQueue<GameUnit>();
        private List<GameUnit> _unitsDone = new List<GameUnit>();

        public event EventHandler<EventArgs> TurnOrderUpdated;

        public event EventHandler<EventArgs> TurnAdvanced;

        public static TurnOrderTracker Instance
        {
            get
            {
                return _instance;
            }
        }

        public GameUnit ActiveUnit
        {
            get; private set;
        }

        public IEnumerable<GameUnit> UnitsDone
        {
            get
            {
                return _unitsDone;
            }
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
            //if (UnitActionCoordinator.Instance.ActionInProgress)
            //{
            //    Debug.LogError("Attempting to advance turn while action still in progress.");
            //    return;
            //}

            if (ActiveUnit != null && !ActiveUnit.Ready)
            {
                return;
            }

            if (ActiveUnit == null)
            {
                SetNextUnit();
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
                _unitsWaiting.Enqueue(unit, unit.SecondaryStats.Initiative);
            }
            _unitsDone.Clear();
        }

        public void RegisterUnit(GameUnit unit)
        {
            unit.TurnEnded += OnUnitTurnEnded;

            if (ActiveUnit == null)
            {
                _unitsDone.Add(unit);
                SetNextUnit();
            }
            else if (unit.SecondaryStats.Initiative < ActiveUnit.SecondaryStats.Initiative)
            {
                _unitsDone.Add(unit);
            }
            else
            {
                _unitsWaiting.Enqueue(unit, unit.SecondaryStats.Initiative);
            }
        }

        public void UnregisterUnit(GameUnit unit)
        {
            if (ActiveUnit == unit)
            {
                ActiveUnit.TurnEnded -= OnUnitTurnEnded;
                ActiveUnit = null;
                unit.EndTurn();
                AdvanceTurn();
                return;
            }

            if (!_unitsDone.Remove(unit))
            {
                _unitsWaiting.Remove(unit);
            }
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
            // Move the currently active unit (if any) to the done list
            if (ActiveUnit != null)
            {
                _unitsDone.Add(ActiveUnit);
                ActiveUnit = null;
            }

            // Bail if there aren't any units registered
            if (!_unitsDone.Any())
            {
                return;
            }

            // If everyone has acted, start a new round
            if (!_unitsWaiting.Any())
            {
                BeginNewRound();
            }

            // Finally, get the next unit to act, notify it that its turn is
            // starting
            ActiveUnit = _unitsWaiting.Dequeue();
            ActiveUnit.BeginTurn();
            TurnAdvanced.SafeRaiseEvent(this);
            if (ActiveUnit.Faction == Faction.Player)
            {
                ActiveSelectionTracker.Instance.SetSelection(ActiveUnit);
            }
        }
    }
}