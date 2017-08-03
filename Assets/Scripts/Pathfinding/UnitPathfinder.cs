namespace DLS.LD39.Pathfinding
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    class UnitPathfinder : MonoBehaviour
    {
        private TilePosition _position;
        private MoveAction _mover;
        private Tile _target;

        private Queue<Tile> _path = new Queue<Tile>();
        private SimplePathfinder _pathFinder = new SimplePathfinder();
        private float _timeSinceLastMove = 0.0f;
        private bool _moving = false;

        public event EventHandler<EventArgs> TurnMoveComplete;

        public event EventHandler<EventArgs> UnitArrived;

        public event EventHandler<EventArgs> UnitBlocked;

        public event EventHandler<EventArgs> PathChanged;

        public float MoveTimer
        {
            get; private set;
        }

        public IEnumerable<Tile> Path
        {
            get
            {
                return _path;
            }
        }

        public void Initialize(TilePosition position, MoveAction mover)
        {
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }
            if (mover == null)
            {
                throw new ArgumentNullException("mover");
            }

            _position = position;
            _mover = mover;
            MoveTimer = 0.25f;
        }

        public bool SetTarget(Tile target)
        {
            if (_position == null || target == null || 
                _position.CurrentTile.Equals(target))
            {
                return false;
            }

            _path.Clear();
            _path = _pathFinder.GetPath(_position.CurrentTile, target);
            _target = target;

            if (!_path.Any())
            {
                Debug.LogFormat("No path from {0} to {1}", 
                    _position.CurrentTile, target);
            }

            OnPathChanged();
            return _path.Any();
        }

        public void BeginTurn()
        {
            OnPathChanged();
        }

        public void StartMove()
        {
            if (_position == null || _target == null || !_path.Any())
            {
                MoveCompleted(); ;
                return;
            }

            _moving = true;
        }

        private void Update()
        {
            if (!_moving || _mover == null)
            {
                return;
            }

            _timeSinceLastMove += Time.deltaTime;
            if (_timeSinceLastMove > MoveTimer)
            {
                TakeNextStepOnPath();
                _timeSinceLastMove = 0.0f;
            }
        }

        private void TakeNextStepOnPath()
        {
            var nextStep = _path.Peek();
            var result = _mover.Move(nextStep);
            if (result == MoveAction.MoveResult.Blocked)
            {
                DestinationUnreachable();
                MoveCompleted();
            }
            else if (result == MoveAction.MoveResult.NoAP)
            {
                MoveCompleted();
            }
            else
            {
                _path.Dequeue();
                OnPathChanged();
            }

            if (_position.CurrentTile.Equals(_target))
            {
                ArrivedAtDestination();
                MoveCompleted();
            }
        }

        private void MoveCompleted()
        {
            Debug.Log("Move completed");
            _moving = false;
            if (TurnMoveComplete != null)
            {
                TurnMoveComplete(this, EventArgs.Empty);
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

        private void OnPathChanged()
        {
            if (PathChanged != null)
            {
                PathChanged(this, EventArgs.Empty);
            }
        }
    }
}
