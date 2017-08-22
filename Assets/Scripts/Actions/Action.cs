namespace DLS.LD39.Actions
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using UnityEngine;

    public delegate bool ActionCompletedDelegate();

    public abstract class Action : ScriptableObject
    {
        public string ID;
        public Sprite IconSprite;

        public abstract ActionSelectMode Mode
        {
            get;
        }

        public abstract string GetName(GameUnit unit);

        public abstract string GetDescription(GameUnit unit);

        public abstract int GetAPCost(GameUnit unit);

        public abstract int GetEnergyCost(GameUnit unit);

        public abstract bool ActionIsValid(GameUnit actor, GameObject target, Tile targetTile);

        public abstract bool AttemptAction(GameUnit actor, GameObject target, Tile targetTile, ActionCompletedDelegate actionCB);
    }
}
