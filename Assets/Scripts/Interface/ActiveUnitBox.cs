
namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;
    using DLS.LD39.Units;

    [RequireComponent(typeof(SpriteRenderer))]
    public class ActiveUnitBox : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private GameUnit _targetSelection;
        private bool _shouldDisplay;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            TurnOrderTracker.Instance.TurnAdvanced += OnTurnAdvanced;
        }

        private void OnTurnAdvanced(object sender, EventArgs e)
        {
            var newSelection = TurnOrderTracker.Instance.ActiveUnit;

            if (newSelection == null)
            {
                _targetSelection = null;
                _renderer.enabled = false;
                _shouldDisplay = false;
                return;
            }

            _renderer.enabled = true;
            _shouldDisplay = true;
            _targetSelection = newSelection;
            transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            if (!_shouldDisplay)
            {
                return;
            }

            if (TurnOrderTracker.Instance.ActiveUnit.gameObject == ActiveSelectionTracker.Instance.SelectedObject)
            {
                _renderer.enabled = false;
            }
            else
            {
                transform.position = _targetSelection.transform.position;
                _renderer.enabled = true;
            }
        }
    }
}