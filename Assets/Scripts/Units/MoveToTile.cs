namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Pathfinding;

    [RequireComponent(typeof(TilePosition))]
    [RequireComponent(typeof(GameUnit))]
    public class MoveToTile : MonoBehaviour, IGameUnitComponent
    {
        public float MoveTimer = 1.0f;

        private GameUnit _unit;
        private TilePosition _position;
        private Queue<Tile> _path = new Queue<Tile>();
        private float _timeSinceLastMove = 0.0f;
        private SimplePathfinder _pathFinder = new SimplePathfinder();
        private Tile _target;

        public event EventHandler<EventArgs> UnitArrived;

        public event EventHandler<EventArgs> UnitBlocked;

        public bool HasQueuedMovement
        {
            get
            {
                return _path.Any();
            }
        }

        public bool SetNewTarget(Tile target)
        {
            _path.Clear();
            _path = _pathFinder.GetPath(_position.CurrentTile, target);
            return _path.Any();
        }

        public void BeginTurn()
        {
            _timeSinceLastMove = 0.0f;
        }

        public void EndTurn() { }

        private void Start()
        {
            _unit = GetComponent<GameUnit>();
            var tileMap = FindObjectOfType<TileMap>();
            _position = GetComponent<TilePosition>();
            _position.SetTile(tileMap.GetTile(0, 0));
        }

        private void Update()
        {
            if (!_unit.Init.IsActiveUnit || !_path.Any())
            {
                return;
            }
            

            _timeSinceLastMove += Time.deltaTime;
            if (_timeSinceLastMove > MoveTimer)
            {
                TakeStep();
            }
        }

        private void TakeStep()
        {
            
            var nextMove = _path.Peek();
            if (TileIsValid(nextMove))
            {
                _position.SetTile(nextMove);
                _timeSinceLastMove = 0.0f;
                if (nextMove.Equals(_target))
                {
                    ArrivedAtDestination();
                    _path.Clear();
                }
            }
            else
            {
                _path.Clear();
                _timeSinceLastMove = 0.0f;
                DestinationUnreachable();
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
            return Mathf.Abs(target.X - _position.CurrentTile.X) <= 1 &&
                Mathf.Abs(target.Y - _position.CurrentTile.Y) <= 1 &&
                target.Passable &&
                GetTileCost(target) <= _unit.AP.PointsRemaining;
        }

        private void ArrivedAtDestination()
        {
            if (UnitArrived != null)
            {
                UnitArrived(this, EventArgs.Empty);
            }
        }

        private void DestinationUnreachable()
        {
            if (UnitBlocked != null)
            {
                UnitBlocked(this, EventArgs.Empty);
            }
        }
    }
}
