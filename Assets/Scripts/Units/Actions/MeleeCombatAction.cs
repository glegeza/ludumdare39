namespace DLS.LD39.Units.Actions
{
    using DLS.LD39.Combat;
    using DLS.LD39.Interface;
    using DLS.LD39.Map;
    using System;
    using UnityEngine;

    public class MeleeCombatAction : UnitAction
    {
        private AttackResult _pendingResult;

        public void TryMeleeAttack(Tile targetTile, ITargetable target, WeaponStats weapon)
        {
            var meleeWeapon = CheckWeaponIsValidAndCast(weapon);
            if (meleeWeapon == null)
            {
                Debug.Log("Attempting to make melee attack with invalid or not weapon.");
                return;
            }

            var attackCost = CombatManager.Instance.GetAttackCost(
                AttachedUnit, meleeWeapon, target);
            if (!APAvailable(attackCost))
            {
                FloatingCombatTextController.Instance.RegisterNoAP(AttachedUnit);
                Debug.Log("Not enough AP for melee attack");
            }

            AttachedUnit.Facing.FaceTile(targetTile);
            AttachedUnit.AnimationController.StartMeleeAnimation();
            _pendingResult = CombatManager.Instance.MakeMeleeAttack(
                AttachedUnit, meleeWeapon, target, targetTile);
            StartAction(EventArgs.Empty, attackCost);
        }

        private MeleeWeapon CheckWeaponIsValidAndCast(WeaponStats weapon)
        {
            if (weapon == null || weapon.Type != WeaponType.Melee)
            {
                return null;
            }
            return weapon as MeleeWeapon;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            AttachedUnit.AnimationController.ReturnedToIdle += OnAnimationComplete;
        }

        private void OnAnimationComplete(object sender, EventArgs e)
        {
            if (!ActionInProgress)
            {
                return;
            }

            if (_pendingResult == null)
            {
                Debug.LogError("No pending attack result");
                return;
            }

            CombatManager.Instance.ApplyAttackResult(_pendingResult);
            _pendingResult = null;
            CompleteAction(EventArgs.Empty);
        }
    }
}
