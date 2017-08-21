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

        public override bool ActionIsValid(GameUnit actor, GameObject target, Tile targetTile)
        {
            if (actor == null || actor.Equipment == null)
            {
                return false;
            }
            var targetable = target.GetComponent<ITargetable>();
            if (targetable == null)
            {
                return false;
            }

            Weapon weapon = null;
            if (Slot == WeaponSlot.Primary)
            {
                weapon = actor.Equipment.PrimaryWeapon.SlotItem;
            }
            else
            {
                weapon = actor.Equipment.SecondaryWeapon.SlotItem;
            }

            return weapon != null && weapon.TargetIsValid(actor, targetable, targetTile);
        }

        public override bool AttemptAction(GameUnit actor, GameObject target, Tile targetTile)
        {
            if (!ActionIsValid(actor, target, targetTile))
            {
                Debug.LogError("Attempting invalid attack action");
                return false;
            }



            return true;
        }

        public override int GetAPCost()
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        private IEnumerator StartAttack(GameUnit actor, ITargetable target, Tile targetTile)
        {

        }
    }
}
