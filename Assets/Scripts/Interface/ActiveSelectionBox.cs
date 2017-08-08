
namespace DLS.LD39.Interface
{
    using UnityEngine;
    using System;

    [RequireComponent(typeof(SpriteRenderer))]
    public class ActiveSelectionBox : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            ActiveSelectionTracker.Instance.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var newSelection = ActiveSelectionTracker.Instance.SelectedObject;

            if (newSelection == null)
            {
                _renderer.enabled = false;
                transform.SetParent(null);
                return;
            }

            _renderer.enabled = true;
            transform.SetParent(newSelection.transform, false);
            transform.localPosition = Vector3.zero;
        }
    }
}
