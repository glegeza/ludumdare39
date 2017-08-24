namespace DLS.LD39
{
    using DLS.LD39.Generation;
    using DLS.LD39.Units;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameState : SingletonComponent<GameState>
    {
        private List<GameUnit> _playerUnits = new List<GameUnit>();
        private RoomMap _map;

        public bool GameActive
        {
            get; private set;
        }

        protected override void Awake()
        {
            base.Awake();
            GameActive = true;
            _map = FindObjectOfType<RoomMap>();
            ActiveUnits.Instance.UnitAdded += OnUnitAdded;
            ActiveUnits.Instance.UnitRemoved += OnUnitRemoved;
            TurnOrderTracker.Instance.TurnAdvanced += OnTurnAdvanced;
        }

        private void OnTurnAdvanced(object sender, System.EventArgs e)
        {
            if (_playerUnits.Count == 0)
            {
                return;
            }

            foreach (var unit in _playerUnits)
            {
                if (!_map.ExitRoom.UnitInRoom(unit))
                {
                    return;
                }
            }

            YouWin();
        }

        private void OnUnitRemoved(object sender, ActiveUnitsChangedEventArgs e)
        {
            if (e.Unit.Faction == Faction.Player)
            {
                Debug.Log("Removed player unit");
                _playerUnits.Remove(e.Unit);
            }

            if (_playerUnits.Count == 0)
            {
                GameOver();
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
        }

        private void GameOver()
        {
            Debug.Log("GAME OVER MAN");
            GameActive = false;
        }
    }
}
