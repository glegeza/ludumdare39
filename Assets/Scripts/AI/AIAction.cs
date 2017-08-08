namespace DLS.LD39.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents a single action for an AI unit to take. These should be
    /// discrete actions, like stepping to an adjacent tile or attacking an
    /// adjacent unit.
    /// </summary>
    abstract class AIAction : ScriptableObject
    {
        /// <summary>
        /// Called repeatedly to allow a unit to take actions during its turn.
        /// Returns true until the unit is ready to ends its turn.
        /// </summary>
        /// <param name="controller">The unit's state controller.</param>
        /// <returns>True if the unit was able to act, false if the unit could
        /// not act and is ready to end it turn.</returns>
        public abstract bool Act(StateController controller);
    }
}
