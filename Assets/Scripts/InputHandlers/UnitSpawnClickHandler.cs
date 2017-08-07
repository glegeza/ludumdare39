namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    class UnitSpawnClickHandler : MapClickInputHandler
    {
        public UnitSpawnClickHandler() : base("spawn", "Unit Spawning")
        { }

        public override bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public override bool HandleTileClick(int button, Tile clickedTile)
        {
            if (button == 0)
            {
                UnitSpawner.Instance.SpawnTestUnit("test_player", clickedTile);
            }
            else if (button == 1)
            {
                var unit = ActiveUnits.Instance.GetUnitAtTile(clickedTile);
                if (unit != null)
                {
                    ActiveUnits.Instance.RemoveUnit(unit);
                }
            }
            return false;
        }
    }
}
