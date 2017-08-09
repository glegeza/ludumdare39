namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MoveController : GameUnitComponent
    {
        private Animator _animator;
        private GameUnit _unit;
        private TilePosition _position;
        private float _inverseMoveTime;
        private float _moveTime;

        public event EventHandler<EventArgs> StartedMovement;

        public event EventHandler<EventArgs> CompletedMovement;

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

        public bool IsMoving
        {
            get; private set;
        }

        public MoveResult TryMove(Tile target)
        {
            if (IsMoving)
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
            if (!_unit.AP.CanSpendPoints(cost))
            {
                return MoveResult.NotEnoughAP;
            }

            _unit.AP.SpendPoints(cost);
            StartCoroutine(DoMovement(target));
            return MoveResult.ValidMove;
        }

        public int GetMaxMoveAlongPath(IEnumerable<Tile> path)
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
            _animator = GetComponent<Animator>();
        }

        private IEnumerator DoMovement(Tile target)
        {
            AttachedUnit.AnimationController.StartWalkAnimation();
            var end = new Vector3(target.WorldCoords.x, target.WorldCoords.y, transform.position.z);
            StartedMovement?.Invoke(this, EventArgs.Empty);
            IsMoving = true;

            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostion = Vector3.MoveTowards(transform.position, end, _inverseMoveTime * Time.deltaTime);
                transform.position = newPostion;
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                yield return null;
            }

            _position.SetTile(target, false);
            IsMoving = false;
            AttachedUnit.AnimationController.StartIdleAnimation();

            CompletedMovement?.Invoke(this, EventArgs.Empty);
        }
    }
}
