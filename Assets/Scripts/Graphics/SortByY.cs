namespace DLS.LD39.Graphics
{
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(SpriteRenderer))]
    [UsedImplicitly]
    public class SortByY : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        [UsedImplicitly]
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        [UsedImplicitly]
        private void Update()
        {
            _renderer.sortingOrder = (int)((transform.position.y * 100) * -1);
        }
    }
}
