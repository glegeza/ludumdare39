namespace DLS.LD39.AI
{
    using UnityEngine;

    /// <summary>
    /// Pluggable component that checks the currently active unit's state to
    /// determine if it should end its turn.
    /// </summary>
    public abstract class TurnEndDecision : ScriptableObject
    {
        public abstract bool ShouldEndTurn(StateController controller);
    }
}
