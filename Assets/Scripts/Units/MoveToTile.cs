namespace DLS.LD39.Units
{
    using DLS.LD39.Map;
    using System;
    using UnityEngine;

    class MoveToTile : MonoBehaviour, IGameUnitComponent
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
            var cost = GetTileCost(target);
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

        private int GetTileCost(Tile target)
        {
            return (target.X == _position.CurrentTile.X || target.Y == _position.CurrentTile.Y)
                ? 10
                : 14;
        }

        private bool TileIsValid(Tile target)
        {
            return target.IsAdjacent(_position.CurrentTile) && target.IsEnterable();
        }
    }
}
