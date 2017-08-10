namespace DLS.LD39.Combat
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    class CombatManager : SingletonComponent<CombatManager>
    {
        public float HitChance(GameUnit unit, ITargetable target)
        {
            var baseChance = unit.Stats.Aim;
            var modifiedChance = baseChance - target.Evasion;

            return Mathf.Clamp(modifiedChance, 0, 100);
        }

        public bool MakeAttack(GameUnit unit, ITargetable target)
        {
            var chance = HitChance(unit, target);
            var roll = UnityEngine.Random.Range(0, 100);

            return roll <= chance;
        }
    }
}