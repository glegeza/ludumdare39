namespace DLS.LD39.Combat
{
    using DLS.LD39.Units;

    public class AttackResult
    {
        public AttackResult(GameUnit attacker, ITargetable target, int targetNum,
            int roll, bool wasSuccessful, int damage)
        {
            Attacker = attacker;
            Target = target;
            ModifiedRollTarget = targetNum;
            AttackRoll = roll;
            AttackSuccessful = wasSuccessful;
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

        public bool AttackSuccessful
        {
            get; private set;
        }

        public int DamageDone
        {
            get; private set;
        }
    }
}
