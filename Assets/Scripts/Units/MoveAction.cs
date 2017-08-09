namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MoveAction : MonoBehaviour
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

        public void Initialize(GameUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException("unit");
            }
            MoveAnimationTime = 1.0f;
            _unit = unit;
            _position = _unit.Position;
            _animator = GetComponent<Animator>();
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
            StartCoroutine(DoMovement(_position.TileWorldPosition, target));
            return MoveResult.ValidMove;
        }

        public int GetMaxMoveAlongPath(IEnumerable<Tile> path)
        {
            var curMove = 0;
            var apLeft = _unit.AP.PointsRemaining;
            var lastStep = _position.CurrentTile;
            foreach (var nextStep in path)
            {
                var cost = lastStep.GetMoveCost(nextStep);
                if (cost > apLeft)
                {
                    break;
                }
                apLeft -= cost;
                lastStep = nextStep;
                curMove++;
            }

            return curMove;
        }

        private IEnumerator DoMovement(Vector3 end, Tile target)
        {
            StartedMovement?.Invoke(this, EventArgs.Empty);

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

            CompletedMovement?.Invoke(this, EventArgs.Empty);
        }
    }
}
