// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global
namespace DLS.LD39.AI.Decisions
{
    using Units;
    using System.Linq;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    [CreateAssetMenu(menuName = "AI/Decisions/If Has Target")]
    public class IfHasTarget : Decision
    {
        public Faction TargetFaction = Faction.Aliens;

        public override bool Decide(StateController controller)
        {
            var visibleUnits = controller.Unit.Visibility.VisibleUnits;

            return visibleUnits.Any(u => u.Faction == TargetFaction);
        }

    }
}
