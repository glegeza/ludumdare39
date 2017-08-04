namespace DLS.LD39.Map
{
    using UnityEngine;

    class TilePosition : MonoBehaviour
    {
        public Tile CurrentTile
        {
            get; private set;
        }

        public void SetTile(Tile tile)
        {
            var worldCoords = tile.WorldCoords;
            transform.position = new Vector3(
                worldCoords.x, worldCoords.y, transform.position.z);
            CurrentTile = tile;
        }
    }
}
