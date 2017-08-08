namespace DLS.LD39.Graphics
{
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private List<Sprite> _frames = new List<Sprite>();
        private int _currentFrame;
        private float _frameSpeed;
        private float _elapsedTime;
        private bool _isRunning;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(SpriteAnimationData data)
        {
            _frameSpeed = data.FrameSpeed;
            _isRunning = true;
            _frames.AddRange(Resources.LoadAll<Sprite>("sprites/" + data.SpriteTexture.name));
            _elapsedTime = 0.0f;
        }

        private void Update()
        {
            if (!_isRunning)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _frameSpeed)
            {
                _currentFrame = (_currentFrame + 1) % _frames.Count;
                _renderer.sprite = _frames[_currentFrame];
                _elapsedTime = 0.0f;
            }
        }
    }
}
