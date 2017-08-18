
namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;
    using DLS.LD39.Units;

    [RequireComponent(typeof(SpriteRenderer))]
    public class SelectedUnitBox : MonoBehaviour
    {
        private GameObject _currentSelection;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            ActiveSelectionTracker.Instance.SelectionChanged += OnSelectionChanged;
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

        private void Update()
        {
            if (_currentSelection != null)
            {
                transform.position = _currentSelection.transform.position;
            }
            
        }
    }
}
