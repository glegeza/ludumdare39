// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
namespace DLS.LD39.Map
{
    using UnityEngine;

    public class IndexedTile
    {
        public IndexedTile(int idx, Vector2 bottomLeft, float width, float height, TileType type)
        {
            Index = idx;
            BottomLeft = bottomLeft;
            Width = width;
            Height = height;
            Type = type;
        }

        public TileType Type
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
