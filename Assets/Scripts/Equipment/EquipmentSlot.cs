﻿namespace DLS.LD39.Equipment
{
    using System;
    using Utility;

    public class EquipmentSlot<T> where T : Loot
    {
        private T _storedItem;

        /// <summary>
        /// Raised when one piece of equipment is replaced by another.
        /// </summary>
        public event EventHandler<EventArgs> EquipmentChanged;

        /// <summary>
        /// Raised if a filled slot has its equipment removed and not replaced.
        /// </summary>
        public event EventHandler<EventArgs> SlotEmptied;

        /// <summary>
        /// Raised if an empty slot has equipment added to it.
        /// </summary>
        public event EventHandler<EventArgs> SlotFilled;

        public T SlotItem
        {
            get
            {
                return _storedItem;
            }
        }
        
        public void SetItem(T item)
        {
            if (item == _storedItem)
            {
                return;
            }

            var prevItem = _storedItem;
            _storedItem = item;
            if (prevItem == null)
            {
                SlotFilled.SafeRaiseEvent(this);
            }
            else if (item == null)
            {
                SlotEmptied.SafeRaiseEvent(this);
            }

            EquipmentChanged.SafeRaiseEvent(this);
        }
    }
}
