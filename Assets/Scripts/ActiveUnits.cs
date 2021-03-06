﻿namespace DLS.LD39
{
    using Graphics;
    using Map;
    using Units;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Utility;

    class ActiveUnits : SingletonComponent<ActiveUnits>
    {
        private HashSet<GameUnit> _activeUnits = new HashSet<GameUnit>();

        public event EventHandler<ActiveUnitsChangedEventArgs> UnitAdded;

        public event EventHandler<ActiveUnitsChangedEventArgs> UnitRemoved;

        public event EventHandler<ActiveUnitsChangedEventArgs> UnitDestroyed;

        public IEnumerable<GameUnit> Units
        {
            get
            {
                return _activeUnits;
            }
        }

        public void UpdateVisibility()
        {
            foreach (var unit in _activeUnits)
            {
                unit.Visibility.UpdateVisibility();
            }
        }

        public void AddActiveUnit(GameUnit unit)
        {
            unit.UnitDestroyed += OnUnitDestroyed;
            TurnOrderTracker.Instance.RegisterUnit(unit);
            _activeUnits.Add(unit);
            UnitAdded.SafeRaiseEvent(this, new ActiveUnitsChangedEventArgs(unit));
        }

        public GameUnit GetUnitAtTile(Tile tile)
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
            UnitRemoved.SafeRaiseEvent(this, new ActiveUnitsChangedEventArgs(unit));
            Destroy(unit.gameObject); // wait at least one frame
        }

        private void OnUnitDestroyed(object sender, EventArgs e)
        {
            var unit = sender as GameUnit;
            if (unit == null)
            {
                return;
            }

            if (ActiveSelectionTracker.Instance.SelectedObject == unit.gameObject)
            {
                ActiveSelectionTracker.Instance.ClearSelection();
            }

            UnitDestroyed.SafeRaiseEvent(this, new ActiveUnitsChangedEventArgs(unit));
            RemoveUnit(unit);
            if (unit.Data.GraphicsData.DeathAnimation != null)
            {
                ExplosionSpawner.Instance.SpawnExplosion(unit.Data.GraphicsData.DeathAnimation, unit.Position.CurrentTile);
            }
        }
    }
}
