namespace DLS.LD39.AI.Decisions
{
    using UnityEngine;
    using Data;

    [CreateAssetMenu(menuName = "AI/Decisions/If Arrived At Target Tile")]
    public class IfArrivedAtTargetTile : Decision
    {
        public override bool Decide(StateController controller)
        {
            var pathData = controller.Data as IMovePathData;

            return pathData != null && Equals(controller.Unit.Position.CurrentTile, pathData.MoveTarget);
        }
    }
}
