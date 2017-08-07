namespace DLS.LD39.AI
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    class NearestTargetPursuit : MonoBehaviour, IGameUnitComponent
    {
        public GameUnit CurrentTarget
        {
            get; private set;
        }

        public void BeginTurn()
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }
    }
}
