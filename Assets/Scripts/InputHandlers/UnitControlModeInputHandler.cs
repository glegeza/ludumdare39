namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Combat;
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using DLS.LD39.Units;
    using UnityEngine;

    class UnitControlModeInputHandler : MapClickInputHandler
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
                var target = GetUnitTarget(activeUnit, clickedTile);
                if (activeUnit.CurrentTarget != target)
                {
                    activeUnit.CurrentTarget = target;
                    return false;
                }
                if (target != null)
                {
                    DoAttackAction(activeUnit, target, clickedTile);
                }
            }

            return false;
        }

        private GameUnit GetUnitTarget(GameUnit activeUnit, Tile clickedTile)
        {
            var target = ActiveUnits.Instance.GetUnitAtTile(clickedTile);
            if (target == null)
            {
                return null;
            }
            if (target.Faction != Faction.Aliens)
            {
                return null;
            }
            return target;
        }

        private void DoAttackAction(GameUnit activeUnit, GameUnit target, Tile clickedTile)
        {
            AttackResult damage;
            if (activeUnit.Position.CurrentTile.IsAdjacent(target.Position.CurrentTile))
            {
                activeUnit.CombatInfo.TryMeleeAttack(clickedTile, target.CombatInfo, out damage);
            }
            else
            {
                activeUnit.RangedCombat.TryRangedAttack(clickedTile, target.CombatInfo);
            }
        }

        private void DoMoveAction(GameUnit activeUnit, Tile clickedTile)
        {
            var pathfinder = activeUnit.GetComponent<UnitPathfinder>();
            if (pathfinder == null)
            {
                Debug.LogError("Active unit missing UnitPathFinder");
                return;
            }

            if (clickedTile == pathfinder.Target)
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
