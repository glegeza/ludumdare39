namespace DLS.LD39.Map
{
    using UnityEngine;
    
    public class TilePicker
    {
        public Tile GetTileAtScreenPosition(Vector2 screenPos)
        {
            var ray = Camera.main.ScreenToWorldPoint(screenPos);
            var hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit.collider != null)
            {
                var tileMap = hit.collider.gameObject.GetComponent<TileMap>();
                if (tileMap != null)
                {
                    int x, y;
                    if (tileMap.GetTileCoords(hit.point, out x, out y))
                    {
                        return tileMap.GetTile(x, y);
                    }
                }
            }

            return null;
        }
    }
}
