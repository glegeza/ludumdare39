namespace DLS.LD39.Units
{
    using System;
    using UnityEngine;

    public class Combat : MonoBehaviour, IDestructible, IAttackable
    {
        private GameUnit _unit;

        public int Evasion
        {
            get
            {
                return _unit.Stats.Evasion;
            }
        }

        public int HitPoints
        {
            get; private set;
        }

        public int MaxHitPoints
        {
            get
            {
                return _unit.Stats.MaxHP;
            }
        }

        public int Armor
        {
            get
            {
                return _unit.Stats.Armor;
            }
        }

        public event EventHandler<EventArgs> Destroyed;

        public void Initialize(GameUnit unit)
        {
            _unit = unit;
        }

        public int Damage(int amt)
        {
            var dmg = Mathf.Max(amt - Armor, 0);

            HitPoints -= dmg;
            if (HitPoints < 0)
            {
                OnDestroyed();
            }
            return HitPoints;
        }

        public int Heal(int amt)
        {
            HitPoints += amt;
            HitPoints = Mathf.Clamp(HitPoints, 0, MaxHitPoints);
            return HitPoints;
        }

        private void OnDestroyed()
        {
            Destroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
