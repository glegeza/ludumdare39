namespace DLS.LD39.Interface
{
    using Pathfinding;
    using Map;
    using Units;
    using Units.Movement;
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;

    public class SelectedUnitMovementRangeOverlay : SingletonComponent<SelectedUnitMovementRangeOverlay>
    {
        public int MarkerPoolSize = 100;
        public GameObject MarkerPrefab;

        private readonly List<GameObject> _markerPool = new List<GameObject>();
        private readonly UnitMovementHelper _movementHelper = new UnitMovementHelper();
        private bool _isPathing;
        private GameObject _markerPoolContainer;
        private GameUnit _trackedObject;

        private static GameUnit GetSelectedUnit()
        {
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;
            return newSelection == null ? null : newSelection.GetComponent<GameUnit>();
        }

        [UsedImplicitly]
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
            _isPathing = false;
            var unit = GetSelectedUnit();
            if (unit == null && _trackedObject != null)
            {
                ClearOldTrackedObject();
            }
            else if (_trackedObject == unit)
            {
                return;
            }
            
            SetNewTrackedObject(unit);
            if (_trackedObject != null)
            {
                StartCoroutine(_movementHelper.GetReachableTilesFast(_trackedObject.Position.CurrentTile,
                    _trackedObject.AP.PointsRemaining, UpdateTilesImmediate));
            }
        }

        private void ClearOldTrackedObject()
        {
            _trackedObject.TurnBegan -= OnTurnStarted;
            var oldPathController = _trackedObject.GetComponent<UnitPathfinder>();
            _trackedObject = null;

            if (oldPathController == null) return;
            oldPathController.StartPathMovement -= OnPathStarted;
            oldPathController.TurnMoveComplete -= OnPathFinished;
        }

        private void SetNewTrackedObject(GameUnit unit)
        {
            _trackedObject = unit;
            _trackedObject.TurnBegan += OnTurnStarted;
            var pathController = _trackedObject.GetComponent<UnitPathfinder>();

            if (pathController == null) return;
            pathController.StartPathMovement += OnPathStarted;
            pathController.TurnMoveComplete += OnPathFinished;
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        private void OnPathFinished(object sender, EventArgs e)
        {
            _isPathing = false;
            StartCoroutine(_movementHelper.GetReachableTilesFast(_trackedObject.Position.CurrentTile,
                _trackedObject.AP.PointsRemaining, UpdateTilesImmediate));
        }

        private void OnPathStarted(object sender, EventArgs e)
        {
            _isPathing = true;
            ReturnMarkersToPool();
        }

        private void OnTurnStarted(object sender, EventArgs e)
        {
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
