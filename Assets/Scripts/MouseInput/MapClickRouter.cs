namespace DLS.LD39.MouseInput
{
    using InputHandlers;
    using DLS.LD39.Map;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;

    /// <summary>
    /// Responsible for calling the appropriate IMapClickInputHandler when the
    /// player clicks the mouse anywhere on the map. 
    /// </summary>
    class MapClickRouter : MonoBehaviour
    {
        private static MapClickRouter _instance;
        
        private TilePicker _picker;

        private Dictionary<string, IMapClickInputHandler> _inputHandlers =
            new Dictionary<string, IMapClickInputHandler>();

        private IMapClickInputHandler _activeHandler;
        
        public static MapClickRouter Instance
        {
            get
            {
                return _instance;
            }
        }

        public IMapClickInputHandler ActiveMode
        {
            get
            {
                return _activeHandler;
            }
        }

        public void AddHandler(IMapClickInputHandler handler)
        {
            _inputHandlers.Add(handler.HandlerID, handler);
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
                _activeHandler = null;
            }
            else
            {
                _activeHandler = newHandler;
            }
        }

        public void Initialize()
        {
            _picker = new TilePicker();

            AddHandler(new UnitSpawnModeInputHandler());
            AddHandler(new TileEditModeInputHandler());
            AddHandler(new UnitControlModeInputHandler());
            AddHandler(new PropEditModeInputHandler());
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
            if (_activeHandler == null || EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            var checkedForTarget = false;
            Tile targetTile = null;
            for (var btn = 0; btn < 3; btn++)
            {
                if (Input.GetMouseButtonDown(btn))
                {
                    if (targetTile == null && !checkedForTarget)
                    {
                        targetTile = _picker.GetTileAtScreenPosition(Input.mousePosition);
                        checkedForTarget = true;
                    }

                    if (targetTile != null)
                    {
                        _activeHandler.HandleTileClick(btn, targetTile);
                    }
                }
                else if (Input.GetMouseButton(btn))
                {
                    if (targetTile == null && !checkedForTarget)
                    {
                        targetTile = _picker.GetTileAtScreenPosition(Input.mousePosition);
                        checkedForTarget = true;
                    }

                    if (targetTile != null)
                    {
                        _activeHandler.HandleButtonDown(btn, targetTile);
                    }
                }
            }
        }
    }
}
