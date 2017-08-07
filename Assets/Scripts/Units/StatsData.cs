namespace DLS.LD39.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class StatsData : ScriptableObject
    {
        public static int MaxStatValue = 20;

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
