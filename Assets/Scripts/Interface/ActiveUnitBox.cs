
namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;

    [RequireComponent(typeof(SpriteRenderer))]
    public class ActiveUnitBox : MonoBehaviour
    {
        private SpriteRenderer _renderer;
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
                _renderer.enabled = false;
                transform.SetParent(null);
                _shouldDisplay = false;
                return;
            }

            _renderer.enabled = true;
            _shouldDisplay = true;
            transform.SetParent(newSelection.transform, false);
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
                _renderer.enabled = true;
            }
        }
    }
}