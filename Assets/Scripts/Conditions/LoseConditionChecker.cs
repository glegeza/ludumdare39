namespace DLS.LD39.Conditions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LoseConditionChecker : ConditionChecker<LoseConditionChecker>
    {
        protected override void OnAnyConditionsMet(IEnumerable<GameCondition> metConditions)
        {
            GameState.Instance.GameOver();
        }
    }
}
