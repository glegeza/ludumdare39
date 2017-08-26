namespace DLS.LD39.Generation
{
    using Map;
    using Utility;
    using System.Collections.Generic;

    public abstract class MapElement
    {
        public abstract IEnumerable<IntVector2> Tiles
        {
            get;
        }

        public abstract bool TileInElement(int x, int y);

        public bool TileInElement(IntVector2 tilePos)
        {
            return TileInElement(tilePos.X, tilePos.Y);
        }

        public bool TileInElement(Tile tile)
        {
            return TileInElement(tile.X, tile.Y);
        }
    }
}
