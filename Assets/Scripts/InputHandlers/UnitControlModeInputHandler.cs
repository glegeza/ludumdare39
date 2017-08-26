namespace DLS.LD39.InputHandlers
{
    using Map;
    using Pathfinding;
    using Units;
    using UnityEngine;

    public class UnitControlModeInputHandler : MapClickInputHandler
    {
        public UnitControlModeInputHandler() : base("control", "Normal")
        { }

        public override bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public override bool HandleTileClick(int button, Tile clickedTile)
        {
            var activeObject = ActiveSelectionTracker.Instance.SelectedObject;
            if (activeObject == null)
            {
                return false;
            }

            var activeUnit = activeObject.GetComponent<GameUnit>();
            if (activeUnit == null)
            {
                return false;
            }

            if (!activeUnit.Ready || activeUnit.Faction != Faction.Player)
            {
                return false;
            }

            if (button == 0)
            {
                DoMoveAction(activeUnit, clickedTile);
            }
            if (button == 1)
            {
                var target = GetUnitTarget(clickedTile);
                if (activeUnit.CurrentTarget != target)
                {
                    activeUnit.CurrentTarget = target;
                    return false;
                }
            }

            return false;
        }

        private GameUnit GetUnitTarget(Tile clickedTile)
        {
            var target = ActiveUnits.Instance.GetUnitAtTile(clickedTile);
            if (target == null)
            {
                return null;
            }
            return target.Faction != Faction.Aliens ? null : target;
        }

        private void DoMoveAction(GameUnit activeUnit, Tile clickedTile)
        {
            var pathfinder = activeUnit.GetComponent<UnitPathfinder>();
            if (pathfinder == null)
            {
                Debug.LogError("Active unit missing UnitPathFinder");
                return;
            }

            if (Equals(clickedTile, pathfinder.Target))
            {
                pathfinder.StartMove();
            }
            else
            {
                pathfinder.SetTarget(clickedTile);
            }
        }
    }
}
