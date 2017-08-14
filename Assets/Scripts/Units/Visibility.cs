namespace DLS.LD39.Units
{
    using DLS.LD39.Combat;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Visibility : GameUnitComponent
    {
        private HashSet<GameUnit> _visibleUnits = new HashSet<GameUnit>();

        public event EventHandler<EventArgs> VisibilityUpdated;

        public float VisionRange
        {
            get; private set;
        }

        public IEnumerable<GameUnit> VisibleUnits
        {
            get
            {
                return _visibleUnits;
            }
        }

        protected override void OnInitialized(GameUnit unit)
        {
            VisionRange = 100;
            unit.MoveController.CompletedMovement += OnMoveCompleted;
            unit.TurnBegan += OnTurnBegan;
            UpdateVisibility();
        }

        private void OnTurnBegan(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void OnMoveCompleted(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        public bool IsUnitVisible(GameUnit unit)
        {
            return _visibleUnits.Contains(unit);
        }

        public void UpdateVisibility()
        {
            _visibleUnits.Clear();

            var activeUnits = ActiveUnits.Instance.Units;
            foreach (var unit in activeUnits)
            {
                if (unit == AttachedUnit)
                {
                    continue;
                }
                var distance = Vector2.Distance(
                    unit.Position.CurrentTile.TileCoords, 
                    AttachedUnit.Position.CurrentTile.TileCoords);
                if (distance <= VisionRange)
                {
                    CheckVisibilityAndAdd(unit);
                }
            }

            VisibilityUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void CheckVisibilityAndAdd(GameUnit unit)
        {
            var isVisible = LOSChecker.Instance.LOSClear(
                AttachedUnit.Position.CurrentTile,
                unit.Position.CurrentTile);
            if (isVisible)
            {
                _visibleUnits.Add(unit);
            }
        }
    }
}
