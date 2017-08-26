namespace DLS.LD39.Interface
{
    using Units;
    using System;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    [UsedImplicitly]
    public class UnitAPCounter : MonoBehaviour
    {
        private Text _text;
        private GameObject _currentObject;
        private GameUnit _currentUnit;

        [UsedImplicitly]
        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        [UsedImplicitly]
        private void Update()
        {
            var currentSelection = ActiveSelectionTracker.Instance.SelectedObject;

            if (currentSelection == null)
            {
                _text.text = "No object selected";
            }
            else if (currentSelection != _currentObject)
            {
                _currentObject = currentSelection;
                _currentUnit = currentSelection.GetComponent<GameUnit>();
            }

            if (_currentUnit != null)
            {
                _text.text = String.Format("AP: {0}", _currentUnit.AP.PointsRemaining);
            }
        }
    }
}
