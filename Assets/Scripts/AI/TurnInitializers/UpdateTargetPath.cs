namespace DLS.LD39.AI.TurnInitializers
{
    using DLS.LD39.AI.Data;
    using DLS.LD39.Map;
    using DLS.LD39.Pathfinding;
    using DLS.LD39.Units;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Turn Initializers/Update Target Path")]
    public class UpdateTargetPath : StateTurnInitializer
    {
        private SimplePathfinder _pathfinder = new SimplePathfinder();

        public override void OnTurnStart(StateController controller)
        {
            var data = controller.Data as TrackedTargetData;
            if (data == null)
            {
                Debug.LogError("UpdateTargetPath missing TrackedTargetData");
                return;
            }

            var targetTile = GetBestAdjacentTile(data.CurrentTarget, controller.Unit);
            if (targetTile == data.MoveTarget)
            {
                return;
            }

            data.MoveTarget = targetTile;
            UpdateData(data, controller.Unit);
        }

        private void UpdateData(TrackedTargetData data, GameUnit actor)
        {
            if (data.MoveTarget == null)
            {
                data.CurrentPath = new Queue<Tile>();
                return;
            }

            var cost = 0;
            data.CurrentPath = _pathfinder.GetPath(actor.Position.CurrentTile,
                data.MoveTarget, out cost);
        }

        private Tile GetBestAdjacentTile(GameUnit target, GameUnit actor)
        {
            var unitPosition = actor.Position.CurrentTile;

            var validTiles = target.Position.CurrentTile.AdjacentTiles
                .Where(t => t.Passable)
                .OrderBy(t => Tile.GetDistance(t, unitPosition));

            if (!validTiles.Any())
            {
                return null;
            }

            return validTiles.First();
        }
    }
}
