namespace DLS.LD39.Interface
{
    using Units;
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    public class SelectedUnitVisibilityMarker : SingletonComponent<SelectedUnitVisibilityMarker>
    {
        public int MarkerPoolSize = 30;
        public GameObject MarkerPrefab;

        private GameObject _markerPoolContainer;
        private List<GameObject> _markerPool = new List<GameObject>();
        private GameUnit _trackedObject;
        
        private static GameUnit GetTrackedUnit()
        {
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;
            return newSelection == null ? null : newSelection.GetComponent<GameUnit>();
        }
        
        [UsedImplicitly]
        private void Start()
        {
            _markerPoolContainer = new GameObject("Visibility Marker Pool");
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
            var unit = GetTrackedUnit();
            if (unit == null && _trackedObject != null)
            {
                ClearOldTrackedObject();
            }
            else if (_trackedObject == unit)
            {
                return;
            }

            SetNewTrackedObject(unit);
            UpdateMarkers();
        }

        private void OnVisibilityUpdated(object sender, EventArgs e)
        {
            UpdateMarkers();
        }

        private void ClearOldTrackedObject()
        {
            _trackedObject.Visibility.VisibilityUpdated -= OnVisibilityUpdated;
            _trackedObject = null;
        }

        private void SetNewTrackedObject(GameUnit unit)
        {
            _trackedObject = unit;
            if (_trackedObject == null) return;
            _trackedObject.Visibility.VisibilityUpdated += OnVisibilityUpdated;
        }

        private void UpdateMarkers()
        {
            ReturnMarkersToPool();

            if (_trackedObject == null)
            {
                return;
            }

            var visibility = _trackedObject.Visibility;

            var idx = 0;
            foreach (var tile in visibility.VisibleTiles)
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
