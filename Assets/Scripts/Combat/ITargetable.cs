namespace DLS.LD39.Combat
{
    using System;

    interface ITargetable
    {
        event EventHandler<EventArgs> Destroyed;

        int HitPoints { get; }

        int Armor { get; }

        int Evasion { get; }

        int ApplyDamage(int amt);

        int ApplyHeal(int amt);
    }
}
