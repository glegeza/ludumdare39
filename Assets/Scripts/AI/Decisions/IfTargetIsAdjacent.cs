namespace DLS.LD39.AI.Decisions
{
    using DLS.LD39.AI.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Decisions/If Target Is Adjacent")]
    public class IfTargetIsAdjacent : Decision
    {
        public override bool Decide(StateController controller)
        {
            var trackedData = controller.Data as TrackedTargetData;

            if (trackedData == null)
            {
                Debug.LogError("IfTargetIsAdjacent missing TrackedTargetData");
            }

            return controller.Unit.Position.CurrentTile.IsAdjacent(
                trackedData.CurrentTarget.Position.CurrentTile);
        }
    }
}
