namespace DLS.LD39.Conditions
{
    public enum ConditionCheckInterval
    {
        TurnStart, // when a unit begins its turn
        TurnEnd, // when a unit ends its turn
        PerRound, // when all units have taken their turn
        OnUnitDestroyed // whenever a unit is destroyed
    }
}
