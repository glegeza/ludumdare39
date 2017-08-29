namespace DLS.LD39.Generation
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using JetBrains.Annotations;
    using UnityEngine;

    public class RoomRollTable
    {
        private readonly Dictionary<float, RoomType> _roomTable = new Dictionary<float, RoomType>();

        public RoomRollTable([NotNull] IEnumerable<RoomProbability> probabilityList)
        {
            var roomProbabilities = probabilityList as IList<RoomProbability> ?? probabilityList.ToList();

            var total = roomProbabilities.Sum(p => p.Probability);
            var curProb = 0.0f;
            foreach (var prob in roomProbabilities)
            {
                curProb += prob.Probability / total;
                _roomTable.Add(curProb, prob.RoomType);
            }
        }

        public RoomType GetRandomRoomType()
        {
            var roll = Random.Range(0.0f, 1.0f);
            var chances = _roomTable.Keys.ToList();
            foreach (var chance in chances.OrderBy(c => c))
            {
                if (roll <= chance)
                {
                    return _roomTable[chance];
                }
            }
            return _roomTable[chances.Last()];
        }
    }
}
