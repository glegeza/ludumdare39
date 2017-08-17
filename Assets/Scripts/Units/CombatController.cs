namespace DLS.LD39.Units
{
    using System;
    using UnityEngine;
    using Combat;
    using Utility;

    public class CombatController : GameUnitComponent, ITargetable
    {
        private bool _dead = false;

        public event EventHandler<EventArgs> Destroyed;

        public int Evasion
        {
            get
            {
                return AttachedUnit.PrimaryStats.Evasion;
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
                return AttachedUnit.PrimaryStats.Armor;
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
            HitPoints = Mathf.Clamp(HitPoints, 0, AttachedUnit.PrimaryStats.MaxHP);
            return HitPoints;
        }

        protected override void OnInitialized(GameUnit unit)
        {
            HitPoints = unit.PrimaryStats.MaxHP;
        }

        private void OnDestroyed()
        {
            if (_dead)
            {
                return;
            }
            _dead = true;
            Destroyed.SafeRaiseEvent(this);
        }
    }
}
