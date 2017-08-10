namespace DLS.LD39.MouseInput
{
    using DLS.LD39.Units;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Input Handlers/Unit Selector")]
    public class UnitSelector : BaseClickHandler<GameUnit>
    {
        public override bool HandleClick(GameUnit comp, int btn, Vector2 hitPoint)
        {
            if (btn == 0)
            {
                ActiveSelectionTracker.Instance.SetSelection(comp);
                return true;
            }

            return true;
        }
    }
}
