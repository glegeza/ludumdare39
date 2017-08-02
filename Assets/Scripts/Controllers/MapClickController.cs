namespace DLS.LD39.Controllers
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    class MapClickController : MonoBehaviour
    {
        private static MapClickController _instance;

        private TileMap _tileMap;
        private TilePicker _picker;

        private IMapClickInputHandler _spawnHandler;
        private IMapClickInputHandler _moveHandler;
        private IMapClickInputHandler _editHandler;

        private IMapClickInputHandler _activeHandler;

        public static MapClickController Instance
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

        public void SetMoveMode()
        {
            _activeHandler = _moveHandler;
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
            _tileMap = FindObjectOfType<TileMap>();
            _picker = new TilePicker(_tileMap);

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
