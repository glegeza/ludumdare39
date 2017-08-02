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

            var unitName = String.Format("{0} - {1}", "Unit", "TestUnit");
            var unitObject = Instantiate(UnitPrefab);
            var unit = unitObject.AddComponent<GameUnit>();
            var mover = unitObject.AddComponent<MoveToTile>();
            unit.Initialize(tilePos, Faction.Player, "TestUnit");
            unit.AP.MaximumPoints = 40;
            unit.AP.PointsPerTurn = 20;
            unit.AP.PointsRemaining = 20;
            mover.Initialize(unit);
            unitObject.name = unitName;
            _activeUnits.Add(unit);
            unit.Initiative.InitiativeValue = UnityEngine.Random.Range(0.1f, 10.0f);
            TurnManager.Instance.RegisterUnit(unit);

            Debug.LogFormat("Spawning {0} at {1}", unitName, tilePos);

            return true;
        }

        public void RemoveUnit(GameUnit unit)
        {
            Debug.LogFormat("Removing {0} from {1}", unit.name, unit.Position.CurrentTile);

            _activeUnits.Remove(unit);
            TurnManager.Instance.UnregisterUnit(unit);
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
