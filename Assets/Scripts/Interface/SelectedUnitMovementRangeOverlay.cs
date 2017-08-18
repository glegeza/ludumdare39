namespace DLS.LD39.Interface
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using DLS.LD39.Units.Movement;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SelectedUnitMovementRangeOverlay : SingletonComponent<SelectedUnitMovementRangeOverlay>
    {
        public int MarkerPoolSize = 100;
        public GameObject MarkerPrefab;

        private UnitMovementHelper _movementHelper = new UnitMovementHelper();
        private GameObject _markerPoolContainer;
        private List<GameObject> _markerPool = new List<GameObject>();
        private GameUnit _trackedObject = null;
        private HashSet<Tile> _currenTileList = new HashSet<Tile>();

        private void Start()
        {
            _markerPoolContainer = new GameObject("Movement Range Overlay Pool");
            _markerPoolContainer.transform.SetParent(transform, false);
            _markerPoolContainer.transform.position = Vector3.zero;

            for (var i = 0; i < MarkerPoolSize; i++)
            {
                var newMarker = Instantiate(MarkerPrefab, _markerPoolContainer.transform, false);
                newMarker.SetActive(false);
                _markerPool.Add(newMarker);
            }

            ActiveSelectionTracker.Instance.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;
            if (newSelection == null)
            {
                return;
            }

            var unit = newSelection.GetComponent<GameUnit>();
            if (unit == null && _trackedObject != null)
            {
                _trackedObject.TurnBegan -= OnTurnStarted;
                _trackedObject.MoveAction.StartedAction -= OnBeginMove;
                _trackedObject.MoveAction.CompletedAction -= OnFinishedMove;
                _trackedObject = null;
            }
            else if (_trackedObject == unit)
            {
                return;
            }

            _trackedObject = unit;
            _trackedObject.TurnBegan += OnTurnStarted;
            _trackedObject.MoveAction.StartedAction += OnBeginMove;
            _trackedObject.MoveAction.CompletedAction += OnFinishedMove;

            StartCoroutine(_movementHelper.GetReachableTilesFast(_trackedObject.Position.CurrentTile,
                _trackedObject.AP.PointsRemaining, UpdateTilesImmediate));
        }

        private void OnTurnStarted(object sender, EventArgs e)
        {
            if (_trackedObject == null)
            {
                ReturnMarkersToPool();
                return;
            }

            StartCoroutine(_movementHelper.GetReachableTilesFast(_trackedObject.Position.CurrentTile,
                _trackedObject.AP.PointsRemaining, UpdateTilesImmediate));
        }

        private void OnBeginMove(object sender, EventArgs e)
        {
            BeginUpdate();
        }

        private void OnFinishedMove(object sender, EventArgs e)
        {
            UpdateTilesImmediate(_currenTileList);
        }

        private void BeginUpdate()
        {
            if (_trackedObject == null)
            {
                ReturnMarkersToPool();
                return;
            }

            StartCoroutine(_movementHelper.GetReachableTilesFast(_trackedObject.Position.CurrentTile,
                _trackedObject.AP.PointsRemaining, SetTiles));
            
        }

        private void UpdateTilesImmediate(HashSet<Tile> reachableTiles)
        {
            ReturnMarkersToPool();

            var idx = 0;
            foreach (var tile in reachableTiles)
            {
                var marker = _markerPool[idx++];
                marker.SetActive(true);
                marker.transform.position = tile.WorldCoords;
            }
        }

        private void SetTiles(HashSet<Tile> reachableTiles)
        {
            _currenTileList = reachableTiles;
        }

        private void ReturnMarkersToPool()
        {
            foreach (var marker in _markerPool)
            {
                if (!marker.activeSelf)
                {
                    break;
                }
                marker.transform.SetParent(_markerPoolContainer.transform, false);
                marker.transform.position = Vector3.zero;
                marker.SetActive(false);
            }
        }
    }
}
