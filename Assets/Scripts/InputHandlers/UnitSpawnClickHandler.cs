namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    class UnitSpawnClickHandler : MapClickInputHandler
    {
        public UnitSpawnClickHandler() : base("spawn")
        { }

        public override bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public override bool HandleTileClick(int button, Tile clickedTile)
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
