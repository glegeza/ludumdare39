namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    interface IMapClickInputHandler
    {
        bool HandleTileClick(int button, Tile clickedTile);

        bool HandleButtonDown(int button, Tile tile);
    }
}
