namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using DLS.LD39.Combat;
    using System;
    using System.Collections.Generic;

    public class Visibility : GameUnitComponent
    {
        private HashSet<Tile> _visibleTiles = new HashSet<Tile>();

        public event EventHandler<EventArgs> VisibilityUpdated;

        public float VisionRange
        {
            get; private set;
        }

        public IEnumerable<Tile> VisibleTiles
        {
            get
            {
                return _visibleTiles;
            }
        }

        protected override void OnInitialized(GameUnit unit)
        {
            VisionRange = 5;
            unit.MoveAction.CompletedAction += OnMoveCompleted;
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
            return _visibleTiles.Contains(unit.Position.CurrentTile);
        }

        public void UpdateVisibility()
        {
            _visibleTiles.Clear();

            foreach (var tile in LOSChecker.Instance.GetVisibleTiles(
                AttachedUnit.Position.CurrentTile, VisionRange))
            {
                _visibleTiles.Add(tile);
            }

            RaiseEvent(VisibilityUpdated, this, EventArgs.Empty);
        }

        private void RaiseEvent(EventHandler<EventArgs> handler, object s, EventArgs e)
        {
            if (handler != null)
            {
                handler(s, e);
            }
        }
    }
}
