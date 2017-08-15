namespace DLS.LD39.Combat
{
    using DLS.LD39.Interface;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using UnityEngine;

    public class CombatManager : SingletonComponent<CombatManager>
    {
        public GameObject CombatTextPrefab;

        private const int ATTACK_COST = 4;

        public int HitChance(GameUnit unit, WeaponStats weapon, ITargetable target)
        {
            var baseChance = unit.Stats.Aim;
            var modifiedChance = baseChance + weapon.BaseToHit - target.Evasion;

            return Mathf.Clamp(modifiedChance, 0, 100);
        }

        public int GetAttackCost(GameUnit unit, WeaponStats weapon, ITargetable target)
        {
            return ATTACK_COST;
        }

        public AttackResult MakeMeleeAttack(GameUnit unit, MeleeWeapon weapon, ITargetable target, Tile targetPos, out AttackResult damage)
        {
            CheckArgumentsNotNull(unit, weapon, target, targetPos);

            damage = null;
            if (!targetPos.IsAdjacent(unit.Position.CurrentTile))
            {
                return new AttackResult(unit, target, AttackResult.Outcome.OutOfRange);
            }

            return MakeAttack(unit, weapon, target, targetPos);
        }

        public AttackResult MakeRangedAttack(GameUnit unit, RangedWeapon weapon, ITargetable target, Tile targetPos)
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
            var textPosition = result.Target as MonoBehaviour;
            if (textPosition == null)
            {
                return;
            }
            FloatingCombatTextController.Instance.CreateText(
                result.GetCombatText(), textPosition.transform.position);
        }

        private AttackResult MakeAttack(GameUnit unit, WeaponStats weapon, ITargetable target, Tile targetPos)
        {
            var chance = HitChance(unit, weapon, target);
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

        private void CreateCombatText(string text, Vector3 pos)
        {
            FloatingCombatTextController.Instance.CreateText(text, pos);
        }

        private void ApplyDamage(ITargetable target, AttackResult damage)
        {
            target.ApplyDamage(damage.DamageDone);
        }

        private bool TargetInRange(RangedWeapon weapon, Tile origin, Tile targetPos)
        {
            var distance = Vector2.Distance(origin.WorldCoords, targetPos.WorldCoords);
            return distance <= weapon.Range;
        }

        private bool TargetInLOS(RangedWeapon weapon, Tile origin, Tile targetPos)
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