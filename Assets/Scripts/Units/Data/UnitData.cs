﻿namespace DLS.LD39.Units.Data
{
    using DLS.LD39.AI;
    using DLS.LD39.Combat.Data;
    using DLS.LD39.Graphics;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Unit")]
    public class UnitData : ScriptableObject
    {
        public string ID;
        public Faction Faction;
        public StatsData StatsGenerator;

        [Header("Graphics")]
        public Sprite Sprite;
        public Color SpriteColor = Color.white;
        public RuntimeAnimatorController AnimationController;
        public Explosion DeathPrefab;

        [Header("AI")]
        public State DefaultState;

        [Header("Combat")]
        public WeaponData DefaultMeleeWeapon;
        public WeaponData DefaultRangedWeapon;
    }
}