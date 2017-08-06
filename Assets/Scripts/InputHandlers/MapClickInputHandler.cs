namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;

    abstract class MapClickInputHandler : IMapClickInputHandler
    {
        protected MapClickInputHandler(string id, string name)
        {
            HandlerID = id;
            Name = name;
        }

        public string HandlerID
        {
            get; private set;
        }

        public string Name
        {
            get; private set;
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
