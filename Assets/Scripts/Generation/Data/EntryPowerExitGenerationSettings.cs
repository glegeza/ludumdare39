namespace DLS.LD39.Generation.Data
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Map Generation/Generator Parameters/Entry-Power-Exit")]
    public class EntryPowerExitGenerationSettings : ScriptableObject
    {
        [Header("Map Size Parameters")]
        public int MinWidth;
        public int MaxWidth;
        public int MinHeight;
        public int MaxHeight;

        [Header("Core Rooms")]
        public RoomType EntryRoom;
        public RoomType ExitRoom;
        public RoomType GeneratorRoom;
    }
}
