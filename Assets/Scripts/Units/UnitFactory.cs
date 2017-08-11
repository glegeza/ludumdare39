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
            unitObj.layer = LayerMask.NameToLayer("Units");
            unitObj.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            var renderer = unitObj.AddComponent<SpriteRenderer>();
            renderer.sprite = unitData.Sprite;
            renderer.sortingLayerName = "Units";

            unitObj.AddComponent<BoxCollider2D>().size = new Vector2(1.0f, 1.0f);

            Animator animator = null;
            if (unitData.Controller != null)
            {
                animator = unitObj.AddComponent<Animator>();
                animator.runtimeAnimatorController = unitData.Controller;
            }
            unitObj.AddComponent<SortByY>();

            var unitComp = unitObj.AddComponent<GameUnit>();
            unitComp.Initialize(unitData, tile, name);

            if (unitData.DefaultWeapon != null)
            {
                unitComp.CombatInfo.EquippedWeapon = unitData.DefaultWeapon.GetStats();
            }

            return unitComp;
        }
    }
}
