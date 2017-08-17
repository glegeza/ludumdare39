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

            var attackAPCost = CombatManager.Instance.GetAttackAPCost(
                AttachedUnit, meleeWeapon, target);
            var attackEnergyCost = CombatManager.Instance.GetAttackEnergyCost(
                AttachedUnit, meleeWeapon, target);
            if (!APAvailable(attackAPCost))
            {
                FloatingCombatTextController.Instance.RegisterNoAP(AttachedUnit);
                Debug.Log("Not enough AP for melee attack");
                return;
            }
            if (!EnergyAvailable(attackEnergyCost))
            {
                FloatingCombatTextController.Instance.RegisterNoEnergy(AttachedUnit);
                Debug.Log("Not enough energy for melee attack");
                return;
            }

            AttachedUnit.Facing.FaceTile(targetTile);
            AttachedUnit.AnimationController.StartMeleeAnimation();
            _pendingResult = CombatManager.Instance.MakeMeleeAttack(
                AttachedUnit, meleeWeapon, target, targetTile);
            StartAction(EventArgs.Empty, attackAPCost, attackEnergyCost);
        }

        private MeleeWeaponStats CheckWeaponIsValidAndCast(WeaponStats weapon)
        {
            if (weapon == null || weapon.Type != WeaponType.Melee)
            {
                return null;
            }
            return weapon as MeleeWeaponStats;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            base.OnInitialized(unit);
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
