﻿namespace DLS.LD39.AI
{
    using JetBrains.Annotations;
    using Units;
    using UnityEngine;

    /// <summary>
    /// Controls the AI state for AI controlled game units.
    /// </summary>
    public class StateController : MonoBehaviour
    {
        private int _currentAction;
        private bool _controllerActive;
        private bool _checkForTurnEndNextCycle;
        private float _elapsedActionTime;
        private State _currentState;

        /// <summary>
        /// The Game Unit this StateController is controlling.
        /// </summary>
        public GameUnit Unit
        {
            get; private set;
        }

        /// <summary>
        /// Special data that the current state needs to keep track of. 
        /// </summary>
        public IStateData Data
        {
            get; set;
        }

        /// <summary>
        /// The number of complete turns that the current state has been
        /// active.
        /// </summary>
        public int TurnsActive
        {
            get; private set;
        }

        /// <summary>
        /// The number of times the current state has had its actions
        /// updated.
        /// </summary>
        public int ActionsActive
        {
            get; private set;
        }

        /// <summary>
        /// The number of complete action cycles the unit has taken this turn
        /// </summary>
        public int ActionCyclesThisTurn
        {
            get; private set;
        }

        /// <summary>
        /// The number of actions that have failed this cycle.
        /// </summary>
        public int ActionsFailedThisCycle
        {
            get; private set;
        }

        public float ActionDelay { get; set; }

        public void Initialize(GameUnit unit)
        {
            if (unit == null)
            {
                Debug.LogErrorFormat("Initializing StateController with null GameUnit {0}", gameObject.name);
            }
            Unit = unit;
        }

        public void TransitionToState(State nextState)
        {
            if (!CanTransitionToNewState(nextState))
            {
                return;
            }

            EndCurrentStateIfAny();
            BeginNewState(nextState);
        }

        public void BeginTurn()
        {
            if (Unit != null && _currentState != null)
            {
                Debug.LogFormat("Unit {0} beginning turn for state {1}", Unit.UnitName, _currentState.name);
                ActionCyclesThisTurn = 0;
                _controllerActive = true;
                _currentState.BeginTurn(this);
                ActionsFailedThisCycle = 0;
                _currentAction = 0;
            }
        }

        public void EndTurn()
        {
            if (Unit != null && _currentState != null)
            {
                Debug.LogFormat("Unit {0} ending turn for state {1}", Unit.UnitName, _currentState.name);
                _currentState.EndTurn(this);
                TurnsActive++;
                _controllerActive = false;
            }
        }

        [UsedImplicitly]
        private void Awake()
        {
            ActionDelay = 0.0f;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (!ShouldUpdate())
            {
                return;
            }

            _elapsedActionTime += Time.deltaTime;
            if (_elapsedActionTime > ActionDelay)
            {
                RunNextAction();
            }
        }

        private void BeginNewState(State nextState)
        {
            _currentState = nextState;
            Debug.LogFormat("Starting state {0} - {1}", _currentState.Name, _controllerActive);
            nextState.BeginState(this);
            ActionCyclesThisTurn = 0;
            if (_controllerActive)
            {
                // if the controller is already active then we transitioned
                // during a turn, so do turn initialization stuff now
                nextState.BeginTurn(this);
            }
            _currentAction = 0;
            _checkForTurnEndNextCycle = false;
            TurnsActive = 0;
            ActionsActive = 0;
        }

        private void EndCurrentStateIfAny()
        {
            if (_currentState != null)
            {
                Debug.LogFormat("Ending state {0}", _currentState.Name);
                _currentState.EndState(this);
            }
        }

        private bool CanTransitionToNewState(State nextState)
        {
            if (Unit == null)
            {
                Debug.LogError("Transitioning on StateController with null Unit");
                return false;
            }
            if (nextState == null)
            {
                Debug.LogError("Attempting to transition to null state");
                return false;
            }
            if (nextState == _currentState)
            {
                Debug.LogError("Attempting to transition to same state");
                return false;
            }
            return true;
        }

        private void RunNextAction()
        {
            _elapsedActionTime = 0.0f;
            if (ShouldEndturn())
            {
                TurnOrderTracker.Instance.AdvanceTurn();
                return;
            }

            if (!_currentState.DoNextAction(this, _currentAction))
            {
                ActionsFailedThisCycle += 1;
            }

            _currentAction = (_currentAction + 1) % _currentState.Actions.Count;
            if (_currentAction == 0)
            {
                ActionCyclesThisTurn += 1;
                _checkForTurnEndNextCycle = true;
            }

            ActionsActive++;
        }

        private bool ShouldUpdate()
        {
            return Unit != null && _controllerActive && _currentState != null &&
                Unit.Ready;
        }

        private bool ShouldEndturn()
        {
            if (!_checkForTurnEndNextCycle)
            {
                return false;
            }
            _checkForTurnEndNextCycle = false;
            var shouldEnd = _currentState.ShouldEndTurn(this);
            ActionsFailedThisCycle = 0;
            return shouldEnd;
        }
    }
}
