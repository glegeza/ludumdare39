namespace DLS.LD39
{
    using Units;
    using System;

    public class ActiveUnitsChangedEventArgs : EventArgs
    {
        public ActiveUnitsChangedEventArgs(GameUnit unit)
        {
            Unit = unit;
        }

        public GameUnit Unit
        {
            get; private set;
        }
    }
}
