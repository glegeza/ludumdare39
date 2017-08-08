namespace DLS.LD39.AI
{
    using UnityEngine;

    /// <summary>
    /// Represents a discrete question (ie, "Can I see an enemy?" or "Am I
    /// about to die?") the AI needs to answer in order to transition from
    /// one AI state to another.
    /// </summary>
    /// <remarks>
    /// Each Decision needs to have a simple yes/no answer and should not alter
    /// the StateController's state in any way. The purpose of these classes is
    /// to allow the AI to move from one mode to another. For example, An AI 
    /// that is patrolling from tile to tile looking for enemies will need to
    /// constantly ask itself if it can see a target. If it can, then it should
    /// switch to hunter mode. If it can't, it should continue to patrol.
    /// </remarks>
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(StateController controller);
    }
}
