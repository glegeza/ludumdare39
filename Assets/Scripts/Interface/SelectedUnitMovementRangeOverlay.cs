namespace DLS.LD39.Interface
{
    using Pathfinding;
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

        private bool _isPathing = false;
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
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;
            if (newSelection == null)
            {
                return;
            }

            var unit = newSelection.GetComponent<GameUnit>();
            _isPathing = false;
            if (unit == null && _trackedObject != null)
            {
                var oldPathController = _trackedObject.GetComponent<UnitPathfinder>();
                if (oldPathController != null)
                {
                    oldPathController.StartPathMovement -= OnPathStarted;
                    oldPathController.TurnMoveComplete -= OnPathFinished;
                }

                _trackedObject.TurnBegan -= OnTurnStarted;
                _trackedObject = null;
            }
            else if (_trackedObject == unit)
            {
                return;
            }

            _trackedObject = unit;
            var pathController = _trackedObject.GetComponent<UnitPathfinder>();
            if (pathController != null)
            {
                pathController.StartPathMovement += OnPathStarted;
                pathController.TurnMoveComplete += OnPathFinished;
            }
            _trackedObject.TurnBegan += OnTurnStarted;

            StartCoroutine(_movementHelper.GetReachableTilesFast(_trackedObject.Position.CurrentTile,
                _trackedObject.AP.PointsRemaining, UpdateTilesImmediate));
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        private void OnPathFinished(object sender, EventArgs e)
        {
            Debug.Log("OnPathFinished.");
            _isPathing = false;
            StartCoroutine(_movementHelper.GetReachableTilesFast(_trackedObject.Position.CurrentTile,
                _trackedObject.AP.PointsRemaining, UpdateTilesImmediate));
        }

        private void OnPathStarted(object sender, EventArgs e)
        {
            Debug.Log("OnPathStarted");
            _isPathing = true;
            ReturnMarkersToPool();
        }

        private void OnTurnStarted(object sender, EventArgs e)
        {
            Debug.Log("OnTurnStarted");
            if (_isPathing)
            {
                return;
            }
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
            Debug.Log("OnBeginMove");
            if (_isPathing)
            {
                return;
            }
            BeginUpdate();
        }

        private void OnFinishedMove(object sender, EventArgs e)
        {
            Debug.Log("OnFinishedMove");
            if (_isPathing)
            {
                return;
            }

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
            Debug.LogFormat("Reachable Tiles: {0}", reachableTiles.Count);
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
