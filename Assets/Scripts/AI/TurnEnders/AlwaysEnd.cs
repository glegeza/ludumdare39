namespace DLS.LD39.AI.TurnEnders
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Turn End Decisions/Always End")]
    public class AlwaysEnd : TurnEndDecision
    {
        public override bool ShouldEndTurn(StateController controller)
        {
            return true;
        }
    }
}
