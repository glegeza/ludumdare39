namespace DLS.LD39.Map
{
    using UnityEngine;
    using Utility.Unity.ProceduralMeshes;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class TileMap : MonoBehaviour
    {
        public int Width;
        public int Height;
        public Vector2 TileSize;

        private MeshFilter _filter;
        private BoxCollider2D _collider;

        public Vector2 WorldSize
        {
            get; private set;
        }
        
        public bool GetTileCoords(Vector2 worldCoords, out float x, out float y)
        {
            x = y = -1;
            var localCoords = transform.InverseTransformPoint(worldCoords);

            // Translate so we're starting from the upper left corner
            var halfWidth = WorldSize.x / 2.0f;
            var halfHeight = WorldSize.y / 2.0f;
            localCoords += new Vector3(halfWidth, halfHeight, 0.0f);

            x = (int)localCoords.x;
            y = (int)(Height - localCoords.y);

            if (localCoords.x < 0.0f || localCoords.x > WorldSize.x ||
                localCoords.y < 0.0f || localCoords.y > WorldSize.y)
            {
                return false;
            }
            
            return true;
        }

        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _collider = GetComponent<BoxCollider2D>();
            GetMesh();
            WorldSize = new Vector2(Width * TileSize.x,
                Width * TileSize.y);
            _collider.size = WorldSize;
        }

        private void GetMesh()
        {
            var mesh = TilePlaneGenerator.GetPlane(TileSize, Width, Height);
            _filter.sharedMesh = mesh;
        }
    }
}