namespace DLS.LD39.Conditions
{
    using UnityEngine;

    public abstract class GameCondition : ScriptableObject
    {
        public abstract ConditionCheckInterval Interval { get; }

        public abstract bool IsConditionMet();
    }
}
