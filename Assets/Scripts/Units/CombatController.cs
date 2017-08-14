namespace DLS.LD39.Units
{
    using System;
    using UnityEngine;
    using Combat;
    using DLS.LD39.Map;

    public class CombatController : GameUnitComponent, ITargetable
    {
        private bool _dead = false;
        private Tile _targetTile;
        private ITargetable _targetUnit;

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

        public WeaponStats EquippedMeleeWeapon
        {
            get; set;
        }

        public WeaponStats EquippedRangedWeapon
        {
            get; set;
        }

        public void TryMeleeAttack(Tile targetTile, ITargetable target, out DamageResult damage)
        {
            damage = null;
            if (EquippedMeleeWeapon == null || EquippedMeleeWeapon.Type != WeaponType.Melee)
            {
                return;
            }

            var cost = CombatManager.Instance.GetAttackCost(AttachedUnit, EquippedMeleeWeapon, target);
            if (!AttachedUnit.AP.PointsAvailable(cost))
            {
                return;
            }

            Attacking = true;
            AttachedUnit.Facing.FaceTile(targetTile);
            _targetTile = targetTile;
            _targetUnit = target;
            StartedAttack?.Invoke(this, EventArgs.Empty);
            AttachedUnit.AP.SpendPoints(cost);
            AttachedUnit.AnimationController.StartMeleeAnimation();
        }

        public void TryRangedAttack(Tile targetTile, ITargetable target, out DamageResult damage)
        {
            throw new NotImplementedException();
        }

        public int ApplyDamage(int amt)
        {
            var dmg = Mathf.Max(amt - Armor, 0);

            HitPoints -= dmg;
            if (HitPoints <= 0)
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
            AttachedUnit.AnimationController.ReturnedToIdle += OnAnimationComplete;

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

        private void OnAnimationComplete(object sender, EventArgs e)
        {
            if (!Attacking)
            {
                return;
            }

            DamageResult damage;
            var result = CombatManager.Instance.MakeMeleeAttack(
                AttachedUnit, EquippedMeleeWeapon as MeleeWeapon, _targetUnit, _targetTile, out damage);
            Attacking = false;
            CompletedAttack?.Invoke(this, EventArgs.Empty);
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
