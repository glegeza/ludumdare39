
namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;
    using JetBrains.Annotations;
    using Units;

    [RequireComponent(typeof(SpriteRenderer))]
    [UsedImplicitly]
    public class SelectedUnitBox : MonoBehaviour
    {
        private GameObject _currentSelection;
        private SpriteRenderer _renderer;

        [UsedImplicitly]
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            ActiveSelectionTracker.Instance.SelectionChanged += OnSelectionChanged;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (_currentSelection != null)
            {
                transform.position = _currentSelection.transform.position;
            }
            
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;

            if (_currentSelection != null && newSelection != _currentSelection)
            {
                var currentUnit = _currentSelection.GetComponent<GameUnit>();
                if (currentUnit != null)
                {
                    currentUnit.UnitDestroyed -= OnUnitDestroyed;
                }
            }

            if (newSelection == null)
            {
                _currentSelection = null;
                _renderer.enabled = false;
                return;
            }

            _currentSelection = newSelection;
            _renderer.enabled = true;
            var target = FindObjectOfType<CameraTargeter>();
            if (target != null)
            {
                target.TargetUnit(_currentSelection);
            }
            transform.localPosition = Vector3.zero;
            var unit = newSelection.GetComponent<GameUnit>();
            if (unit != null)
            {
                unit.UnitDestroyed += OnUnitDestroyed;
            }
        }

        private void OnUnitDestroyed(object sender, EventArgs e)
        {
            _currentSelection = null;
            _renderer.enabled = false;
        }
    }
}
