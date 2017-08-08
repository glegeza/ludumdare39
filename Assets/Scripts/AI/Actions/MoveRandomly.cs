namespace DLS.LD39.AI.Actions
{
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Actions/MoveRandomly")]
    class MoveRandomly : AIAction
    {
        public override bool Act(StateController controller)
        {
            var adjTiles = controller.Unit.Position.CurrentTile.AdjacentTiles.ToList();
            var nextTile = adjTiles[Random.Range(0, adjTiles.Count)];

            if (controller.Unit.MoveController.Move(nextTile) == Units.MoveAction.MoveResult.Success)
            {
                return true;
            }
            return false;
        }
    }
}
