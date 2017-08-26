namespace DLS.LD39.Equipment.Data
{
    using UnityEngine;
    using System.Collections.Generic;
    using Actions;

    public abstract class EquipmentData<T> : ScriptableObject where T : Loot
    {
        [Header("Equipment Info")]
        public string ID;
        public string Name;
        public string Description;

        [Header("Actions")]
        public List<Action> Actions = new List<Action>();

        public abstract T GetLoot();
    }
}
