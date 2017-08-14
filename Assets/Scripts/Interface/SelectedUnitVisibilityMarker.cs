namespace DLS.LD39.Interface
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SelectedUnitVisibilityMarker : SingletonComponent<SelectedUnitVisibilityMarker>
    {
        public int MarkerPoolSize = 30;
        public GameObject MarkerPrefab;

        private GameObject _markerPoolContainer;
        private List<GameObject> _markerPool = new List<GameObject>();
        private GameUnit _trackedObject = null;

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
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;
            if (newSelection == null)
            {
                return;
            }

            var unit = newSelection.GetComponent<GameUnit>();
            if (unit == null && _trackedObject != null)
            {
                _trackedObject.Visibility.VisibilityUpdated -= OnVisibilityUpdated;
                _trackedObject = null;
            }
            else if (_trackedObject == unit)
            {
                return;
            }

            _trackedObject = unit;
            _trackedObject.Visibility.VisibilityUpdated += OnVisibilityUpdated;
            UpdateMarkers();
        }

        private void OnVisibilityUpdated(object sender, EventArgs e)
        {
            UpdateMarkers();
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
            foreach (var unit in visibility.VisibleUnits)
            {
                var marker = _markerPool[idx++];
                marker.SetActive(true);
                marker.transform.SetParent(unit.transform, false);
                marker.transform.localPosition = Vector3.zero;
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
