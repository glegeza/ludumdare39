namespace DLS.LD39.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tile : IEquatable<Tile>
    {
        public Tile(int x, int y, int layer, TileMap map)
        {
            X = x;
            Y = y;
            Layer = layer;
            Map = map;
        }

        public TileMap Map
        {
            get;
            private set;
        }

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public int Layer
        {
            get;
            private set;
        }

        public IEnumerable<Tile> AdjacentTiles
        {
            get;
            private set;
        }

        public void AddAdjacentTile(Tile tile)
        {

        }

        public bool Equals(Tile other)
        {
            return X == other.X && Y == other.Y &&
                Map.Equals(other.Map) && Layer == other.Layer;
        }
    }
}
