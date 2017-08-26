namespace DLS.LD39.Map
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class TileSet
    {
        private List<IndexedTile> _tiles = new List<IndexedTile>();
        private Dictionary<string, IndexedTile> _tileDict = new Dictionary<string, IndexedTile>();

        public TileSet(TileSetData data)
        {
            if (data == null)
            {
                Debug.LogError("null tile data!");
                throw new ArgumentNullException("data");
            }
            Data = data;
            ID = Data.ID;

            TileMaterial = new Material(Data.TileShader)
            {
                mainTexture = Data.TileTexture
            };

            var tileUVWidth = (float)Data.TileWidthPixels / Data.TileTexture.width;
            var tileUVHeight = (float)Data.TileHeightPixels / Data.TileTexture.height;

            var numTilesX = Data.TileTexture.width / Data.TileWidthPixels;
            var numTilesY = Data.TileTexture.height / Data.TileHeightPixels;

            var curIdx = 0;
            for (var y = 0; y < numTilesY; y++)
            {
                for (var x = 0; x < numTilesX; x++)
                {
                    var botLeft = new Vector2(
                        x * tileUVWidth,
                        y * tileUVHeight);
                    if (curIdx < Data.TileTypes.Count)
                    {
                        var nextTile = new IndexedTile(curIdx, botLeft,
                            tileUVWidth, tileUVHeight, Data.TileTypes[curIdx]);
                        _tileDict.Add(Data.TileTypes[curIdx].ID.ToLower(), nextTile);
                        _tiles.Add(nextTile);
                    }
                    curIdx++;
                }
            }
        }

        public string ID
        {
            get; private set;
        }

        public TileSetData Data
        {
            get; private set;
        }

        public Material TileMaterial
        {
            get; private set;
        }

        public IndexedTile GetIndexedTile(int idx)
        {
            if (idx >= _tiles.Count)
            {
                Debug.LogErrorFormat("Tile index {0} out of bounds for tile set {1}",
                    idx, ID);
                return null;
            }

            return _tiles[idx];
        }

        public IndexedTile GetTileByID(string tileID)
        {
            var lowerID = tileID.ToLower();
            if (!_tileDict.ContainsKey(lowerID))
            {
                Debug.LogErrorFormat("Tile ID {0} not found in tile set {1}",
                    tileID, ID);
            }

            return _tileDict[lowerID];
        }
    }
}
