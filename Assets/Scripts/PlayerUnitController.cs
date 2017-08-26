namespace DLS.LD39
{
    using JetBrains.Annotations;

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

        [UsedImplicitly]
        private void Start()
        {
            _turnTracker = TurnOrderTracker.Instance;
        }
    }
}
