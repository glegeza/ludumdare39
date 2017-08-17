namespace DLS.LD39
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections;
    using UnityEngine;

    public class MoveAnimator : MonoBehaviour
    {
        private float _moveTime;
        private float _inverseMoveTime;
        private UnitAnimationController _animationController;

        public event EventHandler<EventArgs> CompletedMovement;

        public float MoveAnimationTime
        {
            get
            {
                return _moveTime;
            }
            set
            {
                _moveTime = value;
                _moveTime = Mathf.Max(0.0f, _moveTime);
                _inverseMoveTime = 1.0f / _moveTime;
            }
        }

        private void Awake()
        {
            MoveAnimationTime = 1.0f;
            _animationController = GetComponent<UnitAnimationController>();
        }

        public void BeginMotion(Tile target)
        {
            StartCoroutine(DoMovement(target));
        }

        private IEnumerator DoMovement(Tile target)
        {
            StartAnimation();
            var end = new Vector3(target.WorldCoords.x, target.WorldCoords.y, transform.position.z);

            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostion = Vector3.MoveTowards(transform.position, end, _inverseMoveTime * Time.deltaTime);
                transform.position = newPostion;
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                yield return null;
            }

            EndAnimation();
            CompletedMovement?.Invoke(this, EventArgs.Empty);
            Destroy(this, 0.01f);
        }

        private void StartAnimation()
        {
            if (_animationController != null)
            {
                _animationController.StartWalkAnimation();
            }
        }

        private void EndAnimation()
        {
            if (_animationController != null)
            {
                _animationController.StartIdleAnimation();
            }
        }
    }
}
