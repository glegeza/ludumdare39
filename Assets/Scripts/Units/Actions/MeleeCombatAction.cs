namespace DLS.LD39.Units.Actions
{
    using DLS.LD39.Combat;
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    public class MeleeCombatAction : UnitAction
    {
        private AttackResult _pendingResult;
        private Transform _targetTransform;

        public MeleeWeapon EquippedWeapon
        {
            get; private set;
        }

        public void TryMeleeAttack(Tile targetTile, ITargetable target)
        {

        }
    }
}
