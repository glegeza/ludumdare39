namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;

    class UnitClickMover : IMapClickInputHandler
    {
        public bool HandleTileClick(int button, Tile clickedTile)
        {
            var activeUnit = TurnOrderTracker.Instance.ActiveUnit;
            if (activeUnit == null)
            {
                return false;
            }
            
            activeUnit.GetComponent<PathToTargetController>().SetTarget(clickedTile);

            return false;
        }
    }
}
