namespace DLS.LD39.Combat
{
    using DLS.LD39.Units;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class Bullet : MonoBehaviour
    {
        public event EventHandler<EventArgs> HitTarget;
        public float Speed = 5.0f;

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

        public void StartPath()
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
        }

        private void FixedUpdate()
        {
            if (_moving)
            {
                //var rb = GetComponent<Rigidbody2D>();
                transform.position += _dirvec * Speed * Time.deltaTime;
            }
        }

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
            HitTarget?.Invoke(this, EventArgs.Empty);
            _moving = false;
            Destroy(gameObject);
        }
    }
}
