namespace DLS.LD39.MouseInput
{
    using JetBrains.Annotations;
    using Units;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Input Handlers/Unit Selector")]
    [UsedImplicitly]
    public class UnitSelector : BaseClickHandler<GameUnit>
    {
        public override bool HandleClick(GameUnit comp, int btn, Vector2 hitPoint)
        {
            if (btn == 0)
            {
                ActiveSelectionTracker.Instance.SetSelection(comp);
                return true;
            }
            if (btn == 1)
            {

            }

            return true;
        }
    }
}
