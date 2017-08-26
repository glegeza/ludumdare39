namespace DLS.LD39.Props
{
    using Map;
    using JetBrains.Annotations;
    using UnityEngine;

    [RequireComponent(typeof(TilePosition))]
    public class Prop : MonoBehaviour
    {
        public PropData Data
        {
            get; private set;
        }

        public TilePosition Position
        {
            get; private set;
        }

        public void Initialize(PropData data)
        {
            Data = data;
        }

        [UsedImplicitly]
        private void Awake()
        {
            Position = GetComponent<TilePosition>();
        }
    }
}
