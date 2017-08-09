namespace DLS.LD39.Pathfinding
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    class SelectedUnitPathMarker : MonoBehaviour
    {
        public int MarkerPoolSize = 100;
        public GameObject MarkerPrefab;
        public Sprite TurnPath;
        public Sprite FuturePath;
        public Sprite TargetPath;

        private static SelectedUnitPathMarker _instance;

        private GameObject _markerPoolContainer;
        private GameObject _pathContainer;
        private List<GameObject> _markerPool = new List<GameObject>();
        private GameObject _trackedObject = null;
        private UnitPathfinder _trackedPathfinder = null;
        private MoveController _trackedMover;

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

            var mover = _trackedObject.GetComponent<MoveController>();
            var max = MarkerPoolSize + 1;
            if (mover != null)
            {
                max = mover.GetMaxMovementThisTurn(_trackedPathfinder.Path);
            }
            var idx = 0;
            foreach (var step in _trackedPathfinder.Path)
            {
                var currentMarker = _markerPool[idx];
                currentMarker.SetActive(true);
                currentMarker.transform.position = step.WorldCoords;
                var renderer = currentMarker.GetComponent<SpriteRenderer>();
                if (idx == _trackedPathfinder.Path.Count())
                {
                    renderer.sprite = TargetPath;
                }
                else if (idx >= max)
                {
                    renderer.sprite = FuturePath;
                }
                else
                {
                    renderer.sprite = TurnPath;
                }
                idx++;
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
