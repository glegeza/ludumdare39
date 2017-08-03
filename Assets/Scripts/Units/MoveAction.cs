namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    class MoveAction : MonoBehaviour, IGameUnitComponent
    {
        public enum MoveResult
        {
            Success,
            Blocked,
            NoAP
        }

        private GameUnit _unit;
        private TilePosition _position;

        public void Initialize(GameUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException("unit");
            }
            _unit = unit;
            _position = _unit.Position;
        }

        public MoveResult Move(Tile target)
        {
            var cost = _position.CurrentTile.GetMoveCost(target);
            var validMove = TileIsValid(target);

            if (validMove && _unit.AP.CanSpendPoints(cost))
            {
                _unit.AP.SpendPoints(cost);
                _position.SetTile(target);
                Debug.Log("Moving!");
                return MoveResult.Success;
            }
            else if (!validMove)
            {
                Debug.Log("Blocked!");
                return MoveResult.Blocked;
            }

            Debug.Log("Out of AP!");
            return MoveResult.NoAP;
        }

        public void BeginTurn() { }

        public void EndTurn() { }

        public int GetMaxMoveAlongPath(IEnumerable<Tile> path)
        {
            var curMove = 0;
            var apLeft = _unit.AP.PointsRemaining;
            var lastStep = _position.CurrentTile;
            foreach (var nextStep in path)
            {
                var cost = lastStep.GetMoveCost(nextStep);
                if (cost > apLeft)
                {
                    break;
                }
                apLeft -= cost;
                lastStep = nextStep;
                curMove++;
            }

            return curMove;
        }
        
        private bool TileIsValid(Tile target)
        {
            return target.IsAdjacent(_position.CurrentTile) && target.IsEnterable();
        }
    }
}
