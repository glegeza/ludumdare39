namespace DLS.LD39.Conditions
{
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Conditions/All Player Units Dead")]
    public class AllPlayerUnitsDead : GameCondition
    {
        public override ConditionCheckInterval Interval
        {
            get { return ConditionCheckInterval.TurnEnd; }
        }

        public override bool IsConditionMet()
        {
            return !GameState.Instance.PlayerUnits.Any();
        }
    }
}
