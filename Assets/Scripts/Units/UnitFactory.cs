﻿namespace DLS.LD39.Units
{
    using DLS.LD39.Combat;
    using DLS.LD39.Graphics;
    using DLS.LD39.Map;
    using System;
    using UnityEngine;
    using Data;

    class UnitFactory
    {
        public GameUnit GetUnit(string name, UnitData unitData, Tile tile)
        {
            var unitObj = new GameObject(String.Format("{0}: {1}", unitData.ID, name));
            unitObj.layer = LayerMask.NameToLayer("Units");
            var renderer = unitObj.AddComponent<SpriteRenderer>();
            renderer.sprite = unitData.Sprite;
            renderer.color = unitData.SpriteColor;
            renderer.sortingLayerName = "Units";

            var collider = unitObj.AddComponent<BoxCollider2D>();
            Vector2 S = renderer.sprite.bounds.size;
            collider.size = S;
            //collider.center = new Vector2((S.x / 2), 0);


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

            if (unitData.DefaultMeleeWeapon != null)
            {
                unitComp.MeleeCombat.EquippedWeapon = unitData.DefaultMeleeWeapon.GetStats() as MeleeWeapon;
            }
            if (unitData.DefaultRangedWeapon != null)
            {
                unitComp.RangedCombat.EquippedWeapon = unitData.DefaultRangedWeapon.GetStats() as RangedWeapon;
            }

            return unitComp;
        }
    }
}
