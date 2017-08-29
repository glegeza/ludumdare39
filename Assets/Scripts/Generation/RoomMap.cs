// ReSharper disable RedundantDefaultMemberInitializer
namespace DLS.LD39.Generation
{
    using System;
    using Map;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using JetBrains.Annotations;
    using UnityEngine;
    using Utility;
    using Random = UnityEngine.Random;

    [UsedImplicitly]
    [RequireComponent(typeof(TileMap))]
    public class RoomMap : MonoBehaviour
    {
        public RoomMapGenerationSettings Settings;

        private TileMap _map;
        private readonly Dictionary<string, Room> _rooms 
            = new Dictionary<string, Room>();

        public IEnumerable<Room> Rooms
        {
            get { return _rooms.Values; }
        }

        public Room GetRoom(string id)
        {
            Room room;
            _rooms.TryGetValue(id, out room);
            return room;
        }

        [UsedImplicitly]
        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        [UsedImplicitly]
        private void Start()
        {
            GenerateTileMap();
            BuildStaticRooms();
            BuildRooms();
            //BuildCorridors();
            SpawnPlayerUnits();
        }

        private void GenerateTileMap()
        {
            var width = Random.Range(Settings.MinWidth, Settings.MaxWidth);
            var height = Random.Range(Settings.MinHeight, Settings.MaxHeight);
            _map.Width = width;
            _map.Height = height;
            _map.RebuildMap();
        }

        private void AddRoom(Room newRoom, string tileType="default")
        {
            if (_rooms.ContainsKey(newRoom.ID))
            {
                Debug.LogErrorFormat("Adding duplicate room {0}", newRoom.ID);
            }
            newRoom.SetTiles(_map, tileType);
            _rooms.Add(newRoom.ID, newRoom);
        }

        private void BuildStaticRooms()
        {
            foreach (var room in Settings.StaticRooms)
            {
                var position = GetStaticRoomPosition(room);
                var newRoom = room.Type.GetRoomAtPosition(_map, position, room.ID);
                AddRoom(newRoom, room.Type.BaseTileType);
            }
        }

        private IntVector2 GetStaticRoomPosition(StaticRoom room)
        {
            int x;
            if (room.RoomPos.x < 0)
            {
                x = _map.Width + (int)room.RoomPos.x;
            }
            else
            {
                x = (int) room.RoomPos.x;
            }

            int y;
            if (room.RoomPos.y < 0)
            {
                y = _map.Width + (int) room.RoomPos.y;
            }
            else
            {
                y = (int) room.RoomPos.y;
            }

            return new IntVector2(x, y);
        }

        private void BuildRooms()
        {
            var failures = 0;
            var roomTable = Settings.GetRoomRollTable();
            var roomsBuilt = 0;
            while (roomsBuilt < Settings.TargetRooms && failures < Settings.MaximumFailures)
            {
                var newRoom = GetRandomRoom(roomTable, roomsBuilt);
                if (!CanPlaceRoom(newRoom))
                {
                    failures++;
                    continue;
                }
                roomsBuilt += 1;
                AddRoom(newRoom);
                SpawnBadGuys(_map, newRoom);
            }
        }

        private Room GetRandomRoom(RoomRollTable rollTable, int roomIdx)
        {
            var roomType = rollTable.GetRandomEntry();
            return roomType.GetRoomRandomPosition(_map, 
                String.Format("{0} {1}", roomType.ID, roomIdx));
        }

        private bool CanPlaceRoom(Room candidateRoom)
        {
            return RoomIsEntirelyContainedWithinMap(_map, candidateRoom) && !_rooms.Values.Any(
                       r => candidateRoom.Overlaps(r, 1));
        }

        private void BuildCorridors()
        {
            var rooms = _rooms.Values.ToList();
            for (var i = 0; i < rooms.Count - 1; i++)
            {
                var corridor = new SingleWidthCorridor();
                corridor.AddNode(rooms[i].MapRect.Center);
                corridor.AddNode(rooms[i + 1].MapRect.Center);
                corridor.SetTiles(_map);
            }
        }

        private void SpawnPlayerUnits()
        {
            var tile1 = _map.GetTile(_rooms["entry"].TranslateLocalTileCoords(2, 1));
            var tile2 = _map.GetTile(_rooms["entry"].TranslateLocalTileCoords(2, 2));
            var tile3 = _map.GetTile(_rooms["entry"].TranslateLocalTileCoords(2, 3));
            var tile4 = _map.GetTile(_rooms["entry"].TranslateLocalTileCoords(3, 2));
            UnitSpawner.Instance.SpawnUnit("test_player", tile1);
            UnitSpawner.Instance.SpawnUnit("test_player", tile2);
            UnitSpawner.Instance.SpawnUnit("test_player", tile3);
            UnitSpawner.Instance.SpawnUnit("test_player", tile4);
        }

        private void SpawnBadGuys(TileMap map, Room room)
        {
            var enemyTable = room.Template.DefaultSpawnTable.GetRollTable();
            var placed = 0;
            var remainingTiles = new List<IntVector2>(room.Tiles);
            var tileCount = remainingTiles.Count;
            var unitsToPlace = Random.Range(
                room.Template.DefaultSpawnTable.MinimumUnits, 
                room.Template.DefaultSpawnTable.MaximumUnits + 1);
            while (placed < unitsToPlace && tileCount > 0)
            {
                var enemyType = enemyTable.GetRandomEntry().ID;
                var tileIdx = Random.Range(0, tileCount);
                var tile = map.GetTile(remainingTiles[tileIdx]);
                UnitSpawner.Instance.SpawnUnit(enemyType, tile);
                tileCount -= 1;
                if (tileCount > 0)
                {
                    var tmp = remainingTiles[tileCount];
                    remainingTiles[tileCount] = remainingTiles[tileIdx];
                    remainingTiles[tileIdx] = tmp;
                }
                placed++;
            }
        }

        private bool RoomIsEntirelyContainedWithinMap(TileMap map, Room room)
        {
            var upLeft = room.MapRect.TopLeft;
            var botRight = room.MapRect.BottomRight;

            return map.GetTile(upLeft) != null && map.GetTile(botRight) != null;
        }
    }
}
