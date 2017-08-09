namespace DLS.LD39.Map
{
    using UnityEngine;

    public class TilePosition : MonoBehaviour
    {
        public Tile CurrentTile
        {
            get; private set;
        }

        public Vector2 TileWorldPosition
        {
            get
            {
                return CurrentTile.WorldCoords;
            }
        }

        public void SetTile(Tile tile, bool setWorldCoords=true)
        {
            CurrentTile = tile;
            if (setWorldCoords)
            {
                transform.position = new Vector3(
                    TileWorldPosition.x, TileWorldPosition.y, transform.position.z);
            }
        }
    }
}
