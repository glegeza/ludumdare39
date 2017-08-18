namespace DLS.LD39.Pathfinding
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using DLS.Utility;

    public class UnitPathfinder : GameUnitComponent
    {
        private Tile _target;

        private Queue<Tile> _path = new Queue<Tile>();
        private SimplePathfinder _pathFinder = new SimplePathfinder();
        private float _timeSinceLastMove = 0.0f;
        private bool _moving = false;

        public event EventHandler<EventArgs> StartPathMovement;

        public event EventHandler<EventArgs> TurnMoveComplete;

        public event EventHandler<EventArgs> UnitArrived;

        public event EventHandler<EventArgs> UnitBlocked;

        public event EventHandler<EventArgs> PathChanged;

        public float MoveTimer
        {
            get; private set;
        }

        public Tile Target
        {
            get
            {
                return _target;
            }
        }

        public IEnumerable<Tile> Path
        {
            get
            {
                return _path;
            }
        }

        public bool SetTarget(Tile target)
        {
            if (target == null || 
                AttachedUnit.Position.CurrentTile.Equals(target))
            {
                return false;
            }

            _path.Clear();
            int cost;
            _path = _pathFinder.GetPath(AttachedUnit.Position.CurrentTile, target, out cost);
            _target = target;
            OnPathChanged();

            return _path.Any();
        }

        public void StartMove()
        {
            if (_target == null || !_path.Any())
            {
                MoveCompleted();
                return;
            }

            StartPathMovement.SafeRaiseEvent(this);

            _moving = true;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            AttachedUnit.MoveAction.CompletedAction += OnUnitCompletedMovement;
        }

        private void OnUnitCompletedMovement(object sender, EventArgs e)
        {
            if (_moving)
            {
                _path.Dequeue();
                OnPathChanged();
                CheckPathState();
            }
        }

        protected override void OnTurnStarted()
        {
            OnPathChanged();
        }

        private void Update()
        {
            if (!_moving || AttachedUnit.MoveAction.ActionInProgress)
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
            var result = AttachedUnit.MoveAction.TryMove(nextStep);
            if (result == MoveResult.Blocked)
            {
                AttemptToRecalculatePath();
            }
            else if (result == MoveResult.NotEnoughAP)
            {
                MoveCompleted();
            }
        }

        private void AttemptToRecalculatePath()
        {
            var oldTarget = _target;
            _target = null;
            var targetSet = SetTarget(oldTarget);
            if (!targetSet)
            {
                DestinationUnreachable();
                MoveCompleted();
            }
        }

        private void MoveCompleted()
        {
            _moving = false;
            TurnMoveComplete.SafeRaiseEvent(this);
        }

        private void ArrivedAtDestination()
        {
            _path.Clear();
            _timeSinceLastMove = 0.0f;
            _target = null;
            UnitArrived.SafeRaiseEvent(this);
        }

        private void DestinationUnreachable()
        {
            _path.Clear();
            _timeSinceLastMove = 0.0f;
            _target = null;
            UnitBlocked.SafeRaiseEvent(this);
        }

        private void OnPathChanged()
        {
            RaiseEvent(PathChanged, this, EventArgs.Empty);
        }

        private void CheckPathState()
        {
            if (AttachedUnit.Position.CurrentTile.Equals(_target))
            {
                ArrivedAtDestination();
                MoveCompleted();
            }
        }

        private void RaiseEvent(EventHandler<EventArgs> handler, object s, EventArgs e)
        {
            if (handler != null)
            {
                handler(s, e);
            }
        }
    }
}
