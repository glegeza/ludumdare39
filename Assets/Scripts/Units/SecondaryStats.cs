namespace DLS.LD39.Units
{
    using System;

    public class SecondaryStats
    {
        private PrimaryStats _stats;

        public SecondaryStats(PrimaryStats stats)
        {
            if (stats == null)
            {
                throw new ArgumentNullException("stats");
            }
            _stats = stats;
        }

        public float Initiative
        {
            get
            {
                return _stats.Speed;
            }
        }

        public int ActionPointRegen
        {
            get
            {
                return _stats.Speed * 3;
            }
        }

        public int ActionPointCap
        {
            get
            {
                return _stats.Speed * 4;
            }
        }
    }
}
