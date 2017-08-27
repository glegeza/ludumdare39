namespace DLS.LD39.Map
{
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(MeshRenderer))]
    public class TileLayerQuad : MonoBehaviour
    {
        private MeshFilter _filter;
        private BoxCollider2D _collider;

        [UsedImplicitly]
        private void Awake()
        {
            _filter = GetComponent<MeshFilter>();
            _collider = GetComponent<BoxCollider2D>();

        }

        [UsedImplicitly]
        private void Update()
        {
            
        }
    }
}
