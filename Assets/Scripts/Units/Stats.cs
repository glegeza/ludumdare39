namespace DLS.LD39.Units
{
    using UnityEngine;

    public class Stats : MonoBehaviour
    {
        public void Initialize(StatsData data)
        {
            Aim = Random.Range(data.BaseAimMin, data.BaseAimMax);
            Evasion = Random.Range(data.BaseEvasionMin, data.BaseEvasionMax);
            Armor = Random.Range(data.BaseArmorMin, data.BaseArmorMax);
            Speed = Random.Range(data.BaseSpeedMin, data.BaseSpeedMax);
            MaxHP = Random.Range(data.BaseHPMin, data.BaseHPMax);
        }

        public int Aim { get; set; }

        public int Evasion { get; set; }

        public int Armor { get; set; }

        public int Speed { get; set; }

        public int MaxHP { get; set; }
    }
}
