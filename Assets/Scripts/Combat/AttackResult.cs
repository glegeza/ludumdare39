﻿namespace DLS.LD39.Combat
{
    using System;
    using Units;
    using Interface;

    /// <summary>
    /// Stores information about an attack that was completed.
    /// </summary>
    public class AttackResult
    {
        private bool _resultApplied;

        public enum Outcome
        {
            Hit,
            Missed,
            NotEnoughAP,
            InvalidTarget,
            OutOfRange,
            LOSBlocked,
            NoValidWeapon,
        }

        public AttackResult(GameUnit attacker, ITargetable target, Outcome result)
        {
            Attacker = attacker;
            Target = target;
            Result = result;
        }

        public AttackResult(GameUnit attacker, ITargetable target, Outcome result,
            int targetNum, int roll, int damage)
        {
            Attacker = attacker;
            Target = target;
            Result = result;
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

        public Outcome Result
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
        
        public void ApplyResults()
        {
            if (_resultApplied)
            {
                UnityEngine.Debug.LogError("Attempting to apply attack results twice.");
                return;
            }

            _resultApplied = true;

            if (Result == Outcome.Hit)
            {
                Target.ApplyDamage(DamageDone);
                FloatingCombatTextController.Instance.RegisterDamage(DamageDone, Target);
            }
            else if (Result == Outcome.Missed)
            {
                FloatingCombatTextController.Instance.RegisterMiss(Target);
            }
        }

        public string GetCombatText()
        {
            switch (Result)
            {
                case Outcome.Hit:
                    return DamageDone.ToString();
                case Outcome.Missed:
                    return "Miss!";
                default:
                    return "Error!";
            }
        }

        public override string ToString()
        {
            return String.Format("{0} vs {1} for {2} damage!",
                AttackRoll, ModifiedRollTarget, DamageDone);
        }
    }
}
