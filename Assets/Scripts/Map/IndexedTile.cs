namespace DLS.LD39.Map
{
    using UnityEngine;

    class IndexedTile
    {
        public IndexedTile(TileSheet sheet, int idx, Vector2 bottomLeft, float width, float height)
        {
            BottomLeft = bottomLeft;
            Width = width;
            Height = height;
        }

        public TileSheet Sheet
        {
            get; private set;
        }

        public int Index
        {
            get; private set;
        }

        public Vector2 BottomLeft
        {
            get; private set;
        }

        public float Width
        {
            get; private set;
        }

        public float Height
        {
            get; private set;
        }
    }
}
