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

        public GameUnit Origin
        {
            get; set;
        }

        public GameUnit Target
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
            _dirvec = Target.Position.CurrentTile.WorldCoords - new Vector2(transform.position.x, transform.position.y);
            _moving = true;
        }

        private void Update()
        {
            if (_moving)
            {
                transform.position += _dirvec * Speed * Time.deltaTime;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var unit = collision.collider.GetComponent<GameUnit>();
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
