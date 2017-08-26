namespace DLS.LD39.AI.Data
{
    using Map;
    using System.Collections.Generic;

    public interface IMovePathData : IStateData
    {
        Queue<Tile> CurrentPath { get; set; } 
        Tile MoveTarget { get; set; }
    }
}
