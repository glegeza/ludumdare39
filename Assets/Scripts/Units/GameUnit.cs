namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using UnityEngine;

    class GameUnit : MonoBehaviour
    {
        public event EventHandler<EventArgs> TurnEnded;

        public string UnitType
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

        public MoveToTile MoveController
        {
            get; private set;
        }

        public UnitRenderer Renderer
        {
            get; private set;
        }

        public void Initialize(Tile startPos, Faction faction, string unitType)
        {
            if (startPos == null)
            {
                throw new ArgumentNullException("startPos");
            }
            Position.SetTile(startPos);
            MoveController.Initialize(this);
            UnitType = unitType;
        }

        public void BeginTurn()
        {
            Initiative.BeginTurn();
            AP.BeginTurn();
            MoveController.BeginTurn();
            Renderer.BeginTurn();
        }

        public void EndTurn()
        {
            Initiative.EndTurn();
            AP.EndTurn();
            MoveController.EndTurn();
            Renderer.EndTurn();

            if (TurnEnded != null)
            {
                TurnEnded(this, EventArgs.Empty);
            }
        }

        private void Awake()
        {
            Position = gameObject.AddComponent<TilePosition>();
            AP = gameObject.AddComponent<ActionPoints>();
            Initiative = gameObject.AddComponent<Initiative>();
            MoveController = gameObject.AddComponent<MoveToTile>();
            Renderer = gameObject.AddComponent<UnitRenderer>();
        }
    }
}
