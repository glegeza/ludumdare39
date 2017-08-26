namespace DLS.LD39.Units
{
    using Map;
    using UnityEngine;

    public class UnitFacing : GameUnitComponent
    {
        public void FaceTile(Tile tile)
        {
            if (tile == null)
            {
                return;
            }

            var curTile = AttachedUnit.Position.CurrentTile;
            if (tile.X < curTile.X)
            {
                SetLeft();
            }
            if (tile.X > curTile.X)
            {
                SetRight();
            }
        }

        private void SetLeft()
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        private void SetRight()
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }
}
