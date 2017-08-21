namespace DLS.LD39.Actions
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public delegate bool ActionCompletedDelegate();

    public abstract class Action : ScriptableObject
    {
        public string ID;
        public Sprite IconSprite;

        public abstract string GetName();

        public abstract string GetDescription();

        public abstract int GetAPCost();

        public abstract bool ActionIsValid(GameUnit actor, GameObject target, Tile targetTile);

        public abstract bool AttemptAction(GameUnit actor, GameObject target, Tile targetTile);
    }
}
