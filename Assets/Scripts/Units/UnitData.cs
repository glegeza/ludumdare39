namespace DLS.LD39.Units
{
    using DLS.LD39.AI;
    using UnityEngine;

    public class UnitData : ScriptableObject
    {
        public string ID;
        public Sprite Sprite;
        public RuntimeAnimatorController Controller;
        public Faction Faction;
        public State DefaultState;

        public StatsData Stats;
    }
}
