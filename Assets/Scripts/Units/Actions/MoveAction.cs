namespace DLS.LD39.Units.Actions
{
    using DLS.LD39.Map;
    using System;
    using Units.Movement;

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

        protected override void OnInitialized(GameUnit unit)
        {
            _unit = unit;
            _position = _unit.Position;
        }
    }
}
