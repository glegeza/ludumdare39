namespace DLS.LD39.Generation
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Utility;

    [RequireComponent(typeof(TileMap))]
    public class RoomMap : MonoBehaviour
    {
        private TileMap _map;

        private void Awake()
        {
            _map = GetComponent<TileMap>();
        }

        private void Start()
        {
            var corridor = new SingleWidthCorridor();
            corridor.AddNode(new IntVector2(0, 0));
            corridor.AddNode(new IntVector2(10, 10));
            foreach (var tile in corridor.Tiles)
            {
                _map.SetTileAt(tile.X, tile.Y, "default");
            }
        }
    }
}
