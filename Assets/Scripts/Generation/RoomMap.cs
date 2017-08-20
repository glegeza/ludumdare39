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
        [Header("Map Layout")]
        public int TargetRooms = 10;
        public int MaxFailures = 100;

        [Header("Room Dimensions")]
        public int RoomMaxWidth = 10;
        public int RoomMinWidth = 4;
        public int RoomMaxHeight = 10;
        public int RoomMinHeight = 4;

        private TileMap _map;

        private void Awake()
        {
            _map = GetComponent<TileMap>();

            RoomMaxWidth = Mathf.Max(1, RoomMaxWidth);
            RoomMaxHeight = Mathf.Max(1, RoomMaxHeight);
            RoomMinWidth = Mathf.Clamp(RoomMinWidth, 1, RoomMaxWidth);
            RoomMinHeight = Mathf.Clamp(RoomMinHeight, 1, RoomMaxHeight);
        }

        private void Start()
        {
            var failures = 0;
            var rooms = new List<Room>();

            var entryRoom = new Room(1, 1, 5, 5);
            entryRoom.SetTiles(_map);
            rooms.Add(entryRoom);

            while (rooms.Count < TargetRooms && failures < MaxFailures)
            {
                var w = UnityEngine.Random.Range(RoomMinWidth, RoomMaxWidth);
                var h = UnityEngine.Random.Range(RoomMinHeight, RoomMaxHeight);
                var x = UnityEngine.Random.Range(0, _map.Width - w);
                var y = UnityEngine.Random.Range(0, _map.Height - h);

                var newRoom = new Room(x, y, w, h);
                if (!RoomIsEntirelyContainedWithinMap(_map, newRoom) ||
                    rooms.Any(r => r.Overlaps(newRoom, 1)))
                {
                    failures++;
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

            var tile1 = _map.GetTile(entryRoom.TranslateLocalTileCoords(2, 1));
            var tile2 = _map.GetTile(entryRoom.TranslateLocalTileCoords(2, 2));
            var tile3 = _map.GetTile(entryRoom.TranslateLocalTileCoords(2, 3));
            var tile4 = _map.GetTile(entryRoom.TranslateLocalTileCoords(3, 2));
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
