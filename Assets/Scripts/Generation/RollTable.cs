namespace DLS.LD39.Generation
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using JetBrains.Annotations;
    using UnityEngine;

    public abstract class RollTable<T>
    {
        private readonly Dictionary<float, T> _rollTable = new Dictionary<float, T>();

        protected RollTable([NotNull] IEnumerable<RollTableEntry<T>> tableEntries)
        {
            var entryList = GetEntryList(tableEntries);

            var total = entryList.Sum(p => p.Probability);
            var curProb = 0.0f;
            foreach (var prob in entryList)
            {
                curProb += prob.Probability / total;
                _rollTable.Add(curProb, prob.Item);
            }
        }

        public T GetRandomEntry()
        {
            var roll = Random.Range(0.0f, 1.0f);
            var chances = _rollTable.Keys.ToList();
            foreach (var chance in chances.OrderBy(c => c))
            {
                if (roll <= chance)
                {
                    return _rollTable[chance];
                }
            }
            return _rollTable[chances.Last()];
        }

        private static IList<RollTableEntry<T>> GetEntryList(IEnumerable<RollTableEntry<T>> tableEntries)
        {
            return tableEntries as IList<RollTableEntry<T>> ?? tableEntries.ToList();
        }
    }
}
