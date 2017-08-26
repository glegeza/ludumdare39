namespace DLS.LD39.Map
{
    using Units;
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
            if (CurrentTile != null)
            {
                CurrentTile.SetUnit(null);
            }

            CurrentTile = tile;
            if (CurrentTile != null)
            {
                var go = GetComponent<GameUnit>();
                CurrentTile.SetUnit(go);
            }
            if (setWorldCoords)
            {
                transform.position = new Vector3(
                    TileWorldPosition.x, TileWorldPosition.y, transform.position.z);
            }
        }
    }
}
