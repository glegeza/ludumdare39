namespace DLS.LD39.AI.TurnEnders
{
    using DLS.LD39.Units;
    using UnityEngine;

    /// <summary>
    /// Ends the units turn if it exceeds the maximum number of turns on this
    /// state, falls below the minimum AP, OR falls below the minimum energy.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Turn End Decisions/Any Threshold")]
    public class AnyThreshold : TurnEndDecision
    {
        public int MaximumActions;
        public int MinimumAP;
        public int MinimumEnergy;

        public override bool ShouldEndTurn(StateController controller)
        {
            var energyComp = controller.Unit.GetComponent<EnergyPoints>();
            var energy = energyComp == null ? 0 : energyComp.PointsRemaining;
            var ap = controller.Unit.AP.PointsRemaining;
            var actions = controller.ActionCyclesThisTurn;

            return actions >= MaximumActions || ap < MinimumAP || energy < MinimumEnergy;
        }
    }
}