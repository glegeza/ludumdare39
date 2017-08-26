namespace DLS.LD39.Conditions
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class LoseConditionChecker : ConditionChecker<LoseConditionChecker>
    {
        protected override void OnAnyConditionsMet(IEnumerable<GameCondition> metConditions)
        {
            GameState.Instance.GameOver();
        }
    }
}
