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

        public GameObject UnitPrefab;

        private List<GameUnit> _activeUnits = new List<GameUnit>();
        private float _testInit = 0.0f;
        private int _testUnitIdx = 0;

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

        public bool SpawnTestUnit(Tile tilePos)
        {
            if (!CanSpawnUnit(tilePos))
            {
                return false;
            }

            var unitName = String.Format("{0} - {1}", "TestUnit", _testUnitIdx++);
            var unitObject = Instantiate(UnitPrefab);
            var unit = unitObject.AddComponent<GameUnit>();
            var mover = unitObject.AddComponent<MoveToTile>();
            unit.Initialize(tilePos, Faction.Player, "TestUnit", unitName);
            unit.AP.MaximumPoints = 40;
            unit.AP.PointsPerTurn = 20;
            unit.AP.PointsRemaining = 20;
            mover.Initialize(unit);
            unitObject.name = unitName;
            _activeUnits.Add(unit);
            unit.Initiative.InitiativeValue = _testInit;
            TurnOrderTracker.Instance.RegisterUnit(unit);

            _testInit += 0.2f;

            Debug.LogFormat("Spawning {0} at {1}", unitName, tilePos);

            return true;
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
    }
}
