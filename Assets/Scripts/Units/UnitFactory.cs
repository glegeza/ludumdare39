namespace DLS.LD39.Units
{
    using DLS.LD39.Graphics;
    using DLS.LD39.Map;
    using System;
    using UnityEngine;
    using Data;
    using DLS.LD39.Equipment;

    class UnitFactory
    {
        public int UnitNumber = 0;

        public GameUnit GetUnit(string name, UnitData unitData, Tile tile)
        {
            var unitObj = new GameObject(String.Format("{0}:{1}", name, UnitNumber++));
            unitObj.layer = LayerMask.NameToLayer("Units");
            var renderer = unitObj.AddComponent<SpriteRenderer>();
            renderer.sprite = unitData.GraphicsData.Sprite;
            renderer.color = unitData.SpriteTint;
            renderer.sortingLayerName = "Units";

            var collider = unitObj.AddComponent<BoxCollider2D>();
            Vector2 S = renderer.sprite.bounds.size;
            collider.size = S;


            var rb = unitObj.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;

            Animator animator = null;
            if (unitData.GraphicsData.AnimationController != null)
            {
                animator = unitObj.AddComponent<Animator>();
                animator.runtimeAnimatorController = unitData.GraphicsData.AnimationController;
            }
            unitObj.AddComponent<SortByY>();

            var unitComp = unitObj.AddComponent<GameUnit>();

            if (unitData.DefaultPrimaryWeapon != null)
            {
                var primaryWeapon = unitData.DefaultPrimaryWeapon.GetLoot() as PrimaryWeapon;
                unitComp.Equipment.PrimaryWeapon.SetItem(primaryWeapon);
            }
            if (unitData.DefaultSecondaryWeapon != null)
            {
                var secondaryWeapon = unitData.DefaultSecondaryWeapon.GetLoot() as SecondaryWeapon;
                unitComp.Equipment.SecondaryWeapon.SetItem(secondaryWeapon);
            }
            if (unitData.BatteryPack != null)
            {
                unitComp.Equipment.Battery.SetItem(unitData.BatteryPack.GetLoot());
            }

            unitComp.Initialize(unitData, tile, name);

            return unitComp;
        }
    }
}
