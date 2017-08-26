namespace DLS.LD39.Interface.Actions
{
    using LD39.Actions;
    using Units;
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    public class ActionButtonPanel : MonoBehaviour
    {
        public UnitActionButton ButtonPrefab;

        private TurnOrderTracker _tracker;

        [UsedImplicitly]
        private void Awake()
        {
            _tracker = TurnOrderTracker.Instance;
        }

        [UsedImplicitly]
        private void Start()
        {
            _tracker.TurnAdvanced += OnTurnAdvanced;
        }

        private void OnTurnAdvanced(object sender, EventArgs e)
        {
            if (_tracker.ActiveUnit != null && _tracker.ActiveUnit.Faction == Faction.Player)
            {
                UpdatePanel(_tracker.ActiveUnit);
                return;
            }

            DisablePanel();
        }

        private void DisablePanel()
        {
            ClearButtons();
            gameObject.SetActive(false);
        }

        private void UpdatePanel(GameUnit currentUnit)
        {
            ClearButtons();
            var actionController = currentUnit.GetComponent<UnitActionController>();
            if (actionController == null)
            {
                return;
            }

            gameObject.SetActive(true);

            foreach (var action in actionController.Actions)
            {
                var actionButton = Instantiate(ButtonPrefab);
                actionButton.transform.SetParent(transform, false);
                actionButton.SetAction(action, actionController);
            }
        }

        private void ClearButtons()
        {
            var buttons = GetComponentsInChildren<UnitActionButton>();
            foreach (var button in buttons)
            {
                Destroy(button.gameObject);
            }
        }
    }
}
