
namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;
    using DLS.LD39.Units;

    [RequireComponent(typeof(SpriteRenderer))]
    public class ActiveTargetBox : MonoBehaviour
    {
        private GameUnit _currentSelection;
        private SpriteRenderer _renderer;
        private bool _shouldDisplay;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            ActiveSelectionTracker.Instance.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var selection = GetUnit(ActiveSelectionTracker.Instance.SelectedObject);
            if (selection == null || selection == _currentSelection)
            {
                _renderer.enabled = false;
                transform.SetParent(null);
                _shouldDisplay = false;
                return;
            }

            _shouldDisplay = true;
            _currentSelection = selection;
        }

        private GameUnit GetUnit(GameObject unit)
        {
            return unit == null ? null : unit.GetComponent<GameUnit>();
        }

        private void Update()
        {
            if (!_shouldDisplay)
            {
                return;
            }

            if (_currentSelection.CurrentTarget == null)
            {
                _renderer.enabled = false;
                transform.SetParent(null);
                return;
            }

            _renderer.enabled = true;
            transform.SetParent(_currentSelection.CurrentTarget.transform);
            transform.localPosition = Vector3.zero;
        }
    }
}