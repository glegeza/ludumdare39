namespace DLS.LD39.Graphics
{
    using JetBrains.Annotations;
    using UnityEngine;
    using Map;

    [UsedImplicitly]
    public class ExplosionSpawner : SingletonComponent<ExplosionSpawner>
    {
        public float RandomOffset = 0.3f;

        public void SpawnExplosion(Explosion prefab, Tile tile)
        {
            var pos = tile.WorldCoords;
            pos += new Vector2(Random.Range(-RandomOffset, RandomOffset),
                Random.Range(-RandomOffset, RandomOffset));
            var explosion = Instantiate(prefab);
            explosion.transform.position = pos;
        }
    }
}
