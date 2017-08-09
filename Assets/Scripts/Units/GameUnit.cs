namespace DLS.LD39.Units
{
    using DLS.LD39.AI;
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using System;
    using System.Linq;
    using UnityEngine;

    public class GameUnit : MonoBehaviour
    {
        private bool _endOfTurnPending = false;
        private bool _inTurn = false;

        public event EventHandler<EventArgs> TurnBegan;

        public event EventHandler<EventArgs> TurnEnded;

        public string UnitType
        {
            get; private set;
        }

        public string Name
        {
            get; private set;
        }

        public Faction Faction
        {
            get; private set;
        }

        public bool Ready
        {
            get
            {
                return _inTurn && !MoveController.IsMoving;
            }
        }

        public TilePosition Position
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

        public Combat CombatInfo
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

        public void Initialize(UnitData data, Tile startPos, string name)
        {
            if (startPos == null)
            {
                throw new ArgumentNullException("startPos");
            }
            Stats = new PrimaryStats(data.Stats);
            SecondaryStats = new SecondaryStats(Stats);
            AP.Initialize(this);
            CombatInfo.Initialize(this);
            Position.SetTile(startPos);
            MoveController.Initialize(this);
            PathController.Initialize(this);
            AnimationController.Initialize(this);
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
            ActiveSelectionTracker.Instance.SetSelection(this);
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
            Position = gameObject.AddComponent<TilePosition>();
            AP = gameObject.AddComponent<ActionPoints>();
            MoveController = gameObject.AddComponent<MoveController>();
            PathController = gameObject.AddComponent<UnitPathfinder>();
            CombatInfo = gameObject.AddComponent<Combat>();
            AnimationController = gameObject.AddComponent<UnitAnimationController>();

            PathController.TurnMoveComplete += OnFinishedEndOfTurnMove;
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
