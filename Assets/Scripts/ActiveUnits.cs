namespace DLS.LD39
{
    using DLS.LD39.Graphics;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System.Collections.Generic;
    using UnityEngine;

    class ActiveUnits : SingletonComponent<ActiveUnits>
    {
        private HashSet<GameUnit> _activeUnits = new HashSet<GameUnit>();

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
            Destroy(unit.gameObject); // wait at least one frame
        }

        private void OnUnitDestroyed(object sender, System.EventArgs e)
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

            RemoveUnit(unit);
            if (unit.Data.GraphicsData.DeathAnimation != null)
            {
                ExplosionSpawner.Instance.SpawnExplosion(unit.Data.GraphicsData.DeathAnimation, unit.Position.CurrentTile);
            }
        }
    }
}
