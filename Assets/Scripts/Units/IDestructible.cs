namespace DLS.LD39.Units
{
    using System;

    interface IDestructible
    {
        event EventHandler<EventArgs> Destroyed;

        int HitPoints { get; }

        int MaxHitPoints { get; }

        int Armor { get; }

        int Damage(int amt);

        int Heal(int amt);
    }
}
