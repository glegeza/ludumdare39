namespace DLS.LD39.Generation.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Units.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Map Generation/Spawning/Spawn Table")]
    public class SpawnTable : ScriptableObject
    {
        public int MinimumUnits;
        public int MaximumUnits;
        public List<EnemyProbability> EnemySpawnChances;

        [UsedImplicitly]
        private void Awake()
        {
            MinimumUnits = Mathf.Max(0, MaximumUnits);
            MaximumUnits = Mathf.Clamp(MinimumUnits, 0, MaximumUnits);
        }

        public EnemySpawnRollTable GetRollTable()
        {
            return new EnemySpawnRollTable(EnemySpawnChances.Cast<RollTableEntry<UnitData>>());
        }
    }
}
