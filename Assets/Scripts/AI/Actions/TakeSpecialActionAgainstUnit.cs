namespace DLS.LD39.AI.Actions
{
    using Data;
    using JetBrains.Annotations;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Actions/Take Special Action Against Unit")]
    [UsedImplicitly]
    public class TakeSpecialActionAgainstUnit : AIAction
    {
        public LD39.Actions.Action Action;

        public override bool Act(StateController controller)
        {
            var trackedUnit = controller.Data as TrackedTargetData;

            if (trackedUnit == null)
            {
                Debug.LogError("TakeSpecialActionAgainstUnit missing TrackedTargetData");
                return false;
            }
            if (trackedUnit.CurrentTarget == null)
            {
                return false;
            }

            return controller.Unit.ActionController.TryAction(Action.ID, trackedUnit.CurrentTarget.gameObject,
                trackedUnit.CurrentTarget.Position.CurrentTile);
        }
    }
}
