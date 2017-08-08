namespace DLS.LD39.AI.Actions
{
    using DLS.LD39.Map;
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName = "AI/Actions/MoveStraightUntilBlocked")]
    class MoveStraightUntilBlocked : AIAction
    {
        public override bool Act(StateController controller)
        {
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

            var moveResult = controller.Unit.MoveController.Move(nextTile);
            if (moveResult == Units.MoveAction.MoveResult.Blocked)
            {
                data.CurrentDirection = GetRandomDirection();
                return false;
            }
            else if (moveResult == Units.MoveAction.MoveResult.NoAP)
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
