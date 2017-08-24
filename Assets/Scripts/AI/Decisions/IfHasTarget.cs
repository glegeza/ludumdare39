namespace DLS.LD39.AI.Decisions
{
    using DLS.LD39.Units;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Decisions/If Has Target")]
    public class IfHasTarget : Decision
    {
        public Faction TargetFaction;

        public override bool Decide(StateController controller)
        {
            var visibleUnits = controller.Unit.Visibility.VisibleUnits;

            return visibleUnits.Any(u => u.Faction == TargetFaction);
        }

    }
}
