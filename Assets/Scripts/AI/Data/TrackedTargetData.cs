namespace DLS.LD39.AI.Data
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System.Collections.Generic;

    public class TrackedTargetData : IMovePathData
    {
        public GameUnit CurrentTarget { get; set; }
        public Tile MoveTarget { get; set; }
        public Queue<Tile> CurrentPath { get; set; }
    }
}