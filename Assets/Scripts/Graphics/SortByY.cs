namespace DLS.LD39.Graphics
{
    using UnityEngine;

    [RequireComponent(typeof(SpriteRenderer))]
    class SortByY : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _renderer.sortingOrder = (int)((transform.position.y * 100) * -1);
        }
    }
}
