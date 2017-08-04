namespace DLS.LD39.Map
{
    using UnityEngine;

    public class IndexedTile
    {
        public IndexedTile(int idx, Vector2 bottomLeft, float width, float height)
        {
            BottomLeft = bottomLeft;
            Width = width;
            Height = height;
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
