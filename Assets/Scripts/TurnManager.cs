namespace DLS.LD39
{
    using DLS.LD39.Units;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TurnManager
    {
        private List<Initiative> _units = new List<Initiative>();

        private Queue<Initiative> _unitOrder = new Queue<Initiative>();
        private Initiative _activeUnit;

        public void RegisterUnit(Initiative unit)
        {
            _units.Add(unit);
            UpdateTurnOrder();
        }

        public void UpdateTurnOrder()
        {
            _unitOrder.Clear();
            var ordered = _units.OrderByDescending(i => i.InitiativeValue);

            if (_activeUnit == null)
            {
                foreach (var i in ordered)
                {
                    _unitOrder.Enqueue(i);
                }
                return;
            }

            var completedUnits = new Queue<Initiative>();
            var inBackQueue = true;
            foreach (var i in ordered)
            {
                if (i == _activeUnit)
                {
                    inBackQueue = false;
                    continue;
                }

                if (inBackQueue)
                {
                    completedUnits.Enqueue(i);
                }
                else
                {
                    _unitOrder.Enqueue(i);
                }
            }
            
            
            while (completedUnits.Any())
            {
                _unitOrder.Enqueue(completedUnits.Dequeue());
            }
        }
    }
}
