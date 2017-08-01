namespace DLS.LD39
{
    using DLS.LD39.Map;
    using UnityEngine;

    class MapClickEditor : MonoBehaviour
    {
        private TilePicker _picker;
        private TileMap _tileMap;

        private void Start()
        {
            _tileMap = FindObjectOfType<TileMap>();
            _picker = new TilePicker(_tileMap);
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                var targetTile = _picker.GetTileAtScreenPosition(Input.mousePosition);
                if (targetTile != null)
                {
                    ToggleTile(targetTile);
                }
            }
        }

        private void ToggleTile(Tile targetTile)
        {
            if (targetTile.Passable)
            {
                targetTile.Passable = false;
                _tileMap.SetTileAt(targetTile.X, targetTile.Y, 1);
            }
            else
            {
                targetTile.Passable = true;
                _tileMap.SetTileAt(targetTile.X, targetTile.Y, 0);
            }
        }
    }
}
