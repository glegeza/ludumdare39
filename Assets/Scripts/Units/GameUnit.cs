namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    [RequireComponent(typeof(ActionPoints))]
    [RequireComponent(typeof(Initiative))]
    [RequireComponent(typeof(MoveToTile))]
    class GameUnit : MonoBehaviour
    {
        public event EventHandler<EventArgs> TurnEnded;

        public TilePosition Position
        {
            get; private set;
        }

        public ActionPoints AP
        {
            get; private set;
        }

        public Initiative Init
        {
            get; private set;
        }

        public MoveToTile MoveController
        {
            get; private set;
        }

        public void BeginTurn()
        {
            Init.BeginTurn();
            AP.BeginTurn();
            MoveController.BeginTurn();
        }

        public void EndTurn()
        {
            Init.EndTurn();
            AP.EndTurn();
            MoveController.EndTurn();

            if (TurnEnded != null)
            {
                TurnEnded(this, EventArgs.Empty);
            }
        }

        private void Start()
        {
            AP = GetComponent<ActionPoints>();
            Init = GetComponent<Initiative>();
            MoveController = GetComponent<MoveToTile>();
        }
    }
}
