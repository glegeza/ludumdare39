namespace DLS.LD39.Graphics
{
    using DLS.LD39.Combat;
    using UnityEngine;

    public class BulletSpawner : SingletonComponent<BulletSpawner>
    {
        public Bullet BulletPrefab;

        public Bullet SpawnBullet(Transform origin, Transform target)
        {
            if (target == null || origin == null)
            {
                Debug.LogError("Attempting to spawn bullet with null origin or target");
                return null;
            }
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = origin.position;
            bullet.Origin = origin;
            bullet.Target = target;
            bullet.StartPath();

            return bullet;
        }
    }
}
