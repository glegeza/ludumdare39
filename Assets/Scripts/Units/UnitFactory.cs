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
            var renderer = unitObj.AddComponent<SpriteRenderer>();
            renderer.sprite = unitData.Sprite;
            renderer.sortingLayerName = "Units";

            Animator animator = null;
            if (unitData.Controller != null)
            {
                animator = unitObj.AddComponent<Animator>();
                animator.runtimeAnimatorController = unitData.Controller;
                Debug.Log(animator);
            }
            unitObj.AddComponent<SortByY>();

            var unitComp = unitObj.AddComponent<GameUnit>();
            unitComp.Initialize(unitData, tile, name);

            return unitComp;
        }
    }
}
