namespace DLS.LD39.Generation
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Utility;

    [RequireComponent(typeof(TileMap))]
    public class RoomMap : MonoBehaviour
    {
        public int MaxTries = 10;

        [Header("Room Dimensions")]
        public int RoomMaxWidth = 10;
        public int RoomMinWidth = 4;
        public int RoomMaxHeight = 10;
        public int RoomMinHeight = 4;

        private TileMap _map;

        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        private void Start()
        {
            var tries = 0;
            var rooms = new List<Room>();
            while (tries < MaxTries)
            {
                tries++;

                var x = UnityEngine.Random.Range(0, _map.Width);
                var y = UnityEngine.Random.Range(0, _map.Height);
                var w = UnityEngine.Random.Range(RoomMinWidth, RoomMaxWidth);
                var h = UnityEngine.Random.Range(RoomMinHeight, RoomMaxHeight);

                var newRoom = new Room(x, y, w, h);
                if (_map.GetTile(newRoom.MapRect.Center.X, newRoom.MapRect.Center.Y) == null ||
                    rooms.Any(r => r.Overlaps(newRoom)))
                {
                    continue;
                }
                rooms.Add(newRoom);
                newRoom.SetTiles(_map);
            }

            for (var i = 0; i < rooms.Count - 1; i++)
            {
                var corridor = new SingleWidthCorridor();
                corridor.AddNode(rooms[i].MapRect.Center);
                corridor.AddNode(rooms[i + 1].MapRect.Center);
                corridor.SetTiles(_map);
            }
        }
    }
}
