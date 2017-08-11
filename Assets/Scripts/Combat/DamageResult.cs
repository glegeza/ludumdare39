﻿namespace DLS.LD39.Combat
{
    using System;
    using DLS.LD39.Units;

    /// <summary>
    /// Stores information about a successful attack.
    /// </summary>
    public class DamageResult
    {
        public DamageResult(GameUnit attacker, ITargetable target, int targetNum,
            int roll, int damage)
        {
            Attacker = attacker;
            Target = target;
            ModifiedRollTarget = targetNum;
            AttackRoll = roll;
            DamageDone = damage;
        }

        public GameUnit Attacker
        {
            get; private set;
        }

        public ITargetable Target
        {
            get; private set;
        }

        public int ModifiedRollTarget
        {
            get; private set;
        }

        public int AttackRoll
        {
            get; private set;
        }

        public int DamageDone
        {
            get; private set;
        }

        public override string ToString()
        {
            return String.Format("{0} vs {1} for {2} damage!",
                AttackRoll, ModifiedRollTarget, DamageDone);
        }
    }
}
