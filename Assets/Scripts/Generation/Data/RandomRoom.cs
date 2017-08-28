namespace DLS.LD39.Generation.Data
{
    using System;
    using JetBrains.Annotations;
    using Map;
    using UnityEngine;
    using Utility;
    using Random = UnityEngine.Random;

    [UsedImplicitly]
    [CreateAssetMenu(menuName = "Map Generation/Room Maps/Random Room")]
    public class RandomRoom : RoomType
    {
        [Header("Room Size")]
        public int MinWidth;
        public int MaxWidth;
        public int MinHeight;
        public int MaxHeight;

        public override Room GetRoomRandomPosition(TileMap map, string id="")
        {
            id = String.IsNullOrEmpty(id) ? ID : id;
            var w = Random.Range(MinWidth, MaxWidth);
            var h = Random.Range(MinHeight, MaxHeight);
            var x = Random.Range(0, map.Width - w);
            var y = Random.Range(0, map.Height - h);
            return new Room(x, y, w, h, id);
        }

        public override Room GetRoomAtPosition(TileMap map, IntVector2 position, string id="")
        {
            id = String.IsNullOrEmpty(id) ? ID : id;
            var w = Random.Range(MinWidth, MaxWidth);
            var h = Random.Range(MinHeight, MaxHeight);
            return new Room(position.X, position.Y,
                w, h, id);
        }
    }
}
