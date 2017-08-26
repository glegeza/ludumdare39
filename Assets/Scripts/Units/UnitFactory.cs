namespace DLS.LD39.Units
{
    using Graphics;
    using Map;
    using System;
    using UnityEngine;
    using Data;
    using Equipment;

    public class UnitFactory
    {
        public int UnitNumber;

        public GameUnit GetUnit(string name, UnitData unitData, Tile tile)
        {
            var unitObj = new GameObject(String.Format("{0}:{1}", name, UnitNumber++));
            unitObj.layer = LayerMask.NameToLayer("Units");
            var renderer = unitObj.AddComponent<SpriteRenderer>();
            renderer.sprite = unitData.GraphicsData.Sprite;
            renderer.color = unitData.SpriteTint;
            renderer.sortingLayerName = "Units";

            var collider = unitObj.AddComponent<BoxCollider2D>();
            collider.size = renderer.sprite.bounds.size;


            var rb = unitObj.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;

            if (unitData.GraphicsData.AnimationController != null)
            {
                var animator = unitObj.AddComponent<Animator>();
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
