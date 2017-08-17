namespace DLS.LD39.Units.Data
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Unit Stat Generator")]
    public class BasicStatGenerator : StatGenerator
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

        public override int GetAim()
        {
            return Random.Range(BaseAimMin, BaseAimMax + 1);
        }

        public override int GetEvasion()
        {
            return Random.Range(BaseEvasionMin, BaseEvasionMax + 1);
        }

        public override int GetArmor()
        {
            return Random.Range(BaseArmorMin, BaseArmorMax + 1);
        }

        public override int GetSpeed()
        {
            return Random.Range(BaseSpeedMin, BaseSpeedMax + 1);
        }

        public override int GetMaxHP()
        {
            return Random.Range(BaseHPMin, BaseHPMax + 1);
        }
    }
}
