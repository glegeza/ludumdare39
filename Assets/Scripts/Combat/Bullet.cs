﻿namespace DLS.LD39.Combat
{
    using JetBrains.Annotations;
    using UnityEngine;

    public delegate void BulletHitCallback();

    [RequireComponent(typeof(BoxCollider2D))]
    [UsedImplicitly]
    public class Bullet : MonoBehaviour
    {
        public float Speed = 5.0f;

        private BulletHitCallback _cb;
        private Vector3 _dirvec;
        private bool _moving;

        public Transform Origin
        {
            get; set;
        }

        public Transform Target
        {
            get; set;
        }

        public void StartPath(BulletHitCallback cb)
        {
            if (Target == null)
            {
                Debug.LogError("Starting bullet with null target");
                return;
            }
            if (Origin == null)
            {
                Debug.LogError("Starting bullet with null origin");
                return;
            }
            Debug.LogFormat("Launching bullet from {0} with target {1}", Origin.name, Target.name);
            _dirvec = (Target.position - Origin.position).normalized;
            _dirvec.z = 0.0f;
            _moving = true;
            _cb = cb;
        }

        [UsedImplicitly]
        private void FixedUpdate()
        {
            if (_moving)
            {
                transform.position += _dirvec * Speed * Time.deltaTime;
            }
        }

        [UsedImplicitly]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.LogFormat("Collided with {0}", collision.collider.name);
            if (!_moving)
            {
                return;
            }
            var unit = collision.collider.transform;
            if (unit == null || unit != Target)
            {
                return;
            }
            if (_cb != null)
            {
                _cb();
                _cb = null;
            }
            _moving = false;
            Destroy(gameObject);
        }
    }
}
