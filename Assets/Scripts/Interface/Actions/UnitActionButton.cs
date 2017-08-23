namespace DLS.LD39.Interface.Actions
{
    using UnityEngine;
    using UnityEngine.UI;
    using DLS.LD39.Actions;
    using DLS.LD39.Units;
    using DLS.LD39.Map;

    [RequireComponent(typeof(Button))]
    public class UnitActionButton : MonoBehaviour
    {
        private Button _button;
        private Text _buttonText;

        private Action _action;
        private GameUnit _unit;
        private UnitActionController _controller;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _buttonText = GetComponentInChildren<Text>();
        }

        public void SetAction(Action action, UnitActionController controller)
        {
            if (action == null || controller == null)
            {
                Debug.LogError("SetAction - action or controller null");
                return;
            }

            _action = action;
            _unit = controller.AttachedUnit;
            _controller = controller;

            _button.onClick.RemoveAllListeners();
            _buttonText.text = action.GetName(controller.AttachedUnit);
            _button.onClick.AddListener(DoAction);
        }

        private void DoAction()
        {
            if (!IsReady())
            {
                return;
            }

            Tile targetTile = null;
            GameObject targetObject = null;
            if (_unit.CurrentTarget != null)
            {
                targetTile = _unit.CurrentTarget.Position.CurrentTile;
                targetObject = _unit.CurrentTarget.gameObject;
            }

            _controller.TryAction(_action.ID, targetObject, targetTile);
        }

        private bool IsReady()
        {
            return _action != null && _unit != null && _controller != null;
        }
    }
}
