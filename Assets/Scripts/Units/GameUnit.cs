namespace DLS.LD39.Units
{
    using DLS.LD39.AI;
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using DLS.LD39.Units.Actions;
    using System;
    using System.Linq;
    using UnityEngine;

    public class GameUnit : MonoBehaviour
    {
        private bool _endOfTurnPending = false;
        private bool _inTurn = false;
        private GameUnit _currentTarget;

        public event EventHandler<EventArgs> TurnBegan;

        public event EventHandler<EventArgs> TurnEnded;

        public event EventHandler<EventArgs> UnitDestroyed;

        public event EventHandler<EventArgs> TargetChanged;

        public string UnitType
        {
            get; private set;
        }

        public string Name
        {
            get; private set;
        }

        public GameUnit CurrentTarget
        {
            get
            {
                return _currentTarget;
            }
            set
            {
                _currentTarget = value;
                TargetChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public UnitData Data
        {
            get; private set;
        }

        public Faction Faction
        {
            get; private set;
        }

        public bool Alive
        {
            get; private set;
        }

        public bool Ready
        {
            get
            {
                return _inTurn &&
                    !MoveController.IsMoving &&
                    !CombatInfo.Attacking &&
                    !RangedCombat.ActionInProgress;
            }
        }

        public TilePosition Position
        {
            get; private set;
        }

        public UnitFacing Facing
        {
            get; private set;
        }

        public ActionPoints AP
        {
            get; private set;
        }

        public MoveController MoveController
        {
            get; private set;
        }

        public UnitPathfinder PathController
        {
            get; private set;
        }

        public CombatController CombatInfo
        {
            get; private set;
        }

        public RangedCombatAction RangedCombat
        {
            get; private set;
        }

        public PrimaryStats Stats
        {
            get; private set;
        }

        public SecondaryStats SecondaryStats
        {
            get; private set;
        }

        public StateController Controller
        {
            get; private set;
        }

        public UnitAnimationController AnimationController
        {
            get; private set;
        }

        public Visibility Visibility
        {
            get; private set;
        }

        public void Initialize(UnitData data, Tile startPos, string name)
        {
            if (startPos == null)
            {
                throw new ArgumentNullException("startPos");
            }
            Data = data;
            Stats = new PrimaryStats(data.StatsGenerator);
            SecondaryStats = new SecondaryStats(Stats);
            AP.Initialize(this);
            CombatInfo.Initialize(this);
            Position.SetTile(startPos);
            MoveController.Initialize(this);
            PathController.Initialize(this);
            AnimationController.Initialize(this);
            Visibility.Initialize(this);
            Facing.Initialize(this);
            RangedCombat.Initialize(this);
            Faction = data.Faction;
            UnitType = data.ID;
            Name = name;
        }

        public void SetController(StateController controller)
        {
            Controller = controller;
        }

        public void BeginTurn()
        {
            _inTurn = true;
            AP.BeginTurn();
            MoveController.BeginTurn();
            PathController.BeginTurn();
            if (Controller != null)
            {
                Controller.BeginTurn();
            }
        }

        public void EndTurn()
        {
            if (Controller != null)
            {
                Controller.EndTurn();
            }
            if (PathController.Path.Any())
            {
                _endOfTurnPending = true;
                PathController.StartMove();
            }
            else
            {
                OnTurnEnd();
            }
        }

        private void Awake()
        {
            Alive = true;
            Position = gameObject.AddComponent<TilePosition>();
            AP = gameObject.AddComponent<ActionPoints>();
            MoveController = gameObject.AddComponent<MoveController>();
            PathController = gameObject.AddComponent<UnitPathfinder>();
            CombatInfo = gameObject.AddComponent<CombatController>();
            AnimationController = gameObject.AddComponent<UnitAnimationController>();
            Facing = gameObject.AddComponent<UnitFacing>();
            Visibility = gameObject.AddComponent<Visibility>();
            RangedCombat = gameObject.AddComponent<RangedCombatAction>();

            PathController.TurnMoveComplete += OnFinishedEndOfTurnMove;
            CombatInfo.Destroyed += (o, e) => 
            {
                Alive = false;
                UnitDestroyed?.Invoke(this, EventArgs.Empty);
            };
        }

        private void OnFinishedEndOfTurnMove(object sender, EventArgs e)
        {
            if (_endOfTurnPending)
            {
                OnTurnEnd();
            }
        }

        private void OnTurnEnd()
        {
            _endOfTurnPending = false;
            _inTurn = false;
            AP.EndTurn();
            MoveController.EndTurn();
            PathController.EndTurn();

            TurnEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
