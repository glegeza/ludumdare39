namespace DLS.LD39.Units
{
    using System;
    using UnityEngine;
    using Combat;
    using DLS.LD39.Map;

    public class CombatController : GameUnitComponent, ITargetable
    {
        private bool _dead = false;

        public event EventHandler<EventArgs> StartedAttack;

        public event EventHandler<EventArgs> CompletedAttack;

        public event EventHandler<EventArgs> Destroyed;

        public bool Attacking
        {
            get; private set;
        }

        public int Evasion
        {
            get
            {
                return AttachedUnit.Stats.Evasion;
            }
        }

        public int HitPoints
        {
            get; private set;
        }

        public int Armor
        {
            get
            {
                return AttachedUnit.Stats.Armor;
            }
        }

        public WeaponStats EquippedWeapon
        {
            get; set;
        }

        public AttackResult TryMeleeAttack(Tile targetTile, ITargetable target, out DamageResult damage)
        {
            damage = null;
            if (EquippedWeapon == null || EquippedWeapon.Type != WeaponType.Melee)
            {
                return AttackResult.InvalidAttackType;
            }

            var cost = CombatManager.Instance.GetAttackCost(AttachedUnit, EquippedWeapon, target);
            if (!AttachedUnit.AP.PointsAvailable(cost))
            {
                return AttackResult.NotEnoughAP;
            }
            
            var result = CombatManager.Instance.MakeMeleeAttack(
                AttachedUnit, EquippedWeapon as MeleeWeapon, target, targetTile, out damage);
            if (result == AttackResult.Hit || result == AttackResult.Missed)
            {
                Attacking = true;
                StartedAttack?.Invoke(this, EventArgs.Empty);
                AttachedUnit.AnimationController.StartMeleeAnimation();
                // Only spend AP if the result is a hit or miss, because 
                // otherwise something went wrong and no attack was made
                AttachedUnit.AP.SpendPoints(cost);
            }
            
            return result;
        }

        public AttackResult TryRangedAttack(Tile targetTile, ITargetable target, out DamageResult damage)
        {
            throw new NotImplementedException();
        }

        public int ApplyDamage(int amt)
        {
            var dmg = Mathf.Max(amt - Armor, 0);

            HitPoints -= dmg;
            if (HitPoints < 0)
            {
                OnDestroyed();
            }
            return HitPoints;
        }

        public int ApplyHeal(int amt)
        {
            HitPoints += amt;
            HitPoints = Mathf.Clamp(HitPoints, 0, AttachedUnit.Stats.MaxHP);
            return HitPoints;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            AttachedUnit.AnimationController.ReturnedToIdle += (o, e) =>
            {
                if (Attacking)
                {
                    Attacking = false;
                    CompletedAttack?.Invoke(this, EventArgs.Empty);
                }
            };
            HitPoints = unit.Stats.MaxHP;
        }

        private void OnDestroyed()
        {
            if (_dead)
            {
                return;
            }
            _dead = true;
            Destroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
