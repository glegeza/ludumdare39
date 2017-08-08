namespace DLS.LD39.AI
{
    using UnityEngine;

    /// <summary>
    /// Contains any special actions that should be taken when a Game Unit
    /// transitions away from its current state.
    /// </summary>
    public abstract class StateFinalizer : ScriptableObject
    {
        public abstract void OnStateExit(StateController state);
    }
}
