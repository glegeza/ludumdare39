namespace DLS.LD39.AI.Actions
{
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Actions/MoveRandomly")]
    class MoveRandomly : AIAction
    {
        [Range(0, 100)]
        public int MoveChance;

        public override bool Act(StateController controller)
        {
            var chance = Random.Range(0, 100);
            if (chance > MoveChance)
            {
                return true;
            }

            var adjTiles = controller.Unit.Position.CurrentTile.AdjacentTiles.ToList();
            var nextTile = adjTiles[Random.Range(0, adjTiles.Count)];

            if (controller.Unit.MoveAction.TryMove(nextTile) == MoveResult.ValidMove)
            {
                return true;
            }
            return false;
        }
    }
}