namespace DLS.LD39.Units.Data
{
    using AI;
    using Equipment.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Units/Unit")]
    public class UnitData : ScriptableObject
    {
        public string ID;
        public Faction Faction;
        public StatGenerator StatsGenerator;


        [Header("Graphics")]
        public UnitSprites GraphicsData;
        public Color SpriteTint = Color.white;

        [Header("AI")]
        public State DefaultState;

        [Header("Equipment")]
        public WeaponData DefaultPrimaryWeapon;
        public WeaponData DefaultSecondaryWeapon;
        public BatteryPackData BatteryPack;
    }
}
