namespace DLS.LD39.Generation.Data
{
    using Map;
    using UnityEngine;
    using Utility;

    public abstract class RoomType : ScriptableObject
    {
        [Header("Room Parameters")]
        public string ID;
        public string BaseTileType;

        [Header("Enemies")]
        [Tooltip("Determines which enemies spawn if no map-specific spawning data is given.")]
        public SpawnTable DefaultSpawnTable;

        public abstract Room GetRoomRandomPosition(TileMap map, string id="");

        public abstract Room GetRoomAtPosition(TileMap map, IntVector2 position, string id="");
    }
}
