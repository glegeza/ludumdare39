namespace DLS.LD39.Controllers
{
    using DLS.LD39.Map;

    interface IMapClickInputHandler
    {
        bool HandleTileClick(int button, Tile clickedTile);
    }
}
