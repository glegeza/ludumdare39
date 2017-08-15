namespace DLS.LD39.Units.Data
{
    using UnityEngine;

    public abstract class StatGenerator : ScriptableObject
    {
        public abstract int GetAim();

        public abstract int GetEvasion();

        public abstract int GetArmor();

        public abstract int GetSpeed();

        public abstract int GetMaxHP();
    }
}
