namespace DLS.LD39.Interface.TurnOrder
{
    using Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
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
        private readonly List<Image> _waitingUnitObjects = new List<Image>();

        [UsedImplicitly]
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

        [UsedImplicitly]
        private void Start()
        {
            _tracker.TurnOrderUpdated += OnTurnOrderChanged;
            UpdateTracker();
        }

        private void OnTurnOrderChanged(object sender, EventArgs e)
        {
            UpdateTracker();
        }

        private void UpdateTracker()
        {
            var curUnit = _tracker.ActiveUnit;
            var prevUnit = _tracker.PreviousUnit;

            SetRenderer(PrevUnit, prevUnit);
            SetRenderer(ActiveUnit, curUnit);
            SetWaitingObjects(GetAvailableImageSlots());
        }

        private void SetWaitingObjects(IList<Image> imageSlots)
        {
            // returns the number of available turn order slots that haven't been used
            var remainingSlots = imageSlots.Count;
            var waiting = GetWaitingUnits();

            for (var i = 0; i < remainingSlots; i++)
            {
                if (i >= waiting.Count)
                {
                    break;
                }
                SetRenderer(imageSlots[i], waiting[i]);
            }
        }

        private static void SetRenderer(Image imgRenderer, GameUnit unit)
        {
            if (unit != null)
            {
                imgRenderer.gameObject.SetActive(true);
                imgRenderer.sprite = unit.Data.GraphicsData.IconSprite;
                imgRenderer.color = unit.Data.SpriteTint;
            }
            else
            {
                imgRenderer.gameObject.SetActive(false);
            }
        }

        private IList<Image> GetAvailableImageSlots()
        {
            var imageSlots = new List<Image>();
            if (_tracker.PreviousUnit == null)
            {
                imageSlots.Add(PrevUnit);
            }
            if (_tracker.ActiveUnit == null)
            {
                imageSlots.Add(ActiveUnit);
            }

            imageSlots.AddRange(_waitingUnitObjects);

            return imageSlots;
        }

        private List<GameUnit> GetWaitingUnits()
        {
            var waiting = _tracker.UnitsWaiting.Where(unit => unit != _tracker.PreviousUnit).ToList();
            waiting.AddRange(_tracker.UnitsDone.Where(unit => unit != _tracker.PreviousUnit));

            return waiting;
        }
    }
}
