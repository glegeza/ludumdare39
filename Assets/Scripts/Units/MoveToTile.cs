namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Pathfinding;

    class MoveToTile : MonoBehaviour, IGameUnitComponent
    {
        public enum MoveResult
        {
            Success,
            Blocked,
            NoAP
        }

        public float MoveTimer = 0.25f;

        private GameUnit _unit;
        private TilePosition _position;
        private Queue<Tile> _path = new Queue<Tile>();
        private float _timeSinceLastMove = 0.0f;
        private SimplePathfinder _pathFinder = new SimplePathfinder();
        private Tile _target;
        private bool _outOfAP;

        public event EventHandler<EventArgs> UnitArrived;

        public event EventHandler<EventArgs> UnitBlocked;

        public bool HasQueuedMovement
        {
            get
            {
                return _path.Any();
            }
        }

        public void Initialize(GameUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException("unit");
            }
            _unit = unit;
            _position = _unit.Position;
            _outOfAP = false;
        }

        public bool SetNewTarget(Tile target)
        {
            _path.Clear();
            _path = _pathFinder.GetPath(_position.CurrentTile, target);
            _target = target;
            return _path.Any();
        }

        public MoveResult Move(Tile target)
        {
            var cost = GetTileCost(target);
            var validMove = TileIsValid(target);

            if (validMove && _unit.AP.CanSpendPoints(cost))
            {
                _unit.AP.SpendPoints(cost);
                _position.SetTile(target);
                _timeSinceLastMove = 0.0f;
                Debug.Log("Moving!");
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

        public void BeginTurn()
        {
            _timeSinceLastMove = 0.0f;
            _outOfAP = false;
        }

        public void EndTurn() { }

        private void Update()
        {
            if (_outOfAP || !_unit.Initiative.IsActiveUnit || !_path.Any())
            {
                return;
            }
            
            _timeSinceLastMove += Time.deltaTime;
            if (_timeSinceLastMove > MoveTimer)
            {
                TakeStepOnPath();
            }
        }

        private void TakeStepOnPath()
        {
            
            var nextMove = _path.Peek();
            var result = Move(nextMove);
            if (result == MoveResult.Blocked)
            {
                DestinationUnreachable();                
            }
            else if (result == MoveResult.NoAP)
            {
                _outOfAP = true;
                return;
            }
            else
            {
                _path.Dequeue();
            }

            if (_position.CurrentTile.Equals(_target))
            {
                ArrivedAtDestination();
            }
        }

        private void ArrivedAtDestination()
        {
            Debug.Log("Arrived!");
            _path.Clear();
            _timeSinceLastMove = 0.0f;
            _target = null;
            if (UnitArrived != null)
            {
                UnitArrived(this, EventArgs.Empty);
            }
        }

        private void DestinationUnreachable()
        {
            Debug.Log("Destination unreachable!");
            _path.Clear();
            _timeSinceLastMove = 0.0f;
            _target = null;
            if (UnitBlocked != null)
            {
                UnitBlocked(this, EventArgs.Empty);
            }
        }

        private int GetTileCost(Tile target)
        {
            return (target.X == _position.CurrentTile.X || target.Y == _position.CurrentTile.Y)
                ? 10
                : 14;
        }

        private bool TileIsValid(Tile target)
        {
            return target.IsAdjacent(_position.CurrentTile) && target.IsEnterable();
        }
    }
}
