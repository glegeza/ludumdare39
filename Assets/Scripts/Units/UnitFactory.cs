namespace DLS.LD39.Units
{
    using DLS.LD39.Combat;
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
            renderer.sprite = unitData.Sprite;
            renderer.color = unitData.SpriteColor;
            renderer.sortingLayerName = "Units";

            var collider = unitObj.AddComponent<BoxCollider2D>();
            Vector2 S = renderer.sprite.bounds.size;
            collider.size = S;


            var rb = unitObj.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;

            Animator animator = null;
            if (unitData.AnimationController != null)
            {
                animator = unitObj.AddComponent<Animator>();
                animator.runtimeAnimatorController = unitData.AnimationController;
            }
            unitObj.AddComponent<SortByY>();

            var unitComp = unitObj.AddComponent<GameUnit>();
            unitComp.Initialize(unitData, tile, name);

            if (unitData.DefaultPrimaryWeapon != null)
            {
                var primaryWeapon = new PrimaryWeapon("Test Primary", "Test", unitData.DefaultPrimaryWeapon);
                unitComp.Equipment.PrimaryWeapon.SetItem(primaryWeapon);
            }
            if (unitData.DefaultSecondaryWeapon != null)
            {
                var secondaryWeapon = new SecondaryWeapon("Test Secondary", "test", unitData.DefaultSecondaryWeapon);
                unitComp.Equipment.SecondaryWeapon.SetItem(secondaryWeapon);
            }

            return unitComp;
        }
    }
}
