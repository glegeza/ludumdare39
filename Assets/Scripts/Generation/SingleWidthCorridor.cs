namespace DLS.LD39.Generation
{
    using Utility;
    using System.Collections.Generic;
    using System.Linq;
    using DLS.LD39.Map;

    public class SingleWidthCorridor : MapElement
    {
        public List<IntVector2> _nodes = new List<IntVector2>();
        public HashSet<IntVector2> _tiles = new HashSet<IntVector2>();

        public bool AddNode(IntVector2 node)
        {
            if (node == null || _nodes.Contains(node))
            {
                return false;
            }

            _nodes.Add(node);
            UpdateTiles();
            return true;
        }

        public override IEnumerable<IntVector2> Tiles
        {
            get
            {
                return _tiles;
            }
        }

        public void SetTiles(TileMap map)
        {
            foreach (var tile in _tiles)
            {
                var existingTile = map.GetTile(tile);
                if (existingTile.Type.ID == "empty")
                {
                    map.SetTileAt(tile.X, tile.Y, "default");
                }
            }
        }

        public bool Overlaps(SingleWidthCorridor other)
        {
            return other != null && _tiles.Intersect(other._tiles).Any();
        }

        private void UpdateTiles()
        {
            _tiles.Clear();
            if (_nodes.Count < 2)
            {
                return;
            }

            _tiles.Add(_nodes[0]);
            for (var i = 0; i < _nodes.Count - 1; i++)
            {
                var start = _nodes[i];
                var target = _nodes[i + 1];
                var last = start;
                while (!(last.Equals(target)))
                {
                    if (target.X > last.X)
                    {
                        last = new IntVector2(last.X + 1, last.Y);
                    }
                    else if (target.X < last.X)
                    {
                        last = new IntVector2(last.X - 1, last.Y);
                    }
                    else if (target.Y > last.Y)
                    {
                        last = new IntVector2(last.X, last.Y + 1);
                    }
                    else if (target.Y < last.Y)
                    {
                        last = new IntVector2(last.X, last.Y - 1);
                    }
                    _tiles.Add(last);
                }
            }
        }

        public override bool TileInElement(int x, int y)
        {
            return _tiles.Contains(new IntVector2(x, y));
        }
    }
}
