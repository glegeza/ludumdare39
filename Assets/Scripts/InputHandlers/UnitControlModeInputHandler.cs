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
            var activeUnit = TurnOrderTracker.Instance.ActiveUnit;
            if (activeUnit == null || activeUnit.Faction != Faction.Player)
            {
                return false;
            }

            if (!activeUnit.Ready)
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
            DamageResult damage;
            activeUnit.CombatInfo.TryMeleeAttack(clickedTile, target.CombatInfo, out damage);
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
