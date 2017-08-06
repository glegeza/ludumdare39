namespace DLS.LD39.InputHandlers
{
    using System;
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;

    class UnitClickMover : MapClickInputHandler
    {
        public UnitClickMover() : base("move")
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
            if (activeUnit == null)
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
