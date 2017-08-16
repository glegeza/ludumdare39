namespace DLS.LD39.Units
{
    using DLS.LD39.AI;
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using DLS.LD39.Units.Actions;
    using System;
    using System.Linq;
    using UnityEngine;
    using Data;
    using System.Collections.Generic;

    public class GameUnit : MonoBehaviour
    {
        private bool _endOfTurnPending = false;
        private bool _inTurn = false;
        private GameUnit _currentTarget;
        private List<GameUnitComponent> _components = new List<GameUnitComponent>();

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
                    !Move.ActionInProgress &&
                    !MeleeCombatAction.ActionInProgress &&
                    !RangedCombatAction.ActionInProgress;
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

        public UnitPathfinder PathController
        {
            get; private set;
        }

        public UnitEquipment Equipment
        {
            get; private set;
        }

        public CombatController CombatInfo
        {
            get; private set;
        }

        public MoveAction Move
        {
            get; private set;
        }

        public RangedCombatAction RangedCombatAction
        {
            get; private set;
        }

        public MeleeCombatAction MeleeCombatAction
        {
            get; private set;
        }

        public PrimaryStats PrimaryStats
        {
            get; private set;
        }

        public SecondaryStats SecondaryStats
        {
            get; private set;
        }

        public StateController AIController
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
            PrimaryStats = new PrimaryStats(data.StatsGenerator);
            SecondaryStats = new SecondaryStats(PrimaryStats);

            Position.SetTile(startPos);
            foreach (var comp in _components)
            {
                comp.Initialize(this);
            }

            Faction = data.Faction;
            UnitType = data.ID;
            Name = name;
        }

        public void SetController(StateController controller)
        {
            AIController = controller;
        }

        public void BeginTurn()
        {
            _inTurn = true;

            foreach (var comp in _components)
            {
                comp.BeginTurn();
            }
            
            if (AIController != null)
            {
                AIController.BeginTurn();
            }
        }

        public void EndTurn()
        {
            if (AIController != null)
            {
                AIController.EndTurn();
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

            CreateComponents();

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
            foreach (var comp in _components)
            {
                comp.EndTurn();
            }

            TurnEnded?.Invoke(this, EventArgs.Empty);
        }

        private void CreateComponents()
        {
            Position = gameObject.AddComponent<TilePosition>();
            AP = gameObject.AddComponent<ActionPoints>();
            Move = gameObject.AddComponent<MoveAction>();
            PathController = gameObject.AddComponent<UnitPathfinder>();
            CombatInfo = gameObject.AddComponent<CombatController>();
            AnimationController = gameObject.AddComponent<UnitAnimationController>();
            Facing = gameObject.AddComponent<UnitFacing>();
            Visibility = gameObject.AddComponent<Visibility>();
            RangedCombatAction = gameObject.AddComponent<RangedCombatAction>();
            MeleeCombatAction = gameObject.AddComponent<MeleeCombatAction>();
            Equipment = gameObject.AddComponent<UnitEquipment>();

            _components = new List<GameUnitComponent>()
            {
                AP, Move, PathController, CombatInfo, AnimationController,
                Visibility, Facing, RangedCombatAction, MeleeCombatAction, Equipment
            };
        }
    }
}
