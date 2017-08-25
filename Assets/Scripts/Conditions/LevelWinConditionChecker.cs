namespace DLS.LD39.Conditions
{
    using System.Collections.Generic;

    public class LevelWinConditionChecker : ConditionChecker<LevelWinConditionChecker>
    {
        protected override void OnAnyConditionsMet(IEnumerable<GameCondition> metConditions)
        {
            GameState.Instance.EndLevel();
        }
    }
}
