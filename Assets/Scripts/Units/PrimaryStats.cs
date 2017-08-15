namespace DLS.LD39.Units
{
    using Data;

    public class PrimaryStats
    {
        private int _baseAim;
        private int _baseEvasion;
        private int _baseArmor;
        private int _baseSpeed;
        private int _baseMaxHP;

        public PrimaryStats(StatGenerator data)
        {
            _baseAim = data.GetAim();
            _baseEvasion = data.GetEvasion();
            _baseArmor = data.GetArmor();
            _baseSpeed = data.GetSpeed();
            _baseMaxHP = data.GetMaxHP();
        }

        public int Aim
        {
            get
            {
                return _baseAim;
            }
        }

        public int Evasion
        {
            get
            {
                return _baseEvasion;
            }
        }

        public int Armor
        {
            get
            {
                return _baseArmor;
            }
        }

        public int Speed
        {
            get
            {
                return _baseSpeed;
            }
        }

        public int MaxHP
        {
            get
            {
                return _baseMaxHP;
            }
        }
    }
}
