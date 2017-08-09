namespace DLS.LD39.Units
{
    using System;
    using DLS.LD39.Map;
    using System.Collections.Generic;
    using UnityEngine;

    public class UnitController : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        public bool IsActive
        {
            get; private set;
        }

        public bool CanAct
        {
            get; private set;
        }

        public void Initialize(GameUnit unit)
        {
        }

        public void BeginTurn()
        {
            IsActive = true;
            CanAct = true;
        }

        public void EndTurn()
        {
            IsActive = false;
            CanAct = false;
        }

        public void PerformRangedAttack(Tile target)
        {
            throw new NotImplementedException();
        }

        public void PerformMeleeAttack(TileEdge direction)
        {
            throw new NotImplementedException();
        }

        public MoveResult MoveAdjacent(Tile tile)
        {
            throw new NotImplementedException();
        }

        public MoveResult MoveDirection(TileEdge direction)
        {
            throw new NotImplementedException();
        }

        public MoveResult FollowPath(IEnumerable<Tile> path)
        {
            throw new NotImplementedException();
        }
    }
}
