namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    class MapClickEditor : MapClickInputHandler
    {
        private const int PassableIdx = 7;
        private const int ImpassableIdx = 4;

        private enum Mode
        {
            DoNothing,
            Passable,
            Impassable
        }

        private Mode _currentMode = Mode.DoNothing;

        public MapClickEditor() : base("edit")
        {

        }

        public override bool HandleButtonDown(int button, Tile targetTile)
        {
            if (button != 0 || _currentMode == Mode.DoNothing)
            {
                return false;
            }

            if (_currentMode == Mode.Impassable)
            {
                targetTile.Passable = false;
                targetTile.Map.SetTileAt(targetTile.X, targetTile.Y, ImpassableIdx);
            }
            else
            {
                targetTile.Passable = true;
                targetTile.Map.SetTileAt(targetTile.X, targetTile.Y, PassableIdx);
            }

            return false;
        }

        public override bool HandleTileClick(int button, Tile targetTile)
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
