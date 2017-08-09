namespace DLS.LD39
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    public class PlayerUnitController : SingletonComponent<PlayerUnitController>
    {
        private TurnOrderTracker _turnTracker;

        public bool IsPlayersTurn
        {
            get
            {
                return _turnTracker.ActiveUnit != null &&
                    _turnTracker.ActiveUnit.Faction == Units.Faction.Player;
            }
        }

        public void EndCurrentUnitTurn()
        {
            if (!IsPlayersTurn)
            {
                return;
            }

            _turnTracker.AdvanceTurn();
        }

        private void Start()
        {
            _turnTracker = TurnOrderTracker.Instance;
        }
    }
}
