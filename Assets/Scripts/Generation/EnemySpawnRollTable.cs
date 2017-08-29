namespace DLS.LD39.Generation
{
    using System.Collections.Generic;
    using Data;
    using JetBrains.Annotations;
    using Units.Data;

    public class EnemySpawnRollTable : RollTable<UnitData>
    {

        public EnemySpawnRollTable([NotNull] IEnumerable<RollTableEntry<UnitData>> probabilityList)
            : base(probabilityList)
        {
        }
    }
}
