namespace DLS.LD39.Combat
{
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using UnityEngine;

    public class CombatManager : SingletonComponent<CombatManager>
    {
        private const int ATTACK_COST = 24;

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

        public AttackResult MakeMeleeAttack(GameUnit unit, MeleeWeapon weapon, ITargetable target, Tile targetPos, out DamageResult damage)
        {
            CheckArgumentsNotNull(unit, weapon, target, targetPos);

            damage = null;
            if (!targetPos.IsAdjacent(unit.Position.CurrentTile))
            {
                return AttackResult.OutOfRange;
            }

            return MakeAttack(unit, weapon, target, out damage);
        }

        public AttackResult MakeRangedAttack(GameUnit unit, RangedWeapon weapon, ITargetable target, Tile targetPos, out DamageResult damage)
        {
            CheckArgumentsNotNull(unit, weapon, target, targetPos);

            damage = null;
            if (!TargetInRange(weapon, targetPos))
            {
                return AttackResult.OutOfRange;
            }

            return MakeAttack(unit, weapon, target, out damage);
        }

        private AttackResult MakeAttack(GameUnit unit, WeaponStats weapon, ITargetable target, out DamageResult damage)
        {
            damage = null;
            var chance = HitChance(unit, weapon, target);
            var roll = UnityEngine.Random.Range(0, 100);
            var hit = roll <= chance;

            if (!hit)
            {
                return AttackResult.Missed;
            }

            var dmgAmt = UnityEngine.Random.Range(weapon.MinDamage, weapon.MaxDamage);
            damage = new DamageResult(unit, target, chance, roll, dmgAmt);
            target.ApplyDamage(dmgAmt);
            return AttackResult.Hit;
        }

        private void ApplyDamage(ITargetable target, DamageResult damage)
        {
            target.ApplyDamage(damage.DamageDone);
        }

        private bool TargetInRange(RangedWeapon weapon, Tile targetPos)
        {
            // TODO actually check range!
            return true;
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