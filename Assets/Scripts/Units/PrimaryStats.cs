namespace DLS.LD39.Units
{
    using UnityEngine;

    public class PrimaryStats : MonoBehaviour
    {
        private int _baseAim;
        private int _baseEvasion;
        private int _baseArmor;
        private int _baseSpeed;
        private int _baseMaxHP;

        public void Initialize(StatsData data)
        {
            _baseAim = Random.Range(data.BaseAimMin, data.BaseAimMax);
            _baseEvasion = Random.Range(data.BaseEvasionMin, data.BaseEvasionMax);
            _baseArmor = Random.Range(data.BaseArmorMin, data.BaseArmorMax);
            _baseSpeed = Random.Range(data.BaseSpeedMin, data.BaseSpeedMax);
            _baseMaxHP = Random.Range(data.BaseHPMin, data.BaseHPMax);
        }

        public int Aim
        {
            get
            {
                return _baseAim;
            }
        }

        public int Evasion
        {
            get
            {
                return _baseEvasion;
            }
        }

        public int Armor
        {
            get
            {
                return _baseArmor;
            }
        }

        public int Speed
        {
            get
            {
                return _baseSpeed;
            }
        }

        public int MaxHP
        {
            get
            {
                return _baseMaxHP;
            }
        }
    }
}
