namespace DLS.LD39.MouseInput
{
    using InputHandlers;
    using Map;
    using UnityEngine;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Responsible for calling the appropriate IMapClickInputHandler when the
    /// player clicks the mouse anywhere on the map. 
    /// </summary>
    [CreateAssetMenu(menuName ="Input Handlers/Tile Picker")]
    public class MapClickRouter : BaseClickHandler<TileMap>
    {
        private readonly Dictionary<string, IMapClickInputHandler> _inputHandlers =
            new Dictionary<string, IMapClickInputHandler>();

        private IMapClickInputHandler _activeHandler;
        private IMapClickInputHandler _defaultHandler;

        public IMapClickInputHandler ActiveMode
        {
            get
            {
                return _activeHandler;
            }
        }

        public void ToggleHandler(string id)
        {
            if (!_inputHandlers.ContainsKey(id))
            {
                Debug.LogErrorFormat("Unknown map click handler {0}", id);
                return;
            }

            var newHandler = _inputHandlers[id];
            if (_activeHandler != null && _activeHandler == newHandler)
            {
                _activeHandler = _defaultHandler;
            }
            else
            {
                _activeHandler = newHandler;
            }
        }

        public override bool HandleClick(TileMap tileMap, int btn, Vector2 hitPoint)
        {
            var tile = GetTile(tileMap, hitPoint);
            if (_activeHandler != null && tile != null)
            {
                _activeHandler.HandleTileClick(btn, tile);
                return true;
            }

            return false;
        }

        public override bool HandleMouseDown(TileMap tileMap, int btn, Vector2 hitPoint)
        {
            var tile = GetTile(tileMap, hitPoint);
            if (_activeHandler != null && tile != null)
            {
                _activeHandler.HandleButtonDown(btn, tile);
                return true;
            }

            return false;
        }

        [UsedImplicitly]
        private void OnEnable()
        {
            AddHandler(new UnitSpawnModeInputHandler());
            AddHandler(new TileEditModeInputHandler());
            AddHandler(new UnitControlModeInputHandler());
            AddHandler(new PropEditModeInputHandler());
            _defaultHandler = _inputHandlers["control"];
            _activeHandler = _defaultHandler;
        }

        private void AddHandler(IMapClickInputHandler handler)
        {
            _inputHandlers.Add(handler.HandlerID, handler);
        }

        private Tile GetTile(TileMap tileMap, Vector2 worldCoord)
        {
            int x, y;
            if (!tileMap.GetTileCoords(worldCoord, out x, out y))
            {
                return null;
            }
            return tileMap.GetTile(x, y);
        }
    }
}
