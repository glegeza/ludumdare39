namespace DLS.LD39.Pathfinding
{
    using Units;
    using Units.Movement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using UnityEngine;

    public class SelectedUnitPathMarker : MonoBehaviour
    {
        public int MarkerPoolSize = 100;
        #pragma warning disable 0649
        public GameObject MarkerPrefab;
        public Sprite TurnPath;
        public Sprite FuturePath;
        public Sprite TargetPath;
        #pragma warning restore 0649

        private static SelectedUnitPathMarker _instance;

        private readonly UnitMovementHelper _movementHelper = new UnitMovementHelper();
        private GameObject _markerPoolContainer;
        private GameObject _pathContainer;
        private readonly List<GameObject> _markerPool = new List<GameObject>();
        private GameObject _trackedObject;
        private UnitPathfinder _trackedPathfinder;

        public static SelectedUnitPathMarker Instance
        {
            get
            {
                return _instance;
            }
        }

        [UsedImplicitly]
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

        [UsedImplicitly]
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

            var unit = _trackedObject.GetComponent<GameUnit>();
            var max = MarkerPoolSize + 1;
            if (unit != null)
            {
                max = _movementHelper.GetMaxMovementAlongPath(unit, _trackedPathfinder.Path);
            }
            var idx = 0;
            foreach (var step in _trackedPathfinder.Path)
            {
                var currentMarker = _markerPool[idx];
                currentMarker.SetActive(true);
                currentMarker.transform.position = step.WorldCoords;
                var spriteRenderer = currentMarker.GetComponent<SpriteRenderer>();
                if (idx == _trackedPathfinder.Path.Count())
                {
                    spriteRenderer.sprite = TargetPath;
                }
                else if (idx >= max)
                {
                    spriteRenderer.sprite = FuturePath;
                }
                else
                {
                    spriteRenderer.sprite = TurnPath;
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
                    break;
                }
                marker.transform.SetParent(_markerPoolContainer.transform, false);
                marker.transform.position = Vector3.zero;
                marker.SetActive(false);
            }
        }
    }
}
