namespace DLS.LD39.AI
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Simple class to set up AI decision-making. The AI uses Transitions to
    /// branch to one of two States based on the result of a Decision.
    /// </summary>
    [Serializable]
    public class Transition
    {
        [Tooltip("The Decision used by this Transition to branch to another State.")]
        public Decision Decision;
        [Tooltip("The state to transition to if Decision evaluates to true.")]
        public State TrueState;
        [Tooltip("The state to transition to if Decision evaluates to false.")]
        public State FalseState;
    }
}
