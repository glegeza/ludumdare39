namespace DLS.LD39.AI.Decisions
{
    using System.Linq;
    using LD39.AI.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Decisions/If Lost Target")]
    public class IfLostTarget : Decision
    {
        public override bool Decide(StateController controller)
        {
            var targetData = controller.Data as TrackedTargetData;
            if (targetData == null || targetData.CurrentTarget.gameObject == null)
            {
                return true;
            }

            if (!controller.Unit.Visibility.VisibleUnits.Contains(targetData.CurrentTarget))
            {
                return true;
            }

            return false;
        }
    }
}
