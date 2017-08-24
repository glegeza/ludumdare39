namespace DLS.LD39.AI
{
    using UnityEngine;

    public abstract class StateTurnInitializer : ScriptableObject
    {
        public abstract void OnTurnStart(StateController controller);
    }
}
