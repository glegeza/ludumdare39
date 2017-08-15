namespace DLS.LD39.Units
{
    using System;
    using UnityEngine;
    using Combat;

    public class CombatController : GameUnitComponent, ITargetable
    {
        private bool _dead = false;

        public event EventHandler<EventArgs> Destroyed;

        public int Evasion
        {
            get
            {
                return AttachedUnit.Stats.Evasion;
            }
        }

        public int HitPoints
        {
            get; private set;
        }

        public int Armor
        {
            get
            {
                return AttachedUnit.Stats.Armor;
            }
        }

        public int ApplyDamage(int amt)
        {
            var dmg = Mathf.Max(amt - Armor, 0);

            HitPoints -= dmg;
            if (HitPoints <= 0)
            {
                OnDestroyed();
            }
            return HitPoints;
        }

        public int ApplyHeal(int amt)
        {
            HitPoints += amt;
            HitPoints = Mathf.Clamp(HitPoints, 0, AttachedUnit.Stats.MaxHP);
            return HitPoints;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            HitPoints = unit.Stats.MaxHP;
        }

        private void OnDestroyed()
        {
            if (_dead)
            {
                return;
            }
            _dead = true;
            Destroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
