namespace DLS.LD39.Equipment
{
    using System;

    public class EquipmentSlot
    {
        private Loot _storedItem;

        public EquipmentSlot(LootType type)
        {
            ValidLoot = type;
        }

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

        public LootType ValidLoot
        {
            get; private set;
        }

        public Loot SlotItem
        {
            get
            {
                return _storedItem;
            }
        }

        /// <summary>
        /// Sets the current item in the slot. Returns true if the item was added
        /// successfully.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>True if the item was added to the slot.</returns>
        public bool SetItem(Loot item)
        {
            if (item == _storedItem)
            {
                return true;
            }

            if (item == null || item.Type == ValidLoot)
            {
                SetNewItem(item);
                return true;
            }

            return false;
        }

        private void SetNewItem(Loot item)
        {
            var prevItem = _storedItem;
            _storedItem = item;
            if (prevItem == null)
            {
                SlotFilled?.Invoke(this, EventArgs.Empty);
                return;
            }
            else if (item == null)
            {
                SlotEmptied?.Invoke(this, EventArgs.Empty);
                return;
            }

            EquipmentChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
