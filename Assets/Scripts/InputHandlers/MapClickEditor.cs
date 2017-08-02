namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    class MapClickEditor : IMapClickInputHandler
    {
        public bool HandleTileClick(int button, Tile targetTile)
        {
            if (button != 0)
            {
                return false;
            }

            if (targetTile.Passable)
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
    }
}
