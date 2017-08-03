namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    class MapClickEditor : IMapClickInputHandler
    {
        private enum Mode
        {
            DoNothing,
            Passable,
            Impassable
        }

        private Mode _currentMode = Mode.DoNothing;

        public bool HandleButtonDown(int button, Tile targetTile)
        {
            if (button != 0 || _currentMode == Mode.DoNothing)
            {
                return false;
            }

            if (_currentMode == Mode.Impassable)
            {
                targetTile.Passable = false;
                targetTile.Map.SetTileAt(targetTile.X, targetTile.Y, 1);
            }
            else
            {
                targetTile.Passable = true;
                targetTile.Map.SetTileAt(targetTile.X, targetTile.Y, 0);
            }

            return false;
        }

        public bool HandleTileClick(int button, Tile targetTile)
        {
            if (button != 0 || targetTile == null)
            {
                return false;
            }

            _currentMode = targetTile.Passable ? Mode.Impassable : Mode.Passable;

            return false;
        }
    }
}
