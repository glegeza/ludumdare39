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
                _trackedObject.TurnBegan -= OnChanged;
                _trackedObject.MoveAction.CompletedAction -= OnChanged;
                _trackedObject = null;
            }
            else if (_trackedObject == unit)
            {
                return;
            }

            _trackedObject = unit;
            _trackedObject.TurnBegan += OnChanged;
            _trackedObject.MoveAction.StartedAction += OnChanged;
            UpdateMarkers();
        }

        private void OnChanged(object sender, EventArgs e)
        {
            UpdateMarkers();
        }

        private void UpdateMarkers()
        {
            if (_trackedObject == null)
            {
                ReturnMarkersToPool();
                return;
            }

            StartCoroutine(_movementHelper.GetReachableTiles(_trackedObject.Position.CurrentTile,
                _trackedObject.AP.PointsRemaining, SetNewMarkers));
            
        }

        private void SetNewMarkers(HashSet<Tile> reachableTiles)
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
