namespace DLS.LD39.AI.TurnEnders
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Turn End Decisions/Failed Actions Threshold")]
    public class FailedActionsThreshold : TurnEndDecision
    {
        public int MaxAllowableFailedActions;

        public override bool ShouldEndTurn(StateController controller)
        {
            return MaxAllowableFailedActions > controller.ActionsFailedThisCycle;
        }
    }
}
