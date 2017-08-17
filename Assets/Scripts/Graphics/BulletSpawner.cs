namespace DLS.LD39.Graphics
{
    using DLS.LD39.Combat;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BulletSpawner : SingletonComponent<BulletSpawner>
    {
        public Bullet BulletPrefab;

        public Bullet SpawnBullet(Tile origin, GameUnit target)
        {
            if (target == null)
            {
                UnityEngine.Debug.LogError("Buh?");
            }
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = origin.WorldCoords;
            bullet.Target = target;
            bullet.StartPath();

            return bullet;
        }
    }
}
