namespace DLS.LD39.Pathfinding
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    class SelectedUnitPathMarker : MonoBehaviour
    {
        public int MarkerPoolSize = 100;
        public GameObject MarkerPrefab;

        private static SelectedUnitPathMarker _instance;

        private GameObject _markerPoolContainer;
        private GameObject _pathContainer;
        private List<GameObject> _markerPool = new List<GameObject>();
        private GameObject _trackedObject = null;
        private UnitPathfinder _trackedPathfinder = null;

        public static SelectedUnitPathMarker Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            _markerPoolContainer = new GameObject("Marker Pool");
            _markerPoolContainer.transform.SetParent(transform, false);
            _markerPoolContainer.transform.position = Vector3.zero;

            _pathContainer = new GameObject("Unit Path");
            _pathContainer.transform.SetParent(transform, false);
            _pathContainer.transform.position = Vector3.zero;

            for (var i = 0; i < MarkerPoolSize; i++)
            {
                var newMarker = Instantiate(MarkerPrefab, _markerPoolContainer.transform, false);
                newMarker.SetActive(false);
                _markerPool.Add(newMarker);
            }
            ActiveSelectionTracker.Instance.SelectionChanged += OnObjectSelectionChanged;
        }

        private void OnObjectSelectionChanged(object sender, EventArgs e)
        {
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;
            if (newSelection == _trackedObject)
            {
                return;
            }
            if (_trackedPathfinder != null)
            {
                _trackedPathfinder.PathChanged -= OnPathChanged;
            }

            _trackedObject = newSelection;
            if (_trackedObject != null)
            {
                _trackedPathfinder = _trackedObject.GetComponent<UnitPathfinder>();
            }
            if (_trackedPathfinder != null)
            {
                _trackedPathfinder.PathChanged += OnPathChanged;
            }

            UpdatePath();
        }

        private void OnPathChanged(object sender, EventArgs e)
        {
            UpdatePath();
        }

        private void UpdatePath()
        {
            ReturnMarkersToPool();

            if (_trackedPathfinder == null || !_trackedPathfinder.Path.Any())
            {
                return;
            }

            var mover = _trackedObject.GetComponent<MoveAction>();
            var max = MarkerPoolSize + 1;
            if (mover != null)
            {
                max = mover.GetMaxMoveAlongPath(_trackedPathfinder.Path);
            }
            var idx = 0;
            foreach (var step in _trackedPathfinder.Path)
            {
                var currentMarker = _markerPool[idx++];
                currentMarker.SetActive(true);
                currentMarker.transform.position = step.WorldCoords;
                if (idx > max)
                {
                    currentMarker.GetComponent<MeshRenderer>().material = MaterialRepository.Instance.FuturePathMarkerMaterial;
                }
                else
                {
                    currentMarker.GetComponent<MeshRenderer>().material = MaterialRepository.Instance.PathMarkerMaterial;
                }
            }
        }

        private void ReturnMarkersToPool()
        {
            foreach (var marker in _markerPool)
            {
                if (!marker.activeSelf)
                {
                    marker.transform.SetParent(_markerPoolContainer.transform, false);
                    marker.transform.position = Vector3.zero;
                    break;
                }
                marker.SetActive(false);
            }
        }
    }
}
