﻿namespace DLS.LD39.Units
{
    using UnityEngine;

    /// <summary>
    /// Component used to indicate that an object is part of the regular turn
    /// order.
    /// </summary>
    public class Initiative : MonoBehaviour, IGameUnitComponent
    {
        public float InitiativeValue
        {
            get;
            set;
        }

        public bool IsActiveUnit
        {
            get;
            set;
        }

        public void BeginTurn()
        {
            IsActiveUnit = true;
        }

        public void EndTurn()
        {
            IsActiveUnit = false;
        }
    }
}
