namespace DLS.LD39.AI
{
    using UnityEngine;

    /// <summary>
    /// Contains any special actions that should be taken when a Game Unit
    /// transitions to a new AI state.
    /// </summary>
    public abstract class StateInitializer : ScriptableObject
    {
        public abstract void OnStateEnter(StateController controller);
    }
}
