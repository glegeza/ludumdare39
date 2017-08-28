namespace DLS.LD39.Generation.Data
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    [CreateAssetMenu(menuName = "Map Generation/Room Maps/Map Settings")]
    public class RoomMapGenerationSettings : ScriptableObject
    {
        [Header("Map Size Parameters")]
        public int MinWidth;
        public int MaxWidth;
        public int MinHeight;
        public int MaxHeight;

        [Header("Room Generation Parameters")]
        [Tooltip("Rooms that will be randomly selected and placed.")]
        public List<RoomProbability> ProbabilityList;
        [Tooltip("Rooms that will always be generated at fixed locations.")]
        public List<StaticRoom> StaticRooms;
    }
}
