namespace DLS.LD39.Units.Actions
{
    using DLS.LD39.Combat;
    using DLS.LD39.Graphics;
    using DLS.LD39.Interface;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using System;
    using UnityEngine;

    public class RangedCombatAction : UnitAction
    {
        private AttackResult _pendingResult;
        private Transform _targetTransform;

        public RangedWeapon EquippedWeapon
        {
            get; set;
        }

        public void TryRangedAttack(Tile targetTile, ITargetable target)
        {
            if (!WeaponIsValid())
            {
                Debug.LogError("Attempting to make ranged attack with invalid or no weapon.");
                return;
            }

            var attackCost = CombatManager.Instance.GetAttackCost(
                AttachedUnit, EquippedWeapon, target);
            if (!APAvailable(attackCost))
            {
                Debug.LogError("Not enough AP for ranged attack.");
                return;
            }

            AttachedUnit.Facing.FaceTile(targetTile);
            StartAction(EventArgs.Empty, attackCost);
            _pendingResult = CombatManager.Instance.MakeRangedAttack(
                AttachedUnit, EquippedWeapon, target, targetTile);
            GetTargetTransform(target);

            if (_targetTransform == null)
            {
                Debug.LogError("Target not a MonoBehaviour");
                return;
            }

            var bullet = BulletSpawner.Instance.SpawnBullet(
                transform, _targetTransform);
            bullet.HitTarget += OnBulletHit;
        }

        private void OnBulletHit(object sender, EventArgs e)
        {
            if (_pendingResult == null)
            {
                Debug.LogError("Received HitTarget event with no prending attack result");
                return;
            }
            
            (sender as Bullet).HitTarget -= OnBulletHit;
            CombatManager.Instance.ApplyAttackResult(_pendingResult);
            _targetTransform = null;
            _pendingResult = null;

            CompleteAction(EventArgs.Empty);
        }

        protected override void OnInitialized(GameUnit unit)
        {
            base.OnInitialized(unit);
        }

        private void GetTargetTransform(ITargetable target)
        {
            var comp = target as MonoBehaviour;
            _targetTransform = comp == null ? null : comp.transform;
        }

        private bool WeaponIsValid()
        {
            return EquippedWeapon != null && EquippedWeapon.Type != WeaponType.Melee;
        }
    }
}
