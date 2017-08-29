namespace DLS.LD39.Generation
{
    using System.Collections.Generic;
    using Data;
    using JetBrains.Annotations;

    public class RoomRollTable : RollTable<RoomType>
    {
        public RoomRollTable([NotNull] IEnumerable<RollTableEntry<RoomType>> tableEntries)
            : base(tableEntries)
        {
        }
    }
}
