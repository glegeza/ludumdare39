namespace DLS.LD39.Generation.Data
{
    using System;
    using JetBrains.Annotations;
    using Map;
    using UnityEngine;
    using Utility;
    using Random = UnityEngine.Random;

    [UsedImplicitly]
    [CreateAssetMenu(menuName = "Map Generation/Room Maps/Fixed Room")]
    public class FixedRoom : RoomType
    {
        [Tooltip("The width and height of the room.")]
        public Vector2 RoomSize;

        public override IntVector2 GetMaximumRoomSize()
        {
            return new IntVector2((int)RoomSize.x, (int)RoomSize.y);
        }

        public override Room GetRoomRandomPosition(TileMap map, string id="")
        {
            id = String.IsNullOrEmpty(id) ? ID : id;
            var w = (int) RoomSize.x;
            var h = (int) RoomSize.y;
            var x = Random.Range(0, map.Width - w);
            var y = Random.Range(0, map.Height - h);
            return new Room(x, y, w, h, id, this);
        }

        public override Room GetRoomAtPosition(TileMap map, IntVector2 position, string id="")
        {
            id = String.IsNullOrEmpty(id) ? ID : id;
            var x = position.X;
            var y = position.Y;
            return new Room(x, y, (int)RoomSize.x, (int)RoomSize.y, id, this);
        }
    }
}
