namespace DLS.LD39.InputHandlers
{
    using DLS.LD39.Map;
    using DLS.LD39.Props;
    using UnityEngine;

    class PropPlacer : IMapClickInputHandler
    {
        private enum Mode
        {
            DoNothing,
            PlaceWall,
            RemoveWall
        }

        private Mode _currentMode = Mode.DoNothing;

        public bool HandleButtonDown(int button, Tile clickedTile)
        {
            if (button != 0 || _currentMode == Mode.DoNothing)
            {
                return false;
            }

            var currentProp = clickedTile.PropOnLayer(PropLayer.Wall);

            if (_currentMode == Mode.PlaceWall && currentProp == null)
            {
                PropFactory.Instance.BuildPropAndAddToTile("test_wall", clickedTile);
            }
            else if (_currentMode == Mode.RemoveWall && currentProp != null)
            {
                clickedTile.RemoveProp(PropLayer.Wall);
                GameObject.Destroy(currentProp.gameObject);
            }
            
            return false;
        }

        public bool HandleTileClick(int button, Tile clickedTile)
        {
            if (button != 0 || clickedTile == null)
            {
                return false;
            }

            var prop = clickedTile.PropOnLayer(PropLayer.Wall);

            _currentMode = prop == null ? Mode.PlaceWall : Mode.RemoveWall;

            return false;
        }
    }
}
