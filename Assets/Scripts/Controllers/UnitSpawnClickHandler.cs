namespace DLS.LD39.Controllers
{
    using DLS.LD39.Map;

    class UnitSpawnClickHandler : IMapClickInputHandler
    {
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
