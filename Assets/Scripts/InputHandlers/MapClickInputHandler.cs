namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    abstract class MapClickInputHandler : IMapClickInputHandler
    {
        public string HandlerID
        {
            get; private set;
        }

        protected MapClickInputHandler(string id)
        {
            HandlerID = id;
        }

        public virtual bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public virtual bool HandleTileClick(int button, Tile clickedTile)
        {
            return false;
        }
    }
}
