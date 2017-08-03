namespace DLS.LD39.Interface
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    class UnitAPCounter : MonoBehaviour
    {
        private Text _text;
        private GameObject _currentObject;
        private GameUnit _currentUnit;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

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
