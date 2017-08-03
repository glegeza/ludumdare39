namespace DLS.LD39.Map
{
    using System.Collections.Generic;
    using UnityEngine;

    class TileSheet
    {
        private Texture _sheetTexture;
        private int _numTiles;
        private float _tileWidth;
        private float _tileHeight;
        private List<IndexedTile> _tiles = new List<IndexedTile>();

        public TileSheet(Texture sheetTexture, int tilePixelWidth)
        {
            _sheetTexture = sheetTexture;
            _numTiles = _sheetTexture.width / tilePixelWidth;

            _tileWidth = (float)tilePixelWidth / _sheetTexture.width;
            _tileHeight = 1.0f;

            for (var i = 0; i < _numTiles; i++)
            {
                var tile = new IndexedTile(this, i,
                    new Vector2(i * _tileWidth, 0.0f), _tileWidth, _tileHeight);
                _tiles.Add(tile);
            }
        }

        public IndexedTile GetIndexedTile(int idx)
        {
            if (!(idx < _numTiles))
            {
                return null;
            }

            return _tiles[idx];
        }
    }
}
