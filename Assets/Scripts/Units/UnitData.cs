namespace DLS.LD39.Units
{
    using DLS.LD39.AI;
    using DLS.LD39.Combat.Data;
    using DLS.LD39.Graphics;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Unit")]
    public class UnitData : ScriptableObject
    {
        public string ID;
        public Sprite Sprite;
        public Color SpriteColor = Color.white;
        public RuntimeAnimatorController Controller;
        public Explosion DeathPrefab;
        public Faction Faction;
        public State DefaultState;
        public WeaponData DefaultWeapon;

        public StatsData Stats;
    }
}
