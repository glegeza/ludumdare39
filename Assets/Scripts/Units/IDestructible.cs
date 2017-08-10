namespace DLS.LD39.Units
{
    using System;

    interface IDestructible
    {
        event EventHandler<EventArgs> Destroyed;

        int HitPoints { get; }

        int Armor { get; }

        int ApplyDamage(int amt);

        int ApplyHeal(int amt);
    }
}
