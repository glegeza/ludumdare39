namespace DLS.LD39.Pathfinding
{
    using DLS.LD39.Map;

    class Node
    {
        public Node(Tile node, Tile parent, int g, int h)
        {
            NodeTile = node;
            ParentTile = parent;
            G = g;
            H = h;
        }
        
        public int F
        {
            get
            {
                return G + H;
            }
        }

        public int G
        {
            get; private set;
        }
        
        public int H
        {
            get; private set;
        }

        public Tile NodeTile
        {
            get; private set;
        }

        public Tile ParentTile
        {
            get; private set;
        }
    }
}
