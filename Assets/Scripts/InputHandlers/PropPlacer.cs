namespace DLS.LD39.InputHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DLS.LD39.Map;
    using DLS.LD39.Props;
    using UnityEngine;

    class PropPlacer : IMapClickInputHandler
    {
        public bool HandleButtonDown(int button, Tile tile)
        {
            return false;
        }

        public bool HandleTileClick(int button, Tile clickedTile)
        {
            var wallPrefab = PropRepository.Instance.WallPrefab;
            var obj = GameObject.Instantiate(wallPrefab);
            var pos = obj.GetComponent<TilePosition>();
            pos.SetTile(clickedTile);
            return false;
        }
    }
}
