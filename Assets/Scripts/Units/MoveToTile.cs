namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using UnityEngine;

    [RequireComponent(typeof(TilePosition))]
    public class MoveToTile : MonoBehaviour
    {
        private TilePicker _picker;
        private TilePosition _position;

        private void Start()
        {
            var tileMap = FindObjectOfType<TileMap>();
            _picker = new TilePicker(tileMap);
            _position = GetComponent<TilePosition>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var tile = _picker.GetTileAtScreenPosition(Input.mousePosition);
                if (tile != null)
                {
                    _position.SetTile(tile);
                }
            }
        }
    }
}
