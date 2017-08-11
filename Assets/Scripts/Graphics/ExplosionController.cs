namespace DLS.LD39.Graphics
{
    using UnityEngine;
    using DLS.LD39.Map;

    public class ExplosionController : SingletonComponent<ExplosionController>
    {
        public float RandomOffset = 0.3f;

        public void SpawnExplosion(Explosion prefab, Tile tile)
        {
            var pos = tile.WorldCoords;
            pos += new Vector2(Random.Range(-RandomOffset, RandomOffset),
                Random.Range(-RandomOffset, RandomOffset));
            var explosion = Instantiate(prefab);
            explosion.transform.position = tile.WorldCoords;
        }
    }
}
