namespace DLS.LD39.Conditions
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ConditionChecker<T> : SingletonComponent<T> where T : ConditionChecker<T>
    {
        public List<GameCondition> DefaultConditions;

        private readonly Dictionary<ConditionCheckInterval, List<GameCondition>> _conditions = 
            new Dictionary<ConditionCheckInterval, List<GameCondition>>();

        public void AddCondition(GameCondition condition)
        {
            if (!_conditions.ContainsKey(condition.Interval))
            {
                _conditions.Add(condition.Interval, new List<GameCondition>());
            }
            _conditions[condition.Interval].Add(condition);
        }

        /// <summary>
        /// Clears all conditions except for default conditions.
        /// </summary>
        public void ClearConditions()
        {
            _conditions.Clear();
            AddDefaultConditions();
        }

        protected override void Awake()
        {
            base.Awake();

            TurnOrderTracker.Instance.TurnAdvanced += OnTurnAdvanced;
            TurnOrderTracker.Instance.UnitEndedTurn += OnTurnEnded;
            TurnOrderTracker.Instance.RoundCompleted += OnRoundCompleted;
            ActiveUnits.Instance.UnitDestroyed += OnUnitDestroyed;

            AddDefaultConditions();
        }

        protected void CheckConditions(ConditionCheckInterval interval)
        {
            if (!_conditions.ContainsKey(interval))
            {
                return;
            }

            var metConditions = 
                _conditions[interval].Where(condition => condition.IsConditionMet()).ToList();

            if (metConditions.Any())
            {
                OnAnyConditionsMet(metConditions);
            }
        }

        protected abstract void OnAnyConditionsMet(IEnumerable<GameCondition> metConditions);

        private void AddDefaultConditions()
        {
            foreach (var defaultCondition in DefaultConditions)
            {
                AddCondition(defaultCondition);
            }
        }

        private void OnUnitDestroyed(object sender, ActiveUnitsChangedEventArgs e)
        {
            CheckConditions(ConditionCheckInterval.OnUnitDestroyed);
        }

        private void OnRoundCompleted(object sender, System.EventArgs e)
        {
            CheckConditions(ConditionCheckInterval.PerRound);
        }

        private void OnTurnEnded(object sender, System.EventArgs e)
        {
            CheckConditions(ConditionCheckInterval.TurnEnd);
        }

        private void OnTurnAdvanced(object sender, System.EventArgs e)
        {
            CheckConditions(ConditionCheckInterval.TurnStart);
        }
    }
}
