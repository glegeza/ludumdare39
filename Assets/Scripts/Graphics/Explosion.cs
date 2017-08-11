namespace DLS.LD39.Graphics
{
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class Explosion : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            var clipInfo = _animator.GetCurrentAnimatorClipInfo(0)[0];
            Destroy(gameObject, clipInfo.clip.length);
        }
    }
}
