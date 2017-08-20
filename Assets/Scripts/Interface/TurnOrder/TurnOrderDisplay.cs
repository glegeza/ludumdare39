namespace DLS.LD39.Interface.TurnOrder
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    public class TurnOrderDisplay : MonoBehaviour
    {
        #pragma warning disable 0649
        public Image PrevUnit;
        public Image ActiveUnit;
        public GameObject UnitListContainer;
        #pragma warning restore 0649

        private TurnOrderTracker _tracker;
        private List<Image> _waitingUnitObjects = new List<Image>();

        private void Awake()
        {
            _tracker = TurnOrderTracker.Instance;
            if (_tracker == null)
            {
                Debug.LogError("TurnOrderDisplay could not locate TurnOrderTracker");
                return;
            }
            if (PrevUnit == null)
            {
                Debug.LogError("TurnOrderDisplay missing PrevUnit reference.");
                return;
            }
            if (ActiveUnit == null)
            {
                Debug.LogError("TurnOrderDisplay missing ActiveUnit reference.");
                return;
            }
            if (UnitListContainer == null)
            {
                Debug.LogError("TurnOrderDisplay missing UnitListContainer reference.");
                return;
            }

            var spriteChildren = UnitListContainer.GetComponentsInChildren<Image>();
                _waitingUnitObjects.AddRange(
                    spriteChildren.OrderBy(s => s.transform.GetSiblingIndex()));
            foreach (var unitObject in _waitingUnitObjects)
            {
                unitObject.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _tracker.TurnOrderUpdated += OnTurnOrderChanged;
        }

        private void OnTurnOrderChanged(object sender, EventArgs e)
        {
            var curUnit = _tracker.ActiveUnit;
            var prevUnit = _tracker.PreviousUnit;
            var waiting = GetWaitingUnits();

            SetRenderer(PrevUnit, prevUnit);
            SetRenderer(ActiveUnit, curUnit);

            var idx = 0;
            for (var i = 0; i < _waitingUnitObjects.Count; i++)
            {
                if (i >= waiting.Count())
                {
                    break;
                }
                SetRenderer(_waitingUnitObjects[i], waiting[i]);
            }
            

            Debug.Log("Turn order updated");
        }

        private void SetRenderer(Image renderer, GameUnit unit)
        {
            if (unit != null)
            {
                renderer.gameObject.SetActive(true);
                renderer.sprite = unit.Data.GraphicsData.IconSprite;
                renderer.color = unit.Data.SpriteTint;
            }
            else
            {
                renderer.gameObject.SetActive(false);
            }
        }

        private List<GameUnit> GetWaitingUnits()
        {
            var waiting = new List<GameUnit>();

            foreach (var unit in _tracker.UnitsWaiting)
            {
                if (unit != _tracker.PreviousUnit)
                {
                    waiting.Add(unit);
                }
            }
            foreach (var unit in _tracker.UnitsDone)
            {
                if (unit != _tracker.PreviousUnit)
                {
                    waiting.Add(unit);
                }
            }

            return waiting;
        }
    }
}
