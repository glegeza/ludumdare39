namespace DLS.LD39.Equipment.Data
{
    using UnityEngine;

    public abstract class EquipmentData<T> : ScriptableObject where T : Loot
    {
        [Header("Equipment Info")]
        public string ID;
        public string Name;
        public string Description;

        public abstract T GetLoot();
    }
}
