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

        public GameUnit Target
        {
            get; set;
        }

        public void StartPath()
        {
            if (Target == null)
            {
                return;
            }
            _dirvec = Target.Position.CurrentTile.WorldCoords - new Vector2(transform.position.x, transform.position.y);
        }

        private void Update()
        {
            transform.position += _dirvec * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var unit = collision.GetComponent<GameUnit>();
            if (unit == null || unit != Target)
            {
                return;
            }
            HitTarget?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
