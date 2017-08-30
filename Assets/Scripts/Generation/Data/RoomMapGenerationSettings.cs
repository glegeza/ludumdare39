namespace DLS.LD39.Generation.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    [CreateAssetMenu(menuName = "Map Generation/Generator Parameters/Room Map")]
    public class RoomMapGenerationSettings : ScriptableObject
    {
        [Header("Map Size Parameters")]
        public int MinWidth;
        public int MaxWidth;
        public int MinHeight;
        public int MaxHeight;

        [Header("Room Placement Parameters")]
        [Tooltip("The maximum number of rooms to generate for the map.")]
        public int TargetRooms;
        [Tooltip("The number of room placement failures to allow before giving up.")]
        public int MaximumFailures;

        [Header("Room Types")]
        [Tooltip("Rooms that will be randomly selected and placed.")]
        public List<RoomProbability> RoomGenerationTable;
        [Tooltip("Rooms that will always be placed, but at a random location.")]
        public List<RoomType> RequiredRooms;
        [Tooltip("Rooms that will always be generated at fixed locations.")]
        public List<StaticRoom> StaticRooms;

        public RoomRollTable GetRoomRollTable()
        {
            return new RoomRollTable(RoomGenerationTable.Cast<RollTableEntry<RoomType>>());
        }
    }
}
