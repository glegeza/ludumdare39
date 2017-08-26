namespace DLS.LD39
{
    using Units;
    using Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Priority_Queue;
    using UnityEngine;

    public class TurnOrderTracker : SingletonComponent<TurnOrderTracker>
    {
        private SimplePriorityQueue<GameUnit> _unitsWaiting = new SimplePriorityQueue<GameUnit>();
        private List<GameUnit> _unitsDone = new List<GameUnit>();

        public event EventHandler<EventArgs> RoundCompleted;

        public event EventHandler<EventArgs> UnitEndedTurn;

        public event EventHandler<EventArgs> TurnOrderUpdated;

        public event EventHandler<EventArgs> TurnAdvanced;

        public int RoundsCompleted { get; private set; }

        public GameUnit PreviousUnit
        {
            get; private set;
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
            if (ActiveUnit != null && !ActiveUnit.Ready)
            {
                return;
            }

            var currentUnitName = ActiveUnit == null
                ? "No Unit"
                : ActiveUnit.UnitName;


            if (ActiveUnit == null)
            {
                SetNextUnit();
                return;
            }

            Debug.LogFormat("{0} is ending turn", currentUnitName);
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
            RoundsCompleted += 1;
            RoundCompleted.SafeRaiseEvent(this);
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
            TurnOrderUpdated.SafeRaiseEvent(this);
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
            TurnOrderUpdated.SafeRaiseEvent(this);
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
            // Move the currently active unit (if any) to the done list
            if (ActiveUnit != null)
            {
                UnitEndedTurn.SafeRaiseEvent(this);
                _unitsDone.Add(ActiveUnit);
                PreviousUnit = ActiveUnit;
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
            Debug.LogFormat("{0} is now the active unit.", ActiveUnit.UnitName);
            ActiveUnit.BeginTurn();
            TurnAdvanced.SafeRaiseEvent(this);
            if (ActiveUnit.Faction == Faction.Player)
            {
                ActiveSelectionTracker.Instance.SetSelection(ActiveUnit);
            }
            TurnOrderUpdated.SafeRaiseEvent(this);
        }
    }
}