namespace DLS.LD39.Controllers
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using UnityEngine;

    [RequireComponent(typeof(MoveToTile))]
    class UnitClickMover : MonoBehaviour
    {
        private TilePicker _picker;
        private MoveToTile _mover;

        private void Start()
        {
            var tileMap = FindObjectOfType<TileMap>();
            _mover = GetComponent<MoveToTile>();
            _picker = new TilePicker(tileMap);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var targetTile = _picker.GetTileAtScreenPosition(Input.mousePosition);
                if (targetTile != null)
                {
                    _mover.SetNewTarget(targetTile);
                }
            }
        }
    }
}
