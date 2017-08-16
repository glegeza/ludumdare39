namespace DLS.LD39.Equipment.Data
{
    using UnityEngine;

    public abstract class EquipmentData : ScriptableObject
    {
        [Header("Equipment Info")]
        public string ID;
        public string Name;
        public string Description;
    }
}
