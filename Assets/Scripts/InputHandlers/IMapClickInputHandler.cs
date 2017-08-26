namespace DLS.LD39.InputHandlers
{
    using Map;

    public interface IMapClickInputHandler
    {
        string HandlerID { get; }

        string Name { get; }

        bool HandleTileClick(int button, Tile clickedTile);

        bool HandleButtonDown(int button, Tile tile);
    }
}
