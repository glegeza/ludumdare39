﻿namespace DLS.LD39.Combat
{
    using DLS.LD39.Equipment;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using UnityEngine;

    public class CombatManager : SingletonComponent<CombatManager>
    {
        public GameObject CombatTextPrefab;

        public int HitChance(GameUnit unit, WeaponStats weapon, ITargetable target, Tile targetTile)
        {
            if (!weapon.TileIsLegalTarget(unit.Position.CurrentTile, targetTile))
            {
                return 0;
            }

            var baseChance = unit.PrimaryStats.Aim;
            var modifiedChance = baseChance + weapon.BaseToHit - target.Evasion;

            return Mathf.Clamp(modifiedChance, 0, 100);
        }

        public int GetAttackAPCost(GameUnit unit, WeaponStats weapon, ITargetable target)
        {
            return weapon.APCost;
        }

        public int GetAttackEnergyCost(GameUnit unit, WeaponStats weapon, ITargetable target)
        {
            return weapon.EnergyCost;
        }

        public AttackResult MakeAttack(GameUnit unit, Weapon weapon, ITargetable target, Tile targetPos)
        {
            switch (weapon.Stats.Type)
            {
                case WeaponType.Melee:
                    return MakeMeleeAttack(unit, weapon.Stats as MeleeWeaponStats, target, targetPos);
                case WeaponType.Ranged:
                    return MakeRangedAttack(unit, weapon.Stats as RangedWeaponStats, target, targetPos);
            }

            throw new ArgumentException("Weapon type is unknown (not melee or ranged)");
        }

        public AttackResult MakeMeleeAttack(GameUnit unit, MeleeWeaponStats weapon, ITargetable target, Tile targetPos)
        {
            CheckArgumentsNotNull(unit, weapon, target, targetPos);

            if (!targetPos.IsAdjacent(unit.Position.CurrentTile))
            {
                return new AttackResult(unit, target, AttackResult.Outcome.OutOfRange);
            }

            return MakeAttack(unit, weapon, target, targetPos);
        }

        public AttackResult MakeRangedAttack(GameUnit unit, RangedWeaponStats weapon, ITargetable target, Tile targetPos)
        {
            CheckArgumentsNotNull(unit, weapon, target, targetPos);
            
            if (!TargetInRange(weapon, unit.Position.CurrentTile, targetPos))
            {
                return new AttackResult(unit, target, AttackResult.Outcome.OutOfRange);
            }
            if (!TargetInLOS(weapon, unit.Position.CurrentTile, targetPos))
            {
                return new AttackResult(unit, target, AttackResult.Outcome.LOSBlocked);
            }

            return MakeAttack(unit, weapon, target, targetPos);
        }

        public void ApplyAttackResult(AttackResult result)
        {
            result.ApplyResults();
        }

        private AttackResult MakeAttack(GameUnit unit, WeaponStats weapon, ITargetable target, Tile targetPos)
        {
            var chance = HitChance(unit, weapon, target, targetPos);
            var roll = UnityEngine.Random.Range(0, 100);
            var hit = roll <= chance;

            if (!hit)
            {
                return new AttackResult(unit, target, AttackResult.Outcome.Missed);
            }

            var dmgAmt = UnityEngine.Random.Range(weapon.MinDamage, weapon.MaxDamage);
            var result = new AttackResult(unit, target, AttackResult.Outcome.Hit, chance, roll, dmgAmt);
            return result;
        }

        private void ApplyDamage(ITargetable target, AttackResult damage)
        {
            target.ApplyDamage(damage.DamageDone);
        }

        private bool TargetInRange(RangedWeaponStats weapon, Tile origin, Tile targetPos)
        {
            var distance = Vector2.Distance(origin.WorldCoords, targetPos.WorldCoords);
            return distance <= weapon.Range;
        }

        private bool TargetInLOS(RangedWeaponStats weapon, Tile origin, Tile targetPos)
        {
            return LOSChecker.Instance.LOSClear(origin, targetPos);
        }

        private void CheckArgumentsNotNull(GameUnit unit, WeaponStats weapon, ITargetable target, Tile targetPos)
        {
            if (unit == null)
            {
                throw new ArgumentNullException("unit");
            }
            if (weapon == null)
            {
                throw new ArgumentNullException("weapon");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (targetPos == null)
            {
                throw new ArgumentNullException("targetPos");
            }
        }
    }
}