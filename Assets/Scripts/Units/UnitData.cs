namespace DLS.LD39.Units
{
    using DLS.LD39.AI;
    using DLS.LD39.Graphics;
    using UnityEngine;

    public class UnitData : ScriptableObject
    {
        public string ID;
        public Sprite Sprite;
        public SpriteAnimationData Animation;
        public Faction Faction;
        public State DefaultState;

        public StatsData Stats;
    }
}
