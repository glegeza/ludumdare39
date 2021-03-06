﻿namespace DLS.LD39.Generation.Data
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

        public override IntVector2 GetMaximumRoomSize()
        {
            return new IntVector2(MaxWidth, MaxHeight);
        }

        public override IntVector2 GetMinimumRoomSize()
        {
            return new IntVector2(MinWidth, MinHeight);
        }

        public override Room GetRoomRandomPosition(TileMap map, string id="")
        {
            id = String.IsNullOrEmpty(id) ? ID : id;
            var w = Random.Range(MinWidth, MaxWidth);
            var h = Random.Range(MinHeight, MaxHeight);
            var x = Random.Range(0, map.Width - w);
            var y = Random.Range(0, map.Height - h);
            return new RectRoom(x, y, w, h, id, this);
        }

        public override Room GetRoomAtPosition(TileMap map, IntVector2 position, string id="")
        {
            id = String.IsNullOrEmpty(id) ? ID : id;
            var w = Random.Range(MinWidth, MaxWidth);
            var h = Random.Range(MinHeight, MaxHeight);
            return new RectRoom(position.X, position.Y,
                w, h, id, this);
        }
    }
}
