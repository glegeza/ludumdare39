namespace DLS.LD39.AI.Initializers
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using DLS.LD39.AI.Data;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/State Initializers/Closest Target")]
    public class ClosestTargetInitializer : StateInitializer
    {
        public Faction TargetFaction;

        public override void OnStateEnter(StateController controller)
        {
            Debug.Log("in OnStateEnter");

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
