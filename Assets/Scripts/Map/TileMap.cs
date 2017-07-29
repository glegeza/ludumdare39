namespace DLS.LD39.Map
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Utility.Unity.ProceduralMeshes;

    [RequireComponent(typeof(MeshFilter))]
    public class TileMap : MonoBehaviour
    {
        public int Width;
        public int Height;
        public Vector2 TileSize;

        private MeshFilter _filter;

        // Use this for initialization
        void Start()
        {
            _filter = GetComponent<MeshFilter>();
            GetMesh();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void GetMesh()
        {
            var mesh = TilePlaneGenerator.GetPlane(TileSize, Width, Height);
            _filter.sharedMesh = mesh;
        }
    }
}