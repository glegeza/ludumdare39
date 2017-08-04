namespace DLS.LD39.Props
{
    using DLS.LD39.Map;
    using UnityEngine;

    [RequireComponent(typeof(TilePosition))]
    class Prop : MonoBehaviour
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

        private void Awake()
        {
            Position = GetComponent<TilePosition>();
        }
    }
}
