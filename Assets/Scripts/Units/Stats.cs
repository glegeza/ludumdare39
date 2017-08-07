namespace DLS.LD39.Units
{
    public class Stats
    {
        public Stats(StatsData data)
        {
            Aim = UnityEngine.Random.Range(data.BaseAimMin, data.BaseAimMax);
            Evasion = UnityEngine.Random.Range(data.BaseEvasionMin, data.BaseEvasionMax);
            Armor = UnityEngine.Random.Range(data.BaseArmorMin, data.BaseArmorMax);
            Speed = UnityEngine.Random.Range(data.BaseSpeedMin, data.BaseSpeedMax);
            MaxHP = UnityEngine.Random.Range(data.BaseHPMin, data.BaseHPMax);
        }

        public int Aim { get; set; }

        public int Evasion { get; set; }

        public int Armor { get; set; }

        public int Speed { get; set; }

        public int MaxHP { get; set; }
    }
}
