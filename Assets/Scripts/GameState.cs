namespace DLS.LD39
{
    using Generation;
    using Units;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameState : SingletonComponent<GameState>
    {
        private List<GameUnit> _playerUnits = new List<GameUnit>();
        private RoomMap _map;

        public IEnumerable<GameUnit> PlayerUnits
        {
            get { return _playerUnits; }
        }

        public RoomMap CurrentMap
        {
            get { return _map; }
        }

        public bool GameActive
        {
            get; private set;
        }

        public void GameOver()
        {
            YouLose();
        }

        public void EndLevel()
        {
            YouWin();
        }

        protected override void Awake()
        {
            base.Awake();
            GameActive = true;
            _map = FindObjectOfType<RoomMap>();
            ActiveUnits.Instance.UnitAdded += OnUnitAdded;
            ActiveUnits.Instance.UnitRemoved += OnUnitRemoved;
        }

        private void OnUnitRemoved(object sender, ActiveUnitsChangedEventArgs e)
        {
            if (e.Unit.Faction == Faction.Player)
            {
                Debug.Log("Removed player unit");
                _playerUnits.Remove(e.Unit);
            }
        }

        private void OnUnitAdded(object sender, ActiveUnitsChangedEventArgs e)
        {
            if (e.Unit.Faction == Faction.Player)
            {
                Debug.Log("Added player unit");
                _playerUnits.Add(e.Unit);
            }
        }

        private void YouWin()
        {
            Debug.Log("YOU TOTALLY WIN OR SOMETHING");
            GameActive = false;
        }

        private void YouLose()
        {
            Debug.Log("GAME OVER MAN");
            GameActive = false;
        }
    }
}
