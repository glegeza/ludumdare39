namespace DLS.LD39.Combat
{
    public enum AttackResult
    {
        Hit,
        Missed,
        NotEnoughAP,
        InvalidTarget,
        OutOfRange,
        InvalidAttackType // unit cannot make melee/ranged attacks
    }
}
