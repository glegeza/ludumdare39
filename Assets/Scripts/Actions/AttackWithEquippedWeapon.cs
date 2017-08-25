namespace DLS.LD39.Actions
{
    using DLS.LD39.Combat;
    using System;
    using DLS.LD39.Map;
    using DLS.LD39.Units;
    using UnityEngine;
    using DLS.LD39.Equipment;
    using DLS.LD39.Graphics;

    [CreateAssetMenu(menuName = "Game Data/Actions/Attack With Equipped Weapon")]
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

        public override Sprite GetSprite(GameUnit unit)
        {
            var weapon = GetWeapon(unit);
            return weapon.Stats.SpriteIcon;
        }

        public override bool ActionIsValid(GameUnit actor, GameObject target, Tile targetTile)
        {
            if (!UnitIsValid(actor))
            {
                Debug.LogError("Unit not valid");
                return false;
            }

            var targetable = GetTargetableComponent(target);
            if (targetable == null)
            {
                Debug.LogError("No targetable component");
                return false;
            }

            Debug.Log("Checking weapon...");

            return WeaponIsValid(GetWeapon(actor), actor, targetable, targetTile);
        }

        public override bool AttemptAction(GameUnit actor, GameObject target, Tile targetTile, ActionCompletedDelegate onActionCompletedDelegate)
        {
            if (!ActionIsValid(actor, target, targetTile))
            {
                Debug.LogError("Attempting invalid attack action");
                return false;
            }

            var targetableComp = target.GetComponent<ITargetable>();
            StartAttack(GetWeapon(actor), actor, targetableComp, targetTile, onActionCompletedDelegate);

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
            var weapon = GetWeapon(unit);
            return weapon.Name;
        }

        private ITargetable GetTargetableComponent(GameObject target)
        {
            return target == null
                ? null
                : target.GetComponent<ITargetable>();
        }

        private void StartAttack(Weapon weapon, GameUnit actor, 
            ITargetable target, Tile targetTile, ActionCompletedDelegate onCompleted)
        {
            // Face towards target
            actor.Facing.FaceTile(targetTile);

            // Get the attack results
            var attackResults = CombatManager.Instance.MakeAttack(actor, weapon, target, targetTile);
            
            switch (weapon.Stats.Type)
            {
                case WeaponType.Melee:
                    StartMeleeAttack(weapon, actor, target, targetTile, onCompleted, attackResults);
                    break;
                case WeaponType.Ranged:
                    StartRangedAttack(weapon, actor, target, targetTile, onCompleted, attackResults);
                    break;
            }
        }

        private void StartMeleeAttack(Weapon weapon, GameUnit actor, ITargetable target, 
            Tile targetTile, ActionCompletedDelegate onCompleted, AttackResult result)
        {
            AnimationCallback cb = () =>
            {
                CombatManager.Instance.ApplyAttackResult(result);
                onCompleted();
            };
            var animator = actor.AnimationController;
            animator.StartMeleeAnimation(cb);
        }

        private void StartRangedAttack(Weapon weapon, GameUnit actor, ITargetable target,
            Tile targetTile, ActionCompletedDelegate onCompleted, AttackResult result)
        {
            AnimationCallback cb = () =>
            {
                var targetTransform = GetTargetTransform(target);
                var bullet = BulletSpawner.Instance.SpawnBullet(actor.transform,
                    targetTransform, () => {
                        CombatManager.Instance.ApplyAttackResult(result);
                        onCompleted();
                    });
            };
            var animator = actor.AnimationController;
            animator.StartMeleeAnimation(cb);
        }

        private Transform GetTargetTransform(ITargetable target)
        {
            var comp = target as MonoBehaviour;
            return comp == null ? null : comp.transform;
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
