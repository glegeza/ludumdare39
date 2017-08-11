namespace DLS.LD39
{
    using DLS.LD39.Graphics;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System.Collections.Generic;
    using UnityEngine;

    class ActiveUnits : SingletonComponent<ActiveUnits>
    {
        private List<GameUnit> _activeUnits = new List<GameUnit>();

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
            Destroy(unit.gameObject);
        }

        private void OnUnitDestroyed(object sender, System.EventArgs e)
        {
            var unit = sender as GameUnit;
            if (unit == null)
            {
                return;
            }
            RemoveUnit(unit);
            if (unit.Data.DeathPrefab != null)
            {
                ExplosionController.Instance.SpawnExplosion(unit.Data.DeathPrefab, unit.Position.CurrentTile);
            }
        }
    }
}
