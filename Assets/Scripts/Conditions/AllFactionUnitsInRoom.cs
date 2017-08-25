namespace DLS.LD39.Conditions
{
    using System.Linq;
    using Units;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Conditions/All Units In Room")]
    public class AllFactionUnitsInRoom : GameCondition
    {
        public string RoomID;
        public Faction Faction;

        public override ConditionCheckInterval Interval
        {
            get { return ConditionCheckInterval.TurnEnd; }
        }

        public override bool IsConditionMet()
        {
            var room = GameState.Instance.CurrentMap.GetRoom(RoomID);
            var totalUnits = 0;
            foreach (var unit in ActiveUnits.Instance.Units.Where(u => u.Faction == Faction))
            {
                totalUnits += 1;
                if (!room.UnitInRoom(unit))
                {
                    return false;
                }
            }

            return totalUnits > 0;
        }
    }
}
