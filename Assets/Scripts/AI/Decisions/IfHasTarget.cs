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
            Debug.Log("In IfHasTarget");
            var visibleUnits = controller.Unit.Visibility.VisibleUnits;

            foreach (var unit in visibleUnits)
            {
                Debug.Log(unit.Name);
            }
            Debug.Log(visibleUnits.Count());

            return visibleUnits.Any(u => u.Faction == TargetFaction);
        }

    }
}
