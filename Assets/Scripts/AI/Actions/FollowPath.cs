// ReSharper disable UnusedMember.Global
namespace DLS.LD39.AI.Actions
{
    using Data;
    using Pathfinding;
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Actions/Follow Path")]
    public class FollowPath : AIAction
    {
        private SimplePathfinder _pathfinder = new SimplePathfinder();

        public override bool Act(StateController controller)
        {
            var data = controller.Data as IMovePathData;
            if (data == null)
            {
                Debug.Log(controller.Data);
                Debug.LogError("FollowPath missing IMovePathData");
                return false;
            }

            if (!data.CurrentPath.Any())
            {
                return false; // unit has arrived
            }

            var next = data.CurrentPath.Dequeue();
            var moveResult = controller.Unit.MoveAction.TryMove(next);
            if (moveResult == MoveResult.ValidMove)
            {
                return true;
            }
            if (moveResult != MoveResult.Blocked) return false;

            int cost;
            data.CurrentPath = _pathfinder.GetPath(
                controller.Unit.Position.CurrentTile,
                data.MoveTarget, out cost);

            return false;
        }
    }
}
