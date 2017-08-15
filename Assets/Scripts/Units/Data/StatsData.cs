namespace DLS.LD39.Units.Data
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Unit Stat Generator")]
    public class StatsData : ScriptableObject
    {
        public static int MaxStatValue = 80;

        public int BaseAimMin;
        public int BaseAimMax;

        public int BaseEvasionMin;
        public int BaseEvasionMax;

        public int BaseArmorMin;
        public int BaseArmorMax;

        public int BaseSpeedMin;
        public int BaseSpeedMax;

        public int BaseHPMin;
        public int BaseHPMax;
    }
}
