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
                if (!RoomIsEntirelyContainedWithinMap(_map, newRoom) ||
                    rooms.Any(r => r.Overlaps(newRoom, 1)))
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

            var startRoom = rooms[2];
            var tile1 = _map.GetTile(startRoom.MapRect.Center);
            var tile2 = _map.GetTile(startRoom.MapRect.Center + new IntVector2(1, 0));
            var tile3 = _map.GetTile(startRoom.MapRect.Center + new IntVector2(-1, 0));
            var tile4 = _map.GetTile(startRoom.MapRect.Center + new IntVector2(0, 1));
            UnitSpawner.Instance.SpawnUnit("test_player", tile1);
            UnitSpawner.Instance.SpawnUnit("test_player", tile2);
            UnitSpawner.Instance.SpawnUnit("test_player", tile3);
            UnitSpawner.Instance.SpawnUnit("test_player", tile4);
        }

        private bool RoomIsEntirelyContainedWithinMap(TileMap map, Room room)
        {
            var upLeft = room.MapRect.TopLeft;
            var botRight = room.MapRect.BottomRight;

            return map.GetTile(upLeft) != null && map.GetTile(botRight) != null;
        }
    }
}
