namespace DLS.LD39.Units.Actions
{
    using DLS.LD39.Map;
    using System;
    using Units.Movement;
    using System.Collections.Generic;

    /// <summary>
    /// Allows a game unit to move from tile to tile. This class is only used
    /// to move from one tile to an adjacent tile.
    /// </summary>
    public class MoveAction : UnitAction
    {
        private GameUnit _unit;
        private TilePosition _position;

        /// <summary>
        /// Attempt to move to an adjacent tile.
        /// </summary>
        /// <param name="target">The target tile to move to.</param>
        /// <returns>The result of the move.</returns>
        public MoveResult TryMove(Tile target)
        {
            if (ActionInProgress)
            {
                return MoveResult.NotReady;
            }

            if (!target.IsAdjacent(_position.CurrentTile))
            {
                return MoveResult.InvalidDestination;
            }

            if (!target.IsEnterable())
            {
                return MoveResult.Blocked;
            }

            var cost = _position.CurrentTile.GetMoveCost(target);
            if (!_unit.AP.PointsAvailable(cost))
            {
                return MoveResult.NotEnoughAP;
            }

            AttachedUnit.Facing.FaceTile(target);
            _position.SetTile(target, false);
            var moveController = gameObject.AddComponent<MoveAnimator>();
            moveController.BeginMotion(target);
            moveController.CompletedMovement += (o, e) =>
            {
                CompleteAction(EventArgs.Empty);
            };

            StartAction(EventArgs.Empty, cost);
            return MoveResult.ValidMove;
        }

        /// <summary>
        /// The maximum number of moves this unit can make this turn along
        /// the given path.
        /// </summary>
        /// <param name="path">The path to follow.</param>
        /// <returns>The number of spaces it can move before running out of AP.
        /// May be 0.</returns>
        public int GetMaxMovementThisTurn(IEnumerable<Tile> path)
        {
            var curMove = 0;
            var cost = 0;

            var lastPos = _position.CurrentTile;
            foreach (var tile in path)
            {
                cost += lastPos.GetMoveCost(tile);
                if (cost > _unit.AP.PointsRemaining)
                {
                    break;
                }
                lastPos = tile;
                curMove++;
            }
            
            return curMove;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            _unit = unit;
            _position = _unit.Position;
        }
    }
}
