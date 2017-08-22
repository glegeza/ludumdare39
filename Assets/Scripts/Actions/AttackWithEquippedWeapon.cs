namespace DLS.LD39.Actions
{
    using DLS.LD39.Combat;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using UnityEngine;
    using DLS.LD39.Equipment;

    public class AttackWithEquippedWeapon : Action
    {
        public WeaponSlot Slot;

        public override ActionSelectMode Mode
        {
            get
            {
                return ActionSelectMode.Enemy;
            }
        }

        public override bool ActionIsValid(GameUnit actor, GameObject target, Tile targetTile)
        {
            if (!UnitIsValid(actor))
            {
                return false;
            }

            var targetable = target.GetComponent<ITargetable>();
            if (targetable == null)
            {
                return false;
            }

            return WeaponIsValid(GetWeapon(actor), actor, targetable, targetTile);
        }

        public override bool AttemptAction(GameUnit actor, GameObject target, Tile targetTile, ActionCompletedDelegate onCompleted)
        {
            if (!ActionIsValid(actor, target, targetTile))
            {
                Debug.LogError("Attempting invalid attack action");
                return false;
            }

            var targetableComp = target.GetComponent<ITargetable>();
            StartAttack(GetWeapon(actor), actor, targetableComp, targetTile, onCompleted);

            return true;
        }

        public override int GetAPCost(GameUnit unit)
        {
            var weapon = GetWeapon(unit);
            if (weapon == null)
            {
                return 0;
            }

            return weapon.Stats.APCost;
        }

        public override int GetEnergyCost(GameUnit unit)
        {
            var weapon = GetWeapon(unit);
            if (weapon == null)
            {
                return 0;
            }

            return weapon.Stats.EnergyCost;
        }

        public override string GetDescription(GameUnit unit)
        {
            var weapon = GetWeapon(unit);
            if (weapon == null)
            {
                return "MISSING WEAPON";
            }

            var attackType = weapon.Stats.Type == WeaponType.Ranged
                ? "ranged"
                : "melee";
            return String.Format("Make a {0} attack with {1}", attackType, weapon.Name);
        }

        public override string GetName(GameUnit unit)
        {
            return Slot == WeaponSlot.Primary
                ? "Primary Attack"
                : "Secondary Attack";
        }

        private void StartAttack(Weapon weapon, GameUnit actor, 
            ITargetable target, Tile targetTile, ActionCompletedDelegate onCompleted)
        {
            // Face towards target
            actor.Facing.FaceTile(targetTile);

            // Get the attack results
            var attackResults = CombatManager.Instance.MakeAttack(actor, weapon, target, targetTile);

            // Create the function to call when the attack is completed
            AnimationCallback cb = () =>
            {
                CombatManager.Instance.ApplyAttackResult(attackResults);
                onCompleted();
            };

            // Start Animation
            var animator = actor.AnimationController;
            switch (weapon.Stats.Type)
            {
                case WeaponType.Melee:
                    animator.StartMeleeAnimation(cb);
                    break;
                case WeaponType.Ranged:
                    animator.StartRangedAnimation(cb);
                    break;
            }
        }

        private bool UnitIsValid(GameUnit unit)
        {
            return unit != null && unit.Equipment != null;
        }

        private bool WeaponIsValid(Weapon weapon, GameUnit attacker, ITargetable target, Tile targetTile)
        {
            return weapon != null && weapon.TargetIsValid(attacker, target, targetTile);
        }

        private Weapon GetWeapon(GameUnit unit)
        {
            Weapon weapon = null;
            switch (Slot)
            {
                case WeaponSlot.Primary:
                    weapon = unit.Equipment.PrimaryWeapon.SlotItem;
                    break;
                case WeaponSlot.Secondary:
                    weapon = unit.Equipment.SecondaryWeapon.SlotItem;
                    break;
            }
            return weapon;
        }
    }
}
