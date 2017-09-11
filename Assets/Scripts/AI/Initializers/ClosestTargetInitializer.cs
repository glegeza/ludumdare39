namespace DLS.LD39.AI.Initializers
{
    using Map;
    using Units;
    using Data;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    [CreateAssetMenu(menuName = "AI/State Initializers/Closest Target")]
    public class ClosestTargetInitializer : StateInitializer
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once ConvertToConstant.Global
        // ReSharper disable once MemberCanBePrivate.Global 
        public Faction TargetFaction = Faction.Aliens;

        public override void OnStateEnter(StateController controller)
        {
            var validUnits = controller.Unit.Visibility.VisibleUnits
                .Where(u => u.Faction == TargetFaction)
                .OrderBy(u => Tile.GetDistance(
                    controller.Unit.Position.CurrentTile, u.Position.CurrentTile));


            if (!validUnits.Any())
            {
                Debug.LogError("No valid targets");
                return;
            }

            var targetData = new TrackedTargetData()
            {
                CurrentTarget = validUnits.First(),
                CurrentPath = new Queue<Tile>()
            };
            controller.Data = targetData;
        }
    }
}
