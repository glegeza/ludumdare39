namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using DLS.LD39.Units.Actions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Allows a game unit to move from tile to tile. This class is only used
    /// to move from one tile to an adjacent tile.
    /// </summary>
    public class MoveAction : UnitAction
    {
        private GameUnit _unit;
        private TilePosition _position;
        private float _inverseMoveTime;
        private float _moveTime;

        /// <summary>
        /// The number of seconds it takes a unit to move to an adjacent tile.
        /// </summary>
        public float MoveAnimationTime
        {
            get
            {
                return _moveTime;
            }
            set
            {
                _moveTime = value;
                _moveTime = Mathf.Max(0.0f, _moveTime);
                _inverseMoveTime = 1.0f / _moveTime;
            }
        }

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
            StartCoroutine(DoMovement(target));
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
            MoveAnimationTime = 1.0f;
            _unit = unit;
            _position = _unit.Position;
        }

        private IEnumerator DoMovement(Tile target)
        {
            _position.SetTile(target, false);
            AttachedUnit.AnimationController.StartWalkAnimation();
            var end = new Vector3(target.WorldCoords.x, target.WorldCoords.y, transform.position.z);

            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostion = Vector3.MoveTowards(transform.position, end, _inverseMoveTime * Time.deltaTime);
                transform.position = newPostion;
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                yield return null;
            }

            CompleteAction(EventArgs.Empty);
            AttachedUnit.AnimationController.StartIdleAnimation();
        }
    }
}
