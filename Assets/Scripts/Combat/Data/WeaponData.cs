namespace DLS.LD39.Combat.Data
{
    using UnityEngine;

    public abstract class WeaponData : ScriptableObject
    {
        public string ID;
        public int MinDamage;
        public int MaxDamage;
        public int BaseToHitModifier;

        public abstract WeaponStats GetStats();
    }
}
