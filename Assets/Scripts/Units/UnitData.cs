namespace DLS.LD39.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class UnitData : ScriptableObject
    {
        public string ID;
        public Sprite Sprite;
        public Faction Faction;

        public int MaximumAP;
        public int APRegen;

        public float BaseInitMin;
        public float BaseInitMax;
    }
}
