﻿namespace DLS.LD39.Equipment.Data
{
    using UnityEngine;
    using Combat;

    [CreateAssetMenu(menuName = "Equipment/Weapons/Melee")]
    public class MeleeWeaponData : WeaponData
    {
        public override WeaponStats GetStats()
        {
            return new MeleeWeaponStats(this);
        }
    }
}
