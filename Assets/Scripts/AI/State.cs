﻿namespace DLS.LD39.AI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents one possible state for a unit's AI to be in.
    /// </summary>
    /// <remarks>
    /// A state is a mode that the AI is operating in, like "chasing" or 
    /// "patrolling." While in a particular state, the AI will repeatedly
    /// take a series of actions. Once it has completed all of its actions,
    /// it will ask itself a series of questions to determine if it should
    /// transition to a new state.
    /// </remarks>
    [CreateAssetMenu(menuName ="AI/State")]
    public class State : ScriptableObject
    {
        [Tooltip("State name displayed for debugging.")]
        public string Name;

        [Tooltip("Handles any special work that should be done at the start of a turn.")]
        public StateTurnInitializer TurnInitializer;

        [Tooltip("Handles special initialization for this state. Can be null.")]
        public StateInitializer Initializer;

        [Tooltip("Handles special clean-up for this state. Can be null.")]
        public StateFinalizer Finalizer;

        [Tooltip("The actions taken by the AI each turn that it is in this state.")]
        public List<AIAction> Actions = new List<AIAction>();

        [Tooltip("Questions the AI asks itself after completing actions to determine if it should transition to a new state.")]
        public List<Transition> ActionTransitions = new List<Transition>();

        [Tooltip("Questions the AI asks itself at the start of a turn to determine if it should transition to a new state.")]
        public List<Transition> TurnStartTransitions = new List<Transition>();

        [Tooltip("Questions the AI asks itself at the end of a turn to determine if it should transition to a new state.")]
        public List<Transition> TurnEndTransitions = new List<Transition>();
        
        /// <summary>
        /// Called when a game unit transitions to this state.
        /// </summary>
        public void BeginState(StateController controller)
        {
            if (Initializer != null)
            {
                Initializer.OnStateEnter(controller);
            }
        }

        /// <summary>
        /// Called when a unit using this state begins its turn.
        /// </summary>
        public void BeginTurn(StateController controller)
        {
            if (TurnInitializer != null)
            {
                TurnInitializer.OnTurnStart(controller);
            }
            CheckTransitions(controller, TurnStartTransitions);
        }

        /// <summary>
        /// Called continuously while a unit using this state is taking
        /// its turn. Returns false when the unit is done with its turn.
        /// </summary>
        /// <returns>True if the unit is ready wants to continue taking
        /// actions.</returns>
        public bool DoNextAction(StateController controller, int action)
        {
            // UGLY DoActions returns true if ANY action was successful. If it returns false, then all of the actions have failed and it's time to end the turn.
            var actionReturn = Actions[action].Act(controller);

            if (action == Actions.Count - 1)
            {
                CheckTransitions(controller, ActionTransitions);
            }

            return actionReturn;
        }

        /// <summary>
        /// Called when a unit using this state has ended its turn.
        /// </summary>
        public void EndTurn(StateController controller)
        {
            CheckTransitions(controller, TurnEndTransitions);
        }

        /// <summary>
        /// Called when a unit transitions away from this state.
        /// </summary>
        public void EndState(StateController controller)
        {
            if (Finalizer != null)
            {
                Finalizer.OnStateExit(controller);
            }
        }

        private void CheckTransitions(StateController controller, IEnumerable<Transition> transitions)
        {
            foreach (var transition in transitions)
            {
                Debug.LogFormat("Checking transition {0}", transition.Decision.name);
                var decisionIsTrue = transition.Decision.Decide(controller);
                if (decisionIsTrue && transition.TrueState != null)
                {
                    Debug.Log("Decision true");
                    controller.TransitionToState(transition.TrueState);
                    return;
                }
                else if (!decisionIsTrue && transition.FalseState != null)
                {
                    Debug.Log("Decision false");
                    controller.TransitionToState(transition.FalseState);
                    return;
                }
            }
        }
    }
}
