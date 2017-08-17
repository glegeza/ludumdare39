namespace DLS.Utility
{
    using System;

    static public class EventExtensions
    {
        public static void SafeRaiseEvent(this EventHandler<EventArgs> @event, object sender)
        {
            var handler = @event;
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        public static void SafeRaiseEvent(this EventHandler @event, object sender, EventArgs e)
        {
            var handler = @event;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
        public static void SafeRaiseEvent<T>(this EventHandler<T> @event, object sender, T e)
            where T : EventArgs
        {
            var handler = @event;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
