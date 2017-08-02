namespace DLS.LD39.Controllers
{
    using InputHandlers;
    using DLS.LD39.Map;
    using UnityEngine;

    /// <summary>
    /// Responsible for calling the appropriate IMapClickInputHandler when the
    /// player clicks the mouse anywhere on the map. 
    /// </summary>
    class MapClickRouter : MonoBehaviour
    {
        private static MapClickRouter _instance;
        
        private TilePicker _picker;

        private IMapClickInputHandler _spawnHandler;
        private IMapClickInputHandler _moveHandler;
        private IMapClickInputHandler _editHandler;

        private IMapClickInputHandler _activeHandler;

        public static MapClickRouter Instance
        {
            get
            {
                return _instance;
            }
        }

        public void ToggleSpawnMode()
        {
            if (_activeHandler == _spawnHandler)
            {
                _activeHandler = null;
            }
            else
            {
                _activeHandler = _spawnHandler;
            }
        }

        public void ToggleMoveMode()
        {
            if (_activeHandler == _moveHandler)
            {
                _activeHandler = null;
            }
            else
            {
                _activeHandler = _moveHandler;
            }
        }

        public void ToggleEditMode()
        {
            if (_activeHandler == _editHandler)
            {
                _activeHandler = null;
            }
            else
            {
                _activeHandler = _editHandler;
            }
        }

        public void Initialize()
        {
            _picker = new TilePicker();

            _spawnHandler = new UnitSpawnClickHandler();
            _editHandler = new MapClickEditor();
            _moveHandler = new UnitClickMover();
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Update()
        {
            if (_activeHandler == null)
            {
                return;
            }

            for (var btn = 0; btn < 3; btn++)
            {
                if (Input.GetMouseButtonDown(btn))
                {
                    var targetTile = _picker.GetTileAtScreenPosition(Input.mousePosition);
                    if (targetTile != null)
                    {
                        _activeHandler.HandleTileClick(btn, targetTile);
                    }
                }
            }
        }
    }
}
