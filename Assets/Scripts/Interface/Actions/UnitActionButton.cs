namespace DLS.LD39.Interface.Actions
{
    using UnityEngine;
    using UnityEngine.UI;
    using DLS.LD39.Actions;

    [RequireComponent(typeof(Button))]
    public class UnitActionButton : MonoBehaviour
    {
        private Button _button;
        private Text _buttonText;

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

            _button.onClick.RemoveAllListeners();
            _buttonText.text = action.GetName(controller.AttachedUnit);
        }
    }
}
