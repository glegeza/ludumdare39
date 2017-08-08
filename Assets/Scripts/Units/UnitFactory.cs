namespace DLS.LD39.Units
{
    using DLS.LD39.Graphics;
    using DLS.LD39.Map;
    using System;
    using UnityEngine;

    class UnitFactory
    {
        public GameUnit GetUnit(string name, UnitData unitData, Tile tile)
        {
            var unitObj = new GameObject(String.Format("{0}: {1}", unitData.ID, name));
            var unitComp = unitObj.AddComponent<GameUnit>();
            var mover = unitObj.AddComponent<MoveAction>();
            unitComp.Initialize(unitData, tile, name);

            mover.Initialize(unitComp);

            var renderer = unitObj.AddComponent<SpriteRenderer>();
            renderer.sprite = unitData.Sprite;
            renderer.sortingLayerName = "Units";

            if (unitData.Controller != null)
            {
                var animator = unitObj.AddComponent<Animator>();
                animator.runtimeAnimatorController = unitData.Controller;
            }

            unitObj.AddComponent<SortByY>();

            return unitComp;
        }
    }
}
