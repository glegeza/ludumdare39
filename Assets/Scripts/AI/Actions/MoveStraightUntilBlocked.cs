namespace DLS.LD39.AI.Actions
{
    using DLS.LD39.Map;
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Actions/MoveStraightUntilBlocked")]
    class MoveStraightUntilBlocked : AIAction
    {
        [Range(0, 100)]
        public int MoveChance;

        public override bool Act(StateController controller)
        {
            var chance = UnityEngine.Random.Range(0, 100);
            if (chance > MoveChance)
            {
                return true;
            }

            if (controller.Data == null)
            {
                controller.Data = new MoveDirectionData();
            }
            var data = controller.Data as MoveDirectionData;
            if (data == null)
            {
                data = new MoveDirectionData();
                controller.Data = data;
                data.CurrentDirection = GetRandomDirection();
            }

            var nextTile = controller.Unit.Position.CurrentTile.GetAdjacent(data.CurrentDirection);
            if (nextTile == null)
            {
                data.CurrentDirection = GetRandomDirection();
                return false;
            }

            var moveResult = controller.Unit.Move.TryMove(nextTile);
            if (moveResult == MoveResult.Blocked)
            {
                data.CurrentDirection = GetRandomDirection();
                return false;
            }
            else if (moveResult == MoveResult.NotEnoughAP)
            {
                return false;
            }

            return true;
        }

        private TileEdge GetRandomDirection()
        {
            var values = Enum.GetValues(typeof(TileEdge));
            return (TileEdge)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }
}
