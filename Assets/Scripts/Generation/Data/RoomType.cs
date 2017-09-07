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

        /// <summary>
        /// The largest possible rectangle occupied by this room.
        /// </summary>
        /// <returns>The maximum width (x) and height (y) for the room</returns>
        public abstract IntVector2 GetMaximumRoomSize();

        /// <summary>
        /// The smallest space this room can fit in.
        /// </summary>
        /// <returns>The minimum width (x) and height (y) for the room</returns>
        public abstract IntVector2 GetMinimumRoomSize();

        public abstract Room GetRoomRandomPosition(TileMap map, string id="");

        public abstract Room GetRoomAtPosition(TileMap map, IntVector2 position, string id="");
    }
}
