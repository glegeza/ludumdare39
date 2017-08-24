namespace DLS.LD39.AI.Data
{
    using DLS.LD39.Map;
    using System.Collections.Generic;

    public interface IMovePathData : IStateData
    {
        Queue<Tile> CurrentPath { get; set; } 
        Tile MoveTarget { get; set; }
    }
}
