namespace DLS.LD39.Generation
{
    using Data;
    using Utility;

    public class RectRoom : Room
    {
        public RectRoom(int x, int y, int width, int height, string id, RoomType type = null) : base(new IntVector2(x, y), id, type)
        {
            Rect = new IntRect(x, y, width, height);
            CreateCornerConnectors();
            SetTiles();
        }

        public RectRoom(IntRect rect, string id, RoomType type = null) : base(rect.BottomLeft, id, type)
        {
            Rect = rect;
            CreateCornerConnectors();
            SetTiles();
        }

        public int Width
        {
            get { return Rect.Width; }
        }

        public int Height
        {
            get { return Rect.Height; }
        }

        public IntRect Rect { get; private set; }

        private void CreateCornerConnectors()
        {
            AddConnector(Rect.TopLeft);
            AddConnector(Rect.TopRight);
            AddConnector(Rect.BottomLeft);
            AddConnector(Rect.BottomRight);
        }

        private void SetTiles()
        {
            for (var x = Rect.Left; x <= Rect.Right; x++)
            {
                for (var y = Rect.Bottom; y <= Rect.Top; y++)
                {
                    AddTile(new IntVector2(x, y));
                }
            }
        }
    }
}
