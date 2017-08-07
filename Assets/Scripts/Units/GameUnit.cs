namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using System;
    using System.Linq;
    using UnityEngine;

    class GameUnit : MonoBehaviour
    {
        private bool _endOfTurnPending = false;

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

        public TilePosition Position
        {
            get; private set;
        }

        public ActionPoints AP
        {
            get; private set;
        }

        public Initiative Initiative
        {
            get; private set;
        }

        public MoveAction MoveController
        {
            get; private set;
        }

        public UnitPathfinder PathController
        {
            get; private set;
        }

        public UnitRenderer Renderer
        {
            get; private set;
        }

        public Combat CombatInfo
        {
            get; private set;
        }

        public Stats Stats
        {
            get; private set;
        }

        public void Initialize(UnitData data, Tile startPos, string name)
        {
            if (startPos == null)
            {
                throw new ArgumentNullException("startPos");
            }
            Stats.Initialize(data.Stats);
            AP.Initialize(this);
            Initiative.Initialize(this);
            CombatInfo.Initialize(this);
            Position.SetTile(startPos);
            MoveController.Initialize(this);
            PathController.Initialize(Position, MoveController);
            UnitType = data.ID;
            Name = name;
        }

        public void BeginTurn()
        {
            Initiative.BeginTurn();
            AP.BeginTurn();
            MoveController.BeginTurn();
            Renderer.BeginTurn();
            PathController.BeginTurn();
            ActiveSelectionTracker.Instance.SetSelection(this);
        }

        public void EndTurn()
        {
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
            Initiative = gameObject.AddComponent<Initiative>();
            MoveController = gameObject.AddComponent<MoveAction>();
            Renderer = gameObject.AddComponent<UnitRenderer>();
            PathController = gameObject.AddComponent<UnitPathfinder>();
            Stats = gameObject.AddComponent<Stats>();
            CombatInfo = gameObject.AddComponent<Combat>();

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
            Initiative.EndTurn();
            AP.EndTurn();
            MoveController.EndTurn();
            Renderer.EndTurn();

            if (TurnEnded != null)
            {
                TurnEnded(this, EventArgs.Empty);
            }
        }
    }
}
