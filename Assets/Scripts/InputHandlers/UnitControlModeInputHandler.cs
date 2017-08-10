namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;

    class UnitControlModeInputHandler : MapClickInputHandler
    {
        public UnitControlModeInputHandler() : base("move", "Move")
        { }

        public override bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public override bool HandleTileClick(int button, Tile clickedTile)
        {
            if (button != 0)
            {
                return false;
            }
            var activeUnit = TurnOrderTracker.Instance.ActiveUnit;
            if (activeUnit == null || activeUnit.Faction != Units.Faction.Player)
            {
                return false;
            }

            var pathfinder = activeUnit.GetComponent<UnitPathfinder>();
            if (pathfinder == null)
            {
                return false;
            }

            if (clickedTile == pathfinder.Target)
            {
                pathfinder.StartMove();
            }
            else
            {
                pathfinder.SetTarget(clickedTile);
            }

            return false;
        }
    }
}
