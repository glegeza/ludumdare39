namespace DLS.LD39.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    [RequireComponent(typeof(TileMap))]
    public class CAGenerator : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float EmptyChance = 0.5f;
        public int Iterations = 4;
        public int SimilarityRule = 5;

        private TileMap _map;

        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        private void Start()
        {
            foreach (var tile in _map.Tiles)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) < EmptyChance)
                {
                    _map.SetTileAt(tile.X, tile.Y, "empty");
                }
                else
                {
                    _map.SetTileAt(tile.X, tile.Y, "default");
                }
            }

            for (var i = 0; i < Iterations; i++)
            {
                Iterate();
            }
        }

        private void Iterate()
        {
            var tiles = new string[_map.Width * _map.Height];

            foreach (var tile in _map.Tiles)
            {
                var wallCount = 0;
                foreach (var adj in tile.AdjacentTiles)
                {
                    if (adj.Type.ID == "empty")
                    {
                        wallCount += 1;
                    }
                }
                if (wallCount >= SimilarityRule)
                {
                    tiles[tile.Y * _map.Width + tile.X] = "empty";
                }
                else
                {
                    tiles[tile.Y * _map.Width + tile.X] = "default";
                }
            }

            for (var x = 0; x < _map.Width; x++)
            {
                for (var y = 0; y < _map.Height; y++)
                {
                    _map.SetTileAt(x, y, tiles[y * _map.Width + x]);
                }
            }
        }
    }
}
