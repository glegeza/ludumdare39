namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;
    using JetBrains.Annotations;
    using Units;

    [RequireComponent(typeof(SpriteRenderer))]
    public class ActiveUnitBox : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private GameUnit _targetSelection;
        private bool _shouldDisplay;

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

        [UsedImplicitly]
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            TurnOrderTracker.Instance.TurnAdvanced += OnTurnAdvanced;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (!_shouldDisplay)
            {
                return;
            }

            transform.position = _targetSelection.transform.position;
            _renderer.enabled = true;
        }
    }
}