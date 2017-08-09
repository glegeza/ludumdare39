namespace DLS.LD39.AI
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// Controls the AI state for AI controlled game units.
    /// </summary>
    public class StateController : UnitController
    {
        public float ActionDelay = 0.1f;

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
        public StateData Data
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

        private int _currentAction = 0;
        private bool _controllerActive = false;
        private float _elapsedActionTime = 0.0f;
        private State _currentState;

        public override void Initialize(GameUnit unit)
        {
            if (unit == null)
            {
                Debug.LogErrorFormat("Initializing StateController with null GameUnit {0}", gameObject.name);
            }
            Unit = unit;
        }

        public void TransitionToState(State nextState)
        {
            if (Unit == null)
            {
                Debug.LogError("Transitioning on StateController with null Unit");
                return;
            }
            if (nextState == null)
            {
                Debug.LogError("Attempting to transition to null state");
                return;
            }
            if (nextState == _currentState)
            {
                return;
            }
            if (_currentState != null)
            {
                Debug.LogFormat("Ending state {0}", _currentState.Name);
                _currentState.EndState(this);
            }

            _currentState = nextState;
            Debug.LogFormat("Starting state {0}", _currentState.Name);
            nextState.BeginState(this);
            TurnsActive = 0;
            ActionsActive = 0;
        }

        public override void BeginTurn()
        {
            if (Unit == null || _currentState != null)
            {
                Debug.Log("AI beginning turn");
                _currentState.BeginTurn(this);
                _controllerActive = true;
                _currentAction = 0;
            }
        }

        public override void EndTurn()
        {
            if (Unit == null || _currentState != null)
            {
                Debug.Log("AI ending turn");
                _currentState.EndTurn(this);
                TurnsActive++;
                _controllerActive = false;
            }
        }

        private void Update()
        {
            if (Unit == null || !_controllerActive || _currentState == null)
            {
                return;
            }

            if (!Unit.Ready)
            {
                return;
            }

            _elapsedActionTime += Time.deltaTime;
            if (_elapsedActionTime > ActionDelay)
            {
                _elapsedActionTime = 0.0f;
                var shouldEndTurn = !_currentState.DoNextAction(this, _currentAction);
                _currentAction = (_currentAction + 1) % _currentState.Actions.Count;
                ActionsActive++;
                if (shouldEndTurn)
                {
                    TurnOrderTracker.Instance.AdvanceTurn();
                }
            }
        }
    }
}
