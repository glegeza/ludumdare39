namespace DLS.LD39.Graphics
{
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    [UsedImplicitly]
    public class Explosion : MonoBehaviour
    {
        private Animator _animator;

        [UsedImplicitly]
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        [UsedImplicitly]
        private void Start()
        {
            var clipInfo = _animator.GetCurrentAnimatorClipInfo(0)[0];
            Destroy(gameObject, clipInfo.clip.length);
        }
    }
}
