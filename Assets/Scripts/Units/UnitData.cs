namespace DLS.LD39.Units
{
    using UnityEngine;

    public class UnitData : ScriptableObject
    {
        public string ID;
        public Sprite Sprite;
        public Faction Faction;

        public StatsData Stats;
    }
}
