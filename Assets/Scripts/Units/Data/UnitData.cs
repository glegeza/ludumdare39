namespace DLS.LD39.Units.Data
{
    using AI;
    using Equipment.Data;
    using Graphics;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Unit")]
    public class UnitData : ScriptableObject
    {
        public string ID;
        public Faction Faction;
        public StatGenerator StatsGenerator;

        [Header("Graphics")]
        public Sprite Sprite;
        public Color SpriteColor = Color.white;
        public RuntimeAnimatorController AnimationController;
        public Explosion DeathPrefab;

        [Header("AI")]
        public State DefaultState;

        [Header("Equipment")]
        public WeaponData DefaultPrimaryWeapon;
        public WeaponData DefaultSecondaryWeapon;
        public BatteryPackData BatteryPack;
    }
}
