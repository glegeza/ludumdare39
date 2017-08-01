namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Pathfinding;

    [RequireComponent(typeof(TilePosition))]
    public class MoveToTile : MonoBehaviour
    {
        public float MoveTimer = 1.0f;
        
        private TilePosition _position;
        private Queue<Tile> _path = new Queue<Tile>();
        private float _timeSinceLastMove = 0.0f;
        private SimplePathfinder _pathFinder = new SimplePathfinder();
        private Tile _target;

        public event EventHandler<EventArgs> UnitArrived;

        public event EventHandler<EventArgs> UnitBlocked;

        public bool SetNewTarget(Tile target)
        {
            _path.Clear();
            _path = _pathFinder.GetPath(_position.CurrentTile, target);
            return _path.Any();
        }

        private void Start()
        {
            var tileMap = FindObjectOfType<TileMap>();
            _position = GetComponent<TilePosition>();
            _position.SetTile(tileMap.GetTile(0, 0));
        }

        private void Update()
        {
            if (!_path.Any())
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
            var nextMove = _path.Dequeue();
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

        private bool TileIsValid(Tile target)
        {
            return Mathf.Abs(target.X - _position.CurrentTile.X) <= 1 &&
                Mathf.Abs(target.Y - _position.CurrentTile.Y) <= 1 &&
                target.Passable;
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
