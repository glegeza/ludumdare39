namespace DLS.LD39.Props
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    class Prop : MonoBehaviour
    {
        public PropLayer Layer;
        public bool Passable;

        private SpriteRenderer _renderer;

        public TilePosition Position
        {
            get; private set;
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            Position = GetComponent<TilePosition>();
        }

        private void Update()
        {
            _renderer.sortingOrder = (int)((transform.position.y * 100) * -1);
        }
    }
}
