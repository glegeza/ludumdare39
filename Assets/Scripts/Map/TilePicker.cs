namespace DLS.LD39.Map
{
    using System;
    using UnityEngine;
    
    public class TilePicker
    {
        private TileMap _map;

        public TilePicker(TileMap map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            _map = map;
        }

        public Tile GetTileAtScreenPosition(Vector2 screenPos)
        {
            var ray = Camera.main.ScreenToWorldPoint(screenPos);
            var hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit.collider != null)
            {
                int x, y;
                if (_map.GetTileCoords(hit.point, out x, out y))
                {
                    return _map.GetTile(x, y);
                }
            }

            return null;
        }
    }
}
