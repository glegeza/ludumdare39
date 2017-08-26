
namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;
    using JetBrains.Annotations;
    using Units;

    [RequireComponent(typeof(SpriteRenderer))]
    [UsedImplicitly]
    public class ActiveTargetBox : MonoBehaviour
    {
        private GameUnit _currentSelection;
        private SpriteRenderer _renderer;
        private bool _shouldDisplay;

        [UsedImplicitly]
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            ActiveSelectionTracker.Instance.SelectionChanged += OnSelectionChanged;
        }

        [UsedImplicitly]
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
            transform.position = _currentSelection.CurrentTarget.transform.position;
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var selection = GetUnit(ActiveSelectionTracker.Instance.SelectedObject);
            if (selection == null || selection == _currentSelection)
            {
                _renderer.enabled = false;
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
    }
}