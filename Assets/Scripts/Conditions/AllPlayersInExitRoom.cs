namespace DLS.LD39.Conditions
{
    using System.Linq;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Conditions/All Player Units In Exit Room")]
    public class AllPlayersInExitRoom : GameCondition
    {
        public override ConditionCheckInterval Interval
        {
            get
            {
                return ConditionCheckInterval.TurnEnd;
            }
        }

        public override bool IsConditionMet()
        {
            return GameState.Instance.PlayerUnits.Any() &&
                   GameState.Instance.CurrentMap != null &&
                   GameState.Instance.PlayerUnits.All(u => GameState.Instance.CurrentMap.ExitRoom.UnitInRoom(u));
        }
    }
}
