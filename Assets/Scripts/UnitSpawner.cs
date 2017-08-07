namespace DLS.LD39
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    class UnitSpawner : MonoBehaviour
    {
        private static UnitSpawner _instance;

        public static UnitSpawner Instance { get { return _instance; } }

        public List<UnitData> UnitTypes = new List<UnitData>();

        private UnitFactory _unitFactory = new UnitFactory();
        private List<GameUnit> _activeUnits = new List<GameUnit>();
        private Dictionary<string, UnitData> _unitTypes = new Dictionary<string, UnitData>();

        public bool SpawnTestUnit(string id, Tile tilePos)
        {
            id = id.ToLower();
            if (!_unitTypes.ContainsKey(id))
            {
                Debug.LogErrorFormat("Unknown unit type {0}", id);
                return false;
            }

            var unitData = _unitTypes[id];
            var name = String.Format("{0} : {1}", "Unit", id);
            var unit = _unitFactory.GetUnit(name, unitData, tilePos);

            TurnOrderTracker.Instance.RegisterUnit(unit);
            _activeUnits.Add(unit);

            return true;
        }

        public GameUnit UnitAtTile(Tile tile)
        {
            foreach (var unit in _activeUnits)
            {
                if (unit.Position.CurrentTile.Equals(tile))
                {
                    return unit;
                }
            }
            return null;
        }

        public void RemoveUnit(GameUnit unit)
        {
            Debug.LogFormat("Removing {0} from {1}", unit.name, unit.Position.CurrentTile);

            _activeUnits.Remove(unit);
            TurnOrderTracker.Instance.UnregisterUnit(unit);
            Destroy(unit.gameObject);
        }

        public bool CanSpawnUnit(Tile tilePos)
        {
            if (!tilePos.Passable)
            {
                return false;
            }

            foreach (var activeUnit in _activeUnits)
            {
                if (activeUnit.Position.CurrentTile.Equals(tilePos))
                {
                    return false;
                }
            }

            return true;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            foreach (var unit in UnitTypes)
            {
                _unitTypes.Add(unit.ID, unit);
            }
        }
    }
}
