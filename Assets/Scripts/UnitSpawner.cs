﻿namespace DLS.LD39
{
    using DLS.LD39.AI;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    class UnitSpawner : SingletonComponent<UnitSpawner>
    {
        private UnitFactory _unitFactory = new UnitFactory();
        private Dictionary<string, UnitData> _unitTypes = new Dictionary<string, UnitData>();

        public IEnumerable<string> UnitTypeIDs
        {
            get
            {
                return _unitTypes.Keys;
            }
        }

        public bool SpawnUnit(string id, Tile tilePos)
        {
            id = id.ToLower();
            if (!_unitTypes.ContainsKey(id))
            {
                Debug.LogErrorFormat("Unknown unit type {0}", id);
                return false;
            }
            if (!CanSpawnUnit(tilePos))
            {
                Debug.LogErrorFormat("Tile at {0} is occupied", id);
                return false;
            }

            var unitData = _unitTypes[id];
            var name = String.Format("{0} : {1}", "Unit", id);
            var unit = _unitFactory.GetUnit(name, unitData, tilePos);

            if (unitData.Faction != Faction.Player)
            {
                var controller = unit.gameObject.AddComponent<StateController>();
                controller.Initialize(unit);
                controller.TransitionToState(unitData.DefaultState);
                unit.SetController(controller);

            }

            ActiveUnits.Instance.AddActiveUnit(unit);
            return true;
        }

        public bool CanSpawnUnit(Tile tilePos)
        {
            return tilePos.Passable &&
                ActiveUnits.Instance.GetUnitAtTile(tilePos) == null;
        }

        protected override void Awake()
        {
            base.Awake();

            var units = Resources.LoadAll<UnitData>("Units");
            foreach (var unit in units)
            {
                _unitTypes.Add(unit.ID, unit);
            }
        }
    }
}
