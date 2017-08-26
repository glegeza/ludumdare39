namespace DLS.LD39.Map
{
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(TileMap))]
    [UsedImplicitly]
    public class CAGenerator : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float EmptyChance = 0.5f;
        public int Iterations = 4;
        public int SimilarityRule = 5;

        private TileMap _map;

        [UsedImplicitly]
        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        [UsedImplicitly]
        private void Start()
        {
            foreach (var tile in _map.Tiles)
            {
                _map.SetTileAt(
                    tile.X, 
                    tile.Y, 
                    Random.Range(0.0f, 1.0f) < EmptyChance ? "default" : "empty");
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
                var wallCount = tile.Type.ID == "default" ? 1 : 0;
                foreach (var adj in tile.AdjacentTiles)
                {
                    if (adj.Type.ID == "default")
                    {
                        wallCount += 1;
                    }
                }
                if (wallCount >= SimilarityRule)
                {
                    tiles[tile.Y * _map.Width + tile.X] = "default";
                }
                else
                {
                    tiles[tile.Y * _map.Width + tile.X] = "empty";
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
