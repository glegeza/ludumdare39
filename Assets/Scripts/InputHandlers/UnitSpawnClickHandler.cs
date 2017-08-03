namespace DLS.LD39.InputHandlers
{
    using System;
    using DLS.LD39.Map;

    class UnitSpawnClickHandler : IMapClickInputHandler
    {
        public bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public bool HandleTileClick(int button, Tile clickedTile)
        {
            if (button == 0)
            {
                UnitSpawner.Instance.SpawnTestUnit(clickedTile);
            }
            else if (button == 1)
            {
                var unit = UnitSpawner.Instance.UnitAtTile(clickedTile);
                if (unit != null)
                {
                    UnitSpawner.Instance.RemoveUnit(unit);
                }
            }
            return false;
        }
    }
}
