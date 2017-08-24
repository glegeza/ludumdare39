namespace DLS.LD39.Generation
{
    using DLS.LD39.Map;
    using DLS.LD39.Units.Data;
    using System.Collections.Generic;
    using System.Linq;
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

        [Header("Spawning")]
        public UnitData EnemyType;
        public int MinUnitsPerRoom = 0;
        public int MaxUnitsPerRoom = 2;

        private TileMap _map;

        public Room ExitRoom
        {
            get; private set;
        }

        private void Awake()
        {
            _map = GetComponent<TileMap>();

            RoomMaxWidth = Mathf.Max(1, RoomMaxWidth);
            RoomMaxHeight = Mathf.Max(1, RoomMaxHeight);
            RoomMinWidth = Mathf.Clamp(RoomMinWidth, 1, RoomMaxWidth);
            RoomMinHeight = Mathf.Clamp(RoomMinHeight, 1, RoomMaxHeight);

            MaxUnitsPerRoom = Mathf.Max(0, MaxUnitsPerRoom);
            MinUnitsPerRoom = Mathf.Clamp(MinUnitsPerRoom, 0, MaxUnitsPerRoom);
        }

        private void Start()
        {
            var entryRoom = BuildEntryRoom();
            var exitRoom = BuildExitRoom();
            var rooms = BuildRooms(entryRoom, exitRoom);
            BuildCorridors(rooms);
            SpawnPlayerUnits(_map, entryRoom);
        }

        private Room BuildExitRoom()
        {
            var exitRoom = new Room(10, 1, 4, 4);
            exitRoom.SetTiles(_map, "yellow");
            ExitRoom = exitRoom;
            return exitRoom;
        }

        private Room BuildEntryRoom()
        {
            var entryRoom = new Room(1, 1, 5, 5);
            entryRoom.SetTiles(_map, "yellow");
            return entryRoom;
        }

        private List<Room> BuildRooms(Room entryRoom, Room exitRoom)
        {
            var failures = 0;
            var rooms = new List<Room>();
            rooms.Add(entryRoom);
            rooms.Add(exitRoom);

            while (rooms.Count < TargetRooms && failures < MaxFailures)
            {
                var w = Random.Range(RoomMinWidth, RoomMaxWidth);
                var h = Random.Range(RoomMinHeight, RoomMaxHeight);
                var x = Random.Range(0, _map.Width - w);
                var y = Random.Range(0, _map.Height - h);

                var newRoom = new Room(x, y, w, h);
                if (!RoomIsEntirelyContainedWithinMap(_map, newRoom) ||
                    rooms.Any(r => newRoom.Overlaps(r, 1)))
                {
                    failures++;
                    continue;
                }
                rooms.Add(newRoom);
                newRoom.SetTiles(_map);
                SpawnBadGuys(_map, newRoom);
            }

            return rooms;
        }

        private void BuildCorridors(List<Room> rooms)
        {
            for (var i = 0; i < rooms.Count - 1; i++)
            {
                var corridor = new SingleWidthCorridor();
                corridor.AddNode(rooms[i].MapRect.Center);
                corridor.AddNode(rooms[i + 1].MapRect.Center);
                corridor.SetTiles(_map);
            }
        }

        private void SpawnPlayerUnits(TileMap map, Room entryRoom)
        {
            var tile1 = map.GetTile(entryRoom.TranslateLocalTileCoords(2, 1));
            var tile2 = map.GetTile(entryRoom.TranslateLocalTileCoords(2, 2));
            var tile3 = map.GetTile(entryRoom.TranslateLocalTileCoords(2, 3));
            var tile4 = map.GetTile(entryRoom.TranslateLocalTileCoords(3, 2));
            UnitSpawner.Instance.SpawnUnit("test_player", tile1);
            UnitSpawner.Instance.SpawnUnit("test_player", tile2);
            UnitSpawner.Instance.SpawnUnit("test_player", tile3);
            UnitSpawner.Instance.SpawnUnit("test_player", tile4);
        }

        private void SpawnBadGuys(TileMap map, Room room)
        {
            if (EnemyType == null)
            {
                return;
            }

            var placed = 0;
            var remainingTiles = new List<IntVector2>(room.Tiles);
            var tileCount = remainingTiles.Count;
            var unitsToPlace = UnityEngine.Random.Range(MinUnitsPerRoom, MaxUnitsPerRoom + 1);
            while (placed < unitsToPlace && tileCount > 0)
            {
                var tileIdx = UnityEngine.Random.Range(0, tileCount);
                var tile = map.GetTile(remainingTiles[tileIdx]);
                UnitSpawner.Instance.SpawnUnit(EnemyType.ID, tile);
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
