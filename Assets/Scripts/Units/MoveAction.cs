namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MoveAction : MonoBehaviour, IGameUnitComponent
    {
        public enum MoveResult
        {
            Success,
            Blocked,
            NoAP
        }

        private Animator _animator;
        private GameUnit _unit;
        private TilePosition _position;
        private float _inverseMoveTime;

        public void Initialize(GameUnit unit, float moveTime=1.0f)
        {
            if (unit == null)
            {
                throw new ArgumentNullException("unit");
            }
            _unit = unit;
            _position = _unit.Position;
            _inverseMoveTime = 1.0f / moveTime;
            _animator = GetComponent<Animator>();
        }

        public MoveResult Move(Tile target)
        {
            var cost = _position.CurrentTile.GetMoveCost(target);
            var validMove = TileIsValid(target);

            if (validMove && _unit.AP.CanSpendPoints(cost))
            {
                _unit.AP.SpendPoints(cost);
                _position.SetTile(target, false);
                Debug.Log("Moving!");
                if (_animator)
                {
                    Debug.Log("Animating!");
                    _animator.SetTrigger("walkingStart");
                }
                _unit.Ready = false;
                StartCoroutine(Movement(_position.TileWorldPosition));
                return MoveResult.Success;
            }
            else if (!validMove)
            {
                Debug.Log("Blocked!");
                return MoveResult.Blocked;
            }

            Debug.Log("Out of AP!");
            return MoveResult.NoAP;
        }

        public void BeginTurn() { }

        public void EndTurn() { }

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
        
        private IEnumerator Movement(Vector3 end)
        {
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            
            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostion = Vector3.MoveTowards(transform.position, end, _inverseMoveTime * Time.deltaTime);
                transform.position = newPostion;
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                yield return null;
            }
            if (_animator != null)
            {
                _animator.SetTrigger("idleStart");
            }
            _unit.Ready = true;
        }

        private bool TileIsValid(Tile target)
        {
            return target.IsAdjacent(_position.CurrentTile) && target.IsEnterable();
        }
    }
}
