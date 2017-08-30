namespace DLS.LD39.Generation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using JetBrains.Annotations;
    using Map;
    using Props;
    using UnityEngine;
    using Utility;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Generates a level with an entry room and exit room on
    /// opposite sides and a generator somewhere in the middle.
    /// </summary>
    public class EntryPowerExitLevelGenerator : MonoBehaviour
    {
        private enum RoomSide
        {
            Left,
            Right,
            Top,
            Bottom
        }

        public EntryPowerExitGenerationSettings Settings;
        public string WallType;

        private TileMap _map;
        private readonly Dictionary<string, Room> _rooms
            = new Dictionary<string, Room>();

        [UsedImplicitly]
        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        [UsedImplicitly]
        private void Start()
        {
            GenerateTileMap();

            PlaceEntryRoom();
            PlaceGeneratorRoom();
            PlaceExitRoom();
            ConnectRooms(_rooms["entry"], RoomSide.Left, _rooms["generator"], RoomSide.Right);

            SpawnPlayerUnits();

            foreach (var tile in _map.Tiles)
            {
                if (tile.Type.ID == "empty" && tile.AdjacentTiles.Any(t => t.Type.ID != "empty"))
                {
                    PropFactory.Instance.BuildPropAndAddToTile(WallType, tile);
                }
            }
        }

        private void GenerateTileMap()
        {
            var width = Random.Range(Settings.MinWidth, Settings.MaxWidth);
            var height = Random.Range(Settings.MinHeight, Settings.MaxHeight);
            _map.Width = width;
            _map.Height = height;
            _map.RebuildMap();
        }

        private void PlaceEntryRoom()
        {
            var maxSize = Settings.EntryRoom.GetMaximumRoomSize();
            var x = _map.Width - maxSize.X;
            var y = Random.Range(0, _map.Height - maxSize.Y);
            var entryRoom = Settings.EntryRoom.GetRoomAtPosition(_map, new IntVector2(x, y), "yellow");
            entryRoom.SetTiles(_map, "yellow");
            _rooms.Add("entry", entryRoom);
        }

        private void PlaceExitRoom()
        {
            var maxSize = Settings.ExitRoom.GetMaximumRoomSize();
            var x = 0;
            var y = Random.Range(0, _map.Height - maxSize.Y);
            var exitRoom = Settings.ExitRoom.GetRoomAtPosition(_map, new IntVector2(x, y), "red");
            exitRoom.SetTiles(_map, "red");
            _rooms.Add("exit", exitRoom);
        }

        private void PlaceGeneratorRoom()
        {
            var maxSize = Settings.GeneratorRoom.GetMaximumRoomSize();
            var startX = _map.Width / 3;
            var endX = startX + (_map.Width / 3);
            var zoneSize = endX - startX;
            if (maxSize.X > zoneSize)
            {
                Debug.LogError("Map too small for proper generator room placement.");
                return;
            }

            var x = Random.Range(startX, endX - maxSize.X);
            var y = Random.Range(0, _map.Height - maxSize.Y);

            var genRoom = Settings.GeneratorRoom.GetRoomAtPosition(_map, new IntVector2(x, y));
            genRoom.SetTiles(_map);
            _rooms.Add("generator", genRoom);
        }

        private void ConnectRooms(Room a, RoomSide sideA, Room b, RoomSide sideB)
        {
            var aTile = GetTileOnSide(a, sideA);
            var bTile = GetTileOnSide(b, sideB);
            var corridor = new SingleWidthCorridor();
            corridor.AddNode(aTile);
            corridor.AddNode(bTile);
            corridor.SetTiles(_map);
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

        private IntVector2 GetTileOnSide(Room room, RoomSide side)
        {
            int x, y;
            var rect = room.MapRect;
            switch (side)
            {
                case RoomSide.Bottom:
                    y = rect.BottomLeft.Y - 1;
                    x = Random.Range(rect.BottomLeft.X, rect.BottomRight.X);
                    break;
                case RoomSide.Top:
                    y = rect.TopLeft.Y + 1;
                    x = Random.Range(rect.TopLeft.X, rect.TopRight.X);
                    break;
                case RoomSide.Left:
                    x = rect.TopLeft.X - 1;
                    y = Random.Range(rect.BottomLeft.Y, rect.TopLeft.Y);
                    break;
                case RoomSide.Right:
                    x = rect.TopRight.X + 1;
                    y = Random.Range(rect.BottomRight.Y, rect.BottomLeft.Y);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("side", side, null);
            }

            return new IntVector2(x, y);
        }
    }
}
